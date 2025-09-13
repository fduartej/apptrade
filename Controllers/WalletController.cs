using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using apptrade.Models;
using apptrade.Data;

namespace apptrade.Controllers
{

    public class WalletController : Controller
    {
        private readonly ILogger<WalletController> _logger;
        private readonly ApplicationDbContext _context;

        public WalletController(ILogger<WalletController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accessing Wallet");
            if(User?.Identity?.IsAuthenticated != true)
            {
                _logger.LogInformation("User is not authenticated. Redirecting to login.");
                return Challenge();
            }

            var customer = await _context.DbCustomer.FirstOrDefaultAsync(c => c.UserName == User.Identity.Name);
            if (customer == null)
            {
                _logger.LogInformation("Customer not found.");
                return NotFound();
            }
            var userWallet =  await _context.DbWallet
                .Include(w => w.customer)
                .FirstOrDefaultAsync(w => w.customer== customer);
            if (userWallet == null)
            {
                _logger.LogInformation("User wallet not found.");
                userWallet = new Wallet
                {
                    customer = customer,
                    Balance = 0
                };
                _context.DbWallet.Add(userWallet);
                await _context.SaveChangesAsync();
            }

            _logger.LogInformation($"User wallet balance: {userWallet.Balance}");

            var transactions = await _context.DbWalletTransaction
                .Where(t => t.Wallet == userWallet)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            ViewBag.Transactions = transactions;
            return View(userWallet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFunds([FromForm] decimal amount, [FromForm] string transactionType)
        {
            _logger.LogInformation("Processing wallet transaction");
            if (string.IsNullOrEmpty(transactionType))
            {
                ModelState.AddModelError("transactionType", "Invalid transaction type.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", await _context.DbWallet.Include(w => w.customer).FirstOrDefaultAsync(w => w.customer.UserName == User.Identity.Name));
            }

            var customer = await _context.DbCustomer.FirstOrDefaultAsync(c => c.UserName == User.Identity.Name);
            if (customer == null)
            {
                _logger.LogInformation("Customer not found.");
                return NotFound();
            }

            var userWallet = await _context.DbWallet
                .Include(w => w.customer)
                .FirstOrDefaultAsync(w => w.customer == customer);
            if (userWallet == null)
            {
                _logger.LogInformation("User wallet not found.");
                return NotFound();
            }

            var transaction = new WalletTransaction
            {
                Wallet = userWallet,
                Amount = amount,
                TransactionType = transactionType,
                TransactionDate = DateTime.UtcNow
            };

            userWallet.Balance += amount;

            _context.Add(transaction);
            _context.Update(userWallet);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Transaction successful: {transactionType} of {amount} for user {User.Identity.Name}");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}

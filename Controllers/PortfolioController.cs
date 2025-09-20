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
   
    public class PortfolioController : Controller
    {
        private readonly ILogger<PortfolioController> _logger;
        private readonly ApplicationDbContext _context;

        public PortfolioController(ILogger<PortfolioController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accessing Portfolio");
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

            var portfolios = await _context.DbPortfolio
                .Where(p => p.customer == customer)
                .ToListAsync();

            _logger.LogInformation($"Found {portfolios.Count} portfolios for user {customer.UserName}");

            return View(portfolios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPortfolio([FromForm] string Name, [FromForm] string Risk, [FromForm] string Owner)
        {
            _logger.LogInformation("Processing add portfolio request");

            var customer = await _context.DbCustomer.FirstOrDefaultAsync(c => c.UserName == User.Identity.Name);
            if (customer == null)
            {
                _logger.LogInformation("Customer not found.");
                return NotFound();
            }

            // Create a new portfolio item
            var portfolioItem = new Portfolio
            {
                Name = Name,
                Risk = Risk,
                Owner = Owner,
                customer = customer
            };

            // Save the portfolio item to the database
            _context.DbPortfolio.Add(portfolioItem);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Portfolio item added successfully");

            // Redirect to the Index action
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
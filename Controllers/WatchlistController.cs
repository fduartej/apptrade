using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using apptrade.Models;
using apptrade.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace apptrade.Controllers
{

    public class WatchlistController : Controller
    {
        private readonly ILogger<WatchlistController> _logger;
        private readonly ApplicationDbContext _context;

        public WatchlistController(ILogger<WatchlistController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()

        {   _logger.LogInformation("Accessing Watchlist Index");
            if(User?.Identity?.IsAuthenticated != true)
            {
                _logger.LogInformation("User is not authenticated. Redirecting to login.");
                return Challenge();
            }
            var userWatchlist = _context.DbWatchlist
                .Include(w => w.Assest)
                .Where(w => w.UserName == User.Identity.Name)
                .ToList();
            _logger.LogInformation($"Found {userWatchlist.Count} items in watchlist for user {User.Identity.Name}");
            return View(userWatchlist);
        }

        public IActionResult Follow(string id)
        {

             _logger.LogInformation($"Fetching details for instrument: {id}");
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            if(User?.Identity?.IsAuthenticated != true)
            {
                _logger.LogInformation("User is not authenticated. Redirecting to login.");
                return Challenge();
            }
            // Aquí puedes implementar la lógica para seguir un instrumento
            var asset = _context.DbAssest.FirstOrDefault(a => a.Ticker == id);
            if (asset == null)
            {
                _logger.LogInformation("Asset not found.");
                return NotFound();
            }

            var existingItem = _context.DbWatchlist.
                    Include(w => w.Assest)
                    .FirstOrDefault(w => w.UserName == User.Identity.Name
                        && w.Assest != null && w.Assest.Id == asset.Id);
            if (existingItem == null)
            {
                var watchlistItem = new Watchlist
                {
                    UserName = User.Identity.Name,
                    Assest = asset,
                    CreatedAt = DateTime.Now
                };
                _context.DbWatchlist.Add(watchlistItem);
                _context.SaveChanges();
            }else
            {
                _logger.LogInformation("Item already in watchlist.");
            }
       
            return RedirectToAction("Index", "Market");
        }

        public IActionResult Unfollow(string id)
        {
            _logger.LogInformation($"Removing instrument from watchlist: {id}");
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            if(User?.Identity?.IsAuthenticated != true)
            {
                _logger.LogInformation("User is not authenticated. Redirecting to login.");
                return Challenge();
            }

            var asset = _context.DbAssest.FirstOrDefault(a => a.Ticker == id);
            if (asset == null)
            {
                _logger.LogInformation("Asset not found.");
                return NotFound();
            }

            var existingItem = _context.DbWatchlist
                .Include(w => w.Assest)
                .FirstOrDefault(w => w.UserName == User.Identity.Name
                    && w.Assest != null && w.Assest.Id == asset.Id);

            if (existingItem != null)
            {
                _context.DbWatchlist.Remove(existingItem);
                _context.SaveChanges();
                _logger.LogInformation("Item removed from watchlist.");
            }
            else
            {
                _logger.LogInformation("Item not found in watchlist.");
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
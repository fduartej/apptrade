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
using apptrade.Services;

namespace apptrade.Controllers
{

    public class WatchlistController : Controller
    {
        private readonly ILogger<WatchlistController> _logger;
        private readonly IWatchlistService _watchlistService;

        public WatchlistController(ILogger<WatchlistController> logger, IWatchlistService watchlistService)
        {
            _logger = logger;
            _watchlistService = watchlistService;
        }

        public async Task<IActionResult> Index()
        {   
            _logger.LogInformation("Accessing Watchlist Index");
            if(User?.Identity?.IsAuthenticated != true)
            {
                _logger.LogInformation("User is not authenticated. Redirecting to login.");
                return Challenge();
            }
            
            var userWatchlist = await _watchlistService.GetUserWatchlistAsync(User.Identity.Name!);
            _logger.LogInformation($"Found {userWatchlist.Count} items in watchlist for user {User.Identity.Name}");
            return View(userWatchlist);
        }

        public async Task<IActionResult> Follow(string id)
        {
            _logger.LogInformation($"Following instrument: {id}");
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            if(User?.Identity?.IsAuthenticated != true)
            {
                _logger.LogInformation("User is not authenticated. Redirecting to login.");
                return Challenge();
            }

            var success = await _watchlistService.AddToWatchlistAsync(User.Identity.Name!, id);
            if (!success)
            {
                _logger.LogWarning("Failed to add {Ticker} to watchlist for user {UserName}", id, User.Identity.Name);
            }
       
            return RedirectToAction("Index", "Market");
        }

        public async Task<IActionResult> Unfollow(string id)
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

            var success = await _watchlistService.RemoveFromWatchlistAsync(User.Identity.Name!, id);
            if (!success)
            {
                _logger.LogWarning("Failed to remove {Ticker} from watchlist for user {UserName}", id, User.Identity.Name);
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
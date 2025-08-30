using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using apptrade.Data;
using apptrade.Models;

namespace apptrade.Controllers
{

    public class MarketController : Controller
    {
        private readonly ILogger<MarketController> _logger;
        private readonly ApplicationDbContext _context;

        public MarketController(ILogger<MarketController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Fetching market data...");
            var instruments = _context.DbAssest.ToList();
            return View(instruments);
        }

        public IActionResult Details(string id)
        {
            _logger.LogInformation($"Fetching details for instrument: {id}");
            
            var instrument = new Assest
            {
                Ticker = id,
                Nombre = "Instrument Name",
                Sector = "Sector",
                Moneda = "USD",
                PrecioActual = 100.00m,
                CapitalizacionUSD = 1000000000m
            };
            return View(instrument);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
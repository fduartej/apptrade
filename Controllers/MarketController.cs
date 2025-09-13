using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using apptrade.Data;
using apptrade.Models;
using apptrade.Services;

namespace apptrade.Controllers
{

    public class MarketController : Controller
    {
        private readonly ILogger<MarketController> _logger;
        private readonly IMarketService _marketService;

        public MarketController(ILogger<MarketController> logger, IMarketService marketService)
        {
            _logger = logger;
            _marketService = marketService;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching market data from Redis cache or database...");
            var instruments = await _marketService.GetAllInstrumentsAsync();
            return View(instruments);
        }

        public async Task<IActionResult> Details(string id)
        {
            _logger.LogInformation("Fetching details for instrument: {Ticker}", id);
            
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var instrument = await _marketService.GetInstrumentByTickerAsync(id);
            
            if (instrument == null)
            {
                // Si no se encuentra en la base de datos, crear uno ficticio para demostración
                instrument = new Assest
                {
                    Ticker = id,
                    Nombre = "Instrument Name",
                    Sector = "Sector",
                    Moneda = "USD",
                    PrecioActual = 100.00m,
                    CapitalizacionUSD = 1000000000m
                };
            }
            
            return View(instrument);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
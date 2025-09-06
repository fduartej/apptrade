using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using apptrade.Models;
using apptrade.Data;

namespace apptrade.Controllers
{

    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly ApplicationDbContext _context;

        public ContactController(ILogger<ContactController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OnPostRegistrar(Contact obj)
        {
            _logger.LogInformation("Contacto recibido: {@Contact}", obj);
            if (ModelState.IsValid)
            {
                _context.DbContact.Add(obj);
                _context.SaveChanges();
                ViewData["Message"] = "Contacto registrado con éxito.";
            }
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
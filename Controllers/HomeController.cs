using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using apptrade.Models;

namespace apptrade.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [TempData]
    public string Message { get; set; }


    public IActionResult Index()
    {

        var cookieOptions = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true, // Para evitar acceso desde JavaScript
            Secure = true,   // Solo se envía por HTTPS
            SameSite = SameSiteMode.Strict
        };

        Message = $"Customer 1000 added";

        Response.Cookies.Append("MiCookie", "ValorDeLaCookie", cookieOptions);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

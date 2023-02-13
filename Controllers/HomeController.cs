using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using oktaMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace oktaMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult Profile()
    {
        return View(HttpContext.User.Claims);
    }

    [Authorize(Roles = "GroupOne")]
    public IActionResult Profile1()
    {
        return View(HttpContext.User.Claims);
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

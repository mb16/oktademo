using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using oktaMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Okta.AspNetCore;



namespace oktaMVC.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public AccountController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult LogIn()
    {
        if (HttpContext?.User?.Identity?.IsAuthenticated == false)
        {
            return Challenge(OktaDefaults.MvcAuthenticationScheme);
        }

        return RedirectToAction("Index", "Home");
    }

    //[HttpPost]
    public SignOutResult LogOut()
    {
        // need fedrated logout,  from IdP too.

        return new SignOutResult(
            new[]
            {
                OktaDefaults.MvcAuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme,
            },
            new AuthenticationProperties { RedirectUri = "/Home/" });
    }
}

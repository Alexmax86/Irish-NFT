using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NFTmvc.Models;
using NFTmvc.Areas.Identity.Data;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using NFTmvc.Data;


namespace NFTmvc.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly NFTmvcIdentityDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(NFTmvcIdentityDbContext context, IHttpClientFactory clientFactory, ILogger<HomeController> logger)
    {
        _context = context;
        _clientFactory = clientFactory;
         _logger = logger;
    }
    

  
    

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public ActionResult Loginverification()
    {
        return Content("Logged in");
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

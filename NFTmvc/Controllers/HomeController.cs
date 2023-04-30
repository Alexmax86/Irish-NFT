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
    

  
    public async Task<IActionResult> Index()
    {
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> NFTProducts()
    {
        HttpClient client = _clientFactory.CreateClient(name: "ProductsApi");
        HttpRequestMessage request = new(method: HttpMethod.Get, requestUri:
        "/api/product");
        HttpResponseMessage response = await client.SendAsync(request);
        IEnumerable<Product>? model = await 
        response.Content.ReadFromJsonAsync<IEnumerable<Product>>();

        //Console.log return value of API
        foreach (var item in model)
        {
            var json = JsonConvert.SerializeObject(item, Formatting.Indented);
            Console.WriteLine(json);
        }
        return View(model);
    }



    public async Task<IActionResult> Details(int id)
    {
        HttpClient client = _clientFactory.CreateClient(name: "ProductsApi");
        string requestUri = $"/api/Product/{id}";
        HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: requestUri);
        HttpResponseMessage response = await client.SendAsync(request);
        Product requestedProduct = await response.Content.ReadFromJsonAsync<Product>();

        //Logs returned object        
        string jsonString = System.Text.Json.JsonSerializer.Serialize(requestedProduct);
        Console.WriteLine(jsonString);
        return View(requestedProduct);
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

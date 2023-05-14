using Microsoft.AspNetCore.Mvc;
using NFTmvc.Models;
using NFTmvc.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using NFTmvc.Data;
using Microsoft.AspNetCore.Identity;


namespace NFTmvc.Controllers;

public class ProductsController : Controller
{
    private readonly ApiHelper _apiHelper;
    private readonly IHttpClientFactory _clientFactory;
    private readonly NFTmvcIdentityDbContext _context;
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    //Injects userManager and httpContextAccessor to retrieve User id from current session
    //Injects API helper to manage API calls, _context for access to DB
    public ProductsController(NFTmvcIdentityDbContext context, IHttpClientFactory clientFactory, ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _clientFactory = clientFactory;
         _logger = logger;
        _apiHelper = new ApiHelper(clientFactory);
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    //Returns view with all the products
    public async Task<IActionResult> Products()
    {
        IEnumerable<Product> products = await _apiHelper.GetAllProductsAsync();
        if (products == null)
        {
            return NotFound();
        }
        return View(products);
    }
    //Returns view with details of a product
    public async Task<IActionResult> Details(int id)
    {
        Product product = await _apiHelper.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    //To buy a product, the product is marked as sold on the Products API table, and an order is created in the Orders API table
    [Authorize]
    public async Task<IActionResult> Buy(int id)
    {
        string? userId = _userManager.GetUserId(_httpContextAccessor.HttpContext!.User);
        bool deletion = await _apiHelper.MarkProductAsSoldAsync(id);
        bool orderCreation = await _apiHelper.CreateOrderAsync(id, userId!);
        Console.WriteLine(orderCreation);
        Console.WriteLine("Userid = " + userId);
        if (deletion == false)
        {
            return NotFound();
        }
        return View();
    }      
}
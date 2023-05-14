using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NFTmvc.Models;
using Microsoft.AspNetCore.Authorization;
using NFTmvc.Data;



namespace NFTmvc.Controllers;

public class OrdersController : Controller
{    
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApiHelper _apiHelper;

    //Injects userManager and httpContextAccessor to retrieve User id from current session
    //Injects API helper to manage API calls
    public OrdersController(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _apiHelper = new ApiHelper(clientFactory);
    }
    
    
    [Authorize]
    public async Task<IActionResult> Index()
    {
        // retrieves the ID of the current user
        string? userId = _userManager.GetUserId(_httpContextAccessor.HttpContext!.User);
        
        // fetches all orders from a user
        List<Order> requestedOrders = await _apiHelper.GetOrdersByUserId(userId!);

        // Extracts Product IDs from the user's orders
        List<int> productIds = new List<int>();        
        foreach (Order thisOrder  in requestedOrders)
        {
            productIds.Add(thisOrder.ProductId);
        }
        if (productIds.Count == 0)
        {
            return View(productIds);
        }
        //Calls order APIs with batch of IDs of products to retrieve
        List<Product> boughtProducts = await  _apiHelper.GetProductsByIdsAsync(productIds);

        //Joins orders and product IDs into anonymous object
        var ordersWithProducts = requestedOrders.Select(order =>
        {
            var matchingProducts = boughtProducts.Where(p => p.Id == order.ProductId);
            return new
            {
                OrderId = order.Id,
                OrderDate = order.DateOrdered,
                ProductName = matchingProducts.FirstOrDefault()?.Name ?? "Unknown",
                ImgLink = matchingProducts.FirstOrDefault()?.ImgLink ?? "Unknown",
                // add other fields as needed
            };
        }).ToList();
        
        if (requestedOrders == null)
        {
            return NotFound();
        }

        //passes the object to the view
        return View(ordersWithProducts);
    }     
}
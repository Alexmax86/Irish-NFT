using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NFTmvc.Models;
using NFTmvc.Areas.Identity.Data;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using NFTmvc.Data;
using NFTmvc.Data;


namespace NFTmvc.Controllers;

public class OrdersController : Controller
{
    // inject the UserManager and HttpContext classes in your controller or service
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApiHelper _apiHelper;

    public OrdersController(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _apiHelper = new ApiHelper(clientFactory);
    }
    
    
    [Authorize]
    public async Task<IActionResult> Index()
    {
        // retrieve the ID of the current user
        string userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
        
        List<Order> requestedOrders = await _apiHelper.GetOrdersByUserId(userId);
        List<int> productIds = new List<int>();
        
        foreach (Order thisOrder  in requestedOrders)
        {
            productIds.Add(thisOrder.ProductId);
        }

        List<Product> boughtProducts = await  _apiHelper.GetProductsByIdsAsync(productIds);

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

        //return Content(System.Text.Json.JsonSerializer.Serialize(ordersWithProducts) );
        return View(ordersWithProducts);
    } 

    public async Task<IActionResult> test ()
    {
        List<int> numbers = new List<int>();
        numbers.Add(132);
        numbers.Add(133);

        List<Product> values = await  _apiHelper.GetProductsByIdsAsync(numbers);
        

        return Content(System.Text.Json.JsonSerializer.Serialize(values) );
    }
}
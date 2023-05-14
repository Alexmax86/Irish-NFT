using NFTmvc.Models;
using Newtonsoft.Json;
using System.Text;


//APIHELPER class to manage in a centralized way API calls

namespace NFTmvc.Data;

public class ApiHelper
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly HttpClient _productsHttpClient;
    private readonly HttpClient _ordersHttpClient;    

    //Injects client factories for the 2 API dependencies
    public ApiHelper(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _productsHttpClient = _clientFactory.CreateClient(name: "ProductsApi");
        _ordersHttpClient = _clientFactory.CreateClient(name: "OrdersApi");        
    }

    private HttpRequestMessage CreateHttpRequestMessage(HttpMethod httpMethod, string requestUri)
    {
        return new HttpRequestMessage(httpMethod, requestUri);
    }

    // Gets single product from Product API
    public async Task<Product> GetProductAsync(int id)
    {
        string requestUri = $"/api/Product/{id}";
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, requestUri);
        HttpResponseMessage response = await _productsHttpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            Product? requestedProduct = await response.Content.ReadFromJsonAsync<Product>();
            return requestedProduct!;
        }
        return null!;
    }

    //Gets all products from the API
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        string requestUri = $"/api/Product";
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, requestUri);
        HttpResponseMessage response = await _productsHttpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            IEnumerable<Product>? products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
            return products!.Where(p => p.Sold == false); 
        }
        return null!;
    }

    //Creates new order in Orders Api
    public async Task<bool> CreateOrderAsync(int productId, string orderedBy)
    {
        string requestUri = $"/api/Orders";
        Order order = new Order()
        {            
            ProductId = productId,
            OrderedBy = orderedBy,
            DateOrdered = DateTime.UtcNow
        };
        string orderJson = JsonConvert.SerializeObject(order);
        HttpContent httpContent = new StringContent(orderJson, Encoding.UTF8, "application/json");
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Post, requestUri);
        request.Content = httpContent;
        HttpResponseMessage response = await _ordersHttpClient.SendAsync(request);
        Console.WriteLine(response);
        return response.IsSuccessStatusCode;
    }

    //Retrieves all orders from the Orders API filtering by User ID    
    public async Task<List<Order>> GetOrdersByUserId(string userId)
    {
        
        string requestUri = $"api/orders/getOrdersByUser/{userId}";
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, requestUri);
        HttpResponseMessage response = await _ordersHttpClient.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            List<Order>? requestedOrders = await response.Content.ReadFromJsonAsync<List<Order>>();
            return requestedOrders!;
        }
        return null!;

    }

    //Marks a product as `Sold` in the products API
    public async Task<bool> MarkProductAsSoldAsync(int id)
    {
    string requestUri = $"/api/Product/{id}";
    Product product = await GetProductAsync(id);
    if (product != null)
    {
        product.Sold = true; // sets the Sold property to true
        string productJson = JsonConvert.SerializeObject(product);
        HttpContent httpContent = new StringContent(productJson, Encoding.UTF8, "application/json");
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Put, requestUri);
        request.Content = httpContent;
        HttpResponseMessage response = await _productsHttpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    return false;
    }

    //GET request to the Products API. The GET requests passes a batch of IDs. The API is programmed to respond with all the products
    //with matching IDs
    public async Task<List<Product>> GetProductsByIdsAsync(List<int> ids)
    {  
        string queryString = $"?id={string.Join("&id=", ids)}";         
        string requestUri = $"/api/getProductsByIds{queryString}";
        HttpResponseMessage response = await _productsHttpClient.GetAsync(requestUri);

        
        if (response.IsSuccessStatusCode)
        {
            
            List<Product>? products = await response.Content.ReadFromJsonAsync<List<Product>>();
            return products!;
        }

        Console.WriteLine(response.StatusCode);
        Console.WriteLine(response.ReasonPhrase);
        throw new HttpRequestException($"Failed to get products by IDs. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
    }
}
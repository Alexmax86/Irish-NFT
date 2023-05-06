using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NFTmvc.Models;
using NFTmvc.Areas.Identity.Data;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace NFTmvc.Data;

public class ApiHelper
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly HttpClient _productsHttpClient;
    private readonly HttpClient _ordersHttpClient;    

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

    public async Task<Product> GetProductAsync(int id)
    {
        string requestUri = $"/api/Product/{id}";
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, requestUri);
        HttpResponseMessage response = await _productsHttpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            Product requestedProduct = await response.Content.ReadFromJsonAsync<Product>();
            return requestedProduct;
        }
        return null;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        string requestUri = $"/api/Product";
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, requestUri);
        HttpResponseMessage response = await _productsHttpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            IEnumerable<Product> products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
            return products.Where(p => p.Sold == false); 
        }
        return null;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        string requestUri = $"/api/Product/{id}";
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Delete, requestUri);
        HttpResponseMessage response = await _productsHttpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

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

    public async Task<List<Order>> GetOrdersByUserId(string userId)
    {
        
        string requestUri = $"api/orders/getOrdersByUser/{userId}";
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, requestUri);
        HttpResponseMessage response = await _ordersHttpClient.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            List<Order> requestedOrders = await response.Content.ReadFromJsonAsync<List<Order>>();
            return requestedOrders;
        }
        return null;

    }

    public async Task<bool> MarkProductAsSoldAsync(int id)
    {
    string requestUri = $"/api/Product/{id}";
    Product product = await GetProductAsync(id);
    if (product != null)
    {
        product.Sold = true; // set the Sold property to true
        string productJson = JsonConvert.SerializeObject(product);
        HttpContent httpContent = new StringContent(productJson, Encoding.UTF8, "application/json");
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Put, requestUri);
        request.Content = httpContent;
        HttpResponseMessage response = await _productsHttpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
    return false;
    }

    public async Task<List<Product>> GetProductsByIdsAsync(List<int> ids)
    {
        // validate input
        if (ids == null || !ids.Any())
        {
            throw new ArgumentException("At least one ID must be provided.");
        }

        // create the query string from the ids
        string queryString = $"?id={string.Join("&id=", ids)}";

        // send the request to the products APIs
        string requestUri = $"/api/getProductsByIds{queryString}";
        HttpResponseMessage response = await _productsHttpClient.GetAsync(requestUri);

        // check if the response is successful
        if (response.IsSuccessStatusCode)
        {
            // deserialize the response body to a list of products
            List<Product> products = await response.Content.ReadFromJsonAsync<List<Product>>();
            return products;
        }

        // if the response is ne ot successful, throw an exception
        throw new HttpRequestException($"Failed to get products by IDs. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
    }

   

    

   
}
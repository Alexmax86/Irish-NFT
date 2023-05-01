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
            return products;
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

   
}
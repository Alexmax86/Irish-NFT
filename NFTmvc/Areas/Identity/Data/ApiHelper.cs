using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NFTmvc.Models;
using NFTmvc.Areas.Identity.Data;
using Newtonsoft.Json;
//using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NFTmvc.Data;

public class ApiHelper
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly HttpClient _httpClient;

    public ApiHelper(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _httpClient = _clientFactory.CreateClient(name: "ProductsApi");
    }

    private HttpRequestMessage CreateHttpRequestMessage(HttpMethod httpMethod, string requestUri)
    {
        return new HttpRequestMessage(httpMethod, requestUri);
    }

    public async Task<Product> GetProductAsync(int id)
    {
        string requestUri = $"/api/Product/{id}";
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, requestUri);
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            Product requestedProduct = await response.Content.ReadFromJsonAsync<Product>();
            return requestedProduct;
        }
        return null;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        string requestUri = $"/api/Product";
        HttpRequestMessage request = CreateHttpRequestMessage(HttpMethod.Get, requestUri);
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            List<Product> products = await response.Content.ReadFromJsonAsync<List<Product>>();
            return products;
        }
        return null;
    }

   
}
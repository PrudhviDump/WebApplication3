using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5052/api/Product/");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("Get");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var products = JsonSerializer.Deserialize<List<ProductModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return View(products);
                }
                else
                {
                    // Handle error appropriately
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Log or handle the exception as needed
                return StatusCode(500); // Internal server error
            }
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductModel model)
        {
            try
            {
                var json = JsonSerializer.Serialize(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("AddProduct", data);

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to product list after successful addition
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to add product. Please try again.");
                    return View(model);
                }
            }
            catch (HttpRequestException ex)
            {
                // Log or handle the exception as needed
                return StatusCode(500); // Internal server error
            }
        }
    }
}

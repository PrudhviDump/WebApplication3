using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<IActionResult> AdminHomepage()
        {
            var response = await _httpClient.GetAsync("Get");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<List<ProductModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); ;
                return View("AdminHomepage", products);
            }
            else
            {
                // Handle error
                return View("Error");
            }
        }

        public async Task<IActionResult> UserHomepage()
        {
            var response = await _httpClient.GetAsync("Get");
            if (response.IsSuccessStatusCode)
            {
                var products = await JsonSerializer.DeserializeAsync<List<ProductModel>>(await response.Content.ReadAsStreamAsync());
                return View("UserHomepage", products);
            }
            else
            {
                // Handle error
                return View("Error");
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

        public async Task<IActionResult> Update(int id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "User"); // Redirect to login if token is not found
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var product = JsonSerializer.Deserialize<UpdateProductModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(UpdateProduct);
            }
            else
            {
                // Handle error
                return View("Error");
            }
        }
        //[HttpPost]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductModel model)
        {
            try
            {
                var token = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "User"); // Redirect to login if token is not found
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"UpdateProduct{id}", data);

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to product list after successful update
                    return RedirectToAction("AdminHomepage", "Product");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to update product. Please try again.");
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

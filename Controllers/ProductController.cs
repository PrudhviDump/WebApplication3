using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("http://localhost:5052/api/product/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("getall");

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

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("addproduct", data);

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
    }
}

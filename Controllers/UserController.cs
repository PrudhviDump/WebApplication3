using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new System.Uri("http://localhost:5052/api/User/");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("login", data);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<UserResponseModel>(responseContent);

                // Save the JWT token in session storage
                HttpContext.Session.SetString("JwtToken", authResponse.token);

                // Save the user role in session storage
                HttpContext.Session.SetString("UserRole", authResponse.role.ToString());

                // Redirect to appropriate dashboard based on user role
                if (authResponse.role == 1) // Admin RoleId
                {
                    return RedirectToAction("AdminHomepage", "Product");
                }
                    return RedirectToAction("UserHomepage", "Homepage");
                
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }
        }
        
        public IActionResult Register()
        {
            return View();
        }
        //[Route("Registration")]
        [HttpPost]
        public async Task<IActionResult> Register(AddUserModel model)
        {
            
            var json = JsonSerializer.Serialize(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync("Registration", data);
                if (response.IsSuccessStatusCode)
                {
                    // Redirect to login page after successful registration
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    return View(model);
                }
            }
            catch (Exception ex) { }
            
                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                return View(model);
            
        }
    }
}

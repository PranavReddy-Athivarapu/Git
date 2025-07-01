using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppMVC.DTO;
using OnlinePharmacyAppMVC.Models;

namespace OnlinePharmacyAppMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;

        public CartController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }


        public async Task<IActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Home");
            }

            var response = await _httpClient.GetAsync($"https://localhost:7112/api/Cart/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var cartItems = await response.Content.ReadFromJsonAsync<List<CartModel>>();
                return View(cartItems);
            }

            return View(new List<CartModel>());
        }


    }
}

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
            ViewBag.UserId = userId;


            var response = await _httpClient.GetAsync($"https://localhost:7107/api/Cart/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var cartItems = await response.Content.ReadFromJsonAsync<List<CartModel>>();
                return View(cartItems);
            }

            return View(new List<CartModel>());
        }
        public async Task<IActionResult> ApplyDiscount()
        {
            var userIdString = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Home");
            }

            // Fetch cart
            var cartResponse = await _httpClient.GetAsync($"https://localhost:7107/api/Cart/{userId}");
            if (!cartResponse.IsSuccessStatusCode)
                return View("Error");

            var cartItems = await cartResponse.Content.ReadFromJsonAsync<List<CartModel>>();
            var subtotal = cartItems.Sum(i => i.Amount);

            // Fetch discount
            var discountResponse = await _httpClient.GetAsync($"https://localhost:7107/api/Discount/apply?userId={userId}&subtotal={subtotal}");
            if (!discountResponse.IsSuccessStatusCode)
                return View("Error");

            var discount = await discountResponse.Content.ReadFromJsonAsync<DiscountResponseDTO>();

            var viewModel = new CartPageViewModel
            {
                CartItems = cartItems,
                DiscountAmount = discount.DiscountAmount,
                DiscountCode = discount.DiscountCode,
                IsPercentage = discount.IsPercentage
            };

            return View("DiscountSummary", viewModel);
        }


    }
}
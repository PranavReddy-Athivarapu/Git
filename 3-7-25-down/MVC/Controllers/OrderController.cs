using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppMVC.DTO;
using OnlinePharmacyAppMVC.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace OnlinePharmacyAppMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _client;

        public OrderController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7107/api/")
            };
        }
        public async Task<IActionResult> ViewOrder()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7107/api/");
            HttpResponseMessage msg = await client.GetAsync("Order");
            msg.EnsureSuccessStatusCode();
            string respstring = await msg.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<GetOrder>(respstring);
            return View(list);

        }
        public async Task<IActionResult> ConfirmOrder()
        {
            var userIdString = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.UserId = userId;


            var response = await _client.GetAsync($"https://localhost:7107/api/Cart/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var cartItems = await response.Content.ReadFromJsonAsync<List<CartModel>>();
                var cartJson = JsonSerializer.Serialize(cartItems);
                HttpContext.Session.SetString("Cart", cartJson);

                return View(cartItems);
            }

            return View(new List<CartModel>());
        }
        [HttpPost]
        public IActionResult DownloadReport()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
                return NotFound("No cart data found.");

            var cart = JsonSerializer.Deserialize<List<CartModel>>(cartJson);

            var csv = new StringBuilder();
            csv.AppendLine("Medicine Name,Quantity,Price (each),Amount");

            foreach (var item in cart)
            {
                csv.AppendLine($"{item.MedName},{item.StockQty},{item.Price:F2},{item.Amount:F2}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "OrderSummary.csv");
        }
    }
}

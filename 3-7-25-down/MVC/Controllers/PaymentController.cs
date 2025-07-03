using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppMVC.DTO;

namespace OnlinePharmacyAppMVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly HttpClient _client;

        public PaymentController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7107/api/")
            };
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(decimal totalAmount)
        {
            var userIdString = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return RedirectToAction("Login", "Home");

            var order = new OrderDTO
            {
                userId = userId,
                totalAmount = totalAmount,
                status = "Placed",
                orderDate = DateOnly.FromDateTime(DateTime.Now),
                
            };

            var response = await _client.PostAsJsonAsync("Order", order);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Order placed!";
                return RedirectToAction("OrderSuccess");
            }

            TempData["Error"] = "Something went wrong while placing the order.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(decimal totalAmount)
        {
            var userIdString = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                TempData["Error"] = "User session expired. Please login again.";
                return RedirectToAction("Login", "Home");
            }

            //decimal totalAmount = 0;
            if (TempData["FinalTotal"] != null)
            {
                decimal.TryParse(TempData["FinalTotal"].ToString(), out totalAmount);
                TempData.Keep("FinalTotal"); // Optional: reuse if needed on redirect
            }

            var order = new OrderDTO
            {
                userId = userId,
                totalAmount = totalAmount,
                status = "Placed",
                orderDate = DateOnly.FromDateTime(DateTime.Now),
               
            };

            var response = await _client.PostAsJsonAsync("Order", order);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Order placed successfully!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "There was a problem placing your order.";
            return RedirectToAction("Index");
        }

    }
}

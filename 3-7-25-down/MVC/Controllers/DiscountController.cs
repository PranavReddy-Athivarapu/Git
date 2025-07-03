using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppMVC.DTO;

namespace OnlinePharmacyAppMVC.Controllers
{
    public class DiscountController : Controller
    {
        private readonly HttpClient _client;

        public DiscountController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7107/api/")
            };
        }

        // POST: /Discount/GetAvailableDiscounts
        [HttpPost]
        public async Task<IActionResult> GetAvailableDiscounts(int userId)
        {
            try
            {
                var response = await _client.GetAsync($"Discount/User/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var discounts = await response.Content.ReadFromJsonAsync<List<DiscountDTO>>();
                    //TempData["AvailableDiscounts"] = JsonConvert.SerializeObject(discounts);
                    TempData["UserId"] = userId;


                    // Optionally redirect or return a view where discounts can be shown
                    return RedirectToAction("Index", "Cart", new { userId = userId });
                }
                else
                {
                    TempData["Error"] = $"API Error: {response.StatusCode}";
                    return RedirectToAction("Index", "Cart");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Exception: {ex.Message}";
                return RedirectToAction("Index", "Cart");
            }
        }

        // POST: /Discount/ApplyDiscount
        [HttpPost]
        public async Task<IActionResult> ApplyDiscount(string discountCode)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("Discount/Apply", new { Code = discountCode });

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Discount applied successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to apply discount.";
                }

                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Exception: {ex.Message}";
                return RedirectToAction("Index", "Cart");
            }
        }
        [HttpGet]
        public async Task<IActionResult> UserDiscounts(int userId)
        {
            try
            {
                var response = await _client.GetAsync($"Discount/User/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var discounts = await response.Content.ReadFromJsonAsync<List<DiscountDTO>>();
                    ViewBag.UserId = userId;
                    return View(discounts);
                }
                TempData["Error"] = "No discounts found for this user.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to load discounts: {ex.Message}";
            }

            return View(new List<DiscountDTO>());
        }

    }

}

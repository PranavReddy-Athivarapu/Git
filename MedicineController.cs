using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppMVC.DTO;
using System.Text.Json;

namespace OnlinePharmacyAppMVC.Controllers
{
    public class MedicineController : Controller

    {
        private readonly HttpClient _client;

        public MedicineController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7269/api/")
            };
        }
        public async Task<IActionResult> ViewMedicine()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7269/api/");
            HttpResponseMessage msg = await client.GetAsync("Medicine");
            msg.EnsureSuccessStatusCode();
            string respstring = await msg.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<GetMedicine>(respstring);
            return View(list);

        }
        [HttpGet]
        public IActionResult AddMedicine()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMedicine(MedicineDTO medicine)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("Medicine", medicine);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Medicine added successfully!";
                    return RedirectToAction("ViewMedicine");
                }
                else
                {
                    TempData["Error"] = $"API Error: {response.StatusCode}";
                    return View(medicine);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Exception: {ex.Message}";
                return View(medicine);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditMedicine(int id)
        {
            var response = await _client.GetAsync($"Medicine/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not load medicine data.";
                return RedirectToAction("ViewMedicine");
            }

            var content = await response.Content.ReadAsStringAsync();
            var meds = JsonSerializer.Deserialize<MedicineDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(meds);
        }

        // POST: /User/EditUser
        [HttpPost]
        public async Task<IActionResult> EditMedicine(MedicineDTO meds)
        {
            var response = await _client.PutAsJsonAsync($"Medicine/{meds.medicineId}", meds);
            if (response.IsSuccessStatusCode)
            {

                TempData["Success"] = "Medicine updated successfully!";
                return RedirectToAction("ViewMedicine");
            }

            TempData["Error"] = "Update failed.";
            return View(meds);
        }
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppAPI.Models;
using OnlinePharmacyAppAPI.Services.Interfaces;

namespace OnlinePharmacyAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicinesController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        // GET: api/medicines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetAllMedicines()
        {
            var medicines = await _medicineService.GetAllMedicinesAsync();
            return Ok(medicines);
        }

        // GET: api/medicines/5/alternatives
        [HttpGet("{id}/alternatives")]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetAlternatives(int id)
        {
            var alternatives = await _medicineService.GetAlternativeMedicinesAsync(id);
            return alternatives.Any() ? Ok(alternatives) : NotFound();
        }

        // POST: api/medicines/replenish/5
        [HttpPost("replenish/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReplenishStock(int id, [FromBody] int quantity)
        {
            await _medicineService.ReplenishStockAsync(id, quantity);
            return NoContent();
        }
    }
}

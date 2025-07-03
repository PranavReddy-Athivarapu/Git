using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppAPI.DTO;
using OnlinePharmacyAppAPI.Services;

namespace OnlinePharmacyAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : Controller
    {
        Unity _unity;
        public DiscountController(Unity dba)
        {
            _unity = dba;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            List<DiscountDTO> lst = _unity.DiscountService.GetAllDiscounts();
            return Ok(new { Data = lst });
        }

        [HttpPost]
        public ActionResult AddMedicine(DiscountDTO inp)
        {
            bool Status = _unity.DiscountService.AddDiscount(inp);
            return Ok(new { Data = "Success in Adding Discount" });

        }
        [HttpPut("{id}")]
        public ActionResult UpdateDiscount(DiscountDTO inp, int id)
        {
            inp.DiscountId = id;
            bool Status = _unity.DiscountService.UpdateDiscount(inp);
            return Ok(new { Data = "Success in Updating Discount" });

        }
        // GET: api/Discount/User/2
        [HttpGet("User/{userId}")]
        public ActionResult GetByUser(int userId)
        {
            var userDiscounts = _unity.DiscountService.GetDiscountsByUserId(userId);

            if (userDiscounts == null || userDiscounts.Count == 0)
                return NotFound(new { Message = "No discounts found for this user." });

            return Ok(new { Data = userDiscounts });
        }
        [HttpGet("apply")]
        public IActionResult ApplyDiscount(int userId, decimal subtotal)
        {
            var (amount, code, isPercent) = _unity.DiscountService.GetApplicableDiscount(userId, subtotal);

            return Ok(new DiscountResponseDTO
            {
                DiscountAmount = amount,
                DiscountCode = code,
                IsPercentage = isPercent
            });
        }
    }


}

using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppAPI.DTO;
using OnlinePharmacyAppAPI.Model;
using OnlinePharmacyAppAPI.Services;

namespace OnlinePharmacyAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        Unity _unity;
        public OrderController(Unity dba)
        {
            _unity = dba;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            List<OrderDTO> lst = _unity.OrderService.GetAllOrders();
            return Ok(new { Data = lst });
        }

        [HttpPost]
        public ActionResult AddOrder(OrderDTO inp)
        {
            bool Status = _unity.OrderService.AddNewOrder(inp);
            return Ok(new { Data = "Success in Adding Order" });

        }
        [HttpPut("{id}")]
        public ActionResult UpdateOrder(OrderDTO inp, int id)
        {
            inp.OrderId = id;
            bool Status = _unity.OrderService.UpdateOrder(inp);
            return Ok(new { Data = "Success in Updating Order" });

        }
        [HttpDelete("{orderId}")]
        public ActionResult DeleteOrder(int orderId)
        {
            bool result = _unity.OrderService.DeleteOrder(orderId);
            if (!result)
                return NotFound(new { Error = "Order not found or could not be deleted" });

            return Ok(new { Data = "Order deleted successfully" });
        }


        //--------------------------------------------------------------------------------
        //[HttpPost]
        //public IActionResult PostOrder([FromBody] Order order)
        //{
        //    if (order == null)
        //        return BadRequest(new { Message = "Invalid order data." });

        //    var savedOrder = _unity.OrderService.AddOrder(order);
        //    return Ok(savedOrder);
        //}



    }
}

using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppAPI.Models;

namespace OnlinePharmacyAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderRequest request)
        {
            var order = await _orderService.CreateOrderAsync(request.UserId, request.Items);
            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderDetailsAsync(id);
            return order != null ? Ok(order) : NotFound();
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            await _orderService.CancelOrderAsync(id);
            return NoContent();
        }
    }
}

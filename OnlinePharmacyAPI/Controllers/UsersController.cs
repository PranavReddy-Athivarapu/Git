using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppAPI.Models;

namespace OnlinePharmacyAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users/5/orders
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(int id)
        {
            var orders = await _userService.GetUserOrdersAsync(id);
            return Ok(orders);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user != null ? Ok(user) : NotFound();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionGrade.DTOs;
using ProductionGrade.Services;

namespace ProductionGrade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;

        public OrdersController(OrderService service)
        {
            _service = service;
        }

        // GET: api/orders
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllAsync();
            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null) return NotFound();

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CreateOrderDto dto)
        {
            var result = await _service.PlaceOrderAsync(dto);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? Ok("Order deleted") : NotFound();
        }

        // POST: api/orders/checkout/{userId}
        [HttpPost("checkout/{userId}")]
        public async Task<IActionResult> Checkout(Guid userId, [FromServices] CheckoutService checkoutService)
        {
            var success = await checkoutService.CheckoutAsync(userId);
            return success ? Ok("Checkout successful") : BadRequest("Cart is empty or invalid");
        }

        // POST: api/orders/checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckoutOrderDto dto)
        {
            var result = await _service.CheckoutAsync(dto.UserId);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }
    }
}

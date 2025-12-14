using Microsoft.AspNetCore.Mvc;
using ProductionGrade.DTOs;
using ProductionGrade.Services;

namespace ProductionGrade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto dto)
        {
            var response = await _orderService.PlaceOrderAsync(dto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}

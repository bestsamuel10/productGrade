using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionGrade.DTOs;
using ProductionGrade.Models;
using ProductionGrade.Services;

namespace ProductionGrade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;

        public CartController(CartService service)
        {
            _service = service;
        }

        // GET: api/cart/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(Guid userId)
        {
            var cart = await _service.GetCartAsync(userId);
            return cart == null ? NotFound() : Ok(cart);
        }

        // POST: api/cart/{userId}/add
        [HttpPost("{userId}/add")]
        public async Task<IActionResult> AddItem(Guid userId, Guid productId, int quantity)
        {
            var cart = await _service.AddItemAsync(userId, productId, quantity);
            return Ok(cart);
        }

        // DELETE: api/cart/{userId}/clear
        [HttpDelete("{userId}/clear")]
        public async Task<IActionResult> ClearCart(Guid userId)
        {
            var cleared = await _service.ClearCartAsync(userId);
            return cleared ? Ok("Cart cleared") : NotFound();
        }

        // GET: api/cart/{userId}/secure
        [HttpGet("{userId}/secure")]
        public async Task<IActionResult> GetCartSecure(Guid userId)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != userId.ToString())
                return Forbid();

            var cart = await _service.GetCartAsync(userId);
            return Ok(cart);
        }
    }
}

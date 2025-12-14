using ProductionGrade.Data;
using ProductionGrade.DTOs;
using ProductionGrade.Models;
using Microsoft.EntityFrameworkCore;


namespace ProductionGrade.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // Get all orders
        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderLines)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderLines = o.OrderLines.Select(ol => new OrderLineDto
                    {
                        ProductId = ol.ProductId,
                        Quantity = ol.Quantity,
                        UnitPrice = ol.UnitPrice
                    }).ToList()
                })
                .ToListAsync();
        }









        public async Task<ApiResponse<OrderDto>> CheckoutAsync(Guid userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.Items.Any())
                    return ApiResponse<OrderDto>.Fail("Cart is empty.");

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderLines = new List<OrderLine>()
                };

                decimal totalAmount = 0;

                foreach (var item in cart.Items)
                {
                    var product = item.Product;
                    if (product == null)
                        return ApiResponse<OrderDto>.Fail($"Product {item.ProductId} not found.");

                    if (product.StockQuantity < item.Quantity)
                        return ApiResponse<OrderDto>.Fail($"Insufficient stock for product {product.Name}.");

                    product.StockQuantity -= item.Quantity;
                    _context.Products.Update(product);

                    var orderLine = new OrderLine
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };

                    order.OrderLines.Add(orderLine);
                    totalAmount += product.Price * item.Quantity;
                }

                order.TotalAmount = totalAmount;

                await _context.Orders.AddAsync(order);

                // Clear cart after checkout
                _context.CartItems.RemoveRange(cart.Items);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderLines = order.OrderLines.Select(ol => new OrderLineDto
                    {
                        ProductId = ol.ProductId,
                        Quantity = ol.Quantity,
                        UnitPrice = ol.UnitPrice
                    }).ToList()
                };

                return ApiResponse<OrderDto>.Ok(orderDto, "Checkout successful.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<OrderDto>.Fail($"Checkout failed: {ex.Message}");
            }
        }

























        // Get order by ID
        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderLines = order.OrderLines.Select(ol => new OrderLineDto
                {
                    ProductId = ol.ProductId,
                    Quantity = ol.Quantity,
                    UnitPrice = ol.UnitPrice
                }).ToList()
            };
        }

        // Place a new order
        public async Task<ApiResponse<OrderDto>> PlaceOrderAsync(CreateOrderDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    UserId = dto.UserId,
                    OrderDate = DateTime.UtcNow,
                    OrderLines = new List<OrderLine>()
                };

                decimal totalAmount = 0;

                foreach (var line in dto.OrderLines)
                {
                    var product = await _context.Products.FindAsync(line.ProductId);
                    if (product == null)
                        return ApiResponse<OrderDto>.Fail($"Product {line.ProductId} not found.");

                    if (product.StockQuantity < line.Quantity)
                        return ApiResponse<OrderDto>.Fail($"Insufficient stock for product {product.Name}.");

                    product.StockQuantity -= line.Quantity;
                    _context.Products.Update(product);

                    var orderLine = new OrderLine
                    {
                        ProductId = product.Id,
                        Quantity = line.Quantity,
                        UnitPrice = product.Price
                    };

                    order.OrderLines.Add(orderLine);
                    totalAmount += product.Price * line.Quantity;
                }

                order.TotalAmount = totalAmount;

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderLines = order.OrderLines.Select(ol => new OrderLineDto
                    {
                        ProductId = ol.ProductId,
                        Quantity = ol.Quantity,
                        UnitPrice = ol.UnitPrice
                    }).ToList()
                };

                return ApiResponse<OrderDto>.Ok(orderDto, "Order placed successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<OrderDto>.Fail($"Order failed: {ex.Message}");
            }
        }

        // Delete an order
        public async Task<bool> DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

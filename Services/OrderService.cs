using ProductionGrade.Abstractions;
using ProductionGrade.Data;
using ProductionGrade.DTOs;
using ProductionGrade.Models;
using Microsoft.EntityFrameworkCore;


namespace ProductionGrade.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;  // inject DbContext directly

        public OrderService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

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
                    var product = await _unitOfWork.Products.GetByIdAsync(line.ProductId);
                    if (product == null)
                        return ApiResponse<OrderDto>.Fail($"Product {line.ProductId} not found.");

                    if (product.StockQuantity < line.Quantity)
                        return ApiResponse<OrderDto>.Fail($"Insufficient stock for product {product.Name}.");

                    product.StockQuantity -= line.Quantity;
                    await _unitOfWork.Products.UpdateAsync(product);

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

                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

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
    }
}

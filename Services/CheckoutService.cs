using ProductionGrade.Abstractions;
using ProductionGrade.Models;

namespace ProductionGrade.Services
{
    public class CheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckoutAsync(Guid userId)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any()) return false;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderLines = new List<OrderLine>()
            };

            foreach (var item in cart.Items)
            {
                order.OrderLines.Add(new OrderLine
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product?.Price ?? 0
                });

                // Deduct stock
                if (item.Product != null)
                {
                    item.Product.StockQuantity -= item.Quantity;
                }
            }

            await _unitOfWork.Orders.CreateAsync(order);
            await _unitOfWork.Carts.ClearCartAsync(userId);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

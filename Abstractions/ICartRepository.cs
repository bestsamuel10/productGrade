using ProductionGrade.Models;

namespace ProductionGrade.Abstractions
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<Cart> AddItemAsync(Guid userId, Guid productId, int quantity);
        Task<bool> ClearCartAsync(Guid userId);
    }
}

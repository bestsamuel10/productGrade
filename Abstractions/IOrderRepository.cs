using ProductionGrade.Models;

namespace ProductionGrade.Abstractions
{
    public interface IOrderRepository
    {

        Task CreateAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
    }
}



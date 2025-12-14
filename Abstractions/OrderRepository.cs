using ProductionGrade.Abstractions;
using ProductionGrade.Data;
using ProductionGrade.Models;

namespace ProductionGrade.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int id) =>
            await _context.Orders.FindAsync(id);

        public async Task AddAsync(Order order) =>
            await _context.Orders.AddAsync(order);
    }
}

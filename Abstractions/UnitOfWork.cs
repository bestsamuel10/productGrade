using ProductionGrade.Abstractions;
using ProductionGrade.Data;

namespace ProductionGrade.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context,
                          IProductRepository productRepository,
                          IOrderRepository orderRepository)
        {
            _context = context;
            Products = productRepository;
            Orders = orderRepository;
        }

        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}

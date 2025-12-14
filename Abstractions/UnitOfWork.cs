using ProductionGrade.Abstractions;
using ProductionGrade.Data;

namespace ProductionGrade.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(
            AppDbContext context,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            ICategoryRepository categoryRepository,
            ICartRepository cartRepository)
        {
            _context = context;
            Products = productRepository;
            Orders = orderRepository;
            Categories = categoryRepository;
            Carts = cartRepository;
        }

        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        public ICategoryRepository Categories { get; }
        public ICartRepository Carts { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

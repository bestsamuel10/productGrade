using System.Threading.Tasks;

namespace ProductionGrade.Abstractions
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        ICategoryRepository Categories { get; }
        ICartRepository Carts { get; }
        Task<int> SaveChangesAsync();
    }
}

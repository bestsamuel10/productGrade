using ProductionGrade.Models;

namespace ProductionGrade.Abstractions
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category> CreateAsync(Category category);
        Task<bool> DeleteAsync(Guid id);
    }
}

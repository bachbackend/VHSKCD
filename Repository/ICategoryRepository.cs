using VHSKCD.Models;

namespace VHSKCD.Repository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category entity);
        Task UpdateAsync(Category entity);
    }
}

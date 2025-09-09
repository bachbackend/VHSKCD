using VHSKCD.DTOs;
using VHSKCD.Models;

namespace VHSKCD.Services.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(CategoriesDTO category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}

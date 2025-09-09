using VHSKCD.DTOs;
using VHSKCD.Models;

namespace VHSKCD.Services.Impl
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<CategoriesDTO> CreateAsync(CategoriesDTO category);
        Task<Category?> UpdateAsync(int id, Category category);
        Task<bool> DeleteAsync(int id);
    }
}

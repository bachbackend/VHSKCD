using VHSKCD.DTOs.Banner;
using VHSKCD.DTOs.Categories;
using VHSKCD.Models;

namespace VHSKCD.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(AddCategory dto);
        Task<Category?> EditAsync(int id, UpdateCategory dto);
    }
}

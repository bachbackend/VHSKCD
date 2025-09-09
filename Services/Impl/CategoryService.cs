using VHSKCD.DTOs;
using VHSKCD.Models;
using VHSKCD.Services.Interface;

namespace VHSKCD.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<Category?> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<CategoriesDTO> CreateAsync(CategoriesDTO category)
        {
            category.CreatedAt = DateTime.Now;
            await _repo.AddAsync(category);
            return category;
        }

        public async Task<Category?> UpdateAsync(int id, Category category)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = category.Name;
            existing.ParentId = category.ParentId;
            existing.CreatedAt = existing.CreatedAt; // giữ nguyên ngày tạo

            await _repo.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}

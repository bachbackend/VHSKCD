using VHSKCD.DTOs.Categories;
using VHSKCD.Models;
using VHSKCD.Repository;
using VHSKCD.Repository.Impl;

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
        {
            var categories = await _repo.GetAllAsync();
            return categories.Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                CreatedAt = (DateTime)c.CreatedAt,
                
            });
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;

            return new Category
            {
                Id = c.Id,
                Name = c.Name,
                ParentId = c.ParentId,
                CreatedAt = (DateTime)c.CreatedAt
            };
        }


        public async Task<Category> AddAsync(AddCategory dto)
        {
            var entity = new Category
            {
                Name = dto.Name,
                ParentId = dto.ParentId,
                CreatedAt = DateTime.UtcNow
            };


            await _repo.AddAsync(entity);

            return entity;
        }

        public async Task<Category?> EditAsync(int id, UpdateCategory dto)
        {

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Category not found.");


            entity.Name = dto.Name;
            entity.ParentId = dto.ParentId;

            await _repo.UpdateAsync(entity);

            return new Category
            {
                //Id = entity.Id,
                Name = entity.Name,
                ParentId = entity.ParentId,
                CreatedAt = (DateTime)entity.CreatedAt
            };
        }
    }
}

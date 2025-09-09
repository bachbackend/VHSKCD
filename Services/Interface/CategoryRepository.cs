using Microsoft.EntityFrameworkCore;
using VHSKCD.DTOs;
using VHSKCD.Models;

namespace VHSKCD.Services.Interface
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly B4zgrbg0p5agywu5uoneContext _context;
        public CategoryRepository(B4zgrbg0p5agywu5uoneContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _context.Categories.ToListAsync();

        public async Task<Category?> GetByIdAsync(int id)
            => await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(CategoriesDTO category)
        {
            var newCate = new Category
            {
                Name = category.Name,
                CreatedAt = DateTime.Now,
                ParentId = category.ParentId,
            };
            await _context.Categories.AddAsync(newCate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}

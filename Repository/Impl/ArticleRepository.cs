using Microsoft.EntityFrameworkCore;
using VHSKCD.DTOs.Articles;
using VHSKCD.Models;

namespace VHSKCD.Repository.Impl
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly B4zgrbg0p5agywu5uoneContext _context;

        public ArticleRepository(B4zgrbg0p5agywu5uoneContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<Article>> GetAllAsync()
        {
            return await Task.FromResult(
                 _context.Articles
                .Include(p => p.Category)
                .AsQueryable()
            );
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null) return false;

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IQueryable<Article>> GetAllStatusZeroAsync()
        {
            return await Task.FromResult(
                _context.Articles
                .Include(p => p.Category)
                .Where(p => p.Status == 0)
                .AsQueryable()
            );
        }

        public async Task<List<Article>> GetRandomAsync(int count)
        {
            return await _context.Articles
            .Include(p => p.Category)
            .OrderBy(r => Guid.NewGuid()) 
            .Take(count)
            .ToListAsync();
        }

        public async Task<List<Article>> GetLatestAsync(int count)
        {
            return await _context.Articles
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
        }

        public async Task<IQueryable<Article>> GetByCategoryIdAsync(int categoryId)
        {
            return await Task.FromResult(
                _context.Articles
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .AsQueryable()
            );
        }

        //public async Task<List<Article>> GetByCategoryIdAsync(int categoryId)
        //{
        //    return await _context.Articles
        //    .Include(p => p.Category)
        //    .Where(p => p.CategoryId == categoryId)
        //    .ToListAsync();
        //}
    }
}

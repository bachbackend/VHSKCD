using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.Include(a => a.Category).Include(a => a.User).ToListAsync();
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles
                .Include(a => a.Category)
                .Include(a => a.User)
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
    }
}

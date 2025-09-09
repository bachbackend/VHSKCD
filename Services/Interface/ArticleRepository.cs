using Microsoft.EntityFrameworkCore;
using VHSKCD.Models;

namespace VHSKCD.Services.Interface
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly B4zgrbg0p5agywu5uoneContext _context;
        public ArticleRepository (B4zgrbg0p5agywu5uoneContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Article>> GetAllAsync()
       => await _context.Articles.Include(a => a.Category).Include(a => a.User).ToListAsync();

        public async Task<Article?> GetByIdAsync(int id)
            => await _context.Articles.Include(a => a.Category).Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);

        public async Task AddAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }
    }
}

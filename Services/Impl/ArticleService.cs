using VHSKCD.Models;
using VHSKCD.Services.Interface;

namespace VHSKCD.Services.Impl
{
    public class ArticleService
    {
        private readonly IArticleRepository _repo;
        public ArticleService(IArticleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Article>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Article?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<Article> CreateAsync(Article article)
        {
            article.CreatedAt = DateTime.Now;
            await _repo.AddAsync(article);
            return article;
        }

        public async Task<Article?> UpdateAsync(int id, Article article)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Title = article.Title;
            existing.Thumbnail = article.Thumbnail;
            existing.Status = article.Status;
            existing.UpdateAt = DateTime.Now;
            existing.CategoryId = article.CategoryId;

            await _repo.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var article = await _repo.GetByIdAsync(id);
            if (article == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}

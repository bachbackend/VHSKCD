using VHSKCD.Models;

namespace VHSKCD.Repository
{
    public interface IArticleRepository
    {
        Task<IQueryable<Article>> GetAllAsync();
        Task<IQueryable<Article>> GetAllStatusZeroAsync();
        Task<Article?> GetByIdAsync(int id);
        Task AddAsync(Article entity);
        Task UpdateAsync(Article entity);
        Task<List<Article>> GetRandomAsync(int count);
        Task<List<Article>> GetLatestAsync(int count);
        Task<List<Article>> GetByCategoryIdAsync(int categoryId);
    }
}

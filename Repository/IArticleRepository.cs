using VHSKCD.Models;

namespace VHSKCD.Repository
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article?> GetByIdAsync(int id);
        Task AddAsync(Article entity);
        Task UpdateAsync(Article entity);
    }
}

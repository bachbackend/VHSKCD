using VHSKCD.DTOs.Articles;
using VHSKCD.Models;

namespace VHSKCD.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article?> GetByIdAsync(int id);
        Task<Article> AddAsync(AddArticle dto);
        Task<Article?> EditAsync(int id, UpdateArticle dto);
    }
}

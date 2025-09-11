using VHSKCD.DTOs.Articles;
using VHSKCD.Models;

namespace VHSKCD.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article?> GetByIdAsync(int id);
        Task<Article> AddAsync(IFormFile file, AddArticle dto);
        Task<Article?> EditAsync(IFormFile file, int id, UpdateArticle dto);
    }
}

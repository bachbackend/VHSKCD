using VHSKCD.DTOs.Articles;
using VHSKCD.DTOs.Paging;
using VHSKCD.Models;

namespace VHSKCD.Services
{
    public interface IArticleService
    {
        Task<(List<ArticleReturnDTO>, PagingReturn)> GetArticlesAsync(
        int pageNumber, int pageSize, int? status, string? title,
        int? categoryId, string? sortBy, string? sortOrder);
        Task<(List<ArticleReturnDTO>, PagingReturn)> GetArticlesStatusZeroAsync(
        int pageNumber, int pageSize, string? title, int? categoryId);
        Task<Article?> GetByIdAsync(int id);
        Task<Article> AddAsync(IFormFile file, AddArticle dto);
        Task<Article?> EditAsync(IFormFile file, int id, UpdateArticle dto);
        Task<List<ArticleReturnDTO>> GetRandomArticlesAsync(int count);
        Task<List<ArticleReturnDTO>> GetLatestArticlesAsync(int count);
        Task<List<ArticleReturnDTO>> GetByCategoryIdAsync(int categoryId);
    }
}

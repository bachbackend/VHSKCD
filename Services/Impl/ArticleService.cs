using VHSKCD.DTOs.Articles;
using VHSKCD.Models;
using VHSKCD.Repository;
using VHSKCD.Repository.Impl;

namespace VHSKCD.Services.Impl
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repo;
        public ArticleService(IArticleRepository repo)
        {
            _repo = repo;
        }

        public async Task<Article> AddAsync(IFormFile file, AddArticle dto)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No image uploaded.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid file type.");

            // Save file
            var fileName = Guid.NewGuid() + extension;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/article", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var art = new Article
            {
                Title = dto.Title,
                Content = dto.Content,
                Thumbnail = fileName,
                Status = dto.Status,
                CreatedAt = DateTime.Now
            };
            await _repo.AddAsync(art);
            return art;

        }

        public async Task<Article?> EditAsync(IFormFile file, int id, UpdateArticle dto)
        {
            var article = await _repo.GetByIdAsync(id);
            if (article == null)
                throw new Exception("Article not found.");

            // update fields
            article.Title = dto.Title;
            article.Content = dto.Content;
            //article.Thumbnail = dto.Thumbnail;
            article.Status = dto.Status;
            article.UpdateAt = DateTime.UtcNow;

            // handle file
            if (file != null && file.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    throw new Exception("Invalid file type. Allowed types: .jpg, .jpeg, .png");

                var fileName = Guid.NewGuid() + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/article", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                article.Thumbnail = fileName;
            }

            await _repo.UpdateAsync(article);

            return article;
        }
        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Article?> GetByIdAsync(int id)
        {
            if (id == null) return null;
            return await _repo.GetByIdAsync(id);
        }
    }
}

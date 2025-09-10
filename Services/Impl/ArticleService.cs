using VHSKCD.DTOs.Articles;
using VHSKCD.Models;
using VHSKCD.Repository.Impl;

namespace VHSKCD.Services.Impl
{
    public class ArticleService
    {
        private readonly ArticleRepository _repo;
        public ArticleService(ArticleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ArticlesDTO>> GetAllAsync()
        {
            var articles = await _repo.GetAllAsync();
            return articles.Select(a => new ArticlesDTO
            {
                Id = a.Id,
                CategoryId = a.CategoryId,
                Title = a.Title,
                Thumbnail = a.Thumbnail,
                Status = a.Status,
                CreatedAt = (DateTime)a.CreatedAt,
                UserId = a.UserId,
                UpdateAt = a.UpdateAt
            });
        }

        public async Task<ArticlesDTO> GetByIdAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return null;

            return new ArticlesDTO
            {
                Id = a.Id,
                CategoryId = a.CategoryId,
                Title = a.Title,
                Thumbnail = a.Thumbnail,
                Status = a.Status,
                CreatedAt = a.CreatedAt,
                UserId = a.UserId,
                UpdateAt = a.UpdateAt
            };
        }

        public async Task<ArticlesDTO> CreateAsync(AddArticle dto)
        {
            var entity = new Article
            {
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Thumbnail = dto.Thumbnail,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow,
                UserId = dto.UserId
            };

            await _repo.AddAsync(entity);

            return new ArticlesDTO
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                Title = entity.Title,
                Thumbnail = entity.Thumbnail,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                UserId = entity.UserId
            };
        }

        public async Task<ArticlesDTO> UpdateAsync(UpdateArticle dto)
        {
            var entity = await _repo.GetByIdAsync(dto.Id);
            if (entity == null) return null;

            entity.CategoryId = dto.CategoryId;
            entity.Title = dto.Title;
            entity.Thumbnail = dto.Thumbnail;
            entity.Status = dto.Status;
            entity.UserId = dto.UserId;
            entity.UpdateAt = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);

            return new ArticlesDTO  
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                Title = entity.Title,
                Thumbnail = entity.Thumbnail,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                UserId = entity.UserId,
                UpdateAt = entity.UpdateAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}

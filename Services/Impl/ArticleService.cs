using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata;
using VHSKCD.DTOs.Articles;
using VHSKCD.DTOs.Paging;
using VHSKCD.Models;
using VHSKCD.Repository;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Description = dto.Description,
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
        //public async Task<IEnumerable<Article>> GetAllAsync()
        //{
        //    return await _repo.GetAllAsync();
        //}

        public async Task<(List<ArticleReturnDTO>, PagingReturn)> GetArticlesAsync(int pageNumber, int pageSize, int? status, string? title, int? categoryId, string? sortBy, string? sortOrder)
        {
            var articles = await _repo.GetAllAsync();

            // Filter
            if (!string.IsNullOrEmpty(title))
                articles = articles.Where(p => p.Title.Contains(title));

            if (categoryId.HasValue)
                articles = articles.Where(p => p.CategoryId == categoryId.Value);

            if (status.HasValue)
                articles = articles.Where(p => p.Status == status.Value);

            // Sort
            sortBy = sortBy?.ToLower();
            sortOrder = sortOrder?.ToLower() ?? "asc";

            if (sortBy == "title")
                articles = sortOrder == "desc" ? articles.OrderByDescending(p => p.Title) : articles.OrderBy(p => p.Title);
            else if (sortBy == "createdate")
                articles = sortOrder == "desc" ? articles.OrderByDescending(p => p.CreatedAt) : articles.OrderBy(p => p.CreatedAt);
            else
                articles = sortOrder == "desc" ? articles.OrderByDescending(p => p.Id) : articles.OrderBy(p => p.Id);

            // Pagination
            int totalArticleCount = await articles.CountAsync();
            int totalPageCount = (int)Math.Ceiling(totalArticleCount / (double)pageSize);
            int nextPage = pageNumber + 1 > totalPageCount ? pageNumber : pageNumber + 1;
            int previousPage = pageNumber - 1 < 1 ? pageNumber : pageNumber - 1;

            var pagingResult = new PagingReturn
            {
                TotalPageCount = totalPageCount,
                CurrentPage = pageNumber,
                NextPage = nextPage,
                PreviousPage = previousPage
            };

            var articletWithPaging = await articles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ArticleReturnDTO
                {
                    Id = p.Id,
                    Thumbnail = p.Thumbnail,
                    Title = p.Title,
                    Content = p.Content,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    ArticleCateId = p.CategoryId,
                    ArticleCateName = p.Category.Name,
                })
                .ToListAsync();

            return (articletWithPaging, pagingResult);
        }

        public async Task<(List<ArticleReturnDTO>, PagingReturn)> GetArticlesStatusZeroAsync(int pageNumber, int pageSize, string? title, int? categoryId)
        {
            var articles = await _repo.GetAllAsync();

            // Filter
            if (!string.IsNullOrEmpty(title))
                articles = articles.Where(p => p.Title.Contains(title));

            if (categoryId.HasValue)
                articles = articles.Where(p => p.CategoryId == categoryId.Value);

            // Sắp xếp theo createdDate giảm dần
            articles = articles.OrderByDescending(p => p.CreatedAt);

            // Pagination
            int totalArticleCount = await articles.CountAsync();
            int totalPageCount = (int)Math.Ceiling(totalArticleCount / (double)pageSize);
            int nextPage = pageNumber + 1 > totalPageCount ? pageNumber : pageNumber + 1;
            int previousPage = pageNumber - 1 < 1 ? pageNumber : pageNumber - 1;

            var pagingResult = new PagingReturn
            {
                TotalPageCount = totalPageCount,
                CurrentPage = pageNumber,
                NextPage = nextPage,
                PreviousPage = previousPage
            };

            var articletWithPaging = await articles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ArticleReturnDTO
                {
                    Id = p.Id,
                    Thumbnail = p.Thumbnail,
                    Title = p.Title,
                    Content = p.Content,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    ArticleCateId = p.CategoryId,
                    ArticleCateName = p.Category.Name,
                })
                .ToListAsync();

            return (articletWithPaging, pagingResult);
        }

        public async Task<(List<ArticleReturnDTO>, PagingReturn)> GetByCategoryIdAsync(int categoryId, int pageNumber, int pageSize)
        {
            var articles = await _repo.GetByCategoryIdAsync(categoryId);

            articles = articles.OrderByDescending(p => p.CreatedAt);

            int totalArticleCount = await articles.CountAsync();
            int totalPageCount = (int)Math.Ceiling(totalArticleCount / (double)pageSize);
            int nextPage = pageNumber + 1 > totalPageCount ? pageNumber : pageNumber + 1;
            int previousPage = pageNumber - 1 < 1 ? pageNumber : pageNumber - 1;

            var pagingResult = new PagingReturn
            {
                TotalPageCount = totalPageCount,
                CurrentPage = pageNumber,
                NextPage = nextPage,
                PreviousPage = previousPage
            };

            var articlesWithPaging = await articles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ArticleReturnDTO
                {
                    Id = p.Id,
                    Thumbnail = p.Thumbnail,
                    Title = p.Title,
                    Content = p.Content,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    ArticleCateId = p.CategoryId,
                    ArticleCateName = p.Category.Name,
                })
                .ToListAsync();

            return (articlesWithPaging, pagingResult);

        }



        //public async Task<List<ArticleReturnDTO>> GetByCategoryIdAsync(int categoryId)
        //{
        //    var articles = await _repo.GetByCategoryIdAsync(categoryId);

        //    return articles.Select(p => new ArticleReturnDTO
        //    {
        //        Id = p.Id,
        //        Thumbnail = p.Thumbnail,
        //        Title = p.Title,
        //        Content = p.Content,
        //        Status = p.Status,
        //        CreatedAt = p.CreatedAt,
        //        ArticleCateId = p.CategoryId,
        //        ArticleCateName = p.Category.Name
        //    }).ToList();
        //}

        public async Task<Article?> GetByIdAsync(int id)
        {
            if (id == null) return null;
            return await _repo.GetByIdAsync(id);
        }

        public async Task<List<ArticleReturnDTO>> GetLatestArticlesAsync(int count)
        {
            var articles = await _repo.GetLatestAsync(count);

            return articles.Select(p => new ArticleReturnDTO
            {
                Id = p.Id,
                ArticleCateId = p.CategoryId,
                ArticleCateName = p.Category.Name,
                Title = p.Title,
                Content = p.Content,
                Thumbnail = p.Thumbnail,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        public async Task<List<ArticleReturnDTO>> GetRandomArticlesAsync(int count)
        {
            var articles = await _repo.GetRandomAsync(count);

            return articles.Select(p => new ArticleReturnDTO
            {
                Id = p.Id,
                ArticleCateId = p.CategoryId,
                ArticleCateName = p.Category.Name,
                Title = p.Title,
                Content = p.Content,
                Thumbnail = p.Thumbnail,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        public async Task<byte[]> GeneratePdfAsync(int id)
        {
            var article = await _repo.GetByIdAsync(id);
            if (article == null)
                throw new Exception("Không tìm thấy bài viết.");

            QuestPDF.Settings.License = LicenseType.Community;

            using var stream = new MemoryStream();

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "article", article.Thumbnail);
            byte[]? imageData = null;
            if (File.Exists(imagePath))
            {
                imageData = await File.ReadAllBytesAsync(imagePath);
            }

            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);

                    page.Header()
                        .Text(article.Title)
                        .SemiBold().FontSize(22)
                        .AlignCenter()
                        .FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        col.Spacing(10);

                        if (imageData != null)
                        {
                            col.Item().Element(e =>
                            {
                                e.Height(200);      
                                e.AlignCenter();
                                using var imgStream = new MemoryStream(imageData);
                                e.Image(imgStream)    
                                 .FitWidth();
                            });
                        }

                        col.Item().Text($"Loại bài viết: {article.Category?.Name ?? "Không có"}")
                            .FontSize(12)
                            .FontColor(Colors.Grey.Darken1);

                        col.Item().Text($"Ngày tạo: {article.CreatedAt?.ToString("dd/MM/yyyy") ?? "Không rõ"}")
                            .FontSize(12)
                            .FontColor(Colors.Grey.Darken1);

                        if (!string.IsNullOrEmpty(article.Description))
                        {
                            col.Item().Text("Mô tả:")
                                .Bold().FontSize(14).FontColor(Colors.Black);
                            col.Item().Text(article.Description)
                                .FontSize(12)
                                .FontColor(Colors.Black);
                        }

                        if (!string.IsNullOrEmpty(article.Content))
                        {
                            col.Item().Text("Nội dung:")
                                .Bold().FontSize(14).FontColor(Colors.Black);
                            col.Item().Text(article.Content)
                                .FontSize(12)
                                .FontColor(Colors.Black)
                                .AlignLeft()
                                .LineHeight(1.4f);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Tải từ hệ thống lúc {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Medium);
                });
            }).GeneratePdf(stream);

            return stream.ToArray();
        }


    }
}

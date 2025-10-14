using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using VHSKCD.DTOs.Articles;
using VHSKCD.Extension;
using VHSKCD.Models;
using VHSKCD.Services;
using VHSKCD.Services.Impl;

namespace VHSKCD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _service;
        private readonly PaginationSettings _paginationSettings;
        public ArticlesController(IArticleService service, IOptions<PaginationSettings> paginationSettings)
        {
            _service = service;
            _paginationSettings = paginationSettings.Value;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var articles = await _service.GetAllAsync();
        //    return Ok(articles);
        //}

        [HttpGet("GetAllArticle")]
        public async Task<IActionResult> GetAllArticle(
        int pageNumber = 1,
        int? pageSize = null,
        int? status = null,
        string? title = null,
        int? categoryId = null,
        string? sortBy = "id",
        string? sortOrder = "asc"
    )
        {
            int actualPageSize = pageSize ?? _paginationSettings.DefaultPageSize;

            var (articles, paging) = await _service.GetArticlesAsync(
                pageNumber, actualPageSize, status, title, categoryId, sortBy, sortOrder);

            return Ok(new { Articles = articles, Paging = paging });
        }

        [HttpGet("GetAllArticleStatusZero")]
        public async Task<IActionResult> GetAllArticleStatusZero(   
        int pageNumber = 1,
        int? pageSize = null,
        string? title = null,
        int? categoryId = null
    )
        {
            int actualPageSize = pageSize ?? _paginationSettings.DefaultPageSize;

            var (articles, paging) = await _service.GetArticlesStatusZeroAsync(
                pageNumber, actualPageSize, title, categoryId);

            return Ok(new { Articles = articles, Paging = paging });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _service.GetByIdAsync(id);
            if (article == null) return NotFound();
            return Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, [FromForm] AddArticle dto)
        {
            var article = await _service.AddAsync(file,dto);
            return Ok(new { articleId = article.Id, fileName = article.Thumbnail });
            //return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateArticle dto, IFormFile? file)
        {
            if (id != dto.Id) return BadRequest();

            var article = await _service.EditAsync(file,id,dto);
            if (article == null) return NotFound();

            return Ok(article);
        }

        [HttpGet("GetRandom5Article")]
        public async Task<IActionResult> GetRandom5Article()
        {
            var randomArticles = await _service.GetRandomArticlesAsync(5);
            return Ok(randomArticles);
        }

        [HttpGet("GetLatestArticles")]
        public async Task<IActionResult> GetLatestArticles()
        {
            var latestArticles = await _service.GetLatestArticlesAsync(5);

            if (latestArticles == null || !latestArticles.Any())
            {
                return NotFound(new { Message = "Không có bài viết nào." });
            }

            return Ok(latestArticles);
        }

        [HttpGet("GetArticleByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetArticleByCategoryId(int categoryId, int pageNumber = 1, int? pageSize = null)
        {
            int actualPageSize = pageSize ?? _paginationSettings.DefaultPageSize;

            var (articles, paging) = await _service.GetByCategoryIdAsync(categoryId, pageNumber, actualPageSize);

            return Ok(new { Articles = articles, Paging = paging });
        }

        [HttpGet("Get5ArticlesByCategoryId/{categoryId}")]
        public async Task<IActionResult> Get5ArticlesByCategoryId(int categoryId)
        {
            
            var articles = await _service.GetTopArticlesByCategoryId(categoryId, 5);
            if (articles == null | !articles.Any())
            {
                return NotFound(new { Message = "Không tìm thấy bài viết nào trong danh mục này." });
            }
            return Ok(articles);
        }

        [HttpGet("DownloadPdf/{id}")]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            try
            {
                var pdfBytes = await _service.GeneratePdfAsync(id);
                return File(pdfBytes, "application/pdf", $"Article_{id}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /*[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }*/
    }
}

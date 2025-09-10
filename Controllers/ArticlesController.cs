using Microsoft.AspNetCore.Mvc;
using VHSKCD.DTOs.Articles;
using VHSKCD.Models;
using VHSKCD.Services.Impl;

namespace VHSKCD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _service;
        public ArticlesController(ArticleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _service.GetAllAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _service.GetByIdAsync(id);
            if (article == null) return NotFound();
            return Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddArticle dto)
        {
            var article = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateArticle dto)
        {
            if (id != dto.Id) return BadRequest();

            var article = await _service.UpdateAsync(dto);
            if (article == null) return NotFound();

            return Ok(article);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}

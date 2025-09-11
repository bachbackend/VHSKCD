using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using VHSKCD.DTOs.Articles;
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
        public ArticlesController(IArticleService service)
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

        /*[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }*/
    }
}

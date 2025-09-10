using Microsoft.AspNetCore.Mvc;
using VHSKCD.DTOs.Categories;
using VHSKCD.Models;
using VHSKCD.Services;
using VHSKCD.Services.Impl;

namespace VHSKCD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCategory dto)
        {
            var category = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategory dto)
        {
            try
            {
                var category = await _service.EditAsync(id, dto);
                return Ok(category);

            } catch (Exception ex)
            {
                return NotFound();
            }

        }


    }
}

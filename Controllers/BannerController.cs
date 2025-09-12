using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VHSKCD.DTOs.Banner;
using VHSKCD.Services;

namespace VHSKCD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        public BannerController(IBannerService service)
        {
            _bannerService = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllBanner()
        {
            var list = await _bannerService.GetAllAsync();
            if (!list.Any())
                return NotFound("Không tìm thấy quảng cáo nào.");
            return Ok(list);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetBannerById(int id)
        {
            try
            {
                var banner = await _bannerService.GetByIdAsync(id);
                return Ok(banner);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAC(IFormFile file, [FromForm] AddBanner model)
        {
            try
            {
                var banner = await _bannerService.AddAsync(file, model);
                return Ok(new { bannerId = banner.Id, fileName = banner.Image });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] AddBanner model, IFormFile? file)
        {
            try
            {
                var banner = await _bannerService.EditAsync(file, id, model);

                return Ok(new
                {
                    message = "Banner updated successfully.",
                    bannerId = banner.Id,
                    title = banner.Title,
                    image = banner.Image,
                    link = banner.Link,
                    status = banner.Status,
                    startDate = banner.StartDate,
                    endDate = banner.EndDate,
                    createdAt = banner.CreatedAt,
                    updatedAt = banner.UpdatedAt,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

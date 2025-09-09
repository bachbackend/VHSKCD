using VHSKCD.DTOs.Banner;
using VHSKCD.Models;
using VHSKCD.Repository;
using Microsoft.EntityFrameworkCore;

namespace VHSKCD.Services.Impl
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _repo;
        public BannerService(IBannerRepository repo)
        {
            _repo = repo;
        }

        public async Task<Banner> AddAsync(IFormFile file, AddBanner dto)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No image uploaded.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid file type.");

            // Save file
            var fileName = Guid.NewGuid() + extension;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/banner", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var banner = new Banner
            {
                Title = dto.Title,
                Image = fileName,
                Link = dto.Link,
                Status = dto.Status,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _repo.AddAsync(banner);

            return banner;
        }

        public async Task<Banner?> EditAsync(IFormFile file, int id, AddBanner dto)
        {
            var banner = await _repo.GetByIdAsync(id);
            if (banner == null)
                throw new Exception("Banner not found.");

            // update fields
            banner.Title = dto.Title;
            banner.Link = dto.Link;
            banner.Status = dto.Status;
            banner.StartDate = dto.StartDate;
            banner.EndDate = dto.EndDate;
            banner.CreatedAt = dto.CreatedAt;
            banner.UpdatedAt = DateTime.UtcNow;

            // handle file
            if (file != null && file.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    throw new Exception("Invalid file type. Allowed types: .jpg, .jpeg, .png");

                var fileName = Guid.NewGuid() + extension;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/banner", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                banner.Image = fileName;
            }

            await _repo.UpdateAsync(banner);

            return banner;
        }

        public async Task<IEnumerable<Banner>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Banner?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
    }
}

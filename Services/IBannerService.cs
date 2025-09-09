using Microsoft.AspNetCore.Http.HttpResults;
using VHSKCD.DTOs.Banner;
using VHSKCD.Models;

namespace VHSKCD.Services
{
    public interface IBannerService
    {
        Task<IEnumerable<Banner>> GetAllAsync();
        Task<Banner?> GetByIdAsync(int id);
        Task<Banner> AddAsync(IFormFile file, AddBanner dto);
        Task<Banner?> EditAsync(IFormFile file, int id, AddBanner dto);
    }
}

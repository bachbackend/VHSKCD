using VHSKCD.Models;

namespace VHSKCD.Repository
{
    public interface IBannerRepository
    {
        Task<IEnumerable<Banner>> GetAllAsync();
        Task<Banner?> GetByIdAsync(int id);
        Task AddAsync(Banner entity);
        Task UpdateAsync(Banner entity);
    }
}

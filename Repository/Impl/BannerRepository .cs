using VHSKCD.Models;
using Microsoft.EntityFrameworkCore;

namespace VHSKCD.Repository.Impl
{
    public class BannerRepository : IBannerRepository
    {
        private readonly B4zgrbg0p5agywu5uoneContext _context;

        public BannerRepository(B4zgrbg0p5agywu5uoneContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Banner entity)
        {
            _context.Banners.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Banner>> GetAllAsync()
        {
            return await _context.Banners.ToListAsync();
        }

        public async Task<Banner?> GetByIdAsync(int id)
        {
            return await _context.Banners.FindAsync(id);
        }

        public async Task UpdateAsync(Banner entity)
        {
            _context.Banners.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

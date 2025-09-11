using Microsoft.EntityFrameworkCore;
using VHSKCD.Models;

namespace VHSKCD.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly B4zgrbg0p5agywu5uoneContext _context;
        public UserRepository(B4zgrbg0p5agywu5uoneContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}

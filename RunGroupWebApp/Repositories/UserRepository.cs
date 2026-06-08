using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(AppUser user)
        {
            await _context.AddAsync(user);
            return await SaveAsync();
        }

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            var isDeleted = _context.SaveChanges();
            return isDeleted > 0 ? true : false;
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<AppUser>> GetUserListAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var isSaved = await _context.SaveChangesAsync();
            return isSaved > 0 ? true : false;
        }

        public async Task<bool> UpdateAsync(AppUser user)
        {
            _context.Update(user);
            return await SaveAsync();
        }
    }
}

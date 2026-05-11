using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _context;
        public ClubRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(Club club)
        {
            await _context.AddAsync(club);
            return await SaveAsync();
        }

        public bool DeleteAsync(Club club)
        {
            _context.Remove(club);
            var deleted = _context.SaveChanges();
            return deleted > 0 ? true : false;
        }

        public async Task<IEnumerable<Club>> GetAllAsync()
        {
            return await _context.Clubs.Include(c => c.Address).ToListAsync();
        }

        public async Task<IEnumerable<Club>> GetByCityAsync(string city)
        {
            return await _context.Clubs.Include(c => c.Address).Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _context.Clubs.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Club> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Clubs.Include(c => c.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            var isSaved = await _context.SaveChangesAsync();
            return isSaved > 0 ? true : false;
        }

        public async Task<bool> UpdateAsync(Club club)
        {
            await Task.Run(() => _context.Update(club));
            return await SaveAsync();
        }
    }
}

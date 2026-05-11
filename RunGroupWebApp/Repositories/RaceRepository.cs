using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;
        public RaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Race race)
        {
            await _context.AddAsync(race);
            return await SaveAsync();
        }

        public bool DeleteAsync(Race race)
        {
            _context.Remove(race);
            var deleted = _context.SaveChanges();
            return deleted > 0 ? true : false;
        }

        public async Task<IEnumerable<Race>> GetAllAsync()
        {
            return await _context.Races.Include(c => c.Address).ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetByCityAsync(string city)
        {
            return await _context.Races.Include(c => c.Address).Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await _context.Races.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Race> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Races.Include(c => c.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            var isSaved = await _context.SaveChangesAsync();
            return isSaved > 0 ? true : false;
        }

        public async Task<bool> UpdateAsync(Race race)
        {
            await Task.Run(() => _context.Update(race));
            return await SaveAsync();
        }
    }
}

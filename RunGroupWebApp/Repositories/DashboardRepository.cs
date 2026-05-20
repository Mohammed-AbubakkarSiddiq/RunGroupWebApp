using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccesssor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccesssor)
        {
            _context = context;
            _httpContextAccesssor = httpContextAccesssor;
        }
        public async Task<List<Club>> GetUserClubs()
        {
            var currentUser = _httpContextAccesssor.HttpContext?.User;
            var userClubs = await _context.Clubs.Where(c => c.AppUserId == currentUser.ToString()).ToListAsync();

            return userClubs;
        }

        public async Task<List<Race>> GetUserRaces()
        {
            var currentUser = _httpContextAccesssor.HttpContext?.User;
            var userRaces = await _context.Races.Where(r => r.AppUserId == currentUser.ToString()).ToListAsync();

            return userRaces;
        }
    }
}

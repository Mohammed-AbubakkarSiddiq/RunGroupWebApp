using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Club>> GetUserClubs();
        Task<List<Race>> GetUserRaces();
        Task<AppUser> GetUserByIdAsync(string id);
        Task<AppUser> GetUserByIdAsyncAsNoTracking(string id);
        Task<bool> UpdateUserAsync(AppUser user);
        Task<bool> SaveAsync();
    }
}

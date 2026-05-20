using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Club>> GetUserClubs();
        Task<List<Race>> GetUserRaces();
    }
}

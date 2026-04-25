using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllAsync();
        Task<Race> GetByIdAsync(int id);
        Task<IEnumerable<Race>> GetByCityAsync(string city);
        Task<bool> AddAsync(Race race);
        bool UpdateAsync(Race race);
        bool DeleteAsync(Race race);
        Task<bool> SaveAsync();
    }
}

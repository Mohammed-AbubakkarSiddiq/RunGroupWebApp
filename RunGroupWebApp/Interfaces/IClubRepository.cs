using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAllAsync();
        Task<Club> GetByIdAsync(int id);
        Task<IEnumerable<Club>> GetByCityAsync(string city);
        Task<bool> AddAsync(Club club);
        bool UpdateAsync(Club club);
        bool DeleteAsync(Club club);
        Task<bool> SaveAsync();
    }
}

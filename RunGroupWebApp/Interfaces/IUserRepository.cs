using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetUserListAsync();
        Task<AppUser> GetByIdAsync(string id);
        Task<bool> AddAsync(AppUser user);
        Task<bool> UpdateAsync(AppUser user);
        bool Delete(AppUser user);
        Task<bool> SaveAsync();
    }
}

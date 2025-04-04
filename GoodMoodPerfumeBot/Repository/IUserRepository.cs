using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Repository
{
    public interface IUserRepository
    {
        public Task<AppUser> CreateAsync(AppUser appUser);
        public AppUser GetAdmin();
        public AppUser GetOwner();
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<List<AppUser>> GetAllAsync();
        public Task<AppUser> UpdateAsync(AppUser appUser);
        public Task<AppUser> GetUserByTelegramIdAsync(long? telegramUserId);
        public Task Delete(AppUser appUser);
        public Task SaveAsync();

    }
}

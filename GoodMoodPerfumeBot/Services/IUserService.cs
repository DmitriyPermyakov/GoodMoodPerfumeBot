using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Services
{
    public interface IUserService
    {
        public Task<AppUser> CreateAsync(long? telegramUserId, long? chatId, string userRole);
        public Task<AppUser> GetUserByTelegramIdAsync(long? telegramUserId);
        public Task<AppUser> GetUserByIdAsync(int id);
        public AppUser GetAdmin();
        public AppUser GetOwner();


        public Task<AppUser> UpdateUserAsync(long userId, long? chatId, string userRole);
    }
}

using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repository;
using GoodMoodPerfumeBot.UserRoles;

namespace GoodMoodPerfumeBot.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }
        public async Task<AppUser> CreateAsync(long? telegramUserId, long? chatId = null, string userRole = SharedData.UserRoles.Member)
        {
            if (telegramUserId == null || telegramUserId < 0)
                throw new Exception("telegram user id not setted");

            AppUser appUser = await this.GetUserByTelegramIdAsync(telegramUserId);

            if (appUser != null)
                return appUser;
            else
            {
                appUser = new AppUser()
                {
                    ChatId = chatId,
                    TelegramUserId = telegramUserId,
                    UserRole = userRole
                };

                var createdUser = await this.repository.CreateAsync(appUser);
                return createdUser;
            }
        }

        public async Task<AppUser> GetUserByTelegramIdAsync(long? telegramUserId)
        {
            AppUser appUser = await this.repository.GetUserByTelegramIdAsync(telegramUserId);
            if (appUser == null)
                throw new Exception("User with this id not found");

            return appUser;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            if (id < 0)
                throw new Exception("wrong user id");

            var user = await this.repository.GetUserByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        public async Task<AppUser> UpdateUserAsync(int userId, long? chatId = null, string userRole = SharedData.UserRoles.Member)
        {
            AppUser userToUpdate = await this.GetUserByIdAsync(userId);

            if (!string.IsNullOrEmpty(userRole))
                userToUpdate.UserRole = userRole;

            if (chatId != null && chatId > 0)
                userToUpdate.ChatId = chatId;

            var updatedUser = await this.repository.UpdateAsync(userToUpdate);

            return updatedUser;
        }
    }
}

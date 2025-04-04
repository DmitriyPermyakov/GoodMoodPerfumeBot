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
                await this.repository.SaveAsync();
                return createdUser;
            }
        }

        public async Task<AppUser> GetUserByTelegramIdAsync(long? telegramUserId)
        {
            AppUser appUser = await this.repository.GetUserByTelegramIdAsync(telegramUserId);
            
            return appUser;
        }
        public AppUser GetOwner()
        {
            return this.repository.GetOwner();
        }


        public AppUser GetAdmin()
        {
            return this.repository.GetAdmin();
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

        public async Task<AppUser> UpdateUserAsync(long userId, long? chatId = null, string userRole = SharedData.UserRoles.Member)
        {
            AppUser user = await this.GetUserByTelegramIdAsync(userId);

            if(user == null)
            {
                user = await this.CreateAsync(userId, chatId, SharedData.UserRoles.Administrator);
            } else
            {
                if (!string.IsNullOrEmpty(userRole))
                    user.UserRole = userRole;

                if (chatId != null && chatId > 0)
                    user.ChatId = chatId;

                user = await this.repository.UpdateAsync(user);
            }

            
            await this.repository.SaveAsync();

            return user;
        }
    }
}

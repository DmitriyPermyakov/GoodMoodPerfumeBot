using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repositiory;
using GoodMoodPerfumeBot.UserRoles;
using Microsoft.EntityFrameworkCore;

namespace GoodMoodPerfumeBot.Repository
{
    public class AppUserRepository : IUserRepository
    {
        private readonly AppDatabaseContext context;
        public AppUserRepository(AppDatabaseContext context)
        {
            this.context = context;
        }
        public async Task<AppUser> CreateAsync(AppUser appUser)
        {
            var userEntity = await this.context.AppUsers.AddAsync(appUser);
            return userEntity.Entity;
        }

        public AppUser GetAdmin()
        {
            var admin = this.context.AppUsers
                .Where(u => u.UserRole.Equals(SharedData.UserRoles.Administrator))
                .FirstOrDefault();

            return admin;
        }

        public AppUser GetOwner()
        {
            var owner = this.context.AppUsers
                .Where(u => u.UserRole.Equals(SharedData.UserRoles.Owner))
                .FirstOrDefault();

            return owner;
        }


        public async Task Delete(AppUser appUser)
        {
            this.context.AppUsers.Remove(appUser);
            await Task.CompletedTask;
        }

        public async Task<List<AppUser>> GetAllAsync()
        {
            return await this.context.AppUsers.ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await this.context.AppUsers.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<AppUser> GetUserByTelegramIdAsync(long? telegramUserId)
        {
            return await this.context.AppUsers.FirstOrDefaultAsync(u => u.TelegramUserId == telegramUserId);
        }

        public async Task<AppUser> UpdateAsync(AppUser appUser)
        {
            var userEntity = this.context.AppUsers.Update(appUser);

            return await Task.FromResult(userEntity.Entity);
        }

        public async Task SaveAsync()
        {
            await this.context.SaveChangesAsync();
        }

    }
}

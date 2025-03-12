using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repositiory;
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
        public async Task<AppUser> Create(AppUser appUser)
        {
            var userEntity = await this.context.AppUsers.AddAsync(appUser);
            await this.context.SaveChangesAsync();
            return userEntity.Entity;
        }

        public async Task Delete(AppUser appUser)
        {
            this.context.AppUsers.Remove(appUser);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<AppUser>> GetAll()
        {
            return await this.context.AppUsers.ToListAsync();
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await this.context.AppUsers.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<AppUser> Update(AppUser appUser)
        {
            var userEntity = this.context.AppUsers.Update(appUser);
            await this.context.SaveChangesAsync();

            return userEntity.Entity;
        }
    }
}

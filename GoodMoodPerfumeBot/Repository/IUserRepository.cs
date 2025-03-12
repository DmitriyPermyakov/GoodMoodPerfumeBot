using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Repository
{
    public interface IUserRepository
    {
        public Task<AppUser> Create(AppUser appUser);
        public Task<AppUser> GetUserById(int id);
        public Task<List<AppUser>> GetAll();
        public Task<AppUser> Update(AppUser appUser);
        public Task Delete(AppUser appUser);

    }
}

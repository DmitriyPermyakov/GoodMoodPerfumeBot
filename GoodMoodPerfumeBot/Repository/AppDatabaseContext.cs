using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoodMoodPerfumeBot.Repositiory
{
    public class AppDatabaseContext : IdentityDbContext
    {
        public AppDatabaseContext(DbContextOptions options) : base(options) { }
    }
}

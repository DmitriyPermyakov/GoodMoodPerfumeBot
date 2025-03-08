using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repositiory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoodMoodPerfumeBot.Extensions
{
    public static class PostgreSQLServiceExtension
    {
        public static void AddPostgreSQLContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDatabaseContext>(opts =>
            {
                opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        //public static void AddPostgreSQLIdentityContext(this IServiceCollection services)
        //{
        //    services.AddIdentity<AppUser, IdentityRole>()
        //        .AddEntityFrameworkStores<AppDatabaseContext>();
        //}
    }
}

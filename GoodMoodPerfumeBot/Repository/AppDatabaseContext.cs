using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoodMoodPerfumeBot.Repositiory
{
    public class AppDatabaseContext : IdentityDbContext
    {
        public AppDatabaseContext(DbContextOptions options) : base(options) { }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {            
            base.OnModelCreating(builder);
            builder.Entity<Product>().HasData(FakeProductGenerator.GenerateProductList());
        }
    }
}

using GoodMoodPerfumeBot.Repositiory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ApplyMigrations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Applying migrations!");

            var webHost = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<ConsoleStartup>()
                .Build();

            using (var context = (AppDatabaseContext)webHost.Services.GetService(typeof(AppDatabaseContext)))
            {
                context?.Database.EnsureDeleted();
                context?.Database.Migrate();
            }

            Console.WriteLine("Migrations done");
        }
    }
}


using GoodMoodPerfumeBot.Configurations;
using GoodMoodPerfumeBot.Extensions;
using GoodMoodPerfumeBot.Repository;
using GoodMoodPerfumeBot.Services;
using Telegram.Bot;

namespace GoodMoodPerfumeBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var botConfiguration = builder.Configuration.GetSection("BotConfiguration");
            builder.Services.Configure<BotConfiguration>(botConfiguration);
            builder.Services.AddHttpClient("tgWebhook").RemoveAllLoggers().AddTypedClient<ITelegramBotClient>(
                client => new TelegramBotClient(botConfiguration.Get<BotConfiguration>()!.BotToken, client));

            builder.Services.AddPostgreSQLContext(builder.Configuration);
            builder.Services.AddTransient<IProductRepository, ProductRepository>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<IImageService, ImageService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IUserRepository, AppUserRepository>();
            builder.Services.AddTransient<IOrderRepository, OrderRepository>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IOrderItemService, OrderItemService>();
            builder.Services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddSingleton<UpdateHandler>();
            builder.Services.ConfigureTelegramBotMvc();
            builder.Services.AddControllers();


            builder.Services.AddCors(opts =>
            {
                opts.AddDefaultPolicy(p =>
                {
                    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            //builder.Services.AddPostgreSQLIdentityContext(); 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

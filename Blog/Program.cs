using Blog.DAL.Entities;

namespace Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //public static void Migrate(IServiceProvider serviceProvider)
        //{
        //    using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //    dbContext.Database.Migrate();
        //}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

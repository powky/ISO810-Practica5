using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Ferreteria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<Program>(); // Add this line to register the Program class as a singleton service
                });
    }
}

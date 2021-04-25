using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Web;

namespace FileSystemSearchService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()//Allows to be run as a Windows Service.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //Permits the website
                    webBuilder.UseStartup<Startup>();
                })
                .UseNLog();
    }
}

using AtlasEngine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace POCPicking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .UseExternalTaskWorkers()
                .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
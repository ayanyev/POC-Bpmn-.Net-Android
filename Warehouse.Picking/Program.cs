using AtlasEngine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Warehouse.Picking.Api.Utilities;

namespace Warehouse.Picking.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotEnv.Load("./.env.dev");
            CreateHostBuilder(args)
                .UseExternalTaskWorkers()
                .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
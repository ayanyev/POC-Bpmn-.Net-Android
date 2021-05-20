using AtlasEngine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Warehouse.Picking.Api.Data;
using Warehouse.Picking.Api.Utilities;

namespace Warehouse.Picking.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotEnv.Load("./.env.dev");
            var host = CreateHostBuilder(args)
                .UseExternalTaskWorkers()
                .Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
                db.Database.Migrate();
            }
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using warehouse.picking.api.Hubs;

namespace Warehouse.Picking.Api.Hubs.Extensions
{
    public static class HubsTasksExtensions
    {
        public static IServiceCollection AddHubServices(this IServiceCollection services, AppName appName)
        {
            services.AddSingleton(new ConnectionMapping());
            return services;
        }
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Picking.Api.Services.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IntakeService>();
            return services;
        }
    }
}
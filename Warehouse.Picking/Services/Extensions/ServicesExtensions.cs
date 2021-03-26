using System;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Picking.Api.Services.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, AppName appName)
        {
            switch (appName)
            {
                case AppName.PickingApp:
                    break;
                case AppName.IntakeApp:
                    services.AddSingleton<IntakeService>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(appName), appName, null);
            }

            return services;
        }
    }
}
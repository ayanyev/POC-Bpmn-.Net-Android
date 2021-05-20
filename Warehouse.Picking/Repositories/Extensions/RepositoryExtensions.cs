using Microsoft.Extensions.DependencyInjection;
using Warehouse.Picking.Api.Repositories.Articles;

namespace Warehouse.Picking.Api.Repositories.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IArticleRepository, FakeDbArticleRepository>();
            services.AddSingleton<ILocationRepository, FakeLocationRepository>();
            return services;
        }
    }
}
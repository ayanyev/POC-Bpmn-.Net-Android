using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Picking.Api.Repositories.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IPickerRepository, PickersRepository>();
            services.AddSingleton<ITaskRepository, TasksRepository>();
            services.AddSingleton<IArticleRepository, FakeArticleRepository>();
            return services;
        }
    }
}
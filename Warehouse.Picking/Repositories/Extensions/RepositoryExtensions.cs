using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Picking.Api.Repositories.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, AppName appName)
        {
            switch (appName)
            {
                case AppName.PickingApp:
                    services.AddSingleton<IPickerRepository, PickersRepository>();
                    services.AddSingleton<ITaskRepository, TasksRepository>();
                    break;
                case AppName.IntakeApp:
                    services.AddSingleton<IArticleRepository, FakeArticleRepository>();
                    services.AddSingleton<ILocationRepository, FakeLocationRepository>();
                    break;
            }
            return services;
        }
    }
}
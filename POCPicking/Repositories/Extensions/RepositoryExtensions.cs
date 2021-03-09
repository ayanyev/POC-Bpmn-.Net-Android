using Microsoft.Extensions.DependencyInjection;
using POCPicking.Processes.ExternalTasks;

namespace POCPicking.Repositories.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var repo = new TaskPickerRepository();
            services.AddSingleton<IPickerRepository>(repo);
            services.AddSingleton<ITaskRepository>(repo);
            return services;
        }
    }
}
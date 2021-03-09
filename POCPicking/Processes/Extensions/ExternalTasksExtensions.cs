using AtlasEngine;
using Microsoft.Extensions.DependencyInjection;
using POCPicking.Processes.ExternalTasks;

namespace POCPicking.Processes.Extensions
{
    [ExternalTaskHandler(topic: "GetOrCreateTaskForPicker")]
    public static class EndpointExternalTasksExtensions
    {
        public static IServiceCollection AddExternalTaskHandlers(this IServiceCollection services)
        {
            services.AddTransient<CreatePickerTaskExternalTask>();

            return services;
        }
    }
}
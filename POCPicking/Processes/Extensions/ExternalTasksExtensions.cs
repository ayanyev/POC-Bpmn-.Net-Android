using AtlasEngine;
using Microsoft.Extensions.DependencyInjection;
using POCPicking.Processes.ExternalTasks;
using POCPicking.Processes.Rest;

namespace POCPicking.Processes.Extensions
{
    [ExternalTaskHandler(topic: "GetOrCreateTaskForPicker")]
    public static class EndpointExternalTasksExtensions
    {
        public static IServiceCollection AddExternalTaskHandlers(this IServiceCollection services)
        {
            services.AddTransient<CreatePickerTaskExternalTask>();
            services.AddSingleton<IProcessClient>(new ProcessEngineClient());
            // services.AddSingleton<IProcessClient>(new ProcessesHttpClient());

            return services;
        }
    }
}
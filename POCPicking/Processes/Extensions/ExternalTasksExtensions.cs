using System;
using AtlasEngine;
using AtlasEngine.Logging;
using Microsoft.Extensions.DependencyInjection;
using POCPicking.Processes.ExternalTasks;

namespace POCPicking.Processes.Extensions
{
    public static class ExternalTasksExtensions
    {
        public static IServiceCollection AddProcessServices(this IServiceCollection services)
        {
            services.AddSingleton(ClientFactory.CreateExternalTaskClient(new Uri("http://localhost:56000"), logger: ConsoleLogger.Default));
            services.AddSingleton<AssignTaskHandler>();
            services.AddSingleton<PeekTaskHandler>();
            services.AddSingleton<IProcessClient, ProcessEngineClient>();
            // services.AddSingleton<IProcessClient, ProcessesHttpClient>();
            return services;
        }
    }
}
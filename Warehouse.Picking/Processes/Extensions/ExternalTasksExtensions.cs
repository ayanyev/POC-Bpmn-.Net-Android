using System;
using AtlasEngine;
using AtlasEngine.Logging;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Picking.Api.Processes;
using Warehouse.Picking.Api.Processes.ExternalTasks;

namespace warehouse.picking.api.Processes.Extensions
{
    public static class ExternalTasksExtensions
    {
        public static IServiceCollection AddProcessServices(this IServiceCollection services)
        {
            services.AddSingleton(ClientFactory.CreateExternalTaskClient(new Uri("http://localhost:56000"), logger: ConsoleLogger.Default));
            services.AddSingleton<AssignTaskHandler>();
            services.AddSingleton<ShiftStatusHandler>();
            services.AddSingleton<DequeueTaskHandler>();
            services.AddSingleton<IProcessClient, ProcessEngineClient>();
            // services.AddSingleton<IProcessClient, ProcessesHttpClient>();
            return services;
        }
    }
}
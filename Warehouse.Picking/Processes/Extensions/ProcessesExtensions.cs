using System;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Picking.Api;
using Warehouse.Picking.Api.Processes;
using Warehouse.Picking.Api.Processes.ExternalTasks;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake;
using Warehouse.Picking.Api.Processes.ExternalTasks.Picking;
using Warehouse.Picking.Api.Processes.UserTasks;

namespace warehouse.picking.api.Processes.Extensions
{
    public static class ExternalTasksExtensions
    {
        public static IServiceCollection AddProcessServices(this IServiceCollection services, AppName appName)
        {
            services.AddSingleton(ProcessEngineClient.CreateExternalTaskClient());
            services.AddSingleton<IProcessClient, ProcessEngineClient>();
            services.AddSingleton<ClientTaskFactory>();
            switch (appName)
            {
                case AppName.PickingApp:
                    services.AddSingleton<AssignTaskHandler>();
                    services.AddSingleton<ShiftStatusHandler>();
                    services.AddSingleton<DequeueTaskHandler>();
                    break;
                case AppName.IntakeApp:
                    services.AddSingleton<FetchArticlesForNoteHandler>();
                    services.AddSingleton<UnfinishedArticlesBarcodesHandler>();
                    services.AddSingleton<MatchArticleByGtinAndBundleHandler>();
                    services.AddSingleton<BookStockyardLocationHandler>();
                    services.AddSingleton<UpdateArticleHandler>();
                    services.AddSingleton<IUserTaskPayloadFactory, IntakeUserTaskPayloadFactory>();
                    services.AddSingleton<IProcessInfoProvider,ProcessInfoProvider>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(appName), appName, null);
            }
            return services;
        }
    }
}
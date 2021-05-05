using Microsoft.Extensions.DependencyInjection;
using Warehouse.Picking.Api.Processes;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake;
using Warehouse.Picking.Api.Processes.UserTasks;

namespace warehouse.picking.api.Processes.Extensions
{
    public static class ExternalTasksExtensions
    {
        public static IServiceCollection AddProcessServices(this IServiceCollection services)
        {
            services.AddSingleton(ProcessEngineClient.CreateExternalTaskClient());
            services.AddSingleton<IProcessClient, ProcessEngineClient>();
            services.AddSingleton<ClientTaskFactory>();
            services.AddSingleton<FetchArticlesForNoteHandler>();
            services.AddSingleton<UnfinishedArticlesBarcodesHandler>();
            services.AddSingleton<MatchArticleByGtinAndBundleHandler>();
            services.AddSingleton<BookStockyardLocationHandler>();
            services.AddSingleton<UpdateArticleHandler>();
            services.AddSingleton<IUserTaskPayloadFactory, IntakeUserTaskPayloadFactory>();
            services.AddSingleton<IProcessInfoProvider, ProcessInfoProvider>();
            return services;
        }
    }
}
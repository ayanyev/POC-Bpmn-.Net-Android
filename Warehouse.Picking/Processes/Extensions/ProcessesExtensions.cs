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
            
            // external task handlers
            // intake
            services.AddSingleton<FetchArticlesForNoteHandler>();
            services.AddSingleton<UnfinishedArticlesBarcodesHandler>();
            services.AddSingleton<MatchArticleByGtinAndBundleHandler>();
            services.AddSingleton<BookStockyardLocationHandler>();
            services.AddSingleton<UpdateArticleHandler>();
            
            // kind of identity provider
            services.AddSingleton<IProcessInfoProvider, ProcessInfoProvider>();

            // client task related
            services.AddSingleton<IProcessHandlersFactory, ProcessHandlersFactory>();
            services.AddTransient<ClientTaskFactory>();
            services.AddTransient<IntakeClientTaskPayloadHandler>();
            
            return services;
        }
    }
}
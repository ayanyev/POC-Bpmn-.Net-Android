using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using AtlasEngine;
using AtlasEngine.ExternalTasks;
using AtlasEngine.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Warehouse.Picking.Api.Processes;
using Warehouse.Picking.Api.Processes.ExternalTasks.Intake;
using Warehouse.Picking.Api.Processes.UserTasks;
using LogLevel = AtlasEngine.Logging.LogLevel;

namespace warehouse.picking.api.Processes.Extensions
{
    public static class ExternalTasksExtensions
    {
        public static IServiceCollection AddProcessServices(this IServiceCollection services, string url)
        {
            ConsoleLogger.Default.Log(LogLevel.Debug, url);
            services.AddSingleton(ClientFactory.CreateExternalTaskClient(new Uri(url), logger: ConsoleLogger.Default));
            services.AddSingleton<IProcessClient>(new ProcessEngineClient(url));
            
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
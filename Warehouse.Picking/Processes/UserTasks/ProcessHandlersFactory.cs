using System;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Picking.Api.Services;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public class ProcessHandlersFactory : IProcessHandlersFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessHandlersFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IClientTaskPayloadHandler GetPayloadHandler(string processModelId)
        {
            return processModelId switch
            {
                "WH.Intake" => _serviceProvider.GetService<IntakeClientTaskPayloadHandler>(),
                _ => null
            };
        }

        public ProcessReattachHandler GetReattachHandler(string processModelId)
        {
            return processModelId switch
            {
                "WH.Intake" => _serviceProvider.GetService<IntakeService>(),
                _ => null
            };
        }
    }
}
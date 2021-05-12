using System;
using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public class PayloadCreatorFactory : IPayloadCreatorFactory
    {
        
        private readonly IServiceProvider _serviceProvider;

        public PayloadCreatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IClientTaskPayloadCreator Get(string processModelId)
        {
            return processModelId switch
            {
                "WH.Intake" => _serviceProvider.GetService<IntakeClientTaskPayloadCreator>(),
                _ => throw new ArgumentException($"No payload creator corresponds to given process model ({processModelId})")
            };
        }
    }
}
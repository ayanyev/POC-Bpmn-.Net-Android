using Warehouse.Picking.Api.Processes.UserTasks;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public interface IProcessHandlersFactory
    {
        public IClientTaskPayloadHandler GetPayloadHandler(string processModelId);
        
        public ProcessReattachHandler GetReattachHandler(string processModelId);
    }
}
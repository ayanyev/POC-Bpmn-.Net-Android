using Warehouse.Picking.Api.Processes.UserTasks;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public interface IPayloadCreatorFactory
    {
        public IClientTaskPayloadCreator Get(string processModelId);
    }
}
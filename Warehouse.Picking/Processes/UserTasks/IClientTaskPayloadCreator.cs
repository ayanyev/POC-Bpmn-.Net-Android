using AtlasEngine.UserTasks;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public interface IClientTaskPayloadCreator
    {
        public SelectionOptions CreateSelectionOptionsPayload(UserTask task);

        public ScanPayload CreateScanPayload(UserTask task);
        
        public string CreateInfoPayload(UserTask task);
    }
}
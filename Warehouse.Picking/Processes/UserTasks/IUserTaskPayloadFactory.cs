using AtlasEngine.UserTasks;

namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public interface IUserTaskPayloadFactory
    {
        public SelectionOptions CreateSelectionOptionsPayload(UserTask task);

        public ScanPayload CreateScanPayload(UserTask task);
    }
}
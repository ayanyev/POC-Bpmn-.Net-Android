using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtlasEngine.ProcessDefinitions.Requests;
using AtlasEngine.UserTasks;

namespace Warehouse.Picking.Api.Processes
{
    public interface IProcessClient
    {
        Task<StartProcessInstanceResponse>
            CreateProcessInstanceByModelId<T>(string modelId, string startEvent, T token);

        Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string modelId, string startEvent, T token,
            string correlationId);

        Task<bool> TerminateProcessInstanceById(string processId);

        Task<bool> IsProcessInstanceRunning(string processId);

        public Task<List<UserTask>> GetAllUserTasks(string processId);

        public Task FinishUserTask(UserTask task, Dictionary<string, object> result);

        public Task FinishUserTask(string taskId, string correlationId, Dictionary<string, object> result);

        public void SubscribeForPendingUserTasks(string correlationId, Func<IEnumerable<UserTask>, UserTask> action);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AtlasEngine.ProcessDefinitions.Requests;
using AtlasEngine.ProcessInstances;
using AtlasEngine.UserTasks;

namespace Warehouse.Picking.Api.Processes
{
    public interface IProcessClient
    {
        Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string modelId, string startEvent, T token);

        Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string modelId, string startEvent, T token,
            string correlationId);

        Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string correlationId,
            ProcessInfo processInfo, T token);

        Task<bool> TerminateProcessInstanceById(string processId);

        Task<bool> TerminateProcessCorrelationId(string correlationId);

        Task<bool> IsProcessInstanceRunning(string processId);

        Task<List<UserTask>> GetAllUserTasks(string processId);

        Task FinishUserTask(UserTask task, Dictionary<string, object> result);

        Task FinishUserTask(string taskId, string correlationId, Dictionary<string, object> result);

        void SubscribeForPendingUserTasks(string correlationId, Func<IEnumerable<UserTask>, UserTask> action);

        Task<UserTask> GetPrevFinishedTaskOfSameKind(UserTask task);

        void SubscribeForProcessInstanceStateChange(string processId, Func<ProcessInstance, ProcessState> action);
    }
}
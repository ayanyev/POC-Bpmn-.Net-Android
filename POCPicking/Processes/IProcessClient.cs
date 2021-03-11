using System.Collections.Generic;
using System.Threading.Tasks;
using AtlasEngine.ProcessDefinitions.Requests;
using AtlasEngine.UserTasks;

namespace POCPicking.Processes
{
    public interface IProcessClient
    {
        Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string modelId, string startEvent, T token);

        Task<bool> TerminateProcessInstanceById(string processId);
        
        Task<bool> IsProcessInstanceRunning(string processId);

        public Task<List<UserTask>> GetAllUserTasks(string processId);

        public Task FinishUserTask(UserTask task, Dictionary<string, object> result);

    }
}
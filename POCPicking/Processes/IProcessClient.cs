using System.Threading.Tasks;
using AtlasEngine.ProcessDefinitions.Requests;

namespace POCPicking.Processes
{
    public interface IProcessClient
    {
        Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string modelId, string startEvent, T token);

        Task<bool> TerminateProcessInstanceById(string processId);
        
        Task<bool> IsProcessInstanceRunning(string processId);

    }
}
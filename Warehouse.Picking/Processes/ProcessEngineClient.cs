using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ProcessDefinitions;
using AtlasEngine.ProcessDefinitions.Requests;
using AtlasEngine.ProcessInstances;
using AtlasEngine.UserTasks;
using AtlasEngine.UserTasks.Requests;

namespace Warehouse.Picking.Api.Processes
{
    public class ProcessEngineClient : IProcessClient
    {
        private const string BaseUrl = "http://localhost:56000";
        
        private readonly IProcessDefinitionsClient _defClient = 
            ClientFactory.CreateProcessDefinitionsClient(new Uri(BaseUrl));
        
        private readonly IProcessInstancesClient _instanceClient = 
            ClientFactory.CreateProcessInstancesClient(new Uri(BaseUrl));
        
        private readonly IUserTaskClient _userTaskClient = 
            ClientFactory.CreateUserTaskClient(new Uri(BaseUrl));

        public async Task FinishUserTask(UserTask task, Dictionary<string, object> result)
        {
            await _userTaskClient.FinishUserTaskAsync(task, new UserTaskResult(result));
        }

        public async Task<List<UserTask>> GetAllUserTasks(string processId)
        {
            var tasks = await _userTaskClient.QueryAsync(
                q => q.FilterByProcessInstanceId(processId)
            );
            return tasks.ToList();
        }

        public async Task<bool> IsProcessInstanceRunning(string processId)
        {
            var res = (await _instanceClient.QueryAsync(
                options => options.FilterByProcessInstanceId(processId)
            )).ToList();
            return res.Count > 0 && res.First().State == ProcessState.Running;
        }

        public async Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string modelId, string startEvent, T token)
        {
            try
            {
                return await _defClient.StartProcessInstanceAsync(modelId, startEvent, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> TerminateProcessInstanceById(string processId)
        {
            try
            {
                await _instanceClient.TerminateProcessInstanceAsync(processId);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        
    }
}
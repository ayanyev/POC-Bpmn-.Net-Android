using System;
using System.Threading.Tasks;
using AtlasEngine;
using AtlasEngine.ProcessDefinitions;
using AtlasEngine.ProcessDefinitions.Requests;
using AtlasEngine.ProcessInstances;

namespace POCPicking.Processes
{
    public class ProcessEngineClient : IProcessClient
    {
        private const string BaseUrl = "http://localhost:56000";
        
        private readonly IProcessDefinitionsClient _defClient = 
            ClientFactory.CreateProcessDefinitionsClient(new Uri(BaseUrl));
        
        private readonly IProcessInstancesClient _instanceClient = 
            ClientFactory.CreateProcessInstancesClient(new Uri(BaseUrl));
        
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
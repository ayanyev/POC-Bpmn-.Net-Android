using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AtlasEngine.ProcessDefinitions.Requests;
using POCPicking.Processes.Rest.Models;

namespace POCPicking.Processes.Rest
{
    public class ProcessesHttpClient : IProcessClient
    {
        private readonly HttpClient _httpClient = new();
        


        private const string BaseUrl = "http://localhost:56000/atlas_engine/api/v1";

        private List<ProcessModel> _models = new();

        public ProcessesHttpClient()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "ZHVtbXlfdG9rZW4='");
        }

        private async Task GetAllProcessModels(string process)
        {
            var url = $"{BaseUrl}/process_definitions";
            var defs = (await _httpClient.GetFromJsonAsync<ProcessDefinitions>(url))
                ?.Definitions.Find(d => d.ProcessDefinitionId.Equals($"{process}_Definition"));
            _models = defs?.ProcessModels ?? _models;
        }
        
        [return: NotNull]
        public async Task<StartProcessInstanceResponse> CreateProcessInstanceByModelId<T>(string modelId, string startEvent, T token)
        {
            await GetAllProcessModels(modelId);
            
            var model = _models.Find(m => m.ProcessModelId.Equals(modelId));

            if (model == null) return default;

            try
            {
                var url = $"{BaseUrl}/process_models/{modelId}/start";

                var startPayload = new StartProcessPayload<T>(model, token);

                var result = await (await _httpClient.PostAsJsonAsync(url, startPayload))
                    .Content.ReadFromJsonAsync<StartProcessInstanceResponse>();

                if (result == null) throw new Exception("CreateProcessInstanceByModelId failed: result is null");

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("CreateProcessInstanceByModelId failed", e);
            }

        }

        public async Task<bool> TerminateProcessInstanceById(string processId)
        {
            var url = $"{BaseUrl}/process_instances/{processId}/terminate";
            try
            {
                var jsonContent = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, jsonContent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
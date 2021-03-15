using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Warehouse.Picking.Api.Processes.Rest.Models
{
    [Serializable]
    public class ProcessDefinitions
    {
        public int TotalCount { get; set; }
        [JsonPropertyName("processDefinitions")]
        public List<ProcessDefinition> Definitions { get; set; }
    }
    
    [Serializable]
    public class ProcessDefinition
    {
        public string ProcessDefinitionId { get; set; }
        public string Hash { get; set; }
        public List<ProcessModel> ProcessModels { get; set; }
    }
}
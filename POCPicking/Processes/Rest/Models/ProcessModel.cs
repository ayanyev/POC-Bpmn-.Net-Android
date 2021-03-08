using System;
using System.Collections.Generic;

namespace POCPicking.Processes.Rest.Models
{
    [Serializable]
    public class ProcessModel
    {
        public string ProcessModelId { get; set; }
        public string ProcessModelName { get; set; }
        public bool IsExecutable { get; set; }
        public List<string> StartEventIds { get; set; }
        public List<string> EndEventIds { get; set; }
    }
}
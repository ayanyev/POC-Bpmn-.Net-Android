using System;

namespace POCPicking.Processes.Rest.Models
{
    [Serializable]
    public class StartProcessPayload<T>
    {
        public int ReturnOn { get; set; }
        public string EndEventId { get; set; }
        public string ProcessModelId { get; set; }
        public string StartEventId { get; set; }
        public string CorrelationId { get; set; }
        public string ParentProcessInstanceId { get; set; }
        public T InitialToken { get; init; }

        public StartProcessPayload(ProcessModel model, T token)
        {
            ReturnOn = 1;
            ProcessModelId = model.ProcessModelId;
            StartEventId = model.StartEventIds[0];
            EndEventId = model.EndEventIds[0];
            CorrelationId = "";
            ParentProcessInstanceId = "";
            InitialToken = token;
        }
    }
}
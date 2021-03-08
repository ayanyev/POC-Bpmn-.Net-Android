using System;

namespace POCPicking.Processes.Rest.Models
{
    [Serializable]
    public class StartProcessResponse<T>
    {
        public string CorrelationId { get; set; }
        public string ProcessInstanceId { get; set; }
        public string EndEventId { get; set; }
        public T TokenPayload { get; set; }
    }
}
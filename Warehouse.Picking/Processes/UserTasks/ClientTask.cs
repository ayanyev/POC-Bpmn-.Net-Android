using System;
using System.Collections.Generic;
using AtlasEngine.UserTasks;

#nullable enable
namespace Warehouse.Picking.Api.Processes.UserTasks
{
    [Serializable]
    public class ClientTask
    {
        public string Id { get; }

        public string Type { get; }

        public string ResultKey { get; }
        
        public string Label { get; }

        public Dictionary<string, string> ResultTemplate { get; }

        public TaskError? Error { get; }

        public object? Payload { get; }

        public ClientTask(string id, string type, string key, string label, TaskError? error, object? payload, Dictionary<string, string> resultTemplate)
        {
            Id = id;
            Type = type;
            Label = label;
            Error = error;
            Payload = payload;
            ResultKey = key;
            ResultTemplate = resultTemplate;
        }
    }

    [Serializable]
    public class TaskError
    {
        public string? Code { get; }

        public string Name { get; }

        public string Message { get; }

        public TaskError(string code, string name, string message)
        {
            Code = code;
            Name = name;
            Message = message;
        }
    }
}
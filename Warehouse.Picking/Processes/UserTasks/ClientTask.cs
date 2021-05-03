using System;
using System.Collections.Generic;

#nullable enable
namespace Warehouse.Picking.Api.Processes.UserTasks
{
    public enum ClientTaskType
    {
        Info,
        Input,
        Scan,
        Selection,
        Quantity
    }
    
    [Serializable]
    public class ClientTask
    {
        public string Id { get; }

        public ClientTaskType Type { get; }

        public string ResultKey { get; }
        
        public string Label { get; }

        public Dictionary<string, object> ResultTemplate { get; }

        public TaskError? Error { get; }

        public object? Payload { get; }

        public ClientTask(string id,
            ClientTaskType type,
            string key,
            string label,
            TaskError error,
            object? payload,
            Dictionary<string, object> resultTemplate)
        {
            Id = id;
            Type = type;
            Label = label;
            Error = error.IsEmpty() switch
            {
                true => null,
                false => error
            };
            Payload = payload;
            ResultKey = key;
            ResultTemplate = resultTemplate;
        }
    }

    [Serializable]
    public class TaskError
    {
        public string? Code { get; }

        public string? Name { get; }

        public string? Message => AdditionalInformation?.DisplayableMessage;
        
        public AdditionalInformation? AdditionalInformation { get; }

        public TaskError(string code, string name, AdditionalInformation? additionalInformation)
        {
            Code = code;
            Name = name;
            AdditionalInformation = additionalInformation;
        }

        internal bool IsEmpty() => Code == null;

    }

    [Serializable]
    public class AdditionalInformation
    {
        public string DisplayableMessage { get; }
        
        public object Payload { get; }

        public AdditionalInformation(string displayableMessage, object payload)
        {
            DisplayableMessage = displayableMessage;
            Payload = payload;
        }
    }
}
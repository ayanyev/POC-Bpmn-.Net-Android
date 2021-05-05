using System;
using System.Linq;
using System.Security.Authentication;
using System.Text.Json;
using AtlasEngine.UserTasks;
using Microsoft.AspNetCore.SignalR;
using Warehouse.Picking.Api.Processes.UserTasks;

namespace Warehouse.Picking.Api.Utilities
{
    public static class Extensions
    {
        internal static string GetUserId(this HubCallerContext context)
        {
            if (context.User?.Identity?.Name == null)
                throw new AuthenticationException("No user auth data");

            return context.User.Identity.Name;
        }

        internal static bool HasErrorPayload(this UserTask task)
        {
            // if payload can be parsed to TaskError
            return task?.Tokens[0].Payload.GetPayload<TaskError>().Code != null;
        }

        internal static ClientTaskType GetClientTaskType(this UserTask task)
        {
            return Enum.Parse<ClientTaskType>(task.SplitUserTaskId()[1]);
        }

        internal static string GetResultKey(this UserTask task)
        {
            return task.SplitUserTaskId()[2];
        }
        
        internal static string GetQualifier(this UserTask task)
        {
            return task.SplitUserTaskId()[3];
        }

        private static string[] SplitUserTaskId(this UserTask task)
        {
            var a = task.Id.Split(".");
            if (a.Length < 4) throw new ArgumentException($"Wrong user task id: {task.Id}\nConsider pattern: UT.<Type>.<ResultKey>.<Qualifier>");
            return a;
        }

        internal static T GetPayload<T>(this UserTask task)
        {
            var payload = task.Tokens[0].Payload.RawPayload;
            var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
            return task.HasErrorPayload() switch
            {
                true => (T) JsonSerializer.Deserialize<TaskError>(payload, options)?.AdditionalInformation?.Payload,
                false => JsonSerializer.Deserialize<T>(payload, options)
            };
        }

        internal static T GetFormFieldValue<T>(this UserTask task, string fieldId)
        {
            var field = task.Configuration.FormFields.ToList().Find(
                f => f.Id.Equals(fieldId)
            );
            return field switch
            {
                null => throw new ArgumentException($"No form field ({fieldId}) found"),
                _ => (T) field.ParseValue()
            };
        }

        internal static object ParseValue(this FormField field)
        {
            return field.Type switch
            {
                FormFieldType.Boolean => bool.TryParse(field.DefaultValue, out var result) && result,
                FormFieldType.Number => int.TryParse(field.DefaultValue, out var result) ? result : 0,
                FormFieldType.Long => long.TryParse(field.DefaultValue, out var result) ? result : 0,
                _ => field.DefaultValue
            };
        }

    }
}
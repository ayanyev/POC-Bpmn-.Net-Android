using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text.Json;
using AtlasEngine.UserTasks;
using Microsoft.AspNetCore.SignalR;
using Warehouse.Picking.Api.Processes.UserTasks;

namespace Warehouse.Picking.Api.Utilities
{
    public static class Extensions
    {
        public static string GetUserId(this HubCallerContext context)
        {
            if (context.User?.Identity?.Name == null)
                throw new AuthenticationException("No user auth data");

            return context.User.Identity.Name;
        }

        public static bool HasErrorPayload(this UserTask task)
        {
            // if payload can be parsed to TaskError
            return task?.Tokens[0].Payload.GetPayload<TaskError>().Code != null;
        }

        public static T GetPayload<T>(this UserTask task)
        {
            var payload = task.Tokens[0].Payload.RawPayload;
            var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
            return task.HasErrorPayload() switch
            {
                true => (T) JsonSerializer.Deserialize<TaskError>(payload, options)?.AdditionalInformation?.Payload,
                false => JsonSerializer.Deserialize<T>(payload, options)
            };
        }
    }
}
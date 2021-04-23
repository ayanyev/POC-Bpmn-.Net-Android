using System.Security.Authentication;
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

    }
}
using System.Security.Authentication;
using Microsoft.AspNetCore.SignalR;

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
    }
}
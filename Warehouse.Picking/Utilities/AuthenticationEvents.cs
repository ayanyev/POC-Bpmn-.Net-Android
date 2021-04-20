using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace Warehouse.Picking.Api.Utilities
{
    public class AuthenticationEvents : BasicAuthenticationEvents
    {
        public override Task ValidatePrincipalAsync(ValidatePrincipalContext context)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, context.UserName, context.Options.ClaimsIssuer)
            };
            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
            return Task.CompletedTask;
        }
    }
}
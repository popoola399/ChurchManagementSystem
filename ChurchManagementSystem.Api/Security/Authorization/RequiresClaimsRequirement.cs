using System;
using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Exceptions;
using ChurchManagementSystem.Core.Security;
using Microsoft.AspNetCore.Authorization;
using SimpleInjector;

namespace ChurchManagementSystem.API.Security.Authorization
{
    public class RequiresClaimsRequirement : IAuthorizationRequirement
    {
        public RequiresClaimsRequirement(string[] claims)
        {
            Claims = claims;
        }

        public string[] Claims { get; }
    }

    public class RequiresClaimHandler : AuthorizationHandler<RequiresClaimsRequirement>
    {
        private readonly IIdentityUserContext _identity;

        public RequiresClaimHandler(Container container)
        {
            _identity = container.GetInstance<IIdentityUserContext>();
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequiresClaimsRequirement requirement)
        {
            try
            {
                var user = _identity.RequestingUser;
                if (user != null && requirement.Claims.All(x => user.Claims.Any(y => y.Name.Equals(x, StringComparison.OrdinalIgnoreCase))))
                    context.Succeed(requirement);
            }
            catch (AuthorizationFailedException)
            {
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}

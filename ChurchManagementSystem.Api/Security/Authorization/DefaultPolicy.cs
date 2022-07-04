using Microsoft.AspNetCore.Authorization;

namespace ChurchManagementSystem.API.Security.Authorization
{
    public static class DefaultPolicy
    {
        public static AuthorizationPolicy Build() => new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    }
}

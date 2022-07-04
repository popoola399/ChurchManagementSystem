using System;
using System.Linq;
using System.Threading.Tasks;

using ChurchManagementSystem.API.Security.Authorization;

using Microsoft.AspNetCore.Authorization;

namespace ChurchManagementSystem.API.Security.Authorization
{
    public class RequiresClaimsPolicyProvider : IAuthorizationPolicyProvider
    {
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => Task.FromResult(DefaultPolicy.Build());

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(RequiresClaimsAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var claimNames = policyName.Substring(RequiresClaimsAttribute.PolicyPrefix.Length).Trim();
                var claims = claimNames
                    .Split(RequiresClaimsAttribute.Delimiter)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();

                if (claims.Any())
                {
                    return Task.FromResult(new AuthorizationPolicyBuilder()
                        .AddRequirements(new RequiresClaimsRequirement(claims))
                        .Build());
                }
            }

            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}
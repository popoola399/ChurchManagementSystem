using Microsoft.AspNetCore.Authorization;

namespace ChurchManagementSystem.API.Security.Authorization
{
    public class RequiresClaimsAttribute : AuthorizeAttribute
    {
        public const string PolicyPrefix = "RequiresClaims";
        public const string Delimiter = "|";

        public RequiresClaimsAttribute(params string[] claims)
        {
            Claims = string.Join(Delimiter, claims);
        }

        public string Claims
        {
            get => Policy.Substring(PolicyPrefix.Length).Trim();
            set => Policy = $"{PolicyPrefix}{value}";
        }
    }
}

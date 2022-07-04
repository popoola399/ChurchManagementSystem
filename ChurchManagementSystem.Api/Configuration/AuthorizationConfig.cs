using ChurchManagementSystem.API.Configuration.Requirements;
using ChurchManagementSystem.API.Security;
using ChurchManagementSystem.API.Security.Authorization;
using ChurchManagementSystem.Core.Security;

using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChurchManagementSystem.API.Configuration
{
    public static class AuthorizationConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new PasswordPolicy
            {
                MinimumLength = int.Parse(configuration["PasswordPolicy:MinimumLength"]),
                MaximumLength = int.Parse(configuration["PasswordPolicy:MaximumLength"]),
                SymbolsRequired = bool.Parse(configuration["PasswordPolicy:Symbol"]),
                NumericCharactersRequired = bool.Parse(configuration["PasswordPolicy:Numeric"]),
                LowercaseCharactersRequired = bool.Parse(configuration["PasswordPolicy:Lowercase"]),
                UppercaseCharactersRequired = bool.Parse(configuration["PasswordPolicy:Uppercase"])
            });

            services.AddTransient<IAuthorizationHandler, ApiKeyRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, RequiresClaimHandler>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IPasswordService, PasswordService>();
         

            services.AddAuthorization();    

            services.AddSingleton<IAuthorizationPolicyProvider, RequiresClaimsPolicyProvider>();
        }
    }
}
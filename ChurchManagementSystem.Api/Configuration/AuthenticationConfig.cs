using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ChurchManagementSystem.API.Configuration
{
    public static class AuthenticationConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = config["Identity:Authority"];
                    options.Audience = IdentityServerConfig.ApiResourceName;
                    options.RequireHttpsMetadata = config.GetValue<bool>("Identity:RequireHttps");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireSignedTokens = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateTokenReplay = false
                    };
                });
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
        }
    }
}
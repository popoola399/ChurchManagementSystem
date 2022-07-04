using ChurchManagementSystem.Api.Filters;
using ChurchManagementSystem.API.Filters;
using IdentityServer4.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.Swagger;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ChurchManagementSystem.API.Configuration
{
    public static class SwaggerConfig
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            var scope = config
                    .GetSection("Identity:Scopes")
                    .Get<IEnumerable<string>>()
                    .First();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "CHURCHMANAGEMENTSYSTEM API",
                    Version = "v1",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "ChurchManagementSystem"
                    },
                    Description = "<h2>Description</h2><p>This is the API for the ChurchManagementSystem platform.</p><br /><h3>Usage</h3><ul><li><strong>Authorization (header)</strong><ul><li><p>The ChurchManagementSystem API uses OAuth2 to authorize requests.</p><p>The API will issue access tokens in the form of JWTs, which must be sent in the Authorization header as a \"Bearer token\".</p><p>Your credentials should be kept secure! Do not share them in publicly accessible areas such as GitHub, client-side code, etc.</p></li></ul></li></ul>"
                });

                options.IncludeXmlComments(
                    $@"{AppDomain.CurrentDomain.BaseDirectory}/Swagger.XML");

                //options.IncludeXmlComments(
                //    $@"{AppDomain.CurrentDomain.BaseDirectory}\SwaggerCore.XML");

                options.OperationFilter<FileUploadFilter>();
                options.OperationFilter<ParameterHeaderFilter>();
                //options.OperationFilter<HideRouteParams>();
               // options.DescribeAllEnumsAsStrings();

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{config["Identity:Authority"]}connect/authorize"),
                            TokenUrl = new Uri($"{config["Identity:Authority"]}connect/token")
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new[] { IdentityServerConfig.ApiResourceName }
                    }
                });
            });
        }
                                                                                             
        public static void Configure(IApplicationBuilder app, IConfiguration config)
        {
            var clientId = config
                .GetSection("Identity:Clients")
                .Get<IEnumerable<Client>>()
                .Select(x => x.ClientId)
                .First();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.DocExpansion(DocExpansion.None);
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ChurchManagementSystem API v1");

                options.OAuthClientId(clientId);
                options.OAuthClientSecret(config["Identity:ClientSecret"]);
            });
        }
    }
}
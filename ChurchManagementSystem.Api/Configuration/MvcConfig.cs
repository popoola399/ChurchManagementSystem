using ChurchManagementSystem.Api.Filters;
using ChurchManagementSystem.API.Filters;
using ChurchManagementSystem.API.Security.Authorization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ChurchManagementSystem.API.Configuration
{
    public static class MvcConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(DefaultPolicy.Build()));
                options.Filters.Add(typeof(AuthorizationExceptionFilter));
                options.Filters.Add(typeof(JsonExceptionFilter));
                options.EnableEndpointRouting = false;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            });
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
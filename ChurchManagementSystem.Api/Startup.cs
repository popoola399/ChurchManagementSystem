using ChurchManagementSystem.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SimpleInjector;
using Microsoft.IdentityModel.Logging;
using ChurchManagementSystem.Core.Configuration;

namespace ChurchManagementSystem.API
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IConfiguration _configuration;

        private static Container _container;

        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            _hostingEnvironment = env;
            _configuration = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            SimpleInjectorConfig.ConfigureServices(services, _configuration);
           // CorsConfig.ConfigureServices(services);
            AuthorizationConfig.ConfigureServices(services, _configuration);
            AuthenticationConfig.ConfigureServices(services, _configuration);
            DatabaseConfig.ConfigureServices(services, _configuration);
            IdentityServerConfig.ConfigureServices(services, _configuration);
              MvcConfig.ConfigureServices(services);
          //  AwsConfig.ConfigureServices(services, _configuration);
            SwaggerConfig.ConfigureServices(services, _configuration);
            //HangfireConfig.ConfigureServices(services, _configuration);
            //_container = HangfireConfig.ConfigureServices(services, _configuration);
            IdentityModelEventSource.ShowPII = true;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AutoMapperConfig.Configure();
            SimpleInjectorConfig.Configure(app, _configuration);
            RewriteConfig.Configure(app);
            CorsConfig.Configure(app, _configuration);
            AuthenticationConfig.Configure(app);
            MvcConfig.Configure(app);
           //AwsConfig.Configure(app, _configuration);
            SwaggerConfig.Configure(app, _configuration);
          //FluentValidationConfig.Configure();
            IdentityServerConfig.Configure(app);
            DatabaseConfig.Configure(app);
            //HangfireConfig.Configure(app, _configuration, env);
          //  app.UseExceptionless(_configuration["CommonSettings:Exceptionless:Key"]);
            //DefaultData.Initialize(app);
        }
    }
}
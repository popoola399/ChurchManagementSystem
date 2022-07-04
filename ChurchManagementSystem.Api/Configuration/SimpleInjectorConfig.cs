using ChurchManagementSystem.API.Controllers;
using ChurchManagementSystem.Core.Configuration;
using ChurchManagementSystem.Core.Logging;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;

namespace ChurchManagementSystem.API.Configuration
{
    public static class SimpleInjectorConfig
    {
        private static Container _container;

        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            _container = new Container();

            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            _container.ConfigureCore(services, config);

            _container.Register<HttpIdentityUserContext>(Lifestyle.Scoped);
            _container.Register<IIdentityUserContext>(() =>
            {
                if (_container.IsVerifying)
                {
                    return new NullIdentityUserContext();
                }
                return new DeferredHttpIdentityUserContext(
                    new Lazy<HttpIdentityUserContext>(_container.GetInstance<HttpIdentityUserContext>));
            }, Lifestyle.Scoped);

            _container.RegisterInitializer<BaseApiController>(controller =>
            {
                controller.Mediator = _container.GetInstance<IMediator>();
            });
            _container.Register<IMemoryCache>(() => new MemoryCache(new MemoryCacheOptions()),
                Lifestyle.Singleton);

            services.AddSingleton(_container);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.UseSimpleInjectorAspNetRequestScoping(_container);

            services.AddTransient(provider => _container.GetInstance<ILogger>());
        }

        public static void Configure(IApplicationBuilder app, IConfiguration config)
        {
            app.UseSimpleInjector(_container);
            _container.RegisterMvcControllers(app);
             _container.Verify();
        }
    }
}                                                      

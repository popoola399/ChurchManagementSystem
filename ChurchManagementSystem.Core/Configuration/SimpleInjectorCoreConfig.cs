using System.Collections.Generic;
using System.Net;
using System.Reflection;

using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Logging;
using ChurchManagementSystem.Core.Mediator.Configuration;
using ChurchManagementSystem.Core.Security;


using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SimpleInjector;

namespace ChurchManagementSystem.Core.Configuration
{
    public static class SimpleInjectorCoreConfig
    {
        public static void ConfigureCore(this Container container,
            IServiceCollection services,
            IConfiguration config,
            params Assembly[] additionalAssemblies)
        {
            var assemblyList = new List<Assembly>(1 + additionalAssemblies.Length)
            {
                typeof(SimpleInjectorCoreConfig).GetTypeInfo().Assembly
            };

            assemblyList.AddRange(additionalAssemblies);

            var configAssemblies = assemblyList.ToArray();

            container.Register(() => config);

            //container.Register<IEmailNotifier, EmailNotifier>();
            //container.Register<ITransactionEmailProcessor, TransactionEmailProcessor>();
            //container.Register<IAuditService, AuditService>();
            //container.Collection.Register<IAccess>(typeof(FacebookAccess), typeof(GoogleAccess));

            var settings = ConfigureCommonSettings(config);
            container.RegisterInstance(settings);

            ConfigureDatabase(container, config);
            ConfigureLogging(container);
            ConfigureMediator(container, configAssemblies);
            ConfigureSimpleInjector(container, services);
           // ConfigureEmail(container, config);
        }

        private static void ConfigureSimpleInjector(Container container, IServiceCollection services)
        {
            services.AddLogging();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore();
                options.CrossWire<IHttpContextAccessor>();
                options.CrossWire<IPasswordService>();
                options.CrossWire<PasswordPolicy>();
                options.AddLogging();
                options.AddLocalization();
            });
        }

        private static void ConfigureLogging(Container container)
        {
            container.Register<ILogger, CompositeLogger>(Lifestyle.Singleton);
            container.Collection.Register<ILogger>(typeof(ExceptionlessLogger));
        }

        private static CommonSettings ConfigureCommonSettings(IConfiguration config)
        {
            var settings = new CommonSettings();

            settings.BaseURL = config["CommonSettings:BaseUrl"];
           
            settings.Exceptionless.UseExceptionless = config.GetValue<bool>("CommonSettings:Exceptionless:Use");

            settings.ConnectionString = config.GetConnectionString("DataContext");
            settings.Environment = config["CommonSettings:Environment"];
           
            return settings;
        }

        private static void ConfigureDatabase(Container container, IConfiguration config)
        {
            container.Register(() => new DataContext(
                new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(config["ConnectionStrings:DataContext"])
                    .Options
            ), Lifestyle.Scoped);
        }
        
        private static void ConfigureMediator(Container container, Assembly[] configAssemblies)
        {
            container.ConfigureMediator(configAssemblies);

           //container.Register<IBackgroundJobMediator, BackgroundJobMediator>();
           //container.Register(() => new BackgroundJobContext(false));

            container.Register(typeof(IValidator<>), configAssemblies);
        }

        //private static void ConfigureEmail(Container container, IConfiguration configuration)
        //{
        //    container.Register<IEmailConfiguration>(() => new EmailConfiguration
        //    {
        //        Credentials = new NetworkCredential
        //        (
        //            configuration["Smtp:Username"],
        //            configuration["Smtp:Password"]
        //        ),
        //        Host = configuration["Smtp:Host"],
        //        Port = configuration.GetValue("Smtp:Port", 25),
        //        UseSsl = configuration.GetValue("Smtp:UseSsl", false),
        //        DefaultFromAddress = configuration["Smtp:DefaultFromAddress"]
        //    }, Lifestyle.Scoped);

        //    container.Register<IEmailProvider, MailKitEmailProvider>(Lifestyle.Scoped);
        //}
    }
}
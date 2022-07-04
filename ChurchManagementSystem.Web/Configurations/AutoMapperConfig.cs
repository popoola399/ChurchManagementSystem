using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoMapper;

//using ChurchManagementSystem.Web.Services.Dashboard;

namespace ChurchManagementSystem.Web.Configurations
{
    public static class AutoMapperConfig
    {
        public static void Configure(params Assembly[] additionalAssemblies)
        {
            Mapper.Initialize(config =>
            {
                config.AddProfiles(typeof(AutoMapperConfig).Assembly);
                //config.AddProfiles(typeof(DashboardAutoMapperConfiguration).Assembly);
                config.AddProfiles(additionalAssemblies);
                config.MapStringEnums(additionalAssemblies);
                config.ValidateInlineMaps = false;
            });
        }

        private static void MapStringEnums(this IMapperConfigurationExpression config, params Assembly[] assemblies)
        {
            var enumTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsEnum);

            var mapMethod = typeof(AutoMapperConfig)
                .GetMethod("MapStringEnum", BindingFlags.NonPublic | BindingFlags.Static);

            enumTypes.ForEach(x => mapMethod.MakeGenericMethod(x).Invoke(null, new object[] { config }));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
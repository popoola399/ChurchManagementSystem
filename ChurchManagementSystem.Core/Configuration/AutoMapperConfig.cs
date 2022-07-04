using ChurchManagementSystem.Core.Extensions;
using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace ChurchManagementSystem.Core.Configuration
{
    public static class AutoMapperConfig
    {
        private static void MapStringEnum<TEnum>(IProfileExpression config)
            where TEnum : struct
        {
            config.CreateMap<string, TEnum>().ConvertUsing(x => Enum.Parse<TEnum>(x, true));
        }

        private static void MapStringEnums(this IMapperConfigurationExpression config, params Assembly[] assemblies)
        {
            var enumTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsEnum);

            var mapMethod = typeof(AutoMapperConfig)
                .GetMethod("MapStringEnum", BindingFlags.NonPublic | BindingFlags.Static);

           // enumTypes.ForEach(x => mapMethod.MakeGenericMethod(x).Invoke(null, new object[] { config }));
        }

        public static void Configure(params Assembly[] additionalAssemblies)
        {
            Mapper.Initialize(config =>
            {
                config.AddProfiles(typeof(AutoMapperConfig).Assembly);
                config.AddProfiles(additionalAssemblies);
                config.MapStringEnums(additionalAssemblies);
            });
        }
    }
}
using System;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;

namespace ChurchManagementSystem.Core.Extensions
{
    public static class MapperExtensions
    {
        public static TTo MapTo<TTo>(this object from) => Mapper.Map<TTo>(from);

        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> profile,
            Expression<Func<TDestination, TMember>> member)
        {
            return profile.ForMember(member, options => options.Ignore());
        }

        public static IMappingExpression<TSource, TDestination> UseDefaultFor<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> profile,
            Expression<Func<TDestination, TMember>> member)
        {
            return profile.ForMember(member, options =>
            {
                options.AllowNull();
                options.MapFrom(src => default(TMember));
            });
        }

        public static IMappingExpression<TSource, TDestination> MapMember<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> profile,
            Expression<Func<TDestination, TMember>> member)
        {
            var memberExpr = member.Body as MemberExpression
                ?? throw new ArgumentException($"Expression '{member}' refers to a method, not a property.");

            var propInfo = memberExpr.Member as PropertyInfo
                ?? throw new ArgumentException($"Expression '{member}' refers to a field, not a property.");

            return profile.ForMember(member, options => options.MapFrom(propInfo.Name));
        }

        public static void IgnoreRest<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> profile)
        {
            profile.ForAllOtherMembers(options => options.Ignore());
        }
    }
}
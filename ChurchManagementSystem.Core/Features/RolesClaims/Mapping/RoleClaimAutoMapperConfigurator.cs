using System;
using System.Linq;
using ChurchManagementSystem.Core.Domain.Authorization;
using ChurchManagementSystem.Core.Extensions;
using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using AutoMapper;

namespace ChurchManagementSystem.Core.Features.RolesClaims.Mapping
{
    public class RoleClaimAutoMapperConfigurator : Profile
    {
        public RoleClaimAutoMapperConfigurator()
        {
            CreateMap<Claim, ClaimDto>();

            CreateMap<Role, GetRoleDto>()
                .ForMember(x => x.Claims, options => 
                    options.MapFrom(x => x.RoleClaims.Where(y => y.Active).Select(y => y.Claim)))
                .Ignore(x => x.IsAssignable);

            CreateMap<AddRoleRequest, Role>()
                .ForMember(x => x.RoleId, options => options.Ignore())
                .ForMember(x => x.RoleClaims, options => options.Ignore())
                .ForMember(x => x.Users, options => options.Ignore())
                .ForMember(x => x.CreatedDate, options => options.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.ModifiedDate, options => options.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.RoleType, options => options.Ignore());

            CreateMap<EditRoleRequest, Role>()
               .ForMember(x => x.RoleClaims, options => options.Ignore())
               .ForMember(x => x.Users, options => options.Ignore())
               .ForMember(x => x.CreatedDate, options => options.Ignore())
               .ForMember(x => x.ModifiedDate, options => options.MapFrom(s => DateTime.UtcNow))
               .ForMember(x => x.RoleType, options => options.Ignore());
        }
    }
}
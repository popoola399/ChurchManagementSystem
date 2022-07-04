using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain.Authorization;
using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;
using ChurchManagementSystem.Core.Mediator.Decorators;
using ChurchManagementSystem.Core.Security;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.RolesClaims
{
    [DataContext]
    [DoNotValidate]
    public class GetAllRolesRequest : IRequest<IEnumerable<GetRoleDto>>
    {
    }

    public class GetAllRolesHandler : IRequestHandler<GetAllRolesRequest, IEnumerable<GetRoleDto>>
    {
        private readonly DataContext _context;
        private readonly IIdentityUserContext _identity;

        public GetAllRolesHandler(DataContext context, IIdentityUserContext identity)
        {
            _context = context;
            _identity = identity;
        }

        public async Task<Response<IEnumerable<GetRoleDto>>> HandleAsync(GetAllRolesRequest request)
        {
            var currentUserClaims = _identity.RequestingUser.Claims;

            var roles = await _context.Set<Role>()
                .AsNoTracking()
                .Include(x => x.RoleClaims)
                .ThenInclude(x => x.Claim)
                .ProjectTo<GetRoleDto>()
                .ToListAsync();

            roles.ForEach(role => role.IsAssignable = role.Claims.All(roleClaim => currentUserClaims.Any(userClaim => userClaim.ClaimId == roleClaim.ClaimId)));

            return roles.AsEnumerable().AsResponse();
        }
    }
}
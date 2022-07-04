using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain.Authorization;
using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using ChurchManagementSystem.Core.Features.RolesClaims.Utility;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Decorators;
using ChurchManagementSystem.Core.Security;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.RolesClaims
{
    [DataContext]
    public class GetRoleByRoleIdRequest : IRequest<GetRoleDto>
    {
        public GetRoleByRoleIdRequest(int roleId)
        {
            RoleId = roleId;
        }

        public int RoleId { get; private set; }
    }

    public class GetRoleByRoleIdHandler : IRequestHandler<GetRoleByRoleIdRequest, GetRoleDto>
    {
        private readonly DataContext _context;
        private readonly IIdentityUserContext _identity;

        public GetRoleByRoleIdHandler(DataContext context, IIdentityUserContext identity)
        {
            _context = context;
            _identity = identity;
        }

        public async Task<Response<GetRoleDto>> HandleAsync(GetRoleByRoleIdRequest request)
        {
            var currentUserClaims = _identity.RequestingUser.Claims;

            var role = await _context.Set<Role>()
                .AsNoTracking()
                .Include(x => x.RoleClaims)
                .ThenInclude(x => x.Claim)
                .ProjectTo<GetRoleDto>()
                .SingleOrDefaultAsync(x => x.RoleId == request.RoleId);

            if (role == null)
                return Error.AsResponse<GetRoleDto>(RoleConstants.ErrorMessages.RoleNotFoundWithID);

            role.IsAssignable = role.Claims.All(
                roleClaim => currentUserClaims.Any(userClaim => roleClaim.ClaimId == userClaim.ClaimId));
            
            return role.AsResponse();
        }

        public class GetRoleByIdValidator : AbstractValidator<GetRoleByRoleIdRequest>
        {
            public GetRoleByIdValidator()
            {
                RuleFor(x => x.RoleId).GreaterThan(0);
            }
        }
    }
}
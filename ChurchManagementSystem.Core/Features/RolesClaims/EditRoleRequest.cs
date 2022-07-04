using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain.Authorization;
using ChurchManagementSystem.Core.Extensions;
using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using ChurchManagementSystem.Core.Features.RolesClaims.Utility;
using ChurchManagementSystem.Core.Features.RolesClaims.Validators;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Decorators;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Features.RolesClaims
{
    [DataContext]
    public class EditRoleRequest : EditRoleDto, IRequest
    {
    }

    public class EditRoleHandler : IRequestHandler<EditRoleRequest>
    {
        private readonly DataContext _context;

        public EditRoleHandler(DataContext context)
        {
            _context = context;
        }

        public Task<Response> HandleAsync(EditRoleRequest request)
        {
            var claimIds = _context.Set<Claim>().Select(claim => claim.ClaimId).ToList();
            foreach (var claimId in request.ClaimIds)
            {
                if (!claimIds.Contains(claimId))
                    return Response.ErrorAsync(RoleConstants.ErrorMessages.InvalidClaimsId);
            }

            var role = _context.Set<Role>().SingleOrDefault(x => x.RoleId == request.RoleId);
            if (role == null)
                return Response.ErrorAsync(RoleConstants.ErrorMessages.RoleNotFoundWithID);

            var roleNameExists = _context.Set<Role>().Any(x => x.Name == request.Name && request.Name != role.Name);
            if (roleNameExists)
                return Response.ErrorAsync(RoleConstants.ErrorMessages.RoleExists);

            role = Mapper.Map<EditRoleRequest, Role>(request, role);

            _context.Set<Role>().Attach(role);
            _context.Entry(role).State = EntityState.Modified;
            _context.SaveChanges();
            _context.Entry(role).State = EntityState.Detached;

            var roleClaims = _context.RoleClaims.Where(x => x.RoleId == request.RoleId).ToList();
            foreach (var roleClaim in roleClaims)
            {
                roleClaim.Active = false;

                _context.Set<RoleClaim>().Attach(roleClaim);
                _context.Entry(roleClaim).State = EntityState.Modified;
            }

            _context.SaveChanges();

            foreach (int claimId in request.ClaimIds)
            {
                var roleClaim = _context.RoleClaims.Where(x => x.RoleId == request.RoleId && x.ClaimId == claimId).FirstOrDefault();

                if (roleClaim != null)
                {
                    roleClaim.Active = true;

                    _context.Set<RoleClaim>().Attach(roleClaim);
                    _context.Entry(roleClaim).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                else
                {
                    Role roleModel = new Role { RoleId = role.RoleId };
                    _context.Roles.Add(roleModel);
                    _context.Roles.Attach(roleModel);

                    Claim claimModel = new Claim { ClaimId = claimId };
                    _context.Claims.Add(claimModel);
                    _context.Claims.Attach(claimModel);

                    RoleClaim roleClaimModel = new RoleClaim
                    {
                        ClaimId = claimId,
                        RoleId = role.RoleId
                    };

                    roleModel.RoleClaims.Add(roleClaimModel);

                    _context.SaveChanges();
                    _context.Entry(roleModel).State = EntityState.Detached;
                    _context.Entry(claimModel).State = EntityState.Detached;
                    _context.Entry(roleClaimModel).State = EntityState.Detached;
                }
            }

            return Response.SuccessAsync();
        }
    }

    public class EditRoleValidator : AbstractValidator<EditRoleRequest>
    {
        public EditRoleValidator()
        {
            Include(new BaseRoleDtoValidator());
            RuleFor(x => x.RoleId).GreaterThan(0);
        }
    }
}
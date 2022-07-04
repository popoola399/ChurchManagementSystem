using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain.Authorization;
using ChurchManagementSystem.Core.Extensions;
using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using ChurchManagementSystem.Core.Features.RolesClaims.Utility;
using ChurchManagementSystem.Core.Features.RolesClaims.Validators;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Decorators;
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
    public class AddRoleRequest : AddRoleDto, IRequest<AddResponse>
    {
    }

    public class AddRoleHandler : IRequestHandler<AddRoleRequest, AddResponse>
    {
        private readonly DataContext _context;

        public AddRoleHandler(DataContext context)
        {
            _context = context;
        }

        public Task<Response<AddResponse>> HandleAsync(AddRoleRequest request)
        {
            var role = _context.Set<Role>()
               .SingleOrDefault(x => x.Name == request.Name);

            if (role != null)
            {
                return new AddResponse
                {
                    Id = role.RoleId,
                    Message = RoleConstants.ErrorMessages.RoleExists
                }.AsResponseAsync();
            }
            role = request.MapTo<Role>();
            role.RoleType = RoleConstants.ErrorMessages.RoleType;

            _context.Set<Role>().Add(role);
            _context.SaveChanges();
            _context.Entry(role).State = EntityState.Detached;

            foreach (int claimId in request.ClaimIds)
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

            return new AddResponse
            {
                Id = role.RoleId,
                Message = RoleConstants.ErrorMessages.RoleSavedSuccessful
            }.AsResponseAsync();
        }

        public class AddRoleValidator : AbstractValidator<AddRoleRequest>
        {
            public AddRoleValidator()
            {
                Include(new BaseRoleDtoValidator());
            }
        }
    }
}
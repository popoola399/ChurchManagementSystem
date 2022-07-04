using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using ChurchManagementSystem.Core.Features.RolesClaims.Utility;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchManagementSystem.Core.Features.RolesClaims.Validators
{
    public class BaseRoleDtoValidator : AbstractValidator<BaseRoleDto>
    {
        public BaseRoleDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(RoleConstants.ValidationConstants.MaxNameLength).NotNull().NotEmpty();
            RuleFor(x => x.Description).MaximumLength(RoleConstants.ValidationConstants.MaxDescriptionLength).NotNull().NotEmpty();
        }
    }
}
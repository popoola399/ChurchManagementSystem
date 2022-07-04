
using ChurchManagementSystem.Core.Features.Users.Dtos;

using FluentValidation;

using static ChurchManagementSystem.Core.Features.Users.Utility.UserConstants;

namespace ChurchManagementSystem.Core.Features.Users.Validators
{
    public class BaseUserDtoValidator : AbstractValidator<BaseUserDto>
    {
        public BaseUserDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(ValidationConstants.MaxFirstNameLength);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(ValidationConstants.MaxLastNameLength);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(ValidationConstants.MaxEmailLength).EmailAddress();
        }
    }
}
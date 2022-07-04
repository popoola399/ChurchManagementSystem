using ChurchManagementSystem.Core.Features.Access.Dtos;
using ChurchManagementSystem.Core.Features.Access.Utility;
using FluentValidation;

namespace ChurchManagementSystem.Core.Features.Access.Validators
{
    public class BaseAccessDtoValidator : AbstractValidator<BaseAccessDto>
    {
        public BaseAccessDtoValidator()
        {
            RuleFor(x => x.ClientId).MaximumLength(ValidationConstants.MaxClientIdLength).NotNull().NotEmpty();
            RuleFor(x => x.ClientSecret).MaximumLength(ValidationConstants.MaxClientSecretLength).NotNull().NotEmpty();
        }
    }
}
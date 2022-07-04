using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Features.UserProfile.Dtos;
using ChurchManagementSystem.Core.Features.UserProfile.Utility;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Decorators;
using ChurchManagementSystem.Core.Security;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.UserProfile
{
    [DataContext]
    public class UserResetPasswordRequest : UserResetPasswordDto, IRequest
    {
    }

    public class UserResetPasswordHandler : IRequestHandler<UserResetPasswordRequest>
    {
        private readonly DataContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IIdentityUserContext _identity;

        public UserResetPasswordHandler(DataContext context, IPasswordService passwordService, IIdentityUserContext identity)
        {
            _context = context;
            _passwordService = passwordService;
            _identity = identity;
        }

        public Task<Response> HandleAsync(UserResetPasswordRequest request)
        {
            var identityUser = _identity.RequestingUser;
            var user = _context.Set<User>().SingleOrDefault(x => x.Id == identityUser.Id);
            if (user == null)
                return Response.ErrorAsync(UserProfileConstants.ErrorMessages.UserNotFoundWithID);

            if (user.CannotChangePassword)
                return Response.ErrorAsync(UserProfileConstants.ErrorMessages.CanNotChangePassword);

            var newPassword = _passwordService.CreateHash(request.OldPassword, user.Salt);

            if (user.HashPassword != newPassword)
                return Response.ErrorAsync(UserProfileConstants.ErrorMessages.OldPasswordIncorrect);

            if (!user.NormalLoginEnabled)
            {
                user.NormalLoginEnabled = true;
            }

            user.Salt = _passwordService.CreateSalt();
            user.HashPassword = _passwordService.CreateHash(request.NewPassword, user.Salt);

            _context.Set<User>().Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();

            return Response.SuccessAsync();
        }
    }

    public class UserResetPasswordValidator : AbstractValidator<UserResetPasswordRequest>
    {
        public UserResetPasswordValidator(PasswordPolicy passwordPolicy)
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .MaximumLength(UserProfileConstants.ValidationConstants.OldPassword);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MaximumLength(UserProfileConstants.ValidationConstants.NewPassword)
                .Must(passwordPolicy.IsValid)
                .WithMessage(UserProfileConstants.ErrorMessages.InvalidPassword);
        }
    }
}
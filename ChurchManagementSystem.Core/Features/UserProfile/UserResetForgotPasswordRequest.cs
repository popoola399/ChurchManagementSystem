using System;
using System.Linq;
using System.Threading.Tasks;

using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Features.UserProfile.Dtos;
using ChurchManagementSystem.Core.Features.UserProfile.Utility;
using ChurchManagementSystem.Core.Features.Utilities;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Decorators;
using ChurchManagementSystem.Core.Security;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.UserProfile
{
    [DataContext]
    public class UserResetForgotPasswordRequest : UserResetForgotPasswordDto, IRequest
    {
    }

    public class UserResetForgotPasswordHandler : IRequestHandler<UserResetForgotPasswordRequest>
    {
        private readonly DataContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IRequestHandler<ValidateTokenRequest, bool> _validateTokenHandler;

        public UserResetForgotPasswordHandler(DataContext context, IPasswordService passwordService, IRequestHandler<ValidateTokenRequest, bool> validateTokenHandler)
        {
            _context = context;
            _passwordService = passwordService;
            _validateTokenHandler = validateTokenHandler;
        }

        public async Task<Response> HandleAsync(UserResetForgotPasswordRequest request)
        {
            var token = "";
            var userEmail = "";

            if (request.Token.Contains("xzyzyx"))
            {
                token = request.Token.Split("xzyzyx")[0];
                userEmail = request.Token.Split("xzyzyx")[1];
            }
            else
            {
                return Response.Error("Invalid token for password reset.");
            }

            var user = _context.Set<User>().SingleOrDefault(x => x.Email == userEmail);
            if (user == null)
            {
                return Response.Error("User not found.");
            }

            var generateTokenRequest = new ValidateTokenRequest(userEmail, token, UseCase.PasswordReset);
            var tokenIsValid = await _validateTokenHandler.HandleAsync(generateTokenRequest);
            if (tokenIsValid.HasErrors)
            {
                return Response.Error(tokenIsValid.Errors[0].ErrorMessage);
            }

            if (tokenIsValid.Data)
            {
                user.Salt = _passwordService.CreateSalt();
                user.HashPassword = _passwordService.CreateHash(request.NewPassword, user.Salt);
                user.NormalLoginEnabled = true;
                _context.Set<User>().Attach(user);
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                return Response.Error("Token not valid");
            }
            return Response.Success();
        }
    }

    public class UserResetForgotPasswordValidator : AbstractValidator<UserResetForgotPasswordRequest>
    {
        public UserResetForgotPasswordValidator(PasswordPolicy passwordPolicy)
        {
            RuleFor(x => x.Token)
                .NotEmpty().NotNull();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MaximumLength(UserProfileConstants.ValidationConstants.NewPassword)
                .Must(passwordPolicy.IsValid)
                .WithMessage(UserProfileConstants.ErrorMessages.InvalidPassword);
        }
    }
}
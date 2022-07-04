using System;
using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Configuration;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Features.Template.Utility;
using ChurchManagementSystem.Core.Features.UserProfile.Dtos;
using ChurchManagementSystem.Core.Features.UserProfile.Utility;
using ChurchManagementSystem.Core.Features.Utilities;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;
using ChurchManagementSystem.Core.Notifications.Email;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.UserProfile
{
    [DoNotValidate]
    public class UserForgotPasswordRequest : UserForgottenPasswordDto, IRequest
    {
    }

    public class ForgotPasswordRequestHandler : IRequestHandler<UserForgotPasswordRequest>
    {
        private readonly DataContext _context;
        private readonly CommonSettings _commonSettings;
        private readonly IRequestHandler<GenerateTokenRequest, string> _generateTokenHandler;

        public ForgotPasswordRequestHandler(DataContext context,
            CommonSettings commonSettings,
            IRequestHandler<GenerateTokenRequest, string> generateTokenHandler)
        {
            _context = context;
            _commonSettings = commonSettings;
            _generateTokenHandler = generateTokenHandler;
        }

        public async Task<Response> HandleAsync(UserForgotPasswordRequest request)
        {
            var user = _context.Set<User>()
                .Where(x => x.Email == request.RecoveryEmail)
                .FirstOrDefault();

            if (user == null)
            {
                return Response.Error(UserProfileConstants.ErrorMessages.NoEmail);
            }

            var generateTokenRequest = new GenerateTokenRequest(DateTime.Now, UseCase.PasswordReset, user.Email);
            var resetToken = _generateTokenHandler.HandleAsync(generateTokenRequest).Result.Data;

            var template = _context.Set<Domain.Template>()
              .Where(x => x.SpecialCode == TemplateConstants.PasswordReset)
              .FirstOrDefault();

            var mailBody = template.Body;

            mailBody = mailBody
                .Replace("{{NAME}}", user.FirstName)
                .Replace("{{URL}}", $"{_commonSettings.ForgotPasswordRedirect}{resetToken}xzyzyx{user.Email}")
                .Replace("{{Sender_Address}}", _commonSettings.ChurchManagementDetails.SenderAddress)
                .Replace("{{Sender_City}}", _commonSettings.ChurchManagementDetails.SenderCity)
                .Replace("{{Sender_State}}", _commonSettings.ChurchManagementDetails.SenderState)
                .Replace("{{Sender_Zip}}", _commonSettings.ChurchManagementDetails.SenderZip);

           // var emailImages = await _context.Set<Image>()
           //.Where(x => x.SpecialCode == ImageConstants.FeelsLogo || x.SpecialCode == ImageConstants.MakeItMeanSomething || x.SpecialCode == ImageConstants.PasswordResetMain || x.SpecialCode == ImageConstants.PasswordResetFacebook || x.SpecialCode == ImageConstants.PasswordResetTwitter || x.SpecialCode == ImageConstants.PasswordResetInstagram)
           //.ToListAsync();

           // foreach (var emailImage in emailImages)
           // {
           //     if (emailImage.SpecialCode == ImageConstants.FeelsLogo)
           //     {
           //         mailBody = mailBody.Replace("{{FEELSLOGO}}", emailImage.Url);
           //     }
           //     else if (emailImage.SpecialCode == ImageConstants.MakeItMeanSomething)
           //     {
           //         mailBody = mailBody.Replace("{{makeitmeansomething}}", emailImage.Url);
           //     }
           //     else if (emailImage.SpecialCode == ImageConstants.PasswordResetMain)
           //     {
           //         mailBody = mailBody.Replace("{{MAIN}}", emailImage.Url);
           //     }
           //     else if (emailImage.SpecialCode == ImageConstants.PasswordResetInstagram)
           //     {
           //         mailBody = mailBody.Replace("{{INSTAGRAM}}", emailImage.Url);
           //     }
           //     else if (emailImage.SpecialCode == ImageConstants.PasswordResetTwitter)
           //     {
           //         mailBody = mailBody.Replace("{{TWITTER}}", emailImage.Url);
           //     }
           //     else if (emailImage.SpecialCode == ImageConstants.PasswordResetFacebook)
           //     {
           //         mailBody = mailBody.Replace("{{FACEBOOK}}", emailImage.Url);
           //     }
           // }

            var message = new EmailMessage
            {
                ToAddress = new EmailInfo
                {
                    Name = $"{user.FirstName} {user.LastName}",
                    Address = user.Email
                },
                Body = mailBody,
                Subject = template.Subject,
                FromAddress = new EmailInfo()
                {
                    Name = template.FromName,
                    Address = template.FromEmail
                }
            };
           // await _emailNotifier.SendEmailAsync(message);

            return Response.Success();
        }
    }

    public class UserForgottPasswordValidator : AbstractValidator<UserForgotPasswordRequest>
    {
        public UserForgottPasswordValidator()
        {
            RuleFor(x => x.RecoveryEmail).EmailAddress().MaximumLength(UserProfileConstants.ValidationConstants.Email).NotNull().NotEmpty();
        }
    }
}
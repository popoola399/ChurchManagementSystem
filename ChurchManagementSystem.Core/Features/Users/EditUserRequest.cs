using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Domain.Authorization;
using ChurchManagementSystem.Core.Features.Users.Dtos;
using ChurchManagementSystem.Core.Features.Users.Utility;
using ChurchManagementSystem.Core.Features.Users.Validators;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Decorators;
using ChurchManagementSystem.Core.Security;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ChurchManagementSystem.Core.Configuration;
using System;

namespace ChurchManagementSystem.Core.Features.Users
{
    [DataContext]
    public class EditUserRequest : EditUserDto, IRequest
    {
    }

    public class EditUserHandler : IRequestHandler<EditUserRequest>
    {
        private readonly DataContext _context;
        private readonly IIdentityUserContext _identity;
        private readonly CommonSettings _commonSettings;
    
       

        public EditUserHandler(DataContext context, IIdentityUserContext identity, CommonSettings commonSettings)
        {
            _context = context;
            _identity = identity;
            _commonSettings = commonSettings;
          
        }

        public async Task<Response> HandleAsync(EditUserRequest request)
        {
            var identityUser = _identity.RequestingUser;

            var user = await _context.Set<User>()
                .Include(x => x.Role)
                .ThenInclude(x => x.RoleClaims)
                .SingleOrDefaultAsync(x => x.Id == identityUser.Id);

            if (user == null)
                return Error.AsResponse(UserConstants.ErrorMessages.UserNotFoundWithID);

            bool isEmailChange = user.Email != request.Email;

            var currentUserClaims = _identity.RequestingUser.Claims.Select(x => x.ClaimId).ToArray();

            var curRoleClaims = user.Role.RoleClaims.Select(x => x.ClaimId);

            if (!curRoleClaims.All(roleClaim => currentUserClaims.Any(userClaim => userClaim == roleClaim)))
                return Error.AsResponse(UserConstants.ErrorMessages.UserNotFoundWithID);

            var newRoleClaims = await _context.Set<Role>()
                .AsNoTracking()
                .Include(x => x.RoleClaims)
                .ThenInclude(x => x.Claim)
                .Where(x => x.Name == RoleType.ChurchUser.ToString())
                .SelectMany(x => x.RoleClaims.Select(y => y.ClaimId))
                .ToArrayAsync();

            if (!newRoleClaims.Any())
                return Error.AsResponse(UserConstants.ErrorMessages.UserNotFoundWithID);

            if (!newRoleClaims.All(roleClaim => currentUserClaims.Any(userClaim => userClaim == roleClaim)))
                return Error.AsResponse(UserConstants.ErrorMessages.UserNotFoundWithID);

            if (isEmailChange)
            {
                var userDuplicateCheck = await _context.Set<User>().SingleOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower());
                if (userDuplicateCheck != null)
                    return Error.AsResponse(UserConstants.ErrorMessages.UserDuplicateCheck);

                user.Email = request.Email;
                //var generateTokenRequest = new GenerateTokenRequest(DateTime.Now, UseCase.EmailConfirmation, user.Email);
                //var token = _generateTokenHandler.HandleAsync(generateTokenRequest).Result.Data;
                //await SendConfirmationEmail(user, token);
            }
            Mapper.Map(request, user);

            await _context.SaveChangesAsync();

            return Response.Success();
        }

        //private Task SendConfirmationEmail(User user, string confirmEmailToken)
        //{
        //    var fromEmail = _commonSettings.MailContents.EmailConfirmation.FromEmail;
        //    var fromName = _commonSettings.MailContents.EmailConfirmation.FromName;
        //    var mailSubject = _commonSettings.MailContents.EmailConfirmation.Subject;
        //    var mailBody = _commonSettings.MailContents.EmailConfirmation.Body
        //        .Replace("{NAME}", user.FirstName)
        //        .Replace("{Url}", $"{_commonSettings.BaseURL}/api/v1/users/confirm-email?confirmationToken={confirmEmailToken}&email={user.Email}");

        //    var message = new EmailMessage
        //    {
        //        ToAddress = new EmailInfo
        //        {
        //            Name = $"{user.FirstName} {user.LastName}",
        //            Address = user.Email
        //        },
        //        Body = mailBody,
        //        Subject = mailSubject,
        //        FromAddress = new EmailInfo()
        //        {
        //            Name = fromName,
        //            Address = fromEmail
        //        }
        //    };
        //    return _emailNotifier.SendEmailAsync(message);
        //}
    }

    public class EditUserValidator : AbstractValidator<EditUserRequest>
    {
        public EditUserValidator()
        {
            Include(new BaseUserDtoValidator());
        }
    }
}
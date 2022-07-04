using System.Linq;
using System.Threading.Tasks;

using ChurchManagementSystem.Core.Configuration;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Domain.Authorization;
using ChurchManagementSystem.Core.Extensions;
using ChurchManagementSystem.Core.Features.Users.Dtos;
using ChurchManagementSystem.Core.Features.Users.Utility;
using ChurchManagementSystem.Core.Features.Users.Validators;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;
using ChurchManagementSystem.Core.Mediator.Decorators;

using ChurchManagementSystem.Core.Security;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.Users
{
    [DataContext]
    
    public class AddUserRequest : AddUserDto, IRequest<AddResponse>
    {
    }

    public class AddUserHandler : IRequestHandler<AddUserRequest, AddResponse>
    {
        private readonly DataContext _context;
        private readonly CommonSettings _commonSettings;
        private readonly IPasswordService _passwordService;

        public AddUserHandler(DataContext context,
          IPasswordService passwordService, CommonSettings commonSettings)
          
        {
            _context = context;
            _passwordService = passwordService;
            _commonSettings = commonSettings;
        }



        public async Task<Response<AddResponse>> HandleAsync(AddUserRequest request)
        { 
            var role = await _context.Set<Role>()
                  .AsNoTracking()
                  .Where(x => x.RoleType == RoleType.unassigned.ToString())
                  .FirstOrDefaultAsync();

            var newUser = request.MapTo<User>();
            newUser.NormalLoginEnabled = true;
            newUser.Salt = _passwordService.CreateSalt();
            newUser.HashPassword = _passwordService.CreateHash(request.Password, newUser.Salt);
            newUser.RoleId = role.RoleId;
            _context.Set<User>().Add(newUser);
            await _context.SaveChangesAsync();

            if (role == null)
                return Error.AsResponse<AddResponse>(UserConstants.ErrorMessages.UserNotSaved);

                                                                                   
            return new AddResponse
            {
                Id = newUser.Id,
                Message = UserConstants.ErrorMessages.UserSavedSuccessful
            }.AsResponse();
        }
        public class AddUserValidator : AbstractValidator<AddUserRequest>
        {
            public AddUserValidator(PasswordPolicy passwordPolicy)
            {
                Include(new BaseUserDtoValidator());

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MaximumLength(UserConstants.ValidationConstants.MaxPasswordLength)
                    .Must(passwordPolicy.IsValid);
                   //WithMessage(UserProfileConstants.ErrorMessages.InvalidPassword);
            }
        }




        //public class AddUserValidator : AbstractValidator<AddUserRequest>
        //{
        //    public AddUserValidator(PasswordPolicy passwordPolicy)
        //    {
        //        Include(new BaseUserDtoValidator());

        //        RuleFor(x => x.Password)
        //            .NotEmpty()
        //            .MaximumLength(UserConstants.ValidationConstants.MaxPasswordLength)
        //            .Must(passwordPolicy.IsValid);
        //    }
        //}
    }
}
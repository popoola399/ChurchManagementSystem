using CSharpFunctionalExtensions;

using ChurchManagementSystem.Core.Security;

using IdentityServer4.Models;
using IdentityServer4.Validation;

using System;
using System.Threading.Tasks;

namespace ChurchManagementSystem.API.Security
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public const string Grant = "password";
        private readonly ILoginService _loginService;

        public ResourceOwnerPasswordValidator(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var login = await _loginService.GetLoginByEmail(context.UserName, context.Password);

                Result validateResponse = Result.Combine(login);

                if (validateResponse.IsFailure)
                {
                    if (validateResponse.Error.ToLower() == "Confirm your email before accessing ChurchManagementSystem.".ToLower())
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, $"{validateResponse.Error}");
                    }
                    else
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, $"{validateResponse.Error}");
                    }
                }
                else
                {
                    context.Result = new GrantValidationResult(login.Value.Id.ToString(), Grant);
                }
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                           $"Error Details: {ex.Message}");
            }
        }
    }
}
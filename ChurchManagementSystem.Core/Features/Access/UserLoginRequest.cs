using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain.Authorization;
using ChurchManagementSystem.Core.Extensions;
using ChurchManagementSystem.Core.Features.Access.Dtos;
using ChurchManagementSystem.Core.Features.Access.Utility;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;
using ChurchManagementSystem.Core.Mediator.Decorators;
using ChurchManagementSystem.Core.Security;

using FluentValidation;

using IdentityModel.Client;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using LoginDto = ChurchManagementSystem.Core.Features.Access.Dtos.LoginDto;

namespace ChurchManagementSystem.Core.Features.Access
{
    [DoNotValidate]
    [DataContext]
    public class UserLoginRequest : IRequest<TokenDto>
    {
        public UserLoginRequest(LoginDto login)
        {
            Login = login;
        }

        public LoginDto Login { get; }
    }

    public class UserLoginHandler : IRequestHandler<UserLoginRequest, TokenDto>
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IPasswordService _passwordService;

        public UserLoginHandler(IConfiguration configuration, DataContext context, IPasswordService passwordService)
        {
            _configuration = configuration;
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<Response<TokenDto>> HandleAsync(UserLoginRequest request)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .ThenInclude(x => x.RoleClaims)
                .SingleOrDefaultAsync(x => x.Email == request.Login.Email && x.Active);

            var portalAccessClaimId = await _context.Claims
                .Where(x => x.Name == Claims.WebPortal)
                .Select<Claim, int?>(x => x.ClaimId)
                .SingleOrDefaultAsync();

            if (user == null)
                return Error.AsResponse<TokenDto>(AccessConstants.ErrorMessages.UserExist);

            var hash = _passwordService.CreateHash(request.Login.Password, user.Salt);
            if (user.HashPassword != hash)
                return Error.AsResponse<TokenDto>(AccessConstants.ErrorMessages.FailedLogin);

            TokenDto response;

            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(_configuration["Identity:Authority"]);
                if (disco.IsError)
                    return Error.AsResponse<TokenDto>(AccessConstants.ErrorMessages.AuthFailed);

                var tokenClient = new TokenClient(disco.TokenEndpoint, request.Login.ClientId, request.Login.ClientSecret);
                var tokenResponse = await tokenClient
                    .RequestResourceOwnerPasswordAsync(request.Login.Email, request.Login.Password, request.Login.Scope);

                response = tokenResponse.MapTo<TokenDto>();
            }

            //response.RoleId = user.RoleId;
            //response.FirstName = user.FirstName;
            //response.LastName = user.LastName;
            //response.UserId = user.Id;
            //response.HasWebPortalAccess = !portalAccessClaimId.HasValue || user.Role.RoleClaims.Any(x => x.ClaimId == portalAccessClaimId.Value && x.Active);

            return response.AsResponse();
        }

        public class UserLoginValidator : AbstractValidator<UserLoginRequest>
        {
            public UserLoginValidator()
            {
                RuleFor(x => x.Login.ClientId)
                    .MaximumLength(ValidationConstants.MaxClientIdLength);

                RuleFor(x => x.Login.ClientSecret)
                    .MaximumLength(ValidationConstants.MaxClientSecretLength);

                RuleFor(x => x.Login.Email)
                    .MaximumLength(ValidationConstants.MaxEmailLength);

                RuleFor(x => x.Login.Password)
                    .MaximumLength(ValidationConstants.MaxPasswordLength);

                RuleFor(x => x.Login.Scope)
                    .MaximumLength(ValidationConstants.MaxScopeLength);
            }
        }
    }
}                                                                

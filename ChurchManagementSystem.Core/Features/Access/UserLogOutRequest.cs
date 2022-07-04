using ChurchManagementSystem.Core.Features.Access.Dtos;
using ChurchManagementSystem.Core.Features.Access.Utility;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;
using FluentValidation;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Response = ChurchManagementSystem.Core.Mediator.Response;

namespace ChurchManagementSystem.Core.Features.Access
{
    [DoNotValidate]
    public class UserLogOutRequest : LogOutDto, IRequest
    {
    }

    public class UserLogOutHandler : IRequestHandler<UserLogOutRequest>
    {
        private readonly IConfiguration _configuration;

        public UserLogOutHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<Response> HandleAsync(UserLogOutRequest request)
        {
            var client = new HttpClient();
            var response = client.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = $"{ _configuration["Identity: Authority"]}/connect/revocation",
                ClientId = request.ClientId,
                ClientSecret = request.ClientSecret,
                Token = request.Token
            });

            return Response.SuccessAsync();
        }

        public class UserLogOutValidator : AbstractValidator<UserLogOutRequest>
        {
            public UserLogOutValidator()
            {
                RuleFor(x => x.ClientId).MaximumLength(ValidationConstants.MaxClientIdLength).NotNull().NotEmpty();
                RuleFor(x => x.ClientSecret).MaximumLength(ValidationConstants.MaxClientSecretLength).NotNull().NotEmpty();
            }
        }
    }
}
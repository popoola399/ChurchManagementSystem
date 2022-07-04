using System;
using System.Linq;
using System.Threading.Tasks;

using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Features.UserProfile.Utility;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;

using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.Utilities
{
    [DoNotValidate]
    public class ValidateTokenRequest : IRequest<bool>
    {
        public ValidateTokenRequest(string email, string token, UseCase useCase)
        {
            Email = email;
            Token = token;
            UseCase = useCase;
        }

        public string Email { get; set; }
        public string Token { get; set; }
        public UseCase UseCase { get; set; }
    }

    public class ValidateTokenRequestHandler : IRequestHandler<ValidateTokenRequest, bool>
    {
        private readonly DataContext _context;

        public ValidateTokenRequestHandler(DataContext context)
        {
            _context = context;
        }

        public Task<Response<bool>> HandleAsync(ValidateTokenRequest request)
        {
            var tokenModel = _context.Set<TokenManager>()
                .Where(x => x.Email == request.Email && x.UseCase == request.UseCase && x.Token == request.Token)
                .FirstOrDefault();

            if (tokenModel == null)
                return new Error { ErrorMessage = UserProfileConstants.ErrorMessages.InvalidToken }.AsResponseAsync<bool>();

            if (tokenModel.Expired)
                return new Error { ErrorMessage = UserProfileConstants.ErrorMessages.TokenExpired }.AsResponseAsync<bool>();

            if (DateTime.Now > tokenModel.TimeStamp.AddMinutes(30))
            {
                tokenModel.Expired = true;

                _context.Set<TokenManager>().Attach(tokenModel);
                _context.Entry(tokenModel).State = EntityState.Modified;
                _context.SaveChanges();
                return new Error { ErrorMessage = UserProfileConstants.ErrorMessages.TokenExpired }.AsResponseAsync<bool>();
            }

            if (tokenModel.Used)
                return new Error { ErrorMessage = UserProfileConstants.ErrorMessages.TokenUsed }.AsResponseAsync<bool>();

            if (request.UseCase != UseCase.ValidateToken)
            {
                tokenModel.Used = true;

                _context.Set<TokenManager>().Attach(tokenModel);
                _context.Entry(tokenModel).State = EntityState.Modified;
                _context.SaveChanges();
            }
         
            return true.AsResponseAsync();
        }
    }
}
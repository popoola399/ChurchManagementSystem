using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;

using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.Utilities
{
    [DoNotValidate]
    public class GenerateTokenRequest : IRequest<string>
    {
        public GenerateTokenRequest(DateTime timeStamp, UseCase useCase, string email, int keyLength = 16)
        {
            TimeStamp = timeStamp;
            UseCase = useCase;
            Email = email;
            KeyLength = keyLength;
        }

        public DateTime TimeStamp { get; set; }
        public UseCase UseCase { get; set; }
        public int KeyLength { get; set; }
        public string Email { get; set; }
    }

    public class GenerateTokenRequestHandler : IRequestHandler<GenerateTokenRequest, string>
    {
        private readonly DataContext _context;

        public GenerateTokenRequestHandler(DataContext context)
        {
            _context = context;
        }

        public Task<Response<string>> HandleAsync(GenerateTokenRequest request)
        {
            var tokenModel = _context.Set<TokenManager>()
               .Where(x => x.Email == request.Email && x.UseCase == request.UseCase && !(x.Expired || x.Used))
               .FirstOrDefault();

            if (tokenModel != null)
            {
                if (DateTime.Now > tokenModel.TimeStamp.AddMinutes(30))
                {
                    tokenModel.Expired = true;

                    _context.Set<TokenManager>().Attach(tokenModel);
                    _context.Entry(tokenModel).State = EntityState.Modified;
                    _context.SaveChanges();
                    _context.Entry(tokenModel).State = EntityState.Detached;

                    tokenModel = GetToken(request.Email, request.TimeStamp, request.KeyLength, request.UseCase);
                    _context.Set<TokenManager>().Add(tokenModel);
                }
                else
                {
                    tokenModel.TimeStamp = DateTime.Now;
                    _context.Set<TokenManager>().Attach(tokenModel);
                    _context.Entry(tokenModel).State = EntityState.Modified;
                }
            }
            else
            {
                tokenModel = GetToken(request.Email, request.TimeStamp, request.KeyLength, request.UseCase);
                _context.Set<TokenManager>().Add(tokenModel);
            }

            _context.SaveChanges();

            return tokenModel.Token.AsResponseAsync();
        }

        public TokenManager GetToken(string email, DateTime TimeStamp, int KeyLength, UseCase useCase)
        {
            var key = new byte[KeyLength];

            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetNonZeroBytes(key);
            }

            var time = BitConverter.GetBytes(TimeStamp.ToBinary());

            var token = Convert.ToBase64String(time.Concat(key).ToArray())
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');

            var tokenModel = new TokenManager
            {
                Email = email,
                Token = token,
                TimeStamp = DateTime.Now,
                CreatedDate = DateTime.UtcNow,
                Expired = false,
                UseCase = useCase
            };

            return tokenModel;
        }
    }
}
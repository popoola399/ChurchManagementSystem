using ChurchManagementSystem.Core.Security;
using ChurchManagementSystem.Core.Logging;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChurchManagementSystem.API.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class ChurchManagementSystemClaimTypes
    {
        /// <summary>
        /// 
        /// </summary>
        public static string FirstName { get { return "FirstName"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string LastName { get { return "LastName"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string RoleId { get { return "RoleId"; } }
        /// <summary>
        /// 
        /// </summary>
        public static string RoleName { get { return "RoleName"; } }
        /// <summary>
        /// 
        /// </summary>
        public static string UserId { get { return "UserId"; } }
        /// <summary>
        /// 
        /// </summary>
        public static string HasWebPortalAccess { get { return "HasWebPortalAccess"; } }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly ILoginService _loginService;
        private readonly ILogger _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginService"></param>
        /// <param name="logger"></param>
        public ProfileService(ILoginService loginService, ILogger logger)
        {
            _loginService = loginService;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var loginId = GetLoginId(context.Subject.GetSubjectId());
            var login = loginId.HasValue ? await _loginService.GetLoginById(loginId.Value) : null;

            if (login != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Subject, login.Id.ToString()),
                    new Claim(JwtClaimTypes.Email, login.Email ?? string.Empty),
                    new Claim(IdentityClaims.UserClaim, login.Username ?? string.Empty),
                    new Claim(ChurchManagementSystemClaimTypes.FirstName, login.FirstName ?? string.Empty),
                    new Claim(ChurchManagementSystemClaimTypes.LastName, login.LastName ?? string.Empty),
                    new Claim(ChurchManagementSystemClaimTypes.RoleId, login.RoleId.ToString() ?? string.Empty),
                    new Claim(ChurchManagementSystemClaimTypes.RoleName, login.Username ?? string.Empty),
                    new Claim(ChurchManagementSystemClaimTypes.UserId, login.Id.ToString() ?? string.Empty),
                    new Claim(ChurchManagementSystemClaimTypes.HasWebPortalAccess, login.HasWebPortalAccess.ToString() ?? string.Empty)
            };

                context.IssuedClaims = claims;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var loginId = GetLoginId(context.Subject.GetSubjectId());
            var login = loginId.HasValue ? await _loginService.GetLoginById(loginId.Value) : null;

            context.IsActive = login != null;
        }

        private int? GetLoginId(string subjectId)
        {
            if (int.TryParse(subjectId, out var loginId))
                return loginId;

            _logger.Log($"Invalid SubjectId found in {nameof(ProfileService)}", subjectId);

            return null;
        }
    }
}
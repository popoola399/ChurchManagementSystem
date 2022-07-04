using ChurchManagementSystem.Core.Common;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Exceptions;
using ChurchManagementSystem.Core.Extensions;
using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using ChurchManagementSystem.Core.Security;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


using System;
using System.Linq;
using System.Security.Claims;

namespace ChurchManagementSystem.Core.Security
{
    public interface IIdentityUserContext
    {
        IdentityUser RequestingUser { get; }
        IdentityUser RequestingCurrentUser { get; }
    }

    public class HttpIdentityUserContext : IIdentityUserContext
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IdentityUser _cachedUser;

        public HttpIdentityUserContext(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public IdentityUser RequestingUser
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userId == null)
                    throw new AuthorizationFailedException(Constants.Auth.NoUser);

                var user = _dataContext
                    .Set<User>()
                    .Include(x => x.Role)
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Id.ToString() == userId.Value && x.Active);

                if (user == null)
                    throw new AuthorizationFailedException(Constants.Auth.NoUser);

                var currentUser = user.MapTo<IdentityUser>();

                currentUser.Claims = _dataContext.RoleClaims
                    .Where(x => x.RoleId == user.RoleId)
                    .Select(x => new ClaimDto
                    {
                        ClaimId = x.ClaimId,
                        Description = x.Claim.Description,
                        Name = x.Claim.Name
                    })
                    .ToList();
                currentUser.RoleName = user.Role.Name;

                currentUser.ByPassAudit = currentUser.Claims
                    .Any(x => x.Name.ToLower().Contains("bypass read audit"));

                if (currentUser == null)
                    throw new AuthorizationFailedException(Constants.Auth.NoUser);

                return _cachedUser = currentUser;
            }
        }

        public IdentityUser RequestingCurrentUser => throw new NotImplementedException();
    }

    public class DeferredHttpIdentityUserContext : IIdentityUserContext
    {
        private readonly Lazy<HttpIdentityUserContext> _deferredContext;

        public DeferredHttpIdentityUserContext(Lazy<HttpIdentityUserContext> deferredContext)
        {
            _deferredContext = deferredContext;
        }

        public IdentityUser RequestingUser => _deferredContext.Value.RequestingUser;

        public IdentityUser RequestingCurrentUser => throw new NotImplementedException();
    }

    public class NullIdentityUserContext : IIdentityUserContext
    {
        public IdentityUser RequestingUser => new IdentityUser
        {
            Id = 1
        };

        public IdentityUser RequestingCurrentUser => throw new NotImplementedException();

    }
}
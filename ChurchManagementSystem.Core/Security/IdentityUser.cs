using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using ChurchManagementSystem.Core.Features.Users.Utility;

using System.Collections.Generic;

namespace ChurchManagementSystem.Core.Security
{
    public class IdentityUser
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public bool IsRestricted { get; set; }

        public bool ByPassAudit { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";

        public bool NormalLoginEnabled { get; set; }

    
       public bool GoogleLoginEnabled { get; set; }

        public string GoogleLoginEmail { get; set; }

        public string GoogleUserId { get; set; }

        public string GoogleProfilePictureUrl { get; set; }

        public List<ClaimDto> Claims { get; set; } = new List<ClaimDto>();
    }

}
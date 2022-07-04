using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchManagementSystem.Core.Features.RolesClaims.Dto
{
    public class BaseRoleDto
    {
        /// <summary>
        ///  The role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Role description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The flag to set role to county restriction
        /// </summary>
        public bool CountyRestriction { get; set; }
    }

    public class GetRoleDto : BaseRoleDto
    {
        /// <summary>
        /// The parent node identifier.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// List of cliams for the role
        /// </summary>
        public List<ClaimDto> Claims { get; set; }

        /// <summary>
        /// Whether or not the current user's permissions are sufficient for assigning this role to a user
        /// </summary>
        public bool IsAssignable { get; set; }
    }

    public class EditRoleDto : BaseRoleDto
    {
        /// <summary>
        /// The parent node identifier.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// The ID list of edited claims
        /// </summary>
        public List<int> ClaimIds { get; set; }
    }

    public class AddRoleDto : BaseRoleDto
    {
        /// <summary>
        /// The ID list of the new claims
        /// </summary>
        public List<int> ClaimIds { get; set; }
    }
}
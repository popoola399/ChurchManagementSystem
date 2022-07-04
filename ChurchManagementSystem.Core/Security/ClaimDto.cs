using System;
using System.Collections.Generic;
using System.Text;

namespace ChurchManagementSystem.Core.Features.RolesClaims.Dto
{
    public class ClaimDto
    {
        /// <summary>
        /// The parent node identifier
        /// </summary>
        public int ClaimId { get; set; }

        /// <summary>
        ///  The claim name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The claim description
        /// </summary>
        public string Description { get; set; }
    }
}
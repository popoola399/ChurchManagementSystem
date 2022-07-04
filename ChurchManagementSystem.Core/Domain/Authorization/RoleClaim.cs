using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchManagementSystem.Core.Domain.Authorization
{
    public class RoleClaim
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleClaimId { get; set; }

        public int RoleId { get; set; }

        public bool Active { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        public int ClaimId { get; set; }

        [ForeignKey("ClaimId")]
        public Claim Claim { get; set; }
    }
}
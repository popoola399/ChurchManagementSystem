using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchManagementSystem.Core.Domain.Authorization
{
    public class Claim
    {
        public int ClaimId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<RoleClaim> RoleClaims { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public Claim()
        {
            RoleClaims = new List<RoleClaim>();
        }
    }
}
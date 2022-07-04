using System;
using System.Collections.Generic;

namespace ChurchManagementSystem.Core.Domain.Authorization
{
    public class Role
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //This can be system generated or Custom
        public string RoleType { get; set; }

        public ICollection<RoleClaim> RoleClaims { get; set; }

        public ICollection<User> Users { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public Role()
        {
            RoleClaims = new List<RoleClaim>();
        }
    }
}
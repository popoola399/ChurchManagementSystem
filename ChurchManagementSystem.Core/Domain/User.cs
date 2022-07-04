using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using ChurchManagementSystem.Core.Domain.Authorization;
using ChurchManagementSystem.Core.Features.Users.Utility;

namespace ChurchManagementSystem.Core.Domain
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public bool Active { get; set; }
        public string Salt { get; set; }
        public string HashPassword { get; set; }
        public bool CannotChangePassword { get; set; }
        public int RoleId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool AcceptedTandC { get; set; }
        public bool NormalLoginEnabled { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
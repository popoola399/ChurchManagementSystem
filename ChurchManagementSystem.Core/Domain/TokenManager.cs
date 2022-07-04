using System;

namespace ChurchManagementSystem.Core.Domain
{
    public class TokenManager : Entity
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Expired { get; set; }
        public bool Used { get; set; }
        public UseCase UseCase { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public enum UseCase
    {
        PasswordReset = 1,
        EmailConfirmation = 2,
        ValidateToken = 3
    }
}
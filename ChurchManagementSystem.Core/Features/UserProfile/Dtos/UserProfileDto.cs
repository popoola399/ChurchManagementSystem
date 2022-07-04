namespace ChurchManagementSystem.Core.Features.UserProfile.Dtos
{
    public class UserResetPasswordDto
    {
        /// <summary>
        /// The user old Password
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// The user new Password
        /// </summary>
        public string NewPassword { get; set; }
    }

    public class UserForgottenPasswordDto
    {
        /// <summary>
        /// The user's Account email for token validation
        /// </summary>
        public string RecoveryEmail { get; set; }
    }

    public class UserResetForgotPasswordDto
    {
        /// <summary>
        /// The user's reset token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The user's new password
        /// </summary>
        public string NewPassword { get; set; }
    }
}
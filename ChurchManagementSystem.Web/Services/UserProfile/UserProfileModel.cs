namespace ChurchManagementSystem.Web.Services.UserProfile
{
    public class ResetForgotPasswordRequest
    {
        public string RecoveryEmail { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string RecoveryEmail { get; set; }
    }
}
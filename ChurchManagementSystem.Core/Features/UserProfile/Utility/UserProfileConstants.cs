namespace ChurchManagementSystem.Core.Features.UserProfile.Utility
{
    public static partial class UserProfileConstants
    {
        public static class ErrorMessages
        {
            public const string UserNotFoundWithID = "No User was found with the provided ID.";
            public const string IncorrectCredentials = "Credentials combination entered are incorrect .";
            public const string CanNotChangePassword = "Password cannot be changed for this user";
            public const string PasswordIncorrect = "User password incorrect.";
            public const string OldPasswordIncorrect = "Old password incorrect.";
            public const string NoEmail = "Email does not exist on this platform";
            public const string InvalidToken = "Token Invalid";
            public const string TokenExpired = "Token has already expired. Generate a new one";
            public const string TokenUsed = "This token has already been used. Generate a new one";
            public const string InvalidPassword = "The provided password is invalid.";
            public const string InvalidFacebookToken = "Facebook Token is a compulsory field";
            public const string InvalidGoogleToken = "Google Token is a compulsory field";
        }

        public static class ValidationConstants
        {
            public const int OldPassword = 50;
            public const int NewPassword = 50;
            public const int Email = 100;
        }
    }
}
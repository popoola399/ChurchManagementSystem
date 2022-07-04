namespace ChurchManagementSystem.Core.Features.Users.Utility
{
    public static partial class UserConstants
    {
        public static class ErrorMessages
        {
            public const string UserNotFoundWithID = "No User was found with the provided ID.";
            public const string CanNotChangePassword = "Password cannot be changed for this user";
            public const string UserExists = "User with the information exists.";
            public const string UserUpdateSuccessful = "User Updated successfully.";
            public const string UserSavedSuccessful = "User saved successfully.";
            public const string PasswordIncorrect = "User password incorrect.";
            public const string UserIDNotGreaterThanZero = "'User Id' must be greater than '0'.";
            public const string UserNotFoundWithParam = "No User was found with the Query parameter(s) provided.";
            public const string UserInaccessible = "The requested User is inaccessible to the current User.";
            public const string UserInvalid = "The user requested is invalid";
            public const string InvalidToken = "The token is invalid";
            public const string AccountDetailsFailure = "Unable to get customer's account details";
            public const string UserDuplicateCheck = "Email already exist in the system";
            public const string UserUploadSuccessdful = "Profile picture uploaded successfully";
            public const string InvalidImage = "Invalid image uploaded";
            public const string UploadError = "Upload Error";
            public const string UploadFileLimitExceeded = "File size cannot exceed 3MB";
            public const string InvalidExtention = "File Extension Is InValid - Only Upload JPG/JPEG/PNG";
            public const string InvalidFacebookToken = "Invalid Token Provided";
            public const string UserNotSaved = "User Not Saved";
        }

        public static class ValidationConstants
        {
            public const int MaxFirstNameLength = 50;
            public const int MaxLastNameLength = 50;
            public const int MaxEmailLength = 50;
            public const int MaxSaltLength = 50;
            public const int MaxPasswordLength = 50;
            public const int MaxHashPasswordLength = 10;
            public const int MaxParamLength = 50;
        }
    }
}
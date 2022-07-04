namespace ChurchManagementSystem.Core.Features.RolesClaims.Utility
{
    public static class RoleConstants
    {
        public static class ErrorMessages
        {
            public const string RoleNotFoundWithID = "No Role was found with the provided ID.";
            public const string RoleUpdateSuccessful = "Role Updated successfully.";
            public const string RoleExists = "Role with the name already exists.";
            public const string RoleSavedSuccessful = "Role saved successfully.";
            public const string RoleIDNotGreaterThanZero = "'Role Id' must be greater than '0'.";
            public const string RoleType = "Custom";
            public const string UserAttachedToRole = "One or more User is attached to this Role. Role cannot be deleted";
            public const string InvalidClaimsId = "Invalid claim Id in the List";
            public const string RoleUnassignableByUser = "The current User does not have sufficient permissions to assign the requested Role.";
            public const string RoleUnremovableByUser = "The current User does not have sufficient permissions to alter this User's Role.";
            public const string UserUnableToUpdateUser = "The current User does not have sufficient permissions to alter another user profile.";
            public const string InvalidRole = "Invalid role selected for user.";
        }

        public static class ValidationConstants
        {
            public const int MaxNameLength = 50;
            public const int MaxDescriptionLength = 200;
        }
    }
}
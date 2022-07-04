using System;

namespace ChurchManagementSystem.Core.Common
{
    public static class Constants
    {
        public const int MaxPageSize = 100;

        public static class Auth
        {
            public const string UserPolicy = "User-policy";
            public const string ApiKey = "api-key";
            public const string NoUser = "No users found for this request.";
            public const string UnauthorizedUser = "User is not Authorized to access this resource";
        }

        public static class DateTimes
        {
            public static DateTime MinimumAllowedDateTime => new DateTime(1900, 1, 1);
            public const int DaysInAWeek = 7;
            public const int ResetTokenExpiryHour = 24;
        }

        public static class DateFormat
        {
            public const string ShortDate = "ShortDate";
            public const string ShortDateFormat = "MM/dd/yyyy";
            public const string LongDate = "LongDate";
            public const string LongDateFormat = "dddd, MMMM d, yyyy";
            public const string ShortTime = "ShortTime";
            public const string ShortTimeFormat = "h:mm tt";
            public const string LongTime = "LongTime";
            public const string LongTimeFormat = "h:mm:ss tt";
            public const string FullDateTime = "FullDateTime";
            public const string FullDateTimeFormat = "MM/dd/yyyy h:mm tt";
            public const string Timestamp = "Timestamp";
            public const string TimestampFormat = "MM/dd/yyyy h:mm:ss tt";
        }

        public static class RegExPatterns
        {
            public const string Ssn = "(?=\\d{5})\\d";
            public const string SsnMask = "x";
            public const string ContainsLetterAndDigit = "^(?=.*[a-zA-Z])(?=.*[0-9])";
        }

        public static class ErrorMessages
        {
            public const string EmailAddressInvalid = "The Email field is invalid.";
            public const string PhoneInvalid = "The Phone field is invalid.";
            public const string CellInvalid = "The Cell field is invalid.";
            public const string FaxInvalid = "The Fax field is invalid.";
            public const string CompanyPhoneInvalid = "The Company Phone field is invalid.";
            public const string PhoneOrEmailRequired = "You must submit either phone number or E-Mail.";
            public const string InvalidDate = "The date is not valid.";
            public const string FromDateLessThanDate = "The from date must be less than the to date.";
        }

        public static class FileTypes
        {
            public const string TextFile = ".txt";
            public const string PDF = ".pdf";
        }
    }
}
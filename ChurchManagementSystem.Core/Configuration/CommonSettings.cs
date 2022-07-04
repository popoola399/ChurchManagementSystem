using System.Net;

namespace ChurchManagementSystem.Core.Configuration
{
    public class CommonSettings
    {
        public MailCredentials MailCredentials { get; } = new MailCredentials();
        public ChurchManagementDetails ChurchManagementDetails { get; } = new ChurchManagementDetails();


        public ExceptionlessSettings Exceptionless { get; } = new ExceptionlessSettings();

        public string BaseURL { get; set; }
        public string ForgotPasswordRedirect { get; set; }
        public string ConfirmEmailRedirect { get; set; }
        public string Environment { get; set; }
        public string TestEmailAccounts { get; set; }
        public string ConnectionString { get; set; }
    }

    public class ChurchManagementSysytemSettings
    {
        public bool IsTest { get; set; }
    }

    public class ExceptionlessSettings
    {
        public bool UseExceptionless { get; set; }

        public string Key { get; set; }
    }

    public class MailCredentials
    {
        public string APIKey { get; set; }
    }


    public class ChurchManagementDetails
    {
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderCity { get; set; }
        public string SenderState { get; set; }
        public string SenderZip { get; set; }
    }

}
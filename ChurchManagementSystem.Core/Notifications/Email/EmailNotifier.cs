
using ChurchManagementSystem.Core.Configuration;

using SendGrid;
using SendGrid.Helpers.Mail;

using System.Threading.Tasks;
using ChurchManagementSystem.Core.Notifications.Email;

namespace ChurchManagementSystem.Core.Notifications.Email
{
    public class EmailNotifier : IEmailNotifier
    {
        private readonly CommonSettings _commonSettings;

        public EmailNotifier(CommonSettings commonSettings)
        {
            _commonSettings = commonSettings;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var client = new SendGridClient(_commonSettings.MailCredentials.APIKey);
            var from = new EmailAddress(message.FromAddress.Address, message.FromAddress.Name);
            var subject = message.Subject;
            var to = new EmailAddress(message.ToAddress.Address, message.ToAddress.Name);
            var plainTextContent = message.Body;
            var htmlContent = message.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg).Result;
        }
    }
}
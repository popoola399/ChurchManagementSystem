using ChurchManagementSystem.Core.Notifications.Email;

using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Notifications.Email
{
    public interface IEmailNotifier
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
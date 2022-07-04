
using System.Collections.Generic;
using System.IO;

namespace ChurchManagementSystem.Core.Notifications.Email
{
    public class EmailInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class EmailAttachment
    {
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public string MediaSubType { get; set; }
        public Stream Content { get; set; }
    }

    public class EmailMessage
    {
        public EmailInfo FromAddress { get; set; }
        public EmailInfo ToAddress { get; set; }
        public List<EmailInfo> ToAddresses { get; set; } = new List<EmailInfo>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
    }
}

using System;

namespace ChurchManagementSystem.Core.Domain
{
    public class Template : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SpecialCode { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
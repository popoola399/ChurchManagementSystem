
using Microsoft.AspNetCore.Http;

using System;

namespace ChurchManagementSystem.Core.Features.Template.Dtos
{
    public class TemplateUploadResponse
    {
        /// <summary>
        /// The response Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The template special code.
        /// </summary>
        public string SpecialCode { get; set; }
    }

    public class GetTemplateDto
    {
        public int Id { get; set; }
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
    }

    public class SaveTemplateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public IFormFile HtmlFile { get; set; }
    }

    public class EditImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile FormFile { get; set; }
    }

    public class EditHtmlTemplateDto
    {
        public int Id { get; set; }
        public IFormFile HtmlFile { get; set; }
    }
}

using ChurchManagementSystem.Core.Features.Template.Dtos;
using ChurchManagementSystem.Core.Features.Template.Utility;

using ChurchManagementSystem.Core.Configuration;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Mediator;

using FluentValidation;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Features.Template
{
    public class SaveTemplateRequest : IRequest<TemplateUploadResponse>
    {
        public SaveTemplateDto SaveTemplateDto { get; set; }
    }

    public class SaveTemplateRequestHandler : IRequestHandler<SaveTemplateRequest, TemplateUploadResponse>
    {
        private readonly DataContext _context;

        public SaveTemplateRequestHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<TemplateUploadResponse>> HandleAsync(SaveTemplateRequest request)
        {

            var result = new StringBuilder();
            using (var reader = new StreamReader(request.SaveTemplateDto.HtmlFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }

            var template = new Domain.Template
            {
                Name = request.SaveTemplateDto.Name,
                Description = request.SaveTemplateDto.Description,
                FromName = request.SaveTemplateDto.FromName,
                FromEmail = request.SaveTemplateDto.FromEmail,
                Subject = request.SaveTemplateDto.Subject,
                Body = result.ToString(),
                SpecialCode = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            await _context.Set<Domain.Template>().AddAsync(template);

            await _context.SaveChangesAsync();


            return new TemplateUploadResponse() { Message = TemplateConstants.ErrorMessages.UploadSuccessdful, SpecialCode = template.SpecialCode }.AsResponse();
        }
    }

    public class UploadTemplateRequestValidator : AbstractValidator<SaveTemplateRequest>
    {
        public UploadTemplateRequestValidator()
        {
            //RuleFor(x => x.Image).NotEmpty().NotNull().Must(ImageValidatiion.CheckIfImageFile).WithMessage(ImageConstants.ErrorMessages.InvalidImage);
            RuleFor(x => x.SaveTemplateDto.Name).NotEmpty().NotNull();
            RuleFor(x => x.SaveTemplateDto.Description).NotEmpty().NotNull();
            RuleFor(x => x.SaveTemplateDto.FromName).NotEmpty().NotNull();
            RuleFor(x => x.SaveTemplateDto.FromEmail).NotEmpty().NotNull();
            RuleFor(x => x.SaveTemplateDto.Subject).NotEmpty().NotNull();
        }
    }
}

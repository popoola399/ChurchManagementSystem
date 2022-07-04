using ChurchManagementSystem.Core.Features.Template.Dtos;

using ChurchManagementSystem.Core.Data;                                          
using ChurchManagementSystem.Core.Mediator;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Features.Template
{
    public class EditHtmlTemplateRequest : IRequest<string>
    {
        public EditHtmlTemplateDto EditHtmlTemplateDto { get; set; }
    }

    public class EditHtmlTemplateRequestHandler : IRequestHandler<EditHtmlTemplateRequest, string>
    {
        private readonly DataContext _context;

        public EditHtmlTemplateRequestHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<string>> HandleAsync(EditHtmlTemplateRequest request)
        {

            var template = await _context.Set<Domain.Template>().SingleOrDefaultAsync(x => x.Id == request.EditHtmlTemplateDto.Id);

            if (template == null)
                return new Error { ErrorMessage = "Template not found" }.AsResponse<string>();

            var result = new StringBuilder();
            using (var reader = new StreamReader(request.EditHtmlTemplateDto.HtmlFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }

            template.Body = result.ToString();

            template.ModifiedDate = DateTime.Now;

            _context.Set<Domain.Template>()
                .Update(template);

            await _context.SaveChangesAsync();


            return "Template updated successfully".AsResponse();
        }
    }

    public class EditHtmlTemplateRequestValidator : AbstractValidator<EditHtmlTemplateRequest>
    {
        public EditHtmlTemplateRequestValidator()
        {
            //RuleFor(x => x.Image).NotEmpty().NotNull().Must(ImageValidatiion.CheckIfImageFile).WithMessage(ImageConstants.ErrorMessages.InvalidImage);
            RuleFor(x => x.EditHtmlTemplateDto.Id).GreaterThan(0);
            RuleFor(x => x.EditHtmlTemplateDto.HtmlFile).NotNull();
        }
    }
}

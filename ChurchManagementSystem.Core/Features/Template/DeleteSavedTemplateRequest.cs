using ChurchManagementSystem.Core.Features.Template.Dtos;
using ChurchManagementSystem.Core.Features.Template.Utility;

using ChurchManagementSystem.Core.Configuration;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Mediator;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Features.Template
{
    public class DeleteSavedTemplateRequest : IRequest<TemplateUploadResponse>
    {
        public int Id { get; set; }
    }

    public class DeleteSavedTemplateRequestHandler : IRequestHandler<DeleteSavedTemplateRequest, TemplateUploadResponse>
    {
        private readonly DataContext _context;

        public DeleteSavedTemplateRequestHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<TemplateUploadResponse>> HandleAsync(DeleteSavedTemplateRequest request)
        {
            var template = await _context.Set<Domain.Template>()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted);

            if (template == null)
                return new Error { ErrorMessage = TemplateConstants.ErrorMessages.TemplateNotFound }.AsResponse<TemplateUploadResponse>();


            template.IsDeleted = true;
            template.DeletedDate = DateTime.Now;

            _context.Set<Domain.Template>()
                .Update(template);

            await _context.SaveChangesAsync();

            return new TemplateUploadResponse() { Message = TemplateConstants.ErrorMessages.DeletedSuccessdful, SpecialCode = template.SpecialCode }.AsResponse();
        }
    }

    public class DeleteSavedTemplateRequestValidator : AbstractValidator<DeleteSavedTemplateRequest>
    {
        public DeleteSavedTemplateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}

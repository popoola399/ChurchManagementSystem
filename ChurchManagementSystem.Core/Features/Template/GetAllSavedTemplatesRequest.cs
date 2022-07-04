using ChurchManagementSystem.Core.Features.Template.Dtos;

using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Extensions;
using ChurchManagementSystem.Core.Logic.Queries;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Extensions;

namespace ChurchManagementSystem.Core.Features.Template
{
    [DoNotValidate]
    public class GetAllSavedTemplatesRequest : PagedQueryRequest, IRequest<PagedQueryResult<GetTemplateDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SpecialCode { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
    }

    public class GetAllSavedTemplatesRequestHandler : IRequestHandler<GetAllSavedTemplatesRequest, PagedQueryResult<GetTemplateDto>>
    {
        private readonly DataContext _context;
        private readonly IMediator _mediator;

        public GetAllSavedTemplatesRequestHandler(DataContext context,
            IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Response<PagedQueryResult<GetTemplateDto>>> HandleAsync(GetAllSavedTemplatesRequest request)
        {
            var templates = _context.Set<Domain.Template>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (request.Id != default)
            {
                templates = templates.Where(x =>
                   x.Id == request.Id);
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                templates = templates.Where(x =>
                    x.Name.Contains(request.Name));
            }

            if (!string.IsNullOrEmpty(request.SpecialCode))
            {
                templates = templates.Where(x =>
                    x.Name.Contains(request.SpecialCode));
            }

            templates = Sort(templates, request.SortBy, request.SortAscending);

            var result = await templates
             .ToPagedResult<Domain.Template, GetTemplateDto>(request.PageNumber, request.PageSize);

            return result.AsResponse();
        }

        private IQueryable<Domain.Template> Sort(IQueryable<Domain.Template> Images, string sortBy, bool ascending)
        {
            switch (sortBy)
            {
                default:
                case nameof(GetTemplateDto.Name):
                    return ascending
                        ? Images.OrderBy(x => x.Name)
                        : Images.OrderByDescending(x => x.Name);
            }
        }
    }
}

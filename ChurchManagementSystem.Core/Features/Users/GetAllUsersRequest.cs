using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Features.Users.Dtos;
using ChurchManagementSystem.Core.Logic.Queries;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;
using ChurchManagementSystem.Core.Mediator.Decorators;
using ChurchManagementSystem.Core.Security;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

namespace  ChurchManagementSystem.Core.Features.Users
{
    [DataContext]
    [DoNotValidate]
    public class GetAllUsersRequest : IRequest<PagedQueryResult<GetUserDto>>
    {
        public PagedQueryRequest Query { get; set; }

        public string Search { get; set; }

        public string SortBy { get; set; }

        public bool SortAscending { get; set; }

        public int? RoleId { get; set; }
    }

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, PagedQueryResult<GetUserDto>>
    {
        private readonly DataContext _context;
        private readonly IIdentityUserContext _identity;
        private readonly IMediator _mediator;

        public GetAllUsersHandler(DataContext context,
            IIdentityUserContext identity,
            IMediator mediator)
        {
            _context = context;
            _identity = identity;
            _mediator = mediator;
        }

        public async Task<Response<PagedQueryResult<GetUserDto>>> HandleAsync(GetAllUsersRequest request)
        {
            var result = new PagedQueryResult<GetUserDto>();

            var users = _context.Set<User>()
                .AsNoTracking()
                .Include(x => x.Role)
                .AsQueryable();

            var search = request.Search?.ToLower();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(x =>
                    x.Email.Contains(search) ||
                    x.LastName.Contains(search) ||
                    x.FirstName.Contains(search) ||
                    x.Role.Name.StartsWith(search));
            }

            if (request.RoleId != null && request.RoleId != 0)
            {
                users = users.Where(x => x.RoleId == request.RoleId);
            }

            users = Sort(users, request.SortBy, request.SortAscending);

            result.Items = await users
                .ProjectTo<GetUserDto>()
                .Skip((request.Query.PageNumber - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .ToListAsync();

            result.TotalItemCount = await users.CountAsync();
            result.PageCount = (result.TotalItemCount + request.Query.PageSize - 1) / request.Query.PageSize;
            result.PageNumber = request.Query.PageNumber;
            result.PageSize = request.Query.PageSize;

            return result.AsResponse();
        }

        private IQueryable<User> Sort(IQueryable<User> users, string sortBy, bool ascending)
        {
            switch (sortBy)
            {
                default:
                case nameof(GetUserDto.LastName):
                    return ascending
                        ? users.OrderBy(x => x.LastName)
                        : users.OrderByDescending(x => x.LastName);

                case nameof(GetUserDto.RoleName):
                    return ascending
                        ? users.OrderBy(x => x.Role.Name)
                        : users.OrderByDescending(x => x.Role.Name);

                case nameof(GetUserDto.Email):
                    return ascending
                        ? users.OrderBy(x => x.Email)
                        : users.OrderByDescending(x => x.Email);
            }
        }

        public class GetUsersPagedValidator : AbstractValidator<GetAllUsersRequest>
        {
            public GetUsersPagedValidator()
            {
                RuleFor(x => x.Query.PageNumber)
                    .GreaterThan(0);

                RuleFor(x => x.Query.PageSize)
                    .GreaterThan(0);
            }
        }
    }
}
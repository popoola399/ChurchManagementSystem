using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Features.Users.Dtos;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;
using ChurchManagementSystem.Core.Mediator.Decorators;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ChurchManagementSystem.Core.Features.Users
{
    [DataContext]
    [DoNotValidate]
    public class GetAllUsersNameRequest : IRequest<List<GetUsersNameDto>>
    {

    }

    public class GetAllUsersNameHandler : IRequestHandler<GetAllUsersNameRequest, List<GetUsersNameDto>>
    {
        private readonly DataContext _context;

        public GetAllUsersNameHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<List<GetUsersNameDto>>> HandleAsync(GetAllUsersNameRequest request)
        {
            var users = _context.Set<User>()
                .AsNoTracking();

            var result = await users
                 .ProjectTo<GetUsersNameDto>()
                 .ToListAsync();

            return result.AsResponse();
        }
    }
}
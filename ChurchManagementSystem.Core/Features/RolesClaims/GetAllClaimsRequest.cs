using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;
using ChurchManagementSystem.Core.Mediator.Decorators;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Features.RolesClaims
{
    [DataContext]
    [DoNotValidate]
    public class GetAllClaimsRequest : IRequest<IEnumerable<ClaimDto>>
    {
    }

    public class GetAllClaimsHandler : IRequestHandler<GetAllClaimsRequest, IEnumerable<ClaimDto>>
    {
        private readonly DataContext _context;

        public GetAllClaimsHandler(DataContext context)
        {
            _context = context;
        }

        public Task<Response<IEnumerable<ClaimDto>>> HandleAsync(GetAllClaimsRequest request)
        {
            var claims = _context.Set<Domain.Authorization.Claim>().ProjectTo<ClaimDto>().ToList();

            return claims.AsEnumerable().AsResponseAsync();
        }
    }
}
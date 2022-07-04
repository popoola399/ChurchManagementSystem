using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Extensions;
using ChurchManagementSystem.Core.Features.Users.Dtos;
using ChurchManagementSystem.Core.Features.Users.Utility;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;
using ChurchManagementSystem.Core.Mediator.Decorators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.Users
{
    [DataContext]
    [DoNotValidate]
    public class GetUserByIdRequest : IRequest<GetUserDto>
    {
        public GetUserByIdRequest(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
    }

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserDto>
    {
        private readonly DataContext _context;

        public GetUserByIdHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<GetUserDto>> HandleAsync(GetUserByIdRequest request)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .Where(x => x.Id == request.UserId)
                .SingleOrDefaultAsync();

            if (user == null)
                return new Error { ErrorMessage = UserConstants.ErrorMessages.UserNotFoundWithID }.AsResponse<GetUserDto>();

            return user.MapTo<GetUserDto>().AsResponse();
        }

        public class GetUserByIdValidator : AbstractValidator<GetUserByIdRequest>
        {
            public GetUserByIdValidator()
            {
                RuleFor(x => x.UserId).GreaterThan(0);
            }
        }
    }
}
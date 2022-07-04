using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Features.Users.Utility;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Decorators;
using ChurchManagementSystem.Core.Security;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ChurchManagementSystem.Core.Features.Users
{
    [DataContext]
    public class DeleteUserRequest : IRequest
    {
        public DeleteUserRequest(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
    }

    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest>
    {
        private readonly DataContext _context;
        private readonly IIdentityUserContext _identity;

        public DeleteUserHandler(DataContext context, IIdentityUserContext identity)
        {
            _context = context;
            _identity = identity;
        }

        public async Task<Response> HandleAsync(DeleteUserRequest request)
        {
            var user = await _context.Set<Domain.User>()
                .SingleOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
                return Response.Error(UserConstants.ErrorMessages.UserNotFoundWithID);

            user.Active = false;

            await _context.SaveChangesAsync();

            return Response.Success();
        }
    }

    public class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
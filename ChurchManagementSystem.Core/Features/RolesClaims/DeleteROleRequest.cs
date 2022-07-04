using System.Linq;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Data;
using ChurchManagementSystem.Core.Features.RolesClaims.Utility;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Decorators;
using FluentValidation;

namespace ChurchManagementSystem.Core.Features.RolesClaims
{
    [DataContext]
    public class DeleteRoleRequest : IRequest
    {
        public DeleteRoleRequest(int roleId)
        {
            RoleId = roleId;
        }

        public int RoleId { get; private set; }
    }

    public class DeleteRoleHandler : IRequestHandler<DeleteRoleRequest>
    {
        private readonly DataContext _context;

        public DeleteRoleHandler(DataContext context)
        {
            _context = context;
        }

        public Task<Response> HandleAsync(DeleteRoleRequest request)
        {
            var role = _context.Set<Domain.Authorization.Role>()
                .SingleOrDefault(x => x.RoleId == request.RoleId);

            if (role == null)
                return Response.ErrorAsync(RoleConstants.ErrorMessages.RoleNotFoundWithID);

            _context.RoleClaims.RemoveRange(_context.RoleClaims.Where(x => x.RoleId == request.RoleId));
            _context.SaveChanges();

            _context.Roles.Remove(role);
            _context.SaveChanges();

            return Response.SuccessAsync();
        }
    }

    public class DeleteRoleValidator : AbstractValidator<DeleteRoleRequest>
    {
        public DeleteRoleValidator()
        {
            RuleFor(x => x.RoleId).GreaterThan(0);
        }
    }
}
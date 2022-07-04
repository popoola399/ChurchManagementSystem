using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Mediator
{
    public interface IMediator
    {
        Task<Response> HandleAsync(IRequest request);

        Task<Response<TResult>> HandleAsync<TResult>(IRequest<TResult> request);
    }
}
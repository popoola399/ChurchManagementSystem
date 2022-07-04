using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Mediator
{
    public interface IRequestHandler<in TRequest> where TRequest : IRequest
    {
        Task<Response> HandleAsync(TRequest request);
    }

    public interface IRequestHandler<in TRequest, TResult> where TRequest : IRequest<TResult>
    {
        Task<Response<TResult>> HandleAsync(TRequest request);
    }
}
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Mediator.Attributes;

using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Features.Utilities
{
    [DoNotLog]
    [DoNotValidate]
    public class CloneObjectRequest<T> : IRequest<T> where T : class
    {
        public T Source { get; set; }
        public T Destination { get; set; }
    }

    public class CloneObjectRequestHandler<T> : IRequestHandler<CloneObjectRequest<T>, T> where T : class
    {
        public Task<Response<T>> HandleAsync(CloneObjectRequest<T> request)
        {
            var propertiesNew = request.Destination.GetType().GetProperties();
            var propertiesOut = request.Source.GetType().GetProperties();
            var i = 0;
            foreach (var objProperty in propertiesNew)
            {
                objProperty.SetValue(request.Destination, propertiesOut[i].GetValue(request.Source));
                i++;
            }
            return request.Destination.AsResponseAsync();
        }
    }
}
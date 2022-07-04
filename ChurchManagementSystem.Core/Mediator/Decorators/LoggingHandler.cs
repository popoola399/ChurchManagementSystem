using ChurchManagementSystem.Core.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChurchManagementSystem.Core.Mediator.Decorators
{
    public class LoggingHandler<TRequest> : IRequestHandler<TRequest> where TRequest : IRequest
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IRequestHandler<TRequest> _inner;

        public LoggingHandler(ILogger<TRequest> logger, IRequestHandler<TRequest> inner)
        {
            _logger = logger;
            _inner = inner;
        }

        public Task<Response> HandleAsync(TRequest request)
        {
            var response = _inner.HandleAsync(request);

            var logResult = new LoggingResult<TRequest>
            {
                Errors = response.Result.Errors,
                Data = response.Result
            };

            var jsonAsString = JsonConvert.SerializeObject(logResult, Formatting.Indented);

            return response;
        }
    }

    public class LoggingHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult> where TRequest : IRequest<TResult>
    {
        private readonly IRequestHandler<TRequest, TResult> _inner;
        private readonly ILogger<TRequest> _logger;

        public LoggingHandler(IRequestHandler<TRequest, TResult> inner,
            ILogger<TRequest> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public Task<Response<TResult>> HandleAsync(TRequest request)
        {
            var response = _inner.HandleAsync(request);

            var logResult = new LoggingResult<TRequest, TResult>
            {
                Errors = response.Result.Errors,
                Data = response.Result.Data
            };

            var jsonAsString = JsonConvert.SerializeObject(logResult, Formatting.Indented);

            return response;
        }
    }

    public class LoggingResult<TRequest>
    {
        public List<Error> Errors { get; set; }
        public Response Data { get; set; }
    }

    public class LoggingResult<TRequest, TResult>
    {
        public List<Error> Errors { get; set; }
        public TResult Data { get; set; }
    }
}
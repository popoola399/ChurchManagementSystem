using ChurchManagementSystem.Core.Mediator;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Net;
using System.Threading.Tasks;

namespace ChurchManagementSystem.API.Controllers
{
    [ApiController]
    public class BaseApiController : Controller
    {
        public IMediator Mediator { get; set; }

        protected async Task<RedirectResult> RedirectHandleAsync<T>(IRequest<T> request, HttpStatusCode successCode = HttpStatusCode.OK)
        {
            var response = await Mediator.HandleAsync(request);
            if (response.HasErrors)
                return Redirect("");
            return Redirect(Convert.ToString(response.Data));
        }

        protected async Task<IActionResult> HandleAsync(IRequest request, HttpStatusCode successCode = HttpStatusCode.OK)
        {
            var response = await Mediator.HandleAsync(request);
            return HandleResult(response, successCode);
        }

        protected async Task<IActionResult> HandleAsync<T>(IRequest<T> request, HttpStatusCode successCode = HttpStatusCode.OK)
        {
            var response = await Mediator.HandleAsync(request);
            return HandleResult(response, successCode);
        }

        private IActionResult HandleResult(Response response, HttpStatusCode successCode)
        {
            return response.HasErrors ? BadRequest(response) : StatusCode((int)successCode, response);
        }

        protected async Task<IActionResult> HandleAsyncAuthorized<T>(IRequest<T> request, HttpStatusCode successCode = HttpStatusCode.OK)
        {
            var response = await Mediator.HandleAsync(request);
            return response.HasErrors ? Unauthorized(response) : StatusCode((int)successCode, response);
        }

        protected async Task<IActionResult> HandleAsync(string message, HttpStatusCode successCode)
        {
            return BadRequest(new Error { ErrorMessage = message }.AsResponse());
        }
    }
}
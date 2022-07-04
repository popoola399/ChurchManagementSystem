using ChurchManagementSystem.Core.Common;
using ChurchManagementSystem.Core.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChurchManagementSystem.Api.Filters
{
    public class AuthorizationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is AuthorizationFailedException))
            {
                return;
            }

            context.Result = new ObjectResult(new
            {
                code = 403,
                hasErrors = true,
                message = !string.IsNullOrEmpty(context.Exception.Message) ? context.Exception.Message : Constants.Auth.UnauthorizedUser
            })
            {
                StatusCode = 403
            };
             
            context.Exception = null;
        }
    }
}
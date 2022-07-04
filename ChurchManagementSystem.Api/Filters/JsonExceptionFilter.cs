using ChurchManagementSystem.Core.Exceptions;
using ChurchManagementSystem.Core.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace ChurchManagementSystem.API.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HandleException(context,
                context.Exception is SimpleInjector.ActivationException
                    ? context.Exception.InnerException
                    : context.Exception);
        }

        private void HandleException(ExceptionContext context, Exception exception)
        {
            var response = new Response
            {
                Errors = new List<Error>
                {
                    new Error
                    {
                        ErrorMessage = exception?.Message ?? "Unauthorized."
                    }
                }
            };

            if (exception is AuthorizationFailedException)
            {
                context.Result = new ObjectResult(new
                {
                    errors = response.Errors,
                    hasErrors = response.HasErrors
                })
                {
                    StatusCode = 403
                };
            }
            else
            {
                context.Result = new ObjectResult(new
                {
                    errors = response.Errors,
                    hasErrors = response.HasErrors
                })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
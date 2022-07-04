
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChurchManagementSystem.Api.Filters
{
    public class FileUploadFilter : IOperationFilter
    {
        private const string formDataMimeType = "multipart/form-data";

        private static readonly string[] formFilePropertyNames = typeof(IFormFile)
            .GetTypeInfo()
            .DeclaredProperties
            .Select(p => p.Name)
            .ToArray();

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null || operation.Parameters.Count == 0)
                return;

            var formFileParameters = context
                .ApiDescription
                .ActionDescriptor
                .Parameters
                .Where(x => x.ParameterType == typeof(IFormFile))
                .ToList();

            if (!formFileParameters.Any())
                return;

            var operationParams = operation.Parameters.ToArray();

            foreach (var parameter in operationParams)
            {
                if (formFilePropertyNames.Contains(parameter.Name))
                    operation.Parameters.Remove(parameter);
            }

            foreach (var formFileParameter in formFileParameters)
            {
                operation.RequestBody = new OpenApiRequestBody()
                {
                    Content =
                    {
                       {"multipart/form-data", new OpenApiMediaType
                            {
                                Schema =
                                {
                                    Type = "object",
                                    Properties =
                                    {
                                        {"FileName",  new OpenApiSchema{Title = formFileParameter.Name, Description = "Upload File", Type = "string", Format = "binary" } }
                                    }
                                }
                            } }
                        }
                };
            }
        }

        //public void Apply(Operation operation, OperationFilterContext context)
        //{
        //    var fileParams = context.MethodInfo.GetParameters()
        //        .Where(p => p.ParameterType.FullName.Equals(typeof(Microsoft.AspNetCore.Http.IFormFile).FullName));

        //    if (fileParams.Any() && fileParams.Count() == 1)
        //    {
        //        operation.Parameters = new List<IParameter>
        //        {
        //            new NonBodyParameter
        //            {
        //                Name = fileParams.First().Name,
        //                Required = true,
        //                Type = "file",
        //                In = "formData"
        //            }
        //        };
        //    }
        //}
    }
}
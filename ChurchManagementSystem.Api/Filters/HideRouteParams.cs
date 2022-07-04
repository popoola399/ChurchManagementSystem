
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

using System.Linq;

namespace ChurchManagementSystem.Api.Filters
{
    //public class HideRouteParams : IOperationFilter
    //{
    //    public void Apply(Operation operation, OperationFilterContext context)
    //    {
    //        if (operation?.Parameters == null)
    //        {
    //            return;
    //        }

    //        var pathParams = operation.Parameters.Where(x => x.In == "path").ToList();
    //        foreach (var operationParameter in operation.Parameters.ToList())
    //        {
    //            var bodyParameter = operationParameter as BodyParameter;
    //            var parameterName = bodyParameter?.Schema?.Ref?.Replace("#/definitions/", string.Empty);
    //            if (parameterName == null)
    //            {
    //                continue;
    //            }

    //            var schema = context.SchemaRegistry.Definitions[parameterName];
    //            foreach (var schemaProperty in schema.Properties.ToList())
    //            {
    //                if (pathParams.Any(x => x.Name == schemaProperty.Key))
    //                {
    //                    schema.Properties.Remove(schemaProperty);
    //                }
    //            }
    //        }
    //    }
    //}
}
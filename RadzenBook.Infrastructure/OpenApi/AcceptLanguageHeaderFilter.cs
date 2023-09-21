using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RadzenBook.Infrastructure.OpenApi;

public class AcceptLanguageHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Description = "Language support (English and Vietnamese)",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "enum",
                Default = new OpenApiString("en-US"),
                Enum = new List<IOpenApiAny>
                {
                    new OpenApiString("en-US"),
                    new OpenApiString("vi-VN")
                }
            },
        });
    }
}
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace GlowGirlsBackend.Swagger;

public class ClientSecretTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(
            new OpenApiParameter
            {
                Name = "GlowGirls-Client-Secret",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "Client secret header required for API access.",
            }
        );

        return Task.FromResult(operation);
    }
}

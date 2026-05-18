using Asp.Versioning;
using Asp.Versioning.Builder;
using Ollama_Chatbot_with_RAG_API.Endpoints;

namespace Ollama_Chatbot_with_RAG_API.Extensions;

public static class EndpointsExtensions
{
    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .HasApiVersion(new ApiVersion(2.0))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder versionedGroup = app
             .MapGroup("/api/v{apiVersion:apiVersion}")
             .WithApiVersionSet(apiVersionSet);

        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(versionedGroup);
        }

        return app;
    }
}
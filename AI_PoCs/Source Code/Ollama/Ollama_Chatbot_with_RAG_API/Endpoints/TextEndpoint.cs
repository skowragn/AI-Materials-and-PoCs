using Ollama_Chatbot_with_RAG_API.Repositories;
using Ollama_Chatbot_with_RAG_API.Services;

namespace Ollama_Chatbot_with_RAG_API.Endpoints;

public class TextEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // POST /add-text + JSON body with a "content" field
        app.MapPost("/add-text", async (TextRepository textRepository, HttpContext context) =>
        {
            var request = await context.Request.ReadFromJsonAsync<AddTextRequest>();
            if (string.IsNullOrWhiteSpace(request?.Content))
            {
                return Results.BadRequest("Content is required.");
            }

            await textRepository.StoreTextAsync(request.Content);

            return Results.Ok("Text added successfully.");
        });

        // GET /ask-text?query=
        app.MapGet("/ask-text", async (RagService ragService, string query) =>
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Results.BadRequest("Query parameter is required.");
            }

            var response = await ragService.GetAnswerAsync(query);

            return Results.Ok(new { query, response });
        });
    }
}

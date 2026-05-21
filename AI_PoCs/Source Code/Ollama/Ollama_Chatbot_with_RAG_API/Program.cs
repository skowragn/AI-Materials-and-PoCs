using Ollama_Chatbot_with_RAG_API.EmbeddingGenerator;
using Ollama_Chatbot_with_RAG_API.Extensions;
using Ollama_Chatbot_with_RAG_API.Repositories;
using Ollama_Chatbot_with_RAG_API.Services;

namespace Ollama_Chatbot_with_RAG_API;

// run llama3 model 
// docker exec -it ollama ollama pull mistral

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHttpClient();
        
        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString("PostgreSQL");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Required configuration settings are missing.");
        }

        builder.Services.AddSingleton<IEmbeddingGenerator>(sp => new OllamaEmbeddingGenerator(sp.GetRequiredService<IHttpClientFactory>(), new Uri("http://localhost:11434"), "mistral"));

        builder.Services.AddSingleton(sp => new TextRepository(connectionString, sp.GetRequiredService<IEmbeddingGenerator>()));

        builder.Services.AddSingleton(sp => new RagService(sp.GetRequiredService<IHttpClientFactory>(), sp.GetRequiredService<TextRepository>(), new Uri("http://localhost:11434"), "mistral"));

        var app = builder.Build();

        app.MapEndpoints();

        app.UseHttpsRedirection();

        app.Run();
    }
}
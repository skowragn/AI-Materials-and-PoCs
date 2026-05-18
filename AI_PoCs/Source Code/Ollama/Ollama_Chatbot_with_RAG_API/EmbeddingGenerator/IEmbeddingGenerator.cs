
namespace Ollama_Chatbot_with_RAG_API.EmbeddingGenerator;

public interface IEmbeddingGenerator
{
    Task<float[]> GenerateEmbeddingAsync(string text);
}

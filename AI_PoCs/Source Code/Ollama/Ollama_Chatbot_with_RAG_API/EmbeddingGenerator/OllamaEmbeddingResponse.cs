using System.Text.Json.Serialization;

namespace Ollama_Chatbot_with_RAG_API.EmbeddingGenerator;

public class OllamaEmbeddingResponse
{
    [JsonPropertyName("embedding")]
    public float[] Embedding { get; set; } = [];
}

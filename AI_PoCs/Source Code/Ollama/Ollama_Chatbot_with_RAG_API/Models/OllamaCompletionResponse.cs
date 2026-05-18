
using System.Text.Json.Serialization;

namespace Ollama_Chatbot_with_RAG_API.Models;

public class OllamaCompletionResponse
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = string.Empty;

    [JsonPropertyName("context")]
    public List<int> Context { get; set; } = [];
}
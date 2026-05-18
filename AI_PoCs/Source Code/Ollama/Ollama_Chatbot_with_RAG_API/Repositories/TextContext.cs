namespace Ollama_Chatbot_with_RAG_API.Repositories;

public class TextContext
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public float[] Embedding { get; set; } = [];
}
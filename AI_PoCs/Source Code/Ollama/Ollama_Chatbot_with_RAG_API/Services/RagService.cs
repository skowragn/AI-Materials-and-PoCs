using Ollama_Chatbot_with_RAG_API.Repositories;
using Ollama_Chatbot_with_RAG_API.Models;
using System.Text;
using System.Text.Json;

namespace Ollama_Chatbot_with_RAG_API.Services;

public class RagService(TextRepository retriever, Uri ollamaUrl, string modelId = "mistral", IHttpClientFactory httpClientFactory)
{
    private readonly TextRepository _textRepository = retriever;
    private readonly Uri _ollamaUrl = ollamaUrl;
    private readonly string _modelId = modelId;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<object> GetAnswerAsync(string query)
    {
        var httpClient = _httpClientFactory.CreateClient();
        
        var contexts = await _textRepository.RetrieveRelevantText(query);
        string combinedContext = string.Join("\n\n---\n\n", contexts);

        if (contexts.Count == 1 && contexts[0] == "No relevant context found.")
        {
            return new
            {
                Context = "No relevant data found in the database.",
                Response = "No relevant data found."
            };
        }

        var requestBody = new
        {
            model = _modelId,
            prompt = $"""
        You are an AI assistant. You MUST answer ONLY using the provided context. 
        If the answer is not in the context, respond with "No relevant data found."

        Context:
        {combinedContext}

        Question: {query}
        """,
            stream = false
        };

        var response = await httpClient.PostAsync(new Uri(_ollamaUrl, "/api/generate"),
                                                  new StringContent(JsonSerializer.Serialize(requestBody), 
                                                  Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            return new
            {
                Context = combinedContext,
                Response = "Error: Unable to generate response."
            };
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        JsonSerializerOptions serializationOptions = new() { PropertyNameCaseInsensitive = true };
        var completionResponse = JsonSerializer.Deserialize<OllamaCompletionResponse>(responseJson, serializationOptions);

        return new
        {
            Context = combinedContext,
            Response = completionResponse?.Response ?? "No relevant data found."
        };
    }
}

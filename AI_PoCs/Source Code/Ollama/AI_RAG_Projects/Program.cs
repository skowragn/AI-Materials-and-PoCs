using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaSharp;

// run llama3 model 
// docker exec -it ollama ollama pull llama3      

var builder = Host.CreateApplicationBuilder();

builder.Services.AddChatClient(new OllamaApiClient(new Uri("http://localhost:11434"), "llama3"));

var app = builder.Build();

var chatClient = app.Services.GetRequiredService<IChatClient>();
var chatCompletion = await chatClient.GetResponseAsync("What is AI? Reply in 20 words.");

Console.WriteLine(chatCompletion.Text);

var chatHistory = new List<ChatMessage>();

while (true)
{
    Console.WriteLine("Your prompt:");
    var userPrompt = Console.ReadLine();
    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

    Console.WriteLine("AI Response:");
    var chatResponse = "";

    await foreach (var item in chatClient.GetStreamingResponseAsync(chatHistory))
    {
        Console.Write(item.Text);
        chatResponse += item.Text;
    }

    chatHistory.Add(new ChatMessage(ChatRole.Assistant, chatResponse));
    Console.WriteLine();
}
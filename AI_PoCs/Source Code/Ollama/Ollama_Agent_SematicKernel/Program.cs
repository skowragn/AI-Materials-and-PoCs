using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Ollama_Agent_SematicKernel;

// run llama3.2:3b model 
// docker exec -it ollama ollama pull llama3.2:3b

var builder = Kernel.CreateBuilder();
builder.AddOllamaChatCompletion("llama3.2:3b", new Uri("http://localhost:11434/"));
builder.Plugins.AddFromType<GeneralPlugin>();

var kernel = builder.Build();

var chatCompletionAgent = new ChatCompletionAgent
{
    Name = "TimeAgent",
    Description = "Agent with the current date and time.",
    Instructions = "You should only reply on questions related to the current date and time.",
    Kernel = kernel,
    Arguments = new KernelArguments(new OllamaPromptExecutionSettings
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
    })
};

var agentThread = new ChatHistoryAgentThread();

while (true)
{
    Console.Write("User:");
    var request = Console.ReadLine();
    agentThread.ChatHistory.AddUserMessage(request!);

    string fullMessage = "";
    Console.Write("Assistant:");

    await foreach (var response in chatCompletionAgent.InvokeAsync(agentThread))
    {
        Console.Write(response.Message.Content);
        fullMessage += response.Message.Content;
    }

    Console.WriteLine();

    agentThread.ChatHistory.AddAssistantMessage(fullMessage);
}
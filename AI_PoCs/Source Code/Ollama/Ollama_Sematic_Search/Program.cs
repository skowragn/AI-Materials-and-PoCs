using System.Numerics.Tensors;
using Microsoft.Extensions.AI; 
using OllamaSharp;


// run all-minilm model 
// docker exec -it ollama ollama pull all-minilm

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = new OllamaApiClient(new Uri("http://localhost:11434"), "all-minilm");

var a = embeddingGenerator;

var blogPostTitles = new[]
{
    "AI_Architecture/AI_Basic",
    "AI_Architecture/Aurora_Agentic_AI",
    "AI_Architecture/AI Costs_and_Tokenomics",
    "AI_Architecture/AI Agents Orchestration Patterns",
    "AI_Architecture/AI Agents Hosting with Infrustructure Orchestration",
    "AI_PoCs/AI_Solutions_and_Frameworks",
    "AI_PoCs/RAG",
    "AI_PoCs/Semantic_Kernel",
    "Trainings&Meetups/AI-Trainings-and-Meetups",
    "Trainings&Meetups/AI_anddotNet",

};

Console.WriteLine("Generating embeddings for blog post titles...");

var candidateEmbeddings = await embeddingGenerator.GenerateAndZipAsync(blogPostTitles); //returns a list of (Value, Embedding) pairs,
Console.WriteLine("Embeddings generated successfully.");

while (true)
{
    Console.WriteLine("\nEnter your query (or press Enter to exit):");
    var userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
    {
        break;
    }

    var userEmbedding = await embeddingGenerator.GenerateAsync(userInput);

    var topMatches = candidateEmbeddings
        .Select(candidate => new
        {
            Text = candidate.Value,
            Similarity = TensorPrimitives.CosineSimilarity(candidate.Embedding.Vector.Span, userEmbedding.Vector.Span)
        })
        .OrderByDescending(match => match.Similarity)
        .Take(1);

    Console.WriteLine("\nTop matching blog post titles:");
    foreach (var match in topMatches)
    {
        Console.WriteLine($"Similarity: {match.Similarity:F4} - {match.Text}");
    }
}

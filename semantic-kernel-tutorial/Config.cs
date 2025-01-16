namespace SemanticKernelTutorial;

using System;
using OllamaSharp;

public static class Config
{
    private const string OllamaApiLocalHostname = "localhost";
    private const string OllamaApiDockerHostname = "host.docker.internal";

    public static readonly string OllamaApiUrl =
      Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
      ? $"http://{OllamaApiDockerHostname}:11434"
      : $"http://{OllamaApiLocalHostname}:11434";

    public static readonly string LanguageModel = "llama3.2:3b";

    public static readonly string EmbeddingsModel = "nomic-embed-text";

    public static async Task GetModels()
    {
        using var ollama = new OllamaApiClient(OllamaApiUrl);
        await foreach (var status in ollama.PullModelAsync(LanguageModel))
            Console.WriteLine($"{status?.Percent}% {status?.Status}");

        await foreach (var status in ollama.PullModelAsync(EmbeddingsModel))
            Console.WriteLine($"{status?.Percent}% {status?.Status}");
    }
}

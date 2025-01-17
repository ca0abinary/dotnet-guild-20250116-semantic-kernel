namespace SemanticKernelTutorial;

using System;
using System.Net.Sockets;
using OllamaSharp;

public static class Config
{
    private const string OllamaApiLocalHostname = "localhost";
    private const string OllamaApiDockerHostname = "host.docker.internal";

    public static string OllamaApiUrl { get; private set; }

    public static readonly string LanguageModel = "llama3.2:3b";

    public static readonly string EmbeddingsModel = "nomic-embed-text";

    static Config()
    {
        OllamaApiUrl = "http://localhost:11434";

        try
        {
            var tcp = new TcpClient
            {
                SendTimeout = 100
            };
            
            tcp.Connect("host.docker.internal", 11434);
            
            OllamaApiUrl = "host.docker.internal";
        } catch { }
    }

    public static async Task GetModels()
    {
        using var ollama = new OllamaApiClient(OllamaApiUrl);
        await foreach (var status in ollama.PullModelAsync(LanguageModel))
            Console.WriteLine($"{status?.Percent}% {status?.Status}");

        await foreach (var status in ollama.PullModelAsync(EmbeddingsModel))
            Console.WriteLine($"{status?.Percent}% {status?.Status}");
    }
}

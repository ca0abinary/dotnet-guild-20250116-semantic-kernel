using Microsoft.Extensions.AI;

namespace SemanticKernelTutorial;

public static partial class Tutorial
{
    public static async Task GettingStarted()
    {
        // Get the models we need
        Config.GetModels().Wait();
        Console.WriteLine("Models downloaded");

        Console.WriteLine($"{Environment.NewLine}🤔 Test a question to the model");
        using IChatClient client = new OllamaChatClient(new Uri(Config.OllamaApiUrl), Config.LanguageModel);
        await foreach (var update in client.CompleteStreamingAsync(
        [
            new ChatMessage(ChatRole.System, @"
                You are an expert on the works of Douglas Adams.
                Answer all questions using only quotations from his books and include citation.
                Do not include additional material outside of the answer and citation."),
            new ChatMessage(ChatRole.User, "What is the meaning of life the universe and everything?"),
        ]))
        {
            Console.Write(update);
        }
        Console.WriteLine();

        Console.WriteLine($"{Environment.NewLine}📈 Beyond the Basics");
        // Now that we've seen how to do a request and response, how does a chat work?
        // It's very similar but we have three personas now:
        // 1. The system (only one prompt needed)
        // 2. The user (that's you!)
        // 3. The assistant (the persona created by the system prompt)

        // Confusing? Let's clarify with an example:
        await foreach (var update in client.CompleteStreamingAsync(
        [
            new ChatMessage(ChatRole.System, @"Answer any question put to you with a quote from one of the works of Douglas Adams."),
            new ChatMessage(ChatRole.User, "What is the meaning of life the universe and everything?"),
            new ChatMessage(ChatRole.Assistant, "Forty-two"),
            new ChatMessage(ChatRole.User, "In what book and chapter can I find this quote?"),
        ]))
        {
            Console.Write(update);
        }
        Console.WriteLine();
    }
}

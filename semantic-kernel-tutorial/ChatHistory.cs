using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.ChatCompletion;

namespace SemanticKernelTutorial;
#pragma warning disable SKEXP0001

public static partial class Tutorial
{
  public static async Task ChatHistory()
  {
    ChatHistory chatHistory = [
        new () { Role = AuthorRole.System, Content = @"Answer any question put to you with a quote from one of the works of Douglas Adams." },
        new () { Role = AuthorRole.User, Content = "What is the meaning of life the universe and everything? Include citation." },
        new () { Role = AuthorRole.Assistant, Content = "Forty-two" },
        new () { Role = AuthorRole.User, Content = "In what book and chapter can I find this quote?" },
    ];

    var historyLength = chatHistory.Count;
    using IChatClient client = new OllamaChatClient(new Uri(Config.OllamaApiUrl), Config.LanguageModel);
    var results = await client.AsChatCompletionService().GetChatMessageContentAsync(chatHistory);

    // Get the new messages added to the chat history object
    for (int i = historyLength; i < chatHistory.Count; i++)
    {
      Console.WriteLine(chatHistory[i]);
    }

    // Print the final message
    Console.WriteLine(results);

    // Add the final message to the chat history object
    chatHistory.Add(results);
  }
}


using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SemanticKernelTutorial;

public static partial class Tutorial
{
    public static async Task FunctionCalling()
    {
#pragma warning disable SKEXP0070

        var kernelBuilder = Kernel
          .CreateBuilder()
          .AddOllamaChatCompletion(Config.LanguageModel, new Uri(Config.OllamaApiUrl));

#pragma warning restore SKEXP0070

        kernelBuilder.Services.AddSingleton<IPizzaService, PizzaService>();
        kernelBuilder.Plugins.AddFromType<OrderPizzaPlugin>("OrderPizza");

        var kernel = kernelBuilder.Build();
        var chatCompletion = kernel.GetRequiredService<IChatCompletionService>();
        var promptSettings = new OpenAIPromptExecutionSettings { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

        ChatHistory chatHistory = [];
        chatHistory.AddSystemMessage(@"
    You a a pizza ordering service.
    Greet the customer and add pizzas to their cart as they make requests.
    All pizzas are free of charge and need to be picked up in person, delivery is not available.
    Summarize the order details when complete, including the order id.");
        chatHistory.AddMessageWithEcho("Can you help me get a free pizza?");
        chatHistory.AddMessageWithEcho(await chatCompletion.GetChatMessageContentAsync(chatHistory, promptSettings, kernel));
        chatHistory.AddMessageWithEcho("I'd like a medium pizza with cheese and pepperoni");
        chatHistory.AddMessageWithEcho(await chatCompletion.GetChatMessageContentAsync(chatHistory, promptSettings, kernel));
        chatHistory.AddMessageWithEcho("Everything looks good. Please place the order. Thanks!");
        chatHistory.AddMessageWithEcho(await chatCompletion.GetChatMessageContentAsync(chatHistory, promptSettings, kernel));

        var allOrders = kernel.GetRequiredService<IPizzaService>().Orders;
        foreach (var order in allOrders)
        {
            Console.WriteLine($"Order ID: {order.OrderId}");
            Console.WriteLine($"Size: {order.Size}");
            Console.WriteLine($"Toppings: {order.Toppings}");
            Console.WriteLine($"Quantity: {order.Quantity}");
            Console.WriteLine($"Special Instructions: {order.SpecialInstructions}");
        }
    }

    private static void AddMessageWithEcho(this ChatHistory chatHistory, ChatMessageContent messageContent)
    {
        chatHistory.Add(messageContent);
        Console.WriteLine($"{messageContent.Role}: {messageContent.Content}");
    }

    private static void AddMessageWithEcho(this ChatHistory chatHistory, string messageContent)
    {
        chatHistory.AddUserMessage(messageContent);
        Console.WriteLine($"{AuthorRole.User}: {messageContent}");
    }
}

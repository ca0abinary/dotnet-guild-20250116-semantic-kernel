using System.ComponentModel;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SemanticKernelTutorial;
#pragma warning disable SKEXP0010

public static partial class Tutorial
{
    public static async Task FormattedOutput()
    {
        var openAiFormattedUrl = $"{Config.OllamaApiUrl}/v1";
        var kernel = Kernel
          .CreateBuilder()
          .AddOpenAIChatCompletion(Config.LanguageModel, new Uri(openAiFormattedUrl), "ollama")
          .Build();

        var result = await kernel.InvokePromptAsync(
            "You are a service that generates a random fantasy character for a novel.",
            new(new OpenAIPromptExecutionSettings { ResponseFormat = typeof(MyGreatNovelCharacter) }));

        var resultObject = JsonSerializer.Deserialize<MyGreatNovelCharacter>(result.ToString());

        Console.WriteLine(JsonSerializer.Serialize(resultObject));
    }

    public struct MyGreatNovelCharacter
    {
        [Description("The character's name")]
        public string Name { get; set; }
        [Description("The character's background story")]
        public string BackgroundStory { get; set; }
        [Description("The name of the town the character lives in")]
        public string Town { get; set; }
        [Description("The character's age")]
        public int Age { get; set; }
        [Description("The job the character has in town")]
        public string Occupation { get; set; }
        [Description("The character's race")]
        public string Race { get; set; }
        [Description("Names of the character's friends")]
        public List<string> Friends { get; set; }
        [Description("The character's personality traits")]
        public List<string> PersonalityTraits { get; set; }
    }
}

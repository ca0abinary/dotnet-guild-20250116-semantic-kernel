using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using SemanticKernelTutorial.Models;

namespace SemanticKernelTutorial;
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0070

public static partial class Tutorial
{
    public static async Task RetrievalAugmentedGeneration()
    {
        var ollamaLlm = new OllamaApiClient(Config.OllamaApiUrl, Config.LanguageModel);
        var ollamaEmbeddings = new OllamaApiClient(Config.OllamaApiUrl, Config.EmbeddingsModel);

        var kernel = Kernel
            .CreateBuilder()
            .AddInMemoryVectorStore()
            .AddInMemoryVectorStoreRecordCollection<Guid, MarkdownArticle>("articles")
            .AddOllamaTextEmbeddingGeneration(ollamaEmbeddings)
            .AddOllamaChatCompletion(ollamaLlm)
            .Build();

        var collection = await BuildCollection(kernel, @"C:\src\Admin.wiki");

        await PerformSearchAsync(kernel, "How do I write a new advanced search in HuB?");
    }

    static async Task<IVectorStoreRecordCollection<Guid, MarkdownArticle>> BuildCollection(Kernel kernel, string markdownCollectionPath)
    {
        var collection = kernel.GetRequiredService<IVectorStoreRecordCollection<Guid, MarkdownArticle>>();
        await collection.CreateCollectionIfNotExistsAsync();

        System.Diagnostics.Stopwatch sw = new();
        sw.Start();

        var markdownFiles = Directory.GetFiles(markdownCollectionPath, "*.md", SearchOption.AllDirectories);
        var tasks = markdownFiles.Select(md => Task.Run(async () =>
        {
            var markdown = await File.ReadAllTextAsync(md);
            var embedding = await kernel.GetRequiredService<ITextEmbeddingGenerationService>().GenerateEmbeddingAsync(markdown);
            await collection.UpsertAsync(new MarkdownArticle { Path = md, ArticleContents = markdown, Embedding = embedding });
        }));

        await Task.WhenAll(tasks);

        sw.Stop();
        Console.WriteLine($"Generated collection from {markdownCollectionPath} in {sw.Elapsed}");

        return collection;
    }

    static async Task PerformSearchAsync(Kernel kernel, string searchString)
    {
        var collection = kernel.GetRequiredService<IVectorStoreRecordCollection<Guid, MarkdownArticle>>();
        var searchVector = await kernel.GetRequiredService<ITextEmbeddingGenerationService>().GenerateEmbeddingAsync(searchString);
        var searchResult = await collection.VectorizedSearchAsync(searchVector, new() { Top = 1 });
        var resultRecords = await searchResult.Results.ToListAsync();

        await foreach(var token in kernel.InvokePromptStreamingAsync($"""
            <message role="system">
                You are an AI agent that searches a collection of markdown articles for answers to questions.
                The following article was returned form a vector search. Use the information to generate a response.

                Markdown document: {resultRecords.First().Record.ArticleContents}
                Markdown document path: {resultRecords.First().Record.Path}

                Include only citations from the article above in your response and provide the document path.
            </message>
            <message role="user">
                {searchString}
            </message>
            """))
            {
                Console.Write(token);
            }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Query: {searchString}");
        Console.WriteLine($"Top article result was: {resultRecords.First().Record.Path}");
    }
}

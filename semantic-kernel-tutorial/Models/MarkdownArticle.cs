using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Data;

namespace SemanticKernelTutorial.Models;
#pragma warning disable SKEXP0001

public sealed class MarkdownArticle
{
    [VectorStoreRecordKey]
    [TextSearchResultName]
    public Guid Key { get; set; } = Guid.NewGuid();

    [VectorStoreRecordData]
    [TextSearchResultLink]
    public string Path { get; set; } = string.Empty;

    [VectorStoreRecordData]
    [TextSearchResultValue]
    public string ArticleContents { get; set; } = string.Empty;

    [VectorStoreRecordVector(1536)]
    public ReadOnlyMemory<float> Embedding { get; init; }
}

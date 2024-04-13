// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.Razor.Snippets;

internal record SnippetInfo
{
    Lazy<XmlSnippetParser.ParsedXmlSnippet?> _parsedXmlSnippet;

    public SnippetInfo(
        string shortcut,
        string title,
        string description,
        string path,
        SnippetLanguage language)
    {
        Shortcut = shortcut;
        Title = title;
        Description = description;
        Path = path;
        Language = language;

        _parsedXmlSnippet = new(() => XmlSnippetParser.GetParsedXmlSnippet(this));
        CompletionData = new(Path);
    }

    public string Shortcut { get; }
    public string Title { get; }
    public string Description { get; }
    public string Path { get; }
    public SnippetLanguage Language { get; }
    public SnippetCompletionData CompletionData { get; }

    internal XmlSnippetParser.ParsedXmlSnippet? GetParsedXmlSnippet()
        => _parsedXmlSnippet.Value;
}

internal record SnippetCompletionData([property: JsonProperty(SnippetCompletionData.PropertyName)] string Path)
{
    private const string PropertyName = "__razor_snippet_path";

    internal static bool TryParse(object? data, [NotNullWhen(true)] out SnippetCompletionData? snippetCompletionData)
    {
        snippetCompletionData = data as SnippetCompletionData;

        if (data is JObject jObject &&
            jObject.Property(PropertyName) is not null)
        {
            try
            {
                snippetCompletionData = jObject.ToObject<SnippetCompletionData>();
            }
            catch { }
        }

        return snippetCompletionData is not null;
    }

    internal bool Matches(SnippetInfo s)
    {
        return s.Path == Path;
    }
}

internal enum SnippetLanguage
{
    CSharp,
    Html,
    Razor
}

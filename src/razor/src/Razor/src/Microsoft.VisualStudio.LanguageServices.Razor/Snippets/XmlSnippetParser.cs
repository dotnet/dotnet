// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.CodeAnalysis.Razor.Logging;

namespace Microsoft.VisualStudio.Razor.Snippets;

internal static partial class XmlSnippetParser
{
    internal static ParsedXmlSnippet? GetParsedXmlSnippet(SnippetInfo matchingSnippetInfo, ILogger? logger = null)
    {
        ParsedXmlSnippet? parsedSnippet = null;
        try
        {
            logger?.LogInformation($"Reading snippet for {matchingSnippetInfo.Title} with path {matchingSnippetInfo.Path}");
            parsedSnippet = GetAndParseSnippetFromFile(matchingSnippetInfo);
        }
        catch (Exception ex)
        {
            logger?.LogCritical(ex, $"Got exception parsing xml snippet {matchingSnippetInfo.Title} from file {matchingSnippetInfo.Path}");
        }

        return parsedSnippet;
    }

    private static ParsedXmlSnippet GetAndParseSnippetFromFile(SnippetInfo snippetInfo)
    {
        // Read the XML file to get the snippet and snippet metadata.
        var matchingSnippet = RetrieveSnippetXmlFromFile(snippetInfo);
        if (!matchingSnippet.IsExpansionSnippet())
        {
            throw new InvalidOperationException("Only expansion snippets are supported");
        }

        var expansion = new ExpansionTemplate(matchingSnippet);

        // Parse the snippet XML into snippet parts we can cache.
        var parsedSnippet = expansion.Parse();
        return parsedSnippet;
    }

    private static CodeSnippet RetrieveSnippetXmlFromFile(SnippetInfo snippetInfo)
    {
        var path = snippetInfo.Path;
        if (path == null)
        {
            throw new ArgumentException($"Missing file path for snippet {snippetInfo.Title}");
        }

        if (!File.Exists(path))
        {
            throw new InvalidOperationException($"Snippet {snippetInfo.Title} has an invalid file path: {snippetInfo.Path}");
        }

        // Load the xml for the snippet from disk.
        // Any exceptions thrown here we allow to bubble up and let the queue log it.
        var snippet = CodeSnippet.ReadSnippetFromFile(snippetInfo.Path, snippetInfo.Title);
        return snippet;
    }
}

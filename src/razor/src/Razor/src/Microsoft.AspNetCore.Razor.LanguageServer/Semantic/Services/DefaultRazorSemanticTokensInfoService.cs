// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class DefaultRazorSemanticTokensInfoService : RazorSemanticTokensInfoService
{
    private const int TokenSize = 5;

    private readonly RazorDocumentMappingService _documentMappingService;
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly ILogger _logger;

    public DefaultRazorSemanticTokensInfoService(
        ClientNotifierServiceBase languageServer,
        RazorDocumentMappingService documentMappingService,
        ILoggerFactory loggerFactory)
    {
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _logger = loggerFactory.CreateLogger<DefaultRazorSemanticTokensInfoService>();
    }

    public override async Task<SemanticTokens?> GetSemanticTokensAsync(
        TextDocumentIdentifier textDocumentIdentifier,
        Range range,
        DocumentContext documentContext,
        CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        var razorSemanticRanges = TagHelperSemanticRangeVisitor.VisitAllNodes(codeDocument, range);
        IReadOnlyList<SemanticRange>? csharpSemanticRanges = null;

        try
        {
            csharpSemanticRanges = await GetCSharpSemanticRangesAsync(
                codeDocument, textDocumentIdentifier, range, documentContext.Version, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error thrown while retrieving CSharp semantic range.");
        }

        var combinedSemanticRanges = CombineSemanticRanges(razorSemanticRanges, csharpSemanticRanges);

        // We return null when we have an incomplete view of the document.
        // Likely CSharp ahead of us in terms of document versions.
        // We return null (which to the LSP is a no-op) to prevent flashing of CSharp elements.
        if (combinedSemanticRanges is null)
        {
            _logger.LogWarning("Incomplete view of document. C# may be ahead of us in document versions.");
            return null;
        }

        var data = ConvertSemanticRangesToSemanticTokensData(combinedSemanticRanges, codeDocument);
        var tokens = new SemanticTokens { Data = data };

        return tokens;
    }

    private static IReadOnlyList<SemanticRange>? CombineSemanticRanges(params IReadOnlyList<SemanticRange>?[] rangesArray)
    {
        if (rangesArray.Any(a => a is null))
        {
            // If we have an incomplete view of the situation we should return null so we avoid flashing.
            return null;
        }

        var newList = new List<SemanticRange>();
        foreach (var list in rangesArray)
        {
            if (list != null)
            {
                newList.AddRange(list);
            }
        }

        // Because SemanticToken data is generated relative to the previous token it must be in order.
        // We have a guarantee of order within any given language server, but the interweaving of them can be quite complex.
        // Rather than attempting to reason about transition zones we can simply order our ranges since we know there can be no overlapping range.
        newList.Sort();

        return newList;
    }

    // Internal and virtual for testing only
    internal virtual async Task<SemanticRange[]?> GetCSharpSemanticRangesAsync(
        RazorCodeDocument codeDocument,
        TextDocumentIdentifier textDocumentIdentifier,
        Range razorRange,
        long documentVersion,
        CancellationToken cancellationToken,
        string? previousResultId = null)
    {
        // We'll try to call into the mapping service to map to the projected range for us. If that doesn't work,
        // we'll try to find the minimal range ourselves.
        if (!_documentMappingService.TryMapToProjectedDocumentRange(codeDocument, razorRange, out var csharpRange) &&
            !TryGetMinimalCSharpRange(codeDocument, razorRange, out csharpRange))
        {
            // There's no C# in the range.
            return Array.Empty<SemanticRange>();
        }

        var csharpResponse = await GetMatchingCSharpResponseAsync(textDocumentIdentifier, documentVersion, csharpRange, cancellationToken);

        // Indicates an issue with retrieving the C# response (e.g. no response or C# is out of sync with us).
        // Unrecoverable, return default to indicate no change. We've already queued up a refresh request in
        // `GetMatchingCSharpResponseAsync` that will cause us to retry in a bit.
        if (csharpResponse is null)
        {
            _logger.LogWarning("Issue with retrieving C# response for Razor range: {razorRange}", razorRange);
            return null;
        }

        var razorRanges = new List<SemanticRange>();

        SemanticRange? previousSemanticRange = null;
        for (var i = 0; i < csharpResponse.Length; i += TokenSize)
        {
            var lineDelta = csharpResponse[i];
            var charDelta = csharpResponse[i + 1];
            var length = csharpResponse[i + 2];
            var tokenType = csharpResponse[i + 3];
            var tokenModifiers = csharpResponse[i + 4];

            var semanticRange = DataToSemanticRange(
                lineDelta, charDelta, length, tokenType, tokenModifiers, previousSemanticRange);
            if (_documentMappingService.TryMapFromProjectedDocumentRange(codeDocument, semanticRange.Range, out var originalRange))
            {
                var razorSemanticRange = new SemanticRange(semanticRange.Kind, originalRange, tokenModifiers);
                if (razorRange is null || razorRange.OverlapsWith(razorSemanticRange.Range))
                {
                    razorRanges.Add(razorSemanticRange);
                }
            }

            previousSemanticRange = semanticRange;
        }

        var result = razorRanges.ToArray();
        return result;
    }

    // Internal for testing only
    internal static bool TryGetMinimalCSharpRange(RazorCodeDocument codeDocument, Range razorRange, [NotNullWhen(true)] out Range? csharpRange)
    {
        SourceSpan? minGeneratedSpan = null;
        SourceSpan? maxGeneratedSpan = null;

        var sourceText = codeDocument.GetSourceText();
        var textSpan = razorRange.AsTextSpan(sourceText);
        var csharpDoc = codeDocument.GetCSharpDocument();

        // We want to find the min and max C# source mapping that corresponds with our Razor range.
        foreach (var mapping in csharpDoc.SourceMappings)
        {
            var mappedTextSpan = mapping.OriginalSpan.AsTextSpan();

            if (textSpan.OverlapsWith(mappedTextSpan))
            {
                if (minGeneratedSpan is null || mapping.GeneratedSpan.AbsoluteIndex < minGeneratedSpan.Value.AbsoluteIndex)
                {
                    minGeneratedSpan = mapping.GeneratedSpan;
                }

                var mappingEndIndex = mapping.GeneratedSpan.AbsoluteIndex + mapping.GeneratedSpan.Length;
                if (maxGeneratedSpan is null || mappingEndIndex > maxGeneratedSpan.Value.AbsoluteIndex + maxGeneratedSpan.Value.Length)
                {
                    maxGeneratedSpan = mapping.GeneratedSpan;
                }
            }
        }

        // Create a new projected range based on our calculated min/max source spans.
        if (minGeneratedSpan is not null && maxGeneratedSpan is not null)
        {
            var csharpSourceText = codeDocument.GetCSharpSourceText();
            var startRange = minGeneratedSpan.Value.AsRange(csharpSourceText);
            var endRange = maxGeneratedSpan.Value.AsRange(csharpSourceText);

            csharpRange = new Range { Start = startRange.Start, End = endRange.End };
            Debug.Assert(csharpRange.Start.CompareTo(csharpRange.End) <= 0, "Range.Start should not be larger than Range.End");

            return true;
        }

        csharpRange = null;
        return false;
    }

    private async Task<int[]?> GetMatchingCSharpResponseAsync(
        TextDocumentIdentifier textDocumentIdentifier,
        long documentVersion,
        Range csharpRange,
        CancellationToken cancellationToken)
    {
        var parameter = new ProvideSemanticTokensRangeParams(textDocumentIdentifier, documentVersion, csharpRange);

        var csharpResponse = await _languageServer.SendRequestAsync<ProvideSemanticTokensRangeParams, ProvideSemanticTokensResponse>(
            RazorLanguageServerCustomMessageTargets.RazorProvideSemanticTokensRangeEndpoint,
            parameter,
            cancellationToken);

        if (csharpResponse is null)
        {
            // C# isn't ready yet, don't make Razor wait for it. Once C# is ready they'll send a refresh notification.
            return Array.Empty<int>();
        }
        else if (csharpResponse.HostDocumentSyncVersion != null && csharpResponse.HostDocumentSyncVersion != documentVersion)
        {
            // No C# response or C# is out of sync with us. Unrecoverable, return null to indicate no change.
            // Once C# syncs up they'll send a refresh notification.
            return null;
        }

        var response = csharpResponse.Tokens ?? Array.Empty<int>();
        return response;
    }

    private static SemanticRange DataToSemanticRange(
        int lineDelta,
        int charDelta,
        int length,
        int tokenType,
        int tokenModifiers,
        SemanticRange? previousSemanticRange = null)
    {
        if (previousSemanticRange is null)
        {
            var previousRange = new Range
            {
                Start = new Position(0, 0),
                End = new Position(0, 0)
            };
            previousSemanticRange = new SemanticRange(0, previousRange, modifier: 0);
        }

        var startLine = previousSemanticRange.Range.End.Line + lineDelta;
        var previousEndChar = lineDelta == 0 ? previousSemanticRange.Range.Start.Character : 0;
        var startCharacter = previousEndChar + charDelta;
        var start = new Position(startLine, startCharacter);

        var endLine = startLine;
        var endCharacter = startCharacter + length;
        var end = new Position(endLine, endCharacter);

        var range = new Range()
        {
            Start = start,
            End = end
        };
        var semanticRange = new SemanticRange(tokenType, range, tokenModifiers);

        return semanticRange;
    }

    private static int[] ConvertSemanticRangesToSemanticTokensData(
        IReadOnlyList<SemanticRange> semanticRanges,
        RazorCodeDocument razorCodeDocument)
    {
        SemanticRange? previousResult = null;

        var data = new int[semanticRanges.Count * TokenSize];
        var semanticRangeCount = 0;
        foreach (var result in semanticRanges)
        {
            AppendData(result, previousResult, razorCodeDocument, data, semanticRangeCount);
            semanticRangeCount += TokenSize;

            previousResult = result;
        }

        return data;

        // We purposely capture and manipulate the "data" array here to avoid allocation
        static void AppendData(
            SemanticRange currentRange,
            SemanticRange? previousRange,
            RazorCodeDocument razorCodeDocument,
            int[] targetArray,
            int currentCount)
        {
            /*
             * In short, each token takes 5 integers to represent, so a specific token `i` in the file consists of the following array indices:
             *  - at index `5*i`   - `deltaLine`: token line number, relative to the previous token
             *  - at index `5*i+1` - `deltaStart`: token start character, relative to the previous token (relative to 0 or the previous token's start if they are on the same line)
             *  - at index `5*i+2` - `length`: the length of the token. A token cannot be multiline.
             *  - at index `5*i+3` - `tokenType`: will be looked up in `SemanticTokensLegend.tokenTypes`
             *  - at index `5*i+4` - `tokenModifiers`: each set bit will be looked up in `SemanticTokensLegend.tokenModifiers`
            */

            // deltaLine
            var previousLineIndex = previousRange?.Range is null ? 0 : previousRange.Range.Start.Line;
            targetArray[currentCount] = currentRange.Range.Start.Line - previousLineIndex;

            // deltaStart
            if (previousRange != null && previousRange?.Range.Start.Line == currentRange.Range.Start.Line)
            {
                targetArray[currentCount + 1] = currentRange.Range.Start.Character - previousRange.Range.Start.Character;
            }
            else
            {
                targetArray[currentCount + 1] = currentRange.Range.Start.Character;
            }

            // length
            var textSpan = currentRange.Range.AsTextSpan(razorCodeDocument.GetSourceText());
            var length = textSpan.Length;
            Debug.Assert(length > 0);
            targetArray[currentCount + 2] = length;

            // tokenType
            targetArray[currentCount + 3] = currentRange.Kind;

            // tokenModifiers
            // We don't currently have any need for tokenModifiers
            targetArray[currentCount + 4] = currentRange.Modifier;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class RazorSemanticTokensInfoService : IRazorSemanticTokensInfoService
{
    private const int TokenSize = 5;

    private readonly IRazorDocumentMappingService _documentMappingService;
    private readonly RazorLSPOptionsMonitor _razorLSPOptionsMonitor;
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly ILogger _logger;

    public RazorSemanticTokensInfoService(
        ClientNotifierServiceBase languageServer,
        IRazorDocumentMappingService documentMappingService,
        RazorLSPOptionsMonitor razorLSPOptionsMonitor,
        ILoggerFactory loggerFactory)
    {
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _razorLSPOptionsMonitor = razorLSPOptionsMonitor ?? throw new ArgumentNullException(nameof(razorLSPOptionsMonitor));

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _logger = loggerFactory.CreateLogger<RazorSemanticTokensInfoService>();
    }

    public async Task<SemanticTokens?> GetSemanticTokensAsync(
        TextDocumentIdentifier textDocumentIdentifier,
        Range range,
        VersionedDocumentContext documentContext,
        RazorSemanticTokensLegend razorSemanticTokensLegend,
        Guid correlationId,
        CancellationToken cancellationToken)
    {
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();
        List<SemanticRange>? razorSemanticRanges = null;
        PooledObject<List<SemanticRange>>? csharpSemanticRanges = null;
        
        try
        {
            var csharpSemanticRangesTask = GetCSharpSemanticRangesAsync(
                codeDocument, textDocumentIdentifier, range, razorSemanticTokensLegend, documentContext.Version, correlationId, cancellationToken);

            razorSemanticRanges = TagHelperSemanticRangeVisitor.VisitAllNodes(codeDocument, range, razorSemanticTokensLegend, _razorLSPOptionsMonitor.CurrentValue.ColorBackground);
            csharpSemanticRanges = await csharpSemanticRangesTask.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error thrown while retrieving CSharp semantic range.");
        }

        if (cancellationToken.IsCancellationRequested)
        {
            if (csharpSemanticRanges.HasValue)
            {
                csharpSemanticRanges.Value.Dispose();
            }

            return null;
        }

        using var combinedSemanticRanges = CombineSemanticRanges(razorSemanticRanges, csharpSemanticRanges is not null ? csharpSemanticRanges.Value.Object : null);

        // We return null when we have an incomplete view of the document.
        // Likely CSharp ahead of us in terms of document versions.
        // We return null (which to the LSP is a no-op) to prevent flashing of CSharp elements.
        if (combinedSemanticRanges is null || !combinedSemanticRanges.HasValue)
        {
            _logger.LogWarning("Incomplete view of document. C# may be ahead of us in document versions.");

            if (csharpSemanticRanges.HasValue)
            {
                csharpSemanticRanges.Value.Dispose();
            }

            return null;
        }

        var data = ConvertSemanticRangesToSemanticTokensData(combinedSemanticRanges.Value.Object, codeDocument);
        var tokens = new SemanticTokens { Data = data };

        if (csharpSemanticRanges.HasValue)
        {
            csharpSemanticRanges.Value.Dispose();
        }

        return tokens;
    }

    private static PooledObject<List<SemanticRange>>? CombineSemanticRanges(List<SemanticRange>? razorRanges, List<SemanticRange>? csharpRanges)
    {
        if (razorRanges is null || csharpRanges is null)
        {
            // If we have an incomplete view of the situation we should return null so we avoid flashing.
            return null;
        }

        // Because SemanticToken data is generated relative to the previous token it must be in order.
        // We have a guarantee of order within any given language server, but when we translate from the ranges
        // in the C# document to ranges into the Razor document we lose that guarantee.
        // Having converted them to SemanticRange objects, we can simply do the final round of a merge sort.
        var pooledList = ListPool<SemanticRange>.GetPooledObject(out var newList);
        newList.SetCapacityIfLarger(razorRanges.Count + csharpRanges.Count);

        var indexRazor = 0;
        var indexCsharp = 0;
        while (indexRazor < razorRanges.Count && indexCsharp < csharpRanges.Count)
        {
            var currentRazorRange = razorRanges[indexRazor];
            var currentCsharpRange = csharpRanges[indexCsharp];

            var comparison = currentRazorRange.CompareTo(currentCsharpRange);

            if (comparison == 0)
            {
                // csharp and razor ranges have the same span; skip the C# item
                ++indexCsharp;
            }
            else if (comparison < 0)
            {
                newList.Add(currentRazorRange);
                ++indexRazor;
            }
            else
            {
                newList.Add(currentCsharpRange);
                ++indexCsharp;
            }
        }

        while (indexRazor < razorRanges.Count)
        {
            newList.Add(razorRanges[indexRazor]);
            ++indexRazor;
        }

        while (indexCsharp < csharpRanges.Count)
        {
            newList.Add(csharpRanges[indexCsharp]);
            ++indexCsharp;
        }

#if DEBUG
        // Verify that the result of merging above matches the old algorithm of doing a 'full sort'.        
        using var _2 = ListPool<SemanticRange>.GetPooledObject(out var fullSortList);

        fullSortList.AddRange(razorRanges);
        fullSortList.AddRange(csharpRanges);

        fullSortList.Sort((left, right) =>
        {
            var rangeCompare = left.CompareTo(right);
            if (rangeCompare != 0)
            {
                return rangeCompare;
            }

            // If we have ranges that are the same, we want a Razor produced token to win over a non-Razor produced token
            if (left.FromRazor && !right.FromRazor)
            {
                return -1;
            }
            else if (right.FromRazor && !left.FromRazor)
            {
                return 1;
            }

            return 0;
        });

        for (var idx = 1; idx < fullSortList.Count; ++idx)
        {
            if (fullSortList[idx].CompareTo(fullSortList[idx - 1]) == 0)
            {
                fullSortList.RemoveAt(idx);
                idx--;
            }
        }

        Debug.Assert(fullSortList.Count == newList.Count,$"After sort and removing duplicates (favoring Razor over C#), the lists should be equal size.  Fast algorithm: ${newList.Count} Full sort: ${fullSortList.Count}");

        for (var idx = 0; idx < fullSortList.Count; ++idx)
        {
            Debug.Assert(Equals(newList[idx], fullSortList[idx]), $"Difference between the full sort and the merge of sorted lists at index ${idx}");
        }
#endif

        return pooledList;
    }

    /// <summary>
    /// Returns a sorted list of SemanticRange objects representing the semantic info
    /// for the C# regions of the Razor file.
    ///
    /// Internal and virtual to enable testing; for running product, no need to override.
    /// </summary>
    /// <returns>List of Semantic range sorted by position</returns>
    internal virtual async Task<PooledObject<List<SemanticRange>>?> GetCSharpSemanticRangesAsync(
        RazorCodeDocument codeDocument,
        TextDocumentIdentifier textDocumentIdentifier,
        Range razorRange,
        RazorSemanticTokensLegend razorSemanticTokensLegend,
        long documentVersion,
        Guid correlationId,
        CancellationToken cancellationToken,
        string? previousResultId = null)
    {
        // We'll try to call into the mapping service to map to the projected range for us. If that doesn't work,
        // we'll try to find the minimal range ourselves.
        var generatedDocument = codeDocument.GetCSharpDocument();
        if (!_documentMappingService.TryMapToGeneratedDocumentRange(generatedDocument, razorRange, out var csharpRange) &&
            !TryGetMinimalCSharpRange(codeDocument, razorRange, out csharpRange))
        {
            // There's no C# in the range.
            return ListPool<SemanticRange>.GetPooledObject();
        }

        // We expect that the response is sorted already.
        var csharpResponse = await GetMatchingCSharpResponseAsync(textDocumentIdentifier, documentVersion, csharpRange, correlationId, cancellationToken).ConfigureAwait(false);

        // Indicates an issue with retrieving the C# response (e.g. no response or C# is out of sync with us).
        // Unrecoverable, return default to indicate no change. We've already queued up a refresh request in
        // `GetMatchingCSharpResponseAsync` that will cause us to retry in a bit.
        if (csharpResponse is null)
        {
            _logger.LogWarning("Issue with retrieving C# response for Razor range: {razorRange}", razorRange);
            return null;
        }

        var colorBackground = _razorLSPOptionsMonitor.CurrentValue.ColorBackground;
        var textClassification = razorSemanticTokensLegend.MarkupTextLiteral;
        var razorSource = codeDocument.GetSourceText();
        var pooledRanges = ListPool<SemanticRange>.GetPooledObject(out var razorRanges);

        SemanticRange? previousSemanticRange = null;
        Range? previousRazorSemanticRange = null;
        for (var i = 0; i < csharpResponse.Length; i += TokenSize)
        {
            var lineDelta = csharpResponse[i];
            var charDelta = csharpResponse[i + 1];
            var length = csharpResponse[i + 2];
            var tokenType = csharpResponse[i + 3];
            var tokenModifiers = csharpResponse[i + 4];

            var semanticRange = CSharpDataToSemanticRange(lineDelta, charDelta, length, tokenType, tokenModifiers, previousSemanticRange);
            if (_documentMappingService.TryMapToHostDocumentRange(generatedDocument, semanticRange.Range, out var originalRange))
            {
                if (razorRange is null || razorRange.OverlapsWith(originalRange))
                {
                    if (colorBackground)
                    {
                        tokenModifiers |= (int)RazorSemanticTokensLegend.RazorTokenModifiers.RazorCode;
                        AddAdditionalCSharpWhitespaceRanges(razorRanges, textClassification, razorSource, previousRazorSemanticRange, originalRange, _logger);
                    }

                    razorRanges.Add(new SemanticRange(semanticRange.Kind, originalRange, tokenModifiers, fromRazor: false));
                }

                previousRazorSemanticRange = originalRange;
            }

            previousSemanticRange = semanticRange;
        }

        // The ranges are sorted by the C# document, but we need to sort them by the Razor
        // document since we mapped all the ranges and the mapping does not necessarily
        // maintain the ordering.
        razorRanges.Sort();

        return pooledRanges;
    }

    private static void AddAdditionalCSharpWhitespaceRanges(List<SemanticRange> razorRanges, int textClassification, SourceText razorSource, Range? previousRazorSemanticRange, Range originalRange, ILogger logger)
    {
        var startChar = originalRange.Start.Character;
        if (previousRazorSemanticRange is not null &&
            previousRazorSemanticRange.End.Line == originalRange.Start.Line &&
            previousRazorSemanticRange.End.Character < originalRange.Start.Character &&
            previousRazorSemanticRange.End.TryGetAbsoluteIndex(razorSource, logger, out var previousSpanEndIndex) &&
            ContainsOnlySpacesOrTabs(razorSource, previousSpanEndIndex + 1, startChar - previousRazorSemanticRange.End.Character - 1))
        {
            // we're on the same line as previous, lets extend ours to include whitespace between us and the proceeding range
            var whitespaceRange = new Range
            {
                Start = new Position(originalRange.Start.Line, previousRazorSemanticRange.End.Character),
                End = originalRange.Start
            };
            razorRanges.Add(new SemanticRange(textClassification, whitespaceRange, (int)RazorSemanticTokensLegend.RazorTokenModifiers.RazorCode, fromRazor: false));
        }
        else if (originalRange.Start.Character > 0 &&
            previousRazorSemanticRange?.End.Line != originalRange.Start.Line &&
            originalRange.Start.TryGetAbsoluteIndex(razorSource, logger, out var originalRangeStartIndex) &&
            ContainsOnlySpacesOrTabs(razorSource, originalRangeStartIndex - startChar + 1, startChar - 1))
        {
            // We're on a new line, and the start of the line is only whitespace, so give that a background color too
            var whitespaceRange = new Range
            {
                Start = new Position(originalRange.Start.Line, 0),
                End = originalRange.Start
            };
            razorRanges.Add(new SemanticRange(textClassification, whitespaceRange, (int)RazorSemanticTokensLegend.RazorTokenModifiers.RazorCode, fromRazor:false));
        }
    }

    private static bool ContainsOnlySpacesOrTabs(SourceText razorSource, int startIndex, int count)
    {
        var end = startIndex + count;
        for (var i = startIndex; i < end; i++)
        {
            if (razorSource[i] is not ' ' or '\t')
            {
                return false;
            }
        }

        return true;
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
        Guid correlationId,
        CancellationToken cancellationToken)
    {
        var parameter = new ProvideSemanticTokensRangeParams(textDocumentIdentifier, documentVersion, csharpRange, correlationId);

        var csharpResponse = await _languageServer.SendRequestAsync<ProvideSemanticTokensRangeParams, ProvideSemanticTokensResponse>(
            RazorLanguageServerCustomMessageTargets.RazorProvideSemanticTokensRangeEndpoint,
            parameter,
            cancellationToken).ConfigureAwait(false);

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

    private static SemanticRange CSharpDataToSemanticRange(
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
            previousSemanticRange = new SemanticRange(0, previousRange, modifier: 0, fromRazor: false);
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
        var semanticRange = new SemanticRange(tokenType, range, tokenModifiers, fromRazor:false);

        return semanticRange;
    }

    private static int[] ConvertSemanticRangesToSemanticTokensData(
        List<SemanticRange> semanticRanges,
        RazorCodeDocument razorCodeDocument)
    {
        SemanticRange? previousResult = null;

        var sourceText = razorCodeDocument.GetSourceText();

        if (sourceText is null)
        {
            return Array.Empty<int>();
        }

        var data = new int[semanticRanges.Count * TokenSize];
        var semanticRangeCount = 0;

        foreach (var result in semanticRanges)
        {
            AppendData(result, previousResult, sourceText, data, semanticRangeCount);
            semanticRangeCount += TokenSize;

            previousResult = result;
        }

        return data;

        // We purposely capture and manipulate the "data" array here to avoid allocation
        static void AppendData(
            SemanticRange currentRange,
            SemanticRange? previousRange,
            SourceText sourceText,
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
            var deltaLine = currentRange.Range.Start.Line - previousLineIndex;

            int deltaStart;
            if (previousRange != null && previousRange?.Range.Start.Line == currentRange.Range.Start.Line)
            {
                deltaStart = currentRange.Range.Start.Character - previousRange.Range.Start.Character;

                // If there is no line delta, no char delta, and this isn't the first range (ie, previousRange is not null)
                // then it means this range overlaps the previous, so we skip it.
                if (deltaStart == 0)
                {
                    return;
                }
            }
            else
            {
                deltaStart = currentRange.Range.Start.Character;
            }

            targetArray[currentCount] = deltaLine;

            // deltaStart
            targetArray[currentCount + 1] = deltaStart;

            // length
            var textSpan = currentRange.Range.AsTextSpan(sourceText);
            var length = textSpan.Length;
            Debug.Assert(length > 0);
            targetArray[currentCount + 2] = length;

            // tokenType
            targetArray[currentCount + 3] = currentRange.Kind;

            // tokenModifiers
            targetArray[currentCount + 4] = currentRange.Modifier;
        }
    }
}

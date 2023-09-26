// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class RazorSemanticTokensInfoService : IRazorSemanticTokensInfoService
{
    private const int TokenSize = 5;

    private readonly IRazorDocumentMappingService _documentMappingService;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly RazorLSPOptionsMonitor _razorLSPOptionsMonitor;
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly ILogger _logger;

    public RazorSemanticTokensInfoService(
        ClientNotifierServiceBase languageServer,
        IRazorDocumentMappingService documentMappingService,
        RazorLSPOptionsMonitor razorLSPOptionsMonitor,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        ILoggerFactory loggerFactory)
    {
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _razorLSPOptionsMonitor = razorLSPOptionsMonitor ?? throw new ArgumentNullException(nameof(razorLSPOptionsMonitor));
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));

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
        var razorSemanticRanges = TagHelperSemanticRangeVisitor.VisitAllNodes(codeDocument, range, razorSemanticTokensLegend, _razorLSPOptionsMonitor.CurrentValue.ColorBackground);
        ImmutableArray<SemanticRange>? csharpSemanticRangesResult = null;

        try
        {
            csharpSemanticRangesResult = await GetCSharpSemanticRangesAsync(codeDocument, textDocumentIdentifier, range, razorSemanticTokensLegend, documentContext.Version, correlationId, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error thrown while retrieving CSharp semantic range.");
        }

        // Didn't get any C# tokens, likely because the user kept typing and a future semantic tokens request will occur.
        // We return null (which to the LSP is a no-op) to prevent flashing of CSharp elements.
        if (csharpSemanticRangesResult is not { } csharpSemanticRanges)
        {
            _logger.LogDebug("Couldn't get C# tokens for version {version} of {doc}. Returning null", documentContext.Version, textDocumentIdentifier.Uri);
            return null;
        }

        var combinedSemanticRanges = CombineSemanticRanges(razorSemanticRanges, csharpSemanticRanges);

        var data = ConvertSemanticRangesToSemanticTokensData(combinedSemanticRanges, codeDocument);
        var tokens = new SemanticTokens { Data = data };

        return tokens;
    }

    private static ImmutableArray<SemanticRange> CombineSemanticRanges(ImmutableArray<SemanticRange> razorRanges, ImmutableArray<SemanticRange> csharpRanges)
    {
        Debug.Assert(razorRanges.SequenceEqual(razorRanges.OrderBy(g => g)));

        // If there are no C# in what we're trying to classify we don't need to do anything special since we know the razor ranges will be sorted
        // because we use a visitor to create them, and the above Assert will validate it in our tests.
        if (csharpRanges.Length == 0)
        {
            return razorRanges;
        }

        // If there are no Razor ranges then we can't just return the C# ranges, as they ranges are not necessarily sorted. They would have been
        // in order when the C# server gave them to us, but the data we have here is after re-mapping to the Razor document, which can result in
        // things being moved around. We need to sort before we return them.
        if (razorRanges.Length == 0)
        {
            return csharpRanges.Sort();
        }

        // If we have both types of tokens then we need to sort them all together, even though we know the Razor ranges will be sorted already,
        // because they can arbitrarily interleave. The SemanticRange.CompareTo method also has some logic to ensure that if Razor and C# ranges
        // are equivalent, the Razor range will be ordered first, so we can later drop the C# range, and prefer our classification over C#s.
        // Additionally, as mentioned above, the C# ranges are not guaranteed to be in order
        using var _ = ArrayBuilderPool<SemanticRange>.GetPooledObject(out var newList);
        newList.SetCapacityIfLarger(razorRanges.Length + csharpRanges.Length);

        newList.AddRange(razorRanges);
        newList.AddRange(csharpRanges);

        newList.Sort();

        return newList.DrainToImmutable();
    }

    // Internal and virtual for testing only
    internal virtual async Task<ImmutableArray<SemanticRange>?> GetCSharpSemanticRangesAsync(
        RazorCodeDocument codeDocument,
        TextDocumentIdentifier textDocumentIdentifier,
        Range razorRange,
        RazorSemanticTokensLegend razorSemanticTokensLegend,
        long documentVersion,
        Guid correlationId,
        CancellationToken cancellationToken,
        string? previousResultId = null)
    {
        var generatedDocument = codeDocument.GetCSharpDocument();
        Range[]? csharpRanges;

        // When the feature flag is enabled we try to get a list of precise ranges for the C# code embedded in the Razor document.
        // The feature flag allows to make calls to Roslyn using multiple smaller and disjoint ranges of the document
        if (_languageServerFeatureOptions.UsePreciseSemanticTokenRanges)
        {
            if (!TryGetSortedCSharpRanges(codeDocument, razorRange, out csharpRanges))
            {
                // There's no C# in the range.
                return ImmutableArray<SemanticRange>.Empty;
            }
        }
        else
        {
            // When the feature flag is disabled, we fallback to computing a single range for the entire document.
            // This single range is the minimal range that contains all of the C# code in the document.
            // We'll try to call into the mapping service to map to the projected range for us. If that doesn't work,
            // we'll try to find the minimal range ourselves.
            if (!_documentMappingService.TryMapToGeneratedDocumentRange(generatedDocument, razorRange, out var csharpRange) &&
                !TryGetMinimalCSharpRange(codeDocument, razorRange, out csharpRange))
            {
                // There's no C# in the range.
                return ImmutableArray<SemanticRange>.Empty;
            }

            csharpRanges = new Range[] { csharpRange };
        }

        var csharpResponse = await GetMatchingCSharpResponseAsync(textDocumentIdentifier, documentVersion, csharpRanges, correlationId, cancellationToken).ConfigureAwait(false);

        // Indicates an issue with retrieving the C# response (e.g. no response or C# is out of sync with us).
        // Unrecoverable, return default to indicate no change. We've already queued up a refresh request in
        // the server call that will cause us to retry in a bit.
        if (csharpResponse is null)
        {
            return null;
        }

        using var _ = ArrayBuilderPool<SemanticRange>.GetPooledObject(out var razorRanges);
        razorRanges.SetCapacityIfLarger(csharpResponse.Length / TokenSize);

        var colorBackground = _razorLSPOptionsMonitor.CurrentValue.ColorBackground;
        var textClassification = razorSemanticTokensLegend.MarkupTextLiteral;
        var razorSource = codeDocument.GetSourceText();

        SemanticRange previousSemanticRange = default;
        LinePositionSpan? previousRazorSemanticRange = null;

        for (var i = 0; i < csharpResponse.Length; i += TokenSize)
        {
            var lineDelta = csharpResponse[i];
            var charDelta = csharpResponse[i + 1];
            var length = csharpResponse[i + 2];
            var tokenType = csharpResponse[i + 3];
            var tokenModifiers = csharpResponse[i + 4];

            var semanticRange = CSharpDataToSemanticRange(lineDelta, charDelta, length, tokenType, tokenModifiers, previousSemanticRange);
            if (_documentMappingService.TryMapToHostDocumentRange(generatedDocument, semanticRange.AsLinePositionSpan(), out var originalRange))
            {
                if (razorRange is null || razorRange.ToLinePositionSpan().OverlapsWith(originalRange))
                {
                    if (colorBackground)
                    {
                        tokenModifiers |= (int)RazorSemanticTokensLegend.RazorTokenModifiers.razorCode;
                        AddAdditionalCSharpWhitespaceRanges(razorRanges, textClassification, razorSource, previousRazorSemanticRange, originalRange, _logger);
                    }

                    razorRanges.Add(new SemanticRange(semanticRange.Kind, originalRange.Start.Line, originalRange.Start.Character, originalRange.End.Line, originalRange.End.Character, tokenModifiers, fromRazor: false));
                }

                previousRazorSemanticRange = originalRange;
            }

            previousSemanticRange = semanticRange;
        }

        return razorRanges.DrainToImmutable();
    }

    private static void AddAdditionalCSharpWhitespaceRanges(ImmutableArray<SemanticRange>.Builder razorRanges, int textClassification, SourceText razorSource, LinePositionSpan? previousRazorSemanticRange, LinePositionSpan originalRange, ILogger logger)
    {
        var startLine = originalRange.Start.Line;
        var startChar = originalRange.Start.Character;
        if (previousRazorSemanticRange is { } previousRange &&
            previousRange.End.Line == startLine &&
            previousRange.End.Character < startChar &&
            previousRange.End.TryGetAbsoluteIndex(razorSource, logger, out var previousSpanEndIndex) &&
            ContainsOnlySpacesOrTabs(razorSource, previousSpanEndIndex + 1, startChar - previousRange.End.Character - 1))
        {
            // we're on the same line as previous, lets extend ours to include whitespace between us and the proceeding range
            razorRanges.Add(new SemanticRange(textClassification, startLine, previousRange.End.Character, startLine, startChar, (int)RazorSemanticTokensLegend.RazorTokenModifiers.razorCode, fromRazor: false));
        }
        else if (startChar > 0 &&
            previousRazorSemanticRange?.End.Line != startLine &&
            originalRange.Start.TryGetAbsoluteIndex(razorSource, logger, out var originalRangeStartIndex) &&
            ContainsOnlySpacesOrTabs(razorSource, originalRangeStartIndex - startChar + 1, startChar - 1))
        {
            // We're on a new line, and the start of the line is only whitespace, so give that a background color too
            razorRanges.Add(new SemanticRange(textClassification, startLine, 0, startLine, startChar, (int)RazorSemanticTokensLegend.RazorTokenModifiers.razorCode, fromRazor: false));
        }
    }

    private static bool ContainsOnlySpacesOrTabs(SourceText razorSource, int startIndex, int count)
    {
        var end = startIndex + count;
        for (var i = startIndex; i < end; i++)
        {
            if (razorSource[i] is not (' ' or '\t'))
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
        var textSpan = razorRange.ToTextSpan(sourceText);
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
            var startRange = minGeneratedSpan.Value.ToRange(csharpSourceText);
            var endRange = maxGeneratedSpan.Value.ToRange(csharpSourceText);

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
        Range[] csharpRanges,
        Guid correlationId,
        CancellationToken cancellationToken)
    {
        var parameter = new ProvideSemanticTokensRangesParams(textDocumentIdentifier, documentVersion, csharpRanges, correlationId);
        ProvideSemanticTokensResponse? csharpResponse;
        if (_languageServerFeatureOptions.UsePreciseSemanticTokenRanges)
        {
            csharpResponse = await GetCsharpResponseAsync(parameter, CustomMessageNames.RazorProvidePreciseRangeSemanticTokensEndpoint, cancellationToken).ConfigureAwait(false);

            // Likely the server doesn't support the new endpoint, fallback to the original one
            if (csharpResponse?.Tokens is null && csharpRanges.Length > 1)
            {
                var minimalRange = new Range
                {
                    Start = csharpRanges[0].Start,
                    End = csharpRanges[^1].End
                };

                var newParams = new ProvideSemanticTokensRangesParams(
                    parameter.TextDocument,
                    parameter.RequiredHostDocumentVersion,
                    new[] { minimalRange },
                    parameter.CorrelationId);

                csharpResponse = await GetCsharpResponseAsync(newParams, CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint, cancellationToken).ConfigureAwait(false);
            }
        }
        else
        {
            csharpResponse = await GetCsharpResponseAsync(parameter, CustomMessageNames.RazorProvideSemanticTokensRangeEndpoint, cancellationToken).ConfigureAwait(false);
        }

        if (csharpResponse is null)
        {
            // C# isn't ready yet, don't make Razor wait for it. Once C# is ready they'll send a refresh notification.
            return Array.Empty<int>();
        }

        var csharpVersion = csharpResponse.HostDocumentSyncVersion;
        if (csharpVersion != documentVersion)
        {
            // No C# response or C# is out of sync with us. Unrecoverable, return null to indicate no change.
            // Once C# syncs up they'll send a refresh notification.
            if (csharpVersion == -1)
            {
                _logger.LogWarning("Didn't get C# tokens because the virtual document wasn't found, or other problem. We were wanting {documentVersion} but C# could not get any version.", documentVersion);
            }
            else if (csharpVersion < documentVersion)
            {
                _logger.LogDebug("Didn't wait for Roslyn to get the C# version we were expecting. We are wanting {documentVersion} but C# is at {csharpVersion}.", documentVersion, csharpVersion);
            }
            else
            {
                _logger.LogWarning("We are behind the C# version which is surprising. Could be an old request that wasn't cancelled, but if not, expect most future requests to fail. We were wanting {documentVersion} but C# is at {csharpVersion}.", documentVersion, csharpVersion);
            }

            return null;
        }

        return csharpResponse.Tokens ?? Array.Empty<int>();
    }

    // Internal for testing only
    internal static bool TryGetSortedCSharpRanges(RazorCodeDocument codeDocument, Range razorRange, [NotNullWhen(true)] out Range[]? ranges)
    {
        using var _ = ListPool<Range>.GetPooledObject(out var csharpRanges);
        var csharpSourceText = codeDocument.GetCSharpSourceText();
        var sourceText = codeDocument.GetSourceText();
        var textSpan = razorRange.ToTextSpan(sourceText);
        var csharpDoc = codeDocument.GetCSharpDocument();

        // We want to find the min and max C# source mapping that corresponds with our Razor range.
        foreach (var mapping in csharpDoc.SourceMappings)
        {
            var mappedTextSpan = mapping.OriginalSpan.AsTextSpan();

            if (textSpan.OverlapsWith(mappedTextSpan))
            {
                var mappedRange = mapping.GeneratedSpan.ToRange(csharpSourceText);
                csharpRanges.Add(mappedRange);
            }
        }

        if (csharpRanges.Count == 0)
        {
            ranges = null;
            return false;
        }

        ranges = csharpRanges.ToArray();
        // Ensure the C# ranges are sorted
        Array.Sort(ranges, static (r1, r2) => r1.CompareTo(r2));
        return true;
    }

    private async Task<ProvideSemanticTokensResponse?> GetCsharpResponseAsync(ProvideSemanticTokensRangesParams parameter, string lspMethodName, CancellationToken cancellationToken)
    {
        return await _languageServer.SendRequestAsync<ProvideSemanticTokensRangesParams, ProvideSemanticTokensResponse>(
            lspMethodName,
            parameter,
            cancellationToken).ConfigureAwait(false);
    }

    private static SemanticRange CSharpDataToSemanticRange(
        int lineDelta,
        int charDelta,
        int length,
        int tokenType,
        int tokenModifiers,
        SemanticRange previousSemanticRange)
    {
        var startLine = previousSemanticRange.EndLine + lineDelta;
        var previousEndChar = lineDelta == 0 ? previousSemanticRange.StartCharacter : 0;
        var startCharacter = previousEndChar + charDelta;

        var endLine = startLine;
        var endCharacter = startCharacter + length;

        var semanticRange = new SemanticRange(tokenType, startLine, startCharacter, endLine, endCharacter, tokenModifiers, fromRazor: false);

        return semanticRange;
    }

    private static int[] ConvertSemanticRangesToSemanticTokensData(
        ImmutableArray<SemanticRange> semanticRanges,
        RazorCodeDocument razorCodeDocument)
    {
        SemanticRange previousResult = default;

        var sourceText = razorCodeDocument.GetSourceText();

        // We don't bother filtering out duplicate ranges (eg, where C# and Razor both have opinions), but instead take advantage of
        // our sort algorithm to be correct, so we can skip duplicates here. That means our final array may end up smaller than the
        // expected size, so we have to use a list to build it.
        using var _ = ListPool<int>.GetPooledObject(out var data);
        data.SetCapacityIfLarger(semanticRanges.Length * TokenSize);

        var firstRange = true;
        foreach (var result in semanticRanges)
        {
            AppendData(result, previousResult, firstRange, sourceText, data);
            firstRange = false;

            previousResult = result;
        }

        return data.ToArray();

        // We purposely capture and manipulate the "data" array here to avoid allocation
        static void AppendData(
            SemanticRange currentRange,
            SemanticRange previousRange,
            bool firstRange,
            SourceText sourceText,
            List<int> data)
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
            var previousLineIndex = previousRange.StartLine;
            var deltaLine = currentRange.StartLine - previousLineIndex;

            int deltaStart;
            if (!firstRange && previousRange.StartLine == currentRange.StartLine)
            {
                deltaStart = currentRange.StartCharacter - previousRange.StartCharacter;

                // If there is no line delta, no char delta, and this isn't the first range
                // then it means this range overlaps the previous, so we skip it.
                if (deltaStart == 0)
                {
                    return;
                }
            }
            else
            {
                deltaStart = currentRange.StartCharacter;
            }

            data.Add(deltaLine);
            data.Add(deltaStart);

            // length

            if (!sourceText.TryGetAbsoluteIndex(currentRange.StartLine, currentRange.StartCharacter, out var startPosition) ||
                !sourceText.TryGetAbsoluteIndex(currentRange.EndLine, currentRange.EndCharacter, out var endPosition))
            {
                throw new ArgumentOutOfRangeException($"Range: All or part of {currentRange} was outside the bounds of the document.");
            }

            var length = endPosition - startPosition;
            Debug.Assert(length > 0);
            data.Add(length);

            // tokenType
            data.Add(currentRange.Kind);

            // tokenModifiers
            data.Add(currentRange.Modifier);
        }
    }
}

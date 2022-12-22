// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

internal class FoldingRangeEndpoint : IVSFoldingRangeEndpoint
{
    private readonly RazorDocumentMappingService _documentMappingService;
    private readonly ClientNotifierServiceBase _languageServer;
    private readonly IEnumerable<RazorFoldingRangeProvider> _foldingRangeProviders;
    private readonly ILogger _logger;

    public bool MutatesSolutionState => false;

    public FoldingRangeEndpoint(
        RazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        IEnumerable<RazorFoldingRangeProvider> foldingRangeProviders,
        ILoggerFactory loggerFactory)
    {
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
        _foldingRangeProviders = foldingRangeProviders ?? throw new ArgumentNullException(nameof(foldingRangeProviders));
        _logger = loggerFactory.CreateLogger<FoldingRangeEndpoint>();
    }

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string AssociatedServerCapability = "foldingRangeProvider";

        var registrationOptions = new SumType<bool, FoldingRangeOptions>(new FoldingRangeOptions());

        return new RegistrationExtensionResult(AssociatedServerCapability, registrationOptions);
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(FoldingRangeParams request)
    {
        return request.TextDocument;
    }

    public async Task<IEnumerable<FoldingRange>?> HandleRequestAsync(FoldingRangeParams @params, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope("FoldingRangeEndpoint.Handle");

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var requestParams = new RazorFoldingRangeRequestParam
        {
            HostDocumentVersion = documentContext.Version,
            TextDocument = @params.TextDocument
        };

        IEnumerable<FoldingRange>? foldingRanges = null;
        var retries = 0;
        const int MaxRetries = 5;

        while (foldingRanges is null && ++retries <= MaxRetries)
        {
            try
            {
                foldingRanges = await HandleCoreAsync(requestParams, documentContext, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception e) when (e is not OperationCanceledException && retries < MaxRetries)
            {
                _logger.LogWarning(e, "Try {retries} to get FoldingRange", retries);
            }
        }

        return foldingRanges;
    }

    private async Task<IEnumerable<FoldingRange>?> HandleCoreAsync(RazorFoldingRangeRequestParam requestParams, DocumentContext documentContext, CancellationToken cancellationToken)
    {
        var foldingResponse = await _languageServer.SendRequestAsync<RazorFoldingRangeRequestParam, RazorFoldingRangeResponse?>(
            RazorLanguageServerCustomMessageTargets.RazorFoldingRangeEndpoint,
            requestParams,
            cancellationToken).ConfigureAwait(false);
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        if (foldingResponse is null)
        {
            return null;
        }

        List<FoldingRange> mappedRanges = new();

        foreach (var foldingRange in foldingResponse.CSharpRanges)
        {
            var range = GetRange(foldingRange);

            if (_documentMappingService.TryMapFromProjectedDocumentRange(
                codeDocument,
                range,
                out var mappedRange))
            {
                mappedRanges.Add(GetFoldingRange(mappedRange, foldingRange.CollapsedText));
            }
        }

        cancellationToken.ThrowIfCancellationRequested();

        mappedRanges.AddRange(foldingResponse.HtmlRanges);

        foreach (var provider in _foldingRangeProviders)
        {
            var ranges = await provider.GetFoldingRangesAsync(documentContext, cancellationToken);
            mappedRanges.AddRange(ranges);
        }

        cancellationToken.ThrowIfCancellationRequested();

        var finalRanges = FinalizeFoldingRanges(mappedRanges, codeDocument);
        return finalRanges;
    }

    private static IEnumerable<FoldingRange> FinalizeFoldingRanges(List<FoldingRange> mappedRanges, RazorCodeDocument codeDocument)
    {
        // Don't allow ranges to be reported if they aren't spanning at least one line
        var validRanges = mappedRanges.Where(r => r.StartLine < r.EndLine);

        // Reduce ranges that have the same start line to be a single instance with the largest
        // range available, since only one button can be shown to collapse per line
        var reducedRanges = validRanges
            .GroupBy(r => r.StartLine)
            .Select(ranges => ranges.OrderByDescending(r => r.EndLine).First());

        // Fix the starting range so the "..." is shown at the end
        return reducedRanges.Select(r => FixFoldingRangeStart(r, codeDocument));
    }

    /// <summary>
    /// Fixes the start of a range so that the offset of the first line is the last character on that line. This makes
    /// it so collapsing will still show the text instead of just "..."
    /// </summary>
    private static FoldingRange FixFoldingRangeStart(FoldingRange range, RazorCodeDocument codeDocument)
    {
        Debug.Assert(range.StartLine < range.EndLine);

        // If the range has collapsed text set, we don't need
        // to adjust anything. Just take that value as what
        // should be shown
        if (!string.IsNullOrEmpty(range.CollapsedText))
        {
            return range;
        }

        var sourceText = codeDocument.GetSourceText();
        var startLine = range.StartLine;
        var lineSpan = sourceText.Lines[startLine].Span;

        // Search from the end of the line to the beginning for the first non whitespace character. We want that
        // to be the offset for the range
        var offset = sourceText.GetLastNonWhitespaceOffset(lineSpan, out _);

        if (offset.HasValue)
        {
            // +1 to the offset value because the helper goes to the character position
            // that we want to be after. Make sure we don't exceed the line end
            var newCharacter = Math.Min(offset.Value + 1, lineSpan.Length);
            range.StartCharacter = newCharacter;
            return range;
        }

        return range;
    }

    private static Range GetRange(FoldingRange foldingRange)
        => new()
        {
            Start = new Position()
            {
                Character = foldingRange.StartCharacter.GetValueOrDefault(),
                Line = foldingRange.StartLine
            },
            End = new Position()
            {
                Character = foldingRange.EndCharacter.GetValueOrDefault(),
                Line = foldingRange.EndLine
            }
        };

    private static FoldingRange GetFoldingRange(Range range, string? collapsedText)
       => new()
       {
           StartLine = range.Start.Line,
           StartCharacter = range.Start.Character,
           EndCharacter = range.End.Character,
           EndLine = range.End.Line,
           CollapsedText = collapsedText
       };
}

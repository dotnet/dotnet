// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.SpellCheck;

[LanguageServerEndpoint(VSInternalMethods.TextDocumentSpellCheckableRangesName)]
internal sealed class DocumentSpellCheckEndpoint : IRazorRequestHandler<VSInternalDocumentSpellCheckableParams, VSInternalSpellCheckableRangeReport[]>, ICapabilitiesProvider
{
    private readonly IRazorDocumentMappingService _documentMappingService;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ClientNotifierServiceBase _languageServer;

    public DocumentSpellCheckEndpoint(
        IRazorDocumentMappingService documentMappingService,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        ClientNotifierServiceBase languageServer)
    {
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
    }

    public bool MutatesSolutionState => false;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.SpellCheckingProvider = true;
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(VSInternalDocumentSpellCheckableParams request)
    {
        if (request.TextDocument is null)
        {
            throw new ArgumentNullException(nameof(request.TextDocument));
        }

        return request.TextDocument;
    }

    public async Task<VSInternalSpellCheckableRangeReport[]> HandleRequestAsync(VSInternalDocumentSpellCheckableParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();

        using var _ = ListPool<SpellCheckRange>.GetPooledObject(out var ranges);

        await AddRazorSpellCheckRangesAsync(ranges, documentContext, cancellationToken).ConfigureAwait(false);

        if (_languageServerFeatureOptions.SingleServerSupport)
        {
            await AddCSharpSpellCheckRangesAsync(ranges, documentContext, cancellationToken).ConfigureAwait(false);
        }

        return new[]
            {
                new VSInternalSpellCheckableRangeReport
                {
                    Ranges = ConvertSpellCheckRangesToIntTriples(ranges),
                    ResultId = Guid.NewGuid().ToString()
                }
            };
    }

    private static async Task AddRazorSpellCheckRangesAsync(List<SpellCheckRange> ranges, VersionedDocumentContext documentContext, CancellationToken cancellationToken)
    {
        var tree = await documentContext.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);

        // We don't want to report spelling errors in script or style tags, so we avoid descending into them at all, which
        // means we don't need complicated logic, and it performs a bit better. We assume any C# in them will still be reported
        // by Roslyn.
        // In an ideal world we wouldn't need this logic at all, as we would defer to the Html LSP server to provide spell checking
        // but it doesn't currently support it. When that support is added, we can remove all of this but the RazorCommentBlockSyntax
        // handling.
        foreach (var node in tree.Root.DescendantNodes(n => n is not MarkupElementSyntax { StartTag.Name.Content: "script" or "style" }))
        {
            if (node is RazorCommentBlockSyntax commentBlockSyntax)
            {
                ranges.Add(new((int)VSInternalSpellCheckableRangeKind.Comment, commentBlockSyntax.Comment.SpanStart, commentBlockSyntax.Comment.Span.Length));
            }
            else if (node is MarkupTextLiteralSyntax textLiteralSyntax)
            {
                // Attribute names are text literals, but we don't want to spell check them because either C# will,
                // whether they're component attributes based on property names, or they come from tag helper attribute
                // parameters as strings, or they're Html attributes which are not necessarily expected to be real words.
                if (node.Parent is MarkupTagHelperAttributeSyntax or MarkupAttributeBlockSyntax)
                {
                    continue;
                }

                // Text literals appear everywhere in Razor to hold newlines and indentation, so its worth saving the tokens
                if (textLiteralSyntax.ContainsOnlyWhitespace())
                {
                    continue;
                }

                ranges.Add(new((int)VSInternalSpellCheckableRangeKind.String, textLiteralSyntax.SpanStart, textLiteralSyntax.Span.Length));
            }
        }
    }

    private async Task AddCSharpSpellCheckRangesAsync(List<SpellCheckRange> ranges, VersionedDocumentContext documentContext, CancellationToken cancellationToken)
    {
        var delegatedParams = new DelegatedSpellCheckParams(documentContext.Identifier);
        var delegatedResponse = await _languageServer.SendRequestAsync<DelegatedSpellCheckParams, VSInternalSpellCheckableRangeReport[]?>(
            CustomMessageNames.RazorSpellCheckEndpoint,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        if (delegatedResponse is null)
        {
            return;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        var csharpDocument = codeDocument.GetCSharpDocument();

        foreach (var report in delegatedResponse)
        {
            if (report.Ranges is not { } csharpRanges)
            {
                continue;
            }

            // Since we get C# tokens that have relative starts, we need to convert them back to absolute indexes
            // so we can sort them with the Razor tokens later
            var absoluteCSharpStartIndex = 0;
            for (var i = 0; i < csharpRanges.Length; i += 3)
            {
                var kind = csharpRanges[i];
                var start = csharpRanges[i + 1];
                var length = csharpRanges[i + 2];

                absoluteCSharpStartIndex += start;

                // We need to map the start index to produce results, and we validate that we can map the end index so we don't have
                // squiggles that go from C# into Razor/Html.
                if (_documentMappingService.TryMapToHostDocumentPosition(csharpDocument, absoluteCSharpStartIndex, out var _1, out var hostDocumentIndex) &&
                    _documentMappingService.TryMapToHostDocumentPosition(csharpDocument, absoluteCSharpStartIndex + length, out var _2, out var _3))
                {
                    ranges.Add(new(kind, hostDocumentIndex, length));
                }

                absoluteCSharpStartIndex += length;
            }
        }
    }

    private static int[] ConvertSpellCheckRangesToIntTriples(List<SpellCheckRange> ranges)
    {
        // Important to sort first, or the client will just ignore anything we say
        ranges.Sort(AbsoluteStartIndexComparer.Instance);

        using var _ = ListPool<int>.GetPooledObject(out var data);
        data.SetCapacityIfLarger(ranges.Count * 3);

        var lastAbsoluteEndIndex = 0;
        foreach (var range in ranges)
        {
            if (range.Length == 0)
            {
                continue;
            }

            data.Add(range.Kind);
            data.Add(range.AbsoluteStartIndex - lastAbsoluteEndIndex);
            data.Add(range.Length);

            lastAbsoluteEndIndex = range.AbsoluteStartIndex + range.Length;
        }

        return data.ToArray();
    }

    private sealed record SpellCheckRange(int Kind, int AbsoluteStartIndex, int Length);

    private sealed class AbsoluteStartIndexComparer : IComparer<SpellCheckRange>
    {
        public static readonly AbsoluteStartIndexComparer Instance = new();

        public int Compare(SpellCheckRange? x, SpellCheckRange? y)
        {
            if (x is null || y is null)
            {
                Debug.Fail("There shouldn't be a null in the list of spell check ranges.");

                return 0;
            }

            return x.AbsoluteStartIndex.CompareTo(y.AbsoluteStartIndex);
        }
    }
}

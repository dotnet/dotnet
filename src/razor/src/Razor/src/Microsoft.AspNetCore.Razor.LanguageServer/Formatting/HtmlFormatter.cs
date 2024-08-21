// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.TextDifferencing;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal class HtmlFormatter
{
    private readonly IDocumentVersionCache _documentVersionCache;
    private readonly IClientConnection _clientConnection;

    public HtmlFormatter(
        IClientConnection clientConnection,
        IDocumentVersionCache documentVersionCache)
    {
        _clientConnection = clientConnection;
        _documentVersionCache = documentVersionCache;
    }

    public async Task<TextEdit[]> FormatAsync(
        FormattingContext context,
        CancellationToken cancellationToken)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (!_documentVersionCache.TryGetDocumentVersion(context.OriginalSnapshot, out var documentVersion))
        {
            return Array.Empty<TextEdit>();
        }

        var @params = new RazorDocumentFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = context.Uri,
            },
            HostDocumentVersion = documentVersion.Value,
            Options = context.Options
        };

        var result = await _clientConnection.SendRequestAsync<DocumentFormattingParams, RazorDocumentFormattingResponse?>(
            CustomMessageNames.RazorHtmlFormattingEndpoint,
            @params,
            cancellationToken).ConfigureAwait(false);

        return result?.Edits ?? Array.Empty<TextEdit>();
    }

    public async Task<TextEdit[]> FormatOnTypeAsync(
       FormattingContext context,
       CancellationToken cancellationToken)
    {
        if (!_documentVersionCache.TryGetDocumentVersion(context.OriginalSnapshot, out var documentVersion))
        {
            return Array.Empty<TextEdit>();
        }

        var @params = new RazorDocumentOnTypeFormattingParams()
        {
            Position = context.SourceText.GetPosition(context.HostDocumentIndex),
            Character = context.TriggerCharacter.ToString(),
            TextDocument = new TextDocumentIdentifier { Uri = context.Uri },
            Options = context.Options,
            HostDocumentVersion = documentVersion.Value,
        };

        var result = await _clientConnection.SendRequestAsync<RazorDocumentOnTypeFormattingParams, RazorDocumentFormattingResponse?>(
            CustomMessageNames.RazorHtmlOnTypeFormattingEndpoint,
            @params,
            cancellationToken).ConfigureAwait(false);

        return result?.Edits ?? Array.Empty<TextEdit>();
    }

    /// <summary>
    /// Sometimes the Html language server will send back an edit that contains a tilde, because the generated
    /// document we send them has lots of tildes. In those cases, we need to do some extra work to compute the
    /// minimal text edits
    /// </summary>
    // Internal for testing
    public static TextEdit[] FixHtmlTestEdits(SourceText htmlSourceText, TextEdit[] edits)
    {
        // Avoid computing a minimal diff if we don't need to
        if (!edits.Any(e => e.NewText.Contains("~")))
            return edits;

        // First we apply the edits that the Html language server wanted, to the Html document
        var textChanges = edits.Select(htmlSourceText.GetTextChange);
        var changedText = htmlSourceText.WithChanges(textChanges);

        // Now we use our minimal text differ algorithm to get the bare minimum of edits
        var minimalChanges = SourceTextDiffer.GetMinimalTextChanges(htmlSourceText, changedText, DiffKind.Char);
        var minimalEdits = minimalChanges.Select(htmlSourceText.GetTextEdit).ToArray();

        return minimalEdits;
    }
}

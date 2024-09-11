// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal sealed class HtmlFormatter(
    IClientConnection clientConnection) : IHtmlFormatter
{
    private readonly IClientConnection _clientConnection = clientConnection;

    public async Task<ImmutableArray<TextChange>> GetDocumentFormattingEditsAsync(
        IDocumentSnapshot documentSnapshot,
        Uri uri,
        FormattingOptions options,
        CancellationToken cancellationToken)
    {
        var @params = new RazorDocumentFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                Uri = uri,
            },
            HostDocumentVersion = documentSnapshot.Version,
            Options = options
        };

        var result = await _clientConnection.SendRequestAsync<DocumentFormattingParams, RazorDocumentFormattingResponse?>(
            CustomMessageNames.RazorHtmlFormattingEndpoint,
            @params,
            cancellationToken).ConfigureAwait(false);

        if (result?.Edits is null)
        {
            return [];
        }

        var sourceText = await documentSnapshot.GetTextAsync().ConfigureAwait(false);
        return result.Edits.SelectAsArray(sourceText.GetTextChange);
    }

    public async Task<ImmutableArray<TextChange>> GetOnTypeFormattingEditsAsync(
        IDocumentSnapshot documentSnapshot,
        Uri uri,
        Position position,
        string triggerCharacter,
        FormattingOptions options,
        CancellationToken cancellationToken)
    {
        var @params = new RazorDocumentOnTypeFormattingParams()
        {
            Position = position,
            Character = triggerCharacter.ToString(),
            TextDocument = new TextDocumentIdentifier { Uri = uri },
            Options = options,
            HostDocumentVersion = documentSnapshot.Version,
        };

        var result = await _clientConnection.SendRequestAsync<RazorDocumentOnTypeFormattingParams, RazorDocumentFormattingResponse?>(
            CustomMessageNames.RazorHtmlOnTypeFormattingEndpoint,
            @params,
            cancellationToken).ConfigureAwait(false);

        if (result?.Edits is null)
        {
            return [];
        }

        var sourceText = await documentSnapshot.GetTextAsync().ConfigureAwait(false);
        return result.Edits.SelectAsArray(sourceText.GetTextChange);
    }

    /// <summary>
    /// Sometimes the Html language server will send back an edit that contains a tilde, because the generated
    /// document we send them has lots of tildes. In those cases, we need to do some extra work to compute the
    /// minimal text edits
    /// </summary>
    // Internal for testing
    public static TextEdit[] FixHtmlTextEdits(SourceText htmlSourceText, TextEdit[] edits)
    {
        // Avoid computing a minimal diff if we don't need to
        if (!edits.Any(static e => e.NewText.Contains("~")))
            return edits;

        var changes = edits.SelectAsArray(htmlSourceText.GetTextChange);

        var fixedChanges = htmlSourceText.MinimizeTextChanges(changes);
        return [.. fixedChanges.Select(htmlSourceText.GetTextEdit)];
    }
}

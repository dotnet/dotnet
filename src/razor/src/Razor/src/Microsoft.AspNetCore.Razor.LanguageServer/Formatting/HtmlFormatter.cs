// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal sealed class HtmlFormatter(
    IClientConnection clientConnection) : IHtmlFormatter
{
    private readonly IClientConnection _clientConnection = clientConnection;

    public async Task<ImmutableArray<TextChange>?> GetDocumentFormattingEditsAsync(
        IDocumentSnapshot documentSnapshot,
        Uri uri,
        FormattingOptions options,
        CancellationToken cancellationToken)
    {
        var @params = new RazorDocumentFormattingParams()
        {
            TextDocument = new TextDocumentIdentifier
            {
                DocumentUri = new(uri),
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
            return null;
        }

        var sourceText = await documentSnapshot.GetTextAsync(cancellationToken).ConfigureAwait(false);
        return result.Edits.SelectAsArray(sourceText.GetTextChange);
    }

    public async Task<ImmutableArray<TextChange>?> GetOnTypeFormattingEditsAsync(
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
            TextDocument = new TextDocumentIdentifier { DocumentUri = new(uri) },
            Options = options,
            HostDocumentVersion = documentSnapshot.Version,
        };

        var result = await _clientConnection.SendRequestAsync<RazorDocumentOnTypeFormattingParams, RazorDocumentFormattingResponse?>(
            CustomMessageNames.RazorHtmlOnTypeFormattingEndpoint,
            @params,
            cancellationToken).ConfigureAwait(false);

        if (result?.Edits is null)
        {
            return null;
        }

        var sourceText = await documentSnapshot.GetTextAsync(cancellationToken).ConfigureAwait(false);
        return result.Edits.SelectAsArray(sourceText.GetTextChange);
    }
}

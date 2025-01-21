// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.ProjectSystem;
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

        var sourceText = await documentSnapshot.GetTextAsync(cancellationToken).ConfigureAwait(false);
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

        var sourceText = await documentSnapshot.GetTextAsync(cancellationToken).ConfigureAwait(false);
        return result.Edits.SelectAsArray(sourceText.GetTextChange);
    }
}

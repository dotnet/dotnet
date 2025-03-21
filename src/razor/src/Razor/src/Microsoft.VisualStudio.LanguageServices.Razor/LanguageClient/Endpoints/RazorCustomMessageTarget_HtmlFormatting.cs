// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.Formatting;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to invoke a razor/htmlFormatting request on the virtual Html buffer.
    [JsonRpcMethod(CustomMessageNames.RazorHtmlFormattingEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<RazorDocumentFormattingResponse?> HtmlFormattingAsync(RazorDocumentFormattingParams request, CancellationToken cancellationToken)
    {
        var response = new RazorDocumentFormattingResponse() { Edits = Array.Empty<TextEdit>() };

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var (synchronized, htmlDocument) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
            request.HostDocumentVersion,
            request.TextDocument,
            cancellationToken);

        var languageServerName = RazorLSPConstants.HtmlLanguageServerName;

        if (!synchronized || htmlDocument is null)
        {
            Debug.Fail("RangeFormatting not synchronized.");
            return null;
        }

        var projectedUri = htmlDocument.Uri;

        var formattingParams = new DocumentFormattingParams()
        {
            TextDocument = request.TextDocument.WithUri(projectedUri),
            Options = request.Options
        };

        var textBuffer = htmlDocument.Snapshot.TextBuffer;
        var edits = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentFormattingParams, TextEdit[]>(
            textBuffer,
            Methods.TextDocumentFormattingName,
            languageServerName,
            formattingParams,
            cancellationToken).ConfigureAwait(false);

        if (edits?.Response is null)
        {
            return null;
        }

        response.Edits = edits.Response;

        return response;
    }

    // Called by the Razor Language Server to invoke a razor/htmlOnTypeFormatting request on the virtual Html buffer.
    [JsonRpcMethod(CustomMessageNames.RazorHtmlOnTypeFormattingEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<RazorDocumentFormattingResponse?> HtmlOnTypeFormattingAsync(RazorDocumentOnTypeFormattingParams request, CancellationToken cancellationToken)
    {
        var response = new RazorDocumentFormattingResponse() { Edits = Array.Empty<TextEdit>() };

        var hostDocument = request.TextDocument;

        var languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        var (synchronized, htmlDocument) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(request.HostDocumentVersion, hostDocument, cancellationToken);

        if (!synchronized || htmlDocument is null)
        {
            return null;
        }

        var formattingParams = new DocumentOnTypeFormattingParams()
        {
            Character = request.Character,
            Position = request.Position,
            TextDocument = request.TextDocument.WithUri(htmlDocument.Uri),
            Options = request.Options
        };

        var textBuffer = htmlDocument.Snapshot.TextBuffer;
        var edits = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentOnTypeFormattingParams, TextEdit[]>(
            textBuffer,
            Methods.TextDocumentOnTypeFormattingName,
            languageServerName,
            formattingParams,
            cancellationToken).ConfigureAwait(false);

        if (edits?.Response is null)
        {
            return null;
        }

        response.Edits = edits.Response;

        return response;
    }
}

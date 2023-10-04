// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to invoke a razor/htmlFormatting request on the virtual Html buffer.
    [JsonRpcMethod(CustomMessageNames.RazorHtmlFormattingEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<RazorDocumentFormattingResponse> HtmlFormattingAsync(RazorDocumentFormattingParams request, CancellationToken cancellationToken)
    {
        var response = new RazorDocumentFormattingResponse() { Edits = Array.Empty<TextEdit>() };

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var (synchronized, htmlDocument) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
            request.HostDocumentVersion,
            request.TextDocument,
            cancellationToken);

        var languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        var projectedUri = htmlDocument.Uri;

        if (!synchronized)
        {
            Debug.Fail("RangeFormatting not synchronized.");
            return response;
        }

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

        response.Edits = edits?.Response ?? Array.Empty<TextEdit>();

        return response;
    }

    // Called by the Razor Language Server to invoke a razor/htmlOnTypeFormatting request on the virtual Html buffer.
    [JsonRpcMethod(CustomMessageNames.RazorHtmlOnTypeFormattingEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<RazorDocumentFormattingResponse> HtmlOnTypeFormattingAsync(RazorDocumentOnTypeFormattingParams request, CancellationToken cancellationToken)
    {
        var response = new RazorDocumentFormattingResponse() { Edits = Array.Empty<TextEdit>() };

        var hostDocument = request.TextDocument;

        var languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        var (synchronized, htmlDocument) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(request.HostDocumentVersion, hostDocument, cancellationToken);

        if (!synchronized)
        {
            return response;
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

        response.Edits = edits?.Response ?? Array.Empty<TextEdit>();

        return response;
    }
}

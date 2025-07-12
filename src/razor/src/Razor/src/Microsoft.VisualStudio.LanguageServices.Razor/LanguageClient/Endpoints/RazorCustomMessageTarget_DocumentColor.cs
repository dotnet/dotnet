// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.ColorPresentation;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to provide document colors from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorProvideHtmlDocumentColorEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<IReadOnlyList<ColorInformation>?> ProvideHtmlDocumentColorAsync(DelegatedDocumentColorParams documentColorParams, CancellationToken cancellationToken)
    {
        if (documentColorParams is null)
        {
            throw new ArgumentNullException(nameof(documentColorParams));
        }

        var (synchronized, htmlDoc) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(documentColorParams.HostDocumentVersion, documentColorParams.TextDocument, cancellationToken);
        if (!synchronized || htmlDoc is null)
        {
            return [];
        }

        documentColorParams.TextDocument.DocumentUri = new(htmlDoc.Uri);
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<DocumentColorParams, ColorInformation[]>(
            Methods.TextDocumentDocumentColor.Name,
            RazorLSPConstants.HtmlLanguageServerName,
            documentColorParams,
            cancellationToken).ConfigureAwait(false);

        return response.Result;
    }

    // Called by the Razor Language Server to provide color presentation from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorProvideHtmlColorPresentationEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<IReadOnlyList<ColorPresentation>> ProvideHtmlColorPresentationAsync(DelegatedColorPresentationParams colorPresentationParams, CancellationToken cancellationToken)
    {
        if (colorPresentationParams is null)
        {
            throw new ArgumentNullException(nameof(colorPresentationParams));
        }

        var (synchronized, htmlDoc) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(colorPresentationParams.RequiredHostDocumentVersion, colorPresentationParams.TextDocument, cancellationToken);
        if (!synchronized || htmlDoc is null)
        {
            return [];
        }

        colorPresentationParams.TextDocument.DocumentUri = new(htmlDoc.Uri);
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<ColorPresentationParams, ColorPresentation[]>(
            Methods.TextDocumentColorPresentationName,
            RazorLSPConstants.HtmlLanguageServerName,
            colorPresentationParams,
            cancellationToken).ConfigureAwait(false);

        return response.Result;
    }
}

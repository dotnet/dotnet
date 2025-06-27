// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.ColorPresentation;
using Microsoft.VisualStudio.Threading;
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
            return new List<ColorInformation>();
        }

        documentColorParams.TextDocument.DocumentUri = new(htmlDoc.Uri);
        var htmlTextBuffer = htmlDoc.Snapshot.TextBuffer;
        var requests = _requestInvoker.ReinvokeRequestOnMultipleServersAsync<DocumentColorParams, ColorInformation[]>(
            htmlTextBuffer,
            Methods.TextDocumentDocumentColor.Name,
            documentColorParams,
            cancellationToken).ConfigureAwait(false);

        var colorInformation = new List<ColorInformation>();
        await foreach (var response in requests)
        {
            if (response.Response is not null)
            {
                colorInformation.AddRange(response.Response);
            }
        }

        return colorInformation;
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
            return new List<ColorPresentation>();
        }

        colorPresentationParams.TextDocument.DocumentUri = new(htmlDoc.Uri);
        var htmlTextBuffer = htmlDoc.Snapshot.TextBuffer;
        var requests = _requestInvoker.ReinvokeRequestOnMultipleServersAsync<ColorPresentationParams, ColorPresentation[]>(
            htmlTextBuffer,
            Methods.TextDocumentColorPresentationName,
            colorPresentationParams,
            cancellationToken).ConfigureAwait(false);

        var colorPresentation = new List<ColorPresentation>();
        await foreach (var response in requests)
        {
            if (response.Response is not null)
            {
                colorPresentation.AddRange(response.Response);
            }
        }

        return colorPresentation;
    }
}

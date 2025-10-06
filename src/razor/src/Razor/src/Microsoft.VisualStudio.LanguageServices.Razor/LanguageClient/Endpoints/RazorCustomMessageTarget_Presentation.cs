// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.DocumentPresentation;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorTextPresentationEndpoint, UseSingleObjectParameterDeserialization = true)]
    public Task<WorkspaceEdit?> ProvideTextPresentationAsync(RazorTextPresentationParams presentationParams, CancellationToken cancellationToken)
    {
        return ProvidePresentationAsync(presentationParams, presentationParams.HostDocumentVersion, presentationParams.Kind, VSInternalMethods.TextDocumentTextPresentationName, cancellationToken);
    }

    [JsonRpcMethod(CustomMessageNames.RazorUriPresentationEndpoint, UseSingleObjectParameterDeserialization = true)]
    public Task<WorkspaceEdit?> ProvideUriPresentationAsync(RazorUriPresentationParams presentationParams, CancellationToken cancellationToken)
    {
        return ProvidePresentationAsync(presentationParams, presentationParams.HostDocumentVersion, presentationParams.Kind, VSInternalMethods.TextDocumentUriPresentationName, cancellationToken);
    }

    private async Task<WorkspaceEdit?> ProvidePresentationAsync<TParams>(TParams presentationParams, int hostDocumentVersion, RazorLanguageKind kind, string methodName, CancellationToken cancellationToken)
        where TParams : notnull, IPresentationParams
    {
        string languageServerName;
        bool synchronized;
        VirtualDocumentSnapshot document;
        if (kind == RazorLanguageKind.CSharp)
        {
            (synchronized, document) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                 hostDocumentVersion,
                 presentationParams.TextDocument,
                 cancellationToken);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
        }
        else if (kind == RazorLanguageKind.Html)
        {
            (synchronized, document) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                hostDocumentVersion,
                presentationParams.TextDocument,
                cancellationToken);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This can't really happen in a real scenario.");
            return null;
        }

        if (!synchronized || document is null)
        {
            return null;
        }

        presentationParams.TextDocument = new TextDocumentIdentifier
        {
            DocumentUri = new(document.Uri),
        };

        var textBuffer = document.Snapshot.TextBuffer;
        var result = await _requestInvoker.ReinvokeRequestOnServerAsync<TParams, WorkspaceEdit?>(
            textBuffer,
            methodName,
            languageServerName,
            presentationParams,
            cancellationToken).ConfigureAwait(false);

        return result?.Response;
    }
}

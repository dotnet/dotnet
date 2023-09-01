// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

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
        VirtualDocumentSnapshot document;
        if (kind == RazorLanguageKind.CSharp)
        {
            var syncResult = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                hostDocumentVersion,
                presentationParams.TextDocument,
                cancellationToken);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
            presentationParams.TextDocument = new TextDocumentIdentifier
            {
                Uri = syncResult.VirtualSnapshot.Uri,
            };
            document = syncResult.VirtualSnapshot;
        }
        else if (kind == RazorLanguageKind.Html)
        {
            var syncResult = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                hostDocumentVersion,
                presentationParams.TextDocument,
                cancellationToken);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
            presentationParams.TextDocument = new TextDocumentIdentifier
            {
                Uri = syncResult.VirtualSnapshot.Uri,
            };
            document = syncResult.VirtualSnapshot;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This can't really happen in a real scenario.");
            return null;
        }

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

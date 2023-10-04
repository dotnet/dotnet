// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to provide code actions from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorProvideCodeActionsEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<IReadOnlyList<VSInternalCodeAction>?> ProvideCodeActionsAsync(DelegatedCodeActionParams codeActionParams, CancellationToken cancellationToken)
    {
        if (codeActionParams is null)
        {
            throw new ArgumentNullException(nameof(codeActionParams));
        }

        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        string languageServerName;
        if (codeActionParams.LanguageKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                codeActionParams.HostDocumentVersion,
                codeActionParams.CodeActionParams.TextDocument,
                cancellationToken).ConfigureAwait(false);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
        }
        else if (codeActionParams.LanguageKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                codeActionParams.HostDocumentVersion,
                codeActionParams.CodeActionParams.TextDocument,
                cancellationToken).ConfigureAwait(false);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This shouldn't really happen in a real scenario.");
            return null;
        }

        if (!synchronized || virtualDocumentSnapshot is null)
        {
            // Document could not synchronize
            return null;
        }

        codeActionParams.CodeActionParams.TextDocument.Uri = virtualDocumentSnapshot.Uri;

        var textBuffer = virtualDocumentSnapshot.Snapshot.TextBuffer;
        var lspMethodName = Methods.TextDocumentCodeActionName;
        using var _ = _telemetryReporter.TrackLspRequest(lspMethodName, languageServerName, codeActionParams.CorrelationId);
        var requests = _requestInvoker.ReinvokeRequestOnMultipleServersAsync<VSCodeActionParams, IReadOnlyList<VSInternalCodeAction>>(
            textBuffer,
            lspMethodName,
            SupportsCodeActionResolve,
            codeActionParams.CodeActionParams,
            cancellationToken).ConfigureAwait(false);

        var codeActions = new List<VSInternalCodeAction>();
        await foreach (var response in requests.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            if (response.Response != null)
            {
                codeActions.AddRange(response.Response);
            }
        }

        return codeActions;
    }

    // Called by the Razor Language Server to resolve code actions from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorResolveCodeActionsEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSInternalCodeAction?> ResolveCodeActionsAsync(RazorResolveCodeActionParams resolveCodeActionParams, CancellationToken cancellationToken)
    {
        if (resolveCodeActionParams is null)
        {
            throw new ArgumentNullException(nameof(resolveCodeActionParams));
        }

        if (!_documentManager.TryGetDocument(resolveCodeActionParams.Uri, out var documentSnapshot))
        {
            // Couldn't resolve the document associated with the code action bail out.
            return null;
        }

        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        if (resolveCodeActionParams.LanguageKind == RazorLanguageKind.Html)
        {
            // TODO: Need to get project context to pass to the synchronizer
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                resolveCodeActionParams.HostDocumentVersion,
                resolveCodeActionParams.Uri,
                cancellationToken).ConfigureAwait(false);
        }
        else if (resolveCodeActionParams.LanguageKind == RazorLanguageKind.CSharp)
        {
            // TODO: Need to get project context to pass to the synchronizer
            (synchronized, virtualDocumentSnapshot) = await _documentSynchronizer.TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                resolveCodeActionParams.HostDocumentVersion,
                resolveCodeActionParams.Uri,
                cancellationToken).ConfigureAwait(false);
        }
        else
        {
            Debug.Fail("Unexpected RazorLanguageKind. This shouldn't really happen in a real scenario.");
            return null;
        }

        if (!synchronized || virtualDocumentSnapshot is null)
        {
            // Document could not synchronize
            return null;
        }

        var textBuffer = virtualDocumentSnapshot.Snapshot.TextBuffer;
        var codeAction = resolveCodeActionParams.CodeAction;
        var requests = _requestInvoker.ReinvokeRequestOnMultipleServersAsync<CodeAction, VSInternalCodeAction?>(
            textBuffer,
            Methods.CodeActionResolveName,
            SupportsCodeActionResolve,
            codeAction,
            cancellationToken).ConfigureAwait(false);

        await foreach (var response in requests.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (response.Response is not null)
            {
                // Only take the first response from a resolution
                return response.Response;
            }
        }

        return null;
    }

    private static bool SupportsCodeActionResolve(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        var (providesCodeActions, resolvesCodeActions) = serverCapabilities?.CodeActionProvider?.Match(
            boolValue => (boolValue, false),
            options => (true, options.ResolveProvider)) ?? (false, false);

        return providesCodeActions && resolvesCodeActions;
    }
}

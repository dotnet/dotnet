// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to provide code actions from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorProvideCodeActionsEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<IReadOnlyList<VSInternalCodeAction>?> ProvideCodeActionsAsync(DelegatedCodeActionParams codeActionParams, CancellationToken cancellationToken)
    {
        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        string languageServerName;
        if (codeActionParams.LanguageKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                codeActionParams.HostDocumentVersion,
                codeActionParams.CodeActionParams.TextDocument,
                cancellationToken).ConfigureAwait(false);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else if (codeActionParams.LanguageKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                codeActionParams.HostDocumentVersion,
                codeActionParams.CodeActionParams.TextDocument,
                cancellationToken).ConfigureAwait(false);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
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

        codeActionParams.CodeActionParams.TextDocument.DocumentUri = new(virtualDocumentSnapshot.Uri);

        var lspMethodName = Methods.TextDocumentCodeActionName;
        using var _ = _telemetryReporter.TrackLspRequest(lspMethodName, languageServerName, TelemetryThresholds.CodeActionSubLSPTelemetryThreshold, codeActionParams.CorrelationId);
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSCodeActionParams, IReadOnlyList<VSInternalCodeAction>>(
            lspMethodName,
            languageServerName,
            codeActionParams.CodeActionParams,
            cancellationToken).ConfigureAwait(false);

        if (response.Result is { } codeActions)
        {
            return codeActions;
        }

        return [];
    }

    // Called by the Razor Language Server to resolve code actions from the platform.
    [JsonRpcMethod(CustomMessageNames.RazorResolveCodeActionsEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task<VSInternalCodeAction?> ResolveCodeActionsAsync(RazorResolveCodeActionParams resolveCodeActionParams, CancellationToken cancellationToken)
    {
        if (resolveCodeActionParams is null)
        {
            throw new ArgumentNullException(nameof(resolveCodeActionParams));
        }

        bool synchronized;
        VirtualDocumentSnapshot virtualDocumentSnapshot;
        string languageServerName;
        if (resolveCodeActionParams.LanguageKind == RazorLanguageKind.Html)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<HtmlVirtualDocumentSnapshot>(
                resolveCodeActionParams.HostDocumentVersion,
                resolveCodeActionParams.Identifier,
                cancellationToken).ConfigureAwait(false);
            languageServerName = RazorLSPConstants.HtmlLanguageServerName;
        }
        else if (resolveCodeActionParams.LanguageKind == RazorLanguageKind.CSharp)
        {
            (synchronized, virtualDocumentSnapshot) = await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(
                resolveCodeActionParams.HostDocumentVersion,
                resolveCodeActionParams.Identifier,
                cancellationToken).ConfigureAwait(false);
            languageServerName = RazorLSPConstants.RazorCSharpLanguageServerName;
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

        var codeAction = resolveCodeActionParams.CodeAction;

        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<CodeAction, VSInternalCodeAction?>(
            Methods.CodeActionResolveName,
            languageServerName,
            codeAction,
            cancellationToken).ConfigureAwait(false);

        if (response.Result is { } resolvedCodeAction)
        {
            return resolvedCodeAction;
        }

        return null;
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorPullDiagnosticEndpointName, UseSingleObjectParameterDeserialization = true)]
    public async Task<RazorPullDiagnosticResponse?> DiagnosticsAsync(DelegatedDiagnosticParams request, CancellationToken cancellationToken)
    {
        // Normally we don't like to construct POCOs directly, because it removes potentially unknown data that has been
        // deserialized from the JSON request. To ensure we don't do that we modify the request object (see WithUri call below)
        // but in this case, where we asynchronously fire off two requests, that introduces a problem as we can end up modifying
        // the object before its been used to synchronize one of the virtual documents.
        // We're okay to construct this object _in this specific scenario_ because we know it is only used to synchronize
        // requests inside Razor, and we only use ProjectContext and Uri to do that.
        var hostDocument = new VSTextDocumentIdentifier
        {
            ProjectContext = request.Identifier.TextDocumentIdentifier.GetProjectContext(),
            Uri = request.Identifier.TextDocumentIdentifier.Uri,
        };

        var csharpTask = Task.Run(() => GetVirtualDocumentPullDiagnosticsAsync<CSharpVirtualDocumentSnapshot>(hostDocument, request.Identifier.Version, request.Identifier.TextDocumentIdentifier, request.CorrelationId, RazorLSPConstants.RazorCSharpLanguageServerName, cancellationToken), cancellationToken);
        var htmlTask = Task.Run(() => GetVirtualDocumentPullDiagnosticsAsync<HtmlVirtualDocumentSnapshot>(hostDocument, request.Identifier.Version, request.Identifier.TextDocumentIdentifier, request.CorrelationId, RazorLSPConstants.HtmlLanguageServerName, cancellationToken), cancellationToken);

        try
        {
            await Task.WhenAll(htmlTask, csharpTask).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (e is not OperationCanceledException)
            {
                _logger?.LogError(e, "Exception thrown in PullDiagnostic delegation");
            }
            // Return null if any of the tasks getting diagnostics results in an error
            return null;
        }

        var csharpDiagnostics = await csharpTask.ConfigureAwait(false);
        var htmlDiagnostics = await htmlTask.ConfigureAwait(false);

        if (csharpDiagnostics is null || htmlDiagnostics is null)
        {
            // If either is null we don't have a complete view and returning anything will cause us to "lock-in" incomplete info. So we return null and wait for a re-try.
            return null;
        }

        return new RazorPullDiagnosticResponse(csharpDiagnostics, htmlDiagnostics);
    }

    private async Task<VSInternalDiagnosticReport[]?> GetVirtualDocumentPullDiagnosticsAsync<TVirtualDocumentSnapshot>(TextDocumentIdentifier hostDocument, int hostDocumentVersion, TextDocumentIdentifier identifierFromOriginalRequest, Guid correlationId, string delegatedLanguageServerName, CancellationToken cancellationToken)
        where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        var (synchronized, virtualDocument) = await TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(
            hostDocumentVersion,
            hostDocument,
            cancellationToken).ConfigureAwait(false);
        if (!synchronized)
        {
            return null;
        }

        var request = new VSInternalDocumentDiagnosticsParams
        {
            TextDocument = identifierFromOriginalRequest.WithUri(virtualDocument.Uri),
        };

        var lspMethodName = VSInternalMethods.DocumentPullDiagnosticName;
        using var _ = _telemetryReporter.TrackLspRequest(lspMethodName, delegatedLanguageServerName, correlationId);
        var response = await _requestInvoker.ReinvokeRequestOnServerAsync<VSInternalDocumentDiagnosticsParams, VSInternalDiagnosticReport[]?>(
            virtualDocument.Snapshot.TextBuffer,
            lspMethodName,
            delegatedLanguageServerName,
            request,
            cancellationToken).ConfigureAwait(false);

        // If the delegated server wants to remove all diagnostics about a document, they will send back a response with an item, but that
        // item will have null diagnostics (and every other property). We don't want to propagate that back out to the client, because
        // it would make the client remove all diagnostics for the .razor file, including potentially any returned from other delegated
        // servers.
        if (response?.Response is null or [{ Diagnostics: null }, ..])
        {
            // Important that we send back an empty list here, because null would result it the above method throwing away any other
            // diagnostics it receives from the other delegated server
            return Array.Empty<VSInternalDiagnosticReport>();
        }

        return response.Response;
    }
}

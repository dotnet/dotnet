// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Endpoints;

internal partial class RazorCustomMessageTarget
{
    // Called by the Razor Language Server to update the contents of the virtual CSharp buffer.
    [JsonRpcMethod(CustomMessageNames.RazorUpdateCSharpBufferEndpoint, UseSingleObjectParameterDeserialization = true)]
    public async Task UpdateCSharpBufferAsync(UpdateBufferRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // We're going to try updating the C# buffer, and it has to happen on the UI thread. That is a single shared resource
        // that can only be updated on a specific thread, and so we can easily hit contention. In particular with provisional
        // completion we can end up with a state where we are waiting for completion to finish before we can update the document
        // but other features are waiting on us to update the document with some changes. Normally this is fine, and we handle it
        // with our sync system and simply cancel the task and let the next one in. When updating buffers from the server though
        // we can't do that, as the server assumes we can always apply the update. Our only option here is to keep trying until
        // we get a successful update.
        // It's worth noting we only try again specifically for provisional completion, so we don't get random deadocks.
        var tryAgain = true;
        while (tryAgain)
        {
            _logger.LogDebug($"Trying a call to UpdateCSharpBufferCoreAsync for v{request.HostDocumentVersion}");
            tryAgain = await UpdateCSharpBufferCoreAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<bool> UpdateCSharpBufferCoreAsync(UpdateBufferRequest request, CancellationToken cancellationToken)
    {
        if (request.HostDocumentFilePath is null || request.HostDocumentVersion is null)
        {
            return false;
        }

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var identifier = CreateTextDocumentIdentifier(request);
        var hostDocumentUri = identifier.Uri;

        _logger.LogDebug($"UpdateCSharpBuffer for {request.HostDocumentVersion} of {hostDocumentUri} in {request.ProjectKeyId}");

        // If we're generating unique file paths for virtual documents, then we have to take a different path here, and do more work
        if (_languageServerFeatureOptions.IncludeProjectKeyInGeneratedFilePath &&
            request.ProjectKeyId is not null &&
            _documentManager.TryGetDocument(hostDocumentUri, out var documentSnapshot) &&
            documentSnapshot.TryGetAllVirtualDocuments<CSharpVirtualDocumentSnapshot>(out var virtualDocuments))
        {
            if (virtualDocuments is [{ ProjectKey.IsUnknown: true }])
            {
                // If there is only a single virtual document, and its got a null id, then that means it's in our "misc files" project
                // but the server clearly knows about it in a real project. That means its probably new, as Visual Studio opens a buffer
                // for a document before we get the notifications about it being added to any projects. Lets try refreshing before
                // we worry.
                _logger.LogDebug($"Refreshing virtual documents, and waiting for them, (for {hostDocumentUri})");

                var task = _csharpVirtualDocumentAddListener.WaitForDocumentAddAsync(cancellationToken);
                _documentManager.RefreshVirtualDocuments();
                var added = await task.ConfigureAwait(true);

                // Since we're dealing with snapshots, we have to get the new ones after refreshing
                if (!_documentManager.TryGetDocument(hostDocumentUri, out documentSnapshot) ||
                    !documentSnapshot.TryGetAllVirtualDocuments<CSharpVirtualDocumentSnapshot>(out virtualDocuments))
                {
                    // This should never happen.
                    // The server clearly wants to tell us about a document in a project, but we don't know which project it's in.
                    // Sadly there isn't anything we can do here to, we're just in a state where the server and client are out of
                    // sync with their understanding of the document contents, and since changes come in as a list of changes,
                    // the user experience is broken. All we can do is hope the user closes and re-opens the document.
                    _logger.LogError($"Server wants to update {hostDocumentUri} in {request.ProjectKeyId} by we only know about that document in misc files. Server and client are now out of sync.");
                    Debug.Fail($"Server wants to update {hostDocumentUri} in {request.ProjectKeyId} but we don't know about the document being in any projects");
                    return false;
                }
            }

            // First we need to make sure we're synced to the previous version, or the changes won't apply properly. This should no-op in most cases, as this
            // is (almost) the only thing that actually moves documents forward, we're really just validating we're in a good state.
            // We're specifically checking here for provisional completion in flight, which is a case where we update the C# document
            // in a way that doesn't represent the actual state, so we can't let server updates through while in this state (represented
            // by a negative version number). Due to provisional completion being on the UI thread, we have to return from this method
            // and try again so we get to the back of the UI thread queue.
            if (request.PreviousHostDocumentVersion is { } previousVersion &&
                await TrySynchronizeVirtualDocumentAsync<CSharpVirtualDocumentSnapshot>(previousVersion, identifier, cancellationToken, rejectOnNewerParallelRequest: false) is { } synchronizedResult &&
                !synchronizedResult.Synchronized &&
                synchronizedResult.VirtualSnapshot?.HostDocumentSyncVersion < 0)
            {
                _logger.LogError($"Request to update C# buffer from {previousVersion} to {request.HostDocumentVersion} failed because the server Roslyn and Razor are out of sync. Server version is {synchronizedResult.VirtualSnapshot?.HostDocumentSyncVersion}. Will try again as provisional completion is in flight which is an expected cause of de-sync, which we recover from.");
                return true;
            }

            foreach (var virtualDocument in virtualDocuments)
            {
                if (virtualDocument.ProjectKey.Equals(new ProjectKey(request.ProjectKeyId)))
                {
                    _logger.LogDebug($"UpdateCSharpBuffer virtual doc for {request.HostDocumentVersion} of {virtualDocument.Uri}. Previous lines: {virtualDocument.Snapshot.LineCount}");

                    _documentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
                        hostDocumentUri,
                        virtualDocument.Uri,
                        request.Changes.Select(change => change.ToVisualStudioTextChange()).ToArray(),
                        request.HostDocumentVersion.Value,
                        state: request.PreviousWasEmpty);

                    _logger.LogDebug($"UpdateCSharpBuffer finished updating doc for {request.HostDocumentVersion} of {virtualDocument.Uri}. New lines: {GetLineCountOfVirtualDocument(hostDocumentUri, virtualDocument)}");

                    return false;
                }
            }

            if (virtualDocuments.Length > 1)
            {
                // If the particular document supports multiple virtual documents, we don't want to try to update a single one. The server could
                // be sending C# for a Misc Files file, but once the server knows about the real project, it will start sending C# for that, and
                // that needs to be a brand new buffer.
                _logger.LogDebug($"""
                    Was looking for {request.ProjectKeyId} but found only:
                    {string.Join(Environment.NewLine, virtualDocuments.Select(d => $"[{d.ProjectKey}] {d.Uri}"))}
                    """);
            }

            _logger.LogDebug($"UpdateCSharpBuffer couldn't find any virtual docs for {request.HostDocumentVersion} of {hostDocumentUri}");

            // Don't know about document, no-op. This can happen if the language server found a project.razor.bin from an old build
            // and is sending us updates.
            return false;
        }

        _logger.LogDebug($"UpdateCSharpBuffer fallback for {request.HostDocumentVersion} of {hostDocumentUri}");

        _documentManager.UpdateVirtualDocument<CSharpVirtualDocument>(
            hostDocumentUri,
            request.Changes.Select(change => change.ToVisualStudioTextChange()).ToArray(),
            request.HostDocumentVersion.Value,
            state: request.PreviousWasEmpty);

        return false;
    }

    private static TextDocumentIdentifier CreateTextDocumentIdentifier(UpdateBufferRequest request)
    {
        var hostDocumentUri = new Uri(request.HostDocumentFilePath);
        if (request.ProjectKeyId is { } id)
        {
            return new VSTextDocumentIdentifier
            {
                Uri = hostDocumentUri,
                ProjectContext = new VSProjectContext
                {
                    Id = id,
                }
            };
        }

        return new TextDocumentIdentifier { Uri = hostDocumentUri };
    }

    private int GetLineCountOfVirtualDocument(Uri hostDocumentUri, CSharpVirtualDocumentSnapshot virtualDocument)
    {
        if (_documentManager.TryGetDocument(hostDocumentUri, out var newDocSnapshot) &&
            newDocSnapshot.VirtualDocuments.FirstOrDefault(e => e.Uri == virtualDocument.Uri) is { } newDoc)
        {
            return newDoc.Snapshot.LineCount;
        }

        return -1;
    }
}

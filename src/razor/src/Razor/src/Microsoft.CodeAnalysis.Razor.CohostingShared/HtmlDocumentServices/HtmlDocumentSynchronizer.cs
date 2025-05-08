// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Remote;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(IHtmlDocumentSynchronizer))]
[method: ImportingConstructor]
internal sealed partial class HtmlDocumentSynchronizer(
    IRemoteServiceInvoker remoteServiceInvoker,
    IHtmlDocumentPublisher htmlDocumentPublisher,
    ILoggerFactory loggerFactory)
    : IHtmlDocumentSynchronizer
{
    private readonly IRemoteServiceInvoker _remoteServiceInvoker = remoteServiceInvoker;
    private readonly IHtmlDocumentPublisher _htmlDocumentPublisher = htmlDocumentPublisher;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<HtmlDocumentSynchronizer>();

    private readonly Dictionary<Uri, SynchronizationRequest> _synchronizationRequests = [];
    private readonly object _gate = new();

    public void DocumentRemoved(Uri razorFileUri)
    {
        lock (_gate)
        {
            if (_synchronizationRequests.TryGetValue(razorFileUri, out var request))
            {
                _logger.LogDebug($"Document {razorFileUri} removed, so we're disposing and clearing out the sync request for it");
                request.Dispose();
                _synchronizationRequests.Remove(razorFileUri);
            }
        }
    }

    public async Task<SynchronizationResult> TrySynchronizeAsync(TextDocument document, CancellationToken cancellationToken)
    {
        var requestedVersion = await RazorDocumentVersion.CreateAsync(document, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug($"TrySynchronize for {document.FilePath} as at {requestedVersion}");

        // We are not passing on the cancellation token through to the actual task that does the generation, because
        // we do the actual work on whatever happens to be the first request that needs Html, without knowing how important
        // that request is, nor why it might be cancelled. If that request is cancelled because of a document update,
        // then the next request will cancel any work we start anyway, and if that request was cancelled for some other reason,
        // then the next request probably wants the same version of the document, so we've got a head start.
        return await GetSynchronizationRequestTaskAsync(document, requestedVersion).ConfigureAwait(false);
    }

    private Task<SynchronizationResult> GetSynchronizationRequestTaskAsync(TextDocument document, RazorDocumentVersion requestedVersion)
    {
        lock (_gate)
        {
            var documentUri = document.CreateUri();
            if (_synchronizationRequests.TryGetValue(documentUri, out var request))
            {
                if (requestedVersion.Checksum.Equals(request.RequestedVersion.Checksum))
                {
                    // Two documents are always equal if their checksums are equal, for the purposes of Html document generation, because
                    // Html documents don't require semantic information. WorkspaceVersion changed too often to be used as a measure
                    // of equality for this purpose.

#pragma warning disable VSTHRD103 // Use await instead of .Result
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
                    if (request.Task.IsCompleted && request.Task.Result.Equals(default))
                    {
                        _logger.LogDebug($"Already finished that version for {document.FilePath}, but was unsuccessful, so will recompute");
                        request.Dispose();
                    }
                    else
                    {
                        _logger.LogDebug($"Already {(request.Task.IsCompleted ? "finished" : "working on")} that version for {document.FilePath}");
                        return request.Task;
                    }
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
#pragma warning restore VSTHRD103 // Use await instead of .Result
                }
                else if (requestedVersion.WorkspaceVersion < request.RequestedVersion.WorkspaceVersion)
                {
                    // We know the documents aren't the same, but checksums can't tell us which is newer, so we use WorkspaceVersion for that.
                    // It is theoretically possible, however, that two different documents could have the same WorkspaceVersion, so we use the
                    // fact that LSP change messages are strictly ordered, and only move the document forward, such that if we get a request
                    // for a different checksum, but the same workspace version, we assume the new request is the newer document.

                    _logger.LogDebug($"We've already seen {request.RequestedVersion} for {document.FilePath} so that's a no from me");
                    return SpecializedTasks.Default<SynchronizationResult>();
                }
                else if (!request.Task.IsCompleted)
                {
                    // We've had a previous request, but this is newer, and our previous work hasn't finished yet
                    _logger.LogDebug($"We were working on {request.RequestedVersion} for {document.FilePath} but you're newer so we're giving up on that");
                    request.Dispose();
                }
            }

            _logger.LogDebug($"Going to start working on Html for {document.FilePath} as at {requestedVersion}");

            var newRequest = SynchronizationRequest.CreateAndStart(document, requestedVersion, PublishHtmlDocumentAsync);
            _synchronizationRequests[documentUri] = newRequest;
            return newRequest.Task;
        }
    }

    private async Task<SynchronizationResult> PublishHtmlDocumentAsync(TextDocument document, RazorDocumentVersion requestedVersion, CancellationToken cancellationToken)
    {
        string? htmlText;
        try
        {
            htmlText = await _remoteServiceInvoker.TryInvokeAsync<IRemoteHtmlDocumentService, string?>(document.Project.Solution,
                (service, solutionInfo, ct) => service.GetHtmlDocumentTextAsync(solutionInfo, document.Id, ct),
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting Html text for {document.FilePath}. Html document contents will be stale");
            return default;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            // Checking cancellation before logging, as a new request coming in doesn't count as "Couldn't get Html"
            return default;
        }

        if (htmlText is null)
        {
            _logger.LogError($"Couldn't get Html text for {document.FilePath}. Html document contents will be stale");
            return default;
        }

        try
        {
            var result = new SynchronizationResult(true, requestedVersion.Checksum);
            await _htmlDocumentPublisher.PublishAsync(document, result, htmlText, cancellationToken).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error publishing Html text for {document.FilePath}. Html document contents will be stale");
            return default;
        }
    }

    internal TestAccessor GetTestAccessor()
    {
        return new TestAccessor(this);
    }

    internal readonly struct TestAccessor
    {
        private readonly HtmlDocumentSynchronizer _instance;

        internal TestAccessor(HtmlDocumentSynchronizer instance)
        {
            _instance = instance;
        }

        public Task<SynchronizationResult> GetSynchronizationRequestTaskAsync(TextDocument document, RazorDocumentVersion requestedVersion)
            => _instance.GetSynchronizationRequestTaskAsync(document, requestedVersion);
    }
}

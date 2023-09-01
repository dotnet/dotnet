// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

[Export(typeof(LSPDocumentChangeListener))]
[ContentType(CodeRemoteContentDefinition.CodeRemoteContentTypeName)]
[Export(typeof(LSPDocumentSynchronizer))]
internal class DefaultLSPDocumentSynchronizer : LSPDocumentSynchronizer
{
    // Internal for testing
    private readonly LSPDocumentManager _documentManager;
    internal TimeSpan _synchronizationTimeout = TimeSpan.FromSeconds(2);
    private readonly Dictionary<Uri, DocumentContext> _virtualDocumentContexts;
    private readonly object _documentContextLock = new();
    private readonly FileUriProvider _fileUriProvider;

    [ImportingConstructor]
    public DefaultLSPDocumentSynchronizer(FileUriProvider fileUriProvider, LSPDocumentManager documentManager)
    {
        if (fileUriProvider is null)
        {
            throw new ArgumentNullException(nameof(fileUriProvider));
        }

        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        _fileUriProvider = fileUriProvider;
        _virtualDocumentContexts = new Dictionary<Uri, DocumentContext>();
        _documentManager = documentManager;
    }

    internal record SynchronizedResult<TVirtualDocumentSnapshot>(bool Synchronized, TVirtualDocumentSnapshot VirtualSnapshot)
        where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
    }

    public override Task<SynchronizedResult<TVirtualDocumentSnapshot>> TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(
        int requiredHostDocumentVersion,
        Uri hostDocumentUri,
        CancellationToken cancellationToken)
        where TVirtualDocumentSnapshot : class
        => TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(
            requiredHostDocumentVersion,
            hostDocumentUri,
            rejectOnNewerParallelRequest: true,
            cancellationToken);

    public override Task<SynchronizedResult<TVirtualDocumentSnapshot>> TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(
        int requiredHostDocumentVersion,
        Uri hostDocumentUri,
        bool rejectOnNewerParallelRequest,
        CancellationToken cancellationToken)
        where TVirtualDocumentSnapshot : class
        => TrySynchronizeVirtualDocumentCoreAsync<TVirtualDocumentSnapshot>(
            requiredHostDocumentVersion,
            hostDocumentUri,
            specificVirtualDocumentUri: null,
            rejectOnNewerParallelRequest,
            cancellationToken);

    public override Task<SynchronizedResult<TVirtualDocumentSnapshot>> TrySynchronizeVirtualDocumentAsync<TVirtualDocumentSnapshot>(
        int requiredHostDocumentVersion,
        Uri hostDocumentUri,
        Uri virtualDocumentUri,
        bool rejectOnNewerParallelRequest,
        CancellationToken cancellationToken)
        where TVirtualDocumentSnapshot : class
        => TrySynchronizeVirtualDocumentCoreAsync<TVirtualDocumentSnapshot>(
            requiredHostDocumentVersion,
            hostDocumentUri,
            virtualDocumentUri,
            rejectOnNewerParallelRequest,
            cancellationToken);

    private async Task<SynchronizedResult<TVirtualDocumentSnapshot>> TrySynchronizeVirtualDocumentCoreAsync<TVirtualDocumentSnapshot>(
        int requiredHostDocumentVersion,
        Uri hostDocumentUri,
        Uri? specificVirtualDocumentUri,
        bool rejectOnNewerParallelRequest,
        CancellationToken cancellationToken)
        where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        if (hostDocumentUri is null)
        {
            throw new ArgumentNullException(nameof(hostDocumentUri));
        }

        Task<bool> onSynchronizedTask;
        lock (_documentContextLock)
        {
            var preSyncedSnapshot = GetVirtualDocumentSnapshot<TVirtualDocumentSnapshot>(hostDocumentUri, specificVirtualDocumentUri);
            var virtualDocumentUri = preSyncedSnapshot.Uri;
            if (!_virtualDocumentContexts.TryGetValue(virtualDocumentUri, out var documentContext))
            {
                // Document was deleted/removed in mid-synchronization
                return new SynchronizedResult<TVirtualDocumentSnapshot>(false, preSyncedSnapshot);
            }

            if (requiredHostDocumentVersion == documentContext.SeenHostDocumentVersion)
            {
                // Already synchronized
                return new SynchronizedResult<TVirtualDocumentSnapshot>(true, preSyncedSnapshot);
            }

            // Currently tracked synchronizing context is not sufficient, need to update a new one.
            onSynchronizedTask = documentContext.GetSynchronizationTaskAsync(requiredHostDocumentVersion, rejectOnNewerParallelRequest, cancellationToken);
        }

        var onSynchronizedResult = await onSynchronizedTask.ConfigureAwait(false);

        // If we couldn't synchronize, there might not be a virtual document with the specific Uri, so we just get whichever one we can
        // so the caller can use it if they want to. Since the result is false, they hopefully don't use it for much!
        var virtualDocumentSnapshot = GetVirtualDocumentSnapshot<TVirtualDocumentSnapshot>(hostDocumentUri, onSynchronizedResult ? specificVirtualDocumentUri : null);

        return new SynchronizedResult<TVirtualDocumentSnapshot>(onSynchronizedResult, virtualDocumentSnapshot);
    }

    internal SynchronizedResult<TVirtualDocumentSnapshot>? TryReturnPossiblyFutureSnapshot<TVirtualDocumentSnapshot>(
        int requiredHostDocumentVersion,
        Uri hostDocumentUri) where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
        => TryReturnPossiblyFutureSnapshot<TVirtualDocumentSnapshot>(
            requiredHostDocumentVersion,
            hostDocumentUri,
            specificVirtualDocumentUri: null);

    internal SynchronizedResult<TVirtualDocumentSnapshot>? TryReturnPossiblyFutureSnapshot<TVirtualDocumentSnapshot>(
        int requiredHostDocumentVersion,
        Uri hostDocumentUri,
        Uri? specificVirtualDocumentUri) where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        lock (_documentContextLock)
        {
            var preSyncedSnapshot = GetVirtualDocumentSnapshot<TVirtualDocumentSnapshot>(hostDocumentUri, specificVirtualDocumentUri);
            var virtualDocumentUri = preSyncedSnapshot.Uri;
            if (!_virtualDocumentContexts.TryGetValue(virtualDocumentUri, out var documentContext))
            {
                // Document was deleted/removed in mid-synchronization
                return new SynchronizedResult<TVirtualDocumentSnapshot>(false, preSyncedSnapshot);
            }

            if (requiredHostDocumentVersion <= documentContext.SeenHostDocumentVersion)
            {
                // Already synchronized
                return new SynchronizedResult<TVirtualDocumentSnapshot>(true, preSyncedSnapshot);
            }
        }

        return null;
    }

    [Obsolete]
    public override Task<bool> TrySynchronizeVirtualDocumentAsync(int requiredHostDocumentVersion, VirtualDocumentSnapshot virtualDocument, CancellationToken cancellationToken)
        => TrySynchronizeVirtualDocumentAsync(requiredHostDocumentVersion, virtualDocument, rejectOnNewerParallelRequest: true, cancellationToken);

    [Obsolete]
    public override Task<bool> TrySynchronizeVirtualDocumentAsync(int requiredHostDocumentVersion, VirtualDocumentSnapshot virtualDocument, bool rejectOnNewerParallelRequest, CancellationToken cancellationToken)
    {
        if (virtualDocument is null)
        {
            throw new ArgumentNullException(nameof(virtualDocument));
        }

        lock (_documentContextLock)
        {
            if (!_virtualDocumentContexts.TryGetValue(virtualDocument.Uri, out var documentContext))
            {
                // Document was deleted/removed in mid-synchronization
                return Task.FromResult(false);
            }

            if (requiredHostDocumentVersion == documentContext.SeenHostDocumentVersion)
            {
                // Already synchronized
                return Task.FromResult(true);
            }

            // Currently tracked synchronizing context is not sufficient, need to update a new one.
            var onSynchronizedTask = documentContext.GetSynchronizationTaskAsync(requiredHostDocumentVersion, rejectOnNewerParallelRequest, cancellationToken);
            return onSynchronizedTask;
        }
    }

    private TVirtualDocumentSnapshot GetVirtualDocumentSnapshot<TVirtualDocumentSnapshot>(Uri hostDocumentUri, Uri? specificVirtualDocumentUri)
        where TVirtualDocumentSnapshot : VirtualDocumentSnapshot
    {
        var normalizedString = hostDocumentUri.GetAbsoluteOrUNCPath();
        var normalizedUri = new Uri(normalizedString);

        if (!_documentManager.TryGetDocument(normalizedUri, out var documentSnapshot))
        {
            throw new InvalidOperationException($"Unable to retrieve snapshot for document {normalizedUri} after synchronization");
        }

        if (specificVirtualDocumentUri is not null)
        {
            if (!documentSnapshot.TryGetAllVirtualDocuments<TVirtualDocumentSnapshot>(out var virtualDocuments))
            {
                throw new InvalidOperationException($"Unable to retrieve virtual documents for {normalizedUri} after document synchronization");
            }

            foreach (var virtualDocument in virtualDocuments)
            {
                if (virtualDocument.Uri == specificVirtualDocumentUri)
                {
                    return virtualDocument;
                }
            }

            throw new InvalidOperationException($"Unable to retrieve virtual document {specificVirtualDocumentUri} for {normalizedUri} after document synchronization");
        }

        if (!documentSnapshot.TryGetVirtualDocument<TVirtualDocumentSnapshot>(out var virtualDoc))
        {
            throw new InvalidOperationException($"Unable to retrieve virtual document for {normalizedUri} after document synchronization");
        }

        return virtualDoc;
    }

    private void VirtualDocumentBuffer_PostChanged(object sender, EventArgs e)
    {
        var textBuffer = (ITextBuffer)sender;

        UpdateDocumentContextVersionInternal(textBuffer);
    }

    private void UpdateDocumentContextVersionInternal(ITextBuffer textBuffer)
    {
        if (!_fileUriProvider.TryGet(textBuffer, out var virtualDocumentUri))
        {
            return;
        }

        lock (_documentContextLock)
        {
            if (!_virtualDocumentContexts.TryGetValue(virtualDocumentUri, out var documentContext))
            {
                return;
            }

            if (!textBuffer.TryGetHostDocumentSyncVersion(out var hostDocumentVersion))
            {
                return;
            }

            documentContext.UpdateSeenDocumentVersion(hostDocumentVersion);
        }
    }

    public override void Changed(LSPDocumentSnapshot? old, LSPDocumentSnapshot? @new, VirtualDocumentSnapshot? virtualOld, VirtualDocumentSnapshot? virtualNew, LSPDocumentChangeKind kind)
    {
        lock (_documentContextLock)
        {
            if (kind == LSPDocumentChangeKind.Added)
            {
                var lspDocument = @new!;
                for (var i = 0; i < lspDocument.VirtualDocuments.Count; i++)
                {
                    var virtualDocument = lspDocument.VirtualDocuments[i];

                    Debug.Assert(!_virtualDocumentContexts.ContainsKey(virtualDocument.Uri));

                    var virtualDocumentTextBuffer = virtualDocument.Snapshot.TextBuffer;
                    virtualDocumentTextBuffer.PostChanged += VirtualDocumentBuffer_PostChanged;
                    _virtualDocumentContexts[virtualDocument.Uri] = new DocumentContext(_synchronizationTimeout);
                }
            }
            else if (kind == LSPDocumentChangeKind.Removed)
            {
                var lspDocument = old!;
                for (var i = 0; i < lspDocument.VirtualDocuments.Count; i++)
                {
                    var virtualDocument = lspDocument.VirtualDocuments[i];

                    if (!_virtualDocumentContexts.TryGetValue(virtualDocument.Uri, out var virtualDocumentContext))
                    {
                        Debug.Fail("Could not locate virtual document context, it should have been added.");
                        continue;
                    }

                    var virtualDocumentTextBuffer = virtualDocument.Snapshot.TextBuffer;
                    virtualDocumentTextBuffer.PostChanged -= VirtualDocumentBuffer_PostChanged;

                    virtualDocumentContext.Dispose();
                    _virtualDocumentContexts.Remove(virtualDocument.Uri);
                }
            }
            else if (kind == LSPDocumentChangeKind.VirtualDocumentChanged)
            {
                if (virtualOld!.Snapshot.Version == virtualNew!.Snapshot.Version)
                {
                    // UpdateDocumentContextVersionInternal is typically invoked through a buffer notification,
                    //   however in the case where VirtualDocumentBase.Update is called with a zero change edit,
                    //   there won't be such an edit to hook into. Instead, we'll detect that case here and
                    //   update the document context version appropriately.
                    UpdateDocumentContextVersionInternal(virtualNew.Snapshot.TextBuffer);
                }
            }
        }
    }

    private class DocumentContext : IDisposable
    {
        private readonly TimeSpan _synchronizingTimeout;
        private readonly List<DocumentSynchronizingContext> _synchronizingContexts;

        public DocumentContext(TimeSpan synchronizingTimeout)
        {
            _synchronizingTimeout = synchronizingTimeout;
            _synchronizingContexts = new List<DocumentSynchronizingContext>();
        }

        public long SeenHostDocumentVersion { get; private set; }

        public void UpdateSeenDocumentVersion(long seenDocumentVersion)
        {
            SeenHostDocumentVersion = seenDocumentVersion;

            if (_synchronizingContexts.Count == 0)
            {
                // No active synchronizing context for this document.
                return;
            }

            for (var i = _synchronizingContexts.Count - 1; i >= 0; i--)
            {
                var synchronizingContext = _synchronizingContexts[i];
                if (SeenHostDocumentVersion == synchronizingContext.RequiredHostDocumentVersion)
                {
                    // We're now synchronized!

                    synchronizingContext.SetSynchronized(true);
                    _synchronizingContexts.RemoveAt(i);
                }
                else if (SeenHostDocumentVersion > synchronizingContext.RequiredHostDocumentVersion)
                {
                    // The LSP document version has surpassed what the projected document was expecting for a version. No longer able to synchronize.
                    synchronizingContext.SetSynchronized(false);
                    _synchronizingContexts.RemoveAt(i);
                }
                else
                {
                    // Seen host document version is less than the required version, need to wait longer.
                }
            }
        }

        public Task<bool> GetSynchronizationTaskAsync(int requiredHostDocumentVersion, bool rejectOnNewerParallelRequest, CancellationToken cancellationToken)
        {
            // Cancel any out-of-date existing synchronizing contexts.

            for (var i = _synchronizingContexts.Count - 1; i >= 0; i--)
            {
                var context = _synchronizingContexts[i];
                if (context.RejectOnNewerParallelRequest &&
                    context.RequiredHostDocumentVersion < requiredHostDocumentVersion)
                {
                    // All of the existing synchronizations that are older than this version are no longer valid.
                    context.SetSynchronized(result: false);
                    _synchronizingContexts.RemoveAt(i);
                }
            }

            var synchronizingContext = new DocumentSynchronizingContext(requiredHostDocumentVersion, rejectOnNewerParallelRequest, _synchronizingTimeout, cancellationToken);
            _synchronizingContexts.Add(synchronizingContext);
            return synchronizingContext.OnSynchronizedAsync;
        }

        public void Dispose()
        {
            for (var i = _synchronizingContexts.Count - 1; i >= 0; i--)
            {
                _synchronizingContexts[i].SetSynchronized(result: false);
            }

            _synchronizingContexts.Clear();
        }

        private class DocumentSynchronizingContext
        {
            private readonly TaskCompletionSource<bool> _onSynchronizedSource;
            private readonly CancellationTokenSource _cts;
            private bool _synchronizedSet;

            public DocumentSynchronizingContext(
                int requiredHostDocumentVersion,
                bool rejectOnNewerParallelRequest,
                TimeSpan timeout,
                CancellationToken requestCancellationToken)
            {
                RequiredHostDocumentVersion = requiredHostDocumentVersion;
                RejectOnNewerParallelRequest = rejectOnNewerParallelRequest;
                _onSynchronizedSource = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

                _cts = CancellationTokenSource.CreateLinkedTokenSource(requestCancellationToken);

                // This might throw because the token has already been marked as cancelled
                try
                {
                    // This cancellation token is the one passed in from the call-site that needs to synchronize an LSP document with a virtual document.
                    // Meaning, if the outer token is cancelled we need to fail to synchronize.
                    _cts.Token.Register(() => SetSynchronized(false));
                    _cts.CancelAfter(timeout);
                }
                catch (ObjectDisposedException)
                {
                    SetSynchronized(false);
                }
            }

            public bool RejectOnNewerParallelRequest { get; }

            public int RequiredHostDocumentVersion { get; }

            public Task<bool> OnSynchronizedAsync => _onSynchronizedSource.Task;

            public void SetSynchronized(bool result)
            {
                lock (_onSynchronizedSource)
                {
                    if (_synchronizedSet)
                    {
                        return;
                    }

                    _synchronizedSet = true;
                }

                _cts.Dispose();
                _onSynchronizedSource.SetResult(result);
            }
        }
    }
}

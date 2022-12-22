// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

[Shared]
[Export(typeof(LSPDocumentManager))]
internal class DefaultLSPDocumentManager : TrackingLSPDocumentManager
{
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly FileUriProvider _fileUriProvider;
    private readonly LSPDocumentFactory _documentFactory;
    private readonly ConcurrentDictionary<Uri, LSPDocument> _documents;
    private readonly IEnumerable<Lazy<LSPDocumentChangeListener, IContentTypeMetadata>> _documentManagerChangeListeners;

    [ImportingConstructor]
    public DefaultLSPDocumentManager(
        JoinableTaskContext joinableTaskContext,
        FileUriProvider fileUriProvider,
        LSPDocumentFactory documentFactory,
        [ImportMany] IEnumerable<Lazy<LSPDocumentChangeListener, IContentTypeMetadata>> documentManagerChangeListeners)
    {
        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (fileUriProvider is null)
        {
            throw new ArgumentNullException(nameof(fileUriProvider));
        }

        if (documentFactory is null)
        {
            throw new ArgumentNullException(nameof(documentFactory));
        }

        if (documentManagerChangeListeners is null)
        {
            throw new ArgumentNullException(nameof(documentManagerChangeListeners));
        }

        _joinableTaskContext = joinableTaskContext;
        _fileUriProvider = fileUriProvider;
        _documentFactory = documentFactory;
        _documentManagerChangeListeners = documentManagerChangeListeners;
        _documents = new ConcurrentDictionary<Uri, LSPDocument>();
    }

    public override void TrackDocument(ITextBuffer buffer)
    {
        if (buffer is null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        Debug.Assert(_joinableTaskContext.IsOnMainThread);

        var uri = _fileUriProvider.GetOrCreate(buffer);
        if (_documents.TryGetValue(uri, out _))
        {
            throw new InvalidOperationException($"Can not track document that's already being tracked {uri}");
        }

        var lspDocument = _documentFactory.Create(buffer);
        _documents[uri] = lspDocument;

        NotifyDocumentManagerChangeListeners(
            old: null,
            @new: lspDocument.CurrentSnapshot,
            virtualOld: null,
            virtualNew: null,
            LSPDocumentChangeKind.Added);
    }

    public override void UntrackDocument(ITextBuffer buffer)
    {
        if (buffer is null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        Debug.Assert(_joinableTaskContext.IsOnMainThread);

        var uri = _fileUriProvider.GetOrCreate(buffer);
        if (!_documents.TryGetValue(uri, out var lspDocument))
        {
            // We don't know about this document, noop.
            return;
        }

        // Given we're no longer tracking the document we don't want to pin the Uri to the current state of the buffer (could have been renamed to another Uri).
        _fileUriProvider.Remove(buffer);

        if (_documents.TryRemove(uri, out _))
        {
            NotifyDocumentManagerChangeListeners(
                lspDocument.CurrentSnapshot,
                @new: null,
                virtualOld: null,
                virtualNew: null,
                LSPDocumentChangeKind.Removed);
        }
        else
        {
            Debug.Fail($"Couldn't remove {uri.AbsolutePath}. This should never ever happen.");
        }

        lspDocument.Dispose();
    }

    public override void UpdateVirtualDocument<TVirtualDocument>(
        Uri hostDocumentUri,
        IReadOnlyList<ITextChange> changes,
        int hostDocumentVersion,
        object? state)
    {
        if (hostDocumentUri is null)
        {
            throw new ArgumentNullException(nameof(hostDocumentUri));
        }

        if (changes is null)
        {
            throw new ArgumentNullException(nameof(changes));
        }

        Debug.Assert(_joinableTaskContext.IsOnMainThread);

        if (!_documents.TryGetValue(hostDocumentUri, out var lspDocument))
        {
            // Don't know about document, noop.
            return;
        }

        if (!lspDocument.TryGetVirtualDocument<TVirtualDocument>(out var virtualDocument))
        {
            // Unable to locate virtual document of typeof(TVirtualDocument)
            // Ex. Microsoft.WebTools.Languages.LanguageServer.Delegation.ContainedLanguage.Css.CssVirtualDocument
            return;
        }

        if (changes.Count == 0 &&
            virtualDocument.HostDocumentVersion == hostDocumentVersion)
        {
            // The current virtual document already knows about this update.
            // Ignore it so we don't prematurely invoke a change event.
            return;
        }

        var old = lspDocument.CurrentSnapshot;
        var oldVirtual = virtualDocument.CurrentSnapshot;
        var @new = lspDocument.UpdateVirtualDocument<TVirtualDocument>(changes, hostDocumentVersion, state);

        if (old == @new)
        {
            return;
        }

        if (!lspDocument.TryGetVirtualDocument<TVirtualDocument>(out var newVirtualDocument))
        {
            throw new InvalidOperationException("This should never ever happen.");
        }

        var newVirtual = newVirtualDocument.CurrentSnapshot;
        NotifyDocumentManagerChangeListeners(old, @new, oldVirtual, newVirtual, LSPDocumentChangeKind.VirtualDocumentChanged);
    }

    public override bool TryGetDocument(Uri uri, [NotNullWhen(returnValue: true)] out LSPDocumentSnapshot? lspDocumentSnapshot)
    {
        if (!_documents.TryGetValue(uri, out var lspDocument))
        {
            // This should never happen in practice but return `null` so our tests can validate
            lspDocumentSnapshot = null;
            return false;
        }

        lspDocumentSnapshot = lspDocument.CurrentSnapshot;
        return true;
    }

    private void NotifyDocumentManagerChangeListeners(
        LSPDocumentSnapshot? old,
        LSPDocumentSnapshot? @new,
        VirtualDocumentSnapshot? virtualOld,
        VirtualDocumentSnapshot? virtualNew,
        LSPDocumentChangeKind kind)
    {
        foreach (var listener in _documentManagerChangeListeners)
        {
            var notifyListener = false;

            if (old is not null &&
                listener.Metadata.ContentTypes.Any(old.Snapshot.ContentType.IsOfType))
            {
                notifyListener = true;
            }
            else if (@new is not null &&
                listener.Metadata.ContentTypes.Any(@new.Snapshot.ContentType.IsOfType))
            {
                notifyListener = true;
            }

            if (notifyListener)
            {
                listener.Value.Changed(old, @new, virtualOld, virtualNew, kind);
            }
        }
    }
}

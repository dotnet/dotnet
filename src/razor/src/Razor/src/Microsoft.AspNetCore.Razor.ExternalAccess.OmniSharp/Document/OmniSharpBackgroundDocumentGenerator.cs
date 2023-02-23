// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Document;

public class OmniSharpBackgroundDocumentGenerator : IOmniSharpProjectSnapshotManagerChangeTrigger
{
    private readonly BackgroundDocumentGenerator _backgroundDocumentGenerator;

    public OmniSharpBackgroundDocumentGenerator(
        OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        RemoteTextLoaderFactory remoteTextLoaderFactory,
        IEnumerable<OmniSharpDocumentProcessedListener> documentProcessedListeners)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (remoteTextLoaderFactory is null)
        {
            throw new ArgumentNullException(nameof(remoteTextLoaderFactory));
        }

        if (documentProcessedListeners is null)
        {
            throw new ArgumentNullException(nameof(documentProcessedListeners));
        }

        var wrappedListeners = documentProcessedListeners.Select(listener => new WrappedDocumentProcessedListener(remoteTextLoaderFactory, listener));
        _backgroundDocumentGenerator = new BackgroundDocumentGenerator(projectSnapshotManagerDispatcher.InternalDispatcher, wrappedListeners);
    }

    public void Initialize(OmniSharpProjectSnapshotManager projectManager)
    {
        _backgroundDocumentGenerator.Initialize(projectManager.InternalProjectSnapshotManager);
    }

    private class WrappedDocumentProcessedListener : DocumentProcessedListener
    {
        private readonly RemoteTextLoaderFactory _remoteTextLoaderFactory;
        private readonly OmniSharpDocumentProcessedListener _innerDocumentProcessedListener;

        internal WrappedDocumentProcessedListener(
            RemoteTextLoaderFactory remoteTextLoaderFactory,
            OmniSharpDocumentProcessedListener innerDocumentProcessedListener)
        {
            if (remoteTextLoaderFactory is null)
            {
                throw new ArgumentNullException(nameof(remoteTextLoaderFactory));
            }

            if (innerDocumentProcessedListener is null)
            {
                throw new ArgumentNullException(nameof(innerDocumentProcessedListener));
            }

            _remoteTextLoaderFactory = remoteTextLoaderFactory;
            _innerDocumentProcessedListener = innerDocumentProcessedListener;
        }

        public override void DocumentProcessed(RazorCodeDocument codeDocument, IDocumentSnapshot document)
        {
            var omniSharpDocument = new OmniSharpDocumentSnapshot(document);
            _innerDocumentProcessedListener.DocumentProcessed(codeDocument, omniSharpDocument);
        }

        public override void Initialize(ProjectSnapshotManager projectManager)
        {
            var omniSharpProjectManager = new OmniSharpProjectSnapshotManager((ProjectSnapshotManagerBase)projectManager, _remoteTextLoaderFactory);
            _innerDocumentProcessedListener.Initialize(omniSharpProjectManager);
        }
    }
}

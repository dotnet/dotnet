// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultDocumentVersionCache : DocumentVersionCache
{
    internal const int MaxDocumentTrackingCount = 20;

    // Internal for testing
    internal readonly Dictionary<string, List<DocumentEntry>> DocumentLookup;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher;
    private ProjectSnapshotManagerBase? _projectSnapshotManager;

    private ProjectSnapshotManagerBase ProjectSnapshotManager
        => _projectSnapshotManager ?? throw new InvalidOperationException("ProjectSnapshotManager accessed before Initialized was called.");

    public DefaultDocumentVersionCache(ProjectSnapshotManagerDispatcher dispatcher)
    {
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        DocumentLookup = new Dictionary<string, List<DocumentEntry>>(FilePathComparer.Instance);
    }

    public override void TrackDocumentVersion(IDocumentSnapshot documentSnapshot, int version)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        _dispatcher.AssertDispatcherThread();

        var filePath = documentSnapshot.FilePath.AssumeNotNull();

        if (!DocumentLookup.TryGetValue(filePath, out var documentEntries))
        {
            documentEntries = new List<DocumentEntry>();
            DocumentLookup[filePath] = documentEntries;
        }

        if (documentEntries.Count == MaxDocumentTrackingCount)
        {
            // Clear the oldest document entry

            // With this approach we'll slowly leak memory as new documents are added to the system. We don't clear up
            // document file paths where where all of the corresponding entries are expired.
            documentEntries.RemoveAt(0);
        }

        var entry = new DocumentEntry(documentSnapshot, version);
        documentEntries.Add(entry);
    }

    public override bool TryGetDocumentVersion(IDocumentSnapshot documentSnapshot, [NotNullWhen(true)] out int? version)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        _dispatcher.AssertDispatcherThread();

        var filePath = documentSnapshot.FilePath.AssumeNotNull();

        if (!DocumentLookup.TryGetValue(filePath, out var documentEntries))
        {
            version = null;
            return false;
        }

        DocumentEntry? entry = null;
        for (var i = documentEntries.Count - 1; i >= 0; i--)
        {
            // We iterate backwards over the entries to prioritize newer entries.
            if (documentEntries[i].Document.TryGetTarget(out var document) &&
                document == documentSnapshot)
            {
                entry = documentEntries[i];
                break;
            }
        }

        if (entry is null)
        {
            version = null;
            return false;
        }

        version = entry.Version;
        return true;
    }

    public override Task<int?> TryGetDocumentVersionAsync(IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        return _dispatcher.RunOnDispatcherThreadAsync(
            () =>
            {
                TryGetDocumentVersion(documentSnapshot, out var version);
                return version;
            },
            cancellationToken);
    }

    public override void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        if (projectManager is null)
        {
            throw new ArgumentNullException(nameof(projectManager));
        }

        _projectSnapshotManager = projectManager;
        ProjectSnapshotManager.Changed += ProjectSnapshotManager_Changed;
    }

    private void ProjectSnapshotManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't do any work if the solution is closing
        if (args.SolutionIsClosing)
        {
            return;
        }

        _dispatcher.AssertDispatcherThread();

        switch (args.Kind)
        {
            case ProjectChangeKind.DocumentChanged:
                var documentFilePath = args.DocumentFilePath!;
                if (DocumentLookup.ContainsKey(documentFilePath) &&
                    !ProjectSnapshotManager.IsDocumentOpen(documentFilePath))
                {
                    // Document closed, evict entry.
                    DocumentLookup.Remove(documentFilePath);
                }

                break;
        }

        // Any event that has a project may have changed the state of the documents
        // and therefore requires us to mark all existing documents as latest.
        if (args.ProjectKey is null)
        {
            return;
        }

        var project = ProjectSnapshotManager.GetLoadedProject(args.ProjectKey);
        if (project is null)
        {
            // Project no longer loaded, wait for document removed event.
            return;
        }

        CaptureProjectDocumentsAsLatest(project);
    }

    // Internal for testing
    internal void MarkAsLatestVersion(IDocumentSnapshot document)
    {
        var filePath = document.FilePath.AssumeNotNull();

        if (!TryGetLatestVersionFromPath(filePath, out var latestVersion))
        {
            return;
        }

        // Update our internal tracking state to track the changed document as the latest document.
        TrackDocumentVersion(document, latestVersion.Value);
    }

    // Internal for testing
    internal bool TryGetLatestVersionFromPath(string filePath, [NotNullWhen(true)] out int? version)
    {
        if (!DocumentLookup.TryGetValue(filePath, out var documentEntries))
        {
            version = null;
            return false;
        }

        var latestEntry = documentEntries[^1];

        version = latestEntry.Version;
        return true;
    }

    private void CaptureProjectDocumentsAsLatest(IProjectSnapshot projectSnapshot)
    {
        foreach (var documentPath in projectSnapshot.DocumentFilePaths)
        {
            if (DocumentLookup.ContainsKey(documentPath) &&
                projectSnapshot.GetDocument(documentPath) is { } document)
            {
                MarkAsLatestVersion(document);
            }
        }
    }

    internal class DocumentEntry
    {
        public DocumentEntry(IDocumentSnapshot document, int version)
        {
            Document = new WeakReference<IDocumentSnapshot>(document);
            Version = version;
        }

        public WeakReference<IDocumentSnapshot> Document { get; }

        public int Version { get; }
    }
}

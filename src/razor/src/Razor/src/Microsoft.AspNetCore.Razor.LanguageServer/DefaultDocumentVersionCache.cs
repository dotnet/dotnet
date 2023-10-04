// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultDocumentVersionCache : DocumentVersionCache
{
    internal const int MaxDocumentTrackingCount = 20;

    // Internal for testing
    internal readonly Dictionary<string, List<DocumentEntry>> DocumentLookup_NeedsLock;
    private readonly ReadWriterLocker _lock = new();
    private ProjectSnapshotManagerBase? _projectSnapshotManager;

    private ProjectSnapshotManagerBase ProjectSnapshotManager
        => _projectSnapshotManager ?? throw new InvalidOperationException("ProjectSnapshotManager accessed before Initialized was called.");

    public DefaultDocumentVersionCache()
    {
        DocumentLookup_NeedsLock = new Dictionary<string, List<DocumentEntry>>(FilePathComparer.Instance);
    }

    public override void TrackDocumentVersion(IDocumentSnapshot documentSnapshot, int version)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        using var upgradeableReadLock = _lock.EnterUpgradeAbleReadLock();
        TrackDocumentVersion(documentSnapshot, version, upgradeableReadLock);
    }

    private void TrackDocumentVersion(IDocumentSnapshot documentSnapshot, int version, ReadWriterLocker.UpgradeableReadLock upgradeableReadLock)
    {
        // Need to ensure the write lock covers all uses of documentEntries, not just DocumentLookup
        using (upgradeableReadLock.EnterWriteLock())
        {
            var key = documentSnapshot.FilePath.AssumeNotNull();
            if (!DocumentLookup_NeedsLock.TryGetValue(key, out var documentEntries))
            {
                documentEntries = new List<DocumentEntry>();
                DocumentLookup_NeedsLock[key] = documentEntries;
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
    }

    public override bool TryGetDocumentVersion(IDocumentSnapshot documentSnapshot, [NotNullWhen(true)] out int? version)
    {
        if (documentSnapshot is null)
        {
            throw new ArgumentNullException(nameof(documentSnapshot));
        }

        using var _ = _lock.EnterReadLock();

        var key = documentSnapshot.FilePath.AssumeNotNull();
        if (!DocumentLookup_NeedsLock.TryGetValue(key, out var documentEntries))
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

        var upgradeableLock = _lock.EnterUpgradeAbleReadLock();

        switch (args.Kind)
        {
            case ProjectChangeKind.DocumentChanged:
                var documentFilePath = args.DocumentFilePath!;
                if (DocumentLookup_NeedsLock.ContainsKey(documentFilePath) &&
                    !ProjectSnapshotManager.IsDocumentOpen(documentFilePath))
                {
                    using (upgradeableLock.EnterWriteLock())
                    {
                        // Document closed, evict entry.
                        DocumentLookup_NeedsLock.Remove(documentFilePath);
                    }
                }

                break;
        }

        // Any event that has a project may have changed the state of the documents
        // and therefore requires us to mark all existing documents as latest.
        var project = ProjectSnapshotManager.GetLoadedProject(args.ProjectKey);
        if (project is null)
        {
            // Project no longer loaded, wait for document removed event.
            return;
        }

        CaptureProjectDocumentsAsLatest(project, upgradeableLock);
    }

    // Internal for testing
    internal void MarkAsLatestVersion(IDocumentSnapshot document)
    {
        using var upgradeableLock = _lock.EnterUpgradeAbleReadLock();
        MarkAsLatestVersion(document, upgradeableLock);
    }

    private void CaptureProjectDocumentsAsLatest(IProjectSnapshot projectSnapshot, ReadWriterLocker.UpgradeableReadLock upgradeableReadLock)
    {
        foreach (var documentPath in projectSnapshot.DocumentFilePaths)
        {
            if (DocumentLookup_NeedsLock.ContainsKey(documentPath) &&
                projectSnapshot.GetDocument(documentPath) is { } document)
            {
                MarkAsLatestVersion(document, upgradeableReadLock);
            }
        }
    }

    private void MarkAsLatestVersion(IDocumentSnapshot document, ReadWriterLocker.UpgradeableReadLock upgradeableReadLock)
    {
        if (!DocumentLookup_NeedsLock.TryGetValue(document.FilePath.AssumeNotNull(), out var documentEntries))
        {
            return;
        }

        var latestEntry = documentEntries[^1];

        // Update our internal tracking state to track the changed document as the latest document.
        TrackDocumentVersion(document, latestEntry.Version, upgradeableReadLock);
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

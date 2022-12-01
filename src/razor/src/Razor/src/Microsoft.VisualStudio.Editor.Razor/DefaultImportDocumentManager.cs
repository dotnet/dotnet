// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Editor.Razor.Documents;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class DefaultImportDocumentManager : ImportDocumentManager
{
    private readonly FileChangeTrackerFactory _fileChangeTrackerFactory;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly ErrorReporter _errorReporter;
    private readonly Dictionary<string, ImportTracker> _importTrackerCache;

    public override event EventHandler<ImportChangedEventArgs>? Changed;

    public DefaultImportDocumentManager(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ErrorReporter errorReporter,
        FileChangeTrackerFactory fileChangeTrackerFactory)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (errorReporter is null)
        {
            throw new ArgumentNullException(nameof(errorReporter));
        }

        if (fileChangeTrackerFactory is null)
        {
            throw new ArgumentNullException(nameof(fileChangeTrackerFactory));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _errorReporter = errorReporter;
        _fileChangeTrackerFactory = fileChangeTrackerFactory;
        _importTrackerCache = new Dictionary<string, ImportTracker>(StringComparer.OrdinalIgnoreCase);
    }

    public override void OnSubscribed(VisualStudioDocumentTracker tracker)
    {
        if (tracker is null)
        {
            throw new ArgumentNullException(nameof(tracker));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var imports = GetImportItems(tracker);
        foreach (var import in imports)
        {
            var importFilePath = import.PhysicalPath;
            Debug.Assert(importFilePath is not null);

            if (importFilePath is null)
                continue;

            if (!_importTrackerCache.TryGetValue(importFilePath, out var importTracker))
            {
                // First time seeing this import. Start tracking it.
                var fileChangeTracker = _fileChangeTrackerFactory.Create(importFilePath);
                importTracker = new ImportTracker(fileChangeTracker);
                _importTrackerCache[importFilePath] = importTracker;

                fileChangeTracker.Changed += FileChangeTracker_Changed;
                fileChangeTracker.StartListening();
            }

            importTracker.AssociatedDocuments.Add(tracker.FilePath);
        }
    }

    public override void OnUnsubscribed(VisualStudioDocumentTracker tracker)
    {
        if (tracker is null)
        {
            throw new ArgumentNullException(nameof(tracker));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var imports = GetImportItems(tracker);
        foreach (var import in imports)
        {
            var importFilePath = import.PhysicalPath;
            Assumes.NotNull(importFilePath);

            if (_importTrackerCache.TryGetValue(importFilePath, out var importTracker))
            {
                importTracker.AssociatedDocuments.Remove(tracker.FilePath);

                if (importTracker.AssociatedDocuments.Count == 0)
                {
                    // There are no open documents that care about this import. We no longer need to track it.
                    importTracker.FileChangeTracker.StopListening();
                    _importTrackerCache.Remove(importFilePath);
                }
            }
        }
    }

    private static IEnumerable<RazorProjectItem> GetImportItems(VisualStudioDocumentTracker tracker)
    {
        var projectEngine = tracker.ProjectSnapshot!.GetProjectEngine();
        var documentSnapshot = tracker.ProjectSnapshot.GetDocument(tracker.FilePath);
        var fileKind = documentSnapshot?.FileKind;
        var trackerItem = projectEngine.FileSystem.GetItem(tracker.FilePath, fileKind);
        var importFeatures = projectEngine.ProjectFeatures.OfType<IImportProjectFeature>();
        var importItems = importFeatures.SelectMany(f => f.GetImports(trackerItem));
        var physicalImports = importItems.Where(import => import.FilePath is not null);

        return physicalImports;
    }

    private void OnChanged(ImportTracker importTracker, FileChangeKind changeKind)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (Changed is null)
        {
            return;
        }

        var args = new ImportChangedEventArgs(importTracker.FilePath, changeKind, importTracker.AssociatedDocuments);
        Changed.Invoke(this, args);
}

    private void FileChangeTracker_Changed(object sender, FileChangeEventArgs args)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_importTrackerCache.TryGetValue(args.FilePath, out var importTracker))
        {
            OnChanged(importTracker, args.Kind);
        }
    }

    private class ImportTracker
    {
        public ImportTracker(FileChangeTracker fileChangeTracker)
        {
            FileChangeTracker = fileChangeTracker;
            AssociatedDocuments = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public string FilePath => FileChangeTracker.FilePath;

        public FileChangeTracker FileChangeTracker { get; }

        public HashSet<string> AssociatedDocuments { get; }
    }
}

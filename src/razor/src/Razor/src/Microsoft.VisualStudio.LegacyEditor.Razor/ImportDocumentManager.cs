// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Editor.Razor.Documents;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

[Export(typeof(IImportDocumentManager))]
[method: ImportingConstructor]
internal sealed class ImportDocumentManager(
    ProjectSnapshotManagerDispatcher dispatcher,
    IFileChangeTrackerFactory fileChangeTrackerFactory) : IImportDocumentManager
{
    private readonly IFileChangeTrackerFactory _fileChangeTrackerFactory = fileChangeTrackerFactory;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;
    private readonly Dictionary<string, ImportTracker> _importTrackerCache = new(StringComparer.OrdinalIgnoreCase);

    public event EventHandler<ImportChangedEventArgs>? Changed;

    public void OnSubscribed(IVisualStudioDocumentTracker documentTracker)
    {
        if (documentTracker is null)
        {
            throw new ArgumentNullException(nameof(documentTracker));
        }

        _dispatcher.AssertDispatcherThread();

        var filePath = documentTracker.FilePath;
        var projectSnapshot = documentTracker.ProjectSnapshot.AssumeNotNull();

        foreach (var import in GetPhysicalImportItems(filePath, projectSnapshot))
        {
            var importFilePath = import.PhysicalPath;

            Debug.Assert(importFilePath is not null);

            if (importFilePath is null)
            {
                continue;
            }

            if (!_importTrackerCache.TryGetValue(importFilePath, out var importTracker))
            {
                // First time seeing this import. Start tracking it.
                var fileChangeTracker = _fileChangeTrackerFactory.Create(importFilePath);
                importTracker = new ImportTracker(fileChangeTracker);
                _importTrackerCache[importFilePath] = importTracker;

                fileChangeTracker.Changed += FileChangeTracker_Changed;
                fileChangeTracker.StartListening();
            }

            importTracker.AssociatedDocuments.Add(documentTracker.FilePath);
        }
    }

    public void OnUnsubscribed(IVisualStudioDocumentTracker documentTracker)
    {
        if (documentTracker is null)
        {
            throw new ArgumentNullException(nameof(documentTracker));
        }

        _dispatcher.AssertDispatcherThread();

        var filePath = documentTracker.FilePath;
        var projectSnapshot = documentTracker.ProjectSnapshot.AssumeNotNull();

        foreach (var import in GetPhysicalImportItems(filePath, projectSnapshot))
        {
            var importPhysicalPath = import.PhysicalPath.AssumeNotNull();

            if (_importTrackerCache.TryGetValue(importPhysicalPath, out var importTracker))
            {
                importTracker.AssociatedDocuments.Remove(documentTracker.FilePath);

                if (importTracker.AssociatedDocuments.Count == 0)
                {
                    // There are no open documents that care about this import. We no longer need to track it.
                    importTracker.FileChangeTracker.StopListening();
                    _importTrackerCache.Remove(importPhysicalPath);
                }
            }
        }
    }

    private static IEnumerable<RazorProjectItem> GetPhysicalImportItems(string filePath, IProjectSnapshot projectSnapshot)
    {
        var projectEngine = projectSnapshot.GetProjectEngine();
        var documentSnapshot = projectSnapshot.GetDocument(filePath);
        var projectItem = projectEngine.FileSystem.GetItem(filePath, documentSnapshot?.FileKind);

        foreach (var projectFeature in projectEngine.ProjectFeatures)
        {
            if (projectFeature is not IImportProjectFeature importFeature)
            {
                continue;
            }

            foreach (var importItem in importFeature.GetImports(projectItem))
            {
                if (importItem.PhysicalPath is null)
                {
                    continue;
                }

                yield return importItem;
            }
        }
    }

    private void FileChangeTracker_Changed(object sender, FileChangeEventArgs args)
    {
        _dispatcher.AssertDispatcherThread();

        if (_importTrackerCache.TryGetValue(args.FilePath, out var importTracker))
        {
            Changed?.Invoke(this, new ImportChangedEventArgs(importTracker.FilePath, args.Kind, importTracker.AssociatedDocuments));
        }
    }

    private sealed class ImportTracker(IFileChangeTracker fileChangeTracker)
    {
        public IFileChangeTracker FileChangeTracker => fileChangeTracker;
        public string FilePath => fileChangeTracker.FilePath;
        public HashSet<string> AssociatedDocuments { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}

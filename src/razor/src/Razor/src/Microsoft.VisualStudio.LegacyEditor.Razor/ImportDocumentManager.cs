// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem.Legacy;
using Microsoft.VisualStudio.Razor.Documents;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

[Export(typeof(IImportDocumentManager))]
[method: ImportingConstructor]
internal sealed class ImportDocumentManager(IFileChangeTrackerFactory fileChangeTrackerFactory) : IImportDocumentManager
{
    private readonly IFileChangeTrackerFactory _fileChangeTrackerFactory = fileChangeTrackerFactory;

    private readonly object _gate = new();
    private readonly Dictionary<string, ImportTracker> _importTrackerCache = new(StringComparer.OrdinalIgnoreCase);

    public event EventHandler<ImportChangedEventArgs>? Changed;

    public void OnSubscribed(IVisualStudioDocumentTracker documentTracker)
    {
        if (documentTracker is null)
        {
            throw new ArgumentNullException(nameof(documentTracker));
        }

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

            lock (_gate)
            {
                if (!_importTrackerCache.TryGetValue(importFilePath, out var importTracker))
                {
                    // First time seeing this import. Start tracking it.
                    var fileChangeTracker = _fileChangeTrackerFactory.Create(importFilePath);
                    importTracker = new ImportTracker(fileChangeTracker);
                    _importTrackerCache[importFilePath] = importTracker;

                    fileChangeTracker.Changed += FileChangeTracker_Changed;
                    fileChangeTracker.StartListening();
                }

                importTracker.AddAssociatedDocument(documentTracker.FilePath);
            }
        }
    }

    public void OnUnsubscribed(IVisualStudioDocumentTracker documentTracker)
    {
        if (documentTracker is null)
        {
            throw new ArgumentNullException(nameof(documentTracker));
        }

        var filePath = documentTracker.FilePath;
        var projectSnapshot = documentTracker.ProjectSnapshot.AssumeNotNull();

        foreach (var import in GetPhysicalImportItems(filePath, projectSnapshot))
        {
            var importPhysicalPath = import.PhysicalPath.AssumeNotNull();

            lock (_gate)
            {
                if (_importTrackerCache.TryGetValue(importPhysicalPath, out var importTracker))
                {
                    importTracker.RemoveAssociatedDocument(documentTracker.FilePath);

                    if (importTracker.AssociatedDocumentCount == 0)
                    {
                        // There are no open documents that care about this import. We no longer need to track it.
                        importTracker.FileChangeTracker.StopListening();
                        _importTrackerCache.Remove(importPhysicalPath);
                    }
                }
            }
        }
    }

    private static ImmutableArray<RazorProjectItem> GetPhysicalImportItems(string filePath, ILegacyProjectSnapshot projectSnapshot)
    {
        var projectEngine = projectSnapshot.GetProjectEngine();

        // If we can get the document, use it's target path to find the project item
        // to avoid GetItem(...) throwing an exception if the file path is rooted outside
        // of the project. If we can't get the document, we'll just go ahead and use
        // the file path, since it's probably OK.
        var projectItem = projectSnapshot.GetDocument(filePath) is { } document
            ? projectEngine.FileSystem.GetItem(document.TargetPath, document.FileKind)
            : projectEngine.FileSystem.GetItem(filePath, fileKind: null);

        return projectEngine.GetImports(projectItem, static i => i is not DefaultImportProjectItem);
    }

    private void FileChangeTracker_Changed(object sender, FileChangeEventArgs args)
    {
        lock (_gate)
        {
            if (_importTrackerCache.TryGetValue(args.FilePath, out var importTracker))
            {
                Changed?.Invoke(this, new ImportChangedEventArgs(importTracker.FilePath, args.Kind, importTracker.GetAssociatedDocuments()));
            }
        }
    }

    private sealed class ImportTracker(IFileChangeTracker fileChangeTracker)
    {
        private readonly HashSet<string> _associatedDocuments = new(StringComparer.OrdinalIgnoreCase);

        public IFileChangeTracker FileChangeTracker => fileChangeTracker;
        public string FilePath => fileChangeTracker.FilePath;

        public int AssociatedDocumentCount => _associatedDocuments.Count;

        public void AddAssociatedDocument(string filePath)
            => _associatedDocuments.Add(filePath);

        public void RemoveAssociatedDocument(string filePath)
            => _associatedDocuments.Remove(filePath);

        public ImmutableArray<string> GetAssociatedDocuments()
            => _associatedDocuments.ToImmutableArray();
    }
}

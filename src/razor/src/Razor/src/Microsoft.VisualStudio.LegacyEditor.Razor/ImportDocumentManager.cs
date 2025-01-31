// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
        var documentSnapshot = projectSnapshot.GetDocument(filePath);
        var projectItem = projectEngine.FileSystem.GetItem(filePath, documentSnapshot?.FileKind);

        return projectEngine.GetImports(projectItem, static i => i.PhysicalPath is not null);
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

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

// Similar to the DocumentProvider in dotnet/Roslyn - but simplified quite a bit to remove
// concepts that we don't need. Responsible for providing data about text changes for documents
// and editor open/closed state.
internal abstract class EditorDocumentManagerBase : EditorDocumentManager
{
    private readonly FileChangeTrackerFactory _fileChangeTrackerFactory;
    private readonly Dictionary<DocumentKey, EditorDocument> _documents;
    private readonly Dictionary<string, List<DocumentKey>> _documentsByFilePath;

    protected readonly object Lock;

    public EditorDocumentManagerBase(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext,
        FileChangeTrackerFactory fileChangeTrackerFactory)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (fileChangeTrackerFactory is null)
        {
            throw new ArgumentNullException(nameof(fileChangeTrackerFactory));
        }

        ProjectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        JoinableTaskContext = joinableTaskContext;
        _fileChangeTrackerFactory = fileChangeTrackerFactory;

        _documents = new Dictionary<DocumentKey, EditorDocument>();
        _documentsByFilePath = new Dictionary<string, List<DocumentKey>>(FilePathComparer.Instance);

        Lock = new object();
    }

    protected ProjectSnapshotManagerDispatcher ProjectSnapshotManagerDispatcher { get; }

    protected JoinableTaskContext JoinableTaskContext { get; }

    protected abstract ITextBuffer? GetTextBufferForOpenDocument(string filePath);

    protected abstract void OnDocumentOpened(EditorDocument document);

    protected abstract void OnDocumentClosed(EditorDocument document);

    public sealed override bool TryGetDocument(DocumentKey key, [NotNullWhen(returnValue: true)] out EditorDocument? document)
    {
        JoinableTaskContext.AssertUIThread();

        lock (Lock)
        {
            return _documents.TryGetValue(key, out document);
        }
    }

    public sealed override bool TryGetMatchingDocuments(string filePath, [NotNullWhen(returnValue: true)] out EditorDocument[]? documents)
    {
        JoinableTaskContext.AssertUIThread();

        lock (Lock)
        {
            if (!_documentsByFilePath.TryGetValue(filePath, out var keys))
            {
                documents = null;
                return false;
            }

            documents = new EditorDocument[keys.Count];
            for (var i = 0; i < keys.Count; i++)
            {
                documents[i] = _documents[keys[i]];
            }

            return true;
        }
    }

    public sealed override EditorDocument GetOrCreateDocument(
        DocumentKey key,
        EventHandler? changedOnDisk,
        EventHandler? changedInEditor,
        EventHandler? opened,
        EventHandler? closed)
    {
        JoinableTaskContext.AssertUIThread();

        lock (Lock)
        {
            if (TryGetDocument(key, out var document))
            {
                return document;
            }

            // Check if the document is already open and initialized, and associate a buffer if possible.
            var textBuffer = GetTextBufferForOpenDocument(key.DocumentFilePath);
            document = new EditorDocument(
                this,
                ProjectSnapshotManagerDispatcher,
                JoinableTaskContext,
                key.ProjectFilePath,
                key.DocumentFilePath,
                new FileTextLoader(key.DocumentFilePath, defaultEncoding: null),
                _fileChangeTrackerFactory.Create(key.DocumentFilePath),
                textBuffer,
                changedOnDisk,
                changedInEditor,
                opened,
                closed);

            _documents.Add(key, document);

            if (!_documentsByFilePath.TryGetValue(key.DocumentFilePath, out var documents))
            {
                documents = new List<DocumentKey>();
                _documentsByFilePath.Add(key.DocumentFilePath, documents);
            }

            if (!documents.Contains(key))
            {
                documents.Add(key);
            }

            if (document.IsOpenInEditor)
            {
                OnDocumentOpened(document);
            }

            return document;
        }
    }

    protected void DocumentOpened(string filePath, ITextBuffer textBuffer)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        JoinableTaskContext.AssertUIThread();

        lock (Lock)
        {
            if (TryGetMatchingDocuments(filePath, out var documents))
            {
                for (var i = 0; i < documents.Length; i++)
                {
                    var document = documents[i];

                    document.ProcessOpen(textBuffer);
                    OnDocumentOpened(document);
                }
            }
        }
    }

    protected void DocumentClosed(string filePath)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        JoinableTaskContext.AssertUIThread();

        lock (Lock)
        {
            if (TryGetMatchingDocuments(filePath, out var documents))
            {
                for (var i = 0; i < documents.Length; i++)
                {
                    var document = documents[i];

                    document.ProcessClose();
                    OnDocumentClosed(document);
                }
            }
        }
    }

    public sealed override void RemoveDocument(EditorDocument document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        JoinableTaskContext.AssertUIThread();

        lock (Lock)
        {
            var key = new DocumentKey(document.ProjectFilePath, document.DocumentFilePath);
            if (_documentsByFilePath.TryGetValue(document.DocumentFilePath, out var documents))
            {
                documents.Remove(key);

                if (documents.Count == 0)
                {
                    _documentsByFilePath.Remove(document.DocumentFilePath);
                }
            }

            _documents.Remove(key);

            if (document.IsOpenInEditor)
            {
                OnDocumentClosed(document);
            }
        }
    }
}

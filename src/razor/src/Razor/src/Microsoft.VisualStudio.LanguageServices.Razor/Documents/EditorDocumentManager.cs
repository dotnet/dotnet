// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor.Documents;

// Similar to the DocumentProvider in dotnet/Roslyn - but simplified quite a bit to remove
// concepts that we don't need. Responsible for providing data about text changes for documents
// and editor open/closed state.
internal abstract class EditorDocumentManager : IEditorDocumentManager
{
    private readonly IFileChangeTrackerFactory _fileChangeTrackerFactory;
    private readonly Dictionary<DocumentKey, EditorDocument> _documents;
    private readonly Dictionary<string, List<DocumentKey>> _documentsByFilePath;

    protected readonly object Lock;

    protected JoinableTaskContext JoinableTaskContext { get; }

    protected EditorDocumentManager(
        IFileChangeTrackerFactory fileChangeTrackerFactory,
        JoinableTaskContext joinableTaskContext)
    {
        JoinableTaskContext = joinableTaskContext;
        _fileChangeTrackerFactory = fileChangeTrackerFactory;

        _documents = new Dictionary<DocumentKey, EditorDocument>();
        _documentsByFilePath = new Dictionary<string, List<DocumentKey>>(FilePathComparer.Instance);

        Lock = new object();
    }

    protected abstract ITextBuffer? GetTextBufferForOpenDocument(string filePath);

    protected abstract void OnDocumentOpened(EditorDocument document);

    protected abstract void OnDocumentClosed(EditorDocument document);

    public bool TryGetDocument(DocumentKey key, [NotNullWhen(returnValue: true)] out EditorDocument? document)
    {
        JoinableTaskContext.AssertUIThread();

        lock (Lock)
        {
            return _documents.TryGetValue(key, out document);
        }
    }

    public bool TryGetMatchingDocuments(string filePath, [NotNullWhen(returnValue: true)] out EditorDocument[]? documents)
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

    public EditorDocument GetOrCreateDocument(
        DocumentKey key,
        string projectFilePath,
        ProjectKey projectKey,
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
            var textBuffer = GetTextBufferForOpenDocument(key.FilePath);
            document = new EditorDocument(
                this,
                JoinableTaskContext,
                projectFilePath,
                key.FilePath,
                projectKey,
                new FileTextLoader(key.FilePath, defaultEncoding: null),
                _fileChangeTrackerFactory.Create(key.FilePath),
                textBuffer,
                changedOnDisk,
                changedInEditor,
                opened,
                closed);

            _documents.Add(key, document);

            if (!_documentsByFilePath.TryGetValue(key.FilePath, out var documents))
            {
                documents = new List<DocumentKey>();
                _documentsByFilePath.Add(key.FilePath, documents);
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

    public void RemoveDocument(EditorDocument document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        JoinableTaskContext.AssertUIThread();

        lock (Lock)
        {
            var key = new DocumentKey(document.ProjectKey, document.DocumentFilePath);
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

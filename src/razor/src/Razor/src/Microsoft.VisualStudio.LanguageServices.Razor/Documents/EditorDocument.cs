// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor.Documents;

// Tracks the mutable state associated with a document - in contrast to DocumentSnapshot
// which tracks the state at a point in time.
internal sealed class EditorDocument : IDisposable
{
    private readonly IEditorDocumentManager _documentManager;
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly IFileChangeTracker _fileTracker;
    private readonly SnapshotChangeTracker _snapshotTracker;
    private readonly EventHandler? _changedOnDisk;
    private readonly EventHandler? _changedInEditor;
    private readonly EventHandler? _opened;
    private readonly EventHandler? _closed;

    private bool _disposed;

    public EditorDocument(
        IEditorDocumentManager documentManager,
        JoinableTaskContext joinableTaskContext,
        string projectFilePath,
        string documentFilePath,
        ProjectKey projectKey,
        TextLoader textLoader,
        IFileChangeTracker fileTracker,
        ITextBuffer? textBuffer,
        EventHandler? changedOnDisk,
        EventHandler? changedInEditor,
        EventHandler? opened,
        EventHandler? closed)
    {
        _documentManager = documentManager;
        _joinableTaskContext = joinableTaskContext;
        ProjectFilePath = projectFilePath;
        DocumentFilePath = documentFilePath;
        ProjectKey = projectKey;
        TextLoader = textLoader;
        _fileTracker = fileTracker;
        _changedOnDisk = changedOnDisk;
        _changedInEditor = changedInEditor;
        _opened = opened;
        _closed = closed;

        _snapshotTracker = new SnapshotChangeTracker();
        _fileTracker.Changed += ChangeTracker_Changed;

        // Only one of these should be active at a time.
        if (textBuffer is null)
        {
            _fileTracker.StartListening();
        }
        else
        {
            _snapshotTracker.StartTracking(textBuffer);

            EditorTextBuffer = textBuffer;
            EditorTextContainer = textBuffer.AsTextContainer();
            EditorTextContainer.TextChanged += TextContainer_Changed;
        }
    }

    public ProjectKey ProjectKey { get; }

    public string ProjectFilePath { get; }

    public string DocumentFilePath { get; }

    public bool IsOpenInEditor => EditorTextBuffer != null;

    public SourceTextContainer? EditorTextContainer { get; private set; }

    public ITextBuffer? EditorTextBuffer { get; private set; }

    public TextLoader TextLoader { get; }

    public void ProcessOpen(ITextBuffer textBuffer)
    {
        if (textBuffer is null)
        {
            throw new ArgumentNullException(nameof(textBuffer));
        }

        _fileTracker.StopListening();

        _snapshotTracker.StartTracking(textBuffer);
        EditorTextBuffer = textBuffer;
        EditorTextContainer = textBuffer.AsTextContainer();
        EditorTextContainer.TextChanged += TextContainer_Changed;

        _opened?.Invoke(this, EventArgs.Empty);
    }

    public void ProcessClose()
    {
        _closed?.Invoke(this, EventArgs.Empty);

        _snapshotTracker.StopTracking(EditorTextBuffer);

        EditorTextContainer!.TextChanged -= TextContainer_Changed;
        EditorTextContainer = null;
        EditorTextBuffer = null;

        _fileTracker.StartListening();
    }

    private void ChangeTracker_Changed(object sender, FileChangeEventArgs e)
    {
        if (e.Kind == FileChangeKind.Changed)
        {
            _changedOnDisk?.Invoke(this, EventArgs.Empty);
        }
    }

    private void TextContainer_Changed(object sender, TextChangeEventArgs e)
    {
        _changedInEditor?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        _joinableTaskContext.AssertUIThread();

        if (!_disposed)
        {
            _fileTracker.Changed -= ChangeTracker_Changed;

            _fileTracker.StopListening();

            if (EditorTextBuffer is not null)
            {
                _snapshotTracker.StopTracking(EditorTextBuffer);
                EditorTextBuffer = null;
            }

            if (EditorTextContainer is not null)
            {
                EditorTextContainer.TextChanged -= TextContainer_Changed;
                EditorTextContainer = null;
            }

            _documentManager.RemoveDocument(this);

            _disposed = true;
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

// Tracks the mutable state associated with a document - in contrast to DocumentSnapshot
// which tracks the state at a point in time.
internal sealed class EditorDocument : IDisposable
{
    private readonly EditorDocumentManager _documentManager;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly FileChangeTracker _fileTracker;
    private readonly SnapshotChangeTracker _snapshotTracker;
    private readonly EventHandler? _changedOnDisk;
    private readonly EventHandler? _changedInEditor;
    private readonly EventHandler? _opened;
    private readonly EventHandler? _closed;

    private bool _disposed;

    public EditorDocument(
        EditorDocumentManager documentManager,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext,
        string projectFilePath,
        string documentFilePath,
        TextLoader textLoader,
        FileChangeTracker fileTracker,
        ITextBuffer? textBuffer,
        EventHandler? changedOnDisk,
        EventHandler? changedInEditor,
        EventHandler? opened,
        EventHandler? closed)
    {
        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (projectFilePath is null)
        {
            throw new ArgumentNullException(nameof(projectFilePath));
        }

        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        if (textLoader is null)
        {
            throw new ArgumentNullException(nameof(textLoader));
        }

        if (fileTracker is null)
        {
            throw new ArgumentNullException(nameof(fileTracker));
        }

        _documentManager = documentManager;
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _joinableTaskContext = joinableTaskContext;
        ProjectFilePath = projectFilePath;
        DocumentFilePath = documentFilePath;
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
            _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                _fileTracker.StartListening, CancellationToken.None).ConfigureAwait(false);
        }
        else
        {
            _snapshotTracker.StartTracking(textBuffer);

            EditorTextBuffer = textBuffer;
            EditorTextContainer = textBuffer.AsTextContainer();
            EditorTextContainer.TextChanged += TextContainer_Changed;
        }
    }

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

        _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
            _fileTracker.StopListening, CancellationToken.None).ConfigureAwait(false);

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

        _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
            _fileTracker.StartListening, CancellationToken.None);
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

            _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                _fileTracker.StopListening, CancellationToken.None);

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

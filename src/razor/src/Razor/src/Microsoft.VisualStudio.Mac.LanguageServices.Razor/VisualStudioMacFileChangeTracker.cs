// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Editor.Razor.Documents;
using MonoDevelop.Core;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor;

internal class VisualStudioMacFileChangeTracker : FileChangeTracker
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly string _normalizedFilePath;
    private bool _listening;

    public override event EventHandler<FileChangeEventArgs>? Changed;

    public VisualStudioMacFileChangeTracker(
        string filePath,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException(SR.ArgumentCannotBeNullOrEmpty, nameof(filePath));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        FilePath = filePath;
        _normalizedFilePath = NormalizePath(FilePath);
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
    }

    public override string FilePath { get; }

    public override void StartListening()
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (_listening)
        {
            return;
        }

        AttachToFileServiceEvents();

        _listening = true;
    }

    public override void StopListening()
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        if (!_listening)
        {
            return;
        }

        DetachFromFileServiceEvents();

        _listening = false;
    }

    // Virtual for testing
    protected virtual void AttachToFileServiceEvents()
    {
        FileService.FileChanged += FileService_FileChanged;
        FileService.FileCreated += FileService_FileCreated;
        FileService.FileRemoved += FileService_FileRemoved;
    }

    // Virtual for testing
    protected virtual void DetachFromFileServiceEvents()
    {
        FileService.FileChanged -= FileService_FileChanged;
        FileService.FileCreated -= FileService_FileCreated;
        FileService.FileRemoved -= FileService_FileRemoved;
    }

    private void FileService_FileChanged(object sender, FileEventArgs args) => HandleFileChangeEvent(FileChangeKind.Changed, args);

    private void FileService_FileCreated(object sender, FileEventArgs args) => HandleFileChangeEvent(FileChangeKind.Added, args);

    private void FileService_FileRemoved(object sender, FileEventArgs args) => HandleFileChangeEvent(FileChangeKind.Removed, args);

    private void HandleFileChangeEvent(FileChangeKind changeKind, FileEventArgs args)
    {
        if (Changed is null)
        {
            return;
        }

        foreach (var fileEvent in args)
        {
            if (fileEvent.IsDirectory)
            {
                continue;
            }

            var normalizedEventPath = NormalizePath(fileEvent.FileName.FullPath);
            if (string.Equals(_normalizedFilePath, normalizedEventPath, StringComparison.OrdinalIgnoreCase))
            {
                _ = _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync((changeKind, ct) => OnChanged(changeKind), changeKind, CancellationToken.None);
                return;
            }
        }
    }

    private void OnChanged(FileChangeKind changeKind)
    {
        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        var args = new FileChangeEventArgs(FilePath, changeKind);
        Changed?.Invoke(this, args);
    }

    private static string NormalizePath(string path)
    {
        path = path.Replace('\\', '/');

        return path;
    }
}

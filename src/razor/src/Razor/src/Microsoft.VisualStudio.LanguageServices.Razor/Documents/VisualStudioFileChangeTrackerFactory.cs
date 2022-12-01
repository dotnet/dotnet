// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

internal class VisualStudioFileChangeTrackerFactory : FileChangeTrackerFactory
{
    private readonly ErrorReporter _errorReporter;
    private readonly IVsAsyncFileChangeEx _fileChangeService;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;

    public VisualStudioFileChangeTrackerFactory(
        ErrorReporter errorReporter,
        IVsAsyncFileChangeEx fileChangeService,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext)
    {
        if (errorReporter is null)
        {
            throw new ArgumentNullException(nameof(errorReporter));
        }

        if (fileChangeService is null)
        {
            throw new ArgumentNullException(nameof(fileChangeService));
        }

        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        _errorReporter = errorReporter;
        _fileChangeService = fileChangeService;
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _joinableTaskContext = joinableTaskContext;
    }

    public override FileChangeTracker Create(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException(SR.ArgumentCannotBeNullOrEmpty, nameof(filePath));
        }

        var fileChangeTracker = new VisualStudioFileChangeTracker(filePath, _errorReporter, _fileChangeService, _projectSnapshotManagerDispatcher, _joinableTaskContext);
        return fileChangeTracker;
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

[Export(typeof(IFileChangeTrackerFactory))]
internal class VisualStudioFileChangeTrackerFactory : IFileChangeTrackerFactory
{
    private readonly IErrorReporter _errorReporter;
    private readonly IVsAsyncFileChangeEx _fileChangeService;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;

    [ImportingConstructor]
    public VisualStudioFileChangeTrackerFactory(
        SVsServiceProvider serviceProvider,
        JoinableTaskContext joinableTaskContext,
        ProjectSnapshotManagerDispatcher dispatcher,
        IErrorReporter errorReporter)
    {
        _errorReporter = errorReporter;
        _fileChangeService = (IVsAsyncFileChangeEx)serviceProvider.GetService(typeof(SVsFileChangeEx));
        _projectSnapshotManagerDispatcher = dispatcher;
        _joinableTaskContext = joinableTaskContext;
    }

    public IFileChangeTracker Create(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException(SR.ArgumentCannotBeNullOrEmpty, nameof(filePath));
        }

        return new VisualStudioFileChangeTracker(filePath, _errorReporter, _fileChangeService, _projectSnapshotManagerDispatcher, _joinableTaskContext);
    }
}

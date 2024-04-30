// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor.Documents;

[Export(typeof(IFileChangeTrackerFactory))]
internal class VisualStudioFileChangeTrackerFactory : IFileChangeTrackerFactory
{
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly ILoggerFactory _loggerFactory;
    private readonly JoinableTask<IVsAsyncFileChangeEx> _getFileChangeServiceTask;

    [ImportingConstructor]
    public VisualStudioFileChangeTrackerFactory(
        [Import(typeof(SAsyncServiceProvider))] IAsyncServiceProvider serviceProvider,
        JoinableTaskContext joinableTaskContext,
        ILoggerFactory loggerFactory)
    {
        _joinableTaskContext = joinableTaskContext;
        _loggerFactory = loggerFactory;

        var jtf = _joinableTaskContext.Factory;
        _getFileChangeServiceTask = jtf.RunAsync(serviceProvider.GetServiceAsync<SVsFileChangeEx, IVsAsyncFileChangeEx>);
    }

    public IFileChangeTracker Create(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException(SR.ArgumentCannotBeNullOrEmpty, nameof(filePath));
        }

        // TODO: Make IFileChangeTrackerFactory.Create(...) asynchronous to avoid blocking here.
        var fileChangeService = _getFileChangeServiceTask.Join();

        return new VisualStudioFileChangeTracker(filePath, _loggerFactory, fileChangeService, _joinableTaskContext);
    }
}

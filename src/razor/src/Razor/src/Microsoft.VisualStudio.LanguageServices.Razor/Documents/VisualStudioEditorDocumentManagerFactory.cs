// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor.Documents;

[Shared]
[ExportWorkspaceServiceFactory(typeof(EditorDocumentManager), ServiceLayer.Host)]
internal class VisualStudioEditorDocumentManagerFactory : IWorkspaceServiceFactory
{
    private readonly SVsServiceProvider _serviceProvider;
    private readonly IVsEditorAdaptersFactoryService _editorAdaptersFactory;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;

    [ImportingConstructor]
    public VisualStudioEditorDocumentManagerFactory(
        SVsServiceProvider serviceProvider,
        IVsEditorAdaptersFactoryService editorAdaptersFactory,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext)
    {
        if (serviceProvider is null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        if (editorAdaptersFactory is null)
        {
            throw new ArgumentNullException(nameof(editorAdaptersFactory));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        _serviceProvider = serviceProvider;
        _editorAdaptersFactory = editorAdaptersFactory;
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _joinableTaskContext = joinableTaskContext;
    }

    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
    {
        if (workspaceServices is null)
        {
            throw new ArgumentNullException(nameof(workspaceServices));
        }

        var runningDocumentTable = (IVsRunningDocumentTable)_serviceProvider.GetService(typeof(SVsRunningDocumentTable));
        var fileChangeTrackerFactory = workspaceServices.GetRequiredService<FileChangeTrackerFactory>();
        return new VisualStudioEditorDocumentManager(
            _projectSnapshotManagerDispatcher, _joinableTaskContext, fileChangeTrackerFactory, runningDocumentTable, _editorAdaptersFactory);
    }
}

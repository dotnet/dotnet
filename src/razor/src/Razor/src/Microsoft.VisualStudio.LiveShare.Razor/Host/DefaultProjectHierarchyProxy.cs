// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LiveShare.Razor.Host;

internal class DefaultProjectHierarchyProxy : IProjectHierarchyProxy, ICollaborationService
{
    private readonly CollaborationSession _session;

    private readonly JoinableTaskFactory _joinableTaskFactory;
    private IVsUIShellOpenDocument? _openDocumentShell;

    public DefaultProjectHierarchyProxy(
        CollaborationSession session,
        JoinableTaskFactory joinableTaskFactory)
    {
        if (session is null)
        {
            throw new ArgumentNullException(nameof(session));
        }

        if (joinableTaskFactory is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskFactory));
        }

        _session = session;
        _joinableTaskFactory = joinableTaskFactory;
    }

    public async Task<Uri?> GetProjectPathAsync(Uri documentFilePath, CancellationToken cancellationToken)
    {
        if (documentFilePath is null)
        {
            throw new ArgumentNullException(nameof(documentFilePath));
        }

        await _joinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

#pragma warning disable VSSDK006 // Check services exist
        _openDocumentShell ??= ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShellOpenDocument)) as IVsUIShellOpenDocument;
#pragma warning restore VSSDK006 // Check services exist
        var hostDocumentFilePath = _session.ConvertSharedUriToLocalPath(documentFilePath);
        var hr = _openDocumentShell!.IsDocumentInAProject(hostDocumentFilePath, out var hierarchy, out _, out _, out _);
        if (ErrorHandler.Succeeded(hr) && hierarchy != null)
        {
            ErrorHandler.ThrowOnFailure(((IVsProject)hierarchy).GetMkDocument((uint)VSConstants.VSITEMID.Root, out var path), VSConstants.E_NOTIMPL);

            return _session.ConvertLocalPathToSharedUri(path);
        }

        return null;
    }
}

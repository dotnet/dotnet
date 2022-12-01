// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LiveShare.Razor.Guest;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LiveShare.Razor;

[Export(typeof(ProjectCapabilityResolver))]
internal class LiveShareProjectCapabilityResolver : ProjectCapabilityResolver
{
    private readonly LiveShareSessionAccessor _sessionAccessor;
    private readonly JoinableTaskFactory _joinableTaskFactory;

    [ImportingConstructor]
    public LiveShareProjectCapabilityResolver(
        LiveShareSessionAccessor sessionAccessor,
        JoinableTaskContext joinableTaskContext)
    {
        if (sessionAccessor is null)
        {
            throw new ArgumentNullException(nameof(sessionAccessor));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        _sessionAccessor = sessionAccessor;
        _joinableTaskFactory = joinableTaskContext.Factory;
    }

    public override bool HasCapability(object project, string capability)
    {
        // In LiveShare scenarios we need a document file path to be able to make reasonable assumptions on if a project has a capability
        return false;
    }

    public override bool HasCapability(string documentFilePath, object project, string capability)
    {
        if (!_sessionAccessor.IsGuestSessionActive)
        {
            // We don't provide capabilities for non-guest sessions.
            return false;
        }

        var remoteHasCapability = RemoteHasCapability(documentFilePath, capability);
        return remoteHasCapability;
    }

    private bool RemoteHasCapability(string documentMoniker, string capability)
    {
        // On a guest box. The project hierarchy is not fully populated. We need to ask the host machine
        // questions on hierarchy capabilities.
        return _joinableTaskFactory.Run(async () =>
        {
            var remoteHierarchyService = await _sessionAccessor.Session!.GetRemoteServiceAsync<IRemoteHierarchyService>(nameof(IRemoteHierarchyService), CancellationToken.None).ConfigureAwait(false);
            var documentMonikerUri = _sessionAccessor.Session.ConvertLocalPathToSharedUri(documentMoniker);
            var hasCapability = await remoteHierarchyService.HasCapabilityAsync(documentMonikerUri, capability, CancellationToken.None).ConfigureAwait(false);
            return hasCapability;
        });
    }
}

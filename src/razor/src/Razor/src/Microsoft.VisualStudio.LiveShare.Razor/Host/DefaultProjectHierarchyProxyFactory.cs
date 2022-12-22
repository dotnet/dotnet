// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LiveShare.Razor.Host;

[ExportCollaborationService(
    typeof(IProjectHierarchyProxy),
    Name = nameof(IProjectHierarchyProxy),
    Scope = SessionScope.Host,
    Role = ServiceRole.RemoteService)]
internal class DefaultProjectHierarchyProxyFactory : ICollaborationServiceFactory
{
    private readonly JoinableTaskContext _joinableTaskContext;

    [ImportingConstructor]
    public DefaultProjectHierarchyProxyFactory(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext)
    {
        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        _joinableTaskContext = joinableTaskContext;
    }

    public Task<ICollaborationService> CreateServiceAsync(CollaborationSession session, CancellationToken cancellationToken)
    {
        if (session is null)
        {
            throw new ArgumentNullException(nameof(session));
        }

        var service = new DefaultProjectHierarchyProxy(session, _joinableTaskContext.Factory);
        return Task.FromResult<ICollaborationService>(service);
    }
}

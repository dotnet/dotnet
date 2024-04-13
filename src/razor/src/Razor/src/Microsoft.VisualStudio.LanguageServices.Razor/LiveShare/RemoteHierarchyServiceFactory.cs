// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LiveShare;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.Razor.LiveShare;

/// <summary>
/// In cloud scenarios a client will not have a project system which means any code running on the client needs to have the ability to
/// query the remote project system. That is what this class is responsible for.
/// </summary>
[ExportCollaborationService(
    typeof(IRemoteHierarchyService),
    Name = nameof(IRemoteHierarchyService),
    Scope = SessionScope.Host,
    Role = ServiceRole.RemoteService)]
[method: ImportingConstructor]
internal sealed class RemoteHierarchyServiceFactory(JoinableTaskContext joinableTaskContext) : ICollaborationServiceFactory
{
    public Task<ICollaborationService> CreateServiceAsync(CollaborationSession session, CancellationToken cancellationToken)
    {
        return Task.FromResult<ICollaborationService>(new RemoteHierarchyService(session, joinableTaskContext.Factory));
    }
}

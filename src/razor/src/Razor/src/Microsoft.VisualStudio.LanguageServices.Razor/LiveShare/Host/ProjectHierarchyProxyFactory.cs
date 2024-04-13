// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LiveShare;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor.LiveShare.Host;

[ExportCollaborationService(
    typeof(IProjectHierarchyProxy),
    Name = nameof(IProjectHierarchyProxy),
    Scope = SessionScope.Host,
    Role = ServiceRole.RemoteService)]
[method: ImportingConstructor]
internal class ProjectHierarchyProxyFactory(
    [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
    JoinableTaskContext joinableTaskContext) : ICollaborationServiceFactory
{
    public Task<ICollaborationService> CreateServiceAsync(CollaborationSession session, CancellationToken cancellationToken)
    {
        var service = new ProjectHierarchyProxy(session, serviceProvider, joinableTaskContext.Factory);
        return Task.FromResult<ICollaborationService>(service);
    }
}

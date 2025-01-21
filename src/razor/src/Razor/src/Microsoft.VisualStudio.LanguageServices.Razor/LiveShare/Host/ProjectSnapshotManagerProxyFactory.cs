// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LiveShare;
using Microsoft.VisualStudio.Razor.LiveShare.Serialization;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.Razor.LiveShare.Host;

[ExportCollaborationService(
    typeof(IProjectSnapshotManagerProxy),
    Name = nameof(IProjectSnapshotManagerProxy),
    Scope = SessionScope.Host,
    Role = ServiceRole.RemoteService)]
[method: ImportingConstructor]
internal class ProjectSnapshotManagerProxyFactory(
    ProjectSnapshotManager projectManager,
    JoinableTaskContext joinableTaskContext) : ICollaborationServiceFactory
{
    public Task<ICollaborationService> CreateServiceAsync(CollaborationSession session, CancellationToken cancellationToken)
    {
        var serializer = (JsonSerializer)session.GetService(typeof(JsonSerializer));
        serializer.Converters.RegisterRazorLiveShareConverters();

        var service = new ProjectSnapshotManagerProxy(
            session, projectManager, joinableTaskContext.Factory);
        return Task.FromResult<ICollaborationService>(service);
    }
}

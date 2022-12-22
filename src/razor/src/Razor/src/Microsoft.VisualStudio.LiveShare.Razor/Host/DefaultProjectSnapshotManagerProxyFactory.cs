// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.LiveShare.Razor.Serialization;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.LiveShare.Razor.Host;

[ExportCollaborationService(
    typeof(IProjectSnapshotManagerProxy),
    Name = nameof(IProjectSnapshotManagerProxy),
    Scope = SessionScope.Host,
    Role = ServiceRole.RemoteService)]
internal class DefaultProjectSnapshotManagerProxyFactory : ICollaborationServiceFactory
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly Workspace _workspace;

    [ImportingConstructor]
    public DefaultProjectSnapshotManagerProxyFactory(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext,
        [Import(typeof(VisualStudioWorkspace))] Workspace workspace)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (workspace is null)
        {
            throw new ArgumentNullException(nameof(workspace));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _joinableTaskContext = joinableTaskContext;
        _workspace = workspace;
    }

    public Task<ICollaborationService> CreateServiceAsync(CollaborationSession session, CancellationToken cancellationToken)
    {
        if (session is null)
        {
            throw new ArgumentNullException(nameof(session));
        }

        var serializer = (JsonSerializer)session.GetService(typeof(JsonSerializer));
        serializer.Converters.RegisterRazorLiveShareConverters();

        var razorLanguageServices = _workspace.Services.GetLanguageServices(RazorLanguage.Name);
        var projectSnapshotManager = razorLanguageServices.GetRequiredService<ProjectSnapshotManager>();

        var service = new DefaultProjectSnapshotManagerProxy(session, _projectSnapshotManagerDispatcher, projectSnapshotManager, _joinableTaskContext.Factory);
        return Task.FromResult<ICollaborationService>(service);
    }
}

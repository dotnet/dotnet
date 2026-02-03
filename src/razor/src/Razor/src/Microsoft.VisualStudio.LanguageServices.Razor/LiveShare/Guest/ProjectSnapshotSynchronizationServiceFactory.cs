// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LiveShare;
using Microsoft.VisualStudio.Razor.LiveShare.Serialization;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.Razor.LiveShare.Guest;

[ExportCollaborationService(typeof(ProjectSnapshotSynchronizationService), Scope = SessionScope.Guest)]
[method: ImportingConstructor]
internal class ProjectSnapshotSynchronizationServiceFactory(
    ProjectSnapshotManager projectManager,
    ILoggerFactory loggerFactory,
    LanguageServerFeatureOptions featureOptions,
    JoinableTaskContext joinableTaskContext) : ICollaborationServiceFactory
{
    public async Task<ICollaborationService> CreateServiceAsync(CollaborationSession sessionContext, CancellationToken cancellationToken)
    {
        if (featureOptions.UseRazorCohostServer)
        {
            // We can't return null, so just return a service that does nothing. Once cohosting is the only option
            // we can remove this class entirely.
            return new NoOpProjectSnapshotSynchronizationService();
        }

        // This collaboration service depends on these serializers being immediately available so we need to register these now so the
        // guest project snapshot manager can retrieve the hosts project state.
        var serializer = (JsonSerializer)sessionContext.GetService(typeof(JsonSerializer));
        serializer.Converters.RegisterRazorLiveShareConverters();

        var projectSnapshotManagerProxy = await sessionContext.GetRemoteServiceAsync<IProjectSnapshotManagerProxy>(typeof(IProjectSnapshotManagerProxy).Name, cancellationToken);
        var synchronizationService = new ProjectSnapshotSynchronizationService(
            sessionContext,
            projectSnapshotManagerProxy,
            projectManager,
            loggerFactory,
            joinableTaskContext.Factory);

        await synchronizationService.InitializeAsync(cancellationToken);

        return synchronizationService;
    }

    private sealed class NoOpProjectSnapshotSynchronizationService : ICollaborationService
    {
    }
}

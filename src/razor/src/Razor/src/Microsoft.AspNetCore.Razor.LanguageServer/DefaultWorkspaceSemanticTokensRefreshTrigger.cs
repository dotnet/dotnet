// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

/// <summary>
/// Sends a 'workspace\semanticTokens\refresh' request each time the project changes.
/// </summary>
internal class DefaultWorkspaceSemanticTokensRefreshTrigger : IProjectSnapshotChangeTrigger
{
    private readonly WorkspaceSemanticTokensRefreshPublisher _publisher;
    private ProjectSnapshotManagerBase? _projectSnapshotManager;

    public DefaultWorkspaceSemanticTokensRefreshTrigger(WorkspaceSemanticTokensRefreshPublisher workspaceSemanticTokensRefreshPublisher)
    {
        _publisher = workspaceSemanticTokensRefreshPublisher;
    }

    public void Initialize(ProjectSnapshotManagerBase projectManager)
    {
        _projectSnapshotManager = projectManager;
        _projectSnapshotManager.Changed += ProjectSnapshotManager_Changed;
    }

    // Does not handle C# files
    private void ProjectSnapshotManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't send for a simple Document edit. The platform should re-request any range that
        // is edited and if a parameter or type change is made it should be reflected as a ProjectChanged.
        if (args.Kind != ProjectChangeKind.DocumentChanged)
        {
            _publisher.EnqueueWorkspaceSemanticTokensRefresh();
        }
    }
}

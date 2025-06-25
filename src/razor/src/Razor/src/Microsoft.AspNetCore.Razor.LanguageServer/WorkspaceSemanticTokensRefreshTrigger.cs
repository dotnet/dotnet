// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

/// <summary>
/// Sends a 'workspace\semanticTokens\refresh' request each time the project changes.
/// </summary>
internal class WorkspaceSemanticTokensRefreshTrigger : IRazorStartupService
{
    private readonly IWorkspaceSemanticTokensRefreshNotifier _publisher;
    private readonly ProjectSnapshotManager _projectManager;

    public WorkspaceSemanticTokensRefreshTrigger(
        IWorkspaceSemanticTokensRefreshNotifier publisher,
        ProjectSnapshotManager projectManager)
    {
        _publisher = publisher;
        _projectManager = projectManager;
        _projectManager.Changed += ProjectManager_Changed;
    }

    // Does not handle C# files
    private void ProjectManager_Changed(object? sender, ProjectChangeEventArgs args)
    {
        // Don't send for a simple Document edit. The platform should re-request any range that
        // is edited and if a parameter or type change is made it should be reflected as a ProjectChanged.
        if (args.Kind != ProjectChangeKind.DocumentChanged)
        {
            _publisher.NotifyWorkspaceSemanticTokensRefresh();
        }
    }
}

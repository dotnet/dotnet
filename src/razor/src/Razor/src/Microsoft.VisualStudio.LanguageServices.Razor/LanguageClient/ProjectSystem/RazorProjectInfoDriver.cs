// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.LanguageClient.ProjectSystem;

internal sealed partial class RazorProjectInfoDriver : AbstractRazorProjectInfoDriver
{
    private readonly IProjectSnapshotManager _projectManager;

    public RazorProjectInfoDriver(
        IProjectSnapshotManager projectManager,
        ILoggerFactory loggerFactory,
        TimeSpan? delay = null) : base(loggerFactory, delay)
    {
        _projectManager = projectManager;

        StartInitialization();
    }

    protected override Task InitializeAsync(CancellationToken cancellationToken)
    {
        // Even though we aren't mutating the project snapshot manager, we call UpdateAsync(...) here to ensure
        // that we run on its dispatcher. That ensures that no changes will code in while we are iterating the
        // current set of projects and connected to the Changed event.
        return _projectManager.UpdateAsync(updater =>
        {
            foreach (var project in updater.GetProjects())
            {
                EnqueueUpdate(project.ToRazorProjectInfo());
            }

            _projectManager.Changed += ProjectManager_Changed;
        },
        cancellationToken);
    }

    private void ProjectManager_Changed(object sender, ProjectChangeEventArgs e)
    {
        // Don't do any work if the solution is closing
        if (e.IsSolutionClosing)
        {
            return;
        }

        switch (e.Kind)
        {
            case ProjectChangeKind.ProjectAdded:
            case ProjectChangeKind.ProjectChanged:
            case ProjectChangeKind.DocumentRemoved:
            case ProjectChangeKind.DocumentAdded:
                var newer = e.Newer.AssumeNotNull();
                EnqueueUpdate(newer.ToRazorProjectInfo());
                break;

            case ProjectChangeKind.ProjectRemoved:
                var older = e.Older.AssumeNotNull();
                EnqueueRemove(older.Key);
                break;

            case ProjectChangeKind.DocumentChanged:
                break;

            default:
                throw new NotSupportedException($"Unsupported {nameof(ProjectChangeKind)}: {e.Kind}");
        }
    }
}

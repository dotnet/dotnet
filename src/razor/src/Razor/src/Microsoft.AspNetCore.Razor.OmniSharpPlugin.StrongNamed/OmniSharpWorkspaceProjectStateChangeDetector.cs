// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin.StrongNamed;

internal class OmniSharpWorkspaceProjectStateChangeDetector : IOmniSharpProjectSnapshotManagerChangeTrigger
{
    public OmniSharpWorkspaceProjectStateChangeDetector(
        OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        OmniSharpProjectWorkspaceStateGenerator workspaceStateGenerator,
        OmniSharpLanguageServerFeatureOptions languageServerFeatureOptions)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (workspaceStateGenerator is null)
        {
            throw new ArgumentNullException(nameof(workspaceStateGenerator));
        }

        InternalWorkspaceProjectStateChangeDetector = new ProjectSnapshotManagerWorkspaceProjectStateChangeDetector(
            projectSnapshotManagerDispatcher.InternalDispatcher,
            workspaceStateGenerator.InternalWorkspaceStateGenerator,
            languageServerFeatureOptions.InternalLanguageServerFeatureOptions);
    }

    internal WorkspaceProjectStateChangeDetector InternalWorkspaceProjectStateChangeDetector { get; }

    public void Initialize(OmniSharpProjectSnapshotManagerBase projectManager)
    {
        InternalWorkspaceProjectStateChangeDetector.Initialize(projectManager.InternalProjectSnapshotManager);
    }

    private class ProjectSnapshotManagerWorkspaceProjectStateChangeDetector : WorkspaceProjectStateChangeDetector
    {
        private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;

        public ProjectSnapshotManagerWorkspaceProjectStateChangeDetector(
            ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
            ProjectWorkspaceStateGenerator workspaceStateGenerator,
            LanguageServerFeatureOptions languageServerFeatureOptions)
            : base(workspaceStateGenerator, projectSnapshotManagerDispatcher, languageServerFeatureOptions)
        {
            if (projectSnapshotManagerDispatcher is null)
            {
                throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
            }

            _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        }

        // We override the InitializeSolution in order to enforce calls to this to be on the project snapshot manager's
        // thread. OmniSharp currently has an issue where they update the Solution on multiple different threads resulting
        // in change events dispatching through the Workspace on multiple different threads. This normalizes
        // that abnormality.
        protected override void InitializeSolution(Solution solution)
        {
            _ = InitializeSolutionAsync(solution, CancellationToken.None);
        }

        private async Task InitializeSolutionAsync(Solution solution, CancellationToken cancellationToken)
        {
            try
            {
                await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                () =>
                {
                    base.InitializeSolution(solution);
                },
                cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.Fail("Unexpected error when initializing solution: " + ex);
            }
        }

        // We override Workspace_WorkspaceChanged in order to enforce calls to this to be on the project snapshot manager's
        // thread. OmniSharp currently has an issue where they update the Solution on multiple different threads resulting
        // in change events dispatching through the Workspace on multiple different threads. This normalizes
        // that abnormality.
        internal override void Workspace_WorkspaceChanged(object? sender, WorkspaceChangeEventArgs args)
        {
            Workspace_WorkspaceChangedAsync(sender, args).Forget();
        }

        private async Task Workspace_WorkspaceChangedAsync(object? sender, WorkspaceChangeEventArgs args)
        {
            try
            {
                await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
                () =>
                {
                    base.Workspace_WorkspaceChanged(sender, args);
                },
                CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.Fail("Unexpected error when handling a workspace changed event: " + ex);
            }
        }
    }
}

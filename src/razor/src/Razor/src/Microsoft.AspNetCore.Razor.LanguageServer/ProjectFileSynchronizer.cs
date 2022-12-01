// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class ProjectFileSynchronizer : IProjectFileChangeListener
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly RazorProjectService _projectService;

    public ProjectFileSynchronizer(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        RazorProjectService projectService)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (projectService is null)
        {
            throw new ArgumentNullException(nameof(projectService));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _projectService = projectService;
    }

    public void ProjectFileChanged(string filePath, RazorFileChangeKind kind)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        _projectSnapshotManagerDispatcher.AssertDispatcherThread();

        switch (kind)
        {
            case RazorFileChangeKind.Added:
                _projectService.AddProject(filePath);
                break;
            case RazorFileChangeKind.Removed:
                _projectService.RemoveProject(filePath);
                break;
        }
    }
}

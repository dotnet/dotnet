// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Editor.Razor;
using MonoDevelop.Projects;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor.ProjectSystem;

[System.Composition.Shared]
[Export(typeof(DotNetProjectHostFactory))]
internal class DotNetProjectHostFactory
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly VisualStudioMacWorkspaceAccessor _workspaceAccessor;
    private readonly TextBufferProjectService _projectService;
    private readonly ProjectConfigurationFilePathStore _projectConfigurationFilePathStore;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    [ImportingConstructor]
    public DotNetProjectHostFactory(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        VisualStudioMacWorkspaceAccessor workspaceAccessor,
        TextBufferProjectService projectService,
        ProjectConfigurationFilePathStore projectConfigurationFilePathStore,
        LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (workspaceAccessor is null)
        {
            throw new ArgumentNullException(nameof(workspaceAccessor));
        }

        if (projectService is null)
        {
            throw new ArgumentNullException(nameof(projectService));
        }

        if (projectConfigurationFilePathStore is null)
        {
            throw new ArgumentNullException(nameof(projectConfigurationFilePathStore));
        }

        if (languageServerFeatureOptions is null)
        {
            throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _workspaceAccessor = workspaceAccessor;
        _projectService = projectService;
        _projectConfigurationFilePathStore = projectConfigurationFilePathStore;
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    public DotNetProjectHost Create(DotNetProject project)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        var projectHost = new DefaultDotNetProjectHost(project, _projectSnapshotManagerDispatcher, _workspaceAccessor, _projectService, _projectConfigurationFilePathStore, _languageServerFeatureOptions);
        return projectHost;
    }
}

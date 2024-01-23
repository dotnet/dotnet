// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.IO;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

/// <summary>
/// This class is responsible for maintaining project information for projects that don't
/// use the Razor or Web SDK, or otherwise don't get picked up by our CPS bits, but have
/// .razor or .cshtml files regardless.
/// </summary>
[Shared]
[Export(typeof(FallbackProjectManager))]
[method: ImportingConstructor]
internal sealed class FallbackProjectManager(
    ProjectConfigurationFilePathStore projectConfigurationFilePathStore,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IProjectSnapshotManagerAccessor projectSnapshotManagerAccessor,
    ITelemetryReporter telemetryReporter)
{
    private readonly ProjectConfigurationFilePathStore _projectConfigurationFilePathStore = projectConfigurationFilePathStore;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IProjectSnapshotManagerAccessor _projectSnapshotManagerAccessor = projectSnapshotManagerAccessor;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    internal void DynamicFileAdded(ProjectId projectId, ProjectKey razorProjectKey, string projectFilePath, string filePath)
    {
        try
        {
            var project = _projectSnapshotManagerAccessor.Instance.GetLoadedProject(razorProjectKey);
            if (project is ProjectSnapshot { HostProject: FallbackHostProject })
            {
                // If this is a fallback project, then Roslyn may not track documents in the project, so these dynamic file notifications
                // are the only way to know about files in the project.
                AddFallbackDocument(razorProjectKey, filePath, projectFilePath);
            }
            else if (project is null)
            {
                // We have been asked to provide dynamic file info, which means there is a .razor or .cshtml file in the project
                // but for some reason our project system doesn't know about the project. In these cases (often when people don't
                // use the Razor or Web SDK) we spin up a fallback experience for them
                AddFallbackProject(projectId, filePath);
            }
        }
        catch (Exception ex)
        {
            _telemetryReporter.ReportFault(ex, "Error while trying to add fallback document to project");
        }
    }

    internal void DynamicFileRemoved(ProjectId projectId, ProjectKey razorProjectKey, string projectFilePath, string filePath)
    {
        try
        {
            var project = _projectSnapshotManagerAccessor.Instance.GetLoadedProject(razorProjectKey);
            if (project is ProjectSnapshot { HostProject: FallbackHostProject })
            {
                // If this is a fallback project, then Roslyn may not track documents in the project, so these dynamic file notifications
                // are the only way to know about files in the project.
                RemoveFallbackDocument(projectId, filePath, projectFilePath);
            }
        }
        catch (Exception ex)
        {
            _telemetryReporter.ReportFault(ex, "Error while trying to remove fallback document from project");
        }
    }

    private void AddFallbackProject(ProjectId projectId, string filePath)
    {
        var project = TryFindProjectForProjectId(projectId);
        if (project?.FilePath is null)
        {
            return;
        }

        var intermediateOutputPath = Path.GetDirectoryName(project.CompilationOutputInfo.AssemblyPath);
        if (intermediateOutputPath is null)
        {
            return;
        }

        var rootNamespace = project.DefaultNamespace;

        // We create this as a fallback project so that other parts of the system can reason about them - eg we don't do code
        // generation for closed files for documents in these projects. If these projects become "real", either because capabilities
        // change or simply a timing difference between Roslyn and our CPS components, the HostProject instance associated with
        // the project will be updated, and it will no longer be a fallback project.
        var hostProject = new FallbackHostProject(project.FilePath, intermediateOutputPath, FallbackRazorConfiguration.Latest, rootNamespace, project.Name);

        _projectSnapshotManagerAccessor.Instance.ProjectAdded(hostProject);

        AddFallbackDocument(hostProject.Key, filePath, project.FilePath);

        var configurationFilePath = Path.Combine(intermediateOutputPath, _languageServerFeatureOptions.ProjectConfigurationFileName);

        _projectConfigurationFilePathStore.Set(hostProject.Key, configurationFilePath);
    }

    private void AddFallbackDocument(ProjectKey projectKey, string filePath, string projectFilePath)
    {
        var hostDocument = CreateHostDocument(filePath, projectFilePath);
        if (hostDocument is null)
        {
            return;
        }

        var textLoader = new FileTextLoader(filePath, defaultEncoding: null);
        _projectSnapshotManagerAccessor.Instance.DocumentAdded(projectKey, hostDocument, textLoader);
    }

    private static HostDocument? CreateHostDocument(string filePath, string projectFilePath)
    {
        // The compiler only supports paths that are relative to the project root, so filter our files
        // that don't match
        var projectPath = FilePathNormalizer.GetNormalizedDirectoryName(projectFilePath);
        var normalizedFilePath = FilePathNormalizer.Normalize(filePath);
        if (!normalizedFilePath.StartsWith(projectPath, FilePathComparison.Instance))
        {
            return null;
        }

        var targetPath = filePath[projectPath.Length..];
        var hostDocument = new HostDocument(filePath, targetPath);
        return hostDocument;
    }

    private void RemoveFallbackDocument(ProjectId projectId, string filePath, string projectFilePath)
    {
        var project = TryFindProjectForProjectId(projectId);
        if (project is null)
        {
            return;
        }

        var projectKey = ProjectKey.From(project);
        if (projectKey is not { } razorProjectKey)
        {
            return;
        }

        var hostDocument = CreateHostDocument(filePath, projectFilePath);
        if (hostDocument is null)
        {
            return;
        }

        _projectSnapshotManagerAccessor.Instance.DocumentRemoved(razorProjectKey, hostDocument);
    }

    private Project? TryFindProjectForProjectId(ProjectId projectId)
    {
        if (_projectSnapshotManagerAccessor.Instance.Workspace is not { } workspace)
        {
            throw new InvalidOperationException("Can not map a ProjectId to a ProjectKey before the project is initialized");
        }

        var project = workspace.CurrentSolution.GetProject(projectId);
        if (project is null ||
            project.Language != LanguageNames.CSharp)
        {
            return null;
        }

        return project;
    }
}

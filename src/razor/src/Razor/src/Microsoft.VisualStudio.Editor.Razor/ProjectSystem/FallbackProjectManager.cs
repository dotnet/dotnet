// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

/// <summary>
/// This class is responsible for maintaining project information for projects that don't
/// use the Razor or Web SDK, or otherwise don't get picked up by our CPS bits, but have
/// .razor or .cshtml files regardless.
/// </summary>
[Export(typeof(FallbackProjectManager))]
[method: ImportingConstructor]
internal sealed class FallbackProjectManager(
    [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
    ProjectConfigurationFilePathStore projectConfigurationFilePathStore,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    ProjectSnapshotManagerBase projectManager,
    ProjectSnapshotManagerDispatcher dispatcher,
    IWorkspaceProvider workspaceProvider,
    ITelemetryReporter telemetryReporter)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ProjectConfigurationFilePathStore _projectConfigurationFilePathStore = projectConfigurationFilePathStore;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly ProjectSnapshotManagerBase _projectManager = projectManager;
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;
    private readonly IWorkspaceProvider _workspaceProvider = workspaceProvider;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    internal async Task DynamicFileAddedAsync(
        ProjectId projectId,
        ProjectKey razorProjectKey,
        string projectFilePath,
        string filePath,
        CancellationToken cancellationToken)
    {
        try
        {
            if (_projectManager.TryGetLoadedProject(razorProjectKey, out var project))
            {
                if (project is ProjectSnapshot { HostProject: FallbackHostProject })
                {
                    // If this is a fallback project, then Roslyn may not track documents in the project, so these dynamic file notifications
                    // are the only way to know about files in the project.
                    await AddFallbackDocumentAsync(razorProjectKey, filePath, projectFilePath, cancellationToken).ConfigureAwait(false);
                }
            }
            else
            {
                // We have been asked to provide dynamic file info, which means there is a .razor or .cshtml file in the project
                // but for some reason our project system doesn't know about the project. In these cases (often when people don't
                // use the Razor or Web SDK) we spin up a fallback experience for them
                await AddFallbackProjectAsync(projectId, filePath, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _telemetryReporter.ReportFault(ex, "Error while trying to add fallback document to project");
        }
    }

    internal async Task DynamicFileRemovedAsync(
        ProjectId projectId,
        ProjectKey razorProjectKey,
        string projectFilePath,
        string filePath,
        CancellationToken cancellationToken)
    {
        try
        {
            if (_projectManager.TryGetLoadedProject(razorProjectKey, out var project) &&
                project is ProjectSnapshot { HostProject: FallbackHostProject })
            {
                // If this is a fallback project, then Roslyn may not track documents in the project, so these dynamic file notifications
                // are the only way to know about files in the project.
                await RemoveFallbackDocumentAsync(projectId, filePath, projectFilePath, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _telemetryReporter.ReportFault(ex, "Error while trying to remove fallback document from project");
        }
    }

    private async Task AddFallbackProjectAsync(ProjectId projectId, string filePath, CancellationToken cancellationToken)
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

        await UpdateProjectManagerAsync(
                () => _projectManager.ProjectAdded(hostProject),
                cancellationToken)
            .ConfigureAwait(false);

        await AddFallbackDocumentAsync(hostProject.Key, filePath, project.FilePath, cancellationToken).ConfigureAwait(false);

        var configurationFilePath = Path.Combine(intermediateOutputPath, _languageServerFeatureOptions.ProjectConfigurationFileName);

        _projectConfigurationFilePathStore.Set(hostProject.Key, configurationFilePath);
    }

    private async Task AddFallbackDocumentAsync(ProjectKey projectKey, string filePath, string projectFilePath, CancellationToken cancellationToken)
    {
        var hostDocument = CreateHostDocument(filePath, projectFilePath);
        if (hostDocument is null)
        {
            return;
        }

        var textLoader = new FileTextLoader(filePath, defaultEncoding: null);

        await UpdateProjectManagerAsync(
                () => _projectManager.DocumentAdded(projectKey, hostDocument, textLoader),
                cancellationToken)
            .ConfigureAwait(false);
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

    private async Task RemoveFallbackDocumentAsync(ProjectId projectId, string filePath, string projectFilePath, CancellationToken cancellationToken)
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

        await UpdateProjectManagerAsync(
                () => _projectManager.DocumentRemoved(razorProjectKey, hostDocument),
                cancellationToken)
            .ConfigureAwait(false);
    }

    private Task UpdateProjectManagerAsync(Action action, CancellationToken cancellationToken)
    {
        return _dispatcher
            .RunAsync(() =>
            {
                RazorStartupInitializer.Initialize(_serviceProvider);

                action();
            },
            cancellationToken);
    }

    private Project? TryFindProjectForProjectId(ProjectId projectId)
    {
        var workspace = _workspaceProvider.GetWorkspace();

        var project = workspace.CurrentSolution.GetProject(projectId);
        if (project is null ||
            project.Language != LanguageNames.CSharp)
        {
            return null;
        }

        return project;
    }
}

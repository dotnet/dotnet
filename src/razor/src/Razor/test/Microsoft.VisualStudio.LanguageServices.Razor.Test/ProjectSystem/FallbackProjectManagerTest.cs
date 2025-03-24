// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Test.Common.TestProjectData;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

public class FallbackProjectManagerTest : VisualStudioWorkspaceTestBase
{
    private readonly FallbackProjectManager _fallbackProjectManger;
    private readonly TestProjectSnapshotManager _projectManager;

    public FallbackProjectManagerTest(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        var languageServerFeatureOptions = TestLanguageServerFeatureOptions.Instance;

        var serviceProvider = VsMocks.CreateServiceProvider(static b =>
            b.AddComponentModel(static b =>
            {
                var startupInitializer = new RazorStartupInitializer([]);
                b.AddExport(startupInitializer);
            }));

        _projectManager = CreateProjectSnapshotManager();

        _fallbackProjectManger = new FallbackProjectManager(
            serviceProvider,
            _projectManager,
            WorkspaceProvider,
            NoOpTelemetryReporter.Instance);
    }

    [UIFact]
    public async Task DynamicFileAdded_KnownProject_DoesNothing()
    {
        var hostProject = SomeProject with
        {
            Configuration = RazorConfiguration.Default,
            RootNamespace = "RootNamespace"
        };

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(hostProject);
        });

        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(
            projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(
                new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")));
        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(
            projectId,
            hostProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        await WaitForProjectManagerUpdatesAsync();

        var project = Assert.Single(_projectManager.GetProjects());
        Assert.False(_fallbackProjectManger.IsFallbackProject(project.Key));
    }

    [UIFact]
    public async Task DynamicFileAdded_UnknownProject_Adds()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(
            projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        await WaitForProjectManagerUpdatesAsync();

        var project = Assert.Single(_projectManager.GetProjects());
        Assert.Equal("DisplayName", project.DisplayName);
        Assert.Equal("RootNamespace", project.RootNamespace);

        Assert.True(_fallbackProjectManger.IsFallbackProject(project.Key));

        var documentFilePath = Assert.Single(project.DocumentFilePaths);
        Assert.Equal(SomeProjectFile1.FilePath, documentFilePath);
        Assert.Equal(SomeProjectFile1.TargetPath, project.GetRequiredDocument(documentFilePath).TargetPath);
    }

    [UIFact]
    public async Task DynamicFileAdded_UnknownToKnownProject_NotFallbackHostProject()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(
            projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        await WaitForProjectManagerUpdatesAsync();

        var project = Assert.Single(_projectManager.GetProjects());
        Assert.True(_fallbackProjectManger.IsFallbackProject(project.Key));

        var hostProject = SomeProject with
        {
            Configuration = RazorConfiguration.Default,
            RootNamespace = "RootNamespace"
        };

        await _projectManager.UpdateAsync(updater =>
        {
            updater.UpdateProjectConfiguration(hostProject);
        });

        project = Assert.Single(_projectManager.GetProjects());
        Assert.False(_fallbackProjectManger.IsFallbackProject(project.Key));
    }

    [UIFact]
    public async Task DynamicFileAdded_TrackedProject_AddsDocuments()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(
            projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        _fallbackProjectManger.DynamicFileAdded(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile2.FilePath,
            DisposalToken);

        _fallbackProjectManger.DynamicFileAdded(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectNestedComponentFile3.FilePath,
            DisposalToken);

        await WaitForProjectManagerUpdatesAsync();

        var project = Assert.Single(_projectManager.GetProjects());

        Assert.Collection(project.DocumentFilePaths.OrderBy(f => f), // DocumentFilePaths comes from a dictionary, so no sort guarantee
            f => Assert.Equal(SomeProjectFile1.FilePath, f),
            f => Assert.Equal(SomeProjectFile2.FilePath, f),
            f => Assert.Equal(SomeProjectNestedComponentFile3.FilePath, f));

        Assert.Equal(SomeProjectFile1.TargetPath, project.GetRequiredDocument(SomeProjectFile1.FilePath).TargetPath);
        Assert.Equal(SomeProjectFile2.TargetPath, project.GetRequiredDocument(SomeProjectFile2.FilePath).TargetPath);
        // The test data is created with a "\" so when the test runs on Linux, and direct string comparison wouldn't work
        Assert.True(FilePathNormalizer.AreFilePathsEquivalent(
            SomeProjectNestedComponentFile3.TargetPath,
            project.GetRequiredDocument(SomeProjectNestedComponentFile3.FilePath).TargetPath));
    }

    [UIFact]
    public async Task DynamicFileAdded_TrackedProject_IgnoresDocumentFromOutsideCone()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(
            projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        // These two represent linked files, or shared project items
        _fallbackProjectManger.DynamicFileAdded(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            AnotherProjectFile2.FilePath,
            DisposalToken);

        _fallbackProjectManger.DynamicFileAdded(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            AnotherProjectComponentFile1.FilePath,
            DisposalToken);

        await WaitForProjectManagerUpdatesAsync();

        var project = Assert.Single(_projectManager.GetProjects());

        Assert.Single(project.DocumentFilePaths,
            filePath => filePath == SomeProjectFile1.FilePath);

        Assert.Equal(SomeProjectFile1.TargetPath, project.GetRequiredDocument(SomeProjectFile1.FilePath).TargetPath);
    }

    private Task WaitForProjectManagerUpdatesAsync()
    {
        // The FallbackProjectManager fires and forgets any updates to the project manager.
        // We can perform a no-op update to wait until the FallbackProjectManager's work is done.
        return _projectManager.UpdateAsync(x => { });
    }
}

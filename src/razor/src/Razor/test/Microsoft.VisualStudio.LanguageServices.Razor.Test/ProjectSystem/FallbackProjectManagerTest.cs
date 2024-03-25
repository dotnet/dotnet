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
using Microsoft.VisualStudio.Editor.Razor;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Test.Common.TestProjectData;

namespace Microsoft.VisualStudio.LanguageServices.Razor.ProjectSystem;

public class FallbackProjectManagerTest : VisualStudioWorkspaceTestBase
{
    private readonly FallbackProjectManager _fallbackProjectManger;
    private readonly TestProjectSnapshotManager _projectManager;
    private readonly TestProjectConfigurationFilePathStore _projectConfigurationFilePathStore;

    public FallbackProjectManagerTest(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        var languageServerFeatureOptions = TestLanguageServerFeatureOptions.Instance;
        _projectConfigurationFilePathStore = new TestProjectConfigurationFilePathStore();

        var serviceProvider = VsMocks.CreateServiceProvider(static b =>
            b.AddComponentModel(static b =>
            {
                var startupInitializer = new RazorStartupInitializer([]);
                b.AddExport(startupInitializer);
            }));

        _projectManager = CreateProjectSnapshotManager();

        _fallbackProjectManger = new FallbackProjectManager(
            serviceProvider,
            _projectConfigurationFilePathStore,
            languageServerFeatureOptions,
            _projectManager,
            WorkspaceProvider,
            NoOpTelemetryReporter.Instance);
    }

    [UIFact]
    public async Task DynamicFileAdded_KnownProject_DoesNothing()
    {
        var hostProject = new HostProject(
            SomeProject.FilePath,
            SomeProject.IntermediateOutputPath,
            RazorConfiguration.Default,
            "RootNamespace",
            "DisplayName");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
        });

        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(
            projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(
                new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")));
        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            hostProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        var project = Assert.Single(_projectManager.GetProjects());
        Assert.IsNotType<FallbackHostProject>(((ProjectSnapshot)project).HostProject);
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

        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        var project = Assert.Single(_projectManager.GetProjects());
        Assert.Equal("DisplayName", project.DisplayName);
        Assert.Equal("RootNamespace", project.RootNamespace);

        Assert.IsType<FallbackHostProject>(((ProjectSnapshot)project).HostProject);

        var documentFilePath = Assert.Single(project.DocumentFilePaths);
        Assert.Equal(SomeProjectFile1.FilePath, documentFilePath);
        Assert.Equal(SomeProjectFile1.TargetPath, project.GetDocument(documentFilePath)!.TargetPath);
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

        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        var project = Assert.Single(_projectManager.GetProjects());
        Assert.IsType<FallbackHostProject>(((ProjectSnapshot)project).HostProject);

        var hostProject = new HostProject(
            SomeProject.FilePath,
            SomeProject.IntermediateOutputPath,
            RazorConfiguration.Default,
            "RootNamespace",
            "DisplayName");

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectConfigurationChanged(hostProject);
        });

        project = Assert.Single(_projectManager.GetProjects());
        Assert.IsNotType<FallbackHostProject>(((ProjectSnapshot)project).HostProject);
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

        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile2.FilePath,
            DisposalToken);

        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectNestedComponentFile3.FilePath,
            DisposalToken);

        var project = Assert.Single(_projectManager.GetProjects());

        Assert.Collection(project.DocumentFilePaths.OrderBy(f => f), // DocumentFilePaths comes from a dictionary, so no sort guarantee
            f => Assert.Equal(SomeProjectFile1.FilePath, f),
            f => Assert.Equal(SomeProjectFile2.FilePath, f),
            f => Assert.Equal(SomeProjectNestedComponentFile3.FilePath, f));

        Assert.Equal(SomeProjectFile1.TargetPath, project.GetDocument(SomeProjectFile1.FilePath)!.TargetPath);
        Assert.Equal(SomeProjectFile2.TargetPath, project.GetDocument(SomeProjectFile2.FilePath)!.TargetPath);
        // The test data is created with a "\" so when the test runs on Linux, and direct string comparison wouldn't work
        Assert.True(FilePathNormalizer.AreFilePathsEquivalent(SomeProjectNestedComponentFile3.TargetPath, project.GetDocument(SomeProjectNestedComponentFile3.FilePath)!.TargetPath));
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

        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        // These two represent linked files, or shared project items
        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            AnotherProjectFile2.FilePath,
            DisposalToken);

        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            AnotherProjectComponentFile1.FilePath,
            DisposalToken);

        var project = Assert.Single(_projectManager.GetProjects());

        Assert.Single(project.DocumentFilePaths,
            filePath => filePath == SomeProjectFile1.FilePath);

        Assert.Equal(SomeProjectFile1.TargetPath, project.GetDocument(SomeProjectFile1.FilePath)!.TargetPath);
    }

    [UIFact]
    public async Task DynamicFileAdded_UnknownProject_SetsConfigurationFileStore()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(
            projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        await _fallbackProjectManger.DynamicFileAddedAsync(
            projectId,
            SomeProject.Key,
            SomeProject.FilePath,
            SomeProjectFile1.FilePath,
            DisposalToken);

        var kvp = Assert.Single(_projectConfigurationFilePathStore.GetMappings());
        Assert.Equal(SomeProject.Key, kvp.Key);
        Assert.Equal(Path.Combine(SomeProject.IntermediateOutputPath, "project.razor.bin"), kvp.Value);
    }
}

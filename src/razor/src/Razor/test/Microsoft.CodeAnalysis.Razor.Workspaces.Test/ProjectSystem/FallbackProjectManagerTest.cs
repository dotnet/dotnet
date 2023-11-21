// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Moq;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.CodeAnalysis.Razor.TestProjectData;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Test.ProjectSystem;

public class FallbackProjectManagerTest : WorkspaceTestBase
{
    private FallbackProjectManager _fallbackProjectManger;
    private TestProjectSnapshotManager _projectSnapshotManager;
    private TestProjectConfigurationFilePathStore _projectConfigurationFilePathStore;

    public FallbackProjectManagerTest(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        var languageServerFeatureOptions = TestLanguageServerFeatureOptions.Instance;
        _projectConfigurationFilePathStore = new TestProjectConfigurationFilePathStore();

        var dispatcher = Mock.Of<ProjectSnapshotManagerDispatcher>(MockBehavior.Strict);
        _projectSnapshotManager = new TestProjectSnapshotManager(Workspace, dispatcher);

        var projectSnapshotManagerAccessor = Mock.Of<ProjectSnapshotManagerAccessor>(a => a.Instance == _projectSnapshotManager, MockBehavior.Strict);

        _fallbackProjectManger = new FallbackProjectManager(_projectConfigurationFilePathStore, languageServerFeatureOptions, projectSnapshotManagerAccessor, NoOpTelemetryReporter.Instance);
    }

    [Fact]
    public void DynamicFileAdded_KnownProject_DoesNothing()
    {
        var hostProject = new HostProject(SomeProject.FilePath, SomeProject.IntermediateOutputPath, RazorConfiguration.Default, "RootNamespace", "DisplayName");
        _projectSnapshotManager.ProjectAdded(hostProject);

        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")));
        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(projectId, hostProject.Key, SomeProject.FilePath, SomeProjectFile1.FilePath);

        var project = Assert.Single(_projectSnapshotManager.GetProjects());
        Assert.IsNotType<FallbackHostProject>(((ProjectSnapshot)project).HostProject);
    }

    [Fact]
    public void DynamicFileAdded_UnknownProject_Adds()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(projectId, SomeProject.Key, SomeProject.FilePath, SomeProjectFile1.FilePath);

        var project = Assert.Single(_projectSnapshotManager.GetProjects());
        Assert.Equal("DisplayName", project.DisplayName);
        Assert.Equal("RootNamespace", project.RootNamespace);

        Assert.IsType<FallbackHostProject>(((ProjectSnapshot)project).HostProject);

        var documentFilePath = Assert.Single(project.DocumentFilePaths);
        Assert.Equal(SomeProjectFile1.FilePath, documentFilePath);
        Assert.Equal(SomeProjectFile1.TargetPath, project.GetDocument(documentFilePath)!.TargetPath);
    }

    [Fact]
    public void DynamicFileAdded_UnknownToKnownProject_NotFallbackHostProject()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(projectId, SomeProject.Key, SomeProject.FilePath, SomeProjectFile1.FilePath);

        var project = Assert.Single(_projectSnapshotManager.GetProjects());
        Assert.IsType<FallbackHostProject>(((ProjectSnapshot)project).HostProject);

        var hostProject = new HostProject(SomeProject.FilePath, SomeProject.IntermediateOutputPath, RazorConfiguration.Default, "RootNamespace", "DisplayName");
        _projectSnapshotManager.ProjectConfigurationChanged(hostProject);

        project = Assert.Single(_projectSnapshotManager.GetProjects());
        Assert.IsNotType<FallbackHostProject>(((ProjectSnapshot)project).HostProject);
    }

    [Fact]
    public void DynamicFileAdded_TrackedProject_AddsDocuments()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(projectId, SomeProject.Key, SomeProject.FilePath, SomeProjectFile1.FilePath);

        _fallbackProjectManger.DynamicFileAdded(projectId, SomeProject.Key, SomeProject.FilePath, SomeProjectFile2.FilePath);

        _fallbackProjectManger.DynamicFileAdded(projectId, SomeProject.Key, SomeProject.FilePath, SomeProjectNestedComponentFile3.FilePath);

        var project = Assert.Single(_projectSnapshotManager.GetProjects());

        Assert.Collection(project.DocumentFilePaths.OrderBy(f => f), // DocumentFilePaths comes from a dictionary, so no sort guarantee
            f => Assert.Equal(SomeProjectFile1.FilePath, f),
            f => Assert.Equal(SomeProjectFile2.FilePath, f),
            f => Assert.Equal(SomeProjectNestedComponentFile3.FilePath, f));

        Assert.Equal(SomeProjectFile1.TargetPath, project.GetDocument(SomeProjectFile1.FilePath)!.TargetPath);
        Assert.Equal(SomeProjectFile2.TargetPath, project.GetDocument(SomeProjectFile2.FilePath)!.TargetPath);
        // The test data is created with a "\" so when the test runs on linux, and direct string comparison wouldn't work
        Assert.True(FilePathNormalizer.FilePathsEquivalent(SomeProjectNestedComponentFile3.TargetPath, project.GetDocument(SomeProjectNestedComponentFile3.FilePath)!.TargetPath));
    }

    [Fact]
    public void DynamicFileAdded_TrackedProject_IgnoresDocumentFromOutsideCone()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(projectId, SomeProject.Key, SomeProject.FilePath, SomeProjectFile1.FilePath);

        // These two represent linked files, or shared project items
        _fallbackProjectManger.DynamicFileAdded(projectId, SomeProject.Key, SomeProject.FilePath, AnotherProjectFile2.FilePath);

        _fallbackProjectManger.DynamicFileAdded(projectId, SomeProject.Key, SomeProject.FilePath, AnotherProjectComponentFile1.FilePath);

        var project = Assert.Single(_projectSnapshotManager.GetProjects());

        Assert.Collection(project.DocumentFilePaths,
            f => Assert.Equal(SomeProjectFile1.FilePath, f));

        Assert.Equal(SomeProjectFile1.TargetPath, project.GetDocument(SomeProjectFile1.FilePath)!.TargetPath);
    }

    [Fact]
    public void DynamicFileAdded_UnknownProject_SetsConfigurationFileStore()
    {
        var projectId = ProjectId.CreateNewId();
        var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Default, "DisplayName", "AssemblyName", LanguageNames.CSharp, filePath: SomeProject.FilePath)
            .WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath(Path.Combine(SomeProject.IntermediateOutputPath, "SomeProject.dll")))
            .WithDefaultNamespace("RootNamespace");

        Workspace.TryApplyChanges(Workspace.CurrentSolution.AddProject(projectInfo));

        _fallbackProjectManger.DynamicFileAdded(projectId, SomeProject.Key, SomeProject.FilePath, SomeProjectFile1.FilePath);

        var kvp = Assert.Single(_projectConfigurationFilePathStore.GetMappings());
        Assert.Equal(SomeProject.Key, kvp.Key);
        Assert.Equal(Path.Combine(SomeProject.IntermediateOutputPath, "project.razor.bin"), kvp.Value);
    }
}

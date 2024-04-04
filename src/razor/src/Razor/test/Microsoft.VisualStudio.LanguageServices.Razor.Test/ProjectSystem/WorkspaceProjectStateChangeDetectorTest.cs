// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServices.Razor;
using Microsoft.VisualStudio.LanguageServices.Razor.Test;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class WorkspaceProjectStateChangeDetectorTest : VisualStudioWorkspaceTestBase
{
    private readonly HostProject _hostProjectOne;
    private readonly HostProject _hostProjectTwo;
    private readonly HostProject _hostProjectThree;
    private readonly Solution _emptySolution;
    private readonly Solution _solutionWithOneProject;
    private readonly Solution _solutionWithTwoProjects;
    private readonly Solution _solutionWithDependentProject;
    private readonly Project _projectNumberOne;
    private readonly Project _projectNumberTwo;
    private readonly Project _projectNumberThree;

    private readonly DocumentId _cshtmlDocumentId;
    private readonly DocumentId _razorDocumentId;
    private readonly DocumentId _backgroundVirtualCSharpDocumentId;
    private readonly DocumentId _partialComponentClassDocumentId;

    public WorkspaceProjectStateChangeDetectorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _emptySolution = Workspace.CurrentSolution;

        var projectId1 = ProjectId.CreateNewId("One");
        var projectId2 = ProjectId.CreateNewId("Two");
        var projectId3 = ProjectId.CreateNewId("Three");

        _cshtmlDocumentId = DocumentId.CreateNewId(projectId1);
        var cshtmlDocumentInfo = DocumentInfo.Create(_cshtmlDocumentId, "Test", filePath: "file.cshtml.g.cs");
        _razorDocumentId = DocumentId.CreateNewId(projectId1);
        var razorDocumentInfo = DocumentInfo.Create(_razorDocumentId, "Test", filePath: "file.razor.g.cs");
        _backgroundVirtualCSharpDocumentId = DocumentId.CreateNewId(projectId1);
        var backgroundDocumentInfo = DocumentInfo.Create(_backgroundVirtualCSharpDocumentId, "Test", filePath: "file.razor__bg__virtual.cs");
        _partialComponentClassDocumentId = DocumentId.CreateNewId(projectId1);
        var partialComponentClassDocumentInfo = DocumentInfo.Create(_partialComponentClassDocumentId, "Test", filePath: "file.razor.cs");

        _solutionWithTwoProjects = Workspace.CurrentSolution
            .AddProject(ProjectInfo.Create(
                projectId1,
                VersionStamp.Default,
                "One",
                "One",
                LanguageNames.CSharp,
                filePath: "One.csproj",
                documents: [cshtmlDocumentInfo, razorDocumentInfo, partialComponentClassDocumentInfo, backgroundDocumentInfo]).WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath("obj1\\One.dll")))
            .AddProject(ProjectInfo.Create(
                projectId2,
                VersionStamp.Default,
                "Two",
                "Two",
                LanguageNames.CSharp,
                filePath: "Two.csproj").WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath("obj2\\Two.dll")));

        _solutionWithOneProject = _emptySolution
            .AddProject(ProjectInfo.Create(
                projectId3,
                VersionStamp.Default,
                "Three",
                "Three",
                LanguageNames.CSharp,
                filePath: "Three.csproj").WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath("obj3\\Three.dll")));

        var project2Reference = new ProjectReference(projectId2);
        var project3Reference = new ProjectReference(projectId3);
        _solutionWithDependentProject = Workspace.CurrentSolution
            .AddProject(ProjectInfo.Create(
                projectId1,
                VersionStamp.Default,
                "One",
                "One",
                LanguageNames.CSharp,
                filePath: "One.csproj",
                documents: [cshtmlDocumentInfo, razorDocumentInfo, partialComponentClassDocumentInfo, backgroundDocumentInfo],
                projectReferences: [project2Reference]).WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath("obj1\\One.dll")))
            .AddProject(ProjectInfo.Create(
                projectId2,
                VersionStamp.Default,
                "Two",
                "Two",
                LanguageNames.CSharp,
                filePath: "Two.csproj",
                projectReferences: [project3Reference]).WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath("obj2\\Two.dll")))
            .AddProject(ProjectInfo.Create(
                projectId3,
                VersionStamp.Default,
                "Three",
                "Three",
                LanguageNames.CSharp,
                filePath: "Three.csproj",
                documents: [razorDocumentInfo]).WithCompilationOutputInfo(new CompilationOutputInfo().WithAssemblyPath("obj3\\Three.dll")));

        _projectNumberOne = _solutionWithTwoProjects.GetProject(projectId1);
        _projectNumberTwo = _solutionWithTwoProjects.GetProject(projectId2);
        _projectNumberThree = _solutionWithOneProject.GetProject(projectId3);

        _hostProjectOne = new HostProject("One.csproj", "obj1", FallbackRazorConfiguration.MVC_1_1, "One");
        _hostProjectTwo = new HostProject("Two.csproj", "obj2", FallbackRazorConfiguration.MVC_1_1, "Two");
        _hostProjectThree = new HostProject("Three.csproj", "obj3", FallbackRazorConfiguration.MVC_1_1, "Three");
    }

    [UIFact]
    public async Task SolutionClosing_StopsActiveWork()
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        using var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        var workspaceChangedTask = detectorAccessor.ListenForWorkspaceChangesAsync(
            WorkspaceChangeKind.ProjectAdded,
            WorkspaceChangeKind.ProjectAdded);

        Workspace.TryApplyChanges(_solutionWithTwoProjects);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
        });

        await workspaceChangedTask;
        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        generator.Clear();

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.SolutionClosed();

            // Trigger a project removed event while solution is closing to clear state.
            updater.ProjectRemoved(_hostProjectOne.Key);
        });

        // Assert

        Assert.Empty(generator.Updates);
    }

    [UITheory]
    [InlineData(WorkspaceChangeKind.DocumentAdded)]
    [InlineData(WorkspaceChangeKind.DocumentChanged)]
    [InlineData(WorkspaceChangeKind.DocumentRemoved)]
    public async Task WorkspaceChanged_DocumentEvents_EnqueuesUpdatesForDependentProjects(WorkspaceChangeKind kind)
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        using var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
            updater.ProjectAdded(_hostProjectTwo);
            updater.ProjectAdded(_hostProjectThree);
        });

        // Initialize with a project. This will get removed.
        var e = new WorkspaceChangeEventArgs(WorkspaceChangeKind.SolutionAdded, oldSolution: _emptySolution, newSolution: _solutionWithOneProject);
        detectorAccessor.WorkspaceChanged(e);

        e = new WorkspaceChangeEventArgs(kind, oldSolution: _solutionWithOneProject, newSolution: _solutionWithDependentProject);

        var solution = _solutionWithDependentProject.WithProjectAssemblyName(_projectNumberThree.Id, "Changed");

        e = new WorkspaceChangeEventArgs(kind, oldSolution: _solutionWithDependentProject, newSolution: solution, projectId: _projectNumberThree.Id, documentId: _razorDocumentId);

        // Act
        detectorAccessor.WorkspaceChanged(e);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Equal(3, generator.Updates.Count);
        Assert.Contains(generator.Updates, u => u.ProjectSnapshot.Key == ProjectKey.From(_projectNumberOne));
        Assert.Contains(generator.Updates, u => u.ProjectSnapshot.Key == ProjectKey.From(_projectNumberTwo));
        Assert.Contains(generator.Updates, u => u.ProjectSnapshot.Key == ProjectKey.From(_projectNumberThree));
    }

    [UITheory]
    [InlineData(WorkspaceChangeKind.ProjectChanged)]
    [InlineData(WorkspaceChangeKind.ProjectAdded)]
    [InlineData(WorkspaceChangeKind.ProjectRemoved)]
    public async Task WorkspaceChanged_ProjectEvents_EnqueuesUpdatesForDependentProjects(WorkspaceChangeKind kind)
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
            updater.ProjectAdded(_hostProjectTwo);
            updater.ProjectAdded(_hostProjectThree);
        });

        // Initialize with a project. This will get removed.
        var e = new WorkspaceChangeEventArgs(WorkspaceChangeKind.SolutionAdded, oldSolution: _emptySolution, newSolution: _solutionWithOneProject);
        detectorAccessor.WorkspaceChanged(e);

        e = new WorkspaceChangeEventArgs(kind, oldSolution: _solutionWithOneProject, newSolution: _solutionWithDependentProject);

        var solution = _solutionWithDependentProject.WithProjectAssemblyName(_projectNumberThree.Id, "Changed");

        e = new WorkspaceChangeEventArgs(kind, oldSolution: _solutionWithDependentProject, newSolution: solution, projectId: _projectNumberThree.Id);

        // Act
        detectorAccessor.WorkspaceChanged(e);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Equal(3, generator.Updates.Count);
        Assert.Contains(generator.Updates, u => u.ProjectSnapshot.Key == ProjectKey.From(_projectNumberOne));
        Assert.Contains(generator.Updates, u => u.ProjectSnapshot.Key == ProjectKey.From(_projectNumberTwo));
        Assert.Contains(generator.Updates, u => u.ProjectSnapshot.Key == ProjectKey.From(_projectNumberThree));
    }

    [UITheory]
    [InlineData(WorkspaceChangeKind.SolutionAdded)]
    [InlineData(WorkspaceChangeKind.SolutionChanged)]
    [InlineData(WorkspaceChangeKind.SolutionCleared)]
    [InlineData(WorkspaceChangeKind.SolutionReloaded)]
    [InlineData(WorkspaceChangeKind.SolutionRemoved)]
    public async Task WorkspaceChanged_SolutionEvents_EnqueuesUpdatesForProjectsInSolution(WorkspaceChangeKind kind)
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
            updater.ProjectAdded(_hostProjectTwo);
        });

        var e = new WorkspaceChangeEventArgs(kind, oldSolution: _emptySolution, newSolution: _solutionWithTwoProjects);

        // Act
        detectorAccessor.WorkspaceChanged(e);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Collection(
            generator.Updates,
            p => Assert.Equal(_projectNumberOne.Id, p.WorkspaceProject.Id),
            p => Assert.Equal(_projectNumberTwo.Id, p.WorkspaceProject.Id));
    }

    [UITheory]
    [InlineData(WorkspaceChangeKind.SolutionAdded)]
    [InlineData(WorkspaceChangeKind.SolutionChanged)]
    [InlineData(WorkspaceChangeKind.SolutionCleared)]
    [InlineData(WorkspaceChangeKind.SolutionReloaded)]
    [InlineData(WorkspaceChangeKind.SolutionRemoved)]
    public async Task WorkspaceChanged_SolutionEvents_EnqueuesStateClear_EnqueuesSolutionProjectUpdates(WorkspaceChangeKind kind)
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
            updater.ProjectAdded(_hostProjectTwo);
            updater.ProjectAdded(_hostProjectThree);
        });

        // Initialize with a project. This will get removed.
        var e = new WorkspaceChangeEventArgs(WorkspaceChangeKind.SolutionAdded, oldSolution: _emptySolution, newSolution: _solutionWithOneProject);
        detectorAccessor.WorkspaceChanged(e);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        e = new WorkspaceChangeEventArgs(kind, oldSolution: _solutionWithOneProject, newSolution: _solutionWithTwoProjects);

        // Act
        detectorAccessor.WorkspaceChanged(e);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Collection(
            generator.Updates,
            p => Assert.Equal(_projectNumberThree.Id, p.WorkspaceProject.Id),
            p => Assert.Null(p.WorkspaceProject),
            p => Assert.Equal(_projectNumberOne.Id, p.WorkspaceProject.Id),
            p => Assert.Equal(_projectNumberTwo.Id, p.WorkspaceProject.Id));
    }

    [UITheory]
    [InlineData(WorkspaceChangeKind.ProjectChanged)]
    [InlineData(WorkspaceChangeKind.ProjectReloaded)]
    public async Task WorkspaceChanged_ProjectChangeEvents_UpdatesProjectState_AfterDelay(WorkspaceChangeKind kind)
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
        });

        // Stop any existing work and clear out any updates that we might have received.
        detectorAccessor.CancelExistingWork();
        generator.Clear();

        // Create a listener for the workspace change we're about to send.
        var listenerTask = detectorAccessor.ListenForWorkspaceChangesAsync(kind);

        var solution = _solutionWithTwoProjects.WithProjectAssemblyName(_projectNumberOne.Id, "Changed");
        var e = new WorkspaceChangeEventArgs(kind, oldSolution: _solutionWithTwoProjects, newSolution: solution, projectId: _projectNumberOne.Id);

        // Act
        detectorAccessor.WorkspaceChanged(e);

        await listenerTask;
        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        var update = Assert.Single(generator.Updates);
        Assert.Equal(update.WorkspaceProject.Id, _projectNumberOne.Id);
        Assert.Equal(update.ProjectSnapshot.FilePath, _hostProjectOne.FilePath);
    }

    [UIFact]
    public async Task WorkspaceChanged_DocumentChanged_BackgroundVirtualCS_UpdatesProjectState_AfterDelay()
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        Workspace.TryApplyChanges(_solutionWithTwoProjects);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
        });

        generator.Clear();

        var solution = _solutionWithTwoProjects.WithDocumentText(_backgroundVirtualCSharpDocumentId, SourceText.From("public class Foo{}"));
        var e = new WorkspaceChangeEventArgs(WorkspaceChangeKind.DocumentChanged, oldSolution: _solutionWithTwoProjects, newSolution: solution, projectId: _projectNumberOne.Id, _backgroundVirtualCSharpDocumentId);

        // Act
        detectorAccessor.WorkspaceChanged(e);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        var update = Assert.Single(generator.Updates);
        Assert.Equal(update.WorkspaceProject.Id, _projectNumberOne.Id);
        Assert.Equal(update.ProjectSnapshot.FilePath, _hostProjectOne.FilePath);
    }

    [UIFact]
    public async Task WorkspaceChanged_DocumentChanged_CSHTML_UpdatesProjectState_AfterDelay()
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        Workspace.TryApplyChanges(_solutionWithTwoProjects);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
        });

        generator.Clear();

        var solution = _solutionWithTwoProjects.WithDocumentText(_cshtmlDocumentId, SourceText.From("Hello World"));
        var e = new WorkspaceChangeEventArgs(WorkspaceChangeKind.DocumentChanged, oldSolution: _solutionWithTwoProjects, newSolution: solution, projectId: _projectNumberOne.Id, _cshtmlDocumentId);

        // Act
        detectorAccessor.WorkspaceChanged(e);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        var update = Assert.Single(generator.Updates);
        Assert.Equal(update.WorkspaceProject.Id, _projectNumberOne.Id);
        Assert.Equal(update.ProjectSnapshot.FilePath, _hostProjectOne.FilePath);
    }

    [UIFact]
    public async Task WorkspaceChanged_DocumentChanged_Razor_UpdatesProjectState_AfterDelay()
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        Workspace.TryApplyChanges(_solutionWithTwoProjects);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
        });

        generator.Clear();

        var solution = _solutionWithTwoProjects.WithDocumentText(_razorDocumentId, SourceText.From("Hello World"));
        var e = new WorkspaceChangeEventArgs(WorkspaceChangeKind.DocumentChanged, oldSolution: _solutionWithTwoProjects, newSolution: solution, projectId: _projectNumberOne.Id, _razorDocumentId);

        // Act
        detectorAccessor.WorkspaceChanged(e);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        var update = Assert.Single(generator.Updates);
        Assert.Equal(update.WorkspaceProject.Id, _projectNumberOne.Id);
        Assert.Equal(update.ProjectSnapshot.FilePath, _hostProjectOne.FilePath);
    }

    [UIFact]
    public async Task WorkspaceChanged_DocumentChanged_PartialComponent_UpdatesProjectState_AfterDelay()
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        Workspace.TryApplyChanges(_solutionWithTwoProjects);

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
        });

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();
        generator.Clear();

        var sourceText = SourceText.From($$"""
            public partial class TestComponent : {{ComponentsApi.IComponent.MetadataName}} {}
            namespace Microsoft.AspNetCore.Components
            {
                public interface IComponent {}
            }
            """);
        var syntaxTreeRoot = await CSharpSyntaxTree.ParseText(sourceText).GetRootAsync();
        var solution = _solutionWithTwoProjects
            .WithDocumentText(_partialComponentClassDocumentId, sourceText)
            .WithDocumentSyntaxRoot(_partialComponentClassDocumentId, syntaxTreeRoot, PreservationMode.PreserveIdentity);
        var document = solution.GetDocument(_partialComponentClassDocumentId);

        // The change detector only operates when a semantic model / syntax tree is available.
        await document.GetSyntaxRootAsync();
        await document.GetSemanticModelAsync();

        var e = new WorkspaceChangeEventArgs(WorkspaceChangeKind.DocumentChanged, oldSolution: solution, newSolution: solution, projectId: _projectNumberOne.Id, _partialComponentClassDocumentId);

        // Act
        detectorAccessor.WorkspaceChanged(e);

        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        var update = Assert.Single(generator.Updates);
        Assert.Equal(update.WorkspaceProject.Id, _projectNumberOne.Id);
        Assert.Equal(update.ProjectSnapshot.FilePath, _hostProjectOne.FilePath);
    }

    [UIFact]
    public async Task WorkspaceChanged_ProjectRemovedEvent_QueuesProjectStateRemoval()
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectOne);
            updater.ProjectAdded(_hostProjectTwo);
        });

        var solution = _solutionWithTwoProjects.RemoveProject(_projectNumberOne.Id);
        var e = new WorkspaceChangeEventArgs(WorkspaceChangeKind.ProjectRemoved, oldSolution: _solutionWithTwoProjects, newSolution: solution, projectId: _projectNumberOne.Id);

        // Act
        detectorAccessor.WorkspaceChanged(e);
        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Single(
            generator.Updates,
            p => p.WorkspaceProject is null);
    }

    [UIFact]
    public async Task WorkspaceChanged_ProjectAddedEvent_AddsProject()
    {
        // Arrange
        var generator = new TestProjectWorkspaceStateGenerator();
        var projectManager = CreateProjectSnapshotManager();
        var detector = new WorkspaceProjectStateChangeDetector(generator, projectManager, TestLanguageServerFeatureOptions.Instance, WorkspaceProvider, Dispatcher, LoggerFactory);
        var detectorAccessor = detector.GetTestAccessor();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProjectThree);
        });

        var solution = _solutionWithOneProject;
        var e = new WorkspaceChangeEventArgs(WorkspaceChangeKind.ProjectAdded, oldSolution: _emptySolution, newSolution: solution, projectId: _projectNumberThree.Id);

        // Act
        var listenerTask = detectorAccessor.ListenForWorkspaceChangesAsync(WorkspaceChangeKind.ProjectAdded);
        detectorAccessor.WorkspaceChanged(e);
        await listenerTask;
        await detectorAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Single(
            generator.Updates,
            p => p.WorkspaceProject.Id == _projectNumberThree.Id);
    }

    [Fact]
    public async Task IsPartialComponentClass_NoIComponent_ReturnsFalse()
    {
        // Arrange
        var sourceText = SourceText.From("""
            public partial class TestComponent{}
            """);
        var syntaxTreeRoot = await CSharpSyntaxTree.ParseText(sourceText).GetRootAsync();
        var solution = _solutionWithTwoProjects
            .WithDocumentText(_partialComponentClassDocumentId, sourceText)
            .WithDocumentSyntaxRoot(_partialComponentClassDocumentId, syntaxTreeRoot, PreservationMode.PreserveIdentity);
        var document = solution.GetDocument(_partialComponentClassDocumentId);

        // Initialize document
        await document.GetSyntaxRootAsync();
        await document.GetSemanticModelAsync();

        // Act
        var result = WorkspaceProjectStateChangeDetector.IsPartialComponentClass(document);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsPartialComponentClass_InitializedDocument_ReturnsTrue()
    {
        // Arrange
        var sourceText = SourceText.From($$"""
            public partial class TestComponent : {{ComponentsApi.IComponent.MetadataName}} {}
            namespace Microsoft.AspNetCore.Components
            {
                public interface IComponent {}
            }
            """);
        var syntaxTreeRoot = await CSharpSyntaxTree.ParseText(sourceText).GetRootAsync();
        var solution = _solutionWithTwoProjects
            .WithDocumentText(_partialComponentClassDocumentId, sourceText)
            .WithDocumentSyntaxRoot(_partialComponentClassDocumentId, syntaxTreeRoot, PreservationMode.PreserveIdentity);
        var document = solution.GetDocument(_partialComponentClassDocumentId);

        // Initialize document
        await document.GetSyntaxRootAsync();
        await document.GetSemanticModelAsync();

        // Act
        var result = WorkspaceProjectStateChangeDetector.IsPartialComponentClass(document);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsPartialComponentClass_Uninitialized_ReturnsFalse()
    {
        // Arrange
        var sourceText = SourceText.From($$"""
            public partial class TestComponent : {{ComponentsApi.IComponent.MetadataName}} {}
            namespace Microsoft.AspNetCore.Components
            {
                public interface IComponent {}
            }
            """);
        var syntaxTreeRoot = CSharpSyntaxTree.ParseText(sourceText).GetRoot();
        var solution = _solutionWithTwoProjects
            .WithDocumentText(_partialComponentClassDocumentId, sourceText)
            .WithDocumentSyntaxRoot(_partialComponentClassDocumentId, syntaxTreeRoot, PreservationMode.PreserveIdentity);
        var document = solution.GetDocument(_partialComponentClassDocumentId);

        // Act
        var result = WorkspaceProjectStateChangeDetector.IsPartialComponentClass(document);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsPartialComponentClass_UninitializedSemanticModel_ReturnsFalse()
    {
        // Arrange
        var sourceText = SourceText.From($$"""
            public partial class TestComponent : {{ComponentsApi.IComponent.MetadataName}} {}
            namespace Microsoft.AspNetCore.Components
            {
                public interface IComponent {}
            }
            """);
        var syntaxTreeRoot = await CSharpSyntaxTree.ParseText(sourceText).GetRootAsync();
        var solution = _solutionWithTwoProjects
            .WithDocumentText(_partialComponentClassDocumentId, sourceText)
            .WithDocumentSyntaxRoot(_partialComponentClassDocumentId, syntaxTreeRoot, PreservationMode.PreserveIdentity);
        var document = solution.GetDocument(_partialComponentClassDocumentId);

        await document.GetSyntaxRootAsync();

        // Act
        var result = WorkspaceProjectStateChangeDetector.IsPartialComponentClass(document);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsPartialComponentClass_NonClass_ReturnsFalse()
    {
        // Arrange
        var sourceText = SourceText.From(string.Empty);
        var syntaxTreeRoot = await CSharpSyntaxTree.ParseText(sourceText).GetRootAsync();
        var solution = _solutionWithTwoProjects
            .WithDocumentText(_partialComponentClassDocumentId, sourceText)
            .WithDocumentSyntaxRoot(_partialComponentClassDocumentId, syntaxTreeRoot, PreservationMode.PreserveIdentity);
        var document = solution.GetDocument(_partialComponentClassDocumentId);

        // Initialize document
        await document.GetSyntaxRootAsync();
        await document.GetSemanticModelAsync();

        // Act
        var result = WorkspaceProjectStateChangeDetector.IsPartialComponentClass(document);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task IsPartialComponentClass_MultipleClassesOneComponentPartial_ReturnsTrue()
    {

        // Arrange
        var sourceText = SourceText.From($$"""
            public partial class NonComponent1 {}
            public class NonComponent2 {}
            public partial class TestComponent : {{ComponentsApi.IComponent.MetadataName}} {}
            public partial class NonComponent3 {}
            public class NonComponent4 {}
            namespace Microsoft.AspNetCore.Components
            {
                public interface IComponent {}
            }
            """);
        var syntaxTreeRoot = await CSharpSyntaxTree.ParseText(sourceText).GetRootAsync();
        var solution = _solutionWithTwoProjects
            .WithDocumentText(_partialComponentClassDocumentId, sourceText)
            .WithDocumentSyntaxRoot(_partialComponentClassDocumentId, syntaxTreeRoot, PreservationMode.PreserveIdentity);
        var document = solution.GetDocument(_partialComponentClassDocumentId);

        // Initialize document
        await document.GetSyntaxRootAsync();
        await document.GetSemanticModelAsync();

        // Act
        var result = WorkspaceProjectStateChangeDetector.IsPartialComponentClass(document);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsPartialComponentClass_NonComponents_ReturnsFalse()
    {

        // Arrange
        var sourceText = SourceText.From("""
            public partial class NonComponent1 {}
            public class NonComponent2 {}
            public partial class NonComponent3 {}
            public class NonComponent4 {}
            namespace Microsoft.AspNetCore.Components
            {
                public interface IComponent {}
            }
            """);
        var syntaxTreeRoot = await CSharpSyntaxTree.ParseText(sourceText).GetRootAsync();
        var solution = _solutionWithTwoProjects
            .WithDocumentText(_partialComponentClassDocumentId, sourceText)
            .WithDocumentSyntaxRoot(_partialComponentClassDocumentId, syntaxTreeRoot, PreservationMode.PreserveIdentity);
        var document = solution.GetDocument(_partialComponentClassDocumentId);

        // Initialize document
        await document.GetSyntaxRootAsync();
        await document.GetSemanticModelAsync();

        // Act
        var result = WorkspaceProjectStateChangeDetector.IsPartialComponentClass(document);

        // Assert
        Assert.False(result);
    }
}

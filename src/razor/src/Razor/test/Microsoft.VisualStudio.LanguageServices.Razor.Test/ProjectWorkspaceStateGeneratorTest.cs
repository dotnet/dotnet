// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Test;

public class ProjectWorkspaceStateGeneratorTest : ProjectSnapshotManagerDispatcherTestBase
{
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider;
    private readonly TestTagHelperResolver _tagHelperResolver;
    private readonly Project _workspaceProject;
    private readonly ProjectSnapshot _projectSnapshot;
    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;

    public ProjectWorkspaceStateGeneratorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _projectEngineFactoryProvider = Mock.Of<IProjectEngineFactoryProvider>(MockBehavior.Strict);

        _tagHelperResolver = new TestTagHelperResolver(
            ImmutableArray.Create(TagHelperDescriptorBuilder.Create("ResolvableTagHelper", "TestAssembly").Build()));

        var testServices = TestServices.Create([], []);
        var workspace = TestWorkspace.Create(testServices);
        AddDisposable(workspace);

        var projectId = ProjectId.CreateNewId("Test");
        var solution = workspace.CurrentSolution.AddProject(ProjectInfo.Create(
            projectId,
            VersionStamp.Default,
            "Test",
            "Test",
            LanguageNames.CSharp,
            TestProjectData.SomeProject.FilePath));
        _workspaceProject = solution.GetProject(projectId).AssumeNotNull();
        _projectSnapshot = new ProjectSnapshot(
            ProjectState.Create(_projectEngineFactoryProvider, TestProjectData.SomeProject, ProjectWorkspaceState.Default));
        _projectWorkspaceStateWithTagHelpers = ProjectWorkspaceState.Create(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build()));
    }

    private IProjectSnapshotManagerAccessor CreateProjectManagerAccessor()
    {
        var projectManager = new TestProjectSnapshotManager(_projectEngineFactoryProvider, Dispatcher);
        var mock = new Mock<IProjectSnapshotManagerAccessor>(MockBehavior.Strict);
        mock.SetupGet(x => x.Instance).Returns(projectManager);

        return mock.Object;
    }

    [UIFact]
    public void Dispose_MakesUpdateNoop()
    {
        // Arrange
        var projectManagerAccessor = CreateProjectManagerAccessor();
        using var stateGenerator = new ProjectWorkspaceStateGenerator(projectManagerAccessor, _tagHelperResolver, Dispatcher, ErrorReporter, NoOpTelemetryReporter.Instance);
        stateGenerator.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        stateGenerator.Dispose();
        stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);

        // Assert
        Assert.Empty(stateGenerator.Updates);
    }

    [UIFact]
    public void Update_StartsUpdateTask()
    {
        // Arrange
        var projectManagerAccessor = CreateProjectManagerAccessor();
        using var stateGenerator = new ProjectWorkspaceStateGenerator(projectManagerAccessor, _tagHelperResolver, Dispatcher, ErrorReporter, NoOpTelemetryReporter.Instance);
        stateGenerator.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);

        // Assert
        var update = Assert.Single(stateGenerator.Updates);
        Assert.False(update.Value.Task.IsCompleted);
    }

    [UIFact]
    public void Update_SoftCancelsIncompleteTaskForSameProject()
    {
        // Arrange
        var projectManagerAccessor = CreateProjectManagerAccessor();
        using var stateGenerator = new ProjectWorkspaceStateGenerator(projectManagerAccessor, _tagHelperResolver, Dispatcher, ErrorReporter, NoOpTelemetryReporter.Instance);
        stateGenerator.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);
        stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);
        var initialUpdate = stateGenerator.Updates.Single().Value;

        // Act
        stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);

        // Assert
        Assert.True(initialUpdate.Cts.IsCancellationRequested);
    }

    [UIFact]
    public async Task Update_NullWorkspaceProject_ClearsProjectWorkspaceState()
    {
        // Arrange
        var projectManagerAccessor = CreateProjectManagerAccessor();
        using var stateGenerator = new ProjectWorkspaceStateGenerator(projectManagerAccessor, _tagHelperResolver, Dispatcher, ErrorReporter, NoOpTelemetryReporter.Instance);
        stateGenerator.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);
        projectManagerAccessor.Instance.ProjectAdded(_projectSnapshot.HostProject);
        projectManagerAccessor.Instance.ProjectWorkspaceStateChanged(_projectSnapshot.Key, _projectWorkspaceStateWithTagHelpers);

        // Act
        stateGenerator.Update(workspaceProject: null, _projectSnapshot, DisposalToken);

        // Jump off the UI thread so the background work can complete.
        await Task.Run(() => stateGenerator.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        // Assert
        var newProjectSnapshot = projectManagerAccessor.Instance.GetLoadedProject(_projectSnapshot.Key);
        Assert.NotNull(newProjectSnapshot);
        Assert.Empty(await newProjectSnapshot.GetTagHelpersAsync(CancellationToken.None));
    }

    [UIFact]
    public async Task Update_ResolvesTagHelpersAndUpdatesWorkspaceState()
    {
        // Arrange
        var projectManagerAccessor = CreateProjectManagerAccessor();
        using var stateGenerator = new ProjectWorkspaceStateGenerator(projectManagerAccessor, _tagHelperResolver, Dispatcher, ErrorReporter, NoOpTelemetryReporter.Instance);
        stateGenerator.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);
        projectManagerAccessor.Instance.ProjectAdded(_projectSnapshot.HostProject);

        // Act
        stateGenerator.Update(_workspaceProject, _projectSnapshot, DisposalToken);

        // Jump off the UI thread so the background work can complete.
        await Task.Run(() => stateGenerator.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        // Assert
        var newProjectSnapshot = projectManagerAccessor.Instance.GetLoadedProject(_projectSnapshot.Key);
        Assert.NotNull(newProjectSnapshot);
        Assert.Equal<TagHelperDescriptor>(_tagHelperResolver.TagHelpers, await newProjectSnapshot.GetTagHelpersAsync(CancellationToken.None));
    }
}

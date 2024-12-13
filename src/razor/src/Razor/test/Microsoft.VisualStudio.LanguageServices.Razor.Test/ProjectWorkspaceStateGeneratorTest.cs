// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor;

public class ProjectWorkspaceStateGeneratorTest : VisualStudioWorkspaceTestBase
{
    private readonly TestTagHelperResolver _tagHelperResolver;
    private readonly Project _workspaceProject;
    private readonly ProjectSnapshot _projectSnapshot;
    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;
    private readonly TestProjectSnapshotManager _projectManager;

    public ProjectWorkspaceStateGeneratorTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _tagHelperResolver = new TestTagHelperResolver(
            [TagHelperDescriptorBuilder.Create("ResolvableTagHelper", "TestAssembly").Build()]);

        var projectId = ProjectId.CreateNewId("Test");
        var solution = Workspace.CurrentSolution.AddProject(ProjectInfo.Create(
            projectId,
            VersionStamp.Default,
            "Test",
            "Test",
            LanguageNames.CSharp,
            TestProjectData.SomeProject.FilePath));
        _workspaceProject = solution.GetProject(projectId).AssumeNotNull();
        _projectSnapshot = new ProjectSnapshot(
            ProjectState.Create(ProjectEngineFactoryProvider, LanguageServerFeatureOptions, TestProjectData.SomeProject, ProjectWorkspaceState.Default));
        _projectWorkspaceStateWithTagHelpers = ProjectWorkspaceState.Create(
            [TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build()]);

        _projectManager = CreateProjectSnapshotManager();
    }

    [UIFact]
    public void Dispose_MakesUpdateIgnored()
    {
        // Arrange
        using var generator = new ProjectWorkspaceStateGenerator(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var generatorAccessor = generator.GetTestAccessor();
        generatorAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        generator.Dispose();

        generator.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        // Assert
        Assert.Empty(generatorAccessor.GetUpdates());
    }

    [UIFact]
    public void Update_StartsUpdateTask()
    {
        // Arrange
        using var generator = new ProjectWorkspaceStateGenerator(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var generatorAccessor = generator.GetTestAccessor();
        generatorAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        generator.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        // Assert
        var update = Assert.Single(generatorAccessor.GetUpdates());
        Assert.False(update.IsCompleted);
    }

    [UIFact]
    public void Update_SoftCancelsIncompleteTaskForSameProject()
    {
        // Arrange
        using var generator = new ProjectWorkspaceStateGenerator(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var generatorAccessor = generator.GetTestAccessor();
        generatorAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        generator.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        var initialUpdate = Assert.Single(generatorAccessor.GetUpdates());

        // Act
        generator.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        // Assert
        Assert.True(initialUpdate.IsCancellationRequested);
    }

    [UIFact]
    public async Task Update_NullWorkspaceProject_ClearsProjectWorkspaceState()
    {
        // Arrange
        using var generator = new ProjectWorkspaceStateGenerator(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var generatorAccessor = generator.GetTestAccessor();
        generatorAccessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(_projectSnapshot.HostProject);
            updater.UpdateProjectWorkspaceState(_projectSnapshot.Key, _projectWorkspaceStateWithTagHelpers);
        });

        // Act
        generator.EnqueueUpdate(workspaceProject: null, _projectSnapshot);

        // Jump off the UI thread so the background work can complete.
        await Task.Run(() => generatorAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        // Assert
        var newProjectSnapshot = _projectManager.GetRequiredProject(_projectSnapshot.Key);

        Assert.Empty(await newProjectSnapshot.GetTagHelpersAsync(DisposalToken));
    }

    [UIFact]
    public async Task Update_ResolvesTagHelpersAndUpdatesWorkspaceState()
    {
        // Arrange
        using var generator = new ProjectWorkspaceStateGenerator(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var generatorAccessor = generator.GetTestAccessor();
        generatorAccessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(_projectSnapshot.HostProject);
        });

        // Act
        generator.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        // Jump off the UI thread so the background work can complete.
        await Task.Run(() => generatorAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        // Assert
        var newProjectSnapshot = _projectManager.GetRequiredProject(_projectSnapshot.Key);

        Assert.Equal<TagHelperDescriptor>(_tagHelperResolver.TagHelpers, await newProjectSnapshot.GetTagHelpersAsync(DisposalToken));
    }
}

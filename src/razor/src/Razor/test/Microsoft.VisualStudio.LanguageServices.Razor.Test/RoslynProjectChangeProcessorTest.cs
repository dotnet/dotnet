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

public class RoslynProjectChangeProcessorTest : VisualStudioWorkspaceTestBase
{
    private readonly TestTagHelperResolver _tagHelperResolver;
    private readonly Project _workspaceProject;
    private readonly ProjectSnapshot _projectSnapshot;
    private readonly ProjectWorkspaceState _projectWorkspaceStateWithTagHelpers;
    private readonly TestProjectSnapshotManager _projectManager;

    public RoslynProjectChangeProcessorTest(ITestOutputHelper testOutput)
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
            ProjectState.Create(ProjectEngineFactoryProvider, TestProjectData.SomeProject, ProjectWorkspaceState.Default));
        _projectWorkspaceStateWithTagHelpers = ProjectWorkspaceState.Create(
            [TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build()]);

        _projectManager = CreateProjectSnapshotManager();
    }

    [UIFact]
    public void Dispose_MakesUpdateNoop()
    {
        // Arrange
        using var processor = new RoslynProjectChangeProcessor(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var processorAccessor = processor.GetTestAccessor();
        processorAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        processor.Dispose();

        processor.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        // Assert
        Assert.Empty(processorAccessor.GetUpdates());
    }

    [UIFact]
    public void Update_StartsUpdateTask()
    {
        // Arrange
        using var processor = new RoslynProjectChangeProcessor(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var processorAccessor = processor.GetTestAccessor();
        processorAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        processor.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        // Assert
        var update = Assert.Single(processorAccessor.GetUpdates());
        Assert.False(update.IsCompleted);
    }

    [UIFact]
    public void Update_SoftCancelsIncompleteTaskForSameProject()
    {
        // Arrange
        using var processor = new RoslynProjectChangeProcessor(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var processorAccessor = processor.GetTestAccessor();
        processorAccessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        processor.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        var initialUpdate = Assert.Single(processorAccessor.GetUpdates());

        // Act
        processor.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        // Assert
        Assert.True(initialUpdate.IsCancellationRequested);
    }

    [UIFact]
    public async Task Update_NullWorkspaceProject_ClearsProjectWorkspaceState()
    {
        // Arrange
        using var processor = new RoslynProjectChangeProcessor(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var processorAccessor = processor.GetTestAccessor();
        processorAccessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_projectSnapshot.HostProject);
            updater.ProjectWorkspaceStateChanged(_projectSnapshot.Key, _projectWorkspaceStateWithTagHelpers);
        });

        // Act
        processor.EnqueueUpdate(workspaceProject: null, _projectSnapshot);

        // Jump off the UI thread so the background work can complete.
        await Task.Run(() => processorAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        // Assert
        var newProjectSnapshot = _projectManager.GetLoadedProject(_projectSnapshot.Key);
        Assert.NotNull(newProjectSnapshot);
        Assert.Empty(await newProjectSnapshot.GetTagHelpersAsync(DisposalToken));
    }

    [UIFact]
    public async Task Update_ResolvesTagHelpersAndUpdatesWorkspaceState()
    {
        // Arrange
        using var processor = new RoslynProjectChangeProcessor(
            _projectManager, _tagHelperResolver, LoggerFactory, NoOpTelemetryReporter.Instance);

        var processorAccessor = processor.GetTestAccessor();
        processorAccessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_projectSnapshot.HostProject);
        });

        // Act
        processor.EnqueueUpdate(_workspaceProject, _projectSnapshot);

        // Jump off the UI thread so the background work can complete.
        await Task.Run(() => processorAccessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        // Assert
        var newProjectSnapshot = _projectManager.GetLoadedProject(_projectSnapshot.Key);
        Assert.NotNull(newProjectSnapshot);
        Assert.Equal<TagHelperDescriptor>(_tagHelperResolver.TagHelpers, await newProjectSnapshot.GetTagHelpersAsync(DisposalToken));
    }
}

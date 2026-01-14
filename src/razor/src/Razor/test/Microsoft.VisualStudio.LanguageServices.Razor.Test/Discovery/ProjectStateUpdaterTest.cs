// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.Discovery;

public class ProjectStateUpdaterTest(ITestOutputHelper testOutput) : VisualStudioWorkspaceTestBase(testOutput)
{
    private static readonly HostProject s_hostProject = TestProjectData.SomeProject;

#nullable disable
    private TestProjectSnapshotManager _projectManager;
    private TestTagHelperResolver _tagHelperResolver;
    private ProjectId _projectId;
    private ProjectWorkspaceState _projectWorkspaceState;
#nullable enable

    protected override Task InitializeAsync()
    {
        var projectInfo = s_hostProject.ToProjectInfo();
        _projectId = projectInfo.Id;

        var solution = Workspace.CurrentSolution.AddProject(projectInfo);
        Workspace.TryApplyChanges(solution);

        TagHelperCollection tagHelpers = [
            TagHelperDescriptorBuilder.CreateTagHelper("ResolvableTagHelper", "TestAssembly").Build()];
        _projectWorkspaceState = ProjectWorkspaceState.Create(tagHelpers);
        _tagHelperResolver = new TestTagHelperResolver(tagHelpers);

        _projectManager = CreateProjectSnapshotManager();

        return Task.CompletedTask;
    }

    private ProjectStateUpdater CreateProjectStateUpdater()
    {
        var updater = new ProjectStateUpdater(_projectManager, _tagHelperResolver, WorkspaceProvider, LoggerFactory, NoOpTelemetryReporter.Instance);
        AddDisposable(updater);

        return updater;
    }

    [UIFact]
    public void Dispose_MakesUpdateIgnored()
    {
        // Arrange
        var updater = CreateProjectStateUpdater();

        var accessor = updater.GetTestAccessor();
        accessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        updater.Dispose();

        updater.EnqueueUpdate(s_hostProject.Key, _projectId);

        // Assert
        Assert.Empty(accessor.GetUpdates());
    }

    [UIFact]
    public async Task Update_StartsUpdateTask()
    {
        // Arrange
        var updater = CreateProjectStateUpdater();

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        var accessor = updater.GetTestAccessor();
        accessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        // Act
        updater.EnqueueUpdate(s_hostProject.Key, _projectId);

        // Assert
        var update = Assert.Single(accessor.GetUpdates());
        Assert.False(update.IsCompleted);
    }

    [UIFact]
    public void Update_SoftCancelsIncompleteTaskForSameProject()
    {
        // Arrange
        var updater = CreateProjectStateUpdater();

        var accessor = updater.GetTestAccessor();
        accessor.BlockBackgroundWorkStart = new ManualResetEventSlim(initialState: false);

        updater.EnqueueUpdate(s_hostProject.Key, _projectId);

        var initialUpdate = Assert.Single(accessor.GetUpdates());

        // Act
        updater.EnqueueUpdate(s_hostProject.Key, _projectId);

        // Assert
        Assert.True(initialUpdate.IsCancellationRequested);
    }

    [UIFact]
    public async Task Update_NullWorkspaceProject_ClearsProjectWorkspaceState()
    {
        // Arrange
        var updater = CreateProjectStateUpdater();

        var accessor = updater.GetTestAccessor();
        accessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
            updater.UpdateProjectWorkspaceState(s_hostProject.Key, _projectWorkspaceState);
        });

        // Act
        updater.EnqueueUpdate(s_hostProject.Key, id: null);

        // Jump off the UI thread so the background work can complete.
        await Task.Run(() => accessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        // Assert
        var newProjectSnapshot = _projectManager.GetRequiredProject(s_hostProject.Key);

        Assert.Empty(await newProjectSnapshot.GetTagHelpersAsync(DisposalToken));
    }

    [UIFact]
    public async Task Update_ResolvesTagHelpersAndUpdatesWorkspaceState()
    {
        // Arrange
        var updater = CreateProjectStateUpdater();

        var accessor = updater.GetTestAccessor();
        accessor.NotifyBackgroundWorkCompleted = new ManualResetEventSlim(initialState: false);

        await _projectManager.UpdateAsync(updater =>
        {
            updater.AddProject(s_hostProject);
        });

        // Act
        updater.EnqueueUpdate(s_hostProject.Key, _projectId);

        // Jump off the UI thread so the background work can complete.
        await Task.Run(() => accessor.NotifyBackgroundWorkCompleted.Wait(TimeSpan.FromSeconds(3)));

        // Assert
        var newProjectSnapshot = _projectManager.GetRequiredProject(s_hostProject.Key);

        Assert.Equal(_tagHelperResolver.TagHelpers, await newProjectSnapshot.GetTagHelpersAsync(DisposalToken));
    }
}

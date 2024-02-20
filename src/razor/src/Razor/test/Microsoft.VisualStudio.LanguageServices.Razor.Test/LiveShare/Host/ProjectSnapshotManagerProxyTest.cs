// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LiveShare.Razor.Test;
using Microsoft.VisualStudio.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LiveShare.Razor.Host;

public class ProjectSnapshotManagerProxyTest : ProjectSnapshotManagerDispatcherTestBase
{
    private readonly IProjectSnapshot _projectSnapshot1;
    private readonly IProjectSnapshot _projectSnapshot2;

    public ProjectSnapshotManagerProxyTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var projectEngineFactoryProvider = StrictMock.Of<IProjectEngineFactoryProvider>();

        var projectWorkspaceState1 = ProjectWorkspaceState.Create(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("test1", "TestAssembly1").Build()));

        _projectSnapshot1 = new ProjectSnapshot(
            ProjectState.Create(
                projectEngineFactoryProvider,
                new HostProject("/host/path/to/project1.csproj", "/host/path/to/obj", RazorConfiguration.Default, "project1"),
                projectWorkspaceState1));

        var projectWorkspaceState2 = ProjectWorkspaceState.Create(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("test2", "TestAssembly2").Build()));

        _projectSnapshot2 = new ProjectSnapshot(
            ProjectState.Create(
                projectEngineFactoryProvider,
                new HostProject("/host/path/to/project2.csproj", "/host/path/to/obj", RazorConfiguration.Default, "project2"),
                projectWorkspaceState2));
    }

    [Fact]
    public async Task CalculateUpdatedStateAsync_ReturnsStateForAllProjects()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1, _projectSnapshot2);
        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectSnapshotManager,
            Dispatcher,
            JoinableTaskFactory);

        // Act
        var state = await JoinableTaskFactory.RunAsync(() => proxy.CalculateUpdatedStateAsync(projectSnapshotManager.GetProjects()));

        // Assert
        var project1TagHelpers = await _projectSnapshot1.GetTagHelpersAsync(CancellationToken.None);
        var project2TagHelpers = await _projectSnapshot2.GetTagHelpersAsync(CancellationToken.None);

        Assert.Collection(
            state.ProjectHandles,
            handle =>
            {
                Assert.Equal("vsls:/path/to/project1.csproj", handle.FilePath.ToString());
                Assert.Equal<TagHelperDescriptor>(project1TagHelpers, handle.ProjectWorkspaceState.TagHelpers);
            },
            handle =>
            {
                Assert.Equal("vsls:/path/to/project2.csproj", handle.FilePath.ToString());
                Assert.Equal<TagHelperDescriptor>(project2TagHelpers, handle.ProjectWorkspaceState.TagHelpers);
            });
    }

    [Fact]
    public async Task Changed_TriggersOnSnapshotManagerChanged()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1);
        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectSnapshotManager,
            Dispatcher,
            JoinableTaskFactory);
        var proxyAccessor = proxy.GetTestAccessor();
        var changedArgs = new ProjectChangeEventArgs(_projectSnapshot1, _projectSnapshot1, ProjectChangeKind.ProjectChanged);
        var called = false;
        proxy.Changed += (sender, args) =>
        {
            called = true;
            Assert.Equal($"vsls:/path/to/project1.csproj", args.ProjectFilePath.ToString());
            Assert.Equal(ProjectProxyChangeKind.ProjectChanged, args.Kind);
            Assert.NotNull(args.Newer);
            Assert.Equal("vsls:/path/to/project1.csproj", args.Newer.FilePath.ToString());
        };

        // Act
        projectSnapshotManager.TriggerChanged(changedArgs);
        await proxyAccessor.ProcessingChangedEventTestTask.AssumeNotNull().JoinAsync();

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public void Changed_NoopsIfProxyDisposed()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1);
        var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectSnapshotManager,
            Dispatcher,
            JoinableTaskFactory);
        var proxyAccessor = proxy.GetTestAccessor();
        var changedArgs = new ProjectChangeEventArgs(_projectSnapshot1, _projectSnapshot1, ProjectChangeKind.ProjectChanged);
        proxy.Changed += (sender, args) => throw new InvalidOperationException("Should not have been called.");
        proxy.Dispose();

        // Act
        projectSnapshotManager.TriggerChanged(changedArgs);

        // Assert
        Assert.Null(proxyAccessor.ProcessingChangedEventTestTask);
    }

    [Fact]
    public async Task GetLatestProjectsAsync_ReturnsSnapshotManagerProjects()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1);
        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectSnapshotManager,
            Dispatcher,
            JoinableTaskFactory);

        // Act
        var projects = await proxy.GetLatestProjectsAsync();

        // Assert
        var project = Assert.Single(projects);
        Assert.Same(_projectSnapshot1, project);
    }

    [Fact]
    public async Task GetStateAsync_ReturnsProjectState()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1, _projectSnapshot2);
        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectSnapshotManager,
            Dispatcher,
            JoinableTaskFactory);

        // Act
        var state = await JoinableTaskFactory.RunAsync(() => proxy.GetProjectManagerStateAsync(DisposalToken));

        // Assert
        var project1TagHelpers = await _projectSnapshot1.GetTagHelpersAsync(CancellationToken.None);
        var project2TagHelpers = await _projectSnapshot2.GetTagHelpersAsync(CancellationToken.None);

        Assert.Collection(
            state.ProjectHandles,
            handle =>
            {
                Assert.Equal("vsls:/path/to/project1.csproj", handle.FilePath.ToString());
                Assert.Equal<TagHelperDescriptor>(project1TagHelpers, handle.ProjectWorkspaceState.TagHelpers);
            },
            handle =>
            {
                Assert.Equal("vsls:/path/to/project2.csproj", handle.FilePath.ToString());
                Assert.Equal<TagHelperDescriptor>(project2TagHelpers, handle.ProjectWorkspaceState.TagHelpers);
            });
    }

    [Fact]
    public async Task GetStateAsync_CachesState()
    {
        // Arrange
        var projectSnapshotManager = new TestProjectSnapshotManager(_projectSnapshot1);
        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectSnapshotManager,
            Dispatcher,
            JoinableTaskFactory);

        // Act
        var state1 = await JoinableTaskFactory.RunAsync(() => proxy.GetProjectManagerStateAsync(DisposalToken));
        var state2 = await JoinableTaskFactory.RunAsync(() => proxy.GetProjectManagerStateAsync(DisposalToken));

        // Assert
        Assert.Same(state1, state2);
    }

    private sealed class TestProjectSnapshotManager(params IProjectSnapshot[] projects) : IProjectSnapshotManager
    {
        private readonly ImmutableArray<IProjectSnapshot> _projects = projects.ToImmutableArray();

        public ImmutableArray<IProjectSnapshot> GetProjects() => _projects;

        public event EventHandler<ProjectChangeEventArgs>? Changed;

        public void TriggerChanged(ProjectChangeEventArgs args)
        {
            Changed?.Invoke(this, args);
        }

        public IProjectSnapshot GetLoadedProject(ProjectKey projectKey)
            => throw new NotImplementedException();

        public ImmutableArray<ProjectKey> GetAllProjectKeys(string projectFileName)
            => throw new NotImplementedException();

        public bool IsDocumentOpen(string documentFilePath)
            => throw new NotImplementedException();

        public bool TryGetLoadedProject(ProjectKey projectKey, [NotNullWhen(true)] out IProjectSnapshot project)
            => throw new NotImplementedException();
    }
}

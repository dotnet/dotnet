// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LiveShare.Razor.Test;
using Microsoft.VisualStudio.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LiveShare.Razor.Host;

public class ProjectSnapshotManagerProxyTest(ITestOutputHelper testOutput) : VisualStudioTestBase(testOutput)
{
    private const string ProjectName1 = "project1";
    private const string ProjectName2 = "project2";
    private const string ProjectFilePath1 = $"/host/path/to/first/{ProjectName1}.csproj";
    private const string ProjectFilePath2 = $"/host/path/to/second/{ProjectName2}.csproj";
    private const string IntermediateOutputPath1 = "/host/path/to/first/obj";
    private const string IntermediateOutputPath2 = "/host/path/to/second/obj";
    private const string LspProjectFilePath1 = $"vsls:/path/to/first/{ProjectName1}.csproj";
    private const string LspProjectFilePath2 = $"vsls:/path/to/second/{ProjectName2}.csproj";

    private readonly HostProject _hostProject1 = new(ProjectFilePath1, IntermediateOutputPath1, RazorConfiguration.Default, ProjectName1);
    private readonly HostProject _hostProject2 = new(ProjectFilePath2, IntermediateOutputPath2, RazorConfiguration.Default, ProjectName2);

    private readonly ProjectWorkspaceState _projectWorkspaceState1 = ProjectWorkspaceState.Create(
        [TagHelperDescriptorBuilder.Create("test1", "TestAssembly1").Build()]);

    private readonly ProjectWorkspaceState _projectWorkspaceState2 = ProjectWorkspaceState.Create(
        [TagHelperDescriptorBuilder.Create("test2", "TestAssembly2").Build()]);

    [UIFact]
    public async Task CalculateUpdatedStateAsync_ReturnsStateForAllProjects()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectWorkspaceStateChanged(_hostProject1.Key, _projectWorkspaceState1);

            updater.ProjectAdded(_hostProject2);
            updater.ProjectWorkspaceStateChanged(_hostProject2.Key, _projectWorkspaceState2);
        });

        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectManager,
            Dispatcher,
            JoinableTaskFactory);

        // Act
        var state = await JoinableTaskFactory.RunAsync(() => proxy.CalculateUpdatedStateAsync(projectManager.GetProjects()));

        // Assert
        var project1TagHelpers = await projectManager.GetLoadedProject(_hostProject1.Key).GetTagHelpersAsync(DisposalToken);
        var project2TagHelpers = await projectManager.GetLoadedProject(_hostProject2.Key).GetTagHelpersAsync(DisposalToken);

        Assert.Collection(
            state.ProjectHandles,
            AssertProjectSnapshotHandle(LspProjectFilePath1, project1TagHelpers),
            AssertProjectSnapshotHandle(LspProjectFilePath2, project2TagHelpers));
    }

    [UIFact]
    public async Task Changed_TriggersOnSnapshotManagerChanged()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectWorkspaceStateChanged(_hostProject1.Key, _projectWorkspaceState1);
        });

        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectManager,
            Dispatcher,
            JoinableTaskFactory);

        var proxyAccessor = proxy.GetTestAccessor();

        var called = false;
        proxy.Changed += (sender, args) =>
        {
            called = true;
            Assert.Equal(LspProjectFilePath1, args.ProjectFilePath.ToString());
            Assert.Equal(ProjectProxyChangeKind.ProjectChanged, args.Kind);
            Assert.NotNull(args.Newer);
            Assert.Equal(LspProjectFilePath1, args.Newer.FilePath.ToString());
        };

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            // Change the project's configuration to force a changed event to be raised.
            var project = updater.GetLoadedProject(_hostProject1.Key);
            updater.ProjectConfigurationChanged(new(
                project.FilePath,
                project.IntermediateOutputPath,
                FallbackRazorConfiguration.MVC_1_0,
                project.RootNamespace,
                project.DisplayName));
        });

        await proxyAccessor.ProcessingChangedEventTestTask.AssumeNotNull().JoinAsync();

        // Assert
        Assert.True(called);
    }

    [UIFact]
    public async Task Changed_DoesNotFireIfProxyIsDisposed()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectWorkspaceStateChanged(_hostProject1.Key, _projectWorkspaceState1);
        });

        var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectManager,
            Dispatcher,
            JoinableTaskFactory);

        var proxyAccessor = proxy.GetTestAccessor();

        proxy.Changed += (sender, args) => throw new InvalidOperationException("Should not have been called.");
        proxy.Dispose();

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            // Change the project's configuration to force a changed event to be raised.
            var project = updater.GetLoadedProject(_hostProject1.Key);
            updater.ProjectConfigurationChanged(new(
                project.FilePath,
                project.IntermediateOutputPath,
                FallbackRazorConfiguration.MVC_1_0,
                project.RootNamespace,
                project.DisplayName));
        });

        // Assert
        Assert.Null(proxyAccessor.ProcessingChangedEventTestTask);
    }

    [UIFact]
    public async Task GetLatestProjectsAsync_ReturnsSnapshotManagerProjects()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectWorkspaceStateChanged(_hostProject1.Key, _projectWorkspaceState1);
        });

        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectManager,
            Dispatcher,
            JoinableTaskFactory);

        // Act
        var projects = await proxy.GetLatestProjectsAsync();

        // Assert
        var project = Assert.Single(projects);
        Assert.NotNull(project);
        Assert.Equal(_hostProject1.Key, project.Key);
        Assert.Equal(_hostProject1.FilePath, project.FilePath);
        Assert.Equal(_hostProject1.IntermediateOutputPath, project.IntermediateOutputPath);
        Assert.Equal(_hostProject1.Configuration, project.Configuration);
        Assert.Equal(_hostProject1.RootNamespace, project.RootNamespace);
        Assert.Equal(_hostProject1.DisplayName, project.DisplayName);
        Assert.Equal(_projectWorkspaceState1, project.ProjectWorkspaceState);
    }

    [UIFact]
    public async Task GetStateAsync_ReturnsProjectState()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectWorkspaceStateChanged(_hostProject1.Key, _projectWorkspaceState1);

            updater.ProjectAdded(_hostProject2);
            updater.ProjectWorkspaceStateChanged(_hostProject2.Key, _projectWorkspaceState2);
        });

        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectManager,
            Dispatcher,
            JoinableTaskFactory);

        // Act
        var state = await JoinableTaskFactory.RunAsync(() => proxy.GetProjectManagerStateAsync(DisposalToken));

        // Assert
        var project1TagHelpers = await projectManager.GetLoadedProject(_hostProject1.Key).GetTagHelpersAsync(DisposalToken);
        var project2TagHelpers = await projectManager.GetLoadedProject(_hostProject2.Key).GetTagHelpersAsync(DisposalToken);

        Assert.Collection(
            state.ProjectHandles,
            AssertProjectSnapshotHandle(LspProjectFilePath1, project1TagHelpers),
            AssertProjectSnapshotHandle(LspProjectFilePath2, project2TagHelpers));
    }

    [UIFact]
    public async Task GetStateAsync_CachesState()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(_hostProject1);
            updater.ProjectWorkspaceStateChanged(_hostProject1.Key, _projectWorkspaceState1);
        });

        using var proxy = new ProjectSnapshotManagerProxy(
            new TestCollaborationSession(true),
            projectManager,
            Dispatcher,
            JoinableTaskFactory);

        // Act
        var state1 = await JoinableTaskFactory.RunAsync(() => proxy.GetProjectManagerStateAsync(DisposalToken));
        var state2 = await JoinableTaskFactory.RunAsync(() => proxy.GetProjectManagerStateAsync(DisposalToken));

        // Assert
        Assert.Same(state1, state2);
    }

    private static Action<ProjectSnapshotHandleProxy> AssertProjectSnapshotHandle(
        string expectedFilePath,
        ImmutableArray<TagHelperDescriptor> expectedTagHelpers)
        => handle =>
        {
            Assert.Equal(expectedFilePath, handle.FilePath.ToString());
            Assert.Equal<TagHelperDescriptor>(expectedTagHelpers, handle.ProjectWorkspaceState.TagHelpers);
        };
}

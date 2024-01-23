// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServices.Razor.Test;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

public class VsSolutionUpdatesProjectSnapshotChangeTriggerTest : ToolingTestBase
{
    private static readonly ProjectSnapshotManagerDispatcher s_dispatcher = new TestDispatcher();

    private readonly HostProject _someProject;
    private readonly HostProject _someOtherProject;
    private Project _someWorkspaceProject;
    private readonly CodeAnalysis.Workspace _workspace;

    public VsSolutionUpdatesProjectSnapshotChangeTriggerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _someProject = new HostProject(TestProjectData.SomeProject.FilePath, TestProjectData.SomeProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_1_0, TestProjectData.SomeProject.RootNamespace);
        _someOtherProject = new HostProject(TestProjectData.AnotherProject.FilePath, TestProjectData.AnotherProject.IntermediateOutputPath, FallbackRazorConfiguration.MVC_2_0, TestProjectData.AnotherProject.RootNamespace);

        var testServices = TestServices.Create([], []);

        _workspace = TestWorkspace.Create(testServices,
            w =>
            {
                _someWorkspaceProject = w.AddProject(
                    ProjectInfo.Create(
                        ProjectId.CreateNewId(),
                        VersionStamp.Create(),
                        "SomeProject",
                        "SomeProject",
                        LanguageNames.CSharp,
                        filePath: _someProject.FilePath)
                    .WithCompilationOutputInfo(
                        new CompilationOutputInfo()
                            .WithAssemblyPath(Path.Combine(_someProject.IntermediateOutputPath, "SomeProject.dll"))));
            });
        AddDisposable(_workspace);
    }

    [Fact]
    public void Initialize_AttachesEventSink()
    {
        // Arrange
        uint cookie;
        var buildManager = new Mock<IVsSolutionBuildManager>(MockBehavior.Strict);
        buildManager
            .Setup(b => b.AdviseUpdateSolutionEvents(It.IsAny<VsSolutionUpdatesProjectSnapshotChangeTrigger>(), out cookie))
            .Returns(VSConstants.S_OK)
            .Verifiable();

        var services = new Mock<IServiceProvider>(MockBehavior.Strict);
        services.Setup(s => s.GetService(It.Is<Type>(f => f == typeof(SVsSolutionBuildManager)))).Returns(buildManager.Object);

        var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(
            services.Object,
            Mock.Of<TextBufferProjectService>(MockBehavior.Strict),
            Mock.Of<IProjectWorkspaceStateGenerator>(MockBehavior.Strict),
            s_dispatcher,
            JoinableTaskFactory.Context);

        // Act
        trigger.Initialize(Mock.Of<ProjectSnapshotManagerBase>(MockBehavior.Strict));

        // Assert
        buildManager.Verify();
    }

    [Fact]
    public async Task Initialize_SwitchesToMainThread()
    {
        // Arrange
        uint cookie;
        var buildManager = new Mock<IVsSolutionBuildManager>(MockBehavior.Strict);
        buildManager
            .Setup(b => b.AdviseUpdateSolutionEvents(It.IsAny<VsSolutionUpdatesProjectSnapshotChangeTrigger>(), out cookie))
            .Returns(VSConstants.S_OK);

        var context = JoinableTaskFactory.Context;

        var services = new Mock<IServiceProvider>(MockBehavior.Strict);
        services.Setup(s => s.GetService(It.Is<Type>(f => f == typeof(SVsSolutionBuildManager)))).Callback(() => Assert.True(context.IsOnMainThread)).Returns(buildManager.Object);

        var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(
            services.Object,
            Mock.Of<TextBufferProjectService>(MockBehavior.Strict),
            Mock.Of<IProjectWorkspaceStateGenerator>(MockBehavior.Strict),
            s_dispatcher,
            context);

        await Task.Run(() =>
        {
            Assert.False(context.IsOnMainThread);
            trigger.Initialize(Mock.Of<ProjectSnapshotManagerBase>(MockBehavior.Strict));
            return Task.CompletedTask;
        });
    }

    [Fact]
    public async Task SolutionClosing_CancelsActiveWork()
    {
        // Arrange
        var projectManager = new TestProjectSnapshotManager(_workspace, ProjectEngineFactories.DefaultProvider, s_dispatcher)
        {
            AllowNotifyListeners = true,
        };
        var expectedProjectPath = _someProject.FilePath;
        var expectedProjectSnapshot = await s_dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.ProjectAdded(_someProject);
            projectManager.ProjectAdded(_someOtherProject);

            return projectManager.GetLoadedProject(_someProject.Key);
        }, DisposalToken);

        var projectService = new Mock<TextBufferProjectService>(MockBehavior.Strict);
        projectService.Setup(p => p.GetProjectPath(It.IsAny<IVsHierarchy>())).Returns(expectedProjectPath);
        var workspaceStateGenerator = new TestProjectWorkspaceStateGenerator();

        var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(TestServiceProvider.Instance, projectService.Object, workspaceStateGenerator, s_dispatcher, JoinableTaskFactory.Context);
        trigger.Initialize(projectManager);
        trigger.UpdateProjectCfg_Done(Mock.Of<IVsHierarchy>(MockBehavior.Strict), pCfgProj: default, pCfgSln: default, dwAction: default, fSuccess: default, fCancel: default);
        await trigger.CurrentUpdateTaskForTests;

        // Act
        await s_dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.SolutionClosed();
            projectManager.ProjectRemoved(_someProject.Key);
        }, DisposalToken);

        // Assert
        var update = Assert.Single(workspaceStateGenerator.UpdateQueue);
        Assert.Equal(update.WorkspaceProject.Id, _someWorkspaceProject.Id);
        Assert.Same(expectedProjectSnapshot, update.ProjectSnapshot);
        Assert.True(update.CancellationToken.IsCancellationRequested);
    }

    [Fact]
    public async Task OnProjectBuiltAsync_KnownProject_EnqueuesProjectStateUpdate()
    {
        // Arrange
        var projectManager = new TestProjectSnapshotManager(_workspace, ProjectEngineFactories.DefaultProvider, s_dispatcher);
        var expectedProjectPath = _someProject.FilePath;
        var expectedProjectSnapshot = await s_dispatcher.RunOnDispatcherThreadAsync(() =>
        {
            projectManager.ProjectAdded(_someProject);
            projectManager.ProjectAdded(_someOtherProject);

            return projectManager.GetLoadedProject(_someProject.Key);
        }, DisposalToken);

        var projectService = new Mock<TextBufferProjectService>(MockBehavior.Strict);
        projectService.Setup(p => p.GetProjectPath(It.IsAny<IVsHierarchy>())).Returns(expectedProjectPath);
        var workspaceStateGenerator = new TestProjectWorkspaceStateGenerator();

        var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(TestServiceProvider.Instance, projectService.Object, workspaceStateGenerator, s_dispatcher, JoinableTaskFactory.Context);
        trigger.Initialize(projectManager);

        // Act
        await trigger.OnProjectBuiltAsync(Mock.Of<IVsHierarchy>(MockBehavior.Strict), DisposalToken);

        // Assert
        var update = Assert.Single(workspaceStateGenerator.UpdateQueue);
        Assert.Equal(update.WorkspaceProject.Id, _someWorkspaceProject.Id);
        Assert.Same(expectedProjectSnapshot, update.ProjectSnapshot);
    }

    [Fact]
    public async Task OnProjectBuiltAsync_WithoutWorkspaceProject_DoesNotEnqueueUpdate()
    {
        // Arrange
        uint cookie;
        var buildManager = new Mock<IVsSolutionBuildManager>(MockBehavior.Strict);
        buildManager
            .Setup(b => b.AdviseUpdateSolutionEvents(It.IsAny<VsSolutionUpdatesProjectSnapshotChangeTrigger>(), out cookie))
            .Returns(VSConstants.S_OK);

        var services = new Mock<IServiceProvider>(MockBehavior.Strict);
        services
            .Setup(s => s.GetService(It.Is<Type>(f => f == typeof(SVsSolutionBuildManager))))
            .Returns(buildManager.Object);

        var projectEngineFactoryProvider = Mock.Of<IProjectEngineFactoryProvider>(MockBehavior.Strict);

        var projectSnapshot = new ProjectSnapshot(
            ProjectState.Create(
                projectEngineFactoryProvider,
                new HostProject("/Some/Unknown/Path.csproj", "/Some/Unknown/obj", RazorConfiguration.Default, "Path"),
                ProjectWorkspaceState.Default));
        var expectedProjectPath = projectSnapshot.FilePath;

        var projectService = new Mock<TextBufferProjectService>(MockBehavior.Strict);
        projectService.Setup(p => p.GetProjectPath(It.IsAny<IVsHierarchy>())).Returns(expectedProjectPath);

        var projectManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectManager.SetupGet(p => p.Workspace).Returns(_workspace);
        projectManager
            .Setup(p => p.GetAllProjectKeys(projectSnapshot.FilePath))
            .Returns(ImmutableArray.Create(projectSnapshot.Key));
        projectManager
            .Setup(p => p.GetLoadedProject(projectSnapshot.Key))
            .Returns(projectSnapshot);
        var workspaceStateGenerator = new TestProjectWorkspaceStateGenerator();

        var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(services.Object, projectService.Object, workspaceStateGenerator, s_dispatcher, JoinableTaskFactory.Context);
        trigger.Initialize(projectManager.Object);

        // Act
        await trigger.OnProjectBuiltAsync(Mock.Of<IVsHierarchy>(MockBehavior.Strict), DisposalToken);

        // Assert
        Assert.Empty(workspaceStateGenerator.UpdateQueue);
    }

    [Fact]
    public async Task OnProjectBuiltAsync_UnknownProject_DoesNotEnqueueUpdate()
    {
        // Arrange
        var expectedProjectPath = "Path/To/Project/proj.csproj";

        uint cookie;
        var buildManager = new Mock<IVsSolutionBuildManager>(MockBehavior.Strict);
        buildManager
            .Setup(b => b.AdviseUpdateSolutionEvents(It.IsAny<VsSolutionUpdatesProjectSnapshotChangeTrigger>(), out cookie))
            .Returns(VSConstants.S_OK);

        var services = new Mock<IServiceProvider>(MockBehavior.Strict);
        services.Setup(s => s.GetService(It.Is<Type>(f => f == typeof(SVsSolutionBuildManager)))).Returns(buildManager.Object);

        var projectService = new Mock<TextBufferProjectService>(MockBehavior.Strict);
        projectService.Setup(p => p.GetProjectPath(It.IsAny<IVsHierarchy>())).Returns(expectedProjectPath);

        var projectManager = new Mock<ProjectSnapshotManagerBase>(MockBehavior.Strict);
        projectManager.SetupGet(p => p.Workspace).Returns(_workspace);
        projectManager
            .Setup(p => p.GetAllProjectKeys(expectedProjectPath))
            .Returns(ImmutableArray<ProjectKey>.Empty);
        var workspaceStateGenerator = new TestProjectWorkspaceStateGenerator();

        var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(services.Object, projectService.Object, workspaceStateGenerator, s_dispatcher, JoinableTaskFactory.Context);
        trigger.Initialize(projectManager.Object);

        // Act
        await trigger.OnProjectBuiltAsync(Mock.Of<IVsHierarchy>(MockBehavior.Strict), DisposalToken);

        // Assert
        Assert.Empty(workspaceStateGenerator.UpdateQueue);
    }

    private class TestServiceProvider : IServiceProvider
    {
        public static readonly TestServiceProvider Instance = new TestServiceProvider();

        private TestServiceProvider()
        {
        }

        public object GetService(Type serviceType)
        {
            return null;
        }
    }

    private class TestDispatcher : ProjectSnapshotManagerDispatcher
    {
        public override bool IsDispatcherThread => true;

        public override TaskScheduler DispatcherScheduler => TaskScheduler.Default;
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Razor.ProjectSystem;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor;

public class VsSolutionUpdatesProjectSnapshotChangeTriggerTest : VisualStudioTestBase
{
    private static readonly HostProject s_someProject = TestProjectData.SomeProject with
    {
        Configuration = FallbackRazorConfiguration.MVC_1_0
    };

    private static readonly HostProject s_someOtherProject = TestProjectData.AnotherProject with
    {
        Configuration = FallbackRazorConfiguration.MVC_2_0
    };

    private readonly Project _someWorkspaceProject;
    private readonly IWorkspaceProvider _workspaceProvider;

    public VsSolutionUpdatesProjectSnapshotChangeTriggerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        Project? someWorkspaceProject = null;
        var workspace = TestWorkspace.Create(
            w =>
            {
                someWorkspaceProject = w.AddProject(
                    ProjectInfo.Create(
                        ProjectId.CreateNewId(),
                        VersionStamp.Create(),
                        "SomeProject",
                        "SomeProject",
                        LanguageNames.CSharp,
                        filePath: s_someProject.FilePath)
                    .WithCompilationOutputInfo(
                        new CompilationOutputInfo()
                            .WithAssemblyPath(Path.Combine(s_someProject.IntermediateOutputPath, "SomeProject.dll"))));
            });
        _someWorkspaceProject = someWorkspaceProject.AssumeNotNull();

        AddDisposable(workspace);

        var workspaceProviderMock = new Mock<IWorkspaceProvider>(MockBehavior.Strict);
        workspaceProviderMock
            .Setup(x => x.GetWorkspace())
            .Returns(workspace);

        _workspaceProvider = workspaceProviderMock.Object;
    }

    [UIFact]
    public async Task Initialize_AttachesEventSink()
    {
        uint cookie = 42;
        var buildManagerMock = new StrictMock<IVsSolutionBuildManager>();
        buildManagerMock
            .Setup(b => b.AdviseUpdateSolutionEvents(It.IsAny<VsSolutionUpdatesProjectSnapshotChangeTrigger>(), out cookie))
            .Returns(VSConstants.S_OK)
            .Verifiable();
        buildManagerMock
            .Setup(b => b.UnadviseUpdateSolutionEvents(cookie))
            .Returns(VSConstants.S_OK)
            .Verifiable();

        var serviceProvider = VsMocks.CreateServiceProvider(b =>
            b.AddService<SVsSolutionBuildManager>(buildManagerMock.Object));
        var projectManager = CreateProjectSnapshotManager();

        // Note: We're careful to use a using statement with a block to allow
        // the call to UnadviseUpdateSolutionEvents() to be verified after disposal.
        using (var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(
            serviceProvider,
            projectManager,
            StrictMock.Of<IProjectWorkspaceStateGenerator>(),
            _workspaceProvider,
            JoinableTaskContext))
        {
            var testAccessor = trigger.GetTestAccessor();

            await testAccessor.InitializeTask;
        }

        buildManagerMock.Verify();
    }

    [UIFact]
    public async Task Initialize_SwitchesToMainThread()
    {
        uint cookie = 42;
        var buildManagerMock = new StrictMock<IVsSolutionBuildManager>();
        buildManagerMock
            .Setup(b => b.AdviseUpdateSolutionEvents(It.IsAny<VsSolutionUpdatesProjectSnapshotChangeTrigger>(), out cookie))
            .Returns(VSConstants.S_OK)
            // When IVsSolutionBuildManager.AdviseUpdateSolutionEvents is called, we should have switched to the main thread.
            .Callback(() => Assert.True(JoinableTaskContext.IsOnMainThread));
        buildManagerMock
            .Setup(b => b.UnadviseUpdateSolutionEvents(cookie))
            .Returns(VSConstants.S_OK);

        var serviceProvider = VsMocks.CreateServiceProvider(b =>
            b.AddService<SVsSolutionBuildManager>(() =>
            {
                // When the IVsSolutionBuildManager service is retrieved, we should have switched to the main thread.
                Assert.True(JoinableTaskContext.IsOnMainThread);
                return buildManagerMock.Object;
            }));

        var projectManager = CreateProjectSnapshotManager();

        using var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(
            serviceProvider,
            projectManager,
            StrictMock.Of<IProjectWorkspaceStateGenerator>(),
            _workspaceProvider,
            JoinableTaskContext);

        var testAccessor = trigger.GetTestAccessor();

        await Task.Run(async () =>
        {
            Assert.False(JoinableTaskContext.IsOnMainThread);

            await testAccessor.InitializeTask;
        });
    }

    [UIFact]
    public async Task SolutionClosing_CancelsActiveWork()
    {
        var projectManager = CreateProjectSnapshotManager();

        var expectedProjectPath = s_someProject.FilePath;

        var expectedProjectSnapshot = await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_someProject);
            updater.ProjectAdded(s_someOtherProject);

            return updater.GetRequiredProject(s_someProject.Key);
        });

        var serviceProvider = VsMocks.CreateServiceProvider();
        var workspaceStateGenerator = new TestProjectWorkspaceStateGenerator();

        var vsHierarchyMock = new StrictMock<IVsHierarchy>();
        var vsProjectMock = vsHierarchyMock.As<IVsProject>();
        vsProjectMock
            .Setup(x => x.GetMkDocument((uint)VSConstants.VSITEMID.Root, out expectedProjectPath))
            .Returns(VSConstants.S_OK);

        using var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(
            serviceProvider,
            projectManager,
            workspaceStateGenerator,
            _workspaceProvider,
            JoinableTaskContext);

        var testAccessor = trigger.GetTestAccessor();

        trigger.UpdateProjectCfg_Done(vsHierarchyMock.Object, pCfgProj: null!, pCfgSln: null!, dwAction: 0, fSuccess: 0, fCancel: 0);

        // UpdateProjectCfg_Done will call OnProjectBuiltAsync(). The test accessor
        // provides a task we can wait on.
        await testAccessor.OnProjectBuiltTask.AssumeNotNull();

        await projectManager.UpdateAsync(updater =>
        {
            updater.SolutionClosed();
            updater.ProjectRemoved(s_someProject.Key);
        });

        var update = Assert.Single(workspaceStateGenerator.Updates);
        Assert.NotNull(update.WorkspaceProject);
        Assert.Equal(update.WorkspaceProject.Id, _someWorkspaceProject.Id);
        Assert.Same(expectedProjectSnapshot, update.ProjectSnapshot);
        Assert.True(update.IsCancelled);
    }

    [UIFact]
    public async Task OnProjectBuiltAsync_KnownProject_EnqueuesProjectStateUpdate()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var expectedProjectPath = s_someProject.FilePath;

        var expectedProjectSnapshot = await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(s_someProject);
            updater.ProjectAdded(s_someOtherProject);

            return updater.GetRequiredProject(s_someProject.Key);
        });

        var serviceProvider = VsMocks.CreateServiceProvider();
        var workspaceStateGenerator = new TestProjectWorkspaceStateGenerator();

        using var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(
            serviceProvider,
            projectManager,
            workspaceStateGenerator,
            _workspaceProvider,
            JoinableTaskContext);

        var vsHierarchyMock = new StrictMock<IVsHierarchy>();
        var vsProjectMock = vsHierarchyMock.As<IVsProject>();
        vsProjectMock
            .Setup(x => x.GetMkDocument((uint)VSConstants.VSITEMID.Root, out expectedProjectPath))
            .Returns(VSConstants.S_OK);

        var testAccessor = trigger.GetTestAccessor();

        // Act
        await testAccessor.OnProjectBuiltAsync(vsHierarchyMock.Object, DisposalToken);

        // Assert
        var update = Assert.Single(workspaceStateGenerator.Updates);
        Assert.NotNull(update.WorkspaceProject);
        Assert.Equal(update.WorkspaceProject.Id, _someWorkspaceProject.Id);
        Assert.Same(expectedProjectSnapshot, update.ProjectSnapshot);
    }

    [UIFact]
    public async Task OnProjectBuiltAsync_WithoutWorkspaceProject_DoesNotEnqueueUpdate()
    {
        // Arrange
        uint cookie = 42;
        var buildManagerMock = new StrictMock<IVsSolutionBuildManager>();
        buildManagerMock
            .Setup(b => b.AdviseUpdateSolutionEvents(It.IsAny<VsSolutionUpdatesProjectSnapshotChangeTrigger>(), out cookie))
            .Returns(VSConstants.S_OK);
        buildManagerMock
            .Setup(b => b.UnadviseUpdateSolutionEvents(cookie))
            .Returns(VSConstants.S_OK);

        var serviceProvider = VsMocks.CreateServiceProvider(b =>
            b.AddService<SVsSolutionBuildManager>(buildManagerMock.Object));

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(
                new HostProject("/Some/Unknown/Path.csproj", "/Some/Unknown/obj", RazorConfiguration.Default, "Path"));
        });

        var workspaceStateGenerator = new TestProjectWorkspaceStateGenerator();

        using var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(
            serviceProvider,
            projectManager,
            workspaceStateGenerator,
            _workspaceProvider,
            JoinableTaskContext);

        var testAccessor = trigger.GetTestAccessor();

        // Act
        await testAccessor.OnProjectBuiltAsync(StrictMock.Of<IVsHierarchy>(), DisposalToken);

        // Assert
        Assert.Empty(workspaceStateGenerator.Updates);
    }

    [UIFact]
    public async Task OnProjectBuiltAsync_UnknownProject_DoesNotEnqueueUpdate()
    {
        // Arrange
        uint cookie = 42;
        var buildManagerMock = new StrictMock<IVsSolutionBuildManager>();
        buildManagerMock
            .Setup(b => b.AdviseUpdateSolutionEvents(It.IsAny<VsSolutionUpdatesProjectSnapshotChangeTrigger>(), out cookie))
            .Returns(VSConstants.S_OK);
        buildManagerMock
            .Setup(b => b.UnadviseUpdateSolutionEvents(cookie))
            .Returns(VSConstants.S_OK);

        var serviceProvider = VsMocks.CreateServiceProvider(b =>
            b.AddService<SVsSolutionBuildManager>(buildManagerMock.Object));

        var projectManager = CreateProjectSnapshotManager();

        var workspaceStateGenerator = new TestProjectWorkspaceStateGenerator();

        using var trigger = new VsSolutionUpdatesProjectSnapshotChangeTrigger(
            serviceProvider,
            projectManager,
            workspaceStateGenerator,
            _workspaceProvider,
            JoinableTaskContext);

        var testAccessor = trigger.GetTestAccessor();

        // Act
        await testAccessor.OnProjectBuiltAsync(StrictMock.Of<IVsHierarchy>(), DisposalToken);

        // Assert
        Assert.Empty(workspaceStateGenerator.Updates);
    }
}

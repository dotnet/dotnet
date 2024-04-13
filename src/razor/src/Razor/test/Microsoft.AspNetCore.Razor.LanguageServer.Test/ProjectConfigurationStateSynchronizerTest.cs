// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.CSharp;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class ProjectConfigurationStateSynchronizerTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    public async Task ProjectConfigurationFileChanged_Removed_UnknownDocumentNoops()
    {
        // Arrange
        var projectService = new Mock<IRazorProjectService>(MockBehavior.Strict);
        var synchronizer = GetSynchronizer(projectService.Object);
        var deserializerMock = new Mock<IRazorProjectInfoDeserializer>(MockBehavior.Strict);
        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/project.razor.bin",
            kind: RazorFileChangeKind.Removed,
            projectInfoDeserializer: deserializerMock.Object);

        // Act
        await Dispatcher.RunAsync(
            () => synchronizer.ProjectConfigurationFileChanged(args), DisposalToken);

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Removed_NonNormalizedPaths()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            displayName: "project",
            ProjectWorkspaceState.Create(LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var intermediateOutputPath = Path.GetDirectoryName(FilePathNormalizer.Normalize(projectInfo.SerializedFilePath));
        var projectKey = TestProjectKey.Create(intermediateOutputPath);
        var projectService = new StrictMock<IRazorProjectService>();
        projectService
            .Setup(x => x.AddProjectAsync(
                projectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectService
            .Setup(x => x.UpdateProjectAsync(
                projectKey,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                projectInfo.ProjectWorkspaceState,
                projectInfo.Documents,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        projectService
            .Setup(x => x.UpdateProjectAsync(
                projectKey,
                null,
                null,
                "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty,
                CancellationToken.None))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var jsonFileDeserializer = CreateDeserializer(projectInfo);
        var addArgs = new ProjectConfigurationFileChangeEventArgs("/path/to\\obj/project.razor.bin", RazorFileChangeKind.Added, jsonFileDeserializer);
        var enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        var removeArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Removed,
            projectInfoDeserializer: Mock.Of<IRazorProjectInfoDeserializer>(MockBehavior.Strict));

        // Act
        enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(removeArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Added_CantDeserialize_Noops()
    {
        // Arrange
        var projectService = new Mock<IRazorProjectService>(MockBehavior.Strict);
        var synchronizer = GetSynchronizer(projectService.Object);

        var deserializerMock = new Mock<IRazorProjectInfoDeserializer>(MockBehavior.Strict);
        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns((RazorProjectInfo)null);

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            projectInfoDeserializer: deserializerMock.Object);

        // Act
        await Dispatcher.RunAsync(
            () => synchronizer.ProjectConfigurationFileChanged(args), DisposalToken);

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Added_AddAndUpdatesProject()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            displayName: "project",
            ProjectWorkspaceState.Create(LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var intermediateOutputPath = Path.GetDirectoryName(FilePathNormalizer.Normalize(projectInfo.SerializedFilePath));
        var projectKey = TestProjectKey.Create(intermediateOutputPath);
        var projectService = new StrictMock<IRazorProjectService>();
        projectService
            .Setup(service => service.AddProjectAsync(
                projectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectService
            .Setup(service => service.UpdateProjectAsync(
                projectKey,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                projectInfo.ProjectWorkspaceState,
                projectInfo.Documents, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var jsonFileDeserializer = CreateDeserializer(projectInfo);
        var args = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.bin", RazorFileChangeKind.Added, jsonFileDeserializer);

        // Act
        var enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(args);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Removed_ResetsProject()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            displayName: "project",
            ProjectWorkspaceState.Create(LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var intermediateOutputPath = Path.GetDirectoryName(FilePathNormalizer.Normalize(projectInfo.SerializedFilePath));
        var projectKey = TestProjectKey.Create(intermediateOutputPath);
        var projectService = new StrictMock<IRazorProjectService>();
        projectService
            .Setup(service => service.AddProjectAsync(
                projectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectService
            .Setup(service => service.UpdateProjectAsync(
                projectKey,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                projectInfo.ProjectWorkspaceState,
                projectInfo.Documents,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        projectService
            .Setup(service => service.UpdateProjectAsync(
                projectKey,
                null,
                null,
                "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty,
                CancellationToken.None))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var deserializer = CreateDeserializer(projectInfo);
        var addArgs = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.bin", RazorFileChangeKind.Added, deserializer);
        var enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        var removeArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Removed,
            projectInfoDeserializer: Mock.Of<IRazorProjectInfoDeserializer>(MockBehavior.Strict));

        // Act
        enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(removeArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Changed_UpdatesProject()
    {
        // Arrange
        var initialProjectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            displayName: "project",
            ProjectWorkspaceState.Create(LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var intermediateOutputPath = Path.GetDirectoryName(FilePathNormalizer.Normalize(initialProjectInfo.SerializedFilePath));
        var projectKey = TestProjectKey.Create(intermediateOutputPath);
        var projectService = new StrictMock<IRazorProjectService>();
        projectService
            .Setup(service => service.AddProjectAsync(
                initialProjectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                initialProjectInfo.RootNamespace,
                initialProjectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectService
            .Setup(service => service.UpdateProjectAsync(
                projectKey,
                It.IsAny<RazorConfiguration>(),
                initialProjectInfo.RootNamespace,
                initialProjectInfo.DisplayName,
                initialProjectInfo.ProjectWorkspaceState,
                initialProjectInfo.Documents,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var changedProjectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "path/to/project.csproj",
            new(RazorLanguageVersion.Experimental,
                "TestConfiguration",
                Extensions: []),
            rootNamespace: "TestRootNamespace2",
            displayName: "project",
            ProjectWorkspaceState.Create(LanguageVersion.CSharp6),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        projectService
            .Setup(service => service.UpdateProjectAsync(
                projectKey,
                It.IsAny<RazorConfiguration>(),
                changedProjectInfo.RootNamespace,
                changedProjectInfo.DisplayName,
                changedProjectInfo.ProjectWorkspaceState,
                changedProjectInfo.Documents,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var addDeserializer = CreateDeserializer(initialProjectInfo);
        var addArgs = new ProjectConfigurationFileChangeEventArgs(
            "/path/to/obj/project.razor.bin",
            RazorFileChangeKind.Added,
            addDeserializer);

        var enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        var changedDeserializer = CreateDeserializer(changedProjectInfo);
        var changedArgs = new ProjectConfigurationFileChangeEventArgs(
            "/path/to/obj/project.razor.bin",
            RazorFileChangeKind.Changed,
            changedDeserializer);

        // Act
        enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(changedArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Changed_CantDeserialize_ResetsProject()
    {
        // Arrange
        var initialProjectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            displayName: "project",
            ProjectWorkspaceState.Create(LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var intermediateOutputPath = Path.GetDirectoryName(FilePathNormalizer.Normalize(initialProjectInfo.SerializedFilePath));
        var projectKey = TestProjectKey.Create(intermediateOutputPath);
        var projectService = new StrictMock<IRazorProjectService>();
        projectService
            .Setup(service => service.AddProjectAsync(
                initialProjectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                initialProjectInfo.RootNamespace,
                initialProjectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectService
            .Setup(service => service.UpdateProjectAsync(
                projectKey,
                It.IsAny<RazorConfiguration>(),
                initialProjectInfo.RootNamespace,
                initialProjectInfo.DisplayName,
                initialProjectInfo.ProjectWorkspaceState,
                initialProjectInfo.Documents,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var changedProjectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "path/to/project.csproj",
            new(RazorLanguageVersion.Experimental,
                "TestConfiguration",
                Extensions: []),
            rootNamespace: "TestRootNamespace2",
            displayName: "project",
            ProjectWorkspaceState.Create(LanguageVersion.CSharp6),
            ImmutableArray<DocumentSnapshotHandle>.Empty);

        // This is the request that happens when the server is reset
        projectService
            .Setup(service => service.UpdateProjectAsync(
                projectKey,
                null,
                null,
                "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty,
                CancellationToken.None))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var addDeserializer = CreateDeserializer(initialProjectInfo);
        var addArgs = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.bin", RazorFileChangeKind.Added, addDeserializer);
        var enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        var changedDeserializerMock = new Mock<IRazorProjectInfoDeserializer>(MockBehavior.Strict);
        changedDeserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns((RazorProjectInfo)null);

        var changedArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Changed,
            projectInfoDeserializer: changedDeserializerMock.Object);

        // Act
        enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(changedArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Changed_UntrackedProject_Noops()
    {
        // Arrange
        var projectService = new Mock<IRazorProjectService>(MockBehavior.Strict);
        var synchronizer = GetSynchronizer(projectService.Object);

        var changedDeserializerMock = new Mock<IRazorProjectInfoDeserializer>(MockBehavior.Strict);
        changedDeserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns((RazorProjectInfo)null);

        var changedArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/project.razor.bin",
            kind: RazorFileChangeKind.Changed,
            projectInfoDeserializer: changedDeserializerMock.Object);

        // Act
        var enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(changedArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer, hasTask: false);
        }, DisposalToken);
        await enqueueTask;

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_RemoveThenAdd_OnlyAdds()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.json",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            displayName: "project",
            ProjectWorkspaceState.Create(LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var intermediateOutputPath = Path.GetDirectoryName(FilePathNormalizer.Normalize(projectInfo.SerializedFilePath));
        var projectKey = TestProjectKey.Create(intermediateOutputPath);
        var projectService = new StrictMock<IRazorProjectService>();
        projectService
            .Setup(service => service.AddProjectAsync(
                projectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectService
            .Setup(p => p.UpdateProjectAsync(
                projectKey,
                It.IsAny<RazorConfiguration>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProjectWorkspaceState>(),
                It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var synchronizer = GetSynchronizer(projectService.Object);
        var changedDeserializer = CreateDeserializer(projectInfo);
        var removedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Removed, changedDeserializer);
        var addedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Added, changedDeserializer);
        var changedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Changed, changedDeserializer);

        // Act
        var enqueueTask = await Dispatcher.RunAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addedArgs);
            synchronizer.ProjectConfigurationFileChanged(changedArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        // Assert
        projectService.Verify(p => p.UpdateProjectAsync(
            projectKey,
            It.IsAny<RazorConfiguration>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<ProjectWorkspaceState>(),
            It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
            It.IsAny<CancellationToken>()), Times.Once);

        projectService.VerifyAll();
    }

    private async Task WaitForEnqueue_DispatcherThreadAsync(ProjectConfigurationStateSynchronizer synchronizer, bool hasTask = true)
    {
        Dispatcher.AssertRunningOnDispatcher();
        if (hasTask)
        {
            var kvp = Assert.Single(synchronizer.ProjectInfoMap);
            await kvp.Value.ProjectUpdateTask;
        }
        else
        {
            Assert.Empty(synchronizer.ProjectInfoMap);
        }
    }

    private ProjectConfigurationStateSynchronizer GetSynchronizer(IRazorProjectService razorProjectService)
    {
        var synchronizer = new ProjectConfigurationStateSynchronizer(Dispatcher, razorProjectService, LoggerFactory, new TestLanguageServerFeatureOptions());
        synchronizer.EnqueueDelay = 5;

        return synchronizer;
    }

    private static IRazorProjectInfoDeserializer CreateDeserializer(RazorProjectInfo projectInfo)
    {
        var deserializer = new Mock<IRazorProjectInfoDeserializer>(MockBehavior.Strict);
        deserializer
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns(projectInfo);

        return deserializer.Object;
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class ProjectConfigurationStateSynchronizerTest : LanguageServerTestBase
{
    public ProjectConfigurationStateSynchronizerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Removed_UnknownDocumentNoops()
    {
        // Arrange
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        var synchronizer = GetSynchronizer(projectService.Object);
        var jsonFileDeserializer = Mock.Of<JsonFileDeserializer>(MockBehavior.Strict);
        var args = new ProjectConfigurationFileChangeEventArgs("/path/to/project.razor.json", RazorFileChangeKind.Removed, jsonFileDeserializer);

        // Act
        await Dispatcher.RunOnDispatcherThreadAsync(
            () => synchronizer.ProjectConfigurationFileChanged(args), DisposalToken);

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Removed_NonNormalizedPaths()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.json",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var intermediateOutputPath = Path.GetDirectoryName(projectInfo.SerializedFilePath);
        var projectKey = TestProjectKey.Create(intermediateOutputPath);
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.AddProject(projectInfo.FilePath, @"path\to\obj", projectInfo.Configuration, projectInfo.RootNamespace))
            .Returns(projectKey);
        projectService.Setup(service => service.UpdateProject(
            projectKey,
            projectInfo.Configuration,
            projectInfo.RootNamespace,
            projectInfo.ProjectWorkspaceState,
            projectInfo.Documents)).Verifiable();
        projectService.Setup(service => service.UpdateProject(
             projectKey,
             null,
             null,
             ProjectWorkspaceState.Default,
             ImmutableArray<DocumentSnapshotHandle>.Empty)).Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var jsonFileDeserializer = CreateJsonFileDeserializer(projectInfo);
        var addArgs = new ProjectConfigurationFileChangeEventArgs("/path/to\\obj/project.razor.json", RazorFileChangeKind.Added, jsonFileDeserializer);
        var enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        var removeArgs = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.json", RazorFileChangeKind.Removed, Mock.Of<JsonFileDeserializer>(MockBehavior.Strict));

        // Act
        enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
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
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        var synchronizer = GetSynchronizer(projectService.Object);
        var jsonFileDeserializer = Mock.Of<JsonFileDeserializer>(d => d.Deserialize<RazorProjectInfo>(It.IsAny<string>()) == null, MockBehavior.Strict);
        var args = new ProjectConfigurationFileChangeEventArgs("/path/to/project.razor.json", RazorFileChangeKind.Added, jsonFileDeserializer);

        // Act
        await Dispatcher.RunOnDispatcherThreadAsync(
            () => synchronizer.ProjectConfigurationFileChanged(args), DisposalToken);

        // Assert
        projectService.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Added_AddAndUpdatesProject()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.json",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var projectKey = TestProjectKey.Create(Path.GetDirectoryName(projectInfo.SerializedFilePath));
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.AddProject(projectInfo.FilePath, @"path\to\obj", projectInfo.Configuration, projectInfo.RootNamespace))
            .Returns(projectKey);
        projectService.Setup(service => service.UpdateProject(
            projectKey,
            projectInfo.Configuration,
            projectInfo.RootNamespace,
            projectInfo.ProjectWorkspaceState,
            projectInfo.Documents)).Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var jsonFileDeserializer = CreateJsonFileDeserializer(projectInfo);
        var args = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.json", RazorFileChangeKind.Added, jsonFileDeserializer);

        // Act
        var enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
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
            "/path/to/obj/project.razor.json",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var projectKey = TestProjectKey.Create(Path.GetDirectoryName(projectInfo.SerializedFilePath));
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.AddProject(projectInfo.FilePath, @"path\to\obj", projectInfo.Configuration, projectInfo.RootNamespace))
            .Returns(projectKey);
        projectService.Setup(service => service.UpdateProject(
            projectKey,
            projectInfo.Configuration,
            projectInfo.RootNamespace,
            projectInfo.ProjectWorkspaceState,
            projectInfo.Documents)).Verifiable();
        projectService.Setup(service => service.UpdateProject(
             projectKey,
             null,
             null,
             ProjectWorkspaceState.Default,
             Array.Empty<DocumentSnapshotHandle>())).Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var jsonFileDeserializer = CreateJsonFileDeserializer(projectInfo);
        var addArgs = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.json", RazorFileChangeKind.Added, jsonFileDeserializer);
        var enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        var removeArgs = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.json", RazorFileChangeKind.Removed, Mock.Of<JsonFileDeserializer>(MockBehavior.Strict));

        // Act
        enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
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
            "/path/to/obj/project.razor.json",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var projectKey = TestProjectKey.Create(Path.GetDirectoryName(initialProjectInfo.SerializedFilePath));
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.AddProject(initialProjectInfo.FilePath, @"path\to\obj", initialProjectInfo.Configuration, initialProjectInfo.RootNamespace))
            .Returns(projectKey);
        projectService.Setup(service => service.UpdateProject(
            projectKey,
            initialProjectInfo.Configuration,
            initialProjectInfo.RootNamespace,
            initialProjectInfo.ProjectWorkspaceState,
            initialProjectInfo.Documents)).Verifiable();
        var changedProjectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.json",
            "path/to/project.csproj",
            RazorConfiguration.Create(
                RazorLanguageVersion.Experimental,
                "TestConfiguration",
                Array.Empty<RazorExtension>()),
            rootNamespace: "TestRootNamespace2",
            new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, LanguageVersion.CSharp6),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        projectService.Setup(service => service.UpdateProject(
            projectKey,
            changedProjectInfo.Configuration,
            changedProjectInfo.RootNamespace,
            changedProjectInfo.ProjectWorkspaceState,
            changedProjectInfo.Documents)).Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var addDeserializer = CreateJsonFileDeserializer(initialProjectInfo);
        var addArgs = new ProjectConfigurationFileChangeEventArgs("path/to/obj/project.razor.json", RazorFileChangeKind.Added, addDeserializer);

        var enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        var changedDeserializer = CreateJsonFileDeserializer(changedProjectInfo);
        var changedArgs = new ProjectConfigurationFileChangeEventArgs("path/to/obj/project.razor.json", RazorFileChangeKind.Changed, changedDeserializer);

        // Act
        enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
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
            "/path/to/obj/project.razor.json",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var projectKey = TestProjectKey.Create(Path.GetDirectoryName(initialProjectInfo.SerializedFilePath));
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.AddProject(initialProjectInfo.FilePath, @"path\to\obj", initialProjectInfo.Configuration, initialProjectInfo.RootNamespace))
            .Returns(projectKey);
        projectService.Setup(service => service.UpdateProject(
            projectKey,
            initialProjectInfo.Configuration,
            initialProjectInfo.RootNamespace,
            initialProjectInfo.ProjectWorkspaceState,
            initialProjectInfo.Documents)).Verifiable();
        var changedProjectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.json",
            "path/to/project.csproj",
            RazorConfiguration.Create(
                RazorLanguageVersion.Experimental,
                "TestConfiguration",
                Array.Empty<RazorExtension>()),
            rootNamespace: "TestRootNamespace2",
            new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, LanguageVersion.CSharp6),
            ImmutableArray<DocumentSnapshotHandle>.Empty);

        // This is the request that happens when the server is reset
        projectService.Setup(service => service.UpdateProject(
             projectKey,
             null,
             null,
             ProjectWorkspaceState.Default,
             Array.Empty<DocumentSnapshotHandle>())).Verifiable();
        var synchronizer = GetSynchronizer(projectService.Object);
        var addDeserializer = CreateJsonFileDeserializer(initialProjectInfo);
        var addArgs = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.json", RazorFileChangeKind.Added, addDeserializer);
        var enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        var changedDeserializer = Mock.Of<JsonFileDeserializer>(d => d.Deserialize<RazorProjectInfo>(It.IsAny<string>()) == null, MockBehavior.Strict);
        var changedArgs = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.json", RazorFileChangeKind.Changed, changedDeserializer);

        // Act
        enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
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
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        var synchronizer = GetSynchronizer(projectService.Object);
        var changedDeserializer = Mock.Of<JsonFileDeserializer>(d => d.Deserialize<RazorProjectInfo>(It.IsAny<string>()) == null, MockBehavior.Strict);
        var changedArgs = new ProjectConfigurationFileChangeEventArgs("/path/to/project.razor.json", RazorFileChangeKind.Changed, changedDeserializer);

        // Act
        var enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
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
            new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, LanguageVersion.CSharp5),
            ImmutableArray<DocumentSnapshotHandle>.Empty);
        var projectKey = TestProjectKey.Create(Path.GetDirectoryName(projectInfo.SerializedFilePath));
        var projectService = new Mock<RazorProjectService>(MockBehavior.Strict);
        projectService.Setup(service => service.AddProject(projectInfo.FilePath, @"path\to\obj", projectInfo.Configuration, projectInfo.RootNamespace))
            .Returns(projectKey);
        projectService.Setup(p => p.UpdateProject(
            projectKey,
            It.IsAny<RazorConfiguration>(),
            It.IsAny<string>(),
            It.IsAny<ProjectWorkspaceState>(),
            It.IsAny<IReadOnlyList<DocumentSnapshotHandle>>()));

        var synchronizer = GetSynchronizer(projectService.Object);
        var changedDeserializer = CreateJsonFileDeserializer(projectInfo);
        var removedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Removed, changedDeserializer);
        var addedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Added, changedDeserializer);
        var changedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Changed, changedDeserializer);

        // Act
        var enqueueTask = await Dispatcher.RunOnDispatcherThreadAsync(async () =>
        {
            synchronizer.ProjectConfigurationFileChanged(addedArgs);
            synchronizer.ProjectConfigurationFileChanged(changedArgs);
            await WaitForEnqueue_DispatcherThreadAsync(synchronizer);
        }, DisposalToken);
        await enqueueTask;

        // Assert
        projectService.Verify(p => p.UpdateProject(
            projectKey,
            It.IsAny<RazorConfiguration>(),
            It.IsAny<string>(),
            It.IsAny<ProjectWorkspaceState>(),
            It.IsAny<IReadOnlyList<DocumentSnapshotHandle>>()), Times.Once);

        projectService.VerifyAll();
    }

    private async Task WaitForEnqueue_DispatcherThreadAsync(ProjectConfigurationStateSynchronizer synchronizer, bool hasTask = true)
    {
        Dispatcher.AssertDispatcherThread();
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

    private ProjectConfigurationStateSynchronizer GetSynchronizer(RazorProjectService razorProjectService)
    {
        var synchronizer = new ProjectConfigurationStateSynchronizer(Dispatcher, razorProjectService, LoggerFactory);
        synchronizer.EnqueueDelay = 5;

        return synchronizer;
    }

    private static JsonFileDeserializer CreateJsonFileDeserializer(RazorProjectInfo deserializedHandle)
    {
        var deserializer = new Mock<JsonFileDeserializer>(MockBehavior.Strict);
        deserializer.Setup(deserializer => deserializer.Deserialize<RazorProjectInfo>(It.IsAny<string>()))
            .Returns(deserializedHandle);

        return deserializer.Object;
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
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
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
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
        var projectServiceMock = new StrictMock<IRazorProjectService>();

        using var synchronizer = GetSynchronizer(projectServiceMock.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/project.razor.bin",
            kind: RazorFileChangeKind.Removed,
            deserializer: StrictMock.Of<IRazorProjectInfoDeserializer>());

        // Act
        synchronizer.ProjectConfigurationFileChanged(args);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.VerifyAll();
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
            documents: []);
        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(projectInfo.SerializedFilePath);
        var projectKey = TestProjectKey.Create(intermediateOutputPath);

        var projectServiceMock = new StrictMock<IRazorProjectService>();
        projectServiceMock
            .Setup(x => x.AddProjectAsync(
                projectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectServiceMock
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
        projectServiceMock
            .Setup(x => x.UpdateProjectAsync(
                projectKey,
                null,
                null,
                "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        using var synchronizer = GetSynchronizer(projectServiceMock.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var addArgs = new ProjectConfigurationFileChangeEventArgs(
            "/path/to\\obj/project.razor.bin",
            RazorFileChangeKind.Added,
            CreateDeserializer(projectInfo));

        synchronizer.ProjectConfigurationFileChanged(addArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        var removeArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Removed,
            deserializer: StrictMock.Of<IRazorProjectInfoDeserializer>());

        // Act
        synchronizer.ProjectConfigurationFileChanged(removeArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Added_CantDeserialize_Noops()
    {
        // Arrange
        var projectServiceMock = new StrictMock<IRazorProjectService>();

        using var synchronizer = GetSynchronizer(projectServiceMock.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            deserializer: CreateDeserializer(projectInfo: null));

        // Act
        synchronizer.ProjectConfigurationFileChanged(args);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.VerifyAll();
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
            documents: []);
        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(projectInfo.SerializedFilePath);
        var projectKey = TestProjectKey.Create(intermediateOutputPath);

        var projectServiceMock = new StrictMock<IRazorProjectService>();
        projectServiceMock
            .Setup(service => service.AddProjectAsync(
                projectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectServiceMock
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

        using var synchronizer = GetSynchronizer(projectServiceMock.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            deserializer: CreateDeserializer(projectInfo));

        // Act
        synchronizer.ProjectConfigurationFileChanged(args);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.VerifyAll();
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
            documents: []);
        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(projectInfo.SerializedFilePath);
        var projectKey = TestProjectKey.Create(intermediateOutputPath);

        var projectServiceMock = new StrictMock<IRazorProjectService>();
        projectServiceMock
            .Setup(service => service.AddProjectAsync(
                projectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectServiceMock
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
        projectServiceMock
            .Setup(service => service.UpdateProjectAsync(
                projectKey,
                null,
                null,
                "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        using var synchronizer = GetSynchronizer(projectServiceMock.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var addArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            deserializer: CreateDeserializer(projectInfo));

        synchronizer.ProjectConfigurationFileChanged(addArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        var removeArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Removed,
            deserializer: StrictMock.Of<IRazorProjectInfoDeserializer>());

        // Act
        synchronizer.ProjectConfigurationFileChanged(removeArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.VerifyAll();
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
            documents: []);
        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(initialProjectInfo.SerializedFilePath);
        var projectKey = TestProjectKey.Create(intermediateOutputPath);

        var projectServiceMock = new StrictMock<IRazorProjectService>();
        projectServiceMock
            .Setup(service => service.AddProjectAsync(
                initialProjectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                initialProjectInfo.RootNamespace,
                initialProjectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectServiceMock
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
            documents: []);
        projectServiceMock
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

        using var synchronizer = GetSynchronizer(projectServiceMock.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var addArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            deserializer: CreateDeserializer(initialProjectInfo));

        synchronizer.ProjectConfigurationFileChanged(addArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        var changedArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Changed,
            deserializer: CreateDeserializer(changedProjectInfo));

        // Act
        synchronizer.ProjectConfigurationFileChanged(changedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.VerifyAll();
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
            documents: []);
        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(initialProjectInfo.SerializedFilePath);
        var projectKey = TestProjectKey.Create(intermediateOutputPath);

        var projectServiceMock = new StrictMock<IRazorProjectService>();
        projectServiceMock
            .Setup(service => service.AddProjectAsync(
                initialProjectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                initialProjectInfo.RootNamespace,
                initialProjectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectServiceMock
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
            documents: []);

        // This is the request that happens when the server is reset
        projectServiceMock
            .Setup(service => service.UpdateProjectAsync(
                projectKey,
                null,
                null,
                "",
                ProjectWorkspaceState.Default,
                ImmutableArray<DocumentSnapshotHandle>.Empty,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        using var synchronizer = GetSynchronizer(projectServiceMock.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var addArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            deserializer: CreateDeserializer(initialProjectInfo));

        synchronizer.ProjectConfigurationFileChanged(addArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        var changedArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Changed,
            deserializer: CreateDeserializer(projectInfo: null));

        // Act
        synchronizer.ProjectConfigurationFileChanged(changedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_Changed_UntrackedProject_Noops()
    {
        // Arrange
        var projectService = new StrictMock<IRazorProjectService>();

        using var synchronizer = GetSynchronizer(projectService.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var changedArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/project.razor.bin",
            kind: RazorFileChangeKind.Changed,
            deserializer: CreateDeserializer(projectInfo: null));

        // Act
        synchronizer.ProjectConfigurationFileChanged(changedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

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
            documents: []);
        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(projectInfo.SerializedFilePath);
        var projectKey = TestProjectKey.Create(intermediateOutputPath);

        var projectServiceMock = new StrictMock<IRazorProjectService>();
        projectServiceMock
            .Setup(service => service.AddProjectAsync(
                projectInfo.FilePath,
                intermediateOutputPath,
                It.IsAny<RazorConfiguration>(),
                projectInfo.RootNamespace,
                projectInfo.DisplayName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(projectKey);
        projectServiceMock
            .Setup(p => p.UpdateProjectAsync(
                projectKey,
                It.IsAny<RazorConfiguration>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProjectWorkspaceState>(),
                It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        using var synchronizer = GetSynchronizer(projectServiceMock.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var deserializer = CreateDeserializer(projectInfo);
        var addedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Added, deserializer);
        var changedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Changed, deserializer);

        // Act
        synchronizer.ProjectConfigurationFileChanged(addedArgs);
        synchronizer.ProjectConfigurationFileChanged(changedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.Verify(p => p.UpdateProjectAsync(
            projectKey,
            It.IsAny<RazorConfiguration>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<ProjectWorkspaceState>(),
            It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        projectServiceMock.VerifyAll();
    }

    private TestProjectConfigurationStateSynchronizer GetSynchronizer(IRazorProjectService razorProjectService)
        => new(razorProjectService, LoggerFactory, TestLanguageServerFeatureOptions.Instance, TimeSpan.FromMilliseconds(5));

    private static IRazorProjectInfoDeserializer CreateDeserializer(RazorProjectInfo? projectInfo)
    {
        var deserializerMock = new StrictMock<IRazorProjectInfoDeserializer>();
        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns(projectInfo);

        return deserializerMock.Object;
    }

    private sealed class TestProjectConfigurationStateSynchronizer(
        IRazorProjectService projectService,
        ILoggerFactory loggerFactory,
        LanguageServerFeatureOptions options,
        TimeSpan delay)
        : ProjectConfigurationStateSynchronizer(projectService, loggerFactory, options, delay);
}

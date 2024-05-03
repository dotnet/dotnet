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
    public async Task ProjectConfigurationFileChanged_Removed_UntrackedProject_CallsUpdate()
    {
        // Arrange
        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/project.razor.bin",
            kind: RazorFileChangeKind.Removed,
            deserializer: StrictMock.Of<IRazorProjectInfoDeserializer>());

        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(args.ConfigurationFilePath);
        var projectKey = TestProjectKey.Create(intermediateOutputPath);

        var projectServiceMock = new StrictMock<IRazorProjectService>();
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
            .Setup(x => x.AddOrUpdateProjectAsync(
                projectKey,
                projectInfo.FilePath,
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
            .Setup(service => service.AddOrUpdateProjectAsync(
                projectKey,
                projectInfo.FilePath,
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
            .Setup(service => service.AddOrUpdateProjectAsync(
                projectKey,
                projectInfo.FilePath,
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
            .Setup(service => service.AddOrUpdateProjectAsync(
                projectKey,
                initialProjectInfo.FilePath,
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
            .Setup(service => service.AddOrUpdateProjectAsync(
                projectKey,
                changedProjectInfo.FilePath,
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
            .Setup(service => service.AddOrUpdateProjectAsync(
                projectKey,
                initialProjectInfo.FilePath,
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
    public async Task ProjectConfigurationFileChanged_Changed_UntrackedProject_CallsUpdate()
    {
        // Arrange
        var changedArgs = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/project.razor.bin",
            kind: RazorFileChangeKind.Changed,
            deserializer: CreateDeserializer(projectInfo: null));

        var intermediateOutputPath = FilePathNormalizer.GetNormalizedDirectoryName(changedArgs.ConfigurationFilePath);
        var projectKey = TestProjectKey.Create(intermediateOutputPath);

        var projectServiceMock = new StrictMock<IRazorProjectService>();
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

        // Act
        synchronizer.ProjectConfigurationFileChanged(changedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_RemoveThenAdd_Updates()
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
            .Setup(p => p.AddOrUpdateProjectAsync(
                projectKey,
                projectInfo.FilePath,
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
        var removedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Removed, deserializer);
        var addedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Added, deserializer);

        // Act
        synchronizer.ProjectConfigurationFileChanged(removedArgs);
        synchronizer.ProjectConfigurationFileChanged(addedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.Verify(p => p.AddOrUpdateProjectAsync(
            projectKey,
            projectInfo.FilePath,
            It.IsAny<RazorConfiguration>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<ProjectWorkspaceState>(),
            It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        projectServiceMock.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_AddThenRemove_AddsAndRemoves()
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
        var removedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Removed, deserializer);

        // Act
        synchronizer.ProjectConfigurationFileChanged(addedArgs);
        synchronizer.ProjectConfigurationFileChanged(removedArgs);

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

    [Fact]
    public async Task ProjectConfigurationFileChanged_AddThenRemoveThenAdd_AddsAndUpdates()
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
            .Setup(p => p.AddOrUpdateProjectAsync(
                projectKey,
                projectInfo.FilePath,
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
        var removedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Removed, deserializer);

        // Act
        synchronizer.ProjectConfigurationFileChanged(addedArgs);
        synchronizer.ProjectConfigurationFileChanged(removedArgs);
        synchronizer.ProjectConfigurationFileChanged(addedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.Verify(p => p.AddOrUpdateProjectAsync(
                projectKey,
                projectInfo.FilePath,
                It.IsAny<RazorConfiguration>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProjectWorkspaceState>(),
                It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        projectServiceMock.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_RemoveThenRemoveThenAdd_UpdatesTwice()
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
            .Setup(p => p.AddOrUpdateProjectAsync(
                projectKey,
                projectInfo.FilePath,
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
        var removedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Removed, deserializer);

        // Act
        synchronizer.ProjectConfigurationFileChanged(removedArgs);
        synchronizer.ProjectConfigurationFileChanged(removedArgs);
        synchronizer.ProjectConfigurationFileChanged(addedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.Verify(p => p.AddOrUpdateProjectAsync(
            projectKey,
            projectInfo.FilePath,
            It.IsAny<RazorConfiguration>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<ProjectWorkspaceState>(),
            It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        projectServiceMock.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_AddThenRemoveThenAddThenUpdate_AddsAndUpdates()
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
            .Setup(p => p.AddOrUpdateProjectAsync(
                projectKey,
                projectInfo.FilePath,
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
        var removedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Removed, deserializer);
        var changedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo.SerializedFilePath, RazorFileChangeKind.Changed, deserializer);

        // Act
        synchronizer.ProjectConfigurationFileChanged(addedArgs);
        synchronizer.ProjectConfigurationFileChanged(removedArgs);
        synchronizer.ProjectConfigurationFileChanged(addedArgs);
        synchronizer.ProjectConfigurationFileChanged(changedArgs);
        synchronizer.ProjectConfigurationFileChanged(changedArgs);
        synchronizer.ProjectConfigurationFileChanged(changedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        // Update is only called twice because the Remove-then-Add is changed to an Update,
        // then that Update is deduped with the one following
        projectServiceMock.Verify(p => p.AddOrUpdateProjectAsync(
                projectKey,
                projectInfo.FilePath,
                It.IsAny<RazorConfiguration>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProjectWorkspaceState>(),
                It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        projectServiceMock.VerifyAll();
    }

    [Fact]
    public async Task ProjectConfigurationFileChanged_RemoveThenAddDifferentProjects_RemovesAndAdds()
    {
        // Arrange
        var projectInfo1 = new RazorProjectInfo(
            "/path/to/obj/project.razor.json",
            "path/to/project.csproj",
            RazorConfiguration.Default,
            rootNamespace: "TestRootNamespace",
            displayName: "project",
            ProjectWorkspaceState.Create(LanguageVersion.CSharp5),
            documents: []);
        var intermediateOutputPath1 = FilePathNormalizer.GetNormalizedDirectoryName(projectInfo1.SerializedFilePath);
        var projectKey1 = TestProjectKey.Create(intermediateOutputPath1);

        var projectInfo2 = new RazorProjectInfo(
           "/path/other/obj/project.razor.json",
           "path/other/project.csproj",
           RazorConfiguration.Default,
           rootNamespace: "TestRootNamespace",
           displayName: "project",
           ProjectWorkspaceState.Create(LanguageVersion.CSharp5),
           documents: []);
        var intermediateOutputPath2 = FilePathNormalizer.GetNormalizedDirectoryName(projectInfo2.SerializedFilePath);
        var projectKey2 = TestProjectKey.Create(intermediateOutputPath2);

        var projectServiceMock = new StrictMock<IRazorProjectService>();
        projectServiceMock
            .Setup(p => p.UpdateProjectAsync(
                projectKey1,
                It.IsAny<RazorConfiguration>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProjectWorkspaceState>(),
                It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        projectServiceMock
            .Setup(p => p.AddOrUpdateProjectAsync(
                projectKey2,
                projectInfo2.FilePath,
                It.IsAny<RazorConfiguration>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProjectWorkspaceState>(),
                It.IsAny<ImmutableArray<DocumentSnapshotHandle>>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        using var synchronizer = GetSynchronizer(projectServiceMock.Object);
        var synchronizerAccessor = synchronizer.GetTestAccessor();

        var removedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo1.SerializedFilePath, RazorFileChangeKind.Removed, CreateDeserializer(projectInfo1));
        var addedArgs = new ProjectConfigurationFileChangeEventArgs(projectInfo2.SerializedFilePath, RazorFileChangeKind.Added, CreateDeserializer(projectInfo2));

        // Act
        synchronizer.ProjectConfigurationFileChanged(removedArgs);
        synchronizer.ProjectConfigurationFileChanged(addedArgs);

        await synchronizerAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        projectServiceMock.VerifyAll();
    }

    private TestProjectConfigurationStateSynchronizer GetSynchronizer(IRazorProjectService razorProjectService)
        => new(razorProjectService, LoggerFactory, TestLanguageServerFeatureOptions.Instance, TimeSpan.FromMilliseconds(50));

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

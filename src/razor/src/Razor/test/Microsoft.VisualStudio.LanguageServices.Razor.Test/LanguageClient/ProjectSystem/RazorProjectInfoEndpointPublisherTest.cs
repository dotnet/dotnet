// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.ProjectSystem;

public class RazorProjectInfoEndpointPublisherTest(ITestOutputHelper testOutput) : LanguageServerTestBase(testOutput)
{
    [Fact]
    [WorkItem("https://github.com/dotnet/aspnetcore/issues/35945")]
    public async Task ProjectManager_Changed_Remove_Change_NoopsOnDelayedPublish()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var callCount = 0;
        var tagHelpers = ImmutableArray.Create(
            new TagHelperDescriptor(FileKinds.Component, "Namespace.FileNameOther", "Assembly", "FileName", "FileName document", "FileName hint",
                caseSensitive: false, tagMatchingRules: default, attributeDescriptors: default, allowedChildTags: default, metadata: null!, diagnostics: default));

        var initialProjectSnapshot = CreateProjectSnapshot(
            @"C:\path\to\project.csproj", ProjectWorkspaceState.Create(tagHelpers, CodeAnalysis.CSharp.LanguageVersion.Preview));
        var expectedProjectSnapshot = CreateProjectSnapshot(
            @"C:\path\to\project.csproj", ProjectWorkspaceState.Create(CodeAnalysis.CSharp.LanguageVersion.Preview));
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) => callCount++)
            .ReturnsAsync(new ReinvokeResponse<object>());

        var serializer = new TestRazorProjectInfoFileSerializer();

        using var publisher = CreateRazorProjectInfoEndpointPublisher(requestInvoker.Object, projectManager, serializer);
        var publisherAccessor = publisher.GetTestAccessor();

        var documentRemovedArgs = ProjectChangeEventArgs.CreateTestInstance(
            initialProjectSnapshot, initialProjectSnapshot, documentFilePath: @"C:\path\to\file.razor", ProjectChangeKind.DocumentRemoved);
        var projectChangedArgs = ProjectChangeEventArgs.CreateTestInstance(
            initialProjectSnapshot, expectedProjectSnapshot, documentFilePath: null!, ProjectChangeKind.ProjectChanged);

        // Act
        publisherAccessor.ProjectManager_Changed(null!, documentRemovedArgs);
        publisherAccessor.ProjectManager_Changed(null!, projectChangedArgs);
        await publisherAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task ProjectManager_Changed_NotActive_Noops()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, rootNamespace: "TestRootNamespace");
        var hostDocument = new HostDocument(@"C:\path\to\file.razor", "file.razor");

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
        });

        var callCount = 0;
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) => callCount++)
            .ReturnsAsync(new ReinvokeResponse<object>());

        var serializer = new TestRazorProjectInfoFileSerializer();

        using var publisher = CreateRazorProjectInfoEndpointPublisher(requestInvoker.Object, projectManager, serializer);
        var publisherAccessor = publisher.GetTestAccessor();

        // Act
        await projectManager.UpdateAsync(updater =>
        {
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(hostDocument.FilePath));
        });
        await publisherAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Equal(0, callCount);
    }

    [Fact]
    public async Task ProjectManager_Changed_ServerStarted_InitializedProject_Publishes()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var hostProject = new HostProject(@"C:\path\to\project.csproj", @"C:\path\to\obj", RazorConfiguration.Default, rootNamespace: "TestRootNamespace");
        var hostDocument = new HostDocument(@"C:\path\to\file.razor", "file.razor");

        await projectManager.UpdateAsync(updater =>
        {
            updater.ProjectAdded(hostProject);
            updater.ProjectWorkspaceStateChanged(hostProject.Key, ProjectWorkspaceState.Default);
            updater.DocumentAdded(hostProject.Key, hostDocument, new EmptyTextLoader(hostDocument.FilePath));
        });

        var projectSnapshot = projectManager.GetProjects()[0];

        var callCount = 0;
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) => callCount++)
            .ReturnsAsync(new ReinvokeResponse<object>());

        var serializer = new TestRazorProjectInfoFileSerializer();

        using var publisher = CreateRazorProjectInfoEndpointPublisher(requestInvoker.Object, projectManager, serializer);

        // Act
        publisher.StartSending();

        // Assert
        Assert.Equal(1, callCount);
    }

    [Theory]
    [InlineData(ProjectChangeKind.DocumentAdded, true, false)]
    [InlineData(ProjectChangeKind.DocumentRemoved, true, false)]
    [InlineData(ProjectChangeKind.ProjectChanged, true, false)]
    [InlineData(ProjectChangeKind.ProjectRemoved, true, true)]
    [InlineData(ProjectChangeKind.ProjectAdded, false, false)]
    internal async Task ProjectManager_Changed_EnqueuesPublishAsync(ProjectChangeKind changeKind, bool waitForQueueEmpty, bool expectNullProjectInfo)
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();
        var projectConfigurationFilePathStore = new DefaultProjectConfigurationFilePathStore();

        var projectSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj", ProjectWorkspaceState.Create(CodeAnalysis.CSharp.LanguageVersion.CSharp7_3));
        var expectedProjectInfo = projectSnapshot.ToRazorProjectInfo();
        var callCount = 0;
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        ProjectInfoParams? projectInfoParams = null;
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) =>
                {
                    callCount++;
                    projectInfoParams = param;
                })
            .ReturnsAsync(new ReinvokeResponse<object>());

        var serializer = new TestRazorProjectInfoFileSerializer();

        using var publisher = CreateRazorProjectInfoEndpointPublisher(requestInvoker.Object, projectManager, serializer);
        var publisherAccessor = publisher.GetTestAccessor();

        var args = ProjectChangeEventArgs.CreateTestInstance(projectSnapshot, projectSnapshot, documentFilePath: null!, changeKind);

        // Act
        publisherAccessor.ProjectManager_Changed(null!, args);
        if (waitForQueueEmpty)
        {
            await publisherAccessor.WaitUntilCurrentBatchCompletesAsync();
        }

        // Assert
        Assert.Equal(1, callCount);
        Assert.NotNull(projectInfoParams);
        var filePath = Assert.Single(projectInfoParams.FilePaths);

        if (expectNullProjectInfo)
        {
            Assert.Null(filePath);
        }
        else
        {
            Assert.NotNull(filePath);
            var projectInfo = await serializer.DeserializeFromFileAndDeleteAsync(filePath, DisposalToken);
            Assert.NotNull(projectInfo);
            Assert.Equal(expectedProjectInfo, projectInfo);
        }
    }

    [Fact]
    public async Task EnqueuePublish_BatchesPublishRequestsAsync()
    {
        // Arrange
        var projectManager = CreateProjectSnapshotManager();

        var firstSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj");
        var secondSnapshot = CreateProjectSnapshot(@"C:\path\to\project.csproj", [@"C:\path\to\file.cshtml"]);
        var expectedProjectInfo = secondSnapshot.ToRazorProjectInfo();

        ProjectInfoParams? projectInfoParams = null;
        var callCount = 0;
        var requestInvoker = new Mock<LSPRequestInvoker>(MockBehavior.Strict);
        requestInvoker
            .Setup(r => r.ReinvokeRequestOnServerAsync<ProjectInfoParams, object>(
                LanguageServerConstants.RazorProjectInfoEndpoint,
                RazorLSPConstants.RazorLanguageServerName,
                It.IsAny<ProjectInfoParams>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, string, ProjectInfoParams, CancellationToken>((s1, s2, param, ct) =>
            {
                callCount++;
                projectInfoParams = param;
            })
            .ReturnsAsync(new ReinvokeResponse<object>());

        var serializer = new TestRazorProjectInfoFileSerializer();

        using var publisher = CreateRazorProjectInfoEndpointPublisher(requestInvoker.Object, projectManager, serializer);
        var publisherAccessor = publisher.GetTestAccessor();

        // Act
        publisherAccessor.EnqueuePublish(firstSnapshot);
        publisherAccessor.EnqueuePublish(secondSnapshot);
        await publisherAccessor.WaitUntilCurrentBatchCompletesAsync();

        // Assert
        Assert.Equal(1, callCount);
        Assert.NotNull(projectInfoParams);
        var filePath = Assert.Single(projectInfoParams.FilePaths);
        Assert.NotNull(filePath);
        var projectInfo = await serializer.DeserializeFromFileAndDeleteAsync(filePath, DisposalToken);
        Assert.Equal(expectedProjectInfo, projectInfo);
    }

    internal static IProjectSnapshot CreateProjectSnapshot(
        string projectFilePath,
        ProjectWorkspaceState? projectWorkspaceState = null,
        string[]? documentFilePaths = null)
    {
        return TestProjectSnapshot.Create(projectFilePath, documentFilePaths ?? [], projectWorkspaceState);
    }

    internal static IProjectSnapshot CreateProjectSnapshot(string projectFilePath, string[] documentFilePaths)
    {
        return TestProjectSnapshot.Create(projectFilePath, documentFilePaths);
    }

    private static RazorProjectInfoEndpointPublisher CreateRazorProjectInfoEndpointPublisher(
        LSPRequestInvoker requestInvoker,
        IProjectSnapshotManager projectSnapshotManager,
        IRazorProjectInfoFileSerializer serializer)
        => new(requestInvoker, projectSnapshotManager, serializer, TimeSpan.FromMilliseconds(5));

    private sealed class TestRazorProjectInfoFileSerializer : IRazorProjectInfoFileSerializer
    {
        private readonly Dictionary<string, RazorProjectInfo> _filePathToProjectInfoMap = new(FilePathComparer.Instance);

        public Task<RazorProjectInfo> DeserializeFromFileAndDeleteAsync(string filePath, CancellationToken cancellationToken)
        {
            return Task.FromResult(_filePathToProjectInfoMap[filePath]);
        }

        public Task<string> SerializeToTempFileAsync(RazorProjectInfo projectInfo, CancellationToken cancellationToken)
        {
            var filePath = Guid.NewGuid().ToString("D");
            _filePathToProjectInfoMap[filePath] = projectInfo;

            return Task.FromResult(filePath);
        }
    }
}

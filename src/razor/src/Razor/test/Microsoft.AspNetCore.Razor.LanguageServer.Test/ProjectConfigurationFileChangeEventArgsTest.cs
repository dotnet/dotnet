// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class ProjectConfigurationFileChangeEventArgsTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void TryDeserialize_RemovedKind_ReturnsFalse()
    {
        // Arrange
        var deserializerMock = new Mock<IRazorProjectInfoDeserializer>(MockBehavior.Strict);
        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns(new RazorProjectInfo(
                "/path/to/obj/project.razor.bin",
                "c:/path/to/project.csproj",
                configuration: RazorConfiguration.Default,
                rootNamespace: null,
                displayName: "project",
                projectWorkspaceState: ProjectWorkspaceState.Default,
                documents: ImmutableArray<DocumentSnapshotHandle>.Empty));

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Removed,
            projectInfoDeserializer: deserializerMock.Object);

        // Act
        var result = args.TryDeserialize(out var handle);

        // Assert
        Assert.False(result);
        Assert.Null(handle);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5907")]
    public void TryDeserialize_DifferingSerializationPaths_ReturnsFalse()
    {
        // Arrange
        var deserializerMock = new Mock<IRazorProjectInfoDeserializer>(MockBehavior.Strict);
        var projectInfo = new RazorProjectInfo(
            "/path/to/ORIGINAL/obj/project.razor.bin",
            "c:/path/to/project.csproj",
            configuration: RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project",
            projectWorkspaceState: ProjectWorkspaceState.Default,
            documents: ImmutableArray<DocumentSnapshotHandle>.Empty);

        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns(projectInfo);

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/DIFFERENT/obj/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            projectInfoDeserializer: deserializerMock.Object);

        // Act
        var result = args.TryDeserialize(out var deserializedProjectInfo);

        // Assert
        Assert.False(result);
        Assert.Null(deserializedProjectInfo);
    }

    [Fact]
    public void TryDeserialize_MemoizesResults()
    {
        // Arrange
        var deserializerMock = new Mock<IRazorProjectInfoDeserializer>(MockBehavior.Strict);
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "c:/path/to/project.csproj",
            configuration: RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project",
            projectWorkspaceState: ProjectWorkspaceState.Default,
            documents: ImmutableArray<DocumentSnapshotHandle>.Empty);

        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns(projectInfo);

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            projectInfoDeserializer: deserializerMock.Object);

        // Act
        var result1 = args.TryDeserialize(out var projectInfo1);
        var result2 = args.TryDeserialize(out var projectInfo2);

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.Same(projectInfo, projectInfo1);
        Assert.Same(projectInfo, projectInfo2);
    }

    [Fact]
    public void TryDeserialize_NullFileDeserialization_MemoizesResults_ReturnsFalse()
    {
        // Arrange
        var deserializerMock = new Mock<IRazorProjectInfoDeserializer>(MockBehavior.Strict);
        var callCount = 0;
        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Callback(() => callCount++)
            .Returns<RazorProjectInfo>(null);

        var args = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.bin", RazorFileChangeKind.Changed, deserializerMock.Object);

        // Act
        var result1 = args.TryDeserialize(out var handle1);
        var result2 = args.TryDeserialize(out var handle2);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
        Assert.Null(handle1);
        Assert.Null(handle2);
        Assert.Equal(1, callCount);
    }
}

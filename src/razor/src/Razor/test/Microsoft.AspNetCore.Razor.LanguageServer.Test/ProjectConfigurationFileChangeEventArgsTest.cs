// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
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
        var deserializerMock = new StrictMock<IRazorProjectInfoDeserializer>();
        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns(new RazorProjectInfo(
                "/path/to/obj/project.razor.bin",
                "c:/path/to/project.csproj",
                configuration: RazorConfiguration.Default,
                rootNamespace: null,
                displayName: "project",
                projectWorkspaceState: ProjectWorkspaceState.Default,
                documents: []));

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Removed,
            deserializer: deserializerMock.Object);

        // Act
        var result = args.TryDeserialize(TestLanguageServerFeatureOptions.Instance, out var handle);

        // Assert
        Assert.False(result);
        Assert.Null(handle);
    }

    [Fact]
    [WorkItem("https://github.com/dotnet/razor-tooling/issues/5907")]
    public void TryDeserialize_DifferingSerializationPaths_ReturnsFalse()
    {
        // Arrange
        var deserializerMock = new StrictMock<IRazorProjectInfoDeserializer>();
        var projectInfo = new RazorProjectInfo(
            "/path/to/ORIGINAL/obj/project.razor.bin",
            "c:/path/to/project.csproj",
            configuration: RazorConfiguration.Default,
            rootNamespace: null,
            displayName: "project",
            projectWorkspaceState: ProjectWorkspaceState.Default,
            documents: []);

        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns(projectInfo);

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/DIFFERENT/obj/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            deserializer: deserializerMock.Object);

        // Act
        var result = args.TryDeserialize(TestLanguageServerFeatureOptions.Instance, out var deserializedProjectInfo);

        // Assert
        Assert.False(result);
        Assert.Null(deserializedProjectInfo);
    }

    [Fact]
    public void TryDeserialize_MemoizesResults()
    {
        // Arrange
        var deserializerMock = new StrictMock<IRazorProjectInfoDeserializer>();
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "c:/path/to/project.csproj",
            configuration: RazorConfiguration.Default with { LanguageServerFlags = TestLanguageServerFeatureOptions.Instance.ToLanguageServerFlags() },
            rootNamespace: null,
            displayName: "project",
            projectWorkspaceState: ProjectWorkspaceState.Default,
            documents: []);

        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Returns(projectInfo);

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Added,
            deserializer: deserializerMock.Object);

        // Act
        var result1 = args.TryDeserialize(TestLanguageServerFeatureOptions.Instance, out var projectInfo1);
        var result2 = args.TryDeserialize(TestLanguageServerFeatureOptions.Instance, out var projectInfo2);

        // Assert
        Assert.True(result1);
        Assert.True(result2);

        // Deserialization will cause the LanguageServerFlags to be updated on the configuration, so reference equality will not hold.
        // Test equality, and that retrieving same instance on repeat calls works by reference equality of projectInfo1 and projectInfo2.
        Assert.Equal(projectInfo, projectInfo1);
        Assert.Same(projectInfo1, projectInfo2);
    }

    [Fact]
    public void TryDeserialize_NullFileDeserialization_MemoizesResults_ReturnsFalse()
    {
        // Arrange
        var deserializerMock = new StrictMock<IRazorProjectInfoDeserializer>();
        var callCount = 0;
        deserializerMock
            .Setup(x => x.DeserializeFromFile(It.IsAny<string>()))
            .Callback(() => callCount++)
            .Returns<RazorProjectInfo>(null);

        var args = new ProjectConfigurationFileChangeEventArgs(
            configurationFilePath: "/path/to/obj/project.razor.bin",
            kind: RazorFileChangeKind.Changed,
            deserializer: deserializerMock.Object);

        // Act
        var result1 = args.TryDeserialize(TestLanguageServerFeatureOptions.Instance, out var handle1);
        var result2 = args.TryDeserialize(TestLanguageServerFeatureOptions.Instance, out var handle2);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
        Assert.Null(handle1);
        Assert.Null(handle2);
        Assert.Equal(1, callCount);
    }
}

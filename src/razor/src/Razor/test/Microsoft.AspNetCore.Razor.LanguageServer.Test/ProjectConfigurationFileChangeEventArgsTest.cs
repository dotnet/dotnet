// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

public class ProjectConfigurationFileChangeEventArgsTest : TestBase
{
    public ProjectConfigurationFileChangeEventArgsTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void TryDeserialize_RemovedKind_ReturnsFalse()
    {
        // Arrange
        var jsonFileDeserializer = new Mock<JsonFileDeserializer>(MockBehavior.Strict);
        jsonFileDeserializer.Setup(deserializer => deserializer.Deserialize<ProjectRazorJson>(It.IsAny<string>()))
            .Returns(new ProjectRazorJson(
                "/path/to/obj/project.razor.json",
                "c:/path/to/project.csproj",
                configuration: null,
                rootNamespace: null,
                projectWorkspaceState: null,
                documents: Array.Empty<DocumentSnapshotHandle>()));
        var args = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.json", RazorFileChangeKind.Removed, jsonFileDeserializer.Object);

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
        var jsonFileDeserializer = new Mock<JsonFileDeserializer>(MockBehavior.Strict);
        var projectRazorJson = new ProjectRazorJson(
            "/path/to/ORIGINAL/obj/project.razor.json",
            "c:/path/to/project.csproj",
            configuration: null,
            rootNamespace: null,
            projectWorkspaceState: null,
            documents: Array.Empty<DocumentSnapshotHandle>());
        jsonFileDeserializer.Setup(deserializer => deserializer.Deserialize<ProjectRazorJson>(It.IsAny<string>()))
            .Returns(projectRazorJson);
        var args = new ProjectConfigurationFileChangeEventArgs("/path/to/DIFFERENT/obj/project.razor.json", RazorFileChangeKind.Added, jsonFileDeserializer.Object);

        // Act
        var result = args.TryDeserialize(out var deserializedProjectRazorJson);

        // Assert
        Assert.False(result);
        Assert.Null(deserializedProjectRazorJson);
    }

    [Fact]
    public void TryDeserialize_MemoizesResults()
    {
        // Arrange
        var jsonFileDeserializer = new Mock<JsonFileDeserializer>(MockBehavior.Strict);
        var projectRazorJson = new ProjectRazorJson(
            "/path/to/obj/project.razor.json",
            "c:/path/to/project.csproj",
            configuration: null,
            rootNamespace: null,
            projectWorkspaceState: null,
            documents: Array.Empty<DocumentSnapshotHandle>());
        jsonFileDeserializer.Setup(deserializer => deserializer.Deserialize<ProjectRazorJson>(It.IsAny<string>()))
            .Returns(projectRazorJson);
        var args = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.json", RazorFileChangeKind.Added, jsonFileDeserializer.Object);

        // Act
        var result1 = args.TryDeserialize(out var projectRazorJson1);
        var result2 = args.TryDeserialize(out var projectRazorJson2);

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.Same(projectRazorJson, projectRazorJson1);
        Assert.Same(projectRazorJson, projectRazorJson2);
    }

    [Fact]
    public void TryDeserialize_NullFileDeserialization_MemoizesResults_ReturnsFalse()
    {
        // Arrange
        var jsonFileDeserializer = new Mock<JsonFileDeserializer>(MockBehavior.Strict);
        var callCount = 0;
        jsonFileDeserializer.Setup(deserializer => deserializer.Deserialize<ProjectRazorJson>(It.IsAny<string>()))
            .Callback(() => callCount++)
            .Returns<ProjectRazorJson>(null);
        var args = new ProjectConfigurationFileChangeEventArgs("/path/to/obj/project.razor.json", RazorFileChangeKind.Changed, jsonFileDeserializer.Object);

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

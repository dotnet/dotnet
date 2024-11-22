// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

public class JsonSerializationTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private readonly RazorConfiguration _configuration = new(RazorLanguageVersion.Experimental, ConfigurationName: "Custom", [new("TestExtension")]);

    private readonly ProjectWorkspaceState _projectWorkspaceState = ProjectWorkspaceState.Create(
        tagHelpers: [TagHelperDescriptorBuilder.Create("Test", "TestAssembly").Build()],
        csharpLanguageVersion: LanguageVersion.LatestMajor);

    [Fact]
    public void RazorProjectInfo_InvalidVersionThrows()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            new ProjectKey("/path/to/obj/"),
            "/path/to/project.csproj",
            _configuration,
            rootNamespace: "TestProject",
            displayName: "project",
            _projectWorkspaceState,
            documents: []);

        var jsonText = JsonDataConvert.Serialize(projectInfo);
        Assert.NotNull(jsonText);

        var serializedJObject = JObject.Parse(jsonText);
        serializedJObject[WellKnownPropertyNames.Version] = -1;

        var updatedJsonText = serializedJObject.ToString();
        Assert.NotNull(updatedJsonText);

        // Act
        RazorProjectInfo? deserializedProjectInfo = null;
        Assert.Throws<RazorProjectInfoSerializationException>(() =>
        {
            deserializedProjectInfo = JsonDataConvert.DeserializeProjectInfo(updatedJsonText);
        });

        // Assert
        Assert.Null(deserializedProjectInfo);
    }

    [Fact]
    public void RazorProjectInfo_MissingVersionThrows()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            new ProjectKey("/path/to/obj/"),
            "/path/to/project.csproj",
            _configuration,
            rootNamespace: "TestProject",
            displayName: "project",
            _projectWorkspaceState,
            documents: []);

        var jsonText = JsonDataConvert.Serialize(projectInfo);
        Assert.NotNull(jsonText);

        var serializedJObject = JObject.Parse(jsonText);
        serializedJObject.Remove(WellKnownPropertyNames.Version);

        var updatedJsonText = serializedJObject.ToString();
        Assert.NotNull(updatedJsonText);

        // Act
        RazorProjectInfo? deserializedProjectInfo = null;
        Assert.Throws<RazorProjectInfoSerializationException>(() =>
        {
            deserializedProjectInfo = JsonDataConvert.DeserializeProjectInfo(updatedJsonText);
        });

        // Assert
        Assert.Null(deserializedProjectInfo);
    }

    [Fact]
    public void RazorProjectInfo_CanRoundTrip()
    {
        // Arrange
        var legacyDocument = new DocumentSnapshotHandle("/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);
        var componentDocument = new DocumentSnapshotHandle("/path/to/otherfile.razor", "otherfile.razor", FileKinds.Component);
        var projectInfo = new RazorProjectInfo(
            new ProjectKey("/path/to/obj/"),
            "/path/to/project.csproj",
            _configuration,
            rootNamespace: "TestProject",
            displayName: "project",
            _projectWorkspaceState,
            documents: [legacyDocument, componentDocument]);

        var jsonText = JsonDataConvert.Serialize(projectInfo);
        Assert.NotNull(jsonText);

        // Act
        var deserializedProjectInfo = JsonDataConvert.DeserializeProjectInfo(jsonText);
        Assert.NotNull(deserializedProjectInfo);

        // Assert
        Assert.Equal(projectInfo.FilePath, deserializedProjectInfo.FilePath);
        Assert.Equal(projectInfo.Configuration, deserializedProjectInfo.Configuration);
        Assert.Equal(projectInfo.RootNamespace, deserializedProjectInfo.RootNamespace);
        Assert.Equal(projectInfo.ProjectWorkspaceState, deserializedProjectInfo.ProjectWorkspaceState);
        Assert.Collection(projectInfo.Documents.OrderBy(doc => doc.FilePath),
            document =>
            {
                Assert.Equal(legacyDocument.FilePath, document.FilePath);
                Assert.Equal(legacyDocument.TargetPath, document.TargetPath);
                Assert.Equal(legacyDocument.FileKind, document.FileKind);
            },
            document =>
            {
                Assert.Equal(componentDocument.FilePath, document.FilePath);
                Assert.Equal(componentDocument.TargetPath, document.TargetPath);
                Assert.Equal(componentDocument.FileKind, document.FileKind);
            });
    }

    [Fact]
    public void RazorConfiguration_CanRoundTrip()
    {
        // Arrange
        var jsonText = JsonDataConvert.Serialize(_configuration);
        Assert.NotNull(jsonText);

        // Act
        var deserializedConfiguration = JsonDataConvert.DeserializeConfiguration(jsonText);

        // Assert
        Assert.Equal(_configuration, deserializedConfiguration);
    }
}

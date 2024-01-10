// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Test;

public class SerializationTest : ToolingTestBase
{
    private readonly RazorConfiguration _configuration;
    private readonly ProjectWorkspaceState _projectWorkspaceState;

    public SerializationTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var languageVersion = RazorLanguageVersion.Experimental;
        var extensions = new RazorExtension[]
        {
            new SerializedRazorExtension("TestExtension"),
        };

        _configuration = RazorConfiguration.Create(languageVersion, "Custom", extensions);
        _projectWorkspaceState = ProjectWorkspaceState.Create(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("Test", "TestAssembly").Build()),
            csharpLanguageVersion: LanguageVersion.LatestMajor);
    }

    [Fact]
    public void RazorProjectInfo_InvalidVersionThrows()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "/path/to/project.csproj",
            _configuration,
            rootNamespace: "TestProject",
            displayName: "project",
            _projectWorkspaceState,
            ImmutableArray<DocumentSnapshotHandle>.Empty);

        var jsonText = JsonDataConvert.SerializeObject(projectInfo, ObjectWriters.WriteProperties);
        Assert.NotNull(jsonText);

        var serializedJObject = JObject.Parse(jsonText);
        serializedJObject[WellKnownPropertyNames.Version] = -1;

        var updatedJsonText = serializedJObject.ToString();
        Assert.NotNull(updatedJsonText);

        // Act
        RazorProjectInfo? deserializedProjectInfo = null;
        Assert.Throws<RazorProjectInfoSerializationException>(() =>
        {
            deserializedProjectInfo = JsonDataConvert.DeserializeObject(updatedJsonText, ObjectReaders.ReadProjectInfoFromProperties);
        });

        // Assert
        Assert.Null(deserializedProjectInfo);
    }

    [Fact]
    public void RazorProjectInfo_MissingVersionThrows()
    {
        // Arrange
        var projectInfo = new RazorProjectInfo(
            "/path/to/obj/project.razor.bin",
            "/path/to/project.csproj",
            _configuration,
            rootNamespace: "TestProject",
            displayName: "project",
            _projectWorkspaceState,
            ImmutableArray<DocumentSnapshotHandle>.Empty);

        var jsonText = JsonDataConvert.SerializeObject(projectInfo, ObjectWriters.WriteProperties);
        Assert.NotNull(jsonText);

        var serializedJObject = JObject.Parse(jsonText);
        serializedJObject.Remove(WellKnownPropertyNames.Version);

        var updatedJsonText = serializedJObject.ToString();
        Assert.NotNull(updatedJsonText);

        // Act
        RazorProjectInfo? deserializedProjectInfo = null;
        Assert.Throws<RazorProjectInfoSerializationException>(() =>
        {
            deserializedProjectInfo = JsonDataConvert.DeserializeObject(updatedJsonText, ObjectReaders.ReadProjectInfoFromProperties);
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
            "/path/to/obj/project.razor.bin",
            "/path/to/project.csproj",
            _configuration,
            rootNamespace: "TestProject",
            displayName: "project",
            _projectWorkspaceState,
            ImmutableArray.Create(legacyDocument, componentDocument));

        var jsonText = JsonDataConvert.SerializeObject(projectInfo, ObjectWriters.WriteProperties);
        Assert.NotNull(jsonText);

        // Act
        var deserializedProjectInfo = JsonDataConvert.DeserializeObject(jsonText, ObjectReaders.ReadProjectInfoFromProperties);
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
        var jsonText = JsonDataConvert.SerializeObject(_configuration, ObjectWriters.WriteProperties);
        Assert.NotNull(jsonText);

        // Act
        var deserializedConfiguration = JsonDataConvert.DeserializeObject(jsonText, ObjectReaders.ReadConfigurationFromProperties);

        // Assert
        Assert.Equal(_configuration, deserializedConfiguration);
    }
}

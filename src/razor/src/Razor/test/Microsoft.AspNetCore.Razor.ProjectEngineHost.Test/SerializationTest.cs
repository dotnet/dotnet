// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Serialization.Converters;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Test;

public class SerializationTest : TestBase
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
        _projectWorkspaceState = new ProjectWorkspaceState(ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("Test", "TestAssembly").Build()),
            csharpLanguageVersion: LanguageVersion.LatestMajor);
    }

    [Fact]
    public void ProjectRazorJson_InvalidVersionThrows()
    {
        // Arrange
        var projectRazorJson = new ProjectRazorJson(
            "/path/to/obj/project.razor.json",
            "/path/to/project.csproj",
            _configuration,
            rootNamespace: "TestProject",
            _projectWorkspaceState,
            ImmutableArray<DocumentSnapshotHandle>.Empty);

        var jsonText = JsonConvert.SerializeObject(projectRazorJson, ProjectRazorJsonJsonConverter.Instance);
        Assert.NotNull(jsonText);

        var serializedJObject = JObject.Parse(jsonText);
        serializedJObject[WellKnownPropertyNames.Version] = -1;

        var updatedJsonText = JsonConvert.SerializeObject(serializedJObject);
        Assert.NotNull(updatedJsonText);

        // Act
        ProjectRazorJson? deserializedProjectRazorJson = null;
        Assert.Throws<ProjectRazorJsonSerializationException>(() =>
        {
            deserializedProjectRazorJson = JsonConvert.DeserializeObject<ProjectRazorJson>(updatedJsonText, ProjectRazorJsonJsonConverter.Instance);
        });

        // Assert
        Assert.Null(deserializedProjectRazorJson);
    }

    [Fact]
    public void ProjectRazorJson_MissingVersionThrows()
    {
        // Arrange
        var projectRazorJson = new ProjectRazorJson(
            "/path/to/obj/project.razor.json",
            "/path/to/project.csproj",
            _configuration,
            rootNamespace: "TestProject",
            _projectWorkspaceState,
            ImmutableArray<DocumentSnapshotHandle>.Empty);

        var jsonText = JsonConvert.SerializeObject(projectRazorJson, ProjectRazorJsonJsonConverter.Instance);
        Assert.NotNull(jsonText);

        var serializedJObject = JObject.Parse(jsonText);
        serializedJObject.Remove(WellKnownPropertyNames.Version);

        var updatedJsonText = JsonConvert.SerializeObject(serializedJObject);
        Assert.NotNull(updatedJsonText);

        // Act
        ProjectRazorJson? deserializedProjectRazorJson = null;
        Assert.Throws<ProjectRazorJsonSerializationException>(() =>
        {
            deserializedProjectRazorJson = JsonConvert.DeserializeObject<ProjectRazorJson>(updatedJsonText, ProjectRazorJsonJsonConverter.Instance);
        });

        // Assert
        Assert.Null(deserializedProjectRazorJson);
    }

    [Fact]
    public void ProjectRazorJson_CanRoundTrip()
    {
        // Arrange
        var legacyDocument = new DocumentSnapshotHandle("/path/to/file.cshtml", "file.cshtml", FileKinds.Legacy);
        var componentDocument = new DocumentSnapshotHandle("/path/to/otherfile.razor", "otherfile.razor", FileKinds.Component);
        var projectRazorJson = new ProjectRazorJson(
            "/path/to/obj/project.razor.json",
            "/path/to/project.csproj",
            _configuration,
            rootNamespace: "TestProject",
            _projectWorkspaceState,
            ImmutableArray.Create(legacyDocument, componentDocument));

        var jsonText = JsonConvert.SerializeObject(projectRazorJson, ProjectRazorJsonJsonConverter.Instance);
        Assert.NotNull(jsonText);

        // Act
        var deserializedProjectRazorJson = JsonConvert.DeserializeObject<ProjectRazorJson>(jsonText, ProjectRazorJsonJsonConverter.Instance);
        Assert.NotNull(deserializedProjectRazorJson);

        // Assert
        Assert.Equal(projectRazorJson.FilePath, deserializedProjectRazorJson.FilePath);
        Assert.Equal(projectRazorJson.Configuration, deserializedProjectRazorJson.Configuration);
        Assert.Equal(projectRazorJson.RootNamespace, deserializedProjectRazorJson.RootNamespace);
        Assert.Equal(projectRazorJson.ProjectWorkspaceState, deserializedProjectRazorJson.ProjectWorkspaceState);
        Assert.Collection(projectRazorJson.Documents.OrderBy(doc => doc.FilePath),
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

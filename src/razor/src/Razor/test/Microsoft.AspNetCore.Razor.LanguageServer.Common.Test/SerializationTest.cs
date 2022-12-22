// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

public class SerializationTest : TestBase
{
    public SerializationTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var languageVersion = RazorLanguageVersion.Experimental;
        var extensions = new RazorExtension[]
        {
            new SerializedRazorExtension("TestExtension"),
        };
        Configuration = RazorConfiguration.Create(languageVersion, "Custom", extensions);
        ProjectWorkspaceState = new ProjectWorkspaceState(new[]
        {
            TagHelperDescriptorBuilder.Create("Test", "TestAssembly").Build(),
        },
        LanguageVersion.LatestMajor);
        var converterCollection = new JsonConverterCollection();
        converterCollection.RegisterRazorConverters();
        Converters = converterCollection.ToArray();
    }

    private RazorConfiguration Configuration { get; }

    private ProjectWorkspaceState ProjectWorkspaceState { get; }

    private JsonConverter[] Converters { get; }

    [Fact]
    public void ProjectRazorJson_InvalidSerializationFormat_SerializesToNull()
    {
        // Arrange
        var projectRazorJson = new ProjectRazorJson(
            "/path/to/obj/project.razor.json",
            "/path/to/project.csproj",
            Configuration,
            rootNamespace: "TestProject",
            ProjectWorkspaceState,
            Array.Empty<DocumentSnapshotHandle>());
        var serializedHandle = JsonConvert.SerializeObject(projectRazorJson, Converters);
        var serializedJObject = JObject.Parse(serializedHandle);
        serializedJObject["SerializationFormat"] = "INVALID";
        var reserializedHandle = JsonConvert.SerializeObject(serializedJObject);

        // Act
        var deserializedHandle = JsonConvert.DeserializeObject<ProjectRazorJson>(reserializedHandle, Converters);

        // Assert
        Assert.Null(deserializedHandle);
    }

    [Fact]
    public void ProjectRazorJson_MissingSerializationFormat_SerializesToNull()
    {
        // Arrange
        var projectRazorJson = new ProjectRazorJson(
            "/path/to/obj/project.razor.json",
            "/path/to/project.csproj",
            Configuration,
            rootNamespace: "TestProject",
            ProjectWorkspaceState,
            Array.Empty<DocumentSnapshotHandle>());
        var serializedHandle = JsonConvert.SerializeObject(projectRazorJson, Converters);
        var serializedJObject = JObject.Parse(serializedHandle);
        serializedJObject.Remove("SerializationFormat");
        var reserializedHandle = JsonConvert.SerializeObject(serializedJObject);

        // Act
        var deserializedHandle = JsonConvert.DeserializeObject<ProjectRazorJson>(reserializedHandle, Converters);

        // Assert
        Assert.Null(deserializedHandle);
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
            Configuration,
            rootNamespace: "TestProject",
            ProjectWorkspaceState,
            new[] { legacyDocument, componentDocument });
        var serializedHandle = JsonConvert.SerializeObject(projectRazorJson, Converters);

        // Act
        var deserializedHandle = JsonConvert.DeserializeObject<ProjectRazorJson>(serializedHandle, Converters);

        // Assert
        Assert.Equal(projectRazorJson.FilePath, deserializedHandle.FilePath);
        Assert.Equal(projectRazorJson.Configuration, deserializedHandle.Configuration);
        Assert.Equal(projectRazorJson.RootNamespace, deserializedHandle.RootNamespace);
        Assert.Equal(projectRazorJson.ProjectWorkspaceState, deserializedHandle.ProjectWorkspaceState);
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
        var serializedConfiguration = JsonConvert.SerializeObject(Configuration, Converters);

        // Act
        var deserializedConfiguration = JsonConvert.DeserializeObject<RazorConfiguration>(serializedConfiguration, Converters);

        // Assert
        Assert.Equal(Configuration, deserializedConfiguration);
    }
}

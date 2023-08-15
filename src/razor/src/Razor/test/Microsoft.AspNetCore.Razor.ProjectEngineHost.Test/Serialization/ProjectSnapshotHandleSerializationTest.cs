// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Serialization.Converters;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Serialization;

public class ProjectSnapshotHandleSerializationTest : TestBase
{
    private readonly JsonConverter[] _converters;

    public ProjectSnapshotHandleSerializationTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var converters = new JsonConverterCollection();
        converters.RegisterRazorConverters();
        _converters = converters.ToArray();
    }

    [Fact]
    public void ProjectSnapshotHandleJsonConverter_Serialization_CanKindaRoundTrip()
    {
        // Arrange
        var projectId = ProjectId.CreateNewId();
        var snapshot = new ProjectSnapshotHandle(
            projectId,
            new ProjectSystemRazorConfiguration(
                RazorLanguageVersion.Version_1_1,
                "Test",
                new[]
                {
                    new ProjectSystemRazorExtension("Test-Extension1"),
                    new ProjectSystemRazorExtension("Test-Extension2"),
                }),
            "Test");

        // Act
        var json = JsonConvert.SerializeObject(snapshot, _converters);
        var obj = JsonConvert.DeserializeObject<ProjectSnapshotHandle>(json, _converters);

        // Assert
        Assert.NotNull(obj);
        Assert.Equal(snapshot.ProjectId, obj.ProjectId);
        Assert.NotNull(snapshot.Configuration);
        Assert.NotNull(obj.Configuration);
        Assert.Equal(snapshot.Configuration.ConfigurationName, obj.Configuration.ConfigurationName);
        Assert.Collection(
            snapshot.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Test-Extension1", e.ExtensionName),
            e => Assert.Equal("Test-Extension2", e.ExtensionName));
        Assert.Equal(snapshot.Configuration.LanguageVersion, obj.Configuration.LanguageVersion);
        Assert.Equal(snapshot.RootNamespace, obj.RootNamespace);
    }

    [Fact]
    public void ProjectSnapshotHandleJsonConverter_SerializationWithNulls_CanKindaRoundTrip()
    {
        // Arrange
        var projectId = ProjectId.CreateNewId();
        var snapshot = new ProjectSnapshotHandle(projectId, null, null);

        // Act
        var json = JsonConvert.SerializeObject(snapshot, _converters);
        var obj = JsonConvert.DeserializeObject<ProjectSnapshotHandle>(json, _converters);

        // Assert
        Assert.NotNull(obj);
        Assert.Equal(snapshot.ProjectId, obj.ProjectId);
        Assert.Null(obj.Configuration);
        Assert.Null(obj.RootNamespace);
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor;
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
        var snapshot = new ProjectSnapshotHandle(
            "Test.csproj",
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
        Assert.Equal(snapshot.FilePath, obj.FilePath);
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
        var snapshot = new ProjectSnapshotHandle("Test.csproj", null, null);

        // Act
        var json = JsonConvert.SerializeObject(snapshot, _converters);
        var obj = JsonConvert.DeserializeObject<ProjectSnapshotHandle>(json, _converters);

        // Assert
        Assert.Equal(snapshot.FilePath, obj.FilePath);
        Assert.Null(obj.Configuration);
        Assert.Null(obj.RootNamespace);
    }
}

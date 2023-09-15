// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Linq;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Test.Serialization;

public class ProjectSnapshotHandleSerializationTest(ITestOutputHelper testOutput) : TestBase(testOutput)
{
    private static readonly MessagePackSerializerOptions s_options = MessagePackSerializerOptions.Standard
        .WithResolver(CompositeResolver.Create(
            ProjectSnapshotHandleResolver.Instance,
            StandardResolver.Instance));

    [Fact]
    public void ProjectSnapshotHandleJsonConverter_Serialization_CanKindaRoundTrip()
    {
        // Arrange
        var projectId = ProjectId.CreateNewId();
        var expectedSnapshot = new ProjectSnapshotHandle(
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
        var bytes = MessagePackConvert.Serialize(expectedSnapshot, s_options);
        var actualSnapshot = MessagePackConvert.Deserialize<ProjectSnapshotHandle>(bytes, s_options);

        // Assert
        Assert.NotNull(actualSnapshot);
        Assert.Equal(expectedSnapshot.ProjectId, actualSnapshot.ProjectId);
        Assert.NotNull(expectedSnapshot.Configuration);
        Assert.NotNull(actualSnapshot.Configuration);
        Assert.Equal(expectedSnapshot.Configuration.ConfigurationName, actualSnapshot.Configuration.ConfigurationName);
        Assert.Collection(
            expectedSnapshot.Configuration.Extensions.OrderBy(e => e.ExtensionName),
            e => Assert.Equal("Test-Extension1", e.ExtensionName),
            e => Assert.Equal("Test-Extension2", e.ExtensionName));
        Assert.Equal(expectedSnapshot.Configuration.LanguageVersion, actualSnapshot.Configuration.LanguageVersion);
        Assert.Equal(expectedSnapshot.RootNamespace, actualSnapshot.RootNamespace);
    }

    [Fact]
    public void ProjectSnapshotHandleJsonConverter_SerializationWithNulls_CanKindaRoundTrip()
    {
        // Arrange
        var projectId = ProjectId.CreateNewId();
        var expectedSnapshot = new ProjectSnapshotHandle(projectId, null, null);

        // Act
        var bytes = MessagePackConvert.Serialize(expectedSnapshot, s_options);
        var actualSnapshot = MessagePackConvert.Deserialize<ProjectSnapshotHandle>(bytes, s_options);

        // Assert
        Assert.NotNull(actualSnapshot);
        Assert.Equal(expectedSnapshot.ProjectId, actualSnapshot.ProjectId);
        Assert.Null(actualSnapshot.Configuration);
        Assert.Null(actualSnapshot.RootNamespace);
    }
}

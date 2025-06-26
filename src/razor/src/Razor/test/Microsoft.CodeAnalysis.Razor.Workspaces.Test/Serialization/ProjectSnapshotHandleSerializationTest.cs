// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Resolvers;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

public class ProjectSnapshotHandleSerializationTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private static readonly MessagePackSerializerOptions s_options = MessagePackSerializerOptions.Standard
        .WithResolver(CompositeResolver.Create(
            ProjectSnapshotHandleResolver.Instance,
            StandardResolver.Instance));

    [Fact]
    public void ProjectSnapshotHandle_Serialization_CanKindaRoundTrip()
    {
        // Arrange
        var projectId = ProjectId.CreateNewId();
        var expectedSnapshot = new ProjectSnapshotHandle(
            projectId,
            new(RazorLanguageVersion.Version_1_1,
                "Test",
                [new("Test-Extension1"), new("Test-Extension2")],
                CodeAnalysis.CSharp.LanguageVersion.CSharp7,
                UseConsolidatedMvcViews: false,
                SuppressAddComponentParameter: true,
                UseRoslynTokenizer: true,
                PreprocessorSymbols: ["DEBUG", "TRACE", "DAVID"]),
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
        Assert.Equal(expectedSnapshot.Configuration.CSharpLanguageVersion, actualSnapshot.Configuration.CSharpLanguageVersion);
        Assert.Equal(expectedSnapshot.Configuration.UseConsolidatedMvcViews, actualSnapshot.Configuration.UseConsolidatedMvcViews);
        Assert.Equal(expectedSnapshot.Configuration.SuppressAddComponentParameter, actualSnapshot.Configuration.SuppressAddComponentParameter);
        Assert.Equal(expectedSnapshot.Configuration.UseRoslynTokenizer, actualSnapshot.Configuration.UseRoslynTokenizer);
        Assert.Collection(actualSnapshot.Configuration.PreprocessorSymbols,
            s => Assert.Equal("DEBUG", s),
            s => Assert.Equal("TRACE", s),
            s => Assert.Equal("DAVID", s));
    }

    [Fact]
    public void ProjectSnapshotHandle_SerializationWithNulls_CanKindaRoundTrip()
    {
        // Arrange
        var projectId = ProjectId.CreateNewId();
        var expectedSnapshot = new ProjectSnapshotHandle(projectId, RazorConfiguration.Default, null);

        // Act
        var bytes = MessagePackConvert.Serialize(expectedSnapshot, s_options);
        var actualSnapshot = MessagePackConvert.Deserialize<ProjectSnapshotHandle>(bytes, s_options);

        // Assert
        Assert.NotNull(actualSnapshot);
        Assert.Equal(expectedSnapshot.ProjectId, actualSnapshot.ProjectId);
        Assert.NotNull(actualSnapshot.Configuration);
        Assert.Null(actualSnapshot.RootNamespace);
    }
}

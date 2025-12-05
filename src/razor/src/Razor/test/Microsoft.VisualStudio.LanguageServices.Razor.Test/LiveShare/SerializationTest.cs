// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Razor.LiveShare.Serialization;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LiveShare;

public class SerializationTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void ProjectSnapshotHandleProxy_RoundTripsProperly()
    {
        // Arrange
        TagHelperCollection tagHelpers = [
            TagHelperDescriptorBuilder.CreateTagHelper("TestTagHelper", "TestAssembly").Build(),
            TagHelperDescriptorBuilder.CreateTagHelper("TestTagHelper2", "TestAssembly2").Build()];

        var projectWorkspaceState = ProjectWorkspaceState.Create(tagHelpers);
        var expectedConfiguration = RazorConfiguration.Default;
        var expectedRootNamespace = "project";
        var handle = new ProjectSnapshotHandleProxy(new Uri("vsls://some/path/project.csproj"), new Uri("vsls://some/path/obj"), RazorConfiguration.Default, expectedRootNamespace, projectWorkspaceState);

        var json = JsonConvert.SerializeObject(handle, ProjectSnapshotHandleProxyJsonConverter.Instance);
        Assert.NotNull(json);

        // Act
        var deserializedHandle = JsonConvert.DeserializeObject<ProjectSnapshotHandleProxy>(json, ProjectSnapshotHandleProxyJsonConverter.Instance);
        Assert.NotNull(deserializedHandle);

        // Assert
        Assert.Equal("vsls://some/path/project.csproj", deserializedHandle.FilePath.ToString());
        Assert.Equal(projectWorkspaceState, deserializedHandle.ProjectWorkspaceState);
        Assert.Equal(expectedConfiguration.ConfigurationName, deserializedHandle.Configuration.ConfigurationName);
        Assert.Equal(expectedConfiguration.Extensions.Length, deserializedHandle.Configuration.Extensions.Length);
        Assert.Equal(expectedConfiguration.LanguageVersion, deserializedHandle.Configuration.LanguageVersion);
        Assert.Equal(expectedRootNamespace, deserializedHandle.RootNamespace);
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.VisualStudio.LiveShare.Razor.Serialization;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LiveShare.Razor;

public class SerializationTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void ProjectSnapshotHandleProxy_RoundTripsProperly()
    {
        // Arrange
        var tagHelpers = ImmutableArray.Create(
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build(),
            TagHelperDescriptorBuilder.Create("TestTagHelper2", "TestAssembly2").Build());

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
        Assert.Equal(expectedConfiguration.Extensions.Count, deserializedHandle.Configuration.Extensions.Count);
        Assert.Equal(expectedConfiguration.LanguageVersion, deserializedHandle.Configuration.LanguageVersion);
        Assert.Equal(expectedRootNamespace, deserializedHandle.RootNamespace);
    }
}

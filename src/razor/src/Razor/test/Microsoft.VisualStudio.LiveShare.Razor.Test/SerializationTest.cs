// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Serialization;
using Microsoft.VisualStudio.LiveShare.Razor.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LiveShare.Razor;

public class SerializationTest : TestBase
{
    public SerializationTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void ProjectSnapshotHandleProxy_RoundTripsProperly()
    {
        // Arrange
        var tagHelpers = new[]
        {
            TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build(),
            TagHelperDescriptorBuilder.Create("TestTagHelper2", "TestAssembly2").Build(),
        };

        var projectWorkspaceState = new ProjectWorkspaceState(tagHelpers, default);
        var expectedConfiguration = RazorConfiguration.Default;
        var expectedRootNamespace = "project";
        var handle = new ProjectSnapshotHandleProxy(new Uri("vsls://some/path/project.csproj"), RazorConfiguration.Default, expectedRootNamespace, projectWorkspaceState);

        var serializedHandle = JsonConvertUtility.SerializeObject(handle, ProjectSnapshotHandleProxyJsonConverter.Instance);
        Assert.NotNull(serializedHandle);

        // Act
        var deserializedHandle = JsonConvertUtility.DeserializeObject<ProjectSnapshotHandleProxy>(serializedHandle, ProjectSnapshotHandleProxyJsonConverter.Instance);
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.Language;

public class TagHelperDescriptorBuilderTest
{
    [Fact]
    public void DisplayName_SetsDescriptorsDisplayName()
    {
        // Arrange
        var expectedDisplayName = "ExpectedDisplayName";
        var builder = TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly");

        // Act
        var descriptor = builder.DisplayName(expectedDisplayName).Build();

        // Assert
        Assert.Equal(expectedDisplayName, descriptor.DisplayName);
    }

    [Fact]
    public void DisplayName_DefaultsToTypeName()
    {
        // Arrange
        var expectedDisplayName = "TestTagHelper";
        var builder = TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly");

        // Act
        var descriptor = builder.Build();

        // Assert
        Assert.Equal(expectedDisplayName, descriptor.DisplayName);
    }

    [Fact]
    public void Metadata_Same()
    {
        // When SetMetadata is called on multiple builders with the same metadata collection,
        // they should share the instance.

        // Arrange
        var builder1 = TagHelperDescriptorBuilder.Create("TestTagHelper1", "TestAssembly1");
        var builder2 = TagHelperDescriptorBuilder.Create("TestTagHelper1", "TestAssembly1");

        var metadata = MetadataCollection.Create(
            RuntimeName("TestRuntime"),
            TypeName("TestTagHelper1"),
            TypeNameIdentifier("TestTagHelper1"));

        builder1.SetMetadata(metadata);
        builder2.SetMetadata(metadata);

        // Act
        var descriptor1 = builder1.Build();
        var descriptor2 = builder2.Build();

        // Assert
        Assert.Same(descriptor1.Metadata, descriptor2.Metadata);
    }

    [Fact]
    public void Metadata_NotSame()
    {
        // When Metadata is accessed on multiple builders with the same metadata,
        // they do not share the instance.

        // Arrange
        var builder1 = TagHelperDescriptorBuilder.Create("TestTagHelper1", "TestAssembly1");
        var builder2 = TagHelperDescriptorBuilder.Create("TestTagHelper1", "TestAssembly1");

        var runtimeName = RuntimeName("TestRuntime");

        builder1.Metadata[runtimeName.Key] = runtimeName.Value;
        builder1.Metadata.Add(TypeName("TestTagHelper1"));
        builder1.Metadata.Add(TypeNameIdentifier("TestTagHelper1"));

        builder2.Metadata[runtimeName.Key] = runtimeName.Value;
        builder2.Metadata.Add(TypeName("TestTagHelper1"));
        builder2.Metadata.Add(TypeNameIdentifier("TestTagHelper1"));

        // Act
        var descriptor1 = builder1.Build();
        var descriptor2 = builder2.Build();

        // Assert
        Assert.NotSame(descriptor1.Metadata, descriptor2.Metadata);
    }
}

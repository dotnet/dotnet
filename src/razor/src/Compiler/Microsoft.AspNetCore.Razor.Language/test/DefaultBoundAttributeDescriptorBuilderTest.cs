// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.Language;

public class DefaultBoundAttributeDescriptorBuilderTest
{
    [Fact]
    public void DisplayName_SetsDescriptorsDisplayName()
    {
        // Arrange
        var expectedDisplayName = "ExpectedDisplayName";

        var tagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");

        var builder = new BoundAttributeDescriptorBuilder(tagHelperBuilder);
        builder.DisplayName(expectedDisplayName);

        // Act
        var descriptor = builder.Build();

        // Assert
        Assert.Equal(expectedDisplayName, descriptor.DisplayName);
    }

    [Fact]
    public void DisplayName_DefaultsToPropertyLookingDisplayName()
    {
        // Arrange
        var tagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        tagHelperBuilder.Metadata(TypeName("TestTagHelper"));

        var builder = new BoundAttributeDescriptorBuilder(tagHelperBuilder);
        builder
            .TypeName(typeof(int).FullName)
            .Metadata(PropertyName("SomeProperty"));

        // Act
        var descriptor = builder.Build();

        // Assert
        Assert.Equal("int TestTagHelper.SomeProperty", descriptor.DisplayName);
    }

    [Fact]
    public void Metadata_Same()
    {
        // When SetMetadata is called on multiple builders with the same metadata collection,
        // they should share the instance.

        // Arrange
        var tagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");

        var metadata = MetadataCollection.Create(PropertyName("SomeProperty"));

        var builder1 = new BoundAttributeDescriptorBuilder(tagHelperBuilder)
        {
            TypeName = typeof(int).FullName
        };

        var builder2 = new BoundAttributeDescriptorBuilder(tagHelperBuilder)
        {
            TypeName = typeof(int).FullName
        };

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
        var tagHelperBuilder = new TagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");

        var builder1 = new BoundAttributeDescriptorBuilder(tagHelperBuilder)
        {
            TypeName = typeof(int).FullName
        };

        var builder2 = new BoundAttributeDescriptorBuilder(tagHelperBuilder)
        {
            TypeName = typeof(int).FullName
        };

        builder1.Metadata.Add(PropertyName("SomeProperty"));
        builder2.Metadata.Add(PropertyName("SomeProperty"));

        // Act
        var descriptor1 = builder1.Build();
        var descriptor2 = builder2.Build();

        // Assert
        Assert.NotSame(descriptor1.Metadata, descriptor2.Metadata);
    }
}

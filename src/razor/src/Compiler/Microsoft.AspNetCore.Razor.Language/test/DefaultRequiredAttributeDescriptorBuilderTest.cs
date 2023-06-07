// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.Language;

public class DefaultRequiredAttributeDescriptorBuilderTest
{
    [Fact]
    public void Build_DisplayNameIsName_NameComparisonFullMatch()
    {
        // Arrange
        var tagHelperBuilder = new DefaultTagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        var tagMatchingRuleBuilder = new DefaultTagMatchingRuleDescriptorBuilder(tagHelperBuilder);
        var builder = new DefaultRequiredAttributeDescriptorBuilder(tagMatchingRuleBuilder);

        builder
            .Name("asp-action")
            .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.FullMatch);

        // Act
        var descriptor = builder.Build();

        // Assert
        Assert.Equal("asp-action", descriptor.DisplayName);
    }

    [Fact]
    public void Build_DisplayNameIsNameWithDots_NameComparisonPrefixMatch()
    {
        // Arrange
        var tagHelperBuilder = new DefaultTagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        var tagMatchingRuleBuilder = new DefaultTagMatchingRuleDescriptorBuilder(tagHelperBuilder);
        var builder = new DefaultRequiredAttributeDescriptorBuilder(tagMatchingRuleBuilder);

        builder
            .Name("asp-route-")
            .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch);

        // Act
        var descriptor = builder.Build();

        // Assert
        Assert.Equal("asp-route-...", descriptor.DisplayName);
    }

    [Fact]
    public void Metadata_Same()
    {
        // When SetMetadata is called on multiple builders with the same metadata collection,
        // they should share the instance.

        // Arrange
        var tagHelperBuilder = new DefaultTagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        var tagMatchingRuleBuilder = new DefaultTagMatchingRuleDescriptorBuilder(tagHelperBuilder);

        var metadata = MetadataCollection.Create(PropertyName("SomeProperty"));

        var builder1 = new DefaultRequiredAttributeDescriptorBuilder(tagMatchingRuleBuilder);
        var builder2 = new DefaultRequiredAttributeDescriptorBuilder(tagMatchingRuleBuilder);

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
        var tagHelperBuilder = new DefaultTagHelperDescriptorBuilder(TagHelperConventions.DefaultKind, "TestTagHelper", "Test");
        var tagMatchingRuleBuilder = new DefaultTagMatchingRuleDescriptorBuilder(tagHelperBuilder);

        var builder1 = new DefaultRequiredAttributeDescriptorBuilder(tagMatchingRuleBuilder);
        var builder2 = new DefaultRequiredAttributeDescriptorBuilder(tagMatchingRuleBuilder);

        builder1.Metadata.Add(PropertyName("SomeProperty"));
        builder2.Metadata.Add(PropertyName("SomeProperty"));

        // Act
        var descriptor1 = builder1.Build();
        var descriptor2 = builder2.Build();

        // Assert
        Assert.NotSame(descriptor1.Metadata, descriptor2.Metadata);
    }
}

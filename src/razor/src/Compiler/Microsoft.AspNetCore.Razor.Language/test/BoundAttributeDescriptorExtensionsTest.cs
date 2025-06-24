// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.Language;

public class BoundAttributeDescriptorExtensionsTest
{
    [Fact]
    public void GetPropertyName_ReturnsPropertyName()
    {
        // Arrange
        var expectedPropertyName = "IntProperty";

        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName(expectedPropertyName))
                .TypeName(typeof(int).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var propertyName = boundAttribute.GetPropertyName();

        // Assert
        Assert.Equal(expectedPropertyName, propertyName);
    }

    [Fact]
    public void GetPropertyName_ReturnsNullIfNoPropertyName()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .TypeName(typeof(int).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var propertyName = boundAttribute.GetPropertyName();

        // Assert
        Assert.Null(propertyName);
    }

    [Fact]
    public void IsDefaultKind_ReturnsTrue_IfKindIsDefault()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("IntProperty"))
                .TypeName(typeof(int).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var isDefault = boundAttribute.IsDefaultKind();

        // Assert
        Assert.True(isDefault);
    }

    [Fact]
    public void IsDefaultKind_ReturnsFalse_IfKindIsNotDefault()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create("other-kind", "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("IntProperty"))
                .TypeName(typeof(int).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var isDefault = boundAttribute.IsDefaultKind();

        // Assert
        Assert.False(isDefault);
    }

    [Fact]
    public void ExpectsStringValue_ReturnsTrue_ForStringProperty()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("BoundProp"))
                .TypeName(typeof(string).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var result = boundAttribute.ExpectsStringValue("test");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ExpectsStringValue_ReturnsFalse_ForNonStringProperty()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("BoundProp"))
                .TypeName(typeof(bool).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var result = boundAttribute.ExpectsStringValue("test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ExpectsStringValue_ReturnsTrue_StringIndexerAndNameMatch()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("BoundProp"))
                .TypeName("System.Collection.Generic.IDictionary<string, string>")
                .AsDictionary("prefix-test-", typeof(string).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var result = boundAttribute.ExpectsStringValue("prefix-test-key");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ExpectsStringValue_ReturnsFalse_StringIndexerAndNameMismatch()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("BoundProp"))
                .TypeName("System.Collection.Generic.IDictionary<string, string>")
                .AsDictionary("prefix-test-", typeof(string).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var result = boundAttribute.ExpectsStringValue("test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ExpectsBooleanValue_ReturnsTrue_ForBooleanProperty()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("BoundProp"))
                .TypeName(typeof(bool).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var result = boundAttribute.ExpectsBooleanValue("test");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ExpectsBooleanValue_ReturnsFalse_ForNonBooleanProperty()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("BoundProp"))
                .TypeName(typeof(int).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var result = boundAttribute.ExpectsBooleanValue("test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ExpectsBooleanValue_ReturnsTrue_BooleanIndexerAndNameMatch()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("BoundProp"))
                .TypeName("System.Collection.Generic.IDictionary<string, bool>")
                .AsDictionary("prefix-test-", typeof(bool).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var result = boundAttribute.ExpectsBooleanValue("prefix-test-key");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ExpectsBooleanValue_ReturnsFalse_BooleanIndexerAndNameMismatch()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.Create(TagHelperConventions.DefaultKind, "TestTagHelper", "Test")
            .Metadata(TypeName("TestTagHelper"))
            .BoundAttributeDescriptor(attribute => attribute
                .Name("test")
                .Metadata(PropertyName("BoundProp"))
                .TypeName("System.Collection.Generic.IDictionary<string, bool>")
                .AsDictionary("prefix-test-", typeof(bool).FullName))
            .Build();

        var boundAttribute = Assert.Single(tagHelper.BoundAttributes);

        // Act
        var result = boundAttribute.ExpectsBooleanValue("test");

        // Assert
        Assert.False(result);
    }
}

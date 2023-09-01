// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

public class TagHelperDescriptorSerializationTest : TestBase
{
    public TagHelperDescriptorSerializationTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void TagHelperDescriptor_DefaultBlazorServerProject_RoundTrips()
    {
        // Arrange
        var expectedTagHelpers = RazorTestResources.BlazorServerAppTagHelpers;

        // Act

        using var writeStream = new MemoryStream();

        // Serialize the tag helpers to a stream
        using (var writer = new StreamWriter(writeStream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
        {
            JsonDataConvert.SerializeData(writer,
                r => r.WriteArray(expectedTagHelpers, ObjectWriters.Write));
        }

        // Deserialize the tag helpers from the stream we just serialized to.
        writeStream.Seek(0, SeekOrigin.Begin);

        ImmutableArray<TagHelperDescriptor> actualTagHelpers;

        using (var reader = new StreamReader(writeStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 4096, leaveOpen: true))
        {
            actualTagHelpers = JsonDataConvert.DeserializeData(reader,
                static r => r.ReadImmutableArray(
                    static r => ObjectReaders.ReadTagHelper(r, useCache: false)));
        }

        // Assert
        Assert.Equal(expectedTagHelpers, actualTagHelpers, TagHelperDescriptorComparer.Default);
    }

    [Fact]
    public void TagHelperDescriptor_RoundTripsProperly()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            kind: TagHelperConventions.DefaultKind,
            tagName: "tag-name",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes: new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("test-attribute")
                    .Metadata(PropertyName("TestAttribute"))
                    .TypeName("string"),
            },
            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
            {
                builder => builder
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-one")
                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch))
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-two")
                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.FullMatch)
                        .Value("something")
                        .ValueComparisonMode(RequiredAttributeDescriptor.ValueComparisonMode.PrefixMatch))
                    .RequireParentTag("parent-name")
                    .RequireTagStructure(TagStructure.WithoutEndTag),
            },
            configureAction: builder =>
            {
                builder.AllowChildTag("allowed-child-one");
                builder.Metadata("foo", "bar");
            });

        // Act
        var json = JsonDataConvert.SerializeObject(expectedDescriptor, ObjectWriters.WriteProperties);
        var descriptor = JsonDataConvert.DeserializeObject(json, static r => ObjectReaders.ReadTagHelperFromProperties(r, useCache: false));

        // Assert
        Assert.Equal(expectedDescriptor, descriptor, TagHelperDescriptorComparer.Default);
    }

    [Fact]
    public void ViewComponentTagHelperDescriptor_RoundTripsProperly()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            kind: "MVC.ViewComponent",
            tagName: "tag-name",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes: new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("test-attribute")
                    .Metadata(PropertyName("TestAttribute"))
                    .TypeName("string"),
            },
            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
            {
                builder => builder
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-one")
                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch))
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-two")
                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.FullMatch)
                        .Value("something")
                        .ValueComparisonMode(RequiredAttributeDescriptor.ValueComparisonMode.PrefixMatch))
                    .RequireParentTag("parent-name")
                    .RequireTagStructure(TagStructure.WithoutEndTag),
            },
            configureAction: builder =>
            {
                builder.AllowChildTag("allowed-child-one");
                builder.Metadata("foo", "bar");
            });

        // Act
        var json = JsonDataConvert.SerializeObject(expectedDescriptor, ObjectWriters.WriteProperties);
        var descriptor = JsonDataConvert.DeserializeObject(json, static r => ObjectReaders.ReadTagHelperFromProperties(r, useCache: false));

        // Assert
        Assert.Equal(expectedDescriptor, descriptor, TagHelperDescriptorComparer.Default);
    }

    [Fact]
    public void TagHelperDescriptor_WithDiagnostic_RoundTripsProperly()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            kind: TagHelperConventions.DefaultKind,
            tagName: "tag-name",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes: new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("test-attribute")
                    .Metadata(PropertyName("TestAttribute"))
                    .TypeName("string"),
            },
            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
            {
                builder => builder
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-one")
                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch))
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-two")
                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.FullMatch)
                        .Value("something")
                        .ValueComparisonMode(RequiredAttributeDescriptor.ValueComparisonMode.PrefixMatch))
                    .RequireParentTag("parent-name"),
            },
            configureAction: builder => builder.AllowChildTag("allowed-child-one")
                    .Metadata("foo", "bar")
                    .AddDiagnostic(RazorDiagnostic.Create(
                        new RazorDiagnosticDescriptor("id", () => "Test Message", RazorDiagnosticSeverity.Error), new SourceSpan(null, 10, 20, 30, 40))));

        // Act
        var json = JsonDataConvert.SerializeObject(expectedDescriptor, ObjectWriters.WriteProperties);
        var descriptor = JsonDataConvert.DeserializeObject(json, static r => ObjectReaders.ReadTagHelperFromProperties(r, useCache: false));

        // Assert
        Assert.Equal(expectedDescriptor, descriptor, TagHelperDescriptorComparer.Default);
    }

    [Fact]
    public void TagHelperDescriptor_WithIndexerAttributes_RoundTripsProperly()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            kind: TagHelperConventions.DefaultKind,
            tagName: "tag-name",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes: new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder => builder
                    .Name("test-attribute")
                    .Metadata(PropertyName("TestAttribute"))
                    .TypeName("SomeEnum")
                    .AsEnum()
                    .Documentation("Summary"),
                builder => builder
                    .Name("test-attribute2")
                    .Metadata(PropertyName("TestAttribute2"))
                    .TypeName("SomeDictionary")
                    .AsDictionaryAttribute("dict-prefix-", "string"),
            },
            ruleBuilders: new Action<TagMatchingRuleDescriptorBuilder>[]
            {
                builder => builder
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-one")
                        .NameComparisonMode(RequiredAttributeDescriptor.NameComparisonMode.PrefixMatch))
            },
            configureAction: builder => builder
                    .AllowChildTag("allowed-child-one")
                    .Metadata("foo", "bar")
                    .TagOutputHint("Hint"));

        // Act
        var json = JsonDataConvert.SerializeObject(expectedDescriptor, ObjectWriters.WriteProperties);
        var descriptor = JsonDataConvert.DeserializeObject(json, static r => ObjectReaders.ReadTagHelperFromProperties(r, useCache: false));

        // Assert
        Assert.Equal(expectedDescriptor, descriptor, TagHelperDescriptorComparer.Default);
    }

    [Fact]
    public void TagHelperDescriptor_WithoutEditorRequired_RoundTripsProperly()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            kind: TagHelperConventions.DefaultKind,
            tagName: "tag-name2",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes: new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder =>
                {
                    builder
                    .Name("test-attribute")
                    .Metadata(PropertyName("TestAttribute"))
                    .TypeName("string");
                },
            });

        // Act
        var json = JsonDataConvert.SerializeObject(expectedDescriptor, ObjectWriters.WriteProperties);
        var descriptor = JsonDataConvert.DeserializeObject(json, static r => ObjectReaders.ReadTagHelperFromProperties(r, useCache: false));

        // Assert
        Assert.NotNull(descriptor);
        Assert.Equal(expectedDescriptor, descriptor, TagHelperDescriptorComparer.Default);

        var boundAttribute = Assert.Single(descriptor.BoundAttributes);
        Assert.False(boundAttribute.IsEditorRequired);
    }

    [Fact]
    public void TagHelperDescriptor_WithEditorRequired_RoundTripsProperly()
    {
        // Arrange
        var expectedDescriptor = CreateTagHelperDescriptor(
            kind: TagHelperConventions.DefaultKind,
            tagName: "tag-name3",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes: new Action<BoundAttributeDescriptorBuilder>[]
            {
                builder =>
                {
                    builder
                    .Name("test-attribute")
                    .Metadata(PropertyName("TestAttribute"))
                    .TypeName("string");

                    builder.IsEditorRequired = true;
                },
            });

        // Act
        var json = JsonDataConvert.SerializeObject(expectedDescriptor, ObjectWriters.WriteProperties);
        var descriptor = JsonDataConvert.DeserializeObject(json, static r => ObjectReaders.ReadTagHelperFromProperties(r, useCache: false));

        // Assert
        Assert.NotNull(descriptor);
        Assert.Equal(expectedDescriptor, descriptor, TagHelperDescriptorComparer.Default);

        var boundAttribute = Assert.Single(descriptor.BoundAttributes);
        Assert.True(boundAttribute.IsEditorRequired);
    }

    private static TagHelperDescriptor CreateTagHelperDescriptor(
        string kind,
        string tagName,
        string typeName,
        string assemblyName,
        IEnumerable<Action<BoundAttributeDescriptorBuilder>>? attributes = null,
        IEnumerable<Action<TagMatchingRuleDescriptorBuilder>>? ruleBuilders = null,
        Action<TagHelperDescriptorBuilder>? configureAction = null)
    {
        var builder = TagHelperDescriptorBuilder.Create(kind, typeName, assemblyName);
        builder.Metadata(TypeName(typeName));

        if (attributes != null)
        {
            foreach (var attributeBuilder in attributes)
            {
                builder.BoundAttributeDescriptor(attributeBuilder);
            }
        }

        if (ruleBuilders != null)
        {
            foreach (var ruleBuilder in ruleBuilders)
            {
                builder.TagMatchingRuleDescriptor(innerRuleBuilder => {
                    innerRuleBuilder.RequireTagName(tagName);
                    ruleBuilder(innerRuleBuilder);
                });
            }
        }
        else
        {
            builder.TagMatchingRuleDescriptor(ruleBuilder => ruleBuilder.RequireTagName(tagName));
        }

        configureAction?.Invoke(builder);

        var descriptor = builder.Build();

        return descriptor;
    }
}

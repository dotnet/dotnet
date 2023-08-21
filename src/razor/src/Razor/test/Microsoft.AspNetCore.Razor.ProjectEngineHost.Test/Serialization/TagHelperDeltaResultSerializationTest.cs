// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Serialization.Converters;
using Microsoft.AspNetCore.Razor.Test.Common;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Remote.Razor.Test;

public class TagHelperDeltaResultSerializationTest(ITestOutputHelper testOutput) : TestBase(testOutput)
{
    [Fact]
    public void TagHelperResolutionResult_DefaultBlazorServerProject_RoundTrips()
    {
        // Arrange
        var bytes = RazorTestResources.GetResourceBytes(RazorTestResources.BlazorServerAppTagHelpersJson);

        ImmutableArray<TagHelperDescriptor> tagHelpers;
        using (var stream = new MemoryStream(bytes))
        using (var reader = new StreamReader(stream))
        {
            tagHelpers = JsonDataConvert.DeserializeData(reader,
                static r => r.ReadImmutableArray(
                    static r => ObjectReaders.ReadTagHelper(r, useCache: false)));
        }

        var expectedResult = new TagHelperDeltaResult(
            Delta: true,
            ResultId: 1,
            Added: tagHelpers,
            Removed: tagHelpers);

        var serializer = new JsonSerializer { Converters = { TagHelperDeltaResultJsonConverter.Instance } };

        // Act
        using var writeStream = new MemoryStream();

        // Serialize the result to a stream
        using (var writer = new StreamWriter(writeStream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
        {
            serializer.Serialize(writer, expectedResult);
        }

        // Deserialize the result from the stream we just serialized to.
        writeStream.Seek(0, SeekOrigin.Begin);

        TagHelperDeltaResult? actualResult;

        using (var reader = new StreamReader(writeStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 4096, leaveOpen: true))
        using (var jsonReader = new JsonTextReader(reader) { CloseInput = false })
        {
            actualResult = serializer.Deserialize<TagHelperDeltaResult>(jsonReader);
        }

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void TagHelperDescriptor_RoundTripsProperly()
    {
        // Arrange
        var descriptor = CreateTagHelperDescriptor(
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

        var expectedResult = new TagHelperDeltaResult(
            Delta: true,
            ResultId: 1,
            Added: ImmutableArray.Create(descriptor),
            Removed: ImmutableArray.Create(descriptor));

        // Act
        var json = JsonConvert.SerializeObject(expectedResult, TagHelperDeltaResultJsonConverter.Instance);
        var actualResult = JsonConvert.DeserializeObject<TagHelperDeltaResult>(json, TagHelperDeltaResultJsonConverter.Instance);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void ViewComponentTagHelperDescriptor_RoundTripsProperly()
    {
        // Arrange
        var descriptor = CreateTagHelperDescriptor(
            kind: ViewComponentTagHelperConventions.Kind,
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

        var expectedResult = new TagHelperDeltaResult(
            Delta: true,
            ResultId: 1,
            Added: ImmutableArray.Create(descriptor),
            Removed: ImmutableArray.Create(descriptor));

        // Act
        var json = JsonConvert.SerializeObject(expectedResult, TagHelperDeltaResultJsonConverter.Instance);
        var actualResult = JsonConvert.DeserializeObject<TagHelperDeltaResult>(json, TagHelperDeltaResultJsonConverter.Instance);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void TagHelperDescriptor_WithDiagnostic_RoundTripsProperly()
    {
        // Arrange
        var descriptor = CreateTagHelperDescriptor(
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

        var expectedResult = new TagHelperDeltaResult(
            Delta: true,
            ResultId: 1,
            Added: ImmutableArray.Create(descriptor),
            Removed: ImmutableArray.Create(descriptor));

        // Act
        var json = JsonConvert.SerializeObject(expectedResult, TagHelperDeltaResultJsonConverter.Instance);
        var actualResult = JsonConvert.DeserializeObject<TagHelperDeltaResult>(json, TagHelperDeltaResultJsonConverter.Instance);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void TagHelperDescriptor_WithIndexerAttributes_RoundTripsProperly()
    {
        // Arrange
        var descriptor = CreateTagHelperDescriptor(
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

        var expectedResult = new TagHelperDeltaResult(
            Delta: true,
            ResultId: 1,
            Added: ImmutableArray.Create(descriptor),
            Removed: ImmutableArray.Create(descriptor));

        // Act
        var json = JsonConvert.SerializeObject(expectedResult, TagHelperDeltaResultJsonConverter.Instance);
        var actualResult = JsonConvert.DeserializeObject<TagHelperDeltaResult>(json, TagHelperDeltaResultJsonConverter.Instance);

        // Assert
        Assert.Equal(expectedResult, actualResult);
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Resolvers;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

public class TagHelperDeltaResultSerializationTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private static readonly MessagePackSerializerOptions s_options = MessagePackSerializerOptions.Standard
        .WithResolver(CompositeResolver.Create(
            TagHelperDeltaResultResolver.Instance,
            StandardResolver.Instance));

    [Fact]
    public void TagHelperResolutionResult_DefaultBlazorServerProject_RoundTrips()
    {
        // Arrange
        var tagHelpers = RazorTestResources.BlazorServerAppTagHelpers;
        var checksums = tagHelpers.SelectAsArray(t => t.Checksum);

        var expectedResult = new TagHelperDeltaResult(
            IsDelta: true,
            ResultId: 1,
            Added: checksums,
            Removed: checksums);

        // Act

        // Serialize the result to bytes
        var bytes = MessagePackConvert.Serialize(expectedResult, s_options);

        // Deserialize the bytes we just serialized.
        var actualResult = MessagePackConvert.Deserialize<TagHelperDeltaResult?>(bytes, s_options);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void TagHelperDescriptor_RoundTripsProperly()
    {
        // Arrange
        var descriptor = CreateTagHelperDescriptor(
            kind: TagHelperKind.ITagHelper,
            tagName: "tag-name",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes:
            [
                builder => builder
                    .Name("test-attribute")
                    .PropertyName("TestAttribute")
                    .TypeName("string"),
            ],
            ruleBuilders:
            [
                builder => builder
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-one", RequiredAttributeNameComparison.PrefixMatch))
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-two", RequiredAttributeNameComparison.FullMatch)
                        .Value("something", RequiredAttributeValueComparison.PrefixMatch))
                    .RequireParentTag("parent-name")
                    .RequireTagStructure(TagStructure.WithoutEndTag),
            ],
            configureAction: builder =>
            {
                builder.AllowChildTag("allowed-child-one");
            });

        var expectedResult = new TagHelperDeltaResult(
            IsDelta: true,
            ResultId: 1,
            Added: [descriptor.Checksum],
            Removed: [descriptor.Checksum]);

        // Act
        var bytes = MessagePackConvert.Serialize(expectedResult, s_options);
        var actualResult = MessagePackConvert.Deserialize<TagHelperDeltaResult>(bytes, s_options);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void ViewComponentTagHelperDescriptor_RoundTripsProperly()
    {
        // Arrange
        var descriptor = CreateTagHelperDescriptor(
            kind: TagHelperKind.ViewComponent,
            tagName: "tag-name",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes:
            [
                builder => builder
                    .Name("test-attribute")
                    .PropertyName("TestAttribute")
                    .TypeName("string"),
            ],
            ruleBuilders:
            [
                builder => builder
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-one", RequiredAttributeNameComparison.PrefixMatch))
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-two", RequiredAttributeNameComparison.FullMatch)
                        .Value("something", RequiredAttributeValueComparison.PrefixMatch))
                    .RequireParentTag("parent-name")
                    .RequireTagStructure(TagStructure.WithoutEndTag),
            ],
            configureAction: builder =>
            {
                builder.AllowChildTag("allowed-child-one");
            });

        var expectedResult = new TagHelperDeltaResult(
            IsDelta: true,
            ResultId: 1,
            Added: [descriptor.Checksum],
            Removed: [descriptor.Checksum]);

        // Act
        var bytes = MessagePackConvert.Serialize(expectedResult, s_options);
        var actualResult = MessagePackConvert.Deserialize<TagHelperDeltaResult>(bytes, s_options);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void TagHelperDescriptor_WithDiagnostic_RoundTripsProperly()
    {
        // Arrange
        var descriptor = CreateTagHelperDescriptor(
            kind: TagHelperKind.ITagHelper,
            tagName: "tag-name",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes:
            [
                builder => builder
                    .Name("test-attribute")
                    .PropertyName("TestAttribute")
                    .TypeName("string"),
            ],
            ruleBuilders:
            [
                builder => builder
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-one", RequiredAttributeNameComparison.PrefixMatch))
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-two", RequiredAttributeNameComparison.FullMatch)
                        .Value("something", RequiredAttributeValueComparison.PrefixMatch))
                    .RequireParentTag("parent-name"),
            ],
            configureAction: builder => builder.AllowChildTag("allowed-child-one")
                .AddDiagnostic(RazorDiagnostic.Create(
                    new RazorDiagnosticDescriptor("id", "Test Message", RazorDiagnosticSeverity.Error), new SourceSpan(null, 10, 20, 30, 40))));

        var expectedResult = new TagHelperDeltaResult(
            IsDelta: true,
            ResultId: 1,
            Added: [descriptor.Checksum],
            Removed: [descriptor.Checksum]);

        // Act
        var bytes = MessagePackConvert.Serialize(expectedResult, s_options);
        var actualResult = MessagePackConvert.Deserialize<TagHelperDeltaResult>(bytes, s_options);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public void TagHelperDescriptor_WithIndexerAttributes_RoundTripsProperly()
    {
        // Arrange
        var descriptor = CreateTagHelperDescriptor(
            kind: TagHelperKind.ITagHelper,
            tagName: "tag-name",
            typeName: "type name",
            assemblyName: "assembly name",
            attributes:
            [
                builder => builder
                    .Name("test-attribute")
                    .PropertyName("TestAttribute")
                    .TypeName("SomeEnum")
                    .AsEnum()
                    .Documentation("Summary"),
                builder => builder
                    .Name("test-attribute2")
                    .PropertyName("TestAttribute2")
                    .TypeName("SomeDictionary")
                    .AsDictionaryAttribute("dict-prefix-", "string"),
            ],
            ruleBuilders:
            [
                builder => builder
                    .RequireAttributeDescriptor(attribute => attribute
                        .Name("required-attribute-one", RequiredAttributeNameComparison.PrefixMatch))
            ],
            configureAction: builder => builder
                .AllowChildTag("allowed-child-one")
                .TagOutputHint("Hint"));

        var expectedResult = new TagHelperDeltaResult(
            IsDelta: true,
            ResultId: 1,
            Added: [descriptor.Checksum],
            Removed: [descriptor.Checksum]);

        // Act
        var bytes = MessagePackConvert.Serialize(expectedResult, s_options);
        var actualResult = MessagePackConvert.Deserialize<TagHelperDeltaResult>(bytes, s_options);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    private static TagHelperDescriptor CreateTagHelperDescriptor(
        TagHelperKind kind,
        string tagName,
        string typeName,
        string assemblyName,
        IEnumerable<Action<BoundAttributeDescriptorBuilder>>? attributes = null,
        IEnumerable<Action<TagMatchingRuleDescriptorBuilder>>? ruleBuilders = null,
        Action<TagHelperDescriptorBuilder>? configureAction = null)
    {
        var builder = TagHelperDescriptorBuilder.CreateTagHelper(kind, typeName, assemblyName);
        builder.TypeName = typeName;

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
                builder.TagMatchingRuleDescriptor(innerRuleBuilder =>
                {
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

        return builder.Build();
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Completion;
using Microsoft.VisualStudio.Editor.Razor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class TagHelperFactsServiceTest(ITestOutputHelper testOutput) : TagHelperServiceTestBase(testOutput)
{
    [Fact]
    public void StringifyAttributes_DirectiveAttribute()
    {
        // Arrange
        var codeDocument = CreateComponentDocument($"<TestElement @test='abc' />", [.. DefaultTagHelpers]);
        var root = codeDocument.GetRequiredSyntaxRoot();
        var startTag = (MarkupTagHelperStartTagSyntax)root.FindInnermostNode(3);

        // Act
        var attributes = TagHelperFacts.StringifyAttributes(startTag.Attributes);

        // Assert
        Assert.Collection(
            attributes,
            attribute =>
            {
                Assert.Equal("@test", attribute.Key);
                Assert.Equal("abc", attribute.Value);
            });
    }

    [Fact]
    public void StringifyAttributes_DirectiveAttributeWithParameter()
    {
        // Arrange
        var codeDocument = CreateComponentDocument($"<TestElement @test:something='abc' />", [.. DefaultTagHelpers]);
        var root = codeDocument.GetRequiredSyntaxRoot();
        var startTag = (MarkupTagHelperStartTagSyntax)root.FindInnermostNode(3);

        // Act
        var attributes = TagHelperFacts.StringifyAttributes(startTag.Attributes);

        // Assert
        Assert.Collection(
            attributes,
            attribute =>
            {
                Assert.Equal("@test:something", attribute.Key);
                Assert.Equal("abc", attribute.Value);
            });
    }

    [Fact]
    public void StringifyAttributes_MinimizedDirectiveAttribute()
    {
        // Arrange
        var codeDocument = CreateComponentDocument($"<TestElement @minimized />", [.. DefaultTagHelpers]);
        var root = codeDocument.GetRequiredSyntaxRoot();
        var startTag = (MarkupTagHelperStartTagSyntax)root.FindInnermostNode(3);

        // Act
        var attributes = TagHelperFacts.StringifyAttributes(startTag.Attributes);

        // Assert
        Assert.Collection(
            attributes,
            attribute =>
            {
                Assert.Equal("@minimized", attribute.Key);
                Assert.Equal(string.Empty, attribute.Value);
            });
    }

    [Fact]
    public void StringifyAttributes_MinimizedDirectiveAttributeWithParameter()
    {
        // Arrange
        var codeDocument = CreateComponentDocument($"<TestElement @minimized:something />", [.. DefaultTagHelpers]);
        var root = codeDocument.GetRequiredSyntaxRoot();
        var startTag = (MarkupTagHelperStartTagSyntax)root.FindInnermostNode(3);

        // Act
        var attributes = TagHelperFacts.StringifyAttributes(startTag.Attributes);

        // Assert
        Assert.Collection(
            attributes,
            attribute =>
            {
                Assert.Equal("@minimized:something", attribute.Key);
                Assert.Equal(string.Empty, attribute.Value);
            });
    }

    [Fact]
    public void StringifyAttributes_TagHelperAttribute()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.CreateTagHelper("WithBoundAttribute", "TestAssembly");
        tagHelper.SetTypeName("WithBoundAttribute", typeNamespace: null, typeNameIdentifier: null);
        tagHelper.TagMatchingRule(rule => rule.TagName = "test");
        tagHelper.BindAttribute(attribute =>
        {
            attribute.Name = "bound";
            attribute.PropertyName = "Bound";
            attribute.TypeName = typeof(bool).FullName;
        });
        var codeDocument = CreateCodeDocument("""
            @addTagHelper *, TestAssembly
            <test bound='true' />
            """, isRazorFile: false, tagHelper.Build());
        var root = codeDocument.GetRequiredSyntaxRoot();
        var startTag = (MarkupTagHelperStartTagSyntax)root.FindInnermostNode(30 + Environment.NewLine.Length);

        // Act
        var attributes = TagHelperFacts.StringifyAttributes(startTag.Attributes);

        // Assert
        Assert.Collection(
            attributes,
            attribute =>
            {
                Assert.Equal("bound", attribute.Key);
                Assert.Equal("true", attribute.Value);
            });
    }

    [Fact]
    public void StringifyAttributes_MinimizedTagHelperAttribute()
    {
        // Arrange
        var tagHelper = TagHelperDescriptorBuilder.CreateTagHelper("WithBoundAttribute", "TestAssembly");
        tagHelper.SetTypeName("WithBoundAttribute", typeNamespace: null, typeNameIdentifier: null);
        tagHelper.TagMatchingRule(rule => rule.TagName = "test");
        tagHelper.BindAttribute(attribute =>
        {
            attribute.Name = "bound";
            attribute.PropertyName = "Bound";
            attribute.TypeName = typeof(bool).FullName;
        });
        var codeDocument = CreateCodeDocument("""
            @addTagHelper *, TestAssembly
            <test bound />
            """, isRazorFile: false, tagHelper.Build());
        var root = codeDocument.GetRequiredSyntaxRoot();
        var startTag = (MarkupTagHelperStartTagSyntax)root.FindInnermostNode(30 + Environment.NewLine.Length);

        // Act
        var attributes = TagHelperFacts.StringifyAttributes(startTag.Attributes);

        // Assert
        Assert.Collection(
            attributes,
            attribute =>
            {
                Assert.Equal("bound", attribute.Key);
                Assert.Equal(string.Empty, attribute.Value);
            });
    }

    [Fact]
    public void StringifyAttributes_UnboundAttribute()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("""
            @addTagHelper *, TestAssembly
            <input unbound='hello world' />
            """, isRazorFile: false, DefaultTagHelpers);
        var root = codeDocument.GetRequiredSyntaxRoot();
        var startTag = (MarkupStartTagSyntax)root.FindInnermostNode(30 + Environment.NewLine.Length);

        // Act
        var attributes = TagHelperFacts.StringifyAttributes(startTag.Attributes);

        // Assert
        Assert.Collection(
            attributes,
            attribute =>
            {
                Assert.Equal("unbound", attribute.Key);
                Assert.Equal("hello world", attribute.Value);
            });
    }

    [Fact]
    public void StringifyAttributes_UnboundMinimizedAttribute()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("""
            @addTagHelper *, TestAssembly
            <input unbound />
            """, isRazorFile: false, DefaultTagHelpers);
        var root = codeDocument.GetRequiredSyntaxRoot();
        var startTag = (MarkupStartTagSyntax)root.FindInnermostNode(30 + Environment.NewLine.Length);

        // Act
        var attributes = TagHelperFacts.StringifyAttributes(startTag.Attributes);

        // Assert
        Assert.Collection(
            attributes,
            attribute =>
            {
                Assert.Equal("unbound", attribute.Key);
                Assert.Equal(string.Empty, attribute.Value);
            });
    }

    [Fact]
    public void StringifyAttributes_IgnoresMiscContent()
    {
        // Arrange
        var codeDocument = CreateCodeDocument("""
            @addTagHelper *, TestAssembly
            <input unbound @DateTime.Now />
            """, isRazorFile: false, DefaultTagHelpers);
        var root = codeDocument.GetRequiredSyntaxRoot();
        var startTag = (MarkupStartTagSyntax)root.FindInnermostNode(30 + Environment.NewLine.Length);

        // Act
        var attributes = TagHelperFacts.StringifyAttributes(startTag.Attributes);

        // Assert
        Assert.Collection(
            attributes,
            attribute =>
            {
                Assert.Equal("unbound", attribute.Key);
                Assert.Equal(string.Empty, attribute.Value);
            });
    }

    private static RazorCodeDocument CreateComponentDocument(string text, TagHelperCollection tagHelpers)
    {
        tagHelpers ??= [];
        var sourceDocument = TestRazorSourceDocument.Create(text);
        var projectEngine = RazorProjectEngine.Create(builder =>
        {
            builder.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });

        return projectEngine.Process(sourceDocument, RazorFileKind.Component, importSources: default, tagHelpers);
    }
}

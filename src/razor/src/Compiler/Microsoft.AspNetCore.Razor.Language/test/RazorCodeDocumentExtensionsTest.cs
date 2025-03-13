// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language;

public class RazorCodeDocumentExtensionsTest
{
    [Fact]
    public void GetRazorSyntaxTree_ReturnsSyntaxTree()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = RazorSyntaxTree.Parse(codeDocument.Source);
        codeDocument.Items[typeof(RazorSyntaxTree)] = expected;

        // Act
        var actual = codeDocument.GetSyntaxTree();

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void SetRazorSyntaxTree_SetsSyntaxTree()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = RazorSyntaxTree.Parse(codeDocument.Source);

        // Act
        codeDocument.SetSyntaxTree(expected);

        // Assert
        Assert.Same(expected, codeDocument.Items[typeof(RazorSyntaxTree)]);
    }

    [Fact]
    public void GetAndSetImportSyntaxTrees_ReturnsSyntaxTrees()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var importSyntaxTree = RazorSyntaxTree.Parse(codeDocument.Source);
        codeDocument.SetImportSyntaxTrees([importSyntaxTree]);

        // Act
        var actual = codeDocument.GetImportSyntaxTrees();

        // Assert
        Assert.False(actual.IsDefault);
        Assert.Equal<RazorSyntaxTree>([importSyntaxTree], actual);
    }

    [Fact]
    public void GetAndSetTagHelpers_ReturnsTagHelpers()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = new[] { TagHelperDescriptorBuilder.Create("TestTagHelper", "TestAssembly").Build() };
        codeDocument.SetTagHelpers(expected);

        // Act
        var actual = codeDocument.GetTagHelpers();

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void GetIRDocument_ReturnsIRDocument()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = new DocumentIntermediateNode();
        codeDocument.Items[typeof(DocumentIntermediateNode)] = expected;

        // Act
        var actual = codeDocument.GetDocumentIntermediateNode();

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void SetIRDocument_SetsIRDocument()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = new DocumentIntermediateNode();

        // Act
        codeDocument.SetDocumentIntermediateNode(expected);

        // Assert
        Assert.Same(expected, codeDocument.Items[typeof(DocumentIntermediateNode)]);
    }

    [Fact]
    public void GetCSharpDocument_ReturnsCSharpDocument()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = TestRazorCSharpDocument.Create(codeDocument, "");
        codeDocument.Items[typeof(RazorCSharpDocument)] = expected;

        // Act
        var actual = codeDocument.GetCSharpDocument();

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void SetCSharpDocument_SetsCSharpDocument()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = TestRazorCSharpDocument.Create(codeDocument, "");

        // Act
        codeDocument.SetCSharpDocument(expected);

        // Assert
        Assert.Same(expected, codeDocument.Items[typeof(RazorCSharpDocument)]);
    }

    [Fact]
    public void GetTagHelperContext_ReturnsTagHelperContext()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = TagHelperDocumentContext.Create(prefix: null, tagHelpers: []);
        codeDocument.Items[typeof(TagHelperDocumentContext)] = expected;

        // Act
        var actual = codeDocument.GetTagHelperContext();

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void SetTagHelperContext_SetsTagHelperContext()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = TagHelperDocumentContext.Create(prefix: null, tagHelpers: []);

        // Act
        codeDocument.SetTagHelperContext(expected);

        // Assert
        Assert.Same(expected, codeDocument.Items[typeof(TagHelperDocumentContext)]);
    }

    [Fact]
    public void TryComputeNamespace_RootNamespaceNotSet_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(filePath: "C:\\Hello\\Test.cshtml", relativePath: "Test.cshtml");
        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default);

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryComputeNamespace_RelativePathNull_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(filePath: "C:\\Hello\\Test.cshtml", relativePath: null);
        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryComputeNamespace_FilePathNull_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(filePath: null, relativePath: "Test.cshtml");
        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryComputeNamespace_RelativePathLongerThanFilePath_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Test.cshtml",
            relativePath: "Some\\invalid\\relative\\path\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryComputeNamespace_ComputesNamespace()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("Hello.Components", @namespace);
    }

    [Fact]
    public void TryComputeNamespace_NoRootNamespaceFallback_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: false, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryComputeNamespace_SanitizesNamespaceName()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components with space\\Test$name.cshtml",
            relativePath: "\\Components with space\\Test$name.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hel?o.World"));

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("Hel_o.World.Components_with_space", @namespace);
    }

    [Fact]
    public void TryComputeNamespace_RespectsNamespaceDirective()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            content: "@namespace My.Custom.NS",
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, FileKinds.Component, builder =>
            {
                builder.Directives = [NamespaceDirective.Directive];
            }),
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello.World"));

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(source, codeDocument.ParserOptions));

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("My.Custom.NS", @namespace);
    }

    [Fact]
    public void TryComputeNamespace_RespectsImportsNamespaceDirective()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, FileKinds.Component, builder =>
            {
                builder.Directives = [NamespaceDirective.Directive];
            }),
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello.World"));

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(source, codeDocument.ParserOptions));

        var importSource = TestRazorSourceDocument.Create(
            content: "@namespace My.Custom.NS",
            filePath: "C:\\Hello\\_Imports.razor",
            relativePath: "\\_Imports.razor");

        var importSyntaxTree = RazorSyntaxTree.Parse(importSource, codeDocument.ParserOptions);
        codeDocument.SetImportSyntaxTrees([importSyntaxTree]);

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("My.Custom.NS.Components", @namespace);
    }

    [Fact]
    public void TryComputeNamespace_RespectsImportsNamespaceDirective_SameFolder()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, FileKinds.Component, builder =>
            {
                builder.Directives = [NamespaceDirective.Directive];
            }),
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello.World"));

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(source, codeDocument.ParserOptions));

        var importSource = TestRazorSourceDocument.Create(
            content: "@namespace My.Custom.NS",
            filePath: "C:\\Hello\\Components\\_Imports.razor",
            relativePath: "\\Components\\_Imports.razor");

        var importSyntaxTree = RazorSyntaxTree.Parse(importSource, codeDocument.ParserOptions);
        codeDocument.SetImportSyntaxTrees([importSyntaxTree]);

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("My.Custom.NS", @namespace);
    }

    [Fact]
    public void TryComputeNamespace_OverrideImportsNamespaceDirective()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            content: "@namespace My.Custom.OverrideNS",
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, FileKinds.Component, builder =>
            {
                builder.Directives = [NamespaceDirective.Directive];
            }));

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(source, codeDocument.ParserOptions));

        var importSource = TestRazorSourceDocument.Create(
            content: "@namespace My.Custom.NS",
            filePath: "C:\\Hello\\_Imports.razor",
            relativePath: "\\_Imports.razor");

        var importSyntaxTree = RazorSyntaxTree.Parse(importSource, codeDocument.ParserOptions);
        codeDocument.SetImportSyntaxTrees([importSyntaxTree]);

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("My.Custom.OverrideNS", @namespace);
    }

    [Theory]
    [InlineData("/", "foo.cshtml", "Base")]
    [InlineData("/", "foo/bar.cshtml", "Base.foo")]
    [InlineData("/", "foo/bar/baz.cshtml", "Base.foo.bar")]
    [InlineData("/foo/", "bar/baz.cshtml", "Base.bar")]
    [InlineData("/Foo/", "bar/baz.cshtml", "Base.bar")]
    [InlineData("c:\\", "foo.cshtml", "Base")]
    [InlineData("c:\\", "foo\\bar.cshtml", "Base.foo")]
    [InlineData("c:\\", "foo\\bar\\baz.cshtml", "Base.foo.bar")]
    [InlineData("c:\\foo\\", "bar\\baz.cshtml", "Base.bar")]
    [InlineData("c:\\Foo\\", "bar\\baz.cshtml", "Base.bar")]
    public void TryComputeNamespace_ComputesNamespaceWithSuffix(string basePath, string relativePath, string expectedNamespace)
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: Path.Combine(basePath, relativePath),
            relativePath: relativePath);

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Default.WithDirectives(NamespaceDirective.Directive));

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(source, codeDocument.ParserOptions));

        var importRelativePath = "_ViewImports.cshtml";
        var importSource = TestRazorSourceDocument.Create(
            content: "@namespace Base",
            filePath: Path.Combine(basePath, importRelativePath),
            relativePath: importRelativePath);

        var importSyntaxTree = RazorSyntaxTree.Parse(importSource, codeDocument.ParserOptions);
        codeDocument.SetImportSyntaxTrees([importSyntaxTree]);

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal(expectedNamespace, @namespace);
    }

    [Fact]
    public void TryComputeNamespace_ForNonRelatedFiles_UsesNamespaceVerbatim()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "c:\\foo\\bar\\bleh.cshtml",
            relativePath: "bar\\bleh.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Default.WithDirectives(NamespaceDirective.Directive));

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(source, codeDocument.ParserOptions));

        var importSource = TestRazorSourceDocument.Create(
            content: "@namespace Base",
            filePath: "c:\\foo\\baz\\bleh.cshtml",
            relativePath: "baz\\bleh.cshtml");

        var importSyntaxTree = RazorSyntaxTree.Parse(importSource, codeDocument.ParserOptions);
        codeDocument.SetImportSyntaxTrees([importSyntaxTree]);

        // Act
        codeDocument.TryComputeNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("Base", @namespace);
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language;

public class RazorCodeDocumentExtensionsTest
{
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
        Assert.False(actual.IsEmpty);
        Assert.Equal<RazorSyntaxTree>([importSyntaxTree], actual);
    }

    [Fact]
    public void GetAndSetTagHelpers_ReturnsTagHelpers()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        TagHelperCollection expected =
        [
            TagHelperDescriptorBuilder.CreateTagHelper("TestTagHelper", "TestAssembly").Build()
        ];

        codeDocument.SetTagHelpers(expected);

        // Act
        var actual = codeDocument.GetTagHelpers();

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void GetAndSetTagHelperContext_ReturnsTagHelperContext()
    {
        // Arrange
        var codeDocument = TestRazorCodeDocument.CreateEmpty();

        var expected = TagHelperDocumentContext.GetOrCreate(tagHelpers: []);
        codeDocument.SetTagHelperContext(expected);

        // Act
        var actual = codeDocument.GetTagHelperContext();

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public void TryGetNamespace_RootNamespaceNotSet_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(filePath: "C:\\Hello\\Test.cshtml", relativePath: "Test.cshtml");
        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default);

        // Act
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryGetNamespace_RelativePathNull_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(filePath: "C:\\Hello\\Test.cshtml", relativePath: null);
        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryGetNamespace_FilePathNull_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(filePath: null, relativePath: "Test.cshtml");
        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryGetNamespace_RelativePathLongerThanFilePath_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Test.cshtml",
            relativePath: "Some\\invalid\\relative\\path\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryGetNamespace_ComputesNamespace()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("Hello.Components", @namespace);
    }

    [Fact]
    public void TryGetNamespace_NoRootNamespaceFallback_ReturnsNull()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello"));

        // Act
        codeDocument.TryGetNamespace(fallbackToRootNamespace: false, out var @namespace);

        // Assert
        Assert.Null(@namespace);
    }

    [Fact]
    public void TryGetNamespace_SanitizesNamespaceName()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components with space\\Test$name.cshtml",
            relativePath: "\\Components with space\\Test$name.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hel?o.World"));

        // Act
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("Hel_o.World.Components_with_space", @namespace);
    }

    [Fact]
    public void TryGetNamespace_RespectsNamespaceDirective()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            content: "@namespace My.Custom.NS",
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, RazorFileKind.Component, builder =>
            {
                builder.Directives = [NamespaceDirective.Directive];
            }),
            codeGenerationOptions: RazorCodeGenerationOptions.Default.WithRootNamespace("Hello.World"));

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(source, codeDocument.ParserOptions));

        // Act
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("My.Custom.NS", @namespace);
    }

    [Fact]
    public void TryGetNamespace_RespectsImportsNamespaceDirective()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, RazorFileKind.Component, builder =>
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
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("My.Custom.NS.Components", @namespace);
    }

    [Fact]
    public void TryGetNamespace_IgnoresImportsNamespaceDirectiveWhenAsked()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");
        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, RazorFileKind.Component, builder =>
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
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, considerImports: false, out var @namespace, out _);

        // Assert
        Assert.Equal("Hello.World.Components", @namespace);
    }

    [Fact]
    public void TryGetNamespace_RespectsImportsNamespaceDirective_SameFolder()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, RazorFileKind.Component, builder =>
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
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("My.Custom.NS", @namespace);
    }

    [Fact]
    public void TryGetNamespace_OverrideImportsNamespaceDirective()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            content: "@namespace My.Custom.OverrideNS",
            filePath: "C:\\Hello\\Components\\Test.cshtml",
            relativePath: "\\Components\\Test.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, RazorFileKind.Component, builder =>
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
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("My.Custom.OverrideNS", @namespace);
    }

    [Fact]
    public void TryGetNamespace_PicksNearestImportsNamespaceDirective()
    {
        // Arrange
        var source = TestRazorSourceDocument.Create(
            filePath: "C:\\RazorPagesWebPage\\Pages\\Namespace\\Nested\\Folder\\Index.cshtml",
            relativePath: "\\Pages\\Namespace\\Nested\\Folder\\Index.cshtml");

        var codeDocument = RazorCodeDocument.Create(
            source,
            parserOptions: RazorParserOptions.Create(RazorLanguageVersion.Latest, RazorFileKind.Legacy, builder =>
            {
                builder.Directives = [NamespaceDirective.Directive];
            }));

        codeDocument.SetSyntaxTree(RazorSyntaxTree.Parse(source, codeDocument.ParserOptions));

        var importSource1 = TestRazorSourceDocument.Create(
            content: "@namespace RazorPagesWebSite.Pages",
            filePath: "C:\\RazorPagesWebPage\\Pages\\_ViewImports.cshtml",
            relativePath: "\\Pages\\_ViewImports.cshtml");

        var importSyntaxTree1 = RazorSyntaxTree.Parse(importSource1, codeDocument.ParserOptions);

        var importSource2 = TestRazorSourceDocument.Create(
            content: "@namespace CustomNamespace",
            filePath: "C:\\RazorPagesWebPage\\Pages\\Namespace\\_ViewImports.cshtml",
            relativePath: "\\Pages\\Namespace\\_ViewImports.cshtml");

        var importSyntaxTree2 = RazorSyntaxTree.Parse(importSource2, codeDocument.ParserOptions);

        codeDocument.SetImportSyntaxTrees([importSyntaxTree1, importSyntaxTree2]);

        // Act
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("CustomNamespace.Nested.Folder", @namespace);
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
    public void TryGetNamespace_ComputesNamespaceWithSuffix(string basePath, string relativePath, string expectedNamespace)
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
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal(expectedNamespace, @namespace);
    }

    [Fact]
    public void TryGetNamespace_ForNonRelatedFiles_UsesNamespaceVerbatim()
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
        codeDocument.TryGetNamespace(fallbackToRootNamespace: true, out var @namespace);

        // Assert
        Assert.Equal("Base", @namespace);
    }
}

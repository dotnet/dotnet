// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Generic;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

public class TagHelperTypeVisitorTest : TagHelperDescriptorProviderTestBase
{
    public TagHelperTypeVisitorTest() : base(AdditionalCode)
    {
        Compilation = BaseCompilation;
        ITagHelperSymbol = Compilation.GetTypeByMetadataName(TagHelperTypes.ITagHelper);
    }

    private Compilation Compilation { get; }

    private INamedTypeSymbol ITagHelperSymbol { get; }

    [Fact]
    public void IsTagHelper_PlainTagHelper_ReturnsTrue()
    {
        // Arrange
        var testVisitor = new TagHelperTypeVisitor(ITagHelperSymbol, new List<INamedTypeSymbol>());
        var tagHelperSymbol = Compilation.GetTypeByMetadataName("TestNamespace.Valid_PlainTagHelper");

        // Act
        var isTagHelper = testVisitor.IsTagHelper(tagHelperSymbol);

        // Assert
        Assert.True(isTagHelper);
    }

    [Fact]
    public void IsTagHelper_InheritedTagHelper_ReturnsTrue()
    {
        // Arrange
        var testVisitor = new TagHelperTypeVisitor(ITagHelperSymbol, new List<INamedTypeSymbol>());
        var tagHelperSymbol = Compilation.GetTypeByMetadataName("TestNamespace.Valid_InheritedTagHelper");

        // Act
        var isTagHelper = testVisitor.IsTagHelper(tagHelperSymbol);

        // Assert
        Assert.True(isTagHelper);
    }

    [Fact]
    public void IsTagHelper_AbstractTagHelper_ReturnsFalse()
    {
        // Arrange
        var testVisitor = new TagHelperTypeVisitor(ITagHelperSymbol, new List<INamedTypeSymbol>());
        var tagHelperSymbol = Compilation.GetTypeByMetadataName("TestNamespace.Invalid_AbstractTagHelper");

        // Act
        var isTagHelper = testVisitor.IsTagHelper(tagHelperSymbol);

        // Assert
        Assert.False(isTagHelper);
    }

    [Fact]
    public void IsTagHelper_GenericTagHelper_ReturnsFalse()
    {
        // Arrange
        var testVisitor = new TagHelperTypeVisitor(ITagHelperSymbol, new List<INamedTypeSymbol>());
        var tagHelperSymbol = Compilation.GetTypeByMetadataName("TestNamespace.Invalid_GenericTagHelper`1");

        // Act
        var isTagHelper = testVisitor.IsTagHelper(tagHelperSymbol);

        // Assert
        Assert.False(isTagHelper);
    }

    [Fact]
    public void IsTagHelper_InternalTagHelper_ReturnsFalse()
    {
        // Arrange
        var testVisitor = new TagHelperTypeVisitor(ITagHelperSymbol, new List<INamedTypeSymbol>());
        var tagHelperSymbol = Compilation.GetTypeByMetadataName("TestNamespace.Invalid_InternalTagHelper");

        // Act
        var isTagHelper = testVisitor.IsTagHelper(tagHelperSymbol);

        // Assert
        Assert.False(isTagHelper);
    }

    private const string AdditionalCode =
        """
        using Microsoft.AspNetCore.Razor.TagHelpers;

        namespace TestNamespace
        {
            public class Invalid_NestedPublicTagHelper : TagHelper
            {
            }

            public class Valid_NestedPublicViewComponent
            {
                public string Invoke(string foo) => null;
            }

            public abstract class Invalid_AbstractTagHelper : TagHelper
            {
            }

            public class Invalid_GenericTagHelper<T> : TagHelper
            {
            }

            internal class Invalid_InternalTagHelper : TagHelper
            {
            }

            public class Valid_PlainTagHelper : TagHelper
            {
            }

            public class Valid_InheritedTagHelper : Valid_PlainTagHelper
            {
            }
        }
        """;
}

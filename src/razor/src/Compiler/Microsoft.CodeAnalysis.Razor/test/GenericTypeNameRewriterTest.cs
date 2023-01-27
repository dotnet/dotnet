// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor;

public class GenericTypeNameRewriterTest
{
    [Theory]
    [InlineData("TItem2", "Type2")]

    // Unspecified argument -> System.Object
    [InlineData("TItem3", "System.Object")]

    // Not a type parameter
    [InlineData("TItem4", "TItem4")]

    // In a qualified name, not a type parameter
    [InlineData("TItem1.TItem2", "TItem1.TItem2")]

    // Type parameters can't have type parameters
    [InlineData("TItem1.TItem2<TItem1, TItem2, TItem3>", "TItem1.TItem2<Type1, Type2, System.Object>")]
    [InlineData("TItem2<TItem1<TItem3>, System.TItem2, RenderFragment<List<TItem1>>", "TItem2<TItem1<System.Object>, System.TItem2, RenderFragment<List<Type1>>")]

    // Tuples
    [InlineData("List<(TItem1 X, TItem2 Y)>", "List<(Type1 X, Type2 Y)>")]
    [InlineData("List<(TItem1, TItem2)>", "List<(Type1, Type2)>")]
    [InlineData("List<(TItem1/*test*/,TItem2)>", "List<(Type1/*test*/,Type2)>")]
    [InlineData("List<(TItem1/*test*/X, TItem2 Y)>", "List<(Type1/*test*/X, Type2 Y)>")]
    [InlineData("""
        List<(TItem1 X // Test
        , TItem2 Y)>
        """,
        """
        List<(Type1 X // Test
        , Type2 Y)>
        """)]
    [InlineData("""
        List<(TItem1// Test
        X, TItem2 Y)>
        """,
        """
        List<(Type1// Test
        X, Type2 Y)>
        """)]
    [InlineData("""
        List<(TItem1
        X, TItem2 Y)>
        """,
        """
        List<(Type1
        X, Type2 Y)>
        """)]
    [InlineData("""
        List<(TItem1 X /* Test
        another line */,
        TItem2 Y)>
        """,
        """
        List<(Type1 X /* Test
        another line */,
        Type2 Y)>
        """)]
    public void GenericTypeNameRewriter_CanReplaceTypeParametersWithTypeArguments(string original, string expected)
    {
        // Arrange
        var visitor = new GenericTypeNameRewriter(new Dictionary<string, string>()
            {
                { "TItem1", "Type1" },
                { "TItem2", "Type2" },
                { "TItem3", null },
            });

        // Act
        var actual = visitor.Rewrite(original);

        // Assert
        Assert.Equal(expected, actual.ToString());
    }
}

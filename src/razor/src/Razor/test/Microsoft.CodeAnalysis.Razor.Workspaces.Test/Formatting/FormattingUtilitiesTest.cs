// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor.Formatting;

public class FormattingUtilitiesTest
{
    [Theory]
    [InlineData(1, 4, 0)]
    [InlineData(2, 4, 0)]
    [InlineData(3, 4, 0)]
    [InlineData(1, 4, 1)]
    [InlineData(2, 4, 2)]
    [InlineData(3, 4, 3)]
    [InlineData(4, 8, 6)]
    public void GetIndentationLevel_Spaces(int level, int tabSize, int additional)
    {
        var input = new string(' ', level * tabSize + additional);
        var text = SourceText.From(input);

        var actual = FormattingUtilities.GetIndentationLevel(text.Lines[0], text.Length, insertSpaces: true, tabSize, out var additionalIndentation);

        Assert.Equal(level, actual);
        Assert.Equal(additional, additionalIndentation.Length);
        Assert.All(additionalIndentation, c => Assert.Equal(' ', c));
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 0)]
    [InlineData(3, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 6)]
    public void GetIndentationLevel_Tabs(int level, int additional)
    {
        var input = new string('\t', level) + new string(' ', additional);
        var text = SourceText.From(input);

        var actual = FormattingUtilities.GetIndentationLevel(text.Lines[0], text.Length, insertSpaces: false, tabSize: 4, out var additionalIndentation);

        Assert.Equal(level, actual);
        Assert.Equal(additional, additionalIndentation.Length);
        Assert.All(additionalIndentation, c => Assert.Equal(' ', c));
    }

    [Theory]
    [InlineData("0123456789")]
    [InlineData("0 1 2 3 4  56789")]
    [InlineData("01234\r\n    56789")]
    [InlineData("012345\n    6789")]
    [InlineData("\t\t\t012345\r\n\t\t\t6789       ")]
    public void CountNonWhitespaceCharacters(string input)
    {
        var text = SourceText.From(input);
        Assert.Equal(10, FormattingUtilities.CountNonWhitespaceChars(text, 0, text.Lines[^1].End));
    }

    [Fact]
    public void ContentEqualIgnoringWhitespace()
    {
        TestCode input1 = """
            public class C
            {
                [|public void M() { }|]
            }
            """;

        TestCode input2 = """
            public class C
            {
                [|public void M()
                {
                }|]
            }
            """;

        Assert.True(SourceText.From(input1.Text).NonWhitespaceContentEquals(SourceText.From(input2.Text),
            input1.Span.Start, input1.Span.End,
            input2.Span.Start, input2.Span.End));
    }

    [Fact]
    public void ContentEqualIgnoringWhitespace_ChangedCode()
    {
        TestCode input1 = """
            public class C
            {
                [|public void M() { }|]
            }
            """;

        TestCode input2 = """
            public class C
            {
                [|public void M()|]
            }
            """;

        Assert.False(SourceText.From(input1.Text).NonWhitespaceContentEquals(SourceText.From(input2.Text),
            input1.Span.Start, input1.Span.End,
            input2.Span.Start, input2.Span.End));
    }
}

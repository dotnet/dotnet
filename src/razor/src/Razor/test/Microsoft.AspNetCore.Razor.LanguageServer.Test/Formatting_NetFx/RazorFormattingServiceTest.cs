// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public class RazorFormattingServiceTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void MergeEdits_ReturnsSingleEditAsExpected()
    {
        // Arrange
        var source = @"
@code {
public class Foo{}
}
";
        var sourceText = SourceText.From(source);
        var edits = new[]
        {
            VsLspFactory.CreateTextEdit(VsLspFactory.CreateSingleLineRange(line: 2, character: 13, length: 3), "Bar"),
            VsLspFactory.CreateTextEdit(2, 0, "    ")
        };

        // Act
        var collapsedEdit = RazorFormattingService.MergeEdits(edits, sourceText);

        // Assert
        var multiEditChange = sourceText.WithChanges(edits.Select(sourceText.GetTextChange));
        var singleEditChange = sourceText.WithChanges(sourceText.GetTextChange(collapsedEdit));

        Assert.Equal(multiEditChange.ToString(), singleEditChange.ToString());
    }

    [Fact]
    public void AllTriggerCharacters_IncludesCSharpTriggerCharacters()
    {
        foreach (var character in RazorFormattingService.TestAccessor.GetCSharpTriggerCharacterSet())
        {
            Assert.Contains(character, RazorFormattingService.AllTriggerCharacterSet);
        }
    }

    [Fact]
    public void AllTriggerCharacters_IncludesHtmlTriggerCharacters()
    {
        foreach (var character in RazorFormattingService.TestAccessor.GetHtmlTriggerCharacterSet())
        {
            Assert.Contains(character, RazorFormattingService.AllTriggerCharacterSet);
        }
    }
}

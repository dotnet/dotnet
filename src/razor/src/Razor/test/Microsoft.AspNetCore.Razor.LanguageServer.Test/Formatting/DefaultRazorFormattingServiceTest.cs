// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Linq;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

public class DefaultRazorFormattingServiceTest : TestBase
{
    public DefaultRazorFormattingServiceTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

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
            new TextEdit()
            {
                NewText = "Bar",
                Range = new Range{ Start = new Position(2, 13), End = new Position(2, 16) }
            },
            new TextEdit()
            {
                NewText = "    ",
                Range = new Range{Start = new Position(2, 0),End = new Position(2, 0)}
            },
        };

        // Act
        var collapsedEdit = DefaultRazorFormattingService.MergeEdits(edits, sourceText);

        // Assert
        var multiEditChange = sourceText.WithChanges(edits.Select(e => e.AsTextChange(sourceText)));
        var singleEditChange = sourceText.WithChanges(collapsedEdit.AsTextChange(sourceText));

        Assert.Equal(multiEditChange.ToString(), singleEditChange.ToString());
    }
}

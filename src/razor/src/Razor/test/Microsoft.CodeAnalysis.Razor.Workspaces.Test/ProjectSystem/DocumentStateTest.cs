// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

public class DocumentStateTest : ToolingTestBase
{
    private readonly HostDocument _hostDocument;
    private readonly TextLoader _textLoader;
    private readonly SourceText _text;

    public DocumentStateTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        _hostDocument = TestProjectData.SomeProjectFile1;
        _text = SourceText.From("Hello, world!");
        _textLoader = TestMocks.CreateTextLoader(_text);
    }

    [Fact]
    public async Task DocumentState_CreatedNew_HasEmptyText()
    {
        // Arrange & Act
        var state = DocumentState.Create(_hostDocument, EmptyTextLoader.Instance);

        // Assert
        var text = await state.GetTextAsync(DisposalToken);
        Assert.Equal(0, text.Length);
    }

    [Fact]
    public async Task DocumentState_WithText_CreatesNewState()
    {
        // Arrange
        var original = DocumentState.Create(_hostDocument, EmptyTextLoader.Instance);

        // Act
        var state = original.WithText(_text, VersionStamp.Create());

        // Assert
        var text = await state.GetTextAsync(DisposalToken);
        Assert.Same(_text, text);
    }

    [Fact]
    public async Task DocumentState_WithTextLoader_CreatesNewState()
    {
        // Arrange
        var original = DocumentState.Create(_hostDocument, EmptyTextLoader.Instance);

        // Act
        var state = original.WithTextLoader(_textLoader);

        // Assert
        var text = await state.GetTextAsync(DisposalToken);
        Assert.Same(_text, text);
    }
}

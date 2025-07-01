// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

public class RazorDirectiveAttributeCommitManagerTest(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    [Fact]
    public void ShouldCommitCompletion_NoCompletionItemKinds_ReturnsFalse()
    {
        // Arrange
        var manager = new RazorDirectiveAttributeCommitManager();
        var properties = new PropertyCollection();
        var session = StrictMock.Of<IAsyncCompletionSession>(s =>
            s.Properties == properties);

        // Act
        var result = manager.ShouldCommitCompletion(session, location: default, typedChar: '=', token: default);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShouldCommitCompletion_CompletionItemKinds_ReturnsTrue()
    {
        // Arrange
        var manager = new RazorDirectiveAttributeCommitManager();
        var properties = new PropertyCollection();
        properties.SetCompletionItemKinds(new HashSet<RazorCompletionItemKind>() { RazorCompletionItemKind.DirectiveAttribute });
        var session = StrictMock.Of<IAsyncCompletionSession>(s =>
            s.Properties == properties);

        // Act
        var result = manager.ShouldCommitCompletion(session, location: default, typedChar: '=', token: default);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShouldCommitCompletion_DirectiveParameterCompletion_ColonCommit_ReturnsFalse()
    {
        // Arrange
        var manager = new RazorDirectiveAttributeCommitManager();
        var properties = new PropertyCollection();
        properties.SetCompletionItemKinds(new HashSet<RazorCompletionItemKind>() { RazorCompletionItemKind.DirectiveAttributeParameter });
        var session = StrictMock.Of<IAsyncCompletionSession>(
            s => s.Properties == properties);

        // Act
        var result = manager.ShouldCommitCompletion(session, location: default, typedChar: ':', token: default);

        // Assert
        Assert.False(result);
    }
}

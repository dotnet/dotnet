// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Utilities;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor.Completion;

public class RazorDirectiveAttributeCommitManagerTest : TestBase
{
    public RazorDirectiveAttributeCommitManagerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [Fact]
    public void ShouldCommitCompletion_NoCompletionItemKinds_ReturnsFalse()
    {
        // Arrange
        var manager = new RazorDirectiveAttributeCommitManager();
        var properties = new PropertyCollection();
        var session = Mock.Of<IAsyncCompletionSession>(s => s.Properties == properties, MockBehavior.Strict);

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
        var session = Mock.Of<IAsyncCompletionSession>(s => s.Properties == properties, MockBehavior.Strict);

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
        var session = Mock.Of<IAsyncCompletionSession>(s => s.Properties == properties, MockBehavior.Strict);

        // Act
        var result = manager.ShouldCommitCompletion(session, location: default, typedChar: ':', token: default);

        // Assert
        Assert.False(result);
    }
}

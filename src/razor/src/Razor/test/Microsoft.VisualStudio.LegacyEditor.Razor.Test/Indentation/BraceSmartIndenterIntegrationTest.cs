// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Indentation;

public class BraceSmartIndenterIntegrationTest(ITestOutputHelper testOutput) : BraceSmartIndenterTestBase(testOutput)
{
    [UIFact]
    public void TextBuffer_OnPostChanged_IndentIsInBetweenBraces_BaseIndentation()
    {
        // Arrange
        var change = Environment.NewLine;
        var initialSnapshot = new StringTextSnapshot("@{ }");
        var afterChangeSnapshot = new StringTextSnapshot("@{ " + change + "}");
        var edit = new TestEdit(3, 0, initialSnapshot, afterChangeSnapshot, change);
        var expectedIndentResult = "@{ " + change + change + "}";

        var caret = CreateCaretFrom(3 + change.Length, afterChangeSnapshot);
        TestTextBuffer? textBuffer = null;
        var focusedTextView = CreateFocusedTextView(() => textBuffer.AssumeNotNull(), caret);
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);
        var editorOperationsFactory = CreateOperationsFactoryService();
        using var braceSmartIndenter = new BraceSmartIndenter(documentTracker, editorOperationsFactory, JoinableTaskFactory.Context);

        // Act
        textBuffer.ApplyEdit(edit);

        // Assert
        Assert.Equal(expectedIndentResult, ((StringTextSnapshot)textBuffer.CurrentSnapshot).Content);
    }

    [UIFact]
    public void TextBuffer_OnPostChanged_IndentIsInBetweenBraces_OneLevelOfIndentation()
    {
        // Arrange
        var change = "\r";
        var initialSnapshot = new StringTextSnapshot("    @{ }");
        var afterChangeSnapshot = new StringTextSnapshot("    @{ " + change + "}");
        var edit = new TestEdit(7, 0, initialSnapshot, afterChangeSnapshot, change);
        var expectedIndentResult = "    @{ " + change + change + "    }";

        var caret = CreateCaretFrom(7 + change.Length, afterChangeSnapshot);
        TestTextBuffer? textBuffer = null;
        var focusedTextView = CreateFocusedTextView(() => textBuffer.AssumeNotNull(), caret);
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);
        var editorOperationsFactory = CreateOperationsFactoryService();
        using var braceSmartIndenter = new BraceSmartIndenter(documentTracker, editorOperationsFactory, JoinableTaskFactory.Context);

        // Act
        textBuffer.ApplyEdit(edit);

        // Assert
        Assert.Equal(expectedIndentResult, ((StringTextSnapshot)textBuffer.CurrentSnapshot).Content);
    }

    [UIFact]
    public void TextBuffer_OnPostChanged_IndentIsInBetweenDirectiveBlockBraces()
    {
        // Arrange
        var change = Environment.NewLine;
        var initialSnapshot = new StringTextSnapshot("    @functions {}");
        var afterChangeSnapshot = new StringTextSnapshot("    @functions {" + change + "}");
        var edit = new TestEdit(16, 0, initialSnapshot, afterChangeSnapshot, change);
        var expectedIndentResult = "    @functions {" + change + change + "    }";

        var caret = CreateCaretFrom(16 + change.Length, afterChangeSnapshot);
        TestTextBuffer? textBuffer = null;
        var focusedTextView = CreateFocusedTextView(() => textBuffer.AssumeNotNull(), caret);
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);
        var editorOperationsFactory = CreateOperationsFactoryService();
        using var braceSmartIndenter = new BraceSmartIndenter(documentTracker, editorOperationsFactory, JoinableTaskFactory.Context);

        // Act
        textBuffer.ApplyEdit(edit);

        // Assert
        Assert.Equal(expectedIndentResult, ((StringTextSnapshot)textBuffer.CurrentSnapshot).Content);
    }

    [UIFact]
    public void TextBuffer_OnPostChanged_DoesNotIndentJavaScript()
    {
        // Arrange
        var change = Environment.NewLine;
        var initialSnapshot = new StringTextSnapshot("    <script>function foo() {}</script>");
        var afterChangeSnapshot = new StringTextSnapshot("    <script>function foo() {" + change + "}</script>");
        var edit = new TestEdit(28, 0, initialSnapshot, afterChangeSnapshot, change);

        var caret = CreateCaretFrom(28 + change.Length, afterChangeSnapshot);
        TestTextBuffer? textBuffer = null;
        var focusedTextView = CreateFocusedTextView(() => textBuffer.AssumeNotNull(), caret);
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);
        var editorOperationsFactory = CreateOperationsFactoryService();
        using var braceSmartIndenter = new BraceSmartIndenter(documentTracker, editorOperationsFactory, JoinableTaskFactory.Context);

        // Act
        textBuffer.ApplyEdit(edit);

        // Assert
        Assert.Equal(afterChangeSnapshot.Content, ((StringTextSnapshot)textBuffer.CurrentSnapshot).Content);
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Moq;
using Xunit;
using Xunit.Abstractions;
using ITextBuffer = Microsoft.VisualStudio.Text.ITextBuffer;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Indentation;

public class BraceSmartIndenterTest(ITestOutputHelper testOutput) : BraceSmartIndenterTestBase(testOutput)
{
    private static readonly RazorParserOptions s_defaultOptions = RazorParserOptions.Default.WithFlags(enableSpanEditHandlers: true);

    [Fact]
    public void AtApplicableRazorBlock_NestedIfBlock_ReturnsFalse()
    {
        // Arrange
        var syntaxTree = GetSyntaxTree("@{ if (true) { } }");

        // Act
        var result = BraceSmartIndenter.AtApplicableRazorBlock(14, syntaxTree);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AtApplicableRazorBlock_SectionBlock_ReturnsTrue()
    {
        // Arrange
        var syntaxTree = GetSyntaxTree("@section Foo { }");

        // Act
        var result = BraceSmartIndenter.AtApplicableRazorBlock(15, syntaxTree);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AtApplicableRazorBlock_FunctionsBlock_ReturnsTrue()
    {
        // Arrange
        var syntaxTree = GetSyntaxTree("@functions { }");

        // Act
        var result = BraceSmartIndenter.AtApplicableRazorBlock(13, syntaxTree);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AtApplicableRazorBlock_ExplicitCodeBlock_ReturnsTrue()
    {
        // Arrange
        var syntaxTree = GetSyntaxTree("@{ }");

        // Act
        var result = BraceSmartIndenter.AtApplicableRazorBlock(3, syntaxTree);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsInvalidContent_NewLineSpan_ReturnsFalse()
    {
        // Arrange
        var span = ExtractSpan(2, """
            @{
            }
            """);
        Assert.NotNull(span);

        // Act
        var result = BraceSmartIndenter.ContainsInvalidContent(span);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsInvalidContent_WhitespaceSpan_ReturnsFalse()
    {
        // Arrange
        var span = ExtractSpan(2, "@{ }");
        Assert.NotNull(span);

        // Act
        var result = BraceSmartIndenter.ContainsInvalidContent(span);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsInvalidContent_MarkerSpan_ReturnsFalse()
    {
        // Arrange
        var span = ExtractSpan(3, "@{}");
        Assert.NotNull(span);

        // Act
        var result = BraceSmartIndenter.ContainsInvalidContent(span);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsInvalidContent_NonWhitespaceMarker_ReturnsTrue()
    {
        // Arrange
        var span = ExtractSpan(2, "@{ if}");
        Assert.NotNull(span);

        // Act
        var result = BraceSmartIndenter.ContainsInvalidContent(span);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUnlinkedSpan_NullPrevious_ReturnsTrue()
    {
        // Arrange
        var span = ExtractSpan(0, "@{}");

        // Act
        var result = BraceSmartIndenter.IsUnlinkedSpan(span);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUnlinkedSpan_NullNext_ReturnsTrue()
    {
        // Arrange
        var span = ExtractSpan(3, "@{}");

        // Act
        var result = BraceSmartIndenter.IsUnlinkedSpan(span);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUnlinkedSpan_NullOwner_ReturnsTrue()
    {
        // Arrange
        SyntaxNode? owner = null;

        // Act
        var result = BraceSmartIndenter.IsUnlinkedSpan(owner);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SurroundedByInvalidContent_MetacodeSurroundings_ReturnsFalse()
    {
        // Arrange
        var span = ExtractSpan(2, "@{}");
        Assert.NotNull(span);

        // Act
        var result = BraceSmartIndenter.SurroundedByInvalidContent(span);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SurroundedByInvalidContent_OnlyNextMetacode_ReturnsTrue()
    {
        // Arrange
        var span = ExtractSpan(9, "@{<p></p>}");
        Assert.NotNull(span);

        // Act
        var result = BraceSmartIndenter.SurroundedByInvalidContent(span);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SurroundedByInvalidContent_OnlyPreviousMetacode_ReturnsTrue()
    {
        // Arrange
        var span = ExtractSpan(2, "@{<p>");
        Assert.NotNull(span);

        // Act
        var result = BraceSmartIndenter.SurroundedByInvalidContent(span);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AtApplicableRazorBlock_AtMarkup_ReturnsFalse()
    {
        // Arrange
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create("<p></p>"), s_defaultOptions);
        var changePosition = 2;

        // Act
        var result = BraceSmartIndenter.AtApplicableRazorBlock(changePosition, syntaxTree);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AtApplicableRazorBlock_AtExplicitCodeBlocksCode_ReturnsTrue()
    {
        // Arrange
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create("@{}"), s_defaultOptions);
        var changePosition = 2;

        // Act
        var result = BraceSmartIndenter.AtApplicableRazorBlock(changePosition, syntaxTree);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AtApplicableRazorBlock_AtMetacode_ReturnsTrue()
    {
        // Arrange
        var parseOptions = RazorParserOptions.Default
            .WithDirectives(FunctionsDirective.Directive)
            .WithFlags(enableSpanEditHandlers: true);

        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create("@functions {}"), parseOptions);
        var changePosition = 12;

        // Act
        var result = BraceSmartIndenter.AtApplicableRazorBlock(changePosition, syntaxTree);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AtApplicableRazorBlock_WhenNoOwner_ReturnsFalse()
    {
        // Arrange
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create("@DateTime.Now"), s_defaultOptions);
        var changePosition = 14; // 1 after the end of the content

        // Act
        var result = BraceSmartIndenter.AtApplicableRazorBlock(changePosition, syntaxTree);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void InsertIndent_InsertsProvidedIndentIntoBuffer()
    {
        // Arrange
        var initialSnapshot = new StringTextSnapshot("@{ \n}");
        var expectedIndentResult = "@{ anything\n}";
        ITextBuffer? textBuffer = null;
        var textView = CreateFocusedTextView(() => textBuffer.AssumeNotNull());
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), textView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);

        // Act
        BraceSmartIndenter.InsertIndent(3, "anything", textBuffer);

        // Assert
        Assert.Equal(expectedIndentResult, ((StringTextSnapshot)textBuffer.CurrentSnapshot).Content);
    }

    [Fact]
    public void RestoreCaretTo_PlacesCursorAtProvidedPosition()
    {
        // Arrange
        var initialSnapshot = new StringTextSnapshot("@{ \n\n}");
        var bufferPosition = new VirtualSnapshotPoint(initialSnapshot, 4);
        var caretMock = new StrictMock<ITextCaret>();
        caretMock
            .Setup(c => c.MoveTo(It.IsAny<SnapshotPoint>()))
            .Callback<SnapshotPoint>(point =>
            {
                Assert.Equal(3, point.Position);
                Assert.Same(initialSnapshot, point.Snapshot);
            }).Returns(new CaretPosition(bufferPosition, StrictMock.Of<IMappingPoint>(), PositionAffinity.Predecessor));
        ITextBuffer? textBuffer = null;
        var textView = CreateFocusedTextView(() => textBuffer.AssumeNotNull(), caretMock.Object);
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), textView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);

        // Act
        BraceSmartIndenter.RestoreCaretTo(3, textView);

        // Assert
        caretMock.VerifyAll();
    }

    [Fact]
    public void TriggerSmartIndent_ForcesEditorToMoveToEndOfLine()
    {
        // Arrange
        var textView = CreateFocusedTextView();
        var editorOperationsMock = new StrictMock<IEditorOperations>();
        editorOperationsMock.Setup(operations => operations.MoveToEndOfLine(false)).Verifiable();
        var editorOperationsFactoryMock = new StrictMock<IEditorOperationsFactoryService>();
        var documentTracker = CreateDocumentTracker(
            () => VsMocks.CreateTextBuffer(VsMocks.ContentTypes.LegacyRazorCore),
            textView);
        editorOperationsFactoryMock
            .Setup(factory => factory.GetEditorOperations(textView))
            .Returns(editorOperationsMock.Object);
        using var smartIndenter = new BraceSmartIndenter(documentTracker, editorOperationsFactoryMock.Object, JoinableTaskFactory.Context);

        // Act
        smartIndenter.TriggerSmartIndent(textView);

        // Assert
        editorOperationsMock.VerifyAll();
    }

    [Fact]
    public void AfterClosingBrace_ContentAfterBrace_ReturnsFalse()
    {
        // Arrange
        var fileSnapshot = new StringTextSnapshot("@functions\n{a\n}");
        var changePosition = 13;
        var line = fileSnapshot.GetLineFromPosition(changePosition);

        // Act & Assert
        Assert.False(BraceSmartIndenter.BeforeClosingBrace(0, line));
    }

    [Theory]
    [InlineData("@functions\n{\n}")]
    [InlineData("@functions\n{   \n}")]
    [InlineData("@functions\n  {   \n}")]
    [InlineData("@functions\n\t\t{\t\t\n}")]
    public void AfterClosingBrace_BraceBeforePosition_ReturnsTrue(string fileContent)
    {
        // Arrange
        var fileSnapshot = new StringTextSnapshot(fileContent);
        var changePosition = fileContent.Length - 3 /* \n} */;
        var line = fileSnapshot.GetLineFromPosition(changePosition);

        // Act & Assert
        Assert.True(BraceSmartIndenter.AfterOpeningBrace(line.Length - 1, line));
    }

    [Fact]
    public void BeforeClosingBrace_ContentPriorToBrace_ReturnsFalse()
    {
        // Arrange
        var fileSnapshot = new StringTextSnapshot("@functions\n{\na}");
        var changePosition = 12;
        var line = fileSnapshot.GetLineFromPosition(changePosition + 1 /* \n */);

        // Act & Assert
        Assert.False(BraceSmartIndenter.BeforeClosingBrace(0, line));
    }

    [Theory]
    [InlineData("@functions\n{\n}")]
    [InlineData("@functions\n{\n   }")]
    [InlineData("@functions\n{\n   }   ")]
    [InlineData("@functions\n{\n\t\t   }   ")]
    public void BeforeClosingBrace_BraceAfterPosition_ReturnsTrue(string fileContent)
    {
        // Arrange
        var fileSnapshot = new StringTextSnapshot(fileContent);
        var changePosition = 12;
        var line = fileSnapshot.GetLineFromPosition(changePosition + 1 /* \n */);

        // Act & Assert
        Assert.True(BraceSmartIndenter.BeforeClosingBrace(0, line));
    }

    [UIFact]
    public void TextBuffer_OnChanged_NoopsIfNoChanges()
    {
        // Arrange
        var editorOperationsFactory = StrictMock.Of<IEditorOperationsFactoryService>();
        var changeCollection = new TestTextChangeCollection();
        var textContentChangeArgs = new TestTextContentChangedEventArgs(changeCollection);
        var documentTracker = CreateDocumentTracker(
            () => VsMocks.CreateTextBuffer(VsMocks.ContentTypes.LegacyRazorCore),
            StrictMock.Of<ITextView>());
        using var braceSmartIndenter = new BraceSmartIndenter(documentTracker, editorOperationsFactory, JoinableTaskFactory.Context);

        // Act & Assert
        braceSmartIndenter.TextBuffer_OnChanged(null, textContentChangeArgs);
    }

    [UIFact]
    public void TextBuffer_OnChanged_NoopsIfChangesThatResultInNoChange()
    {
        // Arrange
        var initialSnapshot = new StringTextSnapshot("Hello World");
        var textBuffer = new TestTextBuffer(initialSnapshot);
        textBuffer.ChangeContentType(VsMocks.ContentTypes.LegacyRazorCore, editTag: null);
        var edit = new TestEdit(0, 0, initialSnapshot, initialSnapshot, string.Empty);
        var editorOperationsFactory = StrictMock.Of<IEditorOperationsFactoryService>();
        var documentTracker = CreateDocumentTracker(() => textBuffer, StrictMock.Of<ITextView>());
        using var braceSmartIndenter = new BraceSmartIndenter(documentTracker, editorOperationsFactory, JoinableTaskFactory.Context);

        // Act & Assert
        textBuffer.ApplyEdits(edit, edit);
    }

    [Fact]
    public void TryCreateIndentationContext_ReturnsFalseIfNoFocusedTextView()
    {
        // Arrange
        var snapshot = new StringTextSnapshot("""
            
            Hello World
            """);
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create(snapshot.Content));
        ITextBuffer? textBuffer = null;
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView: null);
        textBuffer = CreateTextBuffer(snapshot, documentTracker);

        // Act
        var result = BraceSmartIndenter.TryCreateIndentationContext(0, Environment.NewLine.Length, Environment.NewLine, syntaxTree, documentTracker, out var context);

        // Assert
        Assert.Null(context);
        Assert.False(result);
    }

    [Fact]
    public void TryCreateIndentationContext_ReturnsFalseIfTextChangeIsNotNewline()
    {
        // Arrange
        var snapshot = new StringTextSnapshot("This Hello World");
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create(snapshot.Content));
        ITextBuffer? textBuffer = null;
        var focusedTextView = CreateFocusedTextView(() => textBuffer.AssumeNotNull());
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView);
        textBuffer = CreateTextBuffer(snapshot, documentTracker);

        // Act
        var result = BraceSmartIndenter.TryCreateIndentationContext(0, 5, "This ", syntaxTree, documentTracker, out var context);

        // Assert
        Assert.Null(context);
        Assert.False(result);
    }

    [Fact]
    public void TryCreateIndentationContext_ReturnsFalseIfNewLineIsNotPrecededByOpenBrace_FileStart()
    {
        // Arrange
        var initialSnapshot = new StringTextSnapshot("""
            
            Hello World
            """);
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create(initialSnapshot.Content));
        ITextBuffer? textBuffer = null;
        var focusedTextView = CreateFocusedTextView(() => textBuffer.AssumeNotNull());
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);

        // Act
        var result = BraceSmartIndenter.TryCreateIndentationContext(0, Environment.NewLine.Length, Environment.NewLine, syntaxTree, documentTracker, out var context);

        // Assert
        Assert.Null(context);
        Assert.False(result);
    }

    [Fact]
    public void TryCreateIndentationContext_ReturnsFalseIfNewLineIsNotPrecededByOpenBrace_MidFile()
    {
        // Arrange
        var initialSnapshot = new StringTextSnapshot("Hello\u0085World");
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create(initialSnapshot.Content));
        ITextBuffer? textBuffer = null;
        var focusedTextView = CreateFocusedTextView(() => textBuffer.AssumeNotNull());
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);

        // Act
        var result = BraceSmartIndenter.TryCreateIndentationContext(5, 1, "\u0085", syntaxTree, documentTracker, out var context);

        // Assert
        Assert.Null(context);
        Assert.False(result);
    }

    [Fact]
    public void TryCreateIndentationContext_ReturnsFalseIfNewLineIsNotFollowedByCloseBrace()
    {
        // Arrange
        var initialSnapshot = new StringTextSnapshot("""
            @{ 
            World
            """);
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create(initialSnapshot.Content));
        ITextBuffer? textBuffer = null;
        var focusedTextView = CreateFocusedTextView(() => textBuffer.AssumeNotNull());
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);

        // Act
        var result = BraceSmartIndenter.TryCreateIndentationContext(3, Environment.NewLine.Length, Environment.NewLine, syntaxTree, documentTracker, out var context);

        // Assert
        Assert.Null(context);
        Assert.False(result);
    }

    [Fact]
    public void TryCreateIndentationContext_ReturnsTrueIfNewLineIsSurroundedByBraces()
    {
        // Arrange
        var initialSnapshot = new StringTextSnapshot("@{ \n}");
        var syntaxTree = RazorSyntaxTree.Parse(TestRazorSourceDocument.Create(initialSnapshot.Content));
        ITextBuffer? textBuffer = null;
        var focusedTextView = CreateFocusedTextView(() => textBuffer.AssumeNotNull());
        var documentTracker = CreateDocumentTracker(() => textBuffer.AssumeNotNull(), focusedTextView);
        textBuffer = CreateTextBuffer(initialSnapshot, documentTracker);

        // Act
        var result = BraceSmartIndenter.TryCreateIndentationContext(3, 1, "\n", syntaxTree, documentTracker, out var context);

        // Assert
        Assert.NotNull(context);
        Assert.Same(focusedTextView, context.FocusedTextView);
        Assert.Equal(3, context.ChangePosition);
        Assert.True(result);
    }

    private static RazorSyntaxTree GetSyntaxTree(string content)
    {
        var syntaxTree = RazorSyntaxTree.Parse(
            TestRazorSourceDocument.Create(content),
            RazorParserOptions.Default
                .WithDirectives(FunctionsDirective.Directive, SectionDirective.Directive)
                .WithFlags(enableSpanEditHandlers: true));

        return syntaxTree;
    }

    private static SyntaxNode? ExtractSpan(int spanLocation, string content)
    {
        var syntaxTree = GetSyntaxTree(content);
#pragma warning disable CS0618 // Type or member is obsolete
        var span = syntaxTree.Root.LocateOwner(new SourceChange(new SourceSpan(spanLocation, 0), string.Empty));
#pragma warning restore CS0618 // Type or member is obsolete
        return span;
    }

    protected class TestTextContentChangedEventArgs(INormalizedTextChangeCollection changeCollection)
        : TextContentChangedEventArgs(
            CreateBeforeSnapshot(changeCollection),
            StrictMock.Of<ITextSnapshot>(),
            EditOptions.DefaultMinimalChange,
            null)
    {
        protected static ITextSnapshot CreateBeforeSnapshot(INormalizedTextChangeCollection collection)
        {
            var version = StrictMock.Of<ITextVersion>(x =>
                x.Changes == collection);
            var snapshot = StrictMock.Of<ITextSnapshot>(x =>
                x.Version == version);

            return snapshot;
        }
    }

    protected class TestTextChangeCollection : INormalizedTextChangeCollection
    {
        private readonly List<ITextChange> _changes = [];

        public ITextChange this[int index]
        {
            get => _changes[index];
            set => _changes[index] = value;
        }

        public bool IncludesLineChanges => throw new NotImplementedException();
        public int Count => _changes.Count;
        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(ITextChange item) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();
        public bool Contains(ITextChange item) => throw new NotImplementedException();
        public void CopyTo(ITextChange[] array, int arrayIndex) => throw new NotImplementedException();
        public IEnumerator<ITextChange> GetEnumerator() => throw new NotImplementedException();
        public int IndexOf(ITextChange item) => throw new NotImplementedException();
        public void Insert(int index, ITextChange item) => throw new NotImplementedException();
        public bool Remove(ITextChange item) => throw new NotImplementedException();
        public void RemoveAt(int index) => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Moq;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Indentation;

public class BraceSmartIndenterTestBase(ITestOutputHelper testOutput) : VisualStudioTestBase(testOutput)
{
    private protected static IVisualStudioDocumentTracker CreateDocumentTracker(Func<ITextBuffer> bufferAccessor, ITextView? focusedTextView)
    {
        var trackerMock = new StrictMock<IVisualStudioDocumentTracker>();
        trackerMock
            .Setup(t => t.TextBuffer)
            .Returns(bufferAccessor);
        trackerMock
            .Setup(t => t.GetFocusedTextView())
            .Returns(focusedTextView);

        return trackerMock.Object;
    }

    protected static ITextView CreateFocusedTextView(Func<ITextBuffer>? textBufferAccessor = null, ITextCaret? caret = null)
    {
        var focusedTextViewMock = new StrictMock<ITextView>();
        focusedTextViewMock
            .Setup(textView => textView.HasAggregateFocus)
            .Returns(true);

        if (textBufferAccessor != null)
        {
            focusedTextViewMock.Setup(textView => textView.TextBuffer)
                .Returns(textBufferAccessor);
        }

        if (caret != null)
        {
            focusedTextViewMock.Setup(textView => textView.Caret)
                .Returns(caret);
        }

        return focusedTextViewMock.Object;
    }

    protected static ITextCaret CreateCaretFrom(int position, ITextSnapshot snapshot)
    {
        var bufferPosition = new VirtualSnapshotPoint(snapshot, position);
        var mock = new StrictMock<ITextCaret>();
        mock.Setup(c => c.Position)
            .Returns(new CaretPosition(bufferPosition, StrictMock.Of<IMappingPoint>(), PositionAffinity.Predecessor));
        mock.Setup(c => c.MoveTo(It.IsAny<SnapshotPoint>()))
            .Returns<SnapshotPoint>(point => new CaretPosition(bufferPosition, StrictMock.Of<IMappingPoint>(), PositionAffinity.Predecessor));

        return mock.Object;
    }

    protected static IEditorOperationsFactoryService CreateOperationsFactoryService()
    {
        var editorOperationsMock = new StrictMock<IEditorOperations>();
        editorOperationsMock.Setup(operations => operations.MoveToEndOfLine(false)).Verifiable();
        var editorOperationsFactoryMock = new StrictMock<IEditorOperationsFactoryService>();
        editorOperationsFactoryMock
            .Setup(factory => factory.GetEditorOperations(It.IsAny<ITextView>()))
            .Returns(editorOperationsMock.Object);

        return editorOperationsFactoryMock.Object;
    }

    private protected static TestTextBuffer CreateTextBuffer(StringTextSnapshot initialSnapshot, IVisualStudioDocumentTracker documentTracker)
    {
        var textBuffer = new TestTextBuffer(initialSnapshot, VsMocks.ContentTypes.LegacyRazorCore);
        textBuffer.Properties.AddProperty(typeof(IVisualStudioDocumentTracker), documentTracker);

        var content = initialSnapshot.Content;
        var sourceDocument = TestRazorSourceDocument.Create(content);
        var syntaxTree = RazorSyntaxTree.Parse(sourceDocument,
            RazorParserOptions.Default
                .WithDirectives(FunctionsDirective.Directive)
                .WithFlags(enableSpanEditHandlers: true));

        var codeDocument = TestRazorCodeDocument.Create(content);
        codeDocument.SetSyntaxTree(syntaxTree);

        var parserMock = new StrictMock<IVisualStudioRazorParser>();
        parserMock
            .SetupGet(x => x.CodeDocument)
            .Returns(codeDocument);

        textBuffer.Properties.AddProperty(typeof(IVisualStudioRazorParser), parserMock.Object);

        return textBuffer;
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

public class LegacyRazorTextViewConnectionListenerTest(ITestOutputHelper testOutput) : ProjectSnapshotManagerDispatcherTestBase(testOutput)
{
    [UIFact]
    public void SubjectBuffersConnected_CallsRazorDocumentManager_OnTextViewOpened()
    {
        // Arrange
        var textView = StrictMock.Of<ITextView>();
        ITextBuffer[] buffers = [];
        var documentManagerMock = new StrictMock<IRazorDocumentManager>();
        documentManagerMock
            .Setup(d => d.OnTextViewOpenedAsync(textView, buffers))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var listener = new LegacyTextViewConnectionListener(documentManagerMock.Object, JoinableTaskFactory.Context);

        // Act
        listener.SubjectBuffersConnected(textView, ConnectionReason.BufferGraphChange, buffers);

        // Assert
        documentManagerMock.Verify();
    }

    [UIFact]
    public void SubjectBuffersDisconnected_CallsRazorDocumentManager_OnTextViewClosed()
    {
        // Arrange
        var textView = StrictMock.Of<ITextView>();
        ITextBuffer[] buffers = [];
        var documentManagerMock = new StrictMock<IRazorDocumentManager>();
        documentManagerMock
            .Setup(d => d.OnTextViewClosedAsync(textView, buffers))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var listener = new LegacyTextViewConnectionListener(documentManagerMock.Object, JoinableTaskFactory.Context);

        // Act
        listener.SubjectBuffersDisconnected(textView, ConnectionReason.BufferGraphChange, buffers);

        // Assert
        documentManagerMock.Verify();
    }
}

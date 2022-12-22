// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Editor.Razor;

public class RazorTextViewConnectionListenerTest : ProjectSnapshotManagerDispatcherTestBase
{
    public RazorTextViewConnectionListenerTest(ITestOutputHelper testOutput)
        : base(testOutput)
    {
    }

    [UIFact]
    public void SubjectBuffersConnected_CallsRazorDocumentManager_OnTextViewOpened()
    {
        // Arrange

        var textView = Mock.Of<ITextView>(MockBehavior.Strict);
        var buffers = new Collection<ITextBuffer>();
        var documentManager = new Mock<RazorDocumentManager>(MockBehavior.Strict);
        documentManager.Setup(d => d.OnTextViewOpenedAsync(textView, buffers)).Returns(Task.CompletedTask).Verifiable();

        var listener = new RazorTextViewConnectionListener(JoinableTaskFactory.Context, documentManager.Object);

        // Act
        listener.SubjectBuffersConnected(textView, ConnectionReason.BufferGraphChange, buffers);

        // Assert
        documentManager.Verify();
    }

    [UIFact]
    public void SubjectBuffersDisonnected_CallsRazorDocumentManager_OnTextViewClosed()
    {
        // Arrange
        var textView = Mock.Of<ITextView>(MockBehavior.Strict);
        var buffers = new Collection<ITextBuffer>();
        var documentManager = new Mock<RazorDocumentManager>(MockBehavior.Strict);
        documentManager.Setup(d => d.OnTextViewClosedAsync(textView, buffers)).Returns(Task.CompletedTask).Verifiable();

        var listener = new RazorTextViewConnectionListener(JoinableTaskFactory.Context, documentManager.Object);

        // Act
        listener.SubjectBuffersDisconnected(textView, ConnectionReason.BufferGraphChange, buffers);

        // Assert
        documentManager.Verify();
    }
}

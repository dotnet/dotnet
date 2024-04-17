// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

public class LegacyRazorTextViewConnectionListenerTest(ITestOutputHelper testOutput) : VisualStudioTestBase(testOutput)
{
    [UIFact]
    public void SubjectBuffersConnected_CallsRazorDocumentManager_OnTextViewOpened()
    {
        // Arrange
        var serviceProvider = VsMocks.CreateServiceProvider(static b =>
            b.AddComponentModel(static b =>
            {
                var startupInitializer = new RazorStartupInitializer([]);
                b.AddExport(startupInitializer);
            }));

        var textView = StrictMock.Of<ITextView>();
        ITextBuffer[] buffers = [];
        var documentManagerMock = new StrictMock<IRazorDocumentManager>();
        documentManagerMock
            .Setup(d => d.OnTextViewOpenedAsync(textView, buffers))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var listener = new LegacyTextViewConnectionListener(serviceProvider, documentManagerMock.Object, JoinableTaskFactory.Context);

        // Act
        listener.SubjectBuffersConnected(textView, ConnectionReason.BufferGraphChange, buffers);

        // Assert
        documentManagerMock.Verify();
    }

    [UIFact]
    public void SubjectBuffersDisconnected_CallsRazorDocumentManager_OnTextViewClosed()
    {
        // Arrange
        var serviceProvider = VsMocks.CreateServiceProvider(static b =>
            b.AddComponentModel(static b =>
            {
                var startupInitializer = new RazorStartupInitializer([]);
                b.AddExport(startupInitializer);
            }));

        var textView = StrictMock.Of<ITextView>();
        ITextBuffer[] buffers = [];
        var documentManagerMock = new StrictMock<IRazorDocumentManager>();
        documentManagerMock
            .Setup(d => d.OnTextViewClosedAsync(textView, buffers))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var listener = new LegacyTextViewConnectionListener(serviceProvider, documentManagerMock.Object, JoinableTaskFactory.Context);

        // Act
        listener.SubjectBuffersDisconnected(textView, ConnectionReason.BufferGraphChange, buffers);

        // Assert
        documentManagerMock.Verify();
    }
}

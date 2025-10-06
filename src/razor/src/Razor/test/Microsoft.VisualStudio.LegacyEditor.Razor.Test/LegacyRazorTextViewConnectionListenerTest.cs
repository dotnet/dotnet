// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.AspNetCore.Razor.Test.Common.VisualStudio;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.VisualStudio.Razor;
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
                var startupInitializer = new RazorStartupInitializer(TestLanguageServerFeatureOptions.Instance, []);
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
                var startupInitializer = new RazorStartupInitializer(TestLanguageServerFeatureOptions.Instance, []);
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

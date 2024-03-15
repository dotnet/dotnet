// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

[ContentType(RazorLanguage.CoreContentType)]
[ContentType(RazorConstants.LegacyCoreContentType)]
[TextViewRole(PredefinedTextViewRoles.Document)]
[Export(typeof(ITextViewConnectionListener))]
[method: ImportingConstructor]
internal sealed class LegacyTextViewConnectionListener(
    [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider,
    IRazorDocumentManager documentManager,
    JoinableTaskContext joinableTaskContext) : ITextViewConnectionListener
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IRazorDocumentManager _documentManager = documentManager;
    private readonly JoinableTaskContext _joinableTaskContext = joinableTaskContext;

    public void SubjectBuffersConnected(ITextView textView, ConnectionReason reason, IReadOnlyCollection<ITextBuffer> subjectBuffers)
    {
        _joinableTaskContext.AssertUIThread();

        // This is a potential entry point for Razor start up, if the legacy editor is enabled.
        // So, we need to ensure that any Razor start up services are initialized.
        RazorStartupInitializer.Initialize(_serviceProvider);

        _ = HandleAsync();

        async Task HandleAsync()
        {
            await _documentManager.OnTextViewOpenedAsync(textView, subjectBuffers);
        }
    }

    public void SubjectBuffersDisconnected(ITextView textView, ConnectionReason reason, IReadOnlyCollection<ITextBuffer> subjectBuffers)
    {
        _ = HandleAsync();

        async Task HandleAsync()
        {
            await _documentManager.OnTextViewClosedAsync(textView, subjectBuffers);
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Editor.Razor;

[ContentType(RazorLanguage.CoreContentType)]
[ContentType(RazorConstants.LegacyCoreContentType)]
[TextViewRole(PredefinedTextViewRoles.Document)]
[Export(typeof(ITextViewConnectionListener))]
internal class RazorTextViewConnectionListener : ITextViewConnectionListener
{
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly RazorDocumentManager _documentManager;

    [ImportingConstructor]
    public RazorTextViewConnectionListener(JoinableTaskContext joinableTaskContext, RazorDocumentManager documentManager)
    {
        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (documentManager is null)
        {
            throw new ArgumentNullException(nameof(documentManager));
        }

        _joinableTaskContext = joinableTaskContext;
        _documentManager = documentManager;
    }

    public void SubjectBuffersConnected(ITextView textView, ConnectionReason reason, IReadOnlyCollection<ITextBuffer> subjectBuffers)
    {
        _ = SubjectBuffersConnectedAsync(textView, subjectBuffers);
    }

    private async Task SubjectBuffersConnectedAsync(ITextView textView, IReadOnlyCollection<ITextBuffer> subjectBuffers)
    {
        try
        {
            if (textView is null)
            {
                throw new ArgumentException(nameof(textView));
            }

            if (subjectBuffers is null)
            {
                throw new ArgumentNullException(nameof(subjectBuffers));
            }

            _joinableTaskContext.AssertUIThread();
            await _documentManager.OnTextViewOpenedAsync(textView, subjectBuffers);
        }
        catch (Exception ex)
        {
            Debug.Fail("RazorTextViewConnectionListener.SubjectBuffersConnected threw exception:" +
                Environment.NewLine + ex.Message + Environment.NewLine + "Stack trace:" + Environment.NewLine + ex.StackTrace);
        }
    }

    public void SubjectBuffersDisconnected(ITextView textView, ConnectionReason reason, IReadOnlyCollection<ITextBuffer> subjectBuffers)
    {
        _ = SubjectBuffersDisconnectedAsync(textView, subjectBuffers);
    }

    public async Task SubjectBuffersDisconnectedAsync(ITextView textView, IReadOnlyCollection<ITextBuffer> subjectBuffers)
    {
        try
        {
            if (textView is null)
            {
                throw new ArgumentException(nameof(textView));
            }

            if (subjectBuffers is null)
            {
                throw new ArgumentNullException(nameof(subjectBuffers));
            }

            _joinableTaskContext.AssertUIThread();

            await _documentManager.OnTextViewClosedAsync(textView, subjectBuffers);
        }
        catch (Exception ex)
        {
            Debug.Fail("RazorTextViewConnectionListener.SubjectBuffersDisconnected threw exception:" +
                Environment.NewLine + ex.Message + Environment.NewLine + "Stack trace:" + Environment.NewLine + ex.StackTrace);
        }
    }
}

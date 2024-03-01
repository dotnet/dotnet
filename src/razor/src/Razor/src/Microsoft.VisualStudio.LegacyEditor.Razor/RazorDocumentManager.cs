// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

[Export(typeof(IRazorDocumentManager))]
[method: ImportingConstructor]
internal sealed class RazorDocumentManager(
    IRazorEditorFactoryService editorFactoryService,
    ProjectSnapshotManagerDispatcher dispatcher,
    JoinableTaskContext joinableTaskContext) : IRazorDocumentManager
{
    private readonly ProjectSnapshotManagerDispatcher _dispatcher = dispatcher;
    private readonly JoinableTaskFactory _jtf = joinableTaskContext.Factory;
    private readonly IRazorEditorFactoryService _editorFactoryService = editorFactoryService;

    public async Task OnTextViewOpenedAsync(ITextView textView, IEnumerable<ITextBuffer> subjectBuffers)
    {
        await _jtf.SwitchToMainThreadAsync();

        foreach (var textBuffer in subjectBuffers)
        {
            if (!textBuffer.IsLegacyCoreRazorBuffer())
            {
                continue;
            }

            if (!_editorFactoryService.TryGetDocumentTracker(textBuffer, out var documentTracker) ||
                documentTracker is not VisualStudioDocumentTracker tracker)
            {
                Debug.Fail("Tracker should always be available given our expectations of the VS workflow.");
                return;
            }

            tracker.AddTextView(textView);

            if (documentTracker.TextViews.Count == 1)
            {
                // tracker.Subscribe() accesses the project snapshot manager, which needs to be run on the
                // project snapshot manager's specialized thread.
                await _dispatcher.RunAsync(() => tracker.Subscribe(), CancellationToken.None).ConfigureAwait(false);
            }
        }
    }

    public async Task OnTextViewClosedAsync(ITextView textView, IEnumerable<ITextBuffer> subjectBuffers)
    {
        await _jtf.SwitchToMainThreadAsync();

        // This means a Razor buffer has be detached from this ITextView or the ITextView is closing. Since we keep a
        // list of all of the open text views for each text buffer, we need to update the tracker.
        //
        // Notice that this method is called *after* changes are applied to the text buffer(s). We need to check every
        // one of them for a tracker because the content type could have changed.
        foreach (var textBuffer in subjectBuffers)
        {
            if (textBuffer.Properties.TryGetProperty(typeof(IVisualStudioDocumentTracker), out VisualStudioDocumentTracker documentTracker))
            {
                documentTracker.RemoveTextView(textView);

                if (documentTracker.TextViews.Count == 0)
                {
                    // tracker.Unsubscribe() should be in sync with tracker.Subscribe(). The latter of needs to be run
                    // on the project snapshot manager's specialized thread, so we run both on it.
                    await _dispatcher.RunAsync(() => documentTracker.Unsubscribe(), CancellationToken.None).ConfigureAwait(false);
                }
            }
        }
    }
}

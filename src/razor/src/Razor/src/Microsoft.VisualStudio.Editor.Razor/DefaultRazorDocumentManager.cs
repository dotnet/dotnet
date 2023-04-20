// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor;

[System.Composition.Shared]
[Export(typeof(RazorDocumentManager))]
internal class DefaultRazorDocumentManager : RazorDocumentManager
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly RazorEditorFactoryService _editorFactoryService;

    [ImportingConstructor]
    public DefaultRazorDocumentManager(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext,
        RazorEditorFactoryService editorFactoryService)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (editorFactoryService is null)
        {
            throw new ArgumentNullException(nameof(editorFactoryService));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _joinableTaskContext = joinableTaskContext;
        _editorFactoryService = editorFactoryService;
    }

    public async override Task OnTextViewOpenedAsync(ITextView textView, IEnumerable<ITextBuffer> subjectBuffers)
    {
        if (textView is null)
        {
            throw new ArgumentNullException(nameof(textView));
        }

        if (subjectBuffers is null)
        {
            throw new ArgumentNullException(nameof(subjectBuffers));
        }

        _joinableTaskContext.AssertUIThread();

        foreach (var textBuffer in subjectBuffers)
        {
            if (!textBuffer.IsLegacyCoreRazorBuffer())
            {
                continue;
            }

            if (!_editorFactoryService.TryGetDocumentTracker(textBuffer, out var documentTracker) ||
                documentTracker is not DefaultVisualStudioDocumentTracker tracker)
            {
                Debug.Fail("Tracker should always be available given our expectations of the VS workflow.");
                return;
            }

            tracker.AddTextView(textView);

            if (documentTracker.TextViews.Count == 1)
            {
                // tracker.Subscribe() accesses the project snapshot manager, which needs to be run on the
                // project snapshot manager's specialized thread.
                await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(() => tracker.Subscribe(), CancellationToken.None).ConfigureAwait(false);
            }
        }
    }

    public async override Task OnTextViewClosedAsync(ITextView textView, IEnumerable<ITextBuffer> subjectBuffers)
    {
        if (textView is null)
        {
            throw new ArgumentNullException(nameof(textView));
        }

        if (subjectBuffers is null)
        {
            throw new ArgumentNullException(nameof(subjectBuffers));
        }

        _joinableTaskContext.AssertUIThread();

        // This means a Razor buffer has be detached from this ITextView or the ITextView is closing. Since we keep a
        // list of all of the open text views for each text buffer, we need to update the tracker.
        //
        // Notice that this method is called *after* changes are applied to the text buffer(s). We need to check every
        // one of them for a tracker because the content type could have changed.
        foreach (var textBuffer in subjectBuffers)
        {
            if (textBuffer.Properties.TryGetProperty(typeof(VisualStudioDocumentTracker), out DefaultVisualStudioDocumentTracker documentTracker))
            {
                documentTracker.RemoveTextView(textView);

                if (documentTracker.TextViews.Count == 0)
                {
                    // tracker.Unsubscribe() should be in sync with tracker.Subscribe(). The latter of needs to be run
                    // on the project snapshot manager's specialized thread, so we run both on it.
                    await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(() => documentTracker.Unsubscribe(), CancellationToken.None).ConfigureAwait(false);
                }
            }
        }
    }
}

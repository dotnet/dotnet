// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;

namespace Microsoft.VisualStudio.Extensibility.Testing;

internal partial class EditorInProcess
{
    public async Task DismissCompletionSessionsAsync(CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var view = await GetActiveTextViewAsync(cancellationToken);

        var asyncBroker = await GetComponentModelServiceAsync<IAsyncCompletionBroker>(cancellationToken);
        var session = asyncBroker.GetSession(view);
        if (session is not null && !session.IsDismissed)
        {
            session.Dismiss();
        }
    }

    public Task<IAsyncCompletionSession?> WaitForCompletionSessionAsync(CancellationToken cancellationToken)
    {
        return WaitForCompletionSessionAsync(TimeSpan.FromSeconds(10), cancellationToken);
    }

    public async Task<IAsyncCompletionSession?> WaitForCompletionSessionAsync(TimeSpan timeOut, CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var textView = await GetActiveTextViewAsync(cancellationToken);

        var stopWatch = Stopwatch.StartNew();
        var asyncCompletion = await TestServices.Shell.GetComponentModelServiceAsync<IAsyncCompletionBroker>(cancellationToken);
        var session = asyncCompletion.TriggerCompletion(textView, new CompletionTrigger(CompletionTriggerReason.Insertion, textView.TextSnapshot), textView.Caret.Position.BufferPosition, cancellationToken);

        // Loop until completion comes up
        while (session is null || session.IsDismissed)
        {
            if (stopWatch.ElapsedMilliseconds >= timeOut.TotalMilliseconds)
            {
                return null;
            }

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            session = asyncCompletion.TriggerCompletion(textView, new CompletionTrigger(CompletionTriggerReason.Insertion, textView.TextSnapshot), textView.Caret.Position.BufferPosition, cancellationToken);
        }

        return session;
    }
}

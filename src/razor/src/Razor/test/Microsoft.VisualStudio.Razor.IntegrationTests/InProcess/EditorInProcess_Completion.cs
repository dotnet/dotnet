// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;

namespace Microsoft.VisualStudio.Extensibility.Testing;

internal partial class EditorInProcess
{
    public const int DefaultCompletionWaitTimeMilliseconds = 10000;

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

    /// <summary>
    /// Open completion pop-up window UI and wait for the specified item to be present selected
    /// </summary>
    /// <param name="timeOut"></param>
    /// <param name="selectedItemLabel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Completion session that has matching selected item, or null otherwise</returns>
    public async Task<IAsyncCompletionSession?> OpenCompletionSessionAndWaitForItemAsync(TimeSpan timeOut, string selectedItemLabel, CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        // Returns completion session that might or might not be visible in the IDE
        var session = await WaitForCompletionSessionAsync(timeOut, cancellationToken);

        if (session is null)
        {
            return null;
        }

        var textView = await GetActiveTextViewAsync(cancellationToken);
        var stopWatch = Stopwatch.StartNew();

        // Actually open the completion pop-up window and force visible items to be computed or re-computed
        session.OpenOrUpdate(new CompletionTrigger(CompletionTriggerReason.Insertion, textView.TextSnapshot), textView.Caret.Position.BufferPosition, cancellationToken);
        while (session.GetComputedItems(cancellationToken).SelectedItem?.DisplayText != selectedItemLabel)
        {
            if (stopWatch.ElapsedMilliseconds >= timeOut.TotalMilliseconds)
            {
                return null;
            }

            await Task.Delay(100, cancellationToken);

            session.OpenOrUpdate(new CompletionTrigger(CompletionTriggerReason.Insertion, textView.TextSnapshot), textView.Caret.Position.BufferPosition, cancellationToken);
        }

        return session;
    }
}

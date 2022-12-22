// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Razor.IntegrationTests.Extensions;

namespace Microsoft.VisualStudio.Extensibility.Testing;

internal partial class EditorInProcess
{
    public async Task<ITextSnapshot> GetActiveSnapshotAsync(CancellationToken cancellationToken)
        => (await GetActiveTextViewAsync(cancellationToken)).TextSnapshot;

    public async Task ActivateAsync(CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var dte = await GetRequiredGlobalServiceAsync<SDTE, EnvDTE.DTE>(cancellationToken);
        dte.ActiveDocument.Activate();
    }

    public async Task<bool> IsUseSuggestionModeOnAsync(CancellationToken cancellationToken)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var textView = await GetActiveTextViewAsync(cancellationToken);

        var subjectBuffer = textView.GetBufferContainingCaret();
        Assumes.Present(subjectBuffer);

        var options = textView.Options.GlobalOptions;
        EditorOptionKey<bool> optionKey;
        bool defaultOption;
        if (IsDebuggerTextView(textView))
        {
            optionKey = new EditorOptionKey<bool>(PredefinedCompletionNames.SuggestionModeInDebuggerCompletionOptionName);
            defaultOption = true;
        }
        else
        {
            optionKey = new EditorOptionKey<bool>(PredefinedCompletionNames.SuggestionModeInCompletionOptionName);
            defaultOption = false;
        }

        if (!options.IsOptionDefined(optionKey, localScopeOnly: false))
        {
            return defaultOption;
        }

        return options.GetOptionValue(optionKey);

        static bool IsDebuggerTextView(IWpfTextView textView)
        {
            return textView.Roles.Contains("DEBUGVIEW");
        }
    }

    public async Task SetUseSuggestionModeAsync(bool value, CancellationToken cancellationToken)
    {
        if (await IsUseSuggestionModeOnAsync(cancellationToken) != value)
        {
            var dispatcher = await GetRequiredGlobalServiceAsync<SUIHostCommandDispatcher, IOleCommandTarget>(cancellationToken);
            ErrorHandler.ThrowOnFailure(dispatcher.Exec(typeof(VSConstants.VSStd2KCmdID).GUID, (uint)VSConstants.VSStd2KCmdID.ToggleConsumeFirstCompletionMode, (uint)OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, IntPtr.Zero, IntPtr.Zero));

            if (await IsUseSuggestionModeOnAsync(cancellationToken) != value)
            {
                throw new InvalidOperationException($"Edit.ToggleCompletionMode did not leave the editor in the expected state.");
            }
        }

        if (!value)
        {
            // For blocking completion mode, make sure we don't have responsive completion interfering when
            // integration tests run slowly.
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            var options = view.Options.GlobalOptions;
            options.SetOptionValue(DefaultOptions.ResponsiveCompletionOptionId, false);

            var latencyGuardOptionKey = new EditorOptionKey<bool>("EnableTypingLatencyGuard");
            options.SetOptionValue(latencyGuardOptionKey, false);
        }
    }
}

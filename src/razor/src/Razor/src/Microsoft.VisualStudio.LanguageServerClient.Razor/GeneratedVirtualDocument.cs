// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal abstract class GeneratedVirtualDocument<T>(Uri uri, ITextBuffer textBuffer, ITelemetryReporter telemetryReporter) : VirtualDocumentBase<T>(uri, textBuffer) where T : VirtualDocumentSnapshot
{
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    public override VirtualDocumentSnapshot Update(IReadOnlyList<ITextChange> changes, int hostDocumentVersion, object? state)
    {
        var currentSnapshotLength = CurrentSnapshot.Snapshot.Length;
        if (state is bool previousWasEmpty &&
            previousWasEmpty != (currentSnapshotLength == 0))
        {
            Debug.Fail($"The language server is sending us changes for what it/we thought was an empty file, but their/our copy is not empty. Generated C# file may have corrupted file contents after this update.");

            var recoverable = false;
            if (previousWasEmpty && changes is [{ OldPosition: 0, OldEnd: 0 } change])
            {
                recoverable = true;
                // The LSP server thought the file was empty, but we have some contents. That's not good, but we can recover
                // by adjusting the range for the change (which would be (0,0)-(0,0) from the LSP server point of view) to
                // cover the whole buffer, essentially just taking the LSP server as the source of truth.
                changes = new[] { new VisualStudioTextChange(0, currentSnapshotLength, change.NewText) };
            }

            var data = ImmutableDictionary<string, object?>.Empty
                .Add("version", hostDocumentVersion)
                .Add("type", typeof(T).Name)
                .Add("recoverable", recoverable);

            _telemetryReporter.ReportEvent("sync", Severity.High, data);
        }

        return base.Update(changes, hostDocumentVersion, state);
    }
}

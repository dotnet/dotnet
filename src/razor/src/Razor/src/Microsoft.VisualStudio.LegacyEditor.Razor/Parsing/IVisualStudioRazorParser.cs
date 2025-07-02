// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

internal interface IVisualStudioRazorParser
{
    string FilePath { get; }
    RazorCodeDocument? CodeDocument { get; }
    ITextSnapshot? Snapshot { get; }
    ITextBuffer TextBuffer { get; }
    bool HasPendingChanges { get; }

    void QueueReparse();
    Task<RazorCodeDocument?> GetLatestCodeDocumentAsync(ITextSnapshot atOrNewerSnapshot, CancellationToken cancellationToken = default);
    event EventHandler<DocumentStructureChangedEventArgs> DocumentStructureChanged;
}

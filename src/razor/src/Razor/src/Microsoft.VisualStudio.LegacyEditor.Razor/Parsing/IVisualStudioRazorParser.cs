// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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

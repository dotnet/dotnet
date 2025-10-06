// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.VisualStudio.Text;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal class DocumentStructureChangedEventArgs(
    RazorSourceChange? change,
    ITextSnapshot snapshot,
    IRazorCodeDocument codeDocument) : EventArgs
{
    /// <summary>
    /// The <see cref="RazorSourceChange"/> which triggered the re-parse.
    /// </summary>
    public RazorSourceChange? SourceChange { get; } = change;

    /// <summary>
    /// The text snapshot used in the re-parse.
    /// </summary>
    public ITextSnapshot Snapshot { get; } = snapshot ?? throw new ArgumentNullException(nameof(snapshot));

    /// <summary>
    /// The result of the parsing and code generation.
    /// </summary>
    public IRazorCodeDocument CodeDocument { get; } = codeDocument ?? throw new ArgumentNullException(nameof(codeDocument));
}

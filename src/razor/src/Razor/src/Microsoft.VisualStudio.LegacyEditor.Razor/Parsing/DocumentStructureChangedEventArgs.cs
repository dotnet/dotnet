// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

internal sealed class DocumentStructureChangedEventArgs(
    SourceChange? change,
    ITextSnapshot snapshot,
    RazorCodeDocument codeDocument) : EventArgs
{
    /// <summary>
    /// The <see cref="AspNetCore.Razor.Language.SourceChange"/> which triggered the re-parse.
    /// </summary>
    public SourceChange? SourceChange { get; } = change;

    /// <summary>
    /// The text snapshot used in the re-parse.
    /// </summary>
    public ITextSnapshot Snapshot { get; } = snapshot ?? throw new ArgumentNullException(nameof(snapshot));

    /// <summary>
    /// The result of the parsing and code generation.
    /// </summary>
    public RazorCodeDocument CodeDocument { get; } = codeDocument ?? throw new ArgumentNullException(nameof(codeDocument));
}

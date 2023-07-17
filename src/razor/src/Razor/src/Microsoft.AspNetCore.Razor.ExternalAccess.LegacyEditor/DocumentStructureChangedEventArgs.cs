// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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

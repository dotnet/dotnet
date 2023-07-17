// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

[System.Composition.Shared]
[Export(typeof(RazorIndentationFactsService))]
internal class DefaultRazorIndentationFactsService : RazorIndentationFactsService
{
    public override int? GetDesiredIndentation(
        RazorSyntaxTree syntaxTree,
        ITextSnapshot syntaxTreeSnapshot,
        ITextSnapshotLine line,
        int indentSize,
        int tabSize)
    {
        return RazorIndentationFacts.GetDesiredIndentation(syntaxTree, syntaxTreeSnapshot, line, indentSize, tabSize);
    }
}

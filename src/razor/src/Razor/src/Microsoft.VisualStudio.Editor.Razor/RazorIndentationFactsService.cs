// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Host;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class RazorIndentationFactsService : ILanguageService
{
    public abstract int? GetDesiredIndentation(RazorSyntaxTree syntaxTree, ITextSnapshot syntaxTreeSnapshot, ITextSnapshotLine line, int indentSize, int tabSize);
}

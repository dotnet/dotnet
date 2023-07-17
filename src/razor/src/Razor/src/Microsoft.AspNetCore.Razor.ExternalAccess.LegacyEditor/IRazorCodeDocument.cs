// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.VisualStudio.Text;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal interface IRazorCodeDocument
{
    ImmutableArray<ClassifiedSpan> GetClassifiedSpans();
    ImmutableArray<TagHelperSpan> GetTagHelperSpans();

    ImmutableArray<RazorSourceMapping> GetSourceMappings();
    ImmutableArray<IRazorDiagnostic> GetDiagnostics();
    IRazorTagHelperDocumentContext GetTagHelperContext();

    int? GetDesiredIndentation(ITextSnapshot snapshot, ITextSnapshotLine line, int indentSize, int tabSize);

    string GetGeneratedCode();
}

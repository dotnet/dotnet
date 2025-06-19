// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class RazorHtmlDocument : IRazorGeneratedDocument
{
    public RazorCodeDocument CodeDocument { get; }
    public SourceText Text { get; }
    public ImmutableArray<SourceMapping> SourceMappings { get; }

    public RazorHtmlDocument(
        RazorCodeDocument codeDocument,
        SourceText text,
        ImmutableArray<SourceMapping> sourceMappings = default)
    {
        ArgHelper.ThrowIfNull(codeDocument);
        ArgHelper.ThrowIfNull(text);

        CodeDocument = codeDocument;
        Text = text;
        SourceMappings = sourceMappings.NullToEmpty();
    }
}

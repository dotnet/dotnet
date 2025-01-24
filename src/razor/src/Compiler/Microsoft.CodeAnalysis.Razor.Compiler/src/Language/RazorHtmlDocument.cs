// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class RazorHtmlDocument : IRazorGeneratedDocument
{
    public RazorCodeDocument CodeDocument { get; }
    public SourceText Text { get; }
    public RazorCodeGenerationOptions Options { get; }
    public ImmutableArray<SourceMapping> SourceMappings { get; }

    public RazorHtmlDocument(
        RazorCodeDocument codeDocument,
        SourceText text,
        RazorCodeGenerationOptions options,
        ImmutableArray<SourceMapping> sourceMappings = default)
    {
        ArgHelper.ThrowIfNull(codeDocument);
        ArgHelper.ThrowIfNull(text);
        ArgHelper.ThrowIfNull(options);

        CodeDocument = codeDocument;
        Text = text;
        Options = options;
        SourceMappings = sourceMappings.NullToEmpty();
    }
}

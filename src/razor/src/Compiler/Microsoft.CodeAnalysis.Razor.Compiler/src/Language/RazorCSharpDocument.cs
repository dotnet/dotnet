// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed class RazorCSharpDocument : IRazorGeneratedDocument
{
    public RazorCodeDocument CodeDocument { get; }
    public SourceText Text { get; }
    public RazorCodeGenerationOptions Options { get; }
    public ImmutableArray<RazorDiagnostic> Diagnostics { get; }
    public ImmutableArray<SourceMapping> SourceMappings { get; }
    public ImmutableArray<LinePragma> LinePragmas { get; }

    public RazorCSharpDocument(
        RazorCodeDocument codeDocument,
        SourceText text,
        RazorCodeGenerationOptions options,
        ImmutableArray<RazorDiagnostic> diagnostics,
        ImmutableArray<SourceMapping> sourceMappings = default,
        ImmutableArray<LinePragma> linePragmas = default)
    {
        ArgHelper.ThrowIfNull(codeDocument);
        ArgHelper.ThrowIfNull(text);
        ArgHelper.ThrowIfNull(options);

        CodeDocument = codeDocument;
        Text = text;
        Options = options;

        Diagnostics = diagnostics.NullToEmpty();
        SourceMappings = sourceMappings.NullToEmpty();
        LinePragmas = linePragmas.NullToEmpty();
    }
}

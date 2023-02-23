// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal class SpanContext
{
    public SpanContext(ISpanChunkGenerator chunkGenerator, SpanEditHandler editHandler)
    {
        ChunkGenerator = chunkGenerator;
        EditHandler = editHandler;
    }

    public ISpanChunkGenerator ChunkGenerator { get; }

    public SpanEditHandler EditHandler { get; }
}

internal class SpanContextBuilder
{
    public SpanContextBuilder(Func<string, IEnumerable<Syntax.InternalSyntax.SyntaxToken>> defaultLanguageTokenizer)
    {
        EditHandlerBuilder = new(defaultLanguageTokenizer);
        Reset();
    }

    public ISpanChunkGenerator ChunkGenerator { get; set; }

    public SpanEditHandlerBuilder EditHandlerBuilder { get; set; }

    public SpanContext Build()
    {
        var result = new SpanContext(ChunkGenerator, EditHandlerBuilder.Build());
        Reset();
        return result;
    }

    [MemberNotNull(nameof(ChunkGenerator))]
    public void Reset()
    {
        EditHandlerBuilder.Reset();
        ChunkGenerator = SpanChunkGenerator.Null;
    }
}

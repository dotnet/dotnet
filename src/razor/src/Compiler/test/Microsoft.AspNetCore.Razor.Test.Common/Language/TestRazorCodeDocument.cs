// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Language;

public static class TestRazorCodeDocument
{
    public static RazorCodeDocument CreateEmpty()
    {
        var source = TestRazorSourceDocument.Create(content: string.Empty);
        return new RazorCodeDocument(source, imports: default);
    }

    public static RazorCodeDocument Create(string content, bool normalizeNewLines = false)
    {
        var source = TestRazorSourceDocument.Create(content, normalizeNewLines: normalizeNewLines);
        return new RazorCodeDocument(source, imports: default);
    }

    public static RazorCodeDocument Create(RazorSourceDocument source, ImmutableArray<RazorSourceDocument> imports)
    {
        return new RazorCodeDocument(source, imports);
    }
}

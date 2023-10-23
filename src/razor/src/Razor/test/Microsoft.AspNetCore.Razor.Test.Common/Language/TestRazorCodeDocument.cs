// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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

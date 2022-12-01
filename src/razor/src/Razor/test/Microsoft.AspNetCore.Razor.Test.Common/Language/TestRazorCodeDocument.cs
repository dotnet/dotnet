// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Language;

public static class TestRazorCodeDocument
{
    public static RazorCodeDocument CreateEmpty()
    {
        var source = TestRazorSourceDocument.Create(content: string.Empty);
        return new DefaultRazorCodeDocument(source, imports: null);
    }

    public static RazorCodeDocument Create(string content, bool normalizeNewLines = false)
    {
        var source = TestRazorSourceDocument.Create(content, normalizeNewLines: normalizeNewLines);
        return new DefaultRazorCodeDocument(source, imports: null);
    }

    public static RazorCodeDocument Create(RazorSourceDocument source, IEnumerable<RazorSourceDocument> imports)
    {
        return new DefaultRazorCodeDocument(source, imports);
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed class RazorCodeDocument
{
    public RazorSourceDocument Source { get; }
    public ImmutableArray<RazorSourceDocument> Imports { get; }
    public ItemCollection Items { get; }

    private RazorCodeDocument(RazorSourceDocument source, ImmutableArray<RazorSourceDocument> imports)
    {
        Source = source;
        Imports = imports.NullToEmpty();

        Items = new ItemCollection();
    }

    public static RazorCodeDocument Create(RazorSourceDocument source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return Create(source, imports: default);
    }

    public static RazorCodeDocument Create(
        RazorSourceDocument source,
        ImmutableArray<RazorSourceDocument> imports)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return new RazorCodeDocument(source, imports);
    }

    public static RazorCodeDocument Create(
        RazorSourceDocument source,
        ImmutableArray<RazorSourceDocument> imports,
        RazorParserOptions parserOptions,
        RazorCodeGenerationOptions codeGenerationOptions)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var codeDocument = new RazorCodeDocument(source, imports);
        codeDocument.SetParserOptions(parserOptions);
        codeDocument.SetCodeGenerationOptions(codeGenerationOptions);
        return codeDocument;
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed partial class RazorCodeDocument
{
    public RazorSourceDocument Source { get; }
    public ImmutableArray<RazorSourceDocument> Imports { get; }

    public RazorParserOptions ParserOptions { get; }
    public RazorCodeGenerationOptions CodeGenerationOptions { get; }

    public RazorFileKind FileKind => ParserOptions.FileKind;

    private readonly PropertyTable _properties = new();
    private readonly object _htmlDocumentLock = new();

    private RazorCodeDocument(
        RazorSourceDocument source,
        ImmutableArray<RazorSourceDocument> imports,
        RazorParserOptions? parserOptions,
        RazorCodeGenerationOptions? codeGenerationOptions,
        PropertyTable? properties = null)
    {
        Source = source;
        Imports = imports.NullToEmpty();

        ParserOptions = parserOptions ?? RazorParserOptions.Default;
        CodeGenerationOptions = codeGenerationOptions ?? RazorCodeGenerationOptions.Default;

        _properties = properties ?? new();
    }

    public static RazorCodeDocument Create(
        RazorSourceDocument source,
        RazorParserOptions? parserOptions = null,
        RazorCodeGenerationOptions? codeGenerationOptions = null)
    {
        ArgHelper.ThrowIfNull(source);

        return new RazorCodeDocument(source, imports: [], parserOptions, codeGenerationOptions);
    }

    public static RazorCodeDocument Create(
        RazorSourceDocument source,
        ImmutableArray<RazorSourceDocument> imports,
        RazorParserOptions? parserOptions = null,
        RazorCodeGenerationOptions? codeGenerationOptions = null)
    {
        ArgHelper.ThrowIfNull(source);

        return new RazorCodeDocument(source, imports, parserOptions, codeGenerationOptions);
    }

    internal bool TryGetTagHelpers([NotNullWhen(true)] out TagHelperCollection? result)
        => _properties.TagHelpers.TryGetValue(out result);

    internal TagHelperCollection? GetTagHelpers()
        => _properties.TagHelpers.Value;

    internal TagHelperCollection GetRequiredTagHelpers()
        => _properties.TagHelpers.RequiredValue;

    internal void SetTagHelpers(TagHelperCollection? value)
        => _properties.TagHelpers.SetValue(value);

    internal bool TryGetReferencedTagHelpers([NotNullWhen(true)] out TagHelperCollection? result)
        => _properties.ReferencedTagHelpers.TryGetValue(out result);

    internal TagHelperCollection? GetReferencedTagHelpers()
        => _properties.ReferencedTagHelpers.Value;

    internal TagHelperCollection GetRequiredReferencedTagHelpers()
        => _properties.ReferencedTagHelpers.RequiredValue;

    internal void SetReferencedTagHelpers(TagHelperCollection value)
    {
        ArgHelper.ThrowIfNull(value);
        _properties.ReferencedTagHelpers.SetValue(value);
    }

    internal bool TryGetPreTagHelperSyntaxTree([NotNullWhen(true)] out RazorSyntaxTree? result)
        => _properties.PreTagHelperSyntaxTree.TryGetValue(out result);

    internal RazorSyntaxTree? GetPreTagHelperSyntaxTree()
        => _properties.PreTagHelperSyntaxTree.Value;

    internal RazorSyntaxTree GetRequiredPreTagHelperSyntaxTree()
        => _properties.PreTagHelperSyntaxTree.RequiredValue;

    internal void SetPreTagHelperSyntaxTree(RazorSyntaxTree? value)
        => _properties.PreTagHelperSyntaxTree.SetValue(value);

    internal bool TryGetSyntaxTree([NotNullWhen(true)] out RazorSyntaxTree? result)
        => _properties.SyntaxTree.TryGetValue(out result);

    internal RazorSyntaxTree? GetSyntaxTree()
        => _properties.SyntaxTree.Value;

    internal RazorSyntaxTree GetRequiredSyntaxTree()
        => _properties.SyntaxTree.RequiredValue;

    internal void SetSyntaxTree(RazorSyntaxTree value)
    {
        Debug.Assert(value is not null);
        _properties.SyntaxTree.SetValue(value);
    }

    internal bool TryGetImportSyntaxTrees(out ImmutableArray<RazorSyntaxTree> result)
        => _properties.ImportSyntaxTrees.TryGetValue(out result);

    internal ImmutableArray<RazorSyntaxTree> GetImportSyntaxTrees()
        => _properties.ImportSyntaxTrees.Value ?? [];

    internal void SetImportSyntaxTrees(ImmutableArray<RazorSyntaxTree> value)
    {
        Debug.Assert(!value.IsDefault);
        Debug.Assert(value.IsEmpty || value.All(static t => t is not null));

        _properties.ImportSyntaxTrees.SetValue(value);
    }

    internal bool TryGetTagHelperContext([NotNullWhen(true)] out TagHelperDocumentContext? result)
        => _properties.TagHelperContext.TryGetValue(out result);

    internal TagHelperDocumentContext? GetTagHelperContext()
        => _properties.TagHelperContext.Value;

    internal TagHelperDocumentContext GetRequiredTagHelperContext()
        => _properties.TagHelperContext.RequiredValue;

    internal void SetTagHelperContext(TagHelperDocumentContext value)
    {
        Debug.Assert(value is not null);
        _properties.TagHelperContext.SetValue(value);
    }

    internal bool TryGetDocumentNode([NotNullWhen(true)] out DocumentIntermediateNode? result)
        => _properties.DocumentNode.TryGetValue(out result);

    internal DocumentIntermediateNode? GetDocumentNode()
        => _properties.DocumentNode.Value;

    internal DocumentIntermediateNode GetRequiredDocumentNode()
        => _properties.DocumentNode.RequiredValue;

    internal void SetDocumentNode(DocumentIntermediateNode value)
    {
        Debug.Assert(value is not null);
        _properties.DocumentNode.SetValue(value);
    }

    internal bool TryGetCSharpDocument([NotNullWhen(true)] out RazorCSharpDocument? result)
        => _properties.CSharpDocument.TryGetValue(out result);

    internal RazorCSharpDocument? GetCSharpDocument()
        => _properties.CSharpDocument.Value;

    internal RazorCSharpDocument GetRequiredCSharpDocument()
        => _properties.CSharpDocument.RequiredValue;

    internal void SetCSharpDocument(RazorCSharpDocument value)
    {
        Debug.Assert(value is not null);
        _properties.CSharpDocument.SetValue(value);
    }

    internal RazorHtmlDocument GetHtmlDocument()
    {
        if (_properties.HtmlDocument.TryGetValue(out var result))
        {
            return result;
        }

        // Perf: Avoid concurrent requests generating the same html document
        lock (_htmlDocumentLock)
        {
            if (!_properties.HtmlDocument.TryGetValue(out result))
            {
                result = RazorHtmlWriter.GetHtmlDocument(this);
                _properties.HtmlDocument.SetValue(result);
            }
        }

        return result;
    }

    // In general documents will have a relative path (relative to the project root).
    // We can only really compute a nice namespace when we know a relative path.
    //
    // However all kinds of thing are possible in tools. We shouldn't barf here if the document isn't set up correctly.
    internal bool TryGetNamespace(
        bool fallbackToRootNamespace,
        bool considerImports,
        [NotNullWhen(true)] out string? @namespace,
        out SourceSpan? namespaceSpan)
    {
        // We only want to cache the namespace if we're considering all possibilities.
        // Anyone wanting something different (i.e., tooling) has to pay a slight penalty.
        if (fallbackToRootNamespace && considerImports &&
            _properties.NamespaceInfo.TryGetValue(out var info))
        {
            VerifyNamespace(this, fallbackToRootNamespace, considerImports, info.name);

            (@namespace, namespaceSpan) = info;
            return true;
        }

        if (NamespaceComputer.TryComputeNamespace(this, fallbackToRootNamespace, considerImports, out @namespace, out namespaceSpan))
        {
            VerifyNamespace(this, fallbackToRootNamespace, considerImports, @namespace);

            _properties.NamespaceInfo.SetValue((@namespace, namespaceSpan));
            return true;
        }

        return false;

        [Conditional("DEBUG")]
        static void VerifyNamespace(RazorCodeDocument codeDocument, bool fallbackToRootNamespace, bool considerImports, string? @namespace)
        {
            // In debug mode, even if we're cached, lets take the hit to run this again and make sure the cached value is correct.
            // This is to help us find issues with caching logic during development.
            var validateResult = NamespaceComputer.TryComputeNamespace(
                codeDocument, fallbackToRootNamespace, considerImports, out var validateNamespace, out _);

            Debug.Assert(validateResult, "We couldn't compute the namespace, but have a cached value, so something has gone wrong");
            Debug.Assert(validateNamespace == @namespace, $"We cached a namespace of {@namespace} but calculated that it should be {validateNamespace}");
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Do not use. Present to support the legacy editor", error: false)]
    internal RazorCodeDocument Clone()
        => new(Source, Imports, ParserOptions, CodeGenerationOptions, _properties.Clone());
}

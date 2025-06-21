// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor.Threading;

namespace Microsoft.AspNetCore.Razor.Language;

internal static partial class RazorCodeDocumentExtensions
{
    private static readonly ConditionalWeakTable<RazorCodeDocument, CachedData> s_codeDocumentCache = new();

    private static CachedData GetCachedData(RazorCodeDocument codeDocument)
        => s_codeDocumentCache.GetValue(codeDocument, static doc => new CachedData(doc));

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Do not use. Present to support the legacy editor", error: false)]
    public static void CloneCachedData(this RazorCodeDocument fromCodeDocument, RazorCodeDocument toCodeDocument)
    {
        if (!s_codeDocumentCache.TryGetValue(fromCodeDocument, out var fromCachedData))
        {
            // If there isn't any data cached, there's nothing to clone.
            return;
        }

        s_codeDocumentCache.Add(toCodeDocument, fromCachedData.Clone());
    }

    private sealed class CachedData(RazorCodeDocument codeDocument)
    {
        private readonly RazorCodeDocument _codeDocument = codeDocument;

        private readonly SemaphoreSlim _stateLock = new(initialCount: 1);
        private SyntaxTree? _syntaxTree;
        private ImmutableArray<ClassifiedSpanInternal>? _classifiedSpans;
        private ImmutableArray<TagHelperSpanInternal>? _tagHelperSpans;

        public SyntaxTree GetOrParseCSharpSyntaxTree(CancellationToken cancellationToken)
        {
            if (_syntaxTree is { } syntaxTree)
            {
                return syntaxTree;
            }

            using (_stateLock.DisposableWait(cancellationToken))
            {
                return _syntaxTree ??= ParseSyntaxTree(_codeDocument, cancellationToken);
            }

            static SyntaxTree ParseSyntaxTree(RazorCodeDocument codeDocument, CancellationToken cancellationToken)
            {
                var csharpText = codeDocument.GetCSharpSourceText();
                return CSharpSyntaxTree.ParseText(csharpText, cancellationToken: cancellationToken);
            }
        }

        public ImmutableArray<ClassifiedSpanInternal> GetOrComputeClassifiedSpans(CancellationToken cancellationToken)
        {
            if (_classifiedSpans is { } classifiedSpans)
            {
                return classifiedSpans;
            }

            using (_stateLock.DisposableWait(cancellationToken))
            {
                return _classifiedSpans ??= _codeDocument.GetRequiredSyntaxTree().GetClassifiedSpans();
            }
        }

        public ImmutableArray<TagHelperSpanInternal> GetOrComputeTagHelperSpans(CancellationToken cancellationToken)
        {
            if (_tagHelperSpans is { } tagHelperSpans)
            {
                return tagHelperSpans;
            }

            using (_stateLock.DisposableWait(cancellationToken))
            {
                return _tagHelperSpans ??= _codeDocument.GetRequiredSyntaxTree().GetTagHelperSpans();
            }
        }

        public CachedData Clone()
            => new(_codeDocument)
            {
                _syntaxTree = _syntaxTree,
                _classifiedSpans = _classifiedSpans,
                _tagHelperSpans = _tagHelperSpans,
            };
    }
}

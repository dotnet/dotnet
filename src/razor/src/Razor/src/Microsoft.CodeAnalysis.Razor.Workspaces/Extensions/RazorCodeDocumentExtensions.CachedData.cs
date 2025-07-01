// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
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
        private ImmutableArray<ClassifiedSpan>? _classifiedSpans;
        private ImmutableArray<SourceSpan>? _tagHelperSpans;

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

        public ImmutableArray<ClassifiedSpan> GetOrComputeClassifiedSpans(CancellationToken cancellationToken)
        {
            if (_classifiedSpans is { } classifiedSpans)
            {
                return classifiedSpans;
            }

            using (_stateLock.DisposableWait(cancellationToken))
            {
                return _classifiedSpans ??= ClassifiedSpanVisitor.VisitRoot(_codeDocument.GetRequiredSyntaxTree());
            }
        }

        public ImmutableArray<SourceSpan> GetOrComputeTagHelperSpans(CancellationToken cancellationToken)
        {
            if (_tagHelperSpans is { } tagHelperSpans)
            {
                return tagHelperSpans;
            }

            using (_stateLock.DisposableWait(cancellationToken))
            {
                return _tagHelperSpans ??= ComputeTagHelperSpans(_codeDocument.GetRequiredSyntaxTree());
            }

            static ImmutableArray<SourceSpan> ComputeTagHelperSpans(RazorSyntaxTree syntaxTree)
            {
                using var builder = new PooledArrayBuilder<SourceSpan>();

                foreach (var node in syntaxTree.Root.DescendantNodes())
                {
                    if (node is not MarkupTagHelperElementSyntax tagHelperElement ||
                        tagHelperElement.TagHelperInfo is null)
                    {
                        continue;
                    }

                    builder.Add(tagHelperElement.GetSourceSpan(syntaxTree.Source));
                }

                return builder.ToImmutableAndClear();
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

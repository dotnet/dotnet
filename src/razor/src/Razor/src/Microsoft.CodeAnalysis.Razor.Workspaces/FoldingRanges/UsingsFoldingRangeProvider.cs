// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.FoldingRanges;

internal class UsingsFoldingRangeProvider : IRazorFoldingRangeProvider
{
    public ImmutableArray<FoldingRange> GetFoldingRanges(RazorCodeDocument codeDocument)
    {
        using var ranges = new PooledArrayBuilder<FoldingRange>();

        var sourceDocument = codeDocument.Source;
        var syntaxTree = codeDocument.GetRequiredSyntaxTree();

        foreach (var directive in syntaxTree.EnumerateUsingDirectives())
        {
            var span = directive.GetLinePositionSpan(sourceDocument);

            if (ranges.LastOrDefault() is { } lastRange)
            {
                // We can fold consecutive usings if the last range we added *ends* the line *before* this directive.
                if (lastRange.EndLine + 1 == span.Start.Line)
                {
                    lastRange.EndLine = span.End.Line;
                    lastRange.EndCharacter = span.End.Character;
                    continue;
                }

                // We can fold consecutive usings if the last range we added *begins* the line *after* this directive.
                if (lastRange.StartLine - 1 == span.End.Line)
                {
                    lastRange.StartLine = span.Start.Line;
                    lastRange.StartCharacter = span.Start.Character;
                    continue;
                }
            }

            ranges.Add(LspFactory.CreateFoldingRange(FoldingRangeKind.Imports, span));
        }

        return ranges.ToImmutableAndClear();
    }
}

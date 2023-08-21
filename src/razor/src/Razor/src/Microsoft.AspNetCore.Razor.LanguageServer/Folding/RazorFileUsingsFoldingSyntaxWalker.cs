// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

internal sealed class RazorFileUsingsFoldingSyntaxWalker : SyntaxWalker
{
    private readonly RazorSourceDocument _source;
    internal List<FoldingRange> Ranges { get; }

    public RazorFileUsingsFoldingSyntaxWalker(RazorSourceDocument source)
    {
        _source = source;
        Ranges = new List<FoldingRange>();
    }

    public override void VisitRazorDirective(RazorDirectiveSyntax node)
    {
        // first rule: we can fold consecutive usings
        // if we have an existing range that ends the line *before* this current using directive or begins the line *after*,
        // we extend the range one line
        if (node.IsUsingDirective(out _))
        {
            var linePosition = node.GetLinePositionSpan(_source);

            var isPartOfExistingRange = false;
            if (Ranges is [.., { } lastRange])
            {
                if (lastRange.EndLine + 1 == linePosition.Start.Line)
                {
                    lastRange.EndLine = linePosition.End.Line;
                    lastRange.EndCharacter = linePosition.End.Character;
                    isPartOfExistingRange = true;
                }
                else if (lastRange.StartLine - 1 == linePosition.End.Line)
                {
                    lastRange.StartLine = linePosition.Start.Line;
                    lastRange.StartCharacter = linePosition.Start.Character;
                    isPartOfExistingRange = true;
                }
            }


            if (!isPartOfExistingRange)
            {
                Ranges.Add(new FoldingRange
                {
                    StartLine = linePosition.Start.Line,
                    StartCharacter = linePosition.Start.Character,
                    EndLine = linePosition.End.Line,
                    EndCharacter = linePosition.End.Character,
                    Kind = FoldingRangeKind.Imports
                });
            }
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class SemanticRange : IComparable<SemanticRange>
{
    public SemanticRange(int kind, Range range, int modifier)
    {
        Kind = kind;
        Modifier = modifier;
        Range = range;
    }

    public Range Range { get; }

    public int Kind { get; }

    public int Modifier { get; }

    public int CompareTo(SemanticRange? other)
    {
        if (other is null)
        {
            return 1;
        }

        var startCompare = Range.Start.CompareTo(other.Range.Start);
        return startCompare != 0
            ? startCompare
            : Range.End.CompareTo(other.Range.End);
    }

    public override string ToString()
        => $"[Kind: {Kind}, Range: {Range}]";
}

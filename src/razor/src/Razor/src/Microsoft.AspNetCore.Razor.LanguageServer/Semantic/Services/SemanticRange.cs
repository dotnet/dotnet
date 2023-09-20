// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal sealed class SemanticRange : IComparable<SemanticRange>
{
    public SemanticRange(int kind, Range range, int modifier, bool fromRazor)
    {
        Kind = kind;
        Modifier = modifier;
        Range = range;
        FromRazor = fromRazor;
    }

    public Range Range { get; }

    public int Kind { get; }

    public int Modifier { get; }

    /// <summary>
    /// If we produce a token, and a delegated server produces a token, we want to prefer ours, so we use this flag to help our
    /// sort algorithm, that way we can avoid the perf hit of actually finding duplicates, and just take the first instance that
    /// covers a range.
    /// </summary>
    public bool FromRazor { get; }

    public int CompareTo(SemanticRange? other)
    {
        if (other is null)
        {
            return 1;
        }

        var startCompare = Range.Start.CompareTo(other.Range.Start);
        if (startCompare != 0)
        {
            return startCompare;
        }

        var endCompare = Range.End.CompareTo(other.Range.End);
        if (endCompare != 0)
        {
            return endCompare;
        }

        // If we have ranges that are the same, we want a Razor produced token to win over a non-Razor produced token
        if (FromRazor && !other.FromRazor)
        {
            return -1;
        }
        else if (other.FromRazor && !FromRazor)
        {
            return 1;
        }

        return 0;
    }

    public override string ToString()
        => $"[Kind: {Kind}, Range: {Range}]";
}

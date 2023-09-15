// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal readonly struct SemanticRange : IComparable<SemanticRange>
{
    public SemanticRange(int kind, int startLine, int startCharacter, int endLine, int endCharacter, int modifier, bool fromRazor)
    {
        Kind = kind;
        StartLine = startLine;
        StartCharacter = startCharacter;
        EndLine = endLine;
        EndCharacter = endCharacter;
        Modifier = modifier;
        FromRazor = fromRazor;
    }

    public int Kind { get; }

    public int StartLine { get; }
    public int EndLine { get; }
    public int StartCharacter { get; }
    public int EndCharacter { get; }

    public int Modifier { get; }

    /// <summary>
    /// If we produce a token, and a delegated server produces a token, we want to prefer ours, so we use this flag to help our
    /// sort algorithm, that way we can avoid the perf hit of actually finding duplicates, and just take the first instance that
    /// covers a range.
    /// </summary>
    public bool FromRazor { get; }

    public LinePositionSpan AsLinePositionSpan()
        => new(new(StartLine, StartCharacter), new(EndLine, EndCharacter));

    public int CompareTo(SemanticRange other)
    {
        var result = StartLine.CompareTo(other.StartLine);
        if (result != 0)
        {
            return result;
        }

        result = StartCharacter.CompareTo(other.StartCharacter);
        if (result != 0)
        {
            return result;
        }

        result = EndLine.CompareTo(other.EndLine);
        if (result != 0)
        {
            return result;
        }

        result = EndCharacter.CompareTo(other.EndCharacter);
        if (result != 0)
        {
            return result;
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
        => $"[Kind: {Kind}, StartLine: {StartLine}, StartCharacter: {StartCharacter}, EndLine: {EndLine}, EndCharacter: {EndCharacter}]";
}

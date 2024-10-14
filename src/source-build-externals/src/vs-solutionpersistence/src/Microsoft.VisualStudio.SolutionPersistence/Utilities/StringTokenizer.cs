// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.SolutionPersistence.Utilities;

/// <summary>
/// Similar to original parser StringTokenizer class. With slight additions.
/// </summary>
internal ref struct StringTokenizer
{
    private readonly string line;
    private int currentPos;
    private StringSpan state;

    internal StringTokenizer(string? str)
    {
        this.IsNull = str is null;
        this.line = str ?? string.Empty;
        this.currentPos = 0;
        this.state = this.line.AsSpan();
    }

    internal bool IsNull { get; }

    internal readonly bool IsEmpty => this.state.IsEmpty;

    // First char in remaining line, '\0' if empty.
    internal readonly char CurrentChar => this.state.IsEmpty ? '\0' : this.state[0];

    internal readonly StringSpan Current => this.state;

    internal readonly int CurrentPos => this.currentPos;

    internal readonly string StringLine => this.line;

    // charact in given position, or 0 if index is out of bounds.
    internal readonly char this[int index] => index >= 0 && index < this.state.Length ? this.state[index] : '\0';

    // both use the same semantic as VS parser, with minor reduction in slicing and dicing ...
    internal StringSpan NextToken(string delimiters)
    {
        if (this.IsEmpty)
        {
            return StringSpan.Empty;
        }

        int skipLeading = 0;
        while (skipLeading < this.state.Length && delimiters.Contains(this.state[skipLeading]))
        {
            skipLeading++;
        }

        int nextDelimiter = skipLeading;
        while (nextDelimiter < this.state.Length && delimiters.IndexOf(this.state[nextDelimiter]) < 0)
        {
            nextDelimiter++;
        }

        return this.GetNextToken(skipLeading, nextDelimiter);
    }

    internal StringSpan NextTokenKeep(char delimiter)
    {
        if (this.IsEmpty)
        {
            return StringSpan.Empty;
        }

        int skipLeading = 0;
        while (skipLeading < this.state.Length && this.state[skipLeading] == delimiter)
        {
            skipLeading++;
        }

        int nextDelimiter = skipLeading;
        while (nextDelimiter < this.state.Length && this.state[nextDelimiter] != delimiter)
        {
            nextDelimiter++;
        }

        StringSpan result = nextDelimiter > skipLeading ? this.state.Slice(skipLeading, nextDelimiter - skipLeading) : StringSpan.Empty;
        this.currentPos += nextDelimiter;
        this.state = this.state.Slice(nextDelimiter);

        return result;
    }

    internal StringSpan NextToken(char delimiter)
    {
        if (this.IsEmpty)
        {
            return StringSpan.Empty;
        }

        int skipLeading = 0;
        while (skipLeading < this.state.Length && this.state[skipLeading] == delimiter)
        {
            skipLeading++;
        }

        int nextDelimiter = skipLeading;
        while (nextDelimiter < this.state.Length && this.state[nextDelimiter] != delimiter)
        {
            nextDelimiter++;
        }

        return this.GetNextToken(skipLeading, nextDelimiter);
    }

    internal void TrimStart()
    {
        int old = this.state.Length;
        this.state = this.state.TrimStart();
        this.currentPos += old - this.state.Length;
    }

    internal readonly bool StartsWithAt(string match, int pos)
    {
        if (string.IsNullOrEmpty(match) || this.state.Length < match.Length + pos)
        {
            return false;
        }

        return this.state.Slice(pos).StartsWith(match);
    }

    internal readonly bool StartsWith(string match)
    {
        if (string.IsNullOrEmpty(match) || this.state.Length < match.Length)
        {
            return false;
        }

        return this.state.StartsWith(match);
    }

    // will advance tokenizer if it starts with the specified prefix, but only if it is followed by whitesapce or end of line.
    internal bool SliceIfStartsWithAndEmptyAfter(string prefix) => this[prefix.Length].IsWhiteSpace() && this.SliceIfStartsWith(prefix);

    // will advance tokenizer if it starts with the specified prefix.
    internal bool SliceIfStartsWith(string prefix)
    {
        if (this.StartsWith(prefix))
        {
            this.Slice(prefix.Length);
            return true;
        }

        return false;
    }

    internal void Slice(int start)
    {
        if (start == 0)
        {
            return;
        }

        if (start >= 0 && start < this.state.Length)
        {
            this.currentPos += start;
            this.state = this.state.Slice(start);
        }
        else
        {
            this.currentPos += this.state.Length;
            this.state = StringSpan.Empty;
        }
    }

    internal void TrimStartAndSkip(char c1, char c2 = (char)0)
    {
        if (this.IsEmpty)
        {
            return;
        }

        int skipLeading = 0;
        while (skipLeading < this.state.Length)
        {
            char c = this.state[skipLeading];
            if (!char.IsWhiteSpace(c) && c != c1 && c != c2)
            {
                break;
            }

            skipLeading++;
        }

        this.Slice(skipLeading);
    }

    internal void Skip(string toSkip)
    {
        if (this.IsEmpty || string.IsNullOrEmpty(toSkip))
        {
            return;
        }

        int skipLeading = 0;
        while (skipLeading < this.state.Length && toSkip.Contains(this.state[skipLeading]))
        {
            skipLeading++;
        }

        this.Slice(skipLeading);
    }

    internal void SkipAll() => this.Slice(-1);

    internal void Reset()
    {
        this.currentPos = 0;
        this.state = this.line.AsSpan();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private StringSpan GetNextToken(int skipLeading, int nextDelimiter)
    {
        StringSpan result = nextDelimiter > skipLeading ? this.state.Slice(skipLeading, nextDelimiter - skipLeading) : StringSpan.Empty;

        // note +1 capture the case when a delimiter is the last character. The code would always remove the closing delimiter if any.
        nextDelimiter++;
        if (nextDelimiter < this.state.Length)
        {
            this.currentPos += nextDelimiter;
            this.state = this.state.Slice(nextDelimiter);
        }
        else
        {
            this.currentPos += this.state.Length;
            this.state = StringSpan.Empty;
        }

        return result;
    }
}

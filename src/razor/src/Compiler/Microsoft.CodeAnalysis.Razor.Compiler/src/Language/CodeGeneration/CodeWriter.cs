// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.PooledObjects;
using static System.StringExtensions;

namespace Microsoft.AspNetCore.Razor.Language.CodeGeneration;

public sealed partial class CodeWriter : IDisposable
{
    // This is the size of each "page", which are arrays of ReadOnlyMemory<char>.
    // This number was chosen arbitrarily as a "best guess". If changed, care should be
    // taken to ensure that pages are not allocated on the LOH. ReadOnlyMemory<char>
    // takes up 16 bytes, so a page size of 1000 is 16k.
    private const int MinimumPageSize = 1000;

    // Rather than using a StringBuilder, we maintain a linked list of pages, which are arrays
    // of "chunks of text", represented by ReadOnlyMemory<char>. This avoids copying strings
    // into a StringBuilder's internal char arrays only to copy them out later in
    // StringBuilder.ToString(). This also avoids string duplication by holding onto the strings
    // themselves. So, if the same string instance is added multiple times, we won't duplicate it
    // each time. Instead, we'll hold a ReadOnlyMemory<char> for the string.
    //
    // Note that LinkedList<T> was chosen to avoid copying for especially large generated code files.
    // In addition, because LinkedList<T> provides direct access to the last element, appending
    // is extremely efficient.
    private readonly LinkedList<ReadOnlyMemory<char>[]> _pages;
    private int _pageOffset;
    private char? _lastChar;

    private string _newLine;
    private int _indentSize;
    private ReadOnlyMemory<char> _indentString;

    private int _absoluteIndex;
    private int _currentLineIndex;
    private int _currentLineCharacterIndex;

    public CodeWriter()
        : this(RazorCodeGenerationOptions.Default)
    {
    }

    public CodeWriter(RazorCodeGenerationOptions options)
    {
        SetNewLine(options.NewLine);
        IndentWithTabs = options.IndentWithTabs;
        TabSize = options.IndentSize;

        _indentSize = 0;
        _indentString = ReadOnlyMemory<char>.Empty;

        _pages = new();
    }

    private void AddTextChunk(ReadOnlyMemory<char> value)
    {
        if (value.Length == 0)
        {
            return;
        }

        // If we're at the start of a page, we need to add the page first.
        ReadOnlyMemory<char>[] lastPage;

        if (_pageOffset == 0)
        {
            lastPage = ArrayPool<ReadOnlyMemory<char>>.Shared.Rent(MinimumPageSize);
            _pages.AddLast(lastPage);
        }
        else
        {
            lastPage = _pages.Last!.Value;
        }

        // Add our chunk of text (the ReadOnlyMemory<char>) and increment the offset.
        lastPage[_pageOffset] = value;
        _pageOffset++;

        // We've reached the end of a page, so we reset the offset to 0.
        // This will cause a new page to be added next time.
        // _pageOffset is checked against the lastPage.Length as the Rent call that
        // return that array may return an array longer that MinimumPageSize.
        if (_pageOffset == lastPage.Length)
        {
            _pageOffset = 0;
        }

        // Remember the last character of the text chunk we just added.
        _lastChar = value.Span[^1];
    }

    public int CurrentIndent
    {
        get => _indentSize;
        set
        {
            ArgHelper.ThrowIfNegative(value);

            if (_indentSize != value)
            {
                _indentSize = value;
                _indentString = ComputeIndent(value, IndentWithTabs, TabSize);
            }
        }
    }

    // Because of how _absoluteIndex is computed, it is effectively the length
    // of what has been written.
    public int Length => _absoluteIndex;

    public string NewLine
    {
        get => _newLine;
        set => SetNewLine(value);
    }

    [MemberNotNull(nameof(_newLine))]
    private void SetNewLine(string value)
    {
        ArgHelper.ThrowIfNull(value);

        if (value != "\r\n" && value != "\n")
        {
            throw new ArgumentException(Resources.FormatCodeWriter_InvalidNewLine(value), nameof(value));
        }

        _newLine = value;
    }

    public bool IndentWithTabs { get; }

    public int TabSize { get; }

    public SourceLocation Location => new(_absoluteIndex, _currentLineIndex, _currentLineCharacterIndex);

    public char this[int index]
    {
        get
        {
            // This Debug.Fail(...) is present because no Razor code currently accesses this
            // indexer and it isn't implemented efficiently. All Razor code that previously
            // used the indexer were really just inspecting the last char, which is now exposed separately.
            Debug.Fail("Do not use this indexer without reimplementing it more efficiently.");

            foreach (var page in _pages)
            {
                foreach (var chars in page)
                {
                    if (index < chars.Length)
                    {
                        return chars.Span[index];
                    }

                    index -= chars.Length;
                }
            }

            throw new IndexOutOfRangeException(nameof(index));
        }
    }

    public char? LastChar => _lastChar;

    public CodeWriter Indent(int size)
    {
        if (size == 0 || LastChar is not '\n')
        {
            return this;
        }

        var indentString = size == _indentSize
            ? _indentString
            : ComputeIndent(size, IndentWithTabs, TabSize);

        AddTextChunk(indentString);

        var indentLength = indentString.Length;
        _currentLineCharacterIndex += indentLength;
        _absoluteIndex += indentLength;

        return this;
    }

    private static ReadOnlyMemory<char> ComputeIndent(int size, bool useTabs, int tabSize)
    {
        if (size == 0)
        {
            return ReadOnlyMemory<char>.Empty;
        }

        if (useTabs)
        {
            var tabCount = size / tabSize;
            var spaceCount = size % tabSize;

            using var _ = StringBuilderPool.GetPooledObject(out var builder);
            builder.SetCapacityIfLarger(tabCount + spaceCount);

            builder.Append('\t', tabCount);
            builder.Append(' ', spaceCount);

            return builder.ToString().AsMemory();
        }

        return new string(' ', size).AsMemory();
    }

    public CodeWriter Write(string value)
    {
        ArgHelper.ThrowIfNull(value);

        return WriteCore(value.AsMemory());
    }

    public CodeWriter Write(ReadOnlyMemory<char> value)
        => WriteCore(value);

    public CodeWriter Write(string value, int startIndex, int count)
    {
        ArgHelper.ThrowIfNull(value);
        ArgHelper.ThrowIfNegative(startIndex);
        ArgHelper.ThrowIfNegative(count);
        ArgHelper.ThrowIfGreaterThan(startIndex, value.Length - count);

        return WriteCore(value.AsMemory(startIndex, count));
    }

    public CodeWriter Write([InterpolatedStringHandlerArgument("")] ref WriteInterpolatedStringHandler handler)
        => this;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CodeWriter WriteCore(ReadOnlyMemory<char> value, bool allowIndent = true)
    {
        if (value.IsEmpty)
        {
            return this;
        }

        if (allowIndent)
        {
            Indent(_indentSize);
        }

        var lastChar = _lastChar;

        AddTextChunk(value);

        var span = value.Span;

        _absoluteIndex += span.Length;

        // Check the last character *before* the write and the first character of the span that
        // was written to determine whether this is a new-line that is spread across two writes.
        if (lastChar == '\r' && span[0] == '\n')
        {
            // Skip the first character of span to ensure that it isn't considered in the following
            // line break detection loop.
            span = span[1..];
        }

        // Iterate the span, stopping at each occurrence of a new-line character.
        // This lets us count the new-line occurrences and keep the index of the last one.
        int newLineIndex;
        while ((newLineIndex = span.IndexOfAny('\r', '\n')) >= 0)
        {
            _currentLineIndex++;
            _currentLineCharacterIndex = 0;

            newLineIndex++;

            // We might have stopped at a \r, so check if it's followed by \n and then advance the index.
            // Otherwise, we'll count this line break twice.
            if (newLineIndex < span.Length &&
                span[newLineIndex - 1] == '\r' &&
                span[newLineIndex] == '\n')
            {
                newLineIndex++;
            }

            span = span[newLineIndex..];
        }

        _currentLineCharacterIndex += span.Length;

        return this;
    }

    public CodeWriter WriteLine()
        => WriteCore(_newLine.AsMemory(), allowIndent: false);

    public CodeWriter WriteLine(ReadOnlyMemory<char> value)
        => WriteCore(value).WriteLine();

    public CodeWriter WriteLine(string value)
    {
        ArgHelper.ThrowIfNull(value);

        return WriteCore(value.AsMemory()).WriteLine();
    }

    public CodeWriter WriteLine([InterpolatedStringHandlerArgument("")] ref WriteInterpolatedStringHandler handler)
        => WriteLine();

    public string GenerateCode()
    {
        // Eventually, we need to remove this and not return a giant string, which can
        // easily be allocated on the LOH. The work to remove this is tracked by
        // https://github.com/dotnet/razor/issues/8076.
        return CreateString(Length, _pages, static (span, pages) =>
        {
            foreach (var page in pages)
            {
                foreach (var chars in page)
                {
                    if (chars.IsEmpty)
                    {
                        return;
                    }

                    chars.Span.CopyTo(span);
                    span = span[chars.Length..];

                    Debug.Assert(span.Length >= 0);
                }
            }

            Debug.Assert(span.Length == 0, "We didn't fill the whole span!");
        });
    }

    public void Dispose()
    {
        foreach (var page in _pages)
        {
            ArrayPool<ReadOnlyMemory<char>>.Shared.Return(page, clearArray: true);
        }

        _pages.Clear();
    }
}

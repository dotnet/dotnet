// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language.CodeGeneration;

public sealed partial class CodeWriter
{
    // This is the size of each "page", which are arrays of ReadOnlyMemory<char>.
    // This number was chosen arbitrarily as a "best guess". If changed, care should be
    // taken to ensure that pages are not allocated on the LOH. ReadOnlyMemory<char>
    // takes up 16 bytes, so a page size of 1000 is 16k.
    private const int PageSize = 1000;

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
        : this(Environment.NewLine, RazorCodeGenerationOptions.CreateDefault())
    {
    }

    public CodeWriter(string newLine, RazorCodeGenerationOptions options)
    {
        SetNewLine(newLine);
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
        var lastPage = _pageOffset == 0
            ? _pages.AddLast(new ReadOnlyMemory<char>[PageSize]).Value
            : _pages.Last.Value;

        // Add our chunk of text (the ReadOnlyMemory<char>) and increment the offset.
        lastPage[_pageOffset] = value;
        _pageOffset++;

        // We've reached the end of a page, so we reset the offset to 0.
        // This will cause a new page to be added next time.
        if (_pageOffset == PageSize)
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
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

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
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

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
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return WriteCore(value.AsMemory());
    }

    public CodeWriter Write(ReadOnlyMemory<char> value)
        => WriteCore(value);

    public CodeWriter Write(string value, int startIndex, int count)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (startIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        if (startIndex > value.Length - count)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        return WriteCore(value.AsMemory(startIndex, count));
    }

    public CodeWriter Write([InterpolatedStringHandlerArgument("")] ref WriteInterpolatedStringHandler handler)
        => this;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe CodeWriter WriteCore(ReadOnlyMemory<char> value, bool allowIndent = true)
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
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return WriteCore(value.AsMemory()).WriteLine();
    }

    public CodeWriter WriteLine([InterpolatedStringHandlerArgument("")] ref WriteInterpolatedStringHandler handler)
        => WriteLine();

    public string GenerateCode()
    {
        unsafe
        {
            // This might look a bit scary, but it's pretty simple. We allocate our string
            // with the correct length up front and then use simple pointer math to copy
            // the pages of ReadOnlyMemory<char> directly into it.

            // Eventually, we need to remove this and not return a giant string, which can
            // easily be allocated on the LOH. The work to remove this is tracked by
            // https://github.com/dotnet/razor/issues/8076.

            var length = Length;
            var result = new string('\0', length);

            fixed (char* stringPtr = result)
            {
                var destination = stringPtr;

                // destinationSize and sourceSize track the number of bytes (not chars).
                var destinationSize = length * sizeof(char);

                foreach (var page in _pages)
                {
                    foreach (var chars in page)
                    {
                        var source = chars.Span;
                        var sourceSize = source.Length * sizeof(char);

                        fixed (char* srcPtr = source)
                        {
                            Buffer.MemoryCopy(srcPtr, destination, destinationSize, sourceSize);
                        }

                        destination += source.Length;
                        destinationSize -= sourceSize;

                        Debug.Assert(destinationSize >= 0);
                    }
                }

                Debug.Assert(destinationSize == 0, "We didn't exhaust our destination pointer!");
            }

            return result;
        }
    }
}

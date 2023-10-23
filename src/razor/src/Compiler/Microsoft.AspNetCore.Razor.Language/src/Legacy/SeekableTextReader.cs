// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.IO;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal sealed class SeekableTextReader : TextReader, ITextDocument
{
    private readonly RazorSourceDocument _sourceDocument;
    private readonly string _filePath;
    private int _position;
    private int _current;
    private SourceLocation _location;
    private (TextSpan Span, int LineIndex) _cachedLineInfo;

    public SeekableTextReader(string source, string filePath) : this(RazorSourceDocument.Create(source, Encoding.UTF8, RazorSourceDocumentProperties.Create(filePath, relativePath: null))) { }

    public SeekableTextReader(RazorSourceDocument source)
    {
        _sourceDocument = source;
        _filePath = source.FilePath;
        _cachedLineInfo = (_sourceDocument.Text.Lines[0].Span, 0);
        UpdateState();
    }

    public SourceLocation Location => _location;

    public int Length => _sourceDocument.Text.Length;

    public int Position
    {
        get { return _position; }
        set
        {
            if (_position != value)
            {
                _position = value;
                UpdateState();
            }
        }
    }

    public override int Read()
    {
        var c = _current;
        _position++;
        UpdateState();
        return c;
    }

    public override int Peek() => _current;

    private void UpdateState()
    {
        if (_cachedLineInfo.Span.Contains(_position))
        {
            _location = new SourceLocation(_filePath, _position, _cachedLineInfo.LineIndex, _position - _cachedLineInfo.Span.Start);
            _current = _sourceDocument.Text[_location.AbsoluteIndex];

            return;
        }

        if (_position < _sourceDocument.Text.Length)
        {
            if (_position >= _cachedLineInfo.Span.End)
            {
                // Try to avoid the GetLocation call by checking if the next line contains the position
                var nextLineIndex = _cachedLineInfo.LineIndex + 1;
                var nextLineSpan = _sourceDocument.Text.Lines[nextLineIndex].Span;

                if (nextLineSpan.Contains(_position))
                {
                    _cachedLineInfo = (nextLineSpan, nextLineIndex);
                    _location = new SourceLocation(_filePath, _position, nextLineIndex, _position - nextLineSpan.Start);
                    _current = _sourceDocument.Text[_location.AbsoluteIndex];

                    return;
                }
            }
            else
            {
                // Try to avoid the GetLocation call by checking if the previous line contains the position
                var prevLineIndex = _cachedLineInfo.LineIndex - 1;
                var prevLineSpan = _sourceDocument.Text.Lines[prevLineIndex].Span;

                if (prevLineSpan.Contains(_position))
                {
                    _cachedLineInfo = (prevLineSpan, prevLineIndex);
                    _location = new SourceLocation(_filePath, _position, prevLineIndex, _position - prevLineSpan.Start);
                    _current = _sourceDocument.Text[_location.AbsoluteIndex];

                    return;
                }
            }

            // The call to GetLocation is expensive
            _location = new SourceLocation(_sourceDocument.FilePath, _position, _sourceDocument.Text.Lines.GetLinePosition(_position));

            var lineSpan = _sourceDocument.Text.Lines[_location.LineIndex].Span;
            _cachedLineInfo = (lineSpan, _location.LineIndex);

            _current = _sourceDocument.Text[_location.AbsoluteIndex];

            return;
        }

        if (_sourceDocument.Text.Length == 0)
        {
            _location = SourceLocation.Zero;
            _current = -1;

            return;
        }

        var lineNumber = _sourceDocument.Text.Lines.Count - 1;
        _location = new SourceLocation(_filePath, Length, lineNumber, _sourceDocument.Text.Lines[lineNumber].Span.Length);

        _current = -1;
    }
}

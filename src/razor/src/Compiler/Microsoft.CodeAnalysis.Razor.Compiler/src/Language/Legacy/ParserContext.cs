// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal sealed partial class ParserContext : IDisposable
{
    private readonly Stack<ErrorSink> _errorSinkStack;
    private readonly HashSet<string> _seenDirectivesSet;

    public RazorSourceDocument SourceDocument { get; }
    public SeekableTextReader Source { get; }
    public RazorParserOptions Options { get; }

    public bool WhiteSpaceIsSignificantToAncestorBlock { get; set; }
    public bool NullGenerateWhitespaceAndNewLine { get; set; }
    public bool InTemplateContext { get; set; }
    public bool StartOfLine { get; set; } = true;
    public bool MakeMarkerNode { get; set; } = true;
    public AcceptedCharactersInternal CurrentAcceptedCharacters { get; set; } = AcceptedCharactersInternal.Any;

    public ParserContext(RazorSourceDocument source, RazorParserOptions options)
    {
        ArgHelper.ThrowIfNull(source);
        ArgHelper.ThrowIfNull(options);

        SourceDocument = source;
        Options = options;

        _errorSinkStack = StackPool<ErrorSink>.Default.Get();
        _errorSinkStack.Push(new ErrorSink());

        _seenDirectivesSet = StringHashSetPool.Ordinal.Get();

        Source = new SeekableTextReader(SourceDocument);
    }

    public void Dispose()
    {
        while (_errorSinkStack.Count > 0)
        {
            var errorSink = _errorSinkStack.Pop();
            errorSink.Dispose();
        }

        StackPool<ErrorSink>.Default.Return(_errorSinkStack);
        StringHashSetPool.Ordinal.Return(_seenDirectivesSet);
    }

    public ErrorSink ErrorSink => _errorSinkStack.Peek();

    public HashSet<string> SeenDirectives => _seenDirectivesSet;

    public bool DesignTimeMode => Options.DesignTime;

    public bool IsEndOfFile
        => Source.Peek() == -1;

    public ErrorScope PushNewErrorScope(ErrorSink errorSink)
    {
        _errorSinkStack.Push(errorSink);
        return new(this);
    }
}

// Debug Helpers

#if DEBUG
[DebuggerDisplay("{" + nameof(DebuggerToString) + "(),nq}")]
internal partial class ParserContext
{
    private string Unparsed
    {
        get
        {
            var bookmark = Source.Position;
            var remaining = Source.ReadToEnd();
            Source.Position = bookmark;
            return remaining;
        }
    }

    private string DebuggerToString()
    {
        return Unparsed;
    }
}
#endif

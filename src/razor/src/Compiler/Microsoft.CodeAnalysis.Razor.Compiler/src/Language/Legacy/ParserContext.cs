// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

internal partial class ParserContext
{
    public ParserContext(RazorSourceDocument source, RazorParserOptions options)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        SourceDocument = source;

        Source = new SeekableTextReader(SourceDocument);
        DesignTimeMode = options.DesignTime;
        FeatureFlags = options.FeatureFlags;
        ParseLeadingDirectives = options.ParseLeadingDirectives;
        ErrorSink = new ErrorSink();
        SeenDirectives = new HashSet<string>(StringComparer.Ordinal);
        EnableSpanEditHandlers = options.EnableSpanEditHandlers;
    }

    public ErrorSink ErrorSink { get; set; }

    public RazorParserFeatureFlags FeatureFlags { get; }

    public HashSet<string> SeenDirectives { get; }

    public SeekableTextReader Source { get; }

    public RazorSourceDocument SourceDocument { get; }

    public bool DesignTimeMode { get; }

    public bool ParseLeadingDirectives { get; }

    public bool EnableSpanEditHandlers { get; }

    public bool WhiteSpaceIsSignificantToAncestorBlock { get; set; }

    public bool NullGenerateWhitespaceAndNewLine { get; set; }

    public bool InTemplateContext { get; set; }

    public bool StartOfLine { get; set; } = true;

    public bool MakeMarkerNode { get; set; } = true;

    public AcceptedCharactersInternal CurrentAcceptedCharacters { get; set; } = AcceptedCharactersInternal.Any;

    public bool EndOfFile
    {
        get { return Source.Peek() == -1; }
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

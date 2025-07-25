﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

// We have IVT access to the Roslyn APIs for product code, but not for testing.
internal readonly struct ExcerptResultInternal
{
    public readonly SourceText Content;
    public readonly TextSpan MappedSpan;
    public readonly ImmutableArray<ClassifiedSpan> ClassifiedSpans;
    public readonly Document Document;
    public readonly TextSpan Span;

    public ExcerptResultInternal(
        SourceText content,
        TextSpan mappedSpan,
        ImmutableArray<ClassifiedSpan> classifiedSpans,
        Document document,
        TextSpan span)
    {
        Content = content;
        MappedSpan = mappedSpan;
        ClassifiedSpans = classifiedSpans;
        Document = document;
        Span = span;
    }

    public RazorExcerptResult ToExcerptResult()
    {
        return new RazorExcerptResult(Content, MappedSpan, ClassifiedSpans, Document, Span);
    }
}

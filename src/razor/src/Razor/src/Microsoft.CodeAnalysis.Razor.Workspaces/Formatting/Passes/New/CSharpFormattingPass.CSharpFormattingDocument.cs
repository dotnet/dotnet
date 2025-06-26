// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting.New;

internal partial class CSharpFormattingPass
{
    private readonly record struct CSharpFormattingDocument(SourceText SourceText, ImmutableArray<LineInfo> LineInfo);
}

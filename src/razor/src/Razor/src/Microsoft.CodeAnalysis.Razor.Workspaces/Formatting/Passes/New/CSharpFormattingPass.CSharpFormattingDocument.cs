// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Formatting.New;

internal partial class CSharpFormattingPass
{
    private readonly record struct CSharpFormattingDocument(SourceText SourceText, ImmutableArray<LineInfo> LineInfo);
}

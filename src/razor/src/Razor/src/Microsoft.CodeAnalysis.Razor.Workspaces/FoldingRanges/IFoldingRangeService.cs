// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.FoldingRanges;

internal interface IFoldingRangeService
{
    ImmutableArray<FoldingRange> GetFoldingRanges(RazorCodeDocument codeDocument, ImmutableArray<FoldingRange> csharpRanges, ImmutableArray<FoldingRange> htmlRanges, CancellationToken cancellationToken);
}

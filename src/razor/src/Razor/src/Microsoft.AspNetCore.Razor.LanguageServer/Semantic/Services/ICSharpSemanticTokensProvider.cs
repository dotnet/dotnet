// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal interface ICSharpSemanticTokensProvider
{
    Task<int[]?> GetCSharpSemanticTokensResponseAsync(
        VersionedDocumentContext documentContext,
        ImmutableArray<LinePositionSpan> csharpSpans,
        bool usePreciseSemanticTokenRanges,
        Guid correlationId,
        CancellationToken cancellationToken);
}

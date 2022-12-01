// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Folding;

internal abstract class RazorFoldingRangeProvider
{
    public abstract Task<ImmutableArray<FoldingRange>> GetFoldingRangesAsync(DocumentContext documentContext, CancellationToken cancellationToken);
}

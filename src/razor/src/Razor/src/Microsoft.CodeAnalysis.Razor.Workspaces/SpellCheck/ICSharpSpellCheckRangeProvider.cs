// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.SpellCheck;

internal interface ICSharpSpellCheckRangeProvider
{
    Task<ImmutableArray<SpellCheckRange>> GetCSharpSpellCheckRangesAsync(DocumentContext documentContext, CancellationToken cancellationToken);
}

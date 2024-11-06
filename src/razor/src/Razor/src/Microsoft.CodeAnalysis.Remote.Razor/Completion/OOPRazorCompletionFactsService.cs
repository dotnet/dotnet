// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis.Razor.Completion;

namespace Microsoft.CodeAnalysis.Remote.Razor.Completion;

[Export(typeof(IRazorCompletionFactsService)), Shared]
[method: ImportingConstructor]
internal sealed class OOPRazorCompletionFactsService([ImportMany] IEnumerable<IRazorCompletionItemProvider> providers)
    : AbstractRazorCompletionFactsService(providers.ToImmutableArray())
{
}

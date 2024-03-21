// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Razor.Completion;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Completion;

internal sealed class LspRazorCompletionFactsService(IEnumerable<IRazorCompletionItemProvider> providers)
    : AbstractRazorCompletionFactsService(providers.ToImmutableArray())
{
}

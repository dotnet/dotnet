// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor.Completion;

[Shared]
[Export(typeof(IRazorCompletionFactsService))]
internal class RazorCompletionFactsService : IRazorCompletionFactsService
{
    private readonly ImmutableArray<IRazorCompletionItemProvider> _providers;

    [ImportingConstructor]
    public RazorCompletionFactsService([ImportMany] IEnumerable<IRazorCompletionItemProvider> providers)
    {
        if (providers is null)
        {
            throw new ArgumentNullException(nameof(providers));
        }

        _providers = providers.ToImmutableArray();
    }

    public ImmutableArray<RazorCompletionItem> GetCompletionItems(RazorCompletionContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.TagHelperDocumentContext is null)
        {
            throw new ArgumentNullException(nameof(context.TagHelperDocumentContext));
        }

        using var completions = new PooledArrayBuilder<RazorCompletionItem>();

        foreach (var provider in _providers)
        {
            var items = provider.GetCompletionItems(context);
            completions.AddRange(items);
        }

        return completions.DrainToImmutable();
    }
}

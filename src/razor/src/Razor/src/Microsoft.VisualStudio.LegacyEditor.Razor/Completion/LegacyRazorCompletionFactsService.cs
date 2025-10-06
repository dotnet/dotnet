// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Completion;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

[Export(typeof(IRazorCompletionFactsService))]
internal sealed class LegacyRazorCompletionFactsService : AbstractRazorCompletionFactsService
{
    private static readonly ImmutableArray<IRazorCompletionItemProvider> s_providers =
    [
        new DirectiveAttributeCompletionItemProvider(),
        new DirectiveAttributeParameterCompletionItemProvider(),
        new DirectiveCompletionItemProvider()
    ];

    public LegacyRazorCompletionFactsService()
        : base(s_providers)
    {
    }
}

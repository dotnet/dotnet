// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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

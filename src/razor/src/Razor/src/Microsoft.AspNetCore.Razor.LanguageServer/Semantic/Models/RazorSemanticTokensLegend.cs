// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Reflection;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

[Export(typeof(RazorSemanticTokensLegendService))]
[method: ImportingConstructor]
internal sealed partial class RazorSemanticTokensLegendService(IClientCapabilitiesService clientCapabilitiesService)
{
    // DI calls this constructor to build the service container, but we can't access clientCapabilitiesService
    // until the language server has received the Initialize message, so we have to do this lazily.
    private readonly Lazy<Types> _typesLazy = new(() => new Types(clientCapabilitiesService));

    public Types TokenTypes => _typesLazy.Value;
    public Modifiers TokenModifiers { get; } = new Modifiers();
    public SemanticTokensLegend Legend => new SemanticTokensLegend()
    {
        TokenModifiers = TokenModifiers.TokenModifiers,
        TokenTypes = _typesLazy.Value.TokenTypes
    };

    private static ImmutableArray<string> GetStaticFieldValues(Type type)
    {
        using var _ = ArrayBuilderPool<string>.GetPooledObject(out var builder);

        foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Static))
        {
            if (field.GetValue(null) is string value)
            {
                builder.Add(value);
            }
        }

        return builder.ToImmutable();
    }
}

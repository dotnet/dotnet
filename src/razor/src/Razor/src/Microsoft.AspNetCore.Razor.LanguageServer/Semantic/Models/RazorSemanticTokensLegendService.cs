// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Semantic;

internal class RazorSemanticTokensLegendService(IClientCapabilitiesService clientCapabilitiesService) : ISemanticTokensLegendService
{
    private static readonly SemanticTokenModifiers s_modifiers = ConstructTokenModifiers();

    // DI calls this constructor to build the service container, but we can't access clientCapabilitiesService
    // until the language server has received the Initialize message, so we have to do this lazily.
    private readonly Lazy<SemanticTokenTypes> _typesLazy = new(() => ConstructTokenTypes(clientCapabilitiesService.ClientCapabilities.SupportsVisualStudioExtensions));

    public SemanticTokenTypes TokenTypes => _typesLazy.Value;
    public SemanticTokenModifiers TokenModifiers { get; } = s_modifiers;

    private static SemanticTokenTypes ConstructTokenTypes(bool supportsVsExtensions)
    {
        using var _ = ArrayBuilderPool<string>.GetPooledObject(out var builder);

        builder.AddRange(RazorSemanticTokensAccessor.GetTokenTypes(supportsVsExtensions));

        foreach (var razorTokenType in GetStaticFieldValues(typeof(SemanticTokenTypes)))
        {
            builder.Add(razorTokenType);
        }

        return new SemanticTokenTypes(builder.ToArray());
    }

    private static SemanticTokenModifiers ConstructTokenModifiers()
    {
        using var _ = ArrayBuilderPool<string>.GetPooledObject(out var builder);

        builder.AddRange(RazorSemanticTokensAccessor.GetTokenModifiers());

        foreach (var razorModifier in GetStaticFieldValues(typeof(SemanticTokenModifiers)))
        {
            builder.Add(razorModifier);
        }

        return new SemanticTokenModifiers(builder.ToArray());
    }

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

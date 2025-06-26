// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

// Run after the component tag helper provider
internal sealed class KeyTagHelperDescriptorProvider() : TagHelperDescriptorProviderBase(order: 1000)
{
    private static readonly Lazy<TagHelperDescriptor> s_keyTagHelper = new(CreateKeyTagHelper);

    public override void Execute(TagHelperDescriptorProviderContext context)
    {
        ArgHelper.ThrowIfNull(context);

        var compilation = context.Compilation;

        var renderTreeBuilderType = compilation.GetTypeByMetadataName(ComponentsApi.RenderTreeBuilder.FullTypeName);
        if (renderTreeBuilderType == null)
        {
            // If we can't find RenderTreeBuilder, then just bail. We won't be able to compile the
            // generated code anyway.
            return;
        }

        if (context.TargetSymbol is { } targetSymbol && !SymbolEqualityComparer.Default.Equals(targetSymbol, renderTreeBuilderType.ContainingAssembly))
        {
            return;
        }

        context.Results.Add(s_keyTagHelper.Value);
    }

    private static TagHelperDescriptor CreateKeyTagHelper()
    {
        using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
            ComponentMetadata.Key.TagHelperKind, "Key", ComponentsApi.AssemblyName,
            out var builder);

        builder.CaseSensitive = true;
        builder.SetDocumentation(DocumentationDescriptor.KeyTagHelper);

        builder.SetMetadata(
            SpecialKind(ComponentMetadata.Key.TagHelperKind),
            MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
            RuntimeName(ComponentMetadata.Key.RuntimeName),
            TypeName("Microsoft.AspNetCore.Components.Key"));

        builder.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.Attribute(attribute =>
            {
                attribute.Name = "@key";
                attribute.IsDirectiveAttribute = true;
            });
        });

        builder.BindAttribute(attribute =>
        {
            attribute.SetDocumentation(DocumentationDescriptor.KeyTagHelper);
            attribute.Name = "@key";

            attribute.TypeName = typeof(object).FullName;
            attribute.SetMetadata(
                PropertyName("Key"),
                IsDirectiveAttribute);
        });

        return builder.Build();
    }
}

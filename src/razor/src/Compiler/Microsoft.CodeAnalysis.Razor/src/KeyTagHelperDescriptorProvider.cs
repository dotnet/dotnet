// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

internal class KeyTagHelperDescriptorProvider : ITagHelperDescriptorProvider
{
    private static TagHelperDescriptor s_keyTagHelper;

    // Run after the component tag helper provider
    public int Order { get; set; } = 1000;

    public RazorEngine Engine { get; set; }

    public void Execute(TagHelperDescriptorProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var compilation = context.GetCompilation();
        if (compilation == null)
        {
            return;
        }

        var renderTreeBuilderType = compilation.GetTypeByMetadataName(ComponentsApi.RenderTreeBuilder.FullTypeName);
        if (renderTreeBuilderType == null)
        {
            // If we can't find RenderTreeBuilder, then just bail. We won't be able to compile the
            // generated code anyway.
            return;
        }

        var targetSymbol = context.Items.GetTargetSymbol();
        if (targetSymbol is not null && !SymbolEqualityComparer.Default.Equals(targetSymbol, renderTreeBuilderType.ContainingAssembly))
        {
            return;
        }

        context.Results.Add(GetOrCreateKeyTagHelper());
    }

    private static TagHelperDescriptor GetOrCreateKeyTagHelper()
    {
        return s_keyTagHelper ??= CreateKeyTagHelper();

        static TagHelperDescriptor CreateKeyTagHelper()
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
                    attribute.SetMetadata(Attributes.IsDirectiveAttribute);
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
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

internal class SplatTagHelperDescriptorProvider : ITagHelperDescriptorProvider
{
    private static TagHelperDescriptor s_splatTagHelper;

    // Order doesn't matter
    public int Order { get; set; }

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

        var renderTreeBuilder = compilation.GetTypeByMetadataName(ComponentsApi.RenderTreeBuilder.FullTypeName);
        if (renderTreeBuilder == null)
        {
            // If we can't find RenderTreeBuilder, then just bail. We won't be able to compile the
            // generated code anyway.
            return;
        }

        var targetSymbol = context.Items.GetTargetSymbol();
        if (targetSymbol is not null && !SymbolEqualityComparer.Default.Equals(targetSymbol, renderTreeBuilder.ContainingAssembly))
        {
            return;
        }

        context.Results.Add(GetOrCreateSplatTagHelper());
    }

    private static TagHelperDescriptor GetOrCreateSplatTagHelper()
    {
        return s_splatTagHelper ??= CreateSplatTagHelper();

        static TagHelperDescriptor CreateSplatTagHelper()
        {
            using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
                ComponentMetadata.Splat.TagHelperKind, "Attributes", ComponentsApi.AssemblyName,
                out var builder);

            builder.CaseSensitive = true;
            builder.SetDocumentation(DocumentationDescriptor.SplatTagHelper);

            builder.SetMetadata(
                SpecialKind(ComponentMetadata.Splat.TagHelperKind),
                MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
                RuntimeName(ComponentMetadata.Splat.RuntimeName),
                TypeName("Microsoft.AspNetCore.Components.Attributes"));

            builder.TagMatchingRule(rule =>
            {
                rule.TagName = "*";
                rule.Attribute(attribute =>
                {
                    attribute.Name = "@attributes";
                    attribute.SetMetadata(Attributes.IsDirectiveAttribute);
                });
            });

            builder.BindAttribute(attribute =>
            {
                attribute.SetDocumentation(DocumentationDescriptor.SplatTagHelper);
                attribute.Name = "@attributes";

                attribute.TypeName = typeof(object).FullName;
                attribute.SetMetadata(
                    PropertyName("Attributes"),
                    IsDirectiveAttribute);
            });

            return builder.Build();
        }
    }
}

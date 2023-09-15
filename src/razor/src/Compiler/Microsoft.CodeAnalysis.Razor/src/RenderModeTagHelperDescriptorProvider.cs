// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

internal sealed class RenderModeTagHelperDescriptorProvider : ITagHelperDescriptorProvider
{
    private static readonly Lazy<TagHelperDescriptor> s_refTagHelper = new(CreateRenderModeTagHelper);

    // Run after the component tag helper provider
    public int Order { get; set; } = 1000;

    public RazorEngine? Engine { get; set; }

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

        var iComponentRenderMode = compilation.GetTypeByMetadataName(ComponentsApi.IComponentRenderMode.FullTypeName);
        if (iComponentRenderMode == null)
        {
            // If we can't find IComponentRenderMode, then just bail. We won't be able to compile the
            // generated code anyway.
            return;
        }

        var targetSymbol = context.Items.GetTargetSymbol();
        if (targetSymbol is not null && !SymbolEqualityComparer.Default.Equals(targetSymbol, iComponentRenderMode.ContainingAssembly))
        {
            return;
        }

        context.Results.Add(s_refTagHelper.Value);
    }

    private static TagHelperDescriptor CreateRenderModeTagHelper()
    {
        using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
            ComponentMetadata.RenderMode.TagHelperKind, "RenderMode", ComponentsApi.AssemblyName,
            out var builder);

        builder.CaseSensitive = true;

        builder.SetDocumentation(DocumentationDescriptor.RenderModeTagHelper);

        builder.SetMetadata(
            SpecialKind(ComponentMetadata.RenderMode.TagHelperKind),
            MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
            RuntimeName(ComponentMetadata.RenderMode.RuntimeName),
            TypeName("Microsoft.AspNetCore.Components.RenderMode"));

        builder.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.Attribute(attribute =>
            {
                attribute.Name = "@rendermode";
                attribute.SetMetadata(Attributes.IsDirectiveAttribute);
            });
        });

        builder.BindAttribute(attribute =>
        {
            attribute.SetDocumentation(DocumentationDescriptor.RenderModeTagHelper);
            attribute.Name = "@rendermode";

            attribute.TypeName = ComponentsApi.IComponentRenderMode.FullTypeName;
            attribute.SetMetadata(
                PropertyName("RenderMode"),
                IsDirectiveAttribute);
        });

        return builder.Build();
    }
}

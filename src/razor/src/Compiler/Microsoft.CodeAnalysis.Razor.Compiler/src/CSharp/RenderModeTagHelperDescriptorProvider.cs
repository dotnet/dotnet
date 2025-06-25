// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

// Run after the component tag helper provider
internal sealed class RenderModeTagHelperDescriptorProvider() : TagHelperDescriptorProviderBase(order: 1000)
{
    private static readonly Lazy<TagHelperDescriptor> s_renderModeTagHelper = new(CreateRenderModeTagHelper);

    public override void Execute(TagHelperDescriptorProviderContext context)
    {
        ArgHelper.ThrowIfNull(context);

        var compilation = context.Compilation;

        var iComponentRenderMode = compilation.GetTypeByMetadataName(ComponentsApi.IComponentRenderMode.FullTypeName);
        if (iComponentRenderMode == null)
        {
            // If we can't find IComponentRenderMode, then just bail. We won't be able to compile the
            // generated code anyway.
            return;
        }

        if (context.TargetSymbol is { } targetSymbol && !SymbolEqualityComparer.Default.Equals(targetSymbol, iComponentRenderMode.ContainingAssembly))
        {
            return;
        }

        context.Results.Add(s_renderModeTagHelper.Value);
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
                attribute.IsDirectiveAttribute = true;
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

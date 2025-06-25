// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

internal sealed class SplatTagHelperDescriptorProvider : TagHelperDescriptorProviderBase
{
    private static readonly Lazy<TagHelperDescriptor> s_splatTagHelper = new(CreateSplatTagHelper);

    public override void Execute(TagHelperDescriptorProviderContext context)
    {
        ArgHelper.ThrowIfNull(context);

        var compilation = context.Compilation;

        var renderTreeBuilder = compilation.GetTypeByMetadataName(ComponentsApi.RenderTreeBuilder.FullTypeName);
        if (renderTreeBuilder == null)
        {
            // If we can't find RenderTreeBuilder, then just bail. We won't be able to compile the
            // generated code anyway.
            return;
        }

        if (context.TargetSymbol is { } targetSymbol && !SymbolEqualityComparer.Default.Equals(targetSymbol, renderTreeBuilder.ContainingAssembly))
        {
            return;
        }

        context.Results.Add(s_splatTagHelper.Value);
    }

    private static TagHelperDescriptor CreateSplatTagHelper()
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
                attribute.IsDirectiveAttribute = true;
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

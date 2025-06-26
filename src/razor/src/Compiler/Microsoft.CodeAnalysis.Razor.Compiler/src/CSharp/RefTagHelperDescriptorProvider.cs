// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

// Run after the component tag helper provider, because later we may want component-type-specific variants of this
internal sealed class RefTagHelperDescriptorProvider() : TagHelperDescriptorProviderBase(order: 1000)
{
    private static readonly Lazy<TagHelperDescriptor> s_refTagHelper = new(CreateRefTagHelper);

    public override void Execute(TagHelperDescriptorProviderContext context)
    {
        ArgHelper.ThrowIfNull(context);

        var compilation = context.Compilation;

        var elementReference = compilation.GetTypeByMetadataName(ComponentsApi.ElementReference.FullTypeName);
        if (elementReference == null)
        {
            // If we can't find ElementRef, then just bail. We won't be able to compile the
            // generated code anyway.
            return;
        }

        if (context.TargetSymbol is { } targetSymbol && !SymbolEqualityComparer.Default.Equals(targetSymbol, elementReference.ContainingAssembly))
        {
            return;
        }

        context.Results.Add(s_refTagHelper.Value);
    }

    private static TagHelperDescriptor CreateRefTagHelper()
    {
        using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
            ComponentMetadata.Ref.TagHelperKind, "Ref", ComponentsApi.AssemblyName,
            out var builder);

        builder.CaseSensitive = true;
        builder.SetDocumentation(DocumentationDescriptor.RefTagHelper);

        builder.SetMetadata(
            SpecialKind(ComponentMetadata.Ref.TagHelperKind),
            MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
            RuntimeName(ComponentMetadata.Ref.RuntimeName),
            TypeName("Microsoft.AspNetCore.Components.Ref"));

        builder.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.Attribute(attribute =>
            {
                attribute.Name = "@ref";
                attribute.IsDirectiveAttribute = true;
            });
        });

        builder.BindAttribute(attribute =>
        {
            attribute.SetDocumentation(DocumentationDescriptor.RefTagHelper);
            attribute.Name = "@ref";

            attribute.TypeName = typeof(object).FullName;
            attribute.SetMetadata(
                PropertyName("Ref"),
                IsDirectiveAttribute);
        });

        return builder.Build();
    }
}

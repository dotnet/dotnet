// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

// Run after the component tag helper provider
internal sealed class FormNameTagHelperDescriptorProvider() : TagHelperDescriptorProviderBase(order: 1000)
{
    private static readonly Lazy<TagHelperDescriptor> s_formNameTagHelper = new(CreateFormNameTagHelper);

    public override void Execute(TagHelperDescriptorProviderContext context)
    {
        ArgHelper.ThrowIfNull(context);

        var compilation = context.Compilation;

        var renderTreeBuilders = compilation.GetTypesByMetadataName(ComponentsApi.RenderTreeBuilder.FullTypeName)
            .Where(static t => t.DeclaredAccessibility == Accessibility.Public &&
                t.GetMembers(ComponentsApi.RenderTreeBuilder.AddNamedEvent).Any(static m => m.DeclaredAccessibility == Accessibility.Public))
            .Take(2).ToArray();
        if (renderTreeBuilders is not [var renderTreeBuilder])
        {
            return;
        }

        if (context.TargetSymbol is { } targetSymbol && !SymbolEqualityComparer.Default.Equals(targetSymbol, renderTreeBuilder.ContainingAssembly))
        {
            return;
        }

        context.Results.Add(s_formNameTagHelper.Value);
    }

    private static TagHelperDescriptor CreateFormNameTagHelper()
    {
        using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
            kind: ComponentMetadata.FormName.TagHelperKind,
            name: "FormName",
            assemblyName: ComponentsApi.AssemblyName,
            builder: out var builder);

        builder.CaseSensitive = true;
        builder.SetDocumentation(DocumentationDescriptor.FormNameTagHelper);

        builder.SetMetadata(
            SpecialKind(ComponentMetadata.FormName.TagHelperKind),
            MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
            RuntimeName(ComponentMetadata.FormName.RuntimeName),
            TypeName("Microsoft.AspNetCore.Components.FormName"));

        builder.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.Attribute(attribute =>
            {
                attribute.Name = "@formname";
                attribute.IsDirectiveAttribute = true;
            });
        });

        builder.BindAttribute(attribute =>
        {
            attribute.SetDocumentation(DocumentationDescriptor.FormNameTagHelper);
            attribute.Name = "@formname";

            attribute.TypeName = typeof(string).FullName;
            attribute.SetMetadata(
                PropertyName("FormName"),
                IsDirectiveAttribute);
        });

        return builder.Build();
    }
}

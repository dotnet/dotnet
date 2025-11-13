// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.CodeAnalysis.Razor;

// Run after the component tag helper provider
internal sealed class FormNameTagHelperDescriptorProvider() : TagHelperDescriptorProviderBase(order: 1000)
{
    private static readonly Lazy<TagHelperDescriptor> s_formNameTagHelper = new(CreateFormNameTagHelper);

    public override void Execute(TagHelperDescriptorProviderContext context, CancellationToken cancellationToken = default)
    {
        ArgHelper.ThrowIfNull(context);

        var targetAssembly = context.TargetAssembly;
        if (targetAssembly is not null && targetAssembly.Name != ComponentsApi.AssemblyName)
        {
            return;
        }

        var compilation = context.Compilation;

        var renderTreeBuilders = compilation.GetTypesByMetadataName(ComponentsApi.RenderTreeBuilder.FullTypeName)
            .Where(static t => t.DeclaredAccessibility == Accessibility.Public &&
                t.GetMembers(ComponentsApi.RenderTreeBuilder.AddNamedEvent).Any(static m => m.DeclaredAccessibility == Accessibility.Public))
            .Take(2).ToArray();
        if (renderTreeBuilders is not [var renderTreeBuilder])
        {
            return;
        }

        if (targetAssembly is not null &&
            !SymbolEqualityComparer.Default.Equals(targetAssembly, renderTreeBuilder.ContainingAssembly))
        {
            return;
        }

        context.Results.Add(s_formNameTagHelper.Value);
    }

    private static TagHelperDescriptor CreateFormNameTagHelper()
    {
        using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
            kind: TagHelperKind.FormName,
            name: "FormName",
            assemblyName: ComponentsApi.AssemblyName,
            builder: out var builder);

        builder.SetTypeName(
            fullName: "Microsoft.AspNetCore.Components.FormName",
            typeNamespace: "Microsoft.AspNetCore.Components",
            typeNameIdentifier: "FormName");

        builder.CaseSensitive = true;
        builder.ClassifyAttributesOnly = true;
        builder.SetDocumentation(DocumentationDescriptor.FormNameTagHelper);

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
            attribute.IsDirectiveAttribute = true;
            attribute.PropertyName = "FormName";
        });

        return builder.Build();
    }
}

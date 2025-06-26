// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Components;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal static class SimpleTagHelpers
{
    public static ImmutableArray<TagHelperDescriptor> Default { get; }

    static SimpleTagHelpers()
    {
        var builder1 = TagHelperDescriptorBuilder.Create("Test1TagHelper", "TestAssembly");
        builder1.TagMatchingRule(rule => rule.TagName = "test1");
        builder1.SetMetadata(TypeName("Test1TagHelper"));
        builder1.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetMetadata(PropertyName("BoolVal"));
            attribute.TypeName = typeof(bool).FullName;
        });
        builder1.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetMetadata(PropertyName("IntVal"));
            attribute.TypeName = typeof(int).FullName;
        });

        var builder1WithRequiredParent = TagHelperDescriptorBuilder.Create("Test1TagHelper.SomeChild", "TestAssembly");
        builder1WithRequiredParent.TagMatchingRule(rule =>
        {
            rule.TagName = "SomeChild";
            rule.ParentTag = "test1";
        });
        builder1WithRequiredParent.SetMetadata(TypeName("Test1TagHelper.SomeChild"));
        builder1WithRequiredParent.BindAttribute(attribute =>
        {
            attribute.Name = "attribute";
            attribute.SetMetadata(PropertyName("Attribute"));
            attribute.TypeName = typeof(string).FullName;
        });

        var builder2 = TagHelperDescriptorBuilder.Create("Test2TagHelper", "TestAssembly");
        builder2.TagMatchingRule(rule => rule.TagName = "test2");
        builder2.SetMetadata(TypeName("Test2TagHelper"));
        builder2.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetMetadata(PropertyName("BoolVal"));
            attribute.TypeName = typeof(bool).FullName;
        });
        builder2.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetMetadata(PropertyName("IntVal"));
            attribute.TypeName = typeof(int).FullName;
        });

        var builder3 = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "Component1TagHelper", "TestAssembly");
        builder3.TagMatchingRule(rule => rule.TagName = "Component1");
        builder3.SetMetadata(
            TypeName("Component1"),
            TypeNamespace("System"), // Just so we can reasonably assume a using directive is in place
            TypeNameIdentifier("Component1"),
            new(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch));
        builder3.BindAttribute(attribute =>
        {
            attribute.Name = "bool-val";
            attribute.SetMetadata(PropertyName("BoolVal"));
            attribute.TypeName = typeof(bool).FullName;
        });
        builder3.BindAttribute(attribute =>
        {
            attribute.Name = "int-val";
            attribute.SetMetadata(PropertyName("IntVal"));
            attribute.TypeName = typeof(int).FullName;
        });
        builder3.BindAttribute(attribute =>
        {
            attribute.Name = "Title";
            attribute.SetMetadata(PropertyName("Title"));
            attribute.TypeName = typeof(string).FullName;
        });

        var textComponent = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TextTagHelper", "TestAssembly");
        textComponent.TagMatchingRule(rule => rule.TagName = "Text");
        textComponent.SetMetadata(
            TypeName("Text"),
            TypeNamespace("System"),
            TypeNameIdentifier("Text"),
            new(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch));

        var directiveAttribute1 = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "TestDirectiveAttribute", "TestAssembly");
        directiveAttribute1.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(b =>
            {
                b.Name = "@test";
                b.NameComparison = RequiredAttributeNameComparison.PrefixMatch;
            });
        });
        directiveAttribute1.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(b =>
            {
                b.Name = "@test";
                b.NameComparison = RequiredAttributeNameComparison.FullMatch;
            });
        });
        directiveAttribute1.BindAttribute(attribute =>
        {
            attribute.Name = "@test";
            attribute.SetMetadata(PropertyName("Test"), IsDirectiveAttribute);
            attribute.TypeName = typeof(string).FullName;

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "something";
                parameter.PropertyName = "Something";
                parameter.TypeName = typeof(string).FullName;
            });
        });
        directiveAttribute1.SetMetadata(
            MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
            new(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch),
            TypeName("TestDirectiveAttribute"));

        var directiveAttribute2 = TagHelperDescriptorBuilder.Create(ComponentMetadata.Component.TagHelperKind, "MinimizedDirectiveAttribute", "TestAssembly");
        directiveAttribute2.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(b =>
            {
                b.Name = "@minimized";
                b.NameComparison = RequiredAttributeNameComparison.PrefixMatch;
            });
        });
        directiveAttribute2.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(b =>
            {
                b.Name = "@minimized";
                b.NameComparison = RequiredAttributeNameComparison.FullMatch;
            });
        });
        directiveAttribute2.BindAttribute(attribute =>
        {
            attribute.Name = "@minimized";
            attribute.SetMetadata(PropertyName("Minimized"), IsDirectiveAttribute);
            attribute.TypeName = typeof(bool).FullName;

            attribute.BindAttributeParameter(parameter =>
            {
                parameter.Name = "something";
                parameter.PropertyName = "Something";
                parameter.TypeName = typeof(string).FullName;
            });
        });
        directiveAttribute2.SetMetadata(
            MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
            new(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch),
            TypeName("TestDirectiveAttribute"));

        var directiveAttribute3 = TagHelperDescriptorBuilder.Create(ComponentMetadata.EventHandler.TagHelperKind, "OnClickDirectiveAttribute", "TestAssembly");
        directiveAttribute3.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(attribute => attribute
                .Name("@onclick", RequiredAttributeNameComparison.FullMatch)
                .IsDirectiveAttribute());
        });
        directiveAttribute3.TagMatchingRule(rule =>
        {
            rule.TagName = "*";
            rule.RequireAttributeDescriptor(attribute => attribute
                .Name("@onclick", RequiredAttributeNameComparison.PrefixMatch)
                .IsDirectiveAttribute());
        });
        directiveAttribute3.BindAttribute(attribute =>
        {
            attribute.Name = "@onclick";
            attribute.SetMetadata(PropertyName("onclick"), IsDirectiveAttribute, IsWeaklyTyped);
            attribute.TypeName = "Microsoft.AspNetCore.Components.EventCallback<Microsoft.AspNetCore.Components.Web.MouseEventArgs>";
        });
        directiveAttribute3.SetMetadata(
            RuntimeName(ComponentMetadata.EventHandler.RuntimeName),
            SpecialKind(ComponentMetadata.EventHandler.TagHelperKind),
            new(ComponentMetadata.EventHandler.EventArgsType, "Microsoft.AspNetCore.Components.Web.MouseEventArgs"),
            new(ComponentMetadata.Component.NameMatchKey, ComponentMetadata.Component.FullyQualifiedNameMatch),
            MakeTrue(TagHelperMetadata.Common.ClassifyAttributesOnly),
            TypeName("OnClickDirectiveAttribute"),
            TypeNamespace("Microsoft.AspNetCore.Components.Web"),
            TypeNameIdentifier("EventHandlers"));

        var htmlTagMutator = TagHelperDescriptorBuilder.Create("HtmlMutator", "TestAssembly");
        htmlTagMutator.TagMatchingRule(rule =>
        {
            rule.TagName = "title";
            rule.RequireAttributeDescriptor(attributeRule =>
            {
                attributeRule.Name = "mutator";
            });
        });
        htmlTagMutator.SetMetadata(TypeName("HtmlMutator"));
        htmlTagMutator.BindAttribute(attribute =>
        {
            attribute.Name = "Extra";
            attribute.SetMetadata(PropertyName("Extra"));
            attribute.TypeName = typeof(bool).FullName;
        });

        Default =
        [
            builder1.Build(),
            builder1WithRequiredParent.Build(),
            builder2.Build(),
            builder3.Build(),
            textComponent.Build(),
            directiveAttribute1.Build(),
            directiveAttribute2.Build(),
            directiveAttribute3.Build(),
            htmlTagMutator.Build(),
        ];
    }
}

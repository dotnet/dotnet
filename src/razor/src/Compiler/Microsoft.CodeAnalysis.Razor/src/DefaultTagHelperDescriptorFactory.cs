// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis;
using static Microsoft.AspNetCore.Razor.Language.CommonMetadata;

namespace Microsoft.CodeAnalysis.Razor;

internal class DefaultTagHelperDescriptorFactory
{
    private const string TagHelperNameEnding = "TagHelper";

    public DefaultTagHelperDescriptorFactory(Compilation compilation, bool includeDocumentation, bool excludeHidden)
    {
        IncludeDocumentation = includeDocumentation;
        ExcludeHidden = excludeHidden;
    }

    protected bool ExcludeHidden { get; }

    protected bool IncludeDocumentation { get; }

    /// <inheritdoc />
    public virtual TagHelperDescriptor CreateDescriptor(INamedTypeSymbol type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (ShouldSkipDescriptorCreation(type))
        {
            return null;
        }

        var typeName = GetFullName(type);
        var assemblyName = type.ContainingAssembly.Identity.Name;

        using var _ = TagHelperDescriptorBuilder.GetPooledInstance(
            typeName, assemblyName,
            out var descriptorBuilder);

        descriptorBuilder.SetMetadata(
            RuntimeName(TagHelperConventions.DefaultKind),
            TypeName(typeName),
            TypeNamespace(type.ContainingNamespace.ToDisplayString(SymbolExtensions.FullNameTypeDisplayFormat)),
            TypeNameIdentifier(type.Name));

        AddBoundAttributes(type, descriptorBuilder);
        AddTagMatchingRules(type, descriptorBuilder);
        AddAllowedChildren(type, descriptorBuilder);
        AddDocumentation(type, descriptorBuilder);
        AddTagOutputHint(type, descriptorBuilder);

        var descriptor = descriptorBuilder.Build();

        return descriptor;
    }

    private static void AddTagMatchingRules(INamedTypeSymbol type, TagHelperDescriptorBuilder descriptorBuilder)
    {
        using var builder = new PooledArrayBuilder<AttributeData>();

        foreach (var attribute in type.GetAttributes())
        {
            if (attribute.AttributeClass.HasFullName(TagHelperTypes.HtmlTargetElementAttribute))
            {
                builder.Add(attribute);
            }
        }

        var targetElementAttributes = builder.DrainToImmutable();

        // If there isn't an attribute specifying the tag name derive it from the name
        if (!targetElementAttributes.Any())
        {
            var name = type.Name;

            if (name.EndsWith(TagHelperNameEnding, StringComparison.OrdinalIgnoreCase))
            {
                name = name[..^TagHelperNameEnding.Length];
            }

            descriptorBuilder.TagMatchingRule(ruleBuilder =>
            {
                var htmlCasedName = HtmlConventions.ToHtmlCase(name);
                ruleBuilder.TagName = htmlCasedName;
            });

            return;
        }

        foreach (var targetElementAttribute in targetElementAttributes)
        {
            descriptorBuilder.TagMatchingRule(ruleBuilder =>
            {
                var tagName = HtmlTargetElementAttribute_Tag(targetElementAttribute);
                ruleBuilder.TagName = tagName;

                var parentTag = HtmlTargetElementAttribute_ParentTag(targetElementAttribute);
                ruleBuilder.ParentTag = parentTag;

                var tagStructure = HtmlTargetElementAttribute_TagStructure(targetElementAttribute);
                ruleBuilder.TagStructure = tagStructure;

                var requiredAttributeString = HtmlTargetElementAttribute_Attributes(targetElementAttribute);
                RequiredAttributeParser.AddRequiredAttributes(requiredAttributeString, ruleBuilder);
            });
        }
    }

    private void AddBoundAttributes(INamedTypeSymbol type, TagHelperDescriptorBuilder builder)
    {
        var accessibleProperties = GetAccessibleProperties(type);

        foreach (var property in accessibleProperties)
        {
            if (ShouldSkipDescriptorCreation(property))
            {
                continue;
            }

            builder.BindAttribute(attributeBuilder =>
            {
                ConfigureBoundAttribute(attributeBuilder, property, type);
            });
        }
    }

    private static void AddAllowedChildren(INamedTypeSymbol type, TagHelperDescriptorBuilder builder)
    {
        var restrictChildrenAttribute = type.GetAttributes().FirstOrDefault(static a => a.AttributeClass.HasFullName(TagHelperTypes.RestrictChildrenAttribute));
        if (restrictChildrenAttribute == null)
        {
            return;
        }

        builder.AllowChildTag(childTagBuilder => childTagBuilder.Name = (string)restrictChildrenAttribute.ConstructorArguments[0].Value);

        if (restrictChildrenAttribute.ConstructorArguments.Length == 2)
        {
            foreach (var value in restrictChildrenAttribute.ConstructorArguments[1].Values)
            {
                builder.AllowChildTag(childTagBuilder => childTagBuilder.Name = (string)value.Value);
            }
        }
    }

    private void AddDocumentation(INamedTypeSymbol type, TagHelperDescriptorBuilder builder)
    {
        if (!IncludeDocumentation)
        {
            return;
        }

        var xml = type.GetDocumentationCommentXml();

        if (!string.IsNullOrEmpty(xml))
        {
            builder.SetDocumentation(xml);
        }
    }

    private static void AddTagOutputHint(INamedTypeSymbol type, TagHelperDescriptorBuilder builder)
    {
        string outputElementHint = null;
        var outputElementHintAttribute = type.GetAttributes().FirstOrDefault(static a => a.AttributeClass.HasFullName(TagHelperTypes.OutputElementHintAttribute));
        if (outputElementHintAttribute != null)
        {
            outputElementHint = (string)outputElementHintAttribute.ConstructorArguments[0].Value;
            builder.TagOutputHint = outputElementHint;
        }
    }

    private void ConfigureBoundAttribute(
        BoundAttributeDescriptorBuilder builder,
        IPropertySymbol property,
        INamedTypeSymbol containingType)
    {
        var attributeNameAttribute = property
            .GetAttributes()
            .FirstOrDefault(static a => a.AttributeClass.HasFullName(TagHelperTypes.HtmlAttributeNameAttribute));

        bool hasExplicitName;
        string attributeName;
        if (attributeNameAttribute == null ||
            attributeNameAttribute.ConstructorArguments.Length == 0 ||
            string.IsNullOrEmpty((string)attributeNameAttribute.ConstructorArguments[0].Value))
        {
            hasExplicitName = false;
            attributeName = HtmlConventions.ToHtmlCase(property.Name);
        }
        else
        {
            hasExplicitName = true;
            attributeName = (string)attributeNameAttribute.ConstructorArguments[0].Value;
        }

        var hasPublicSetter = property.SetMethod != null && property.SetMethod.DeclaredAccessibility == Accessibility.Public;
        var typeName = GetFullName(property.Type);
        builder.TypeName = typeName;
        builder.SetMetadata(PropertyName(property.Name));

        if (hasPublicSetter)
        {
            builder.Name = attributeName;

            if (property.Type.TypeKind == TypeKind.Enum)
            {
                builder.IsEnum = true;
            }

            if (IncludeDocumentation)
            {
                var xml = property.GetDocumentationCommentXml();

                if (!string.IsNullOrEmpty(xml))
                {
                    builder.SetDocumentation(xml);
                }
            }
        }
        else if (hasExplicitName && !IsPotentialDictionaryProperty(property))
        {
            // Specified HtmlAttributeNameAttribute.Name though property has no public setter.
            var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidAttributeNameNullOrEmpty(GetFullName(containingType), property.Name);
            builder.Diagnostics.Add(diagnostic);
        }

        ConfigureDictionaryBoundAttribute(builder, property, containingType, attributeNameAttribute, attributeName, hasPublicSetter);
    }

    private static void ConfigureDictionaryBoundAttribute(
        BoundAttributeDescriptorBuilder builder,
        IPropertySymbol property,
        INamedTypeSymbol containingType,
        AttributeData attributeNameAttribute,
        string attributeName,
        bool hasPublicSetter)
    {
        string dictionaryAttributePrefix = null;
        var dictionaryAttributePrefixSet = false;

        if (attributeNameAttribute != null)
        {
            foreach (var argument in attributeNameAttribute.NamedArguments)
            {
                if (argument.Key == TagHelperTypes.HtmlAttributeName.DictionaryAttributePrefix)
                {
                    dictionaryAttributePrefix = (string)argument.Value.Value;
                    dictionaryAttributePrefixSet = true;
                    break;
                }
            }
        }

        var dictionaryArgumentTypes = GetDictionaryArgumentTypes(property);
        if (dictionaryArgumentTypes != null)
        {
            var prefix = dictionaryAttributePrefix;
            if (attributeNameAttribute == null || !dictionaryAttributePrefixSet)
            {
                prefix = attributeName + "-";
            }

            if (prefix != null)
            {
                var dictionaryValueType = dictionaryArgumentTypes[1];
                var dictionaryValueTypeName = GetFullName(dictionaryValueType);
                builder.AsDictionary(prefix, dictionaryValueTypeName);
            }
        }

        var dictionaryKeyType = dictionaryArgumentTypes?[0];

        if (dictionaryKeyType?.SpecialType != SpecialType.System_String)
        {
            if (dictionaryAttributePrefix != null)
            {
                // DictionaryAttributePrefix is not supported unless associated with an
                // IDictionary<string, TValue> property.
                var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNotNull(GetFullName(containingType), property.Name);
                builder.Diagnostics.Add(diagnostic);
            }

            return;
        }
        else if (!hasPublicSetter && attributeNameAttribute != null && !dictionaryAttributePrefixSet)
        {
            // Must set DictionaryAttributePrefix when using HtmlAttributeNameAttribute with a dictionary property
            // that lacks a public setter.
            var diagnostic = RazorDiagnosticFactory.CreateTagHelper_InvalidAttributePrefixNull(GetFullName(containingType), property.Name);
            builder.Diagnostics.Add(diagnostic);

            return;
        }
    }

    private static IReadOnlyList<ITypeSymbol> GetDictionaryArgumentTypes(IPropertySymbol property)
    {
        INamedTypeSymbol dictionaryType;
        if ((property.Type as INamedTypeSymbol)?.ConstructedFrom.HasFullName(TagHelperTypes.IDictionary) == true)
        {
            dictionaryType = (INamedTypeSymbol)property.Type;
        }
        else if (property.Type.AllInterfaces.Any(static s => s.ConstructedFrom.HasFullName(TagHelperTypes.IDictionary)))
        {
            dictionaryType = property.Type.AllInterfaces.First(static s => s.ConstructedFrom.HasFullName(TagHelperTypes.IDictionary));
        }
        else
        {
            dictionaryType = null;
        }

        return dictionaryType?.TypeArguments;
    }

    private static string HtmlTargetElementAttribute_Attributes(AttributeData attribute)
    {
        foreach (var kvp in attribute.NamedArguments)
        {
            if (kvp.Key == TagHelperTypes.HtmlTargetElement.Attributes)
            {
                return (string)kvp.Value.Value;
            }
        }

        return null;
    }

    private static string HtmlTargetElementAttribute_ParentTag(AttributeData attribute)
    {
        foreach (var kvp in attribute.NamedArguments)
        {
            if (kvp.Key == TagHelperTypes.HtmlTargetElement.ParentTag)
            {
                return (string)kvp.Value.Value;
            }
        }

        return null;
    }

    private static string HtmlTargetElementAttribute_Tag(AttributeData attribute)
    {
        if (attribute.ConstructorArguments.Length == 0)
        {
            return TagHelperMatchingConventions.ElementCatchAllName;
        }
        else
        {
            return (string)attribute.ConstructorArguments[0].Value;
        }
    }

    private static TagStructure HtmlTargetElementAttribute_TagStructure(AttributeData attribute)
    {
        foreach (var kvp in attribute.NamedArguments)
        {
            if (kvp.Key == TagHelperTypes.HtmlTargetElement.TagStructure)
            {
                return (TagStructure)kvp.Value.Value;
            }
        }

        return TagStructure.Unspecified;
    }

    private static bool IsPotentialDictionaryProperty(IPropertySymbol property)
    {
        return
            ((property.Type as INamedTypeSymbol)?.ConstructedFrom.HasFullName(TagHelperTypes.IDictionary) == true || property.Type.AllInterfaces.Any(static s => s.ConstructedFrom.HasFullName(TagHelperTypes.IDictionary))) &&
            GetDictionaryArgumentTypes(property)?[0].SpecialType == SpecialType.System_String;
    }

    private static IPropertySymbol[] GetAccessibleProperties(INamedTypeSymbol typeSymbol)
    {
        using var names = new PooledHashSet<string>(StringHashSetPool.Ordinal);
        using var resultList = new PooledList<IPropertySymbol>();

        do
        {
            foreach (var member in typeSymbol.GetMembers())
            {
                if (member is IPropertySymbol property &&
                    property.Parameters.Length == 0 &&
                    property.GetMethod != null &&
                    property.GetMethod.DeclaredAccessibility == Accessibility.Public &&
                    property.GetAttributes().FirstOrDefault(static a => a.AttributeClass.HasFullName(TagHelperTypes.HtmlAttributeNotBoundAttribute)) == null &&
                    (property.GetAttributes().Any(static a => a.AttributeClass.HasFullName(TagHelperTypes.HtmlAttributeNameAttribute)) ||
                    property.SetMethod != null && property.SetMethod.DeclaredAccessibility == Accessibility.Public ||
                    IsPotentialDictionaryProperty(property)) &&
                    !names.Contains(property.Name))
                {
                    names.Add(property.Name);
                    resultList.Add(property);
                }
            }

            typeSymbol = typeSymbol.BaseType;
        }
        while (typeSymbol != null);

        return resultList.ToArray();
    }

    private bool ShouldSkipDescriptorCreation(ISymbol symbol)
    {
        if (ExcludeHidden)
        {
            var editorBrowsableAttribute = symbol.GetAttributes().FirstOrDefault(static a => a.AttributeClass.HasFullName(typeof(EditorBrowsableAttribute).FullName));

            if (editorBrowsableAttribute == null)
            {
                return false;
            }

            var constructorArguments = editorBrowsableAttribute.ConstructorArguments;

            if (constructorArguments.Length > 0)
            {
                return (EditorBrowsableState)constructorArguments[0].Value == EditorBrowsableState.Never;
            }
        }

        return false;
    }

    protected static string GetFullName(ITypeSymbol type)
        => type.ToDisplayString(SymbolExtensions.FullNameTypeDisplayFormat);
}

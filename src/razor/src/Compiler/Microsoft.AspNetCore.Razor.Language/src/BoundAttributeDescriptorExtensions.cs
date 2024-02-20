// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;

namespace Microsoft.AspNetCore.Razor.Language;

public static class BoundAttributeDescriptorExtensions
{
    public static string GetPropertyName(this BoundAttributeDescriptor attribute)
    {
        if (attribute == null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        attribute.Metadata.TryGetValue(TagHelperMetadata.Common.PropertyName, out var propertyName);
        return propertyName;
    }

    public static string GetGloballyQualifiedTypeName(this BoundAttributeDescriptor attribute)
    {
        if (attribute == null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        attribute.Metadata.TryGetValue(TagHelperMetadata.Common.GloballyQualifiedTypeName, out var propertyName);
        return propertyName;
    }

    public static bool IsDefaultKind(this BoundAttributeDescriptor attribute)
    {
        if (attribute == null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        return attribute.Kind == TagHelperConventions.DefaultKind;
    }

    internal static bool ExpectsStringValue(this BoundAttributeDescriptor attribute, string name)
    {
        if (attribute.IsStringProperty)
        {
            return true;
        }

        var isIndexerNameMatch = TagHelperMatchingConventions.SatisfiesBoundAttributeIndexer(attribute, name.AsSpan());
        return isIndexerNameMatch && attribute.IsIndexerStringProperty;
    }

    internal static bool ExpectsBooleanValue(this BoundAttributeDescriptor attribute, string name)
    {
        if (attribute.IsBooleanProperty)
        {
            return true;
        }

        var isIndexerNameMatch = TagHelperMatchingConventions.SatisfiesBoundAttributeIndexer(attribute, name.AsSpan());
        return isIndexerNameMatch && attribute.IsIndexerBooleanProperty;
    }

    public static bool IsDefaultKind(this BoundAttributeParameterDescriptor parameter)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        return parameter.Kind == TagHelperConventions.DefaultKind;
    }

    public static string GetPropertyName(this BoundAttributeParameterDescriptor parameter)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        parameter.Metadata.TryGetValue(TagHelperMetadata.Common.PropertyName, out var propertyName);
        return propertyName;
    }
}

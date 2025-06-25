// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.AspNetCore.Razor.Language;

public static class BoundAttributeDescriptorExtensions
{
    public static string? GetPropertyName(this BoundAttributeDescriptor attribute)
    {
        ArgHelper.ThrowIfNull(attribute);

        attribute.Metadata.TryGetValue(TagHelperMetadata.Common.PropertyName, out var propertyName);
        return propertyName;
    }

    public static string? GetGloballyQualifiedTypeName(this BoundAttributeDescriptor attribute)
    {
        ArgHelper.ThrowIfNull(attribute);

        attribute.Metadata.TryGetValue(TagHelperMetadata.Common.GloballyQualifiedTypeName, out var propertyName);
        return propertyName;
    }

    public static bool IsDefaultKind(this BoundAttributeDescriptor attribute)
    {
        ArgHelper.ThrowIfNull(attribute);

        return attribute.Parent.Kind == TagHelperConventions.DefaultKind;
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
        ArgHelper.ThrowIfNull(parameter);

        return parameter.Parent.Parent.Kind == TagHelperConventions.DefaultKind;
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Razor.Language.Components;

namespace Microsoft.AspNetCore.Razor.Language;

public static class BoundAttributeDescriptorBuilderExtensions
{
    public static void SetMetadata(
        this BoundAttributeDescriptorBuilder builder,
        KeyValuePair<string, string> pair)
    {
        builder.SetMetadata(MetadataCollection.Create(pair));
    }

    internal static void SetMetadata(
        this BoundAttributeDescriptorBuilder builder,
        KeyValuePair<string, string> pair1,
        KeyValuePair<string, string> pair2)
    {
        builder.SetMetadata(MetadataCollection.Create(pair1, pair2));
    }

    internal static void SetMetadata(
        this BoundAttributeDescriptorBuilder builder,
        KeyValuePair<string, string> pair1,
        KeyValuePair<string, string> pair2,
        KeyValuePair<string, string> pair3)
    {
        builder.SetMetadata(MetadataCollection.Create(pair1, pair2, pair3));
    }

    [Obsolete($"Do not use this method. {nameof(BoundAttributeDescriptorBuilder.SetMetadata)} should be used instead.")]
    public static void SetPropertyName(this BoundAttributeDescriptorBuilder builder, string propertyName)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        Debug.Fail($"Do not use this method. {nameof(BoundAttributeDescriptorBuilder.SetMetadata)} should be used instead.");

        builder.Metadata[TagHelperMetadata.Common.PropertyName] = propertyName;
    }

    [Obsolete($"Do not use this method. {nameof(BoundAttributeDescriptorBuilder.TryGetMetadataValue)} should be used instead.")]
    public static string GetPropertyName(this BoundAttributeDescriptorBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        Debug.Fail($"Do not use this method. {nameof(BoundAttributeDescriptorBuilder.TryGetMetadataValue)} should be used instead.");

        if (builder.TryGetMetadataValue(TagHelperMetadata.Common.PropertyName, out var value))
        {
            return value;
        }

        return null;
    }

    public static void AsDictionary(
        this BoundAttributeDescriptorBuilder builder,
        string attributeNamePrefix,
        string valueTypeName)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.IsDictionary = true;
        builder.IndexerAttributeNamePrefix = attributeNamePrefix;
        builder.IndexerValueTypeName = valueTypeName;
    }

    public static bool IsDirectiveAttribute(this BoundAttributeDescriptorBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return builder.TryGetMetadataValue(ComponentMetadata.Common.DirectiveAttribute, out var value) &&
               value == bool.TrueString;
    }

    internal static void SetMetadata(this BoundAttributeParameterDescriptorBuilder builder, KeyValuePair<string, string> pair)
    {
        builder.SetMetadata(MetadataCollection.Create(pair));
    }

    [Obsolete($"Do not use this method. {nameof(BoundAttributeParameterDescriptorBuilder.SetMetadata)} should be used instead.")]
    public static void SetPropertyName(this BoundAttributeParameterDescriptorBuilder builder, string propertyName)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        Debug.Fail($"Do not use this method. {nameof(BoundAttributeParameterDescriptorBuilder.SetMetadata)} should be used instead.");

        builder.Metadata[TagHelperMetadata.Common.PropertyName] = propertyName;
    }

    [Obsolete($"Do not use this method. {nameof(BoundAttributeParameterDescriptorBuilder.SetMetadata)} should be used instead.")]
    public static void SetBindAttributeGetSet(this BoundAttributeParameterDescriptorBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        Debug.Fail($"Do not use this method. {nameof(BoundAttributeParameterDescriptorBuilder.SetMetadata)} should be used instead.");

        builder.Metadata[ComponentMetadata.Bind.BindAttributeGetSet] = bool.TrueString;
    }

    [Obsolete($"Do not use this method. {nameof(BoundAttributeParameterDescriptorBuilder.TryGetMetadataValue)} should be used instead.")]
    public static string GetPropertyName(this BoundAttributeParameterDescriptorBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        Debug.Fail($"Do not use this method. {nameof(BoundAttributeParameterDescriptorBuilder.TryGetMetadataValue)} should be used instead.");

        if (builder.TryGetMetadataValue(TagHelperMetadata.Common.PropertyName, out var value))
        {
            return value;
        }

        return null;
    }

    [Obsolete($"Do not use this method. {nameof(BoundAttributeDescriptorBuilder.SetMetadata)} should be used instead.")]
    public static void SetGloballyQualifiedTypeName(this BoundAttributeDescriptorBuilder builder, string globallyQualifiedTypeName)
    {
        Debug.Fail($"Do not use this method. {nameof(BoundAttributeDescriptorBuilder.SetMetadata)} should be used instead.");

        builder.Metadata[TagHelperMetadata.Common.GloballyQualifiedTypeName] = globallyQualifiedTypeName;
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Razor.Language;

public static class TagHelperDescriptorBuilderExtensions
{
    public static void SetMetadata(
        this TagHelperDescriptorBuilder builder,
        KeyValuePair<string, string> pair)
    {
        if (pair.Key == TagHelperMetadata.Runtime.Name)
        {
            builder.SetMetadata(MetadataCollection.Create(pair));
        }
        else
        {
            builder.SetMetadata(
                MetadataCollection.Create(
                    pair,
                    new(TagHelperMetadata.Runtime.Name, TagHelperConventions.DefaultKind)));
        }
    }

    public static void SetMetadata(
        this TagHelperDescriptorBuilder builder,
        KeyValuePair<string, string> pair1,
        KeyValuePair<string, string> pair2)
    {
        if (pair1.Key == TagHelperMetadata.Runtime.Name ||
            pair2.Key == TagHelperMetadata.Runtime.Name)
        {
            builder.SetMetadata(MetadataCollection.Create(pair1, pair2));
        }
        else
        {
            builder.SetMetadata(
                MetadataCollection.Create(
                    pair1,
                    pair2,
                    new(TagHelperMetadata.Runtime.Name, TagHelperConventions.DefaultKind)));
        }
    }

    internal static void SetMetadata(
        this TagHelperDescriptorBuilder builder,
        KeyValuePair<string, string> pair1,
        KeyValuePair<string, string> pair2,
        KeyValuePair<string, string> pair3)
    {
        if (pair1.Key == TagHelperMetadata.Runtime.Name ||
            pair2.Key == TagHelperMetadata.Runtime.Name ||
            pair3.Key == TagHelperMetadata.Runtime.Name)
        {
            builder.SetMetadata(MetadataCollection.Create(pair1, pair2, pair3));
        }
        else
        {
            builder.SetMetadata(
                MetadataCollection.Create(
                    pair1,
                    pair2,
                    pair3,
                    new(TagHelperMetadata.Runtime.Name, TagHelperConventions.DefaultKind)));
        }
    }

    internal static void SetMetadata(
        this TagHelperDescriptorBuilder builder,
        params KeyValuePair<string, string>[] pairs)
    {
        if (Array.Exists(pairs, static pair => pair.Key == TagHelperMetadata.Runtime.Name))
        {
            builder.SetMetadata(MetadataCollection.Create(pairs));
        }
        else
        {
            var newPairs = new KeyValuePair<string, string>[pairs.Length + 1];
            newPairs[0] = new(TagHelperMetadata.Runtime.Name, TagHelperConventions.DefaultKind);
            Array.Copy(pairs, 0, newPairs, 1, pairs.Length);

            builder.SetMetadata(MetadataCollection.Create(newPairs));
        }
    }

    [Obsolete($"Do not use this method. {nameof(TagHelperDescriptorBuilder.SetMetadata)} should be used instead.")]
    public static void SetTypeNamespace(this TagHelperDescriptorBuilder builder, string typeNamespace)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (typeNamespace == null)
        {
            throw new ArgumentNullException(nameof(typeNamespace));
        }

        Debug.Fail($"Do not use this method. {nameof(TagHelperDescriptorBuilder.SetMetadata)} should be used instead.");

        builder.Metadata[TagHelperMetadata.Common.TypeNamespace] = typeNamespace;
    }

    [Obsolete($"Do not use this method. {nameof(TagHelperDescriptorBuilder.SetMetadata)} should be used instead.")]
    public static void SetTypeNameIdentifier(this TagHelperDescriptorBuilder builder, string typeNameIdentifier)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (typeNameIdentifier == null)
        {
            throw new ArgumentNullException(nameof(typeNameIdentifier));
        }

        Debug.Fail($"Do not use this method. {nameof(TagHelperDescriptorBuilder.SetMetadata)} should be used instead.");

        builder.Metadata[TagHelperMetadata.Common.TypeNameIdentifier] = typeNameIdentifier;
    }

    [Obsolete($"Do not use this method. {nameof(TagHelperDescriptorBuilder.SetMetadata)} should be used instead.")]
    public static void SetTypeName(this TagHelperDescriptorBuilder builder, string typeName)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (typeName == null)
        {
            throw new ArgumentNullException(nameof(typeName));
        }

        Debug.Fail($"Do not use this method. {nameof(TagHelperDescriptorBuilder.SetMetadata)} should be used instead.");

        builder.Metadata[TagHelperMetadata.Common.TypeName] = typeName;
    }

    [Obsolete($"Do not use this method. {nameof(TagHelperDescriptorBuilder.TryGetMetadataValue)} should be used instead.")]
    public static string GetTypeName(this TagHelperDescriptorBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        Debug.Fail($"Do not use this method. {nameof(TagHelperDescriptorBuilder.TryGetMetadataValue)} should be used instead.");

        return builder.TryGetMetadataValue(TagHelperMetadata.Common.TypeName, out var value)
            ? value
            : null;
    }
}

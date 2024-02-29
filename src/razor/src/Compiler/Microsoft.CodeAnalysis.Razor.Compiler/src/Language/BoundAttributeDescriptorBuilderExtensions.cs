// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
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

    internal static void SetMetadata(this BoundAttributeParameterDescriptorBuilder builder, KeyValuePair<string, string> pair)
    {
        builder.SetMetadata(MetadataCollection.Create(pair));
    }
}

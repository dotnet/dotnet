// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Language;

public static class TestBoundAttributeDescriptorBuilderExtensions
{
    public static BoundAttributeDescriptorBuilder Name(this BoundAttributeDescriptorBuilder builder, string name)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Name = name;

        return builder;
    }

    public static BoundAttributeDescriptorBuilder TypeName(this BoundAttributeDescriptorBuilder builder, string typeName)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.TypeName = typeName;

        return builder;
    }

    public static BoundAttributeDescriptorBuilder Metadata(this BoundAttributeDescriptorBuilder builder, KeyValuePair<string, string> pair)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.SetMetadata(pair);

        return builder;
    }

    public static BoundAttributeDescriptorBuilder DisplayName(this BoundAttributeDescriptorBuilder builder, string displayName)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.DisplayName = displayName;

        return builder;
    }

    public static BoundAttributeDescriptorBuilder AsEnum(this BoundAttributeDescriptorBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.IsEnum = true;

        return builder;
    }

    public static BoundAttributeDescriptorBuilder AsDictionaryAttribute(
        this BoundAttributeDescriptorBuilder builder,
        string attributeNamePrefix,
        string valueTypeName)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.IsDictionary = true;
        builder.IndexerAttributeNamePrefix = attributeNamePrefix;
        builder.IndexerValueTypeName = valueTypeName;

        return builder;
    }

    public static BoundAttributeDescriptorBuilder Documentation(this BoundAttributeDescriptorBuilder builder, string documentation)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Documentation = documentation;

        return builder;
    }

    public static BoundAttributeDescriptorBuilder AddDiagnostic(this BoundAttributeDescriptorBuilder builder, RazorDiagnostic diagnostic)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Diagnostics.Add(diagnostic);

        return builder;
    }
}

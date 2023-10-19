// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;

namespace Microsoft.AspNetCore.Razor.Language;

public static class TagHelperDescriptorExtensions
{
    public static string GetTypeName(this TagHelperDescriptor tagHelper)
    {
        if (tagHelper == null)
        {
            throw new ArgumentNullException(nameof(tagHelper));
        }

        tagHelper.Metadata.TryGetValue(TagHelperMetadata.Common.TypeName, out var typeName);
        return typeName;
    }

    public static string GetTypeNamespace(this TagHelperDescriptor tagHelper)
    {
        if (tagHelper == null)
        {
            throw new ArgumentNullException(nameof(tagHelper));
        }

        tagHelper.Metadata.TryGetValue(TagHelperMetadata.Common.TypeNamespace, out var typeNamespace);
        return typeNamespace;
    }

    public static string GetTypeNameIdentifier(this TagHelperDescriptor tagHelper)
    {
        if (tagHelper == null)
        {
            throw new ArgumentNullException(nameof(tagHelper));
        }

        tagHelper.Metadata.TryGetValue(TagHelperMetadata.Common.TypeNameIdentifier, out var typeNameIdentifier);
        return typeNameIdentifier;
    }

    public static bool IsDefaultKind(this TagHelperDescriptor tagHelper)
    {
        if (tagHelper == null)
        {
            throw new ArgumentNullException(nameof(tagHelper));
        }

        return string.Equals(tagHelper.Kind, TagHelperConventions.DefaultKind, StringComparison.Ordinal);
    }

    public static bool KindUsesDefaultTagHelperRuntime(this TagHelperDescriptor tagHelper)
    {
        if (tagHelper == null)
        {
            throw new ArgumentNullException(nameof(tagHelper));
        }

        tagHelper.Metadata.TryGetValue(TagHelperMetadata.Runtime.Name, out var value);
        return string.Equals(TagHelperConventions.DefaultKind, value, StringComparison.Ordinal);
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal static class TagHelperDescriptorExtensions
{
    public static bool IsAttributeDescriptor(this TagHelperDescriptor descriptor)
    {
        return descriptor.Metadata.TryGetValue(TagHelperMetadata.Common.ClassifyAttributesOnly, out var value) ||
               string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase);
    }

    public static string? TryGetComponentTag(this TagHelperDescriptor descriptor)
    {
        var typeName = descriptor.GetTypeNameIdentifier();
        if (string.IsNullOrWhiteSpace(typeName))
        {
            return null;
        }

        // TODO: Add @using statements if required, or fully qualify (GetTypeName())

        using var _ = StringBuilderPool.GetPooledObject(out var builder);

        builder.Append('<');
        builder.Append(typeName);

        foreach (var requiredAttribute in descriptor.EditorRequiredAttributes)
        {
            builder.Append(' ');
            builder.Append(requiredAttribute.Name);
            builder.Append("=\"\"");
        }

        if (descriptor.AllowedChildTags.Length > 0)
        {
            builder.Append("></");
            builder.Append(typeName);
            builder.Append('>');
        }
        else
        {
            builder.Append(" />");
        }

        return builder.ToString();
    }
}

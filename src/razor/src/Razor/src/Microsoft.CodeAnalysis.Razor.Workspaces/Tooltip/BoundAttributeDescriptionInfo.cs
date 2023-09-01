// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Tooltip;

internal record BoundAttributeDescriptionInfo
{
    public string ReturnTypeName { get; }
    public string TypeName { get; }
    public string PropertyName { get; }
    public string Documentation { get; }

    public BoundAttributeDescriptionInfo(string returnTypeName, string typeName, string propertyName, string documentation)
    {
        ReturnTypeName = returnTypeName ?? throw new ArgumentNullException(nameof(returnTypeName));
        TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
        PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        Documentation = documentation ?? string.Empty;
    }

    public static BoundAttributeDescriptionInfo From(BoundAttributeParameterDescriptor parameterAttribute, string parentTagHelperTypeName)
    {
        if (parameterAttribute is null)
        {
            throw new ArgumentNullException(nameof(parameterAttribute));
        }

        if (parentTagHelperTypeName is null)
        {
            throw new ArgumentNullException(nameof(parentTagHelperTypeName));
        }

        var propertyName = parameterAttribute.GetPropertyName();

        return new BoundAttributeDescriptionInfo(
            parameterAttribute.TypeName,
            parentTagHelperTypeName,
            propertyName,
            parameterAttribute.Documentation);
    }

    public static BoundAttributeDescriptionInfo From(BoundAttributeDescriptor boundAttribute, bool indexer)
        => From(boundAttribute, indexer, parentTagHelperTypeName: null);

    public static BoundAttributeDescriptionInfo From(BoundAttributeDescriptor boundAttribute, bool indexer, string? parentTagHelperTypeName)
    {
        if (boundAttribute is null)
        {
            throw new ArgumentNullException(nameof(boundAttribute));
        }

        var returnTypeName = indexer ? boundAttribute.IndexerTypeName : boundAttribute.TypeName;
        var propertyName = boundAttribute.GetPropertyName();

        // The BoundAttributeDescriptor does not directly have the TagHelperTypeName information available.
        // Because of this we need to resolve it from other parts of it.
        parentTagHelperTypeName ??= ResolveTagHelperTypeName(propertyName, boundAttribute.DisplayName);

        return new BoundAttributeDescriptionInfo(
            returnTypeName,
            parentTagHelperTypeName,
            propertyName,
            boundAttribute.Documentation);
    }

    // Internal for testing
    internal static string ResolveTagHelperTypeName(string propertyName, string displayName)
    {
        // A BoundAttributeDescriptor does not have a direct reference to its parent TagHelper.
        // However, when it was constructed the parent TagHelper's type name was embedded into
        // its DisplayName. In VSCode we can't use the DisplayName verbatim for descriptions
        // because the DisplayName is typically too long to display properly. Therefore we need
        // to break it apart and then reconstruct it in a reduced format.
        // i.e. this is the format the display name comes in:
        // ReturnTypeName SomeTypeName.SomePropertyName
        //
        // See DefaultBoundAttributeDescriptorBuilder.GetDisplayName() for added detail.

        var displayNameSpan = displayName.AsSpanOrDefault();

        // Search for the first space, which should be immediately after the return type.
        var spaceIndex = displayNameSpan.IndexOf(' ');
        if (spaceIndex < 0)
        {
            return string.Empty;
        }

        // Increment by one to skip over the space.
        displayNameSpan = displayNameSpan[(spaceIndex + 1)..];

        var propertyNameSpan = propertyName.AsSpanOrDefault();

        // Strip off the trailing property name.
        if (displayNameSpan.EndsWith(propertyNameSpan, StringComparison.Ordinal))
        {
            displayNameSpan = displayNameSpan[..^propertyNameSpan.Length];
        }

        // Strip off the trailing '.'
        if (displayNameSpan is [ .. var start, '.'])
        {
            displayNameSpan = start;
        }

        return displayNameSpan.ToString();
    }
}

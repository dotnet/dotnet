// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Language;

public static class TestTagHelperDescriptorBuilderExtensions
{
    public static TagHelperDescriptorBuilder Metadata(this TagHelperDescriptorBuilder builder, string key, string value)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.SetMetadata(new KeyValuePair<string, string>(key, value));

        return builder;
    }

    public static TagHelperDescriptorBuilder Metadata(this TagHelperDescriptorBuilder builder, params KeyValuePair<string, string>[] pairs)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.SetMetadata(pairs);

        return builder;
    }

    public static TagHelperDescriptorBuilder DisplayName(this TagHelperDescriptorBuilder builder, string displayName)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.DisplayName = displayName;

        return builder;
    }

    public static TagHelperDescriptorBuilder AllowChildTag(this TagHelperDescriptorBuilder builder, string allowedChild)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.AllowChildTag(childTagBuilder => childTagBuilder.Name = allowedChild);

        return builder;
    }

    public static TagHelperDescriptorBuilder TagOutputHint(this TagHelperDescriptorBuilder builder, string hint)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.TagOutputHint = hint;

        return builder;
    }

    public static TagHelperDescriptorBuilder SetCaseSensitive(this TagHelperDescriptorBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.CaseSensitive = true;

        return builder;
    }

    public static TagHelperDescriptorBuilder Documentation(this TagHelperDescriptorBuilder builder, string documentation)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Documentation = documentation;

        return builder;
    }

    public static TagHelperDescriptorBuilder AddDiagnostic(this TagHelperDescriptorBuilder builder, RazorDiagnostic diagnostic)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Diagnostics.Add(diagnostic);

        return builder;
    }

    public static TagHelperDescriptorBuilder BoundAttributeDescriptor(
        this TagHelperDescriptorBuilder builder,
        Action<BoundAttributeDescriptorBuilder> configure)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.BindAttribute(configure);

        return builder;
    }

    public static TagHelperDescriptorBuilder TagMatchingRuleDescriptor(
        this TagHelperDescriptorBuilder builder,
        Action<TagMatchingRuleDescriptorBuilder> configure)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.TagMatchingRule(configure);

        return builder;
    }
}

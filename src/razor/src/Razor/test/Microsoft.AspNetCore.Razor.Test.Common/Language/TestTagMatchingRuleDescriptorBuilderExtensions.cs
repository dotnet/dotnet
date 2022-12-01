// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;

namespace Microsoft.AspNetCore.Razor.Language;

public static class TestTagMatchingRuleDescriptorBuilderExtensions
{
    public static TagMatchingRuleDescriptorBuilder RequireTagName(this TagMatchingRuleDescriptorBuilder builder, string tagName)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.TagName = tagName;

        return builder;
    }

    public static TagMatchingRuleDescriptorBuilder RequireParentTag(this TagMatchingRuleDescriptorBuilder builder, string parentTag)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.ParentTag = parentTag;

        return builder;
    }

    public static TagMatchingRuleDescriptorBuilder RequireTagStructure(this TagMatchingRuleDescriptorBuilder builder, TagStructure tagStructure)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.TagStructure = tagStructure;

        return builder;
    }

    public static TagMatchingRuleDescriptorBuilder AddDiagnostic(this TagMatchingRuleDescriptorBuilder builder, RazorDiagnostic diagnostic)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Diagnostics.Add(diagnostic);

        return builder;
    }

    public static TagMatchingRuleDescriptorBuilder RequireAttributeDescriptor(
        this TagMatchingRuleDescriptorBuilder builder,
        Action<RequiredAttributeDescriptorBuilder> configure)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Attribute(configure);

        return builder;
    }
}

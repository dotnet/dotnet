// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class TagMatchingRuleDescriptorComparer : IEqualityComparer<TagMatchingRuleDescriptor?>
{
    /// <summary>
    /// A default instance of the <see cref="TagMatchingRuleDescriptorComparer"/>.
    /// </summary>
    public static readonly TagMatchingRuleDescriptorComparer Default = new TagMatchingRuleDescriptorComparer();

    private TagMatchingRuleDescriptorComparer()
    {
    }

    public bool Equals(TagMatchingRuleDescriptor? ruleX, TagMatchingRuleDescriptor? ruleY)
    {
        if (object.ReferenceEquals(ruleX, ruleY))
        {
            return true;
        }

        if (ruleX is null)
        {
            return ruleY is null;
        }
        else if (ruleY is null)
        {
            return false;
        }

        return
            string.Equals(ruleX.TagName, ruleY.TagName, StringComparison.Ordinal) &&
            string.Equals(ruleX.ParentTag, ruleY.ParentTag, StringComparison.Ordinal) &&
            ruleX.CaseSensitive == ruleY.CaseSensitive &&
            ruleX.TagStructure == ruleY.TagStructure &&
            ComparerUtilities.Equals(ruleX.Attributes, ruleY.Attributes, RequiredAttributeDescriptorComparer.Default);
    }

    public int GetHashCode(TagMatchingRuleDescriptor? rule)
    {
        if (rule == null)
        {
            return 0;
        }

        var hash = HashCodeCombiner.Start();
        hash.Add(rule.TagName, StringComparer.Ordinal);

        return hash.CombinedHash;
    }
}

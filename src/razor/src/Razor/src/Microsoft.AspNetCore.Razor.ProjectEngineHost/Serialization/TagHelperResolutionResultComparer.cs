// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal sealed class TagHelperResolutionResultComparer : IEqualityComparer<TagHelperResolutionResult>
{
    internal static readonly TagHelperResolutionResultComparer Default = new();

    public bool Equals(TagHelperResolutionResult? x, TagHelperResolutionResult? y)
    {
        if (x is null)
        {
            return y is null;
        }
        else if (y is null)
        {
            return false;
        }

        return x.Descriptors.AssumeNotNull().SequenceEqual(y.Descriptors.AssumeNotNull(), TagHelperDescriptorComparer.Default) &&
               x.Diagnostics.AssumeNotNull().SequenceEqual(y.Diagnostics.AssumeNotNull());
    }

    public int GetHashCode(TagHelperResolutionResult obj)
    {
        var hash = new HashCodeCombiner();

        if (obj.Descriptors is not null)
        {
            foreach (var descriptor in obj.Descriptors)
            {
                hash.Add(descriptor);
            }
        }

        if (obj.Diagnostics is not null)
        {
            for (var i = 0; i < obj.Diagnostics.Count; i++)
            {
                hash.Add(obj.Diagnostics[i]);
            }
        }

        return hash.CombinedHash;
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Threading;

namespace Microsoft.AspNetCore.Razor.Language;

public class TestTagHelperFeature : RazorEngineFeatureBase, ITagHelperFeature
{
    public TestTagHelperFeature()
    {
        TagHelpers = [];
    }

    public TestTagHelperFeature(IEnumerable<TagHelperDescriptor> tagHelpers)
    {
        TagHelpers = [.. tagHelpers];
    }

    public List<TagHelperDescriptor> TagHelpers { get; }

    public IReadOnlyList<TagHelperDescriptor> GetDescriptors(CancellationToken cancellationToken = default)
        => [.. TagHelpers];
}

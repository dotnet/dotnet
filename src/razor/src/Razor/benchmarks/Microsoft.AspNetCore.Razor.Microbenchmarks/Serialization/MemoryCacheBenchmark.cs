// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class MemoryCacheBenchmark
{
    private IReadOnlyList<int>? _tagHelperHashes;
    private IReadOnlyList<TagHelperDescriptor>? _tagHelpers;
    private MemoryCache<int, TagHelperDescriptor>? _cache;

    private IReadOnlyList<int> TagHelperHashes => _tagHelperHashes.AssumeNotNull();
    private IReadOnlyList<TagHelperDescriptor> TagHelpers => _tagHelpers.AssumeNotNull();
    private MemoryCache<int, TagHelperDescriptor> Cache => _cache.AssumeNotNull();

    [GlobalSetup]
    public void Setup()
    {
        _tagHelpers = CommonResources.LegacyTagHelpers;
        _tagHelperHashes = TagHelpers.Select(th => th.GetHashCode()).ToArray();

        // Set cache size to 400 so anything more then that will force compacts
        _cache = new MemoryCache<int, TagHelperDescriptor>(400);
    }

    [Benchmark(Description = "MemoryCache Set performance with limited size")]
    public void Set_Performance()
    {
        for (var i = 0; i < TagHelpers.Count; i++)
        {
            Cache.Set(TagHelperHashes[i], TagHelpers[i]);
        }
    }
}

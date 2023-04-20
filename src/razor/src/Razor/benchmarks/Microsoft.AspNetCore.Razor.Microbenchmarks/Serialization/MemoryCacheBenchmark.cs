// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Newtonsoft.Json;

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
        var tagHelperBuffer = Resources.GetResourceBytes("taghelpers.json");

        // Deserialize from json file.
        var serializer = new JsonSerializer();
        serializer.Converters.Add(TagHelperDescriptorJsonConverter.Instance);
        using var stream = new MemoryStream(tagHelperBuffer);
        using var reader = new JsonTextReader(new StreamReader(stream));

        _tagHelpers = serializer.Deserialize<IReadOnlyList<TagHelperDescriptor>>(reader).AssumeNotNull();
        _tagHelperHashes = TagHelpers.Select(th => th.GetHashCode()).ToList();

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

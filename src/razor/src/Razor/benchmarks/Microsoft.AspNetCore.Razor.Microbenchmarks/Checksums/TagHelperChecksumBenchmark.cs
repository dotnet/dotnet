// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;
using System.Collections.Generic;
using Checksum = Microsoft.AspNetCore.Razor.Utilities.Checksum;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Checksums;

public class TagHelperChecksumBenchmarks
{
    private Checksum[]? _checksums;

    [ParamsAllValues]
    public ResourceSet ResourceSet { get; set; }

    private IReadOnlyList<TagHelperDescriptor> TagHelpers
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikTagHelpers,
            _ => CommonResources.LegacyTagHelpers
        };

    [IterationSetup]
    public void Setup()
    {
        _checksums = new Checksum[TagHelpers.Count];
    }

    [Benchmark(Description = "Create Checksums")]
    public void CreateChecksums()
    {
        for (var i = 0; i < TagHelpers.Count; i++)
        {
            _checksums![i] = TagHelpers[i].CreateChecksum();
        }
    }
}

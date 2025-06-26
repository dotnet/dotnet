// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;

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
            _checksums![i] = TagHelpers[i].ComputeChecksum();
        }
    }
}

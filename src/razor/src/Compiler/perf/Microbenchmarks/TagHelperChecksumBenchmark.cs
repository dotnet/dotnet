// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Compiler;

public class TagHelperChecksumBenchmarks
{
    private const int OperationsPerInvoke = 1000;

    private Checksum[]? _checksums;
    private ImmutableArray<TagHelperDescriptor> _tagHelpers;

    [ParamsAllValues]
    public TagHelperSet TagHelperSet { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        // Cache the TagHelpers for this ResourceSet to avoid repeated property access
        _tagHelpers = TagHelperSet switch
        {
            TagHelperSet.BlazorServerApp => Resources.Tooling.BlazorServerApp,
            TagHelperSet.TelerikMvc => Resources.Tooling.TelerikMvc,
            TagHelperSet.Legacy => Resources.Tooling.Legacy,
            _ => Assumed.Unreachable<ImmutableArray<TagHelperDescriptor>>()
        };

        // Warm up to ensure consistent measurements
        _checksums = new Checksum[_tagHelpers.Length];

        for (var i = 0; i < _tagHelpers.Length; i++)
        {
            _checksums[i] = _tagHelpers[i].ComputeChecksum();
        }
    }

    [IterationSetup]
    public void Setup()
    {
        _checksums = new Checksum[_tagHelpers.Length];
    }

    [Benchmark(Description = "Create Checksums", OperationsPerInvoke = OperationsPerInvoke)]
    public void CreateChecksums()
    {
        for (var operation = 0; operation < OperationsPerInvoke; operation++)
        {
            for (var i = 0; i < _tagHelpers.Length; i++)
            {
                _checksums![i] = _tagHelpers[i].ComputeChecksum();
            }
        }
    }
}

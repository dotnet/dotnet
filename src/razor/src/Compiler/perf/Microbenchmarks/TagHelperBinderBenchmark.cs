// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Compiler;

public class TagHelperBinderBenchmark
{
    // We create a number of binders to get a measurable time.
    private const int Count = 2500;

    private readonly TagHelperBinder[] _binders = new TagHelperBinder[Count];
    private ImmutableArray<TagHelperDescriptor> _tagHelpers;

    [ParamsAllValues]
    public TagHelperSet TagHelpers { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        _tagHelpers = TagHelpers switch
        {
            TagHelperSet.BlazorServerApp => Resources.Tooling.BlazorServerApp,
            TagHelperSet.TelerikMvc => Resources.Tooling.TelerikMvc,
            TagHelperSet.Legacy => Resources.Tooling.Legacy,
            _ => Assumed.Unreachable<ImmutableArray<TagHelperDescriptor>>()
        };
    }

    [IterationCleanup]
    public void IterationCleanUp()
    {
        Array.Clear(_binders);
    }

    [Benchmark(Description = "Construct TagHelperBinders")]
    public void ConstructTagHelperBinders()
    {
        for (var i = 0; i < Count; i++)
        {
            _binders[i] = new TagHelperBinder(tagNamePrefix: null, [.. _tagHelpers]);
        }
    }

    [Benchmark(Description = "Construct TagHelperBinders (with prefix)")]
    public void ConstructTagHelperBinderWithPrefix()
    {
        for (var i = 0; i < Count; i++)
        {
            _binders[i] = new TagHelperBinder(tagNamePrefix: "abc", [.. _tagHelpers]);
        }
    }
}

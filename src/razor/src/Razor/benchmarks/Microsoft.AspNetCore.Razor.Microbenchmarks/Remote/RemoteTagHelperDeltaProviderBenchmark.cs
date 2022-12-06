// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Remote.Razor;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public class RemoteTagHelperDeltaProviderBenchmark : TagHelperBenchmarkBase
{
    public RemoteTagHelperDeltaProviderBenchmark()
    {
        DefaultTagHelperSet = DefaultTagHelpers.ToHashSet();

        Added50PercentMoreDefaultTagHelpers = DefaultTagHelperSet
            .Take(DefaultTagHelperSet.Count / 2)
            .Select(th => new RenamedTagHelperDescriptor(th.Name + "Added", th))
            .Concat(DefaultTagHelperSet)
            .ToHashSet();

        RemovedHalfOfDefaultTagHelpers = DefaultTagHelperSet
            .Take(DefaultTagHelpers.Count / 2)
            .ToHashSet();

        var tagHelpersToMutate = DefaultTagHelperSet
            .Take(2)
            .Select(th => new RenamedTagHelperDescriptor(th.Name + "Mutated", th));
        MutatedTwoDefaultTagHelpers = DefaultTagHelperSet
            .Skip(2)
            .Concat(tagHelpersToMutate)
            .ToHashSet();
    }

    private IReadOnlyCollection<TagHelperDescriptor> DefaultTagHelperSet { get; }

    private IReadOnlyCollection<TagHelperDescriptor> Added50PercentMoreDefaultTagHelpers { get; }

    private IReadOnlyCollection<TagHelperDescriptor> RemovedHalfOfDefaultTagHelpers { get; }

    private IReadOnlyCollection<TagHelperDescriptor> MutatedTwoDefaultTagHelpers { get; }

    private string ProjectFilePath { get; } = "C:/path/to/project.csproj";

    private RemoteTagHelperDeltaProvider Provider { get; set; }

    private int LastResultId { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        Provider = new RemoteTagHelperDeltaProvider();
        var delta = Provider.GetTagHelpersDelta(ProjectFilePath, lastResultId: -1, DefaultTagHelperSet);
        LastResultId = delta.ResultId;
    }

    [Benchmark(Description = "Calculate Delta - New project")]
    public void TagHelper_GetTagHelpersDelta_NewProject()
    {
        _ = Provider.GetTagHelpersDelta("C:/path/to/newproject.csproj", lastResultId: -1, DefaultTagHelperSet);
    }

    [Benchmark(Description = "Calculate Delta - Remove project")]
    public void TagHelper_GetTagHelpersDelta_RemoveProject()
    {
        _ = Provider.GetTagHelpersDelta(ProjectFilePath, LastResultId, Array.Empty<TagHelperDescriptor>());
    }

    [Benchmark(Description = "Calculate Delta - Add lots of TagHelpers")]
    public void TagHelper_GetTagHelpersDelta_AddLots()
    {
        _ = Provider.GetTagHelpersDelta(ProjectFilePath, LastResultId, Added50PercentMoreDefaultTagHelpers);
    }

    [Benchmark(Description = "Calculate Delta - Remove lots of TagHelpers")]
    public void TagHelper_GetTagHelpersDelta_RemoveLots()
    {
        _ = Provider.GetTagHelpersDelta(ProjectFilePath, LastResultId, RemovedHalfOfDefaultTagHelpers);
    }

    [Benchmark(Description = "Calculate Delta - Mutate two TagHelpers")]
    public void TagHelper_GetTagHelpersDelta_Mutate2()
    {
        _ = Provider.GetTagHelpersDelta(ProjectFilePath, LastResultId, MutatedTwoDefaultTagHelpers);
    }

    [Benchmark(Description = "Calculate Delta - No change")]
    public void TagHelper_GetTagHelpersDelta_NoChange()
    {
        _ = Provider.GetTagHelpersDelta(ProjectFilePath, LastResultId, DefaultTagHelperSet);
    }

    internal class RenamedTagHelperDescriptor : DefaultTagHelperDescriptor
    {
        public RenamedTagHelperDescriptor(string newName, TagHelperDescriptor origin)
            : base(origin.Kind,
                 newName,
                 origin.AssemblyName,
                 origin.DisplayName,
                 origin.Documentation,
                 origin.TagOutputHint,
                 origin.CaseSensitive,
                 origin.TagMatchingRules.ToArray(),
                 origin.BoundAttributes.ToArray(),
                 origin.AllowedChildTags.ToArray(),
                 origin.Metadata.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                 origin.Diagnostics.ToArray())
        {
        }
    }
}

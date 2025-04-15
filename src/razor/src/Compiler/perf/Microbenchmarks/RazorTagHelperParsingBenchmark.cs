// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using static Microsoft.AspNetCore.Razor.Language.DefaultRazorTagHelperContextDiscoveryPhase;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public class RazorTagHelperParsingBenchmark
{
    public RazorTagHelperParsingBenchmark()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null && !File.Exists(Path.Combine(current.FullName, "taghelpers.json")))
        {
            current = current.Parent;
        }

        var root = current.AssumeNotNull();

        var tagHelpers = ReadTagHelpers(Path.Combine(root.FullName, "taghelpers.json"));
        var tagHelperFeature = new StaticTagHelperFeature(tagHelpers);

        var blazorServerTagHelpersFilePath = Path.Combine(root.FullName, "BlazorServerTagHelpers.razor");

        var fileSystem = RazorProjectFileSystem.Create(root.FullName);
        ProjectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, fileSystem,
            b =>
            {
                RazorExtensions.Register(b);
                b.Features.Add(tagHelperFeature);
            });
        BlazorServerTagHelpersDemoFile = fileSystem.GetItem(Path.Combine(blazorServerTagHelpersFilePath), RazorFileKind.Component);

        ComponentDirectiveVisitor = new ComponentDirectiveVisitor();
        ComponentDirectiveVisitor.Initialize(blazorServerTagHelpersFilePath, tagHelpers, currentNamespace: null);
        var codeDocument = ProjectEngine.ProcessDesignTime(BlazorServerTagHelpersDemoFile);
        SyntaxTree = codeDocument.GetSyntaxTree();
    }

    private RazorProjectEngine ProjectEngine { get; }
    private RazorProjectItem BlazorServerTagHelpersDemoFile { get; }
    private ComponentDirectiveVisitor ComponentDirectiveVisitor { get; }
    private RazorSyntaxTree SyntaxTree { get; }

    [Benchmark(Description = "TagHelper Design Time Processing")]
    public void TagHelper_ProcessDesignTime()
    {
        _ = ProjectEngine.ProcessDesignTime(BlazorServerTagHelpersDemoFile);
    }

    [Benchmark(Description = "Component Directive Parsing")]
    public void TagHelper_ComponentDirectiveVisitor()
    {
        ComponentDirectiveVisitor.Visit(SyntaxTree);
    }

    private static ImmutableArray<TagHelperDescriptor> ReadTagHelpers(string filePath)
    {
        using var reader = new StreamReader(filePath);
        return JsonDataConvert.DeserializeTagHelperArray(reader);
    }

    private sealed class StaticTagHelperFeature : RazorEngineFeatureBase, ITagHelperFeature
    {
        public StaticTagHelperFeature(IReadOnlyList<TagHelperDescriptor> descriptors) => Descriptors = descriptors;

        public IReadOnlyList<TagHelperDescriptor> Descriptors { get; }

        public IReadOnlyList<TagHelperDescriptor> GetDescriptors() => Descriptors;
    }
}

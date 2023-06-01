// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Serialization.Converters;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class TagHelperResolutionResultSerializationBenchmark
{
    [ParamsAllValues]
    public ResourceSet ResourceSet { get; set; }

    private ImmutableArray<TagHelperDescriptor> TagHelpers
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikTagHelpers,
            _ => CommonResources.LegacyTagHelpers
        };

    private JsonSerializer? _serializer;
    private TagHelperResolutionResult? _tagHelperResolutionResult;

    private JsonSerializer Serializer => _serializer.AssumeNotNull();
    private TagHelperResolutionResult TagHelperResolutionResult => _tagHelperResolutionResult.AssumeNotNull();

    [GlobalSetup]
    public void GlobalSetup()
    {
        _serializer = new JsonSerializer();
        _serializer.Converters.Add(TagHelperResolutionResultJsonConverter.Instance);
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _tagHelperResolutionResult = new TagHelperResolutionResult(TagHelpers);
    }

    [Benchmark(Description = "RoundTrip TagHelperDescriptorResult")]
    public void RoundTrip()
    {
        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
        {
            Serializer.Serialize(writer, TagHelperResolutionResult);
        }

        stream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(reader);

        var result = Serializer.Deserialize<TagHelperResolutionResult>(jsonReader);

        if (result is null ||
            result.Descriptors.Length != TagHelpers.Length)
        {
            throw new InvalidDataException();
        }
    }
}

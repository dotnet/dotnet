// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class TagHelperResolutionResultSerializationBenchmark
{
    private JsonSerializer? _serializer;
    private TagHelperResolutionResult? _tagHelperResolutionResult;

    private JsonSerializer Serializer => _serializer.AssumeNotNull();
    private TagHelperResolutionResult TagHelperResolutionResult => _tagHelperResolutionResult.AssumeNotNull();

    [GlobalSetup]
    public void Setup()
    {
        var tagHelperBuffer = Resources.GetResourceBytes("taghelpers.json");

        // Deserialize from json file.
        _serializer = new JsonSerializer();
        _serializer.Converters.Add(TagHelperDescriptorJsonConverter.Instance);
        _serializer.Converters.Add(TagHelperResolutionResultJsonConverter.Instance);

        using var stream = new MemoryStream(tagHelperBuffer);
        using var reader = new JsonTextReader(new StreamReader(stream));

        var tagHelpers = Serializer.Deserialize<IReadOnlyList<TagHelperDescriptor>>(reader);
        _tagHelperResolutionResult = new TagHelperResolutionResult(tagHelpers, Array.Empty<RazorDiagnostic>());
    }

    [Benchmark(Description = "Razor TagHelperResolutionResult Roundtrip JsonConverter Serialization")]
    public void TagHelper_JsonConvert_Serialization_RoundTrip()
    {
        MemoryStream originalStream;
        using (originalStream = new MemoryStream())
        using (var writer = new StreamWriter(originalStream, Encoding.UTF8, bufferSize: 4096))
        {
            Serializer.Serialize(writer, TagHelperResolutionResult);
        }

        TagHelperResolutionResult deserializedResult;
        var stream = new MemoryStream(originalStream.GetBuffer());
        using (stream)
        using (var reader = new JsonTextReader(new StreamReader(stream)))
        {
            deserializedResult = Serializer.Deserialize<TagHelperResolutionResult>(reader).AssumeNotNull();
        }
    }
}

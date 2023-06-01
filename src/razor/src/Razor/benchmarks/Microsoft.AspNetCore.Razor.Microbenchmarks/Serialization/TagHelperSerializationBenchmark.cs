// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class TagHelperSerializationBenchmark
{
    [ParamsAllValues]
    public ResourceSet ResourceSet { get; set; }

    private IReadOnlyList<TagHelperDescriptor> TagHelpers
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikTagHelpers,
            _ => CommonResources.LegacyTagHelpers
        };

    private byte[] TagHelperBytes
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikTagHelperBytes,
            _ => CommonResources.LegacyTagHelperBytes
        };

    private static IReadOnlyList<TagHelperDescriptor> DeserializeTagHelpers(TextReader reader)
    {
        return JsonDataConvert.DeserializeData(reader,
            static r => r.ReadArray(
                static r => ObjectReaders.ReadTagHelper(r, useCache: false)))
            ?? Array.Empty<TagHelperDescriptor>();
    }

    private static void SerializeTagHelpers(TextWriter writer, IReadOnlyList<TagHelperDescriptor> tagHelpers)
    {
        JsonDataConvert.SerializeData(writer,
            w => w.WriteArray(tagHelpers, ObjectWriters.Write));
    }

    [Benchmark(Description = "Serialize Tag Helpers")]
    public void Serialize()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096);

        SerializeTagHelpers(writer, TagHelpers);
    }

    [Benchmark(Description = "Deserialize Tag Helpers")]
    public void Deserialize()
    {
        using var stream = new MemoryStream(TagHelperBytes);
        using var reader = new StreamReader(stream);

        var tagHelpers = DeserializeTagHelpers(reader);

        if (tagHelpers.Count != TagHelpers.Count)
        {
            throw new InvalidDataException();
        }
    }

    [Benchmark(Description = "RoundTrip Tag Helpers")]
    public void RoundTrip()
    {
        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
        {
            SerializeTagHelpers(writer, TagHelpers);
        }

        stream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(stream);

        var tagHelpers = DeserializeTagHelpers(reader);

        if (tagHelpers.Count != TagHelpers.Count)
        {
            throw new InvalidDataException();
        }
    }

    [Benchmark(Description = "TagHelperDescriptor.GetHashCode()")]
    public void TagHelperDescriptor_GetHashCode()
    {
        foreach (var tagHelper in TagHelpers)
        {
            _ = tagHelper.GetHashCode();
        }
    }
}

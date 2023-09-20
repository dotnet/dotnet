// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using MessagePack;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class TagHelperSerializationBenchmark
{
    [AllowNull]
    private ArrayBufferWriter<byte> _buffer;
    private ReadOnlyMemory<byte> _tagHelperMessagePackBytes;

    [ParamsAllValues]
    public ResourceSet ResourceSet { get; set; }

    private ImmutableArray<TagHelperDescriptor> TagHelpers
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikTagHelpers,
            _ => CommonResources.LegacyTagHelpers
        };

    private byte[] TagHelperJsonBytes
        => ResourceSet switch
        {
            ResourceSet.Telerik => CommonResources.TelerikTagHelperJsonBytes,
            _ => CommonResources.LegacyTagHelperJsonBytes
        };

    private static ImmutableArray<TagHelperDescriptor> DeserializeTagHelpers_Json(TextReader reader)
    {
        return JsonDataConvert.DeserializeData(reader,
            static r => r.ReadImmutableArray(
                static r => ObjectReaders.ReadTagHelper(r, useCache: false)));
    }

    private static void SerializeTagHelpers(TextWriter writer, ImmutableArray<TagHelperDescriptor> tagHelpers)
    {
        JsonDataConvert.SerializeData(writer,
            w => w.WriteArray(tagHelpers, ObjectWriters.Write));
    }

    [Benchmark(Description = "Serialize Tag Helpers (JSON)")]
    public void Serialize_Json()
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096);

        SerializeTagHelpers(writer, TagHelpers);
    }

    [Benchmark(Description = "Deserialize Tag Helpers (JSON)")]
    public void Deserialize_Json()
    {
        using var stream = new MemoryStream(TagHelperJsonBytes);
        using var reader = new StreamReader(stream);

        var tagHelpers = DeserializeTagHelpers_Json(reader);

        if (tagHelpers.Length != TagHelpers.Length)
        {
            throw new InvalidDataException();
        }
    }

    [Benchmark(Description = "RoundTrip Tag Helpers (JSON)")]
    public void RoundTrip_Json()
    {
        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
        {
            SerializeTagHelpers(writer, TagHelpers);
        }

        stream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(stream);

        var tagHelpers = DeserializeTagHelpers_Json(reader);

        if (tagHelpers.Length != TagHelpers.Length)
        {
            throw new InvalidDataException();
        }
    }

    [GlobalSetup(Targets = new[] { nameof(Serialize_MessagePack), nameof(Deserialize_MessagePack), nameof(RoundTrip_MessagePack) })]
    public void GlobalSetup_MessagePack()
    {
        _buffer = new ArrayBufferWriter<byte>(initialCapacity: 1024 * 1024);
        _tagHelperMessagePackBytes = SerializeTagHelpers_MessagePack(TagHelpers);
    }

    private static ImmutableArray<TagHelperDescriptor> DeserializeTagHelpers_MessagePack(ReadOnlyMemory<byte> bytes)
    {
        var reader = new MessagePackReader(bytes);

        using var _ = TagHelperSerializationCache.Pool.GetPooledObject(out var cache);
        return TagHelperFormatter.Instance.DeserializeImmutableArray(ref reader, MessagePackSerializerOptions.Standard, cache);
    }

    private ReadOnlyMemory<byte> SerializeTagHelpers_MessagePack(ImmutableArray<TagHelperDescriptor> tagHelpers)
    {
        var writer = new MessagePackWriter(_buffer);

        using var _ = TagHelperSerializationCache.Pool.GetPooledObject(out var cache);

        TagHelperFormatter.Instance.SerializeArray(ref writer, tagHelpers, MessagePackSerializerOptions.Standard, cache);
        writer.Flush();

        return _buffer.WrittenMemory;
    }

    [Benchmark(Description = "Serialize Tag Helpers (MessagePack)")]
    public void Serialize_MessagePack()
    {
        SerializeTagHelpers_MessagePack(TagHelpers);
        _buffer.Clear();
    }

    [Benchmark(Description = "Deserialize Tag Helpers (MessagePack)")]
    public void Deserialize_MessagePack()
    {
        var tagHelpers = DeserializeTagHelpers_MessagePack(_tagHelperMessagePackBytes);

        if (tagHelpers.Length != TagHelpers.Length)
        {
            throw new InvalidDataException();
        }
    }

    [Benchmark(Description = "RoundTrip Tag Helpers (MessagePack)")]
    public void RoundTrip_MessagePack()
    {
        var bytes = SerializeTagHelpers_MessagePack(TagHelpers);
        var tagHelpers = DeserializeTagHelpers_MessagePack(bytes);

        if (tagHelpers.Length != TagHelpers.Length)
        {
            throw new InvalidDataException();
        }

        _buffer.Clear();
    }
}

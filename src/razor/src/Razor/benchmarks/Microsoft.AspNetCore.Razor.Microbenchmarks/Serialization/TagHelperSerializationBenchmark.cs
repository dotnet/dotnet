// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;
using Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Resolvers;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

public class TagHelperSerializationBenchmark
{
    [AllowNull]
    private ArrayBufferWriter<byte> _buffer;
    private ReadOnlyMemory<byte> _tagHelperMessagePackBytes;

    private static readonly MessagePackSerializerOptions s_options = MessagePackSerializerOptions.Standard
        .WithResolver(CompositeResolver.Create(
            TagHelperDeltaResultResolver.Instance,
            StandardResolver.Instance));

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
        => JsonDataConvert.DeserializeTagHelperArray(reader);

    private static void SerializeTagHelpers(TextWriter writer, ImmutableArray<TagHelperDescriptor> tagHelpers)
        => JsonDataConvert.Serialize(tagHelpers, writer);

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
        using var cachingOptions = new SerializerCachingOptions(s_options);

        return MessagePackSerializer.Deserialize<ImmutableArray<TagHelperDescriptor>>(bytes, cachingOptions);
    }

    private ReadOnlyMemory<byte> SerializeTagHelpers_MessagePack(ImmutableArray<TagHelperDescriptor> tagHelpers)
    {
        using var cachingOptions = new SerializerCachingOptions(s_options);

        MessagePackSerializer.Serialize(_buffer, tagHelpers, cachingOptions);

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

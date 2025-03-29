// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

#if !NET
using System.Collections.Generic;
#endif

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class MetadataCollectionFormatter : ValueFormatter<MetadataCollection>
{
    public static readonly ValueFormatter<MetadataCollection> Instance = new MetadataCollectionFormatter();

    private MetadataCollectionFormatter()
    {
    }

    public override MetadataCollection Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        if (reader.NextMessagePackType == MessagePackType.Integer)
        {
            var referenceId = reader.ReadInt32();
            return options.Metadata.GetValue(referenceId);
        }

        // Divide the number of array elements by two because each key/value pair is stored as two elements.
        var count = reader.ReadArrayHeader() / 2;

        using var builder = new MetadataBuilder();

        for (var i = 0; i < count; i++)
        {
            var key = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
            var value = CachedStringFormatter.Instance.Deserialize(ref reader, options);

            builder.Add(key, value);
        }

        var result = builder.Build();

        options.Metadata.Add(result);

        return result;
    }

    public override void Serialize(ref MessagePackWriter writer, MetadataCollection value, SerializerCachingOptions options)
    {
        if (options.Metadata.TryGetReferenceId(value, out var referenceId))
        {
            writer.Write(referenceId);
            return;
        }
        else
        {
            options.Metadata.Add(value);
        }

        writer.WriteArrayHeader(value.Count * 2);

        foreach (var (k, v) in value)
        {
            CachedStringFormatter.Instance.Serialize(ref writer, k, options);
            CachedStringFormatter.Instance.Serialize(ref writer, v, options);
        }
    }

    public override void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        if (reader.NextMessagePackType == MessagePackType.Integer)
        {
            reader.Skip(); // Reference Id
            return;
        }

        // Divide the number of array elements by two because each key/value pair is stored as two elements.
        var count = reader.ReadArrayHeader() / 2;

        using var builder = new MetadataBuilder();

        for (var i = 0; i < count; i++)
        {
            var key = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
            var value = CachedStringFormatter.Instance.Deserialize(ref reader, options);

            builder.Add(key, value);
        }

        var result = builder.Build();

        options.Metadata.Add(result);
    }
}

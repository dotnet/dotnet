// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Language;

#if !NET
using System.Collections.Generic;
#endif

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal sealed class MetadataCollectionFormatter : TagHelperObjectFormatter<MetadataCollection>
{
    public static readonly TagHelperObjectFormatter<MetadataCollection> Instance = new MetadataCollectionFormatter();

    private MetadataCollectionFormatter()
    {
    }

    public override MetadataCollection Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        if (cache is not null && reader.NextMessagePackType == MessagePackType.Integer)
        {
            var referenceId = reader.ReadInt32();
            return cache.Metadata.GetValue(referenceId);
        }

        // Divide the number of array elements by two because each key/value pair is stored as two elements.
        var count = reader.ReadArrayHeader() / 2;

        using var builder = new MetadataBuilder();

        for (var i = 0; i < count; i++)
        {
            var key = reader.ReadString(cache).AssumeNotNull();
            var value = reader.ReadString(cache);

            builder.Add(key, value);
        }

        var result = builder.Build();

        cache?.Metadata.Add(result);

        return result;
    }

    public override void Serialize(ref MessagePackWriter writer, MetadataCollection value, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        if (cache is not null)
        {
            if (cache.Metadata.TryGetReferenceId(value, out var referenceId))
            {
                writer.Write(referenceId);
                return;
            }
            else
            {
                cache.Metadata.Add(value);
            }
        }

        writer.WriteArrayHeader(value.Count * 2);

        foreach (var (k, v) in value)
        {
            writer.Write(k, cache);
            writer.Write(v, cache);
        }
    }
}

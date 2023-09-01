// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using MessagePack;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters.TagHelpers;

internal abstract class TagHelperObjectFormatter<T> : MessagePackFormatter<T>
{
    public sealed override T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
      => Deserialize(ref reader, options, cache: null);

    public sealed override void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options)
        => Serialize(ref writer, value, options, cache: null);

    public sealed override T[] DeserializeArray(ref MessagePackReader reader, MessagePackSerializerOptions options)
        => DeserializeArray(ref reader, options, cache: null);

    public sealed override ImmutableArray<T> DeserializeImmutableArray(ref MessagePackReader reader, MessagePackSerializerOptions options)
        => DeserializeImmutableArray(ref reader, options, cache: null);

    public sealed override void SerializeArray(ref MessagePackWriter writer, ImmutableArray<T> array, MessagePackSerializerOptions options)
        => SerializeArray(ref writer, array, options, cache: null);

    public sealed override void SerializeArray(ref MessagePackWriter writer, IReadOnlyList<T> array, MessagePackSerializerOptions options)
        => SerializeArray(ref writer, array, options, cache: null);

    public abstract T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache);
    public abstract void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options, TagHelperSerializationCache? cache);

    public T[] DeserializeArray(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        var count = reader.ReadArrayHeader();

        if (count == 0)
        {
            return Array.Empty<T>();
        }

        using var builder = new PooledArrayBuilder<T>(capacity: count);

        for (var i = 0; i < count; i++)
        {
            var item = Deserialize(ref reader, options, cache);
            builder.Add(item);
        }

        return builder.ToArray();
    }

    public virtual ImmutableArray<T> DeserializeImmutableArray(ref MessagePackReader reader, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        var count = reader.ReadArrayHeader();

        if (count == 0)
        {
            return ImmutableArray<T>.Empty;
        }

        using var builder = new PooledArrayBuilder<T>(capacity: count);

        for (var i = 0; i < count; i++)
        {
            var item = Deserialize(ref reader, options, cache);
            builder.Add(item);
        }

        return builder.DrainToImmutable();
    }

    public virtual void SerializeArray(ref MessagePackWriter writer, IReadOnlyList<T> array, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        var count = array.Count;
        writer.WriteArrayHeader(count);

        for (var i = 0; i < count; i++)
        {
            Serialize(ref writer, array[i], options, cache);
        }
    }

    public virtual void SerializeArray(ref MessagePackWriter writer, ImmutableArray<T> array, MessagePackSerializerOptions options, TagHelperSerializationCache? cache)
    {
        writer.WriteArrayHeader(array.Length);

        foreach (var item in array)
        {
            Serialize(ref writer, item, options, cache);
        }
    }
}

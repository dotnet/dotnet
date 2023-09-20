// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using MessagePack;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal abstract partial class MessagePackFormatter<T> : IMessagePackFormatter<T>
{
    private static readonly StringInterningFormatter s_stringFormatter = new();

    public AllowNullWrapper AllowNull => new(this);

    public abstract T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options);
    public abstract void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options);

    public static string DeserializeString(ref MessagePackReader reader, MessagePackSerializerOptions options)
        => s_stringFormatter.Deserialize(ref reader, options).AssumeNotNull();

    public virtual T[] DeserializeArray(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var count = reader.ReadArrayHeader();

        if (count == 0)
        {
            return Array.Empty<T>();
        }

        using var builder = new PooledArrayBuilder<T>(capacity: count);

        for (var i = 0; i < count; i++)
        {
            var item = Deserialize(ref reader, options);
            builder.Add(item);
        }

        return builder.ToArray();
    }

    public virtual ImmutableArray<T> DeserializeImmutableArray(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var count = reader.ReadArrayHeader();

        if (count == 0)
        {
            return ImmutableArray<T>.Empty;
        }

        using var builder = new PooledArrayBuilder<T>(capacity: count);

        for (var i = 0; i < count; i++)
        {
            var item = Deserialize(ref reader, options);
            builder.Add(item);
        }

        return builder.DrainToImmutable();
    }

    public virtual void SerializeArray(ref MessagePackWriter writer, IReadOnlyList<T> array, MessagePackSerializerOptions options)
    {
        var count = array.Count;
        writer.WriteArrayHeader(count);

        for (var i = 0; i < count; i++)
        {
            Serialize(ref writer, array[i], options);
        }
    }

    public virtual void SerializeArray(ref MessagePackWriter writer, ImmutableArray<T> array, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(array.Length);

        foreach (var item in array)
        {
            Serialize(ref writer, item, options);
        }
    }
}

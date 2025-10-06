// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using MessagePack;
using MessagePack.Formatters;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

/// <summary>
///  A message pack formatter that can be serialized at the top-level.
///  It will create a <see cref="SerializerCachingOptions"/> instance if one isn't
///  passed to <see cref="MessagePackSerializer"/>.
/// </summary>
internal abstract partial class TopLevelFormatter<T> : IMessagePackFormatter<T>
{
    public static readonly Type TargetType = typeof(T);

    public T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (options is SerializerCachingOptions cachingOptions)
        {
            return Deserialize(ref reader, cachingOptions);
        }

        using (cachingOptions = new SerializerCachingOptions(options))
        {
            return Deserialize(ref reader, cachingOptions);
        }
    }

    public void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options)
    {
        if (options is SerializerCachingOptions cachingOptions)
        {
            Serialize(ref writer, value, cachingOptions);
            return;
        }

        using (cachingOptions = new SerializerCachingOptions(options))
        {
            Serialize(ref writer, value, cachingOptions);
        }
    }

    public abstract T Deserialize(ref MessagePackReader reader, SerializerCachingOptions options);
    public abstract void Serialize(ref MessagePackWriter writer, T value, SerializerCachingOptions options);
}

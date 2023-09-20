// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using MessagePack;
using MessagePack.Formatters;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

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

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using MessagePack;
using MessagePack.Formatters;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

/// <summary>
///  A message package formatter that cannot be serialized at the top-level. It expects a
///  <see cref="SerializerCachingOptions"/> instance to be passed to it.
/// </summary>
internal abstract partial class ValueFormatter<T> : IMessagePackFormatter<T>
{
    public static readonly Type TargetType = typeof(T);

    public T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (options is SerializerCachingOptions cachingOptions)
        {
            return Deserialize(ref reader, cachingOptions);
        }

        ThrowMissingCachingOptions();

        return default!;
    }

    public void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options)
    {
        if (options is SerializerCachingOptions cachingOptions)
        {
            Serialize(ref writer, value, cachingOptions);
            return;
        }

        ThrowMissingCachingOptions();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowMissingCachingOptions()
    {
        throw new InvalidOperationException($"Expected to be given a {nameof(SerializerCachingOptions)} instance.");
    }

    public abstract T Deserialize(ref MessagePackReader reader, SerializerCachingOptions options);
    public abstract void Serialize(ref MessagePackWriter writer, T value, SerializerCachingOptions options);

    /// <summary>
    ///  Partially deserializes an object and ensures that any cached references are handled. Descendents should override
    ///  this method if they support skimming.
    /// </summary>
    public virtual void Skim(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        throw new NotImplementedException("Formatter does not support skimming.");
    }

    public void SkimArray(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        if (reader.TryReadNil())
        {
            return;
        }

        var length = reader.ReadArrayHeader();
        if (length == 0)
        {
            return;
        }

        options.Security.DepthStep(ref reader);
        try
        {
            for (var i = 0; i < length; i++)
            {
                reader.CancellationToken.ThrowIfCancellationRequested();
                Skim(ref reader, options);
            }
        }
        finally
        {
            reader.Depth--;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using MessagePack;
using MessagePack.Formatters;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

internal abstract partial class NonCachingFormatter<T> : IMessagePackFormatter<T>
{
    public static readonly Type TargetType = typeof(T);

    public abstract T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options);
    public abstract void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options);
}

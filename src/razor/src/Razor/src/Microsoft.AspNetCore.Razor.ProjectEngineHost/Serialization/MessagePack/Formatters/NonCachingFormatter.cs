// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using MessagePack;
using MessagePack.Formatters;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal abstract partial class NonCachingFormatter<T> : IMessagePackFormatter<T>
{
    public static readonly Type TargetType = typeof(T);

    public abstract T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options);
    public abstract void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options);
}

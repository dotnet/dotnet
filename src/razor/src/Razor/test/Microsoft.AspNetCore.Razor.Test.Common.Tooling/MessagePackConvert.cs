// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using MessagePack;
using Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

namespace Microsoft.AspNetCore.Razor.Test.Common;

internal static class MessagePackConvert
{
    public static T Deserialize<T>(ReadOnlyMemory<byte> bytes, MessagePackSerializerOptions options)
    {
        using var cachingOptions = new SerializerCachingOptions(options);

        return MessagePackSerializer.Deserialize<T>(bytes, cachingOptions);
    }

    public static ReadOnlyMemory<byte> Serialize<T>(T value, MessagePackSerializerOptions options)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var cachingOptions = new SerializerCachingOptions(options);

        MessagePackSerializer.Serialize(buffer, value, cachingOptions);

        return buffer.WrittenMemory;
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Buffers;
using MessagePack;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

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

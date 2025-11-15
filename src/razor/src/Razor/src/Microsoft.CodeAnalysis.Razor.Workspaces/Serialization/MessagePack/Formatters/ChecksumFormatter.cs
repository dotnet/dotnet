// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

internal sealed class ChecksumFormatter : NonCachingFormatter<Checksum>
{
    private const int PropertyCount = 2;

    public static readonly NonCachingFormatter<Checksum> Instance = new ChecksumFormatter();

    private ChecksumFormatter()
    {
    }

    public override Checksum Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        reader.ReadArrayHeaderAndVerify(PropertyCount);

        var data1 = reader.ReadInt64();
        var data2 = reader.ReadInt64();

        return new Checksum(data1, data2);
    }

    public override void Serialize(ref MessagePackWriter writer, Checksum value, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(PropertyCount);

        writer.Write(value.Data1);
        writer.Write(value.Data2);
    }
}

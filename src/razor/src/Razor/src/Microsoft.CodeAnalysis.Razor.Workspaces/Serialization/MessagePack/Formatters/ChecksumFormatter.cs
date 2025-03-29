// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class ChecksumFormatter : NonCachingFormatter<Checksum>
{
    public static readonly NonCachingFormatter<Checksum> Instance = new ChecksumFormatter();

    private ChecksumFormatter()
    {
    }

    public override Checksum Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        reader.ReadArrayHeaderAndVerify(4);

        var data1 = reader.ReadInt64();
        var data2 = reader.ReadInt64();
        var data3 = reader.ReadInt64();
        var data4 = reader.ReadInt64();

        var hashData = new Checksum.HashData(data1, data2, data3, data4);

        return new Checksum(hashData);
    }

    public override void Serialize(ref MessagePackWriter writer, Checksum value, MessagePackSerializerOptions options)
    {
        writer.WriteArrayHeader(4);

        var hashData = value.Data;

        writer.Write(hashData.Data1);
        writer.Write(hashData.Data2);
        writer.Write(hashData.Data3);
        writer.Write(hashData.Data4);
    }
}

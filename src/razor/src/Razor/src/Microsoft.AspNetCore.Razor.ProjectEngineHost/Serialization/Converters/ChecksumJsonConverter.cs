// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Serialization.Converters;

internal class ChecksumJsonConverter : ObjectJsonConverter<Checksum>
{
    public static readonly ChecksumJsonConverter Instance = new();

    private ChecksumJsonConverter()
    {
    }

    protected override Checksum ReadFromProperties(JsonDataReader reader)
        => ObjectReaders.ReadChecksumFromProperties(reader);

    protected override void WriteProperties(JsonDataWriter writer, Checksum value)
        => ObjectWriters.WriteProperties(writer, value);
}

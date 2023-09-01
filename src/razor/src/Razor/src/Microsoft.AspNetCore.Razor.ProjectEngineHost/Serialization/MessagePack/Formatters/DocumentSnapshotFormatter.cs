// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Formatters;

internal sealed class DocumentSnapshotHandleFormatter : MessagePackFormatter<DocumentSnapshotHandle>
{
    public static readonly MessagePackFormatter<DocumentSnapshotHandle> Instance = new DocumentSnapshotHandleFormatter();

    private DocumentSnapshotHandleFormatter()
    {
    }

    public override DocumentSnapshotHandle Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        var filePath = DeserializeString(ref reader, options);
        var targetPath = DeserializeString(ref reader, options);
        var fileKind = DeserializeString(ref reader, options);

        return new DocumentSnapshotHandle(filePath, targetPath, fileKind);
    }

    public override void Serialize(ref MessagePackWriter writer, DocumentSnapshotHandle value, MessagePackSerializerOptions options)
    {
        writer.Write(value.FilePath);
        writer.Write(value.TargetPath);
        writer.Write(value.FileKind);
    }
}

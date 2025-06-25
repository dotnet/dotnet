// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Formatters;

internal sealed class DocumentSnapshotHandleFormatter : ValueFormatter<DocumentSnapshotHandle>
{
    public static readonly ValueFormatter<DocumentSnapshotHandle> Instance = new DocumentSnapshotHandleFormatter();

    private DocumentSnapshotHandleFormatter()
    {
    }

    public override DocumentSnapshotHandle Deserialize(ref MessagePackReader reader, SerializerCachingOptions options)
    {
        reader.ReadArrayHeaderAndVerify(3);

        var filePath = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var targetPath = CachedStringFormatter.Instance.Deserialize(ref reader, options).AssumeNotNull();
        var fileKind = (RazorFileKind)reader.ReadByte();

        return new DocumentSnapshotHandle(filePath, targetPath, fileKind);
    }

    public override void Serialize(ref MessagePackWriter writer, DocumentSnapshotHandle value, SerializerCachingOptions options)
    {
        writer.WriteArrayHeader(3);

        CachedStringFormatter.Instance.Serialize(ref writer, value.FilePath, options);
        CachedStringFormatter.Instance.Serialize(ref writer, value.TargetPath, options);
        writer.Write((byte)value.FileKind);
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Language;

public enum MetadataKind : byte
{
    None,
    TypeParameter,
    Property,
    ChildContentParameter,
}

public abstract class MetadataObject(MetadataKind kind)
{
    public static readonly MetadataObject None = new NoMetadataObject();

    public MetadataKind Kind { get; } = kind;

    internal void AppendToChecksum(in Checksum.Builder builder)
    {
        builder.AppendData((byte)Kind);

        BuildChecksum(in builder);
    }

    private protected abstract void BuildChecksum(in Checksum.Builder builder);

    private sealed class NoMetadataObject() : MetadataObject(MetadataKind.None)
    {
        private protected override void BuildChecksum(in Checksum.Builder builder)
        {
            // No more data to append.
        }
    }
}

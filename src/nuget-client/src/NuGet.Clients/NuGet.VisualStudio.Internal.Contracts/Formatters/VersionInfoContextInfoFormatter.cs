// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class VersionInfoContextInfoFormatter : NuGetMessagePackFormatter<VersionInfoContextInfo>
    {
        private const string NuGetVersionPropertyName = "nugetversion";
        private const string DownloadCountPropertyName = "downloadcount";
        private const string PackageDeprecationMetadataPropertyName = "packagedeprecationmetadata";
        private const string PackageSearchMetadataPropertyName = "packagesearchmetadata";

        internal static readonly IMessagePackFormatter<VersionInfoContextInfo?> Instance = new VersionInfoContextInfoFormatter();

        private VersionInfoContextInfoFormatter()
        {
        }

        protected override VersionInfoContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            NuGetVersion? nuGetVersion = null;
            long? downloadCount = null;
            PackageDeprecationMetadataContextInfo? packageDeprecationMetadata = null;
            PackageSearchMetadataContextInfo? packageSearchMetadata = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case NuGetVersionPropertyName:
                        nuGetVersion = NuGetVersionFormatter.Instance.Deserialize(ref reader, options);
                        break;
                    case DownloadCountPropertyName:
                        if (!reader.TryReadNil())
                        {
                            downloadCount = reader.ReadInt64();
                        }
                        break;
                    case PackageDeprecationMetadataPropertyName:
                        if (!reader.TryReadNil())
                        {
                            packageDeprecationMetadata = PackageDeprecationMetadataContextInfoFormatter.Instance.Deserialize(ref reader, options);
                        }
                        break;
                    case PackageSearchMetadataPropertyName:
                        if (!reader.TryReadNil())
                        {
                            packageSearchMetadata = PackageSearchMetadataContextInfoFormatter.Instance.Deserialize(ref reader, options);
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNull(nuGetVersion);

            return new VersionInfoContextInfo(nuGetVersion, downloadCount)
            {
                PackageSearchMetadata = packageSearchMetadata,
                PackageDeprecationMetadata = packageDeprecationMetadata,
            };
        }

        protected override void SerializeCore(ref MessagePackWriter writer, VersionInfoContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 4);
            writer.Write(NuGetVersionPropertyName);
            NuGetVersionFormatter.Instance.Serialize(ref writer, value.Version, options);
            writer.Write(DownloadCountPropertyName);
            if (value.DownloadCount.HasValue)
            {
                writer.Write(value.DownloadCount.Value);
            }
            else
            {
                writer.WriteNil();
            }

            writer.Write(PackageSearchMetadataPropertyName);
            if (value.PackageSearchMetadata == null)
            {
                writer.WriteNil();
            }
            else
            {
                PackageSearchMetadataContextInfoFormatter.Instance.Serialize(ref writer, value.PackageSearchMetadata, options);
            }

            writer.Write(PackageDeprecationMetadataPropertyName);
            if (value.PackageDeprecationMetadata == null)
            {
                writer.WriteNil();
            }
            else
            {
                PackageDeprecationMetadataContextInfoFormatter.Instance.Serialize(ref writer, value.PackageDeprecationMetadata, options);
            }
        }
    }
}

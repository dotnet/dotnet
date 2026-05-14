// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class AlternatePackageMetadataContextInfoFormatter : NuGetMessagePackFormatter<AlternatePackageMetadataContextInfo>
    {
        private const string PackageIdPropertyName = "packageid";
        private const string VersionRangePropertyName = "versionrange";

        internal static readonly IMessagePackFormatter<AlternatePackageMetadataContextInfo?> Instance = new AlternatePackageMetadataContextInfoFormatter();

        private AlternatePackageMetadataContextInfoFormatter()
        {
        }

        protected override AlternatePackageMetadataContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            string? packageId = null;
            VersionRange? range = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case PackageIdPropertyName:
                        packageId = reader.ReadString();
                        break;
                    case VersionRangePropertyName:
                        range = VersionRangeFormatter.Instance.Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNullOrEmpty(packageId);
            Assumes.NotNull(range);

            return new AlternatePackageMetadataContextInfo(packageId, range);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, AlternatePackageMetadataContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 2);
            writer.Write(PackageIdPropertyName);
            writer.Write(value.PackageId);
            writer.Write(VersionRangePropertyName);
            VersionRangeFormatter.Instance.Serialize(ref writer, value.VersionRange, options);
        }
    }
}

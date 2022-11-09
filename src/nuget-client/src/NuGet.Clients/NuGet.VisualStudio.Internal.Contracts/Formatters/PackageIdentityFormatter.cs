// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class PackageIdentityFormatter : NuGetMessagePackFormatter<PackageIdentity>
    {
        private const string NuGetVersionPropertyName = "version";
        private const string IdPropertyName = "id";

        internal static readonly IMessagePackFormatter<PackageIdentity?> Instance = new PackageIdentityFormatter();

        private PackageIdentityFormatter()
        {
        }

        protected override PackageIdentity? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            string? id = null;
            NuGetVersion? version = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case IdPropertyName:
                        id = reader.ReadString();
                        break;
                    case NuGetVersionPropertyName:
                        version = NuGetVersionFormatter.Instance.Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return new PackageIdentity(id, version);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, PackageIdentity value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 2);
            writer.Write(IdPropertyName);
            writer.Write(value.Id);
            writer.Write(NuGetVersionPropertyName);
            NuGetVersionFormatter.Instance.Serialize(ref writer, value.Version, options);
        }
    }
}

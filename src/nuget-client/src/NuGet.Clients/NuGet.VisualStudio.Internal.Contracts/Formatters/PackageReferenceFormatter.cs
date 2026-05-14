// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class PackageReferenceFormatter : NuGetMessagePackFormatter<PackageReference>
    {
        private const string AllowedVersionsPropertyName = "allowedversions";
        private const string IsDevelopmentDependencyPropertyName = "isdevelopmentdependency";
        private const string IsUserInstalledPropertyName = "isuserinstalled";
        private const string PackageIdentityPropertyName = "packageidentity";
        private const string RequireReinstallationPropertyName = "requirereinstallation";
        private const string TargetFrameworkPropertyName = "targetframework";

        internal static readonly IMessagePackFormatter<PackageReference?> Instance = new PackageReferenceFormatter();

        private PackageReferenceFormatter()
        {
        }

        protected override PackageReference? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            VersionRange? allowedVersions = null;
            bool isDevelopmentDependency = false;
            bool isUserInstalled = true;
            bool requireReinstallation = false;
            PackageIdentity? identity = null;
            NuGetFramework? framework = null;

            int propertyCount = reader.ReadMapHeader();

            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case AllowedVersionsPropertyName:
                        allowedVersions = VersionRangeFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    case IsDevelopmentDependencyPropertyName:
                        isDevelopmentDependency = reader.ReadBoolean();
                        break;

                    case IsUserInstalledPropertyName:
                        isUserInstalled = reader.ReadBoolean();
                        break;

                    case PackageIdentityPropertyName:
                        identity = PackageIdentityFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    case RequireReinstallationPropertyName:
                        requireReinstallation = reader.ReadBoolean();
                        break;

                    case TargetFrameworkPropertyName:
                        framework = NuGetFrameworkFormatter.Instance.Deserialize(ref reader, options);
                        break;

                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNull(identity);
            Assumes.NotNull(framework);

            return new PackageReference(
                identity,
                framework,
                isUserInstalled,
                isDevelopmentDependency,
                requireReinstallation,
                allowedVersions);
        }

        protected override void SerializeCore(ref MessagePackWriter writer, PackageReference value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 6);
            writer.Write(PackageIdentityPropertyName);
            PackageIdentityFormatter.Instance.Serialize(ref writer, value.PackageIdentity, options);
            writer.Write(TargetFrameworkPropertyName);
            NuGetFrameworkFormatter.Instance.Serialize(ref writer, value.TargetFramework, options);
            writer.Write(IsUserInstalledPropertyName);
            writer.Write(value.IsUserInstalled);
            writer.Write(IsDevelopmentDependencyPropertyName);
            writer.Write(value.IsDevelopmentDependency);
            writer.Write(RequireReinstallationPropertyName);
            writer.Write(value.RequireReinstallation);
            writer.Write(AllowedVersionsPropertyName);
            VersionRangeFormatter.Instance.Serialize(ref writer, value.AllowedVersions, options);
        }
    }
}

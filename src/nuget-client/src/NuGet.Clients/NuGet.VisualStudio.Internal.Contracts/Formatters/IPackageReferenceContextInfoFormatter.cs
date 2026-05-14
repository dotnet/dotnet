// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal sealed class IPackageReferenceContextInfoFormatter : NuGetMessagePackFormatter<IPackageReferenceContextInfo>
    {
        private const string IdentityPropertyName = "identity";
        private const string FrameworkPropertyName = "framework";
        private const string IsUserInstalledPropertyName = "isuserinstalled";
        private const string IsAutoReferencedPropertyName = "isautoreferenced";
        private const string IsDevelopmentDependencyPropertyName = "isdevelopmentdependency";
        private const string AllowedVersionsPropertyName = "allowedversions";

        internal static readonly IMessagePackFormatter<IPackageReferenceContextInfo?> Instance = new IPackageReferenceContextInfoFormatter();

        private IPackageReferenceContextInfoFormatter()
        {
        }

        protected override IPackageReferenceContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            PackageIdentity? identity = null;
            NuGetFramework? framework = null;
            bool isUserInstalled = false;
            bool isAutoReferenced = false;
            bool isDevelopmentDependency = false;
            string? allowedVersions = null;

            int propertyCount = reader.ReadMapHeader();
            for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
            {
                switch (reader.ReadString())
                {
                    case IdentityPropertyName:
                        identity = PackageIdentityFormatter.Instance.Deserialize(ref reader, options);
                        break;
                    case FrameworkPropertyName:
                        framework = NuGetFrameworkFormatter.Instance.Deserialize(ref reader, options);
                        break;
                    case IsUserInstalledPropertyName:
                        isUserInstalled = reader.ReadBoolean();
                        break;
                    case IsAutoReferencedPropertyName:
                        isAutoReferenced = reader.ReadBoolean();
                        break;
                    case IsDevelopmentDependencyPropertyName:
                        isDevelopmentDependency = reader.ReadBoolean();
                        break;
                    case AllowedVersionsPropertyName:
                        if (!reader.TryReadNil()) // Advances beyond the current value if the current value is nil and returns true; otherwise leaves the reader's position unchanged and returns false.
                        {
                            allowedVersions = reader.ReadString();
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNull(identity);

            var packageReferenceContextInfo = new PackageReferenceContextInfo(identity, framework)
            {
                IsUserInstalled = isUserInstalled,
                IsAutoReferenced = isAutoReferenced,
                IsDevelopmentDependency = isDevelopmentDependency
            };

            if (!string.IsNullOrWhiteSpace(allowedVersions) && VersionRange.TryParse(allowedVersions!, out VersionRange? versionRange))
            {
                packageReferenceContextInfo.AllowedVersions = versionRange;
            }

            return packageReferenceContextInfo;
        }

        protected override void SerializeCore(ref MessagePackWriter writer, IPackageReferenceContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 6);

            WriteSerialize(ref writer, value, options);
        }

        internal static void WriteSerialize(ref MessagePackWriter writer, IPackageReferenceContextInfo value, MessagePackSerializerOptions options)
        {
            writer.Write(IdentityPropertyName);
            PackageIdentityFormatter.Instance.Serialize(ref writer, value.Identity, options);
            writer.Write(FrameworkPropertyName);
            NuGetFrameworkFormatter.Instance.Serialize(ref writer, value.Framework, options);
            writer.Write(IsUserInstalledPropertyName);
            writer.Write(value.IsUserInstalled);
            writer.Write(IsAutoReferencedPropertyName);
            writer.Write(value.IsAutoReferenced);
            writer.Write(IsDevelopmentDependencyPropertyName);
            writer.Write(value.IsDevelopmentDependency);
            writer.Write(AllowedVersionsPropertyName);
            if (value.AllowedVersions == null)
            {
                writer.WriteNil();
            }
            else
            {
                writer.Write(value.AllowedVersions.ToString());
            }
        }
    }
}

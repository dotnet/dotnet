// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using MessagePack;
using MessagePack.Formatters;
using Microsoft;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    /// <summary>
    /// Inspired in <see cref="IPackageReferenceContextInfoFormatter"/>
    /// </summary>
    internal class ITransitivePackageReferenceContextInfoFormatter : NuGetMessagePackFormatter<ITransitivePackageReferenceContextInfo>
    {
        private const string IdentityPropertyName = "identity";
        private const string FrameworkPropertyName = "framework";
        private const string IsUserInstalledPropertyName = "isuserinstalled";
        private const string IsAutoReferencedPropertyName = "isautoreferenced";
        private const string IsDevelopmentDependencyPropertyName = "isdevelopmentdependency";
        private const string AllowedVersionsPropertyName = "allowedversions";
        private const string TransitiveOriginsPropertyName = "transitiveorigins";

        internal static readonly IMessagePackFormatter<ITransitivePackageReferenceContextInfo?> Instance = new ITransitivePackageReferenceContextInfoFormatter();

        protected override ITransitivePackageReferenceContextInfo? DeserializeCore(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            PackageIdentity? identity = null;
            NuGetFramework? framework = null;
            bool isUserInstalled = false;
            bool isAutoReferenced = false;
            bool isDevelopmentDependency = false;
            string? allowedVersions = null;

            var transitiveOrigins = new List<IPackageReferenceContextInfo>();

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
                    case TransitiveOriginsPropertyName:
                        var elems = reader.ReadArrayHeader();
                        for (int i = 0; i < elems; i++)
                        {
                            var result = IPackageReferenceContextInfoFormatter.Instance.Deserialize(ref reader, options);
                            if (result != null)
                            {
                                transitiveOrigins.Add(result);
                            }
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            Assumes.NotNull(identity);

            var packageReferenceContextInfo = new TransitivePackageReferenceContextInfo(identity, framework)
            {
                IsUserInstalled = isUserInstalled,
                IsAutoReferenced = isAutoReferenced,
                IsDevelopmentDependency = isDevelopmentDependency
            };

            if (!string.IsNullOrWhiteSpace(allowedVersions) && VersionRange.TryParse(allowedVersions!, out VersionRange? versionRange))
            {
                packageReferenceContextInfo.AllowedVersions = versionRange;
            }
            packageReferenceContextInfo.TransitiveOrigins = transitiveOrigins;

            return packageReferenceContextInfo;
        }

        protected override void SerializeCore(ref MessagePackWriter writer, ITransitivePackageReferenceContextInfo value, MessagePackSerializerOptions options)
        {
            writer.WriteMapHeader(count: 7);

            IPackageReferenceContextInfoFormatter.WriteSerialize(ref writer, value, options);

            writer.Write(TransitiveOriginsPropertyName);
            writer.WriteArrayHeader(value.TransitiveOrigins.Count());
            foreach (IPackageReferenceContextInfo val in value.TransitiveOrigins)
            {
                IPackageReferenceContextInfoFormatter.Instance.Serialize(ref writer, val, options);
            }
        }
    }
}

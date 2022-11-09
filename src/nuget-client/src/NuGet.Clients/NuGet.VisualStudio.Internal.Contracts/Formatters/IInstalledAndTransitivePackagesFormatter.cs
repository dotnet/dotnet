// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;
using Microsoft;

namespace NuGet.VisualStudio.Internal.Contracts
{
    internal class IInstalledAndTransitivePackagesFormatter : IMessagePackFormatter<IInstalledAndTransitivePackages?>
    {
        private const string InstalledPackagesPropertyName = "installedPackages";
        private const string TransitivePackagesPropertyName = "transitivePackages";

        internal static readonly IMessagePackFormatter<IInstalledAndTransitivePackages?> Instance = new IInstalledAndTransitivePackagesFormatter();

        private IInstalledAndTransitivePackagesFormatter()
        {
        }

        public IInstalledAndTransitivePackages? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            // stack overflow mitigation - see https://github.com/neuecc/MessagePack-CSharp/security/advisories/GHSA-7q36-4xx7-xcxf
            options.Security.DepthStep(ref reader);

            try
            {
                List<IPackageReferenceContextInfo>? installedPackages = null;
                List<ITransitivePackageReferenceContextInfo>? transitivePackages = null;

                int propertyCount = reader.ReadMapHeader();

                for (int propertyIndex = 0; propertyIndex < propertyCount; propertyIndex++)
                {
                    switch (reader.ReadString())
                    {
                        case InstalledPackagesPropertyName:
                            if (!reader.TryReadNil())
                            {
                                installedPackages = new List<IPackageReferenceContextInfo>();

                                int installedPackagesCount = reader.ReadArrayHeader();

                                for (int i = 0; i < installedPackagesCount; ++i)
                                {
                                    IPackageReferenceContextInfo? packageReferenceContextInfo = IPackageReferenceContextInfoFormatter.Instance.Deserialize(ref reader, options);
                                    Assumes.NotNull(packageReferenceContextInfo);
                                    installedPackages.Add(packageReferenceContextInfo);
                                }
                            }
                            break;

                        case TransitivePackagesPropertyName:
                            if (!reader.TryReadNil())
                            {
                                transitivePackages = new List<ITransitivePackageReferenceContextInfo>();

                                int transitivePackagesCount = reader.ReadArrayHeader();

                                for (int i = 0; i < transitivePackagesCount; ++i)
                                {
                                    ITransitivePackageReferenceContextInfo? packageReferenceContextInfo = ITransitivePackageReferenceContextInfoFormatter.Instance.Deserialize(ref reader, options);
                                    Assumes.NotNull(packageReferenceContextInfo);
                                    transitivePackages.Add(packageReferenceContextInfo);
                                }
                            }
                            break;

                        default:
                            reader.Skip();
                            break;
                    }
                }

                return new InstalledAndTransitivePackages(installedPackages, transitivePackages);
            }
            finally
            {
                // stack overflow mitigation - see https://github.com/neuecc/MessagePack-CSharp/security/advisories/GHSA-7q36-4xx7-xcxf
                reader.Depth--;
            }
        }

        public void Serialize(ref MessagePackWriter writer, IInstalledAndTransitivePackages? value, MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteMapHeader(count: 2);
            writer.Write(InstalledPackagesPropertyName);
            if (value.InstalledPackages is null)
            {
                writer.WriteNil();
            }
            else
            {
                writer.WriteArrayHeader(value.InstalledPackages.Count);

                foreach (IPackageReferenceContextInfo installedPackage in value.InstalledPackages)
                {
                    IPackageReferenceContextInfoFormatter.Instance.Serialize(ref writer, installedPackage, options);
                }
            }
            writer.Write(TransitivePackagesPropertyName);
            if (value.TransitivePackages is null)
            {
                writer.WriteNil();
            }
            else
            {
                writer.WriteArrayHeader(value.TransitivePackages.Count);

                foreach (IPackageReferenceContextInfo transitivePackage in value.TransitivePackages)
                {
                    IPackageReferenceContextInfoFormatter.Instance.Serialize(ref writer, transitivePackage, options);
                }
            }
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using Microsoft.ServiceHub.Framework;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    internal class PackageLicenseUtilities
    {
        internal static IReadOnlyList<IText> GenerateLicenseLinks(DetailedPackageMetadata metadata)
        {
            return GenerateLicenseLinks(metadata.LicenseMetadata, metadata.LicenseUrl, string.Format(CultureInfo.CurrentCulture, Resources.WindowTitle_LicenseFileWindow, metadata.Id), metadata.PackagePath, new PackageIdentity(metadata.Id, metadata.Version));
        }

        internal static IReadOnlyList<IText> GenerateLicenseLinks(IPackageSearchMetadata metadata)
        {
            if (metadata is LocalPackageSearchMetadata localMetadata)
            {
                return GenerateLicenseLinks(metadata.LicenseMetadata, metadata.LicenseUrl, string.Format(CultureInfo.CurrentCulture, Resources.WindowTitle_LicenseFileWindow, metadata.Identity.Id), localMetadata.PackagePath, metadata.Identity);
            }
            return GenerateLicenseLinks(metadata.LicenseMetadata, metadata.LicenseUrl, metadata.Identity.Id, null, metadata.Identity);
        }

        internal static IReadOnlyList<IText> GenerateLicenseLinks(LicenseMetadata licenseMetadata, Uri licenseUrl, string licenseFileHeader, string packagePath, PackageIdentity packageIdentity)
        {
            if (licenseMetadata != null)
            {
                return GenerateLicenseLinks(licenseMetadata, licenseFileHeader, packagePath, packageIdentity);
            }
            else if (licenseUrl != null)
            {
                return new List<IText>() { new LicenseText(Resources.Text_ViewLicense, licenseUrl) };
            }
            return new List<IText>();
        }

        internal static Paragraph[] GenerateParagraphs(string licenseContent)
        {
            var textParagraphs = licenseContent.Split(
                new[] { "\n\n", "\r\n\r\n" }, // Take care of paragraphs regardless of the name ending. It's a best effort, so weird line ending combinations might not work too well.
                StringSplitOptions.None);

            var paragraphs = new Paragraph[textParagraphs.Length];
            for (var i = 0; i < textParagraphs.Length; i++)
            {
                paragraphs[i] = new Paragraph(new Run(textParagraphs[i]));
            }
            return paragraphs;
        }

        // Internal for testing purposes.
        internal static IReadOnlyList<IText> GenerateLicenseLinks(LicenseMetadata metadata, string licenseFileHeader, string packagePath, PackageIdentity packageIdentity)
        {
            var list = new List<IText>();

            if (metadata.WarningsAndErrors != null)
            {
                list.Add(new WarningText(string.Join(Environment.NewLine, metadata.WarningsAndErrors)));
            }

            switch (metadata.Type)
            {
                case LicenseType.Expression:

                    if (metadata.LicenseExpression != null && !metadata.LicenseExpression.IsUnlicensed())
                    {
                        var identifiers = new List<string>();
                        PopulateLicenseIdentifiers(metadata.LicenseExpression, identifiers);

                        var licenseToBeProcessed = metadata.License;

                        foreach (var identifier in identifiers)
                        {
                            var licenseStart = licenseToBeProcessed.IndexOf(identifier, StringComparison.OrdinalIgnoreCase);
                            if (licenseStart != 0)
                            {
                                list.Add(new FreeText(licenseToBeProcessed.Substring(0, licenseStart)));
                            }
                            var license = licenseToBeProcessed.Substring(licenseStart, identifier.Length);
                            list.Add(new LicenseText(license, new Uri(string.Format(CultureInfo.CurrentCulture, LicenseMetadata.LicenseServiceLinkTemplate, license))));
                            licenseToBeProcessed = licenseToBeProcessed.Substring(licenseStart + identifier.Length);
                        }

                        if (licenseToBeProcessed.Length != 0)
                        {
                            list.Add(new FreeText(licenseToBeProcessed));
                        }
                    }
                    else
                    {
                        list.Add(new FreeText(metadata.License));
                    }

                    break;

                case LicenseType.File:
                    NuGetPackageFileService.AddLicenseToCache(
                        packageIdentity,
                        CreateEmbeddedLicenseUri(packagePath, metadata));
                    list.Add(new LicenseFileText(Resources.Text_ViewLicense, licenseFileHeader, packagePath, metadata.License, packageIdentity));
                    break;

                default:
                    break;
            }

            return list;
        }

        private static Uri CreateEmbeddedLicenseUri(string packagePath, LicenseMetadata licenseMetadata)
        {
            Uri baseUri = new Uri(packagePath, UriKind.Absolute);

            var builder = new UriBuilder(baseUri)
            {
                Fragment = licenseMetadata.License
            };

            return builder.Uri;
        }

        private static void PopulateLicenseIdentifiers(NuGetLicenseExpression expression, IList<string> identifiers)
        {
            switch (expression.Type)
            {
                case LicenseExpressionType.License:
                    var license = (NuGetLicense)expression;
                    identifiers.Add(license.Identifier);
                    break;

                case LicenseExpressionType.Operator:
                    var licenseOperator = (LicenseOperator)expression;
                    switch (licenseOperator.OperatorType)
                    {
                        case LicenseOperatorType.LogicalOperator:
                            var logicalOperator = (LogicalOperator)licenseOperator;
                            PopulateLicenseIdentifiers(logicalOperator.Left, identifiers);
                            PopulateLicenseIdentifiers(logicalOperator.Right, identifiers);
                            break;

                        case LicenseOperatorType.WithOperator:
                            var withOperator = (WithOperator)licenseOperator;
                            identifiers.Add(withOperator.License.Identifier);
                            identifiers.Add(withOperator.Exception.Identifier);
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }
        }

        public static async Task<string> GetEmbeddedLicenseAsync(PackageIdentity packageIdentity, CancellationToken cancellationToken)
        {
            string content = null;

            IServiceBrokerProvider serviceBrokerProvider = await ServiceLocator.GetComponentModelServiceAsync<IServiceBrokerProvider>();
            IServiceBroker serviceBroker = await serviceBrokerProvider.GetAsync();

            using (INuGetPackageFileService packageFileService = await serviceBroker.GetProxyAsync<INuGetPackageFileService>(NuGetServices.PackageFileService))
            {
                if (packageFileService != null)
                {
                    using (Stream stream = await packageFileService.GetEmbeddedLicenseAsync(packageIdentity, CancellationToken.None))
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                content = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return content;
        }
    }
}

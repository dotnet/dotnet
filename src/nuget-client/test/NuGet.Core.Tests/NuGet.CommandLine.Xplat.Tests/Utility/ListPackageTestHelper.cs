// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.CommandLine.Xplat.Tests
{
    public class ListPackageTestHelper
    {
        internal static InstalledPackageReference CreateInstalledPackageReference(
            string packageId = "Package.Id",
            bool autoReference = false,
            bool isDeprecated = false,
            int vulnerabilityCount = 0,
            string resolvedPackageVersionString = "1.0.0",
            string latestPackageVersionString = "2.0.0")
        {
            var latestPackageVersion = new NuGetVersion(latestPackageVersionString);
            var resolvedPackageVersion = new NuGetVersion(resolvedPackageVersionString);

            var resolvedPackageMetadata = new Mock<IPackageSearchMetadata>();
            resolvedPackageMetadata.Setup(m => m.Identity).Returns(new PackageIdentity(packageId, resolvedPackageVersion));
            if (isDeprecated)
            {
                resolvedPackageMetadata
                    .Setup(m => m.GetDeprecationMetadataAsync())
                    .ReturnsAsync(new PackageDeprecationMetadata()
                    {
                        Reasons = new[] { "Legacy" },
                        AlternatePackage = new AlternatePackageMetadata()
                        {
                            PackageId = "Package.New",
                            Range = new VersionRange(new NuGetVersion("1.0.0"))
                        }
                    });
            }

            var vulnerabilities = (List<PackageVulnerabilityMetadata>)null;
            if (vulnerabilityCount > 0)
            {
                vulnerabilities = new List<PackageVulnerabilityMetadata>();
                for (int i = 0; i < vulnerabilityCount; i++)
                {
                    vulnerabilities.Add(new PackageVulnerabilityMetadata()
                    {
                        AdvisoryUrl = new Uri("http://example/advisory" + i),
                        Severity = i
                    });
                }
            }
            resolvedPackageMetadata
                .Setup(m => m.Vulnerabilities)
                .Returns(vulnerabilities);

            var installedPackageReference = new InstalledPackageReference(packageId)
            {
                AutoReference = autoReference,

                LatestPackageMetadata = PackageSearchMetadataBuilder
                .FromIdentity(new PackageIdentity(packageId, latestPackageVersion))
                .Build(),
                OriginalRequestedVersion = resolvedPackageVersionString,

                ResolvedPackageMetadata = resolvedPackageMetadata.Object
            };

            return installedPackageReference;
        }
    }
}

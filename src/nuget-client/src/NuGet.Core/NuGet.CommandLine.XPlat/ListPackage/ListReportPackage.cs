// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Protocol;

namespace NuGet.CommandLine.XPlat.ListPackage
{
    internal class ListReportPackage
    {
        internal string PackageId { get; private set; }
        internal string ResolvedVersion { get; private set; }
        internal string LatestVersion { get; private set; }
        public List<PackageVulnerabilityMetadata> Vulnerabilities { get; private set; }
        internal PackageDeprecationMetadata DeprecationReasons { get; private set; }
        internal AlternatePackageMetadata AlternativePackage { get; private set; }
        internal string RequestedVersion { get; private set; } // not needed for transitive package
        internal bool AutoReference { get; private set; } // not needed for transitive package

        public ListReportPackage(string packageId, string resolvedVersion, string latestVersion, List<PackageVulnerabilityMetadata> vulnerabilities, PackageDeprecationMetadata deprecationReasons, AlternatePackageMetadata alternativePackage, string requestedVersion, bool autoReference)
        {
            PackageId = packageId;
            ResolvedVersion = resolvedVersion;
            LatestVersion = latestVersion;
            Vulnerabilities = vulnerabilities;
            DeprecationReasons = deprecationReasons;
            AlternativePackage = alternativePackage;
            RequestedVersion = requestedVersion;
            AutoReference = autoReference;
        }

        public ListReportPackage(string packageId, string requestedVersion, string resolvedVersion, string latestVersion)
            : this(
                  packageId: packageId,
                  resolvedVersion: resolvedVersion,
                  latestVersion: latestVersion,
                  vulnerabilities: null,
                  deprecationReasons: null,
                  alternativePackage: null,
                  requestedVersion: requestedVersion,
                  autoReference: false)
        { }

        public ListReportPackage(string packageId, string requestedVersion, string resolvedVersion)
            : this(
                  packageId: packageId,
                  requestedVersion: requestedVersion,
                  resolvedVersion: resolvedVersion,
                  autoReference: false)
        { }

        public ListReportPackage(string packageId, string requestedVersion, string resolvedVersion, bool autoReference)
            : this(
                  packageId: packageId,
                  requestedVersion: requestedVersion,
                  resolvedVersion: resolvedVersion,
                  latestVersion: null,
                  autoReference: autoReference)
        { }

        public ListReportPackage(string packageId, string requestedVersion, string resolvedVersion, string latestVersion, bool autoReference)
            : this(
                  packageId: packageId,
                  resolvedVersion: resolvedVersion,
                  latestVersion: latestVersion,
                  vulnerabilities: null,
                  deprecationReasons: null,
                  alternativePackage: null,
                  requestedVersion: requestedVersion,
                  autoReference: autoReference)
        { }

        public ListReportPackage(string packageId, string requestedVersion, string resolvedVersion, PackageDeprecationMetadata deprecationReasons, AlternatePackageMetadata alternativePackage)
            : this(
                  packageId: packageId,
                  resolvedVersion: resolvedVersion,
                  latestVersion: null,
                  vulnerabilities: null,
                  deprecationReasons: deprecationReasons,
                  alternativePackage: alternativePackage,
                  requestedVersion: requestedVersion,
                  autoReference: false)
        { }

        public ListReportPackage(string packageId, string requestedVersion, string resolvedVersion, string latestVersion, List<PackageVulnerabilityMetadata> vulnerabilities)
            : this(
                  packageId: packageId,
                  resolvedVersion: resolvedVersion,
                  latestVersion: latestVersion,
                  vulnerabilities: vulnerabilities,
                  deprecationReasons: null,
                  alternativePackage: null,
                  requestedVersion: requestedVersion,
                  autoReference: false)
        { }
    }
}

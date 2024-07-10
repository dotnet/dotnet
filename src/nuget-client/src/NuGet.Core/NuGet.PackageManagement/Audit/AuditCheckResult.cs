// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement
{
    public record AuditCheckResult
    {
        public IReadOnlyList<ILogMessage> Warnings { get; }
        internal bool IsAuditEnabled { get; set; } = true;

        internal int Severity0VulnerabilitiesFound { get; set; }
        internal int Severity1VulnerabilitiesFound { get; set; }
        internal int Severity2VulnerabilitiesFound { get; set; }
        internal int Severity3VulnerabilitiesFound { get; set; }
        internal int InvalidSeverityVulnerabilitiesFound { get; set; }
        internal int TotalWarningsSuppressedCount { get; set; }
        internal int DistinctAdvisoriesSuppressedCount { get; set; }
        internal List<PackageIdentity>? Packages { get; set; }
        internal double? DownloadDurationInSeconds { get; set; }
        internal double? CheckPackagesDurationInSeconds { get; set; }
        internal int? SourcesWithVulnerabilities { get; set; }

        private const string AuditVulnerabilitiesStatus = "PackagesConfig.Audit.Enabled";
        private const string AuditVulnerabilitiesCount = "PackagesConfig.Audit.Vulnerability.Count";
        private const string AuditVulnerabilitiesSev0Count = "PackagesConfig.Audit.Vulnerability.Severity0.Count";
        private const string AuditVulnerabilitiesSev1Count = "PackagesConfig.Audit.Vulnerability.Severity1.Count";
        private const string AuditVulnerabilitiesSev2Count = "PackagesConfig.Audit.Vulnerability.Severity2.Count";
        private const string AuditVulnerabilitiesSev3Count = "PackagesConfig.Audit.Vulnerability.Severity3.Count";
        private const string AuditVulnerabilitiesInvalidSeverityCount = "PackagesConfig.Audit.Vulnerability.SeverityInvalid.Count";
        private const string AuditSuppressedAdvisoriesTotalWarningsSuppressedCount = "PackagesConfig.Audit.SuppressedAdvisories.TotalWarningsSuppressed.Count";
        private const string AuditSuppressedAdvisoriesDistinctAdvisoriesSuppressedCount = "PackagesConfig.Audit.SuppressedAdvisories.DistinctAdvisoriesSuppressed.Count";
        private const string AuditDurationDownload = "PackagesConfig.Audit.Duration.Download";
        private const string AuditDurationCheck = "PackagesConfig.Audit.Duration.Check";
        private const string SourcesWithVulnerabilitiesCount = "PackagesConfig.Audit.DataSources.Count";
        private const string AuditVulnerabilitiesPackages = "PackagesConfig.Audit.Vulnerability.Packages";


        internal AuditCheckResult(IReadOnlyList<ILogMessage> warnings)
        {
            if (warnings is null)
            {
                throw new ArgumentNullException(nameof(warnings));
            }

            Warnings = warnings;
        }

        public void AddMetricsToTelemetry(TelemetryEvent telemetryEvent)
        {
            telemetryEvent[AuditVulnerabilitiesStatus] = IsAuditEnabled;
            telemetryEvent[AuditVulnerabilitiesSev0Count] = Severity0VulnerabilitiesFound;
            telemetryEvent[AuditVulnerabilitiesSev1Count] = Severity1VulnerabilitiesFound;
            telemetryEvent[AuditVulnerabilitiesSev2Count] = Severity2VulnerabilitiesFound;
            telemetryEvent[AuditVulnerabilitiesSev3Count] = Severity3VulnerabilitiesFound;
            telemetryEvent[AuditVulnerabilitiesInvalidSeverityCount] = InvalidSeverityVulnerabilitiesFound;
            telemetryEvent[AuditSuppressedAdvisoriesTotalWarningsSuppressedCount] = TotalWarningsSuppressedCount;
            telemetryEvent[AuditSuppressedAdvisoriesDistinctAdvisoriesSuppressedCount] = DistinctAdvisoriesSuppressedCount;
            telemetryEvent[AuditVulnerabilitiesCount] = Packages?.Count ?? 0;

            if (DownloadDurationInSeconds.HasValue)
            {
                telemetryEvent[AuditDurationDownload] = DownloadDurationInSeconds;
            }
            if (CheckPackagesDurationInSeconds.HasValue)
            {
                telemetryEvent[AuditDurationCheck] = CheckPackagesDurationInSeconds;
            }

            if (SourcesWithVulnerabilities.HasValue)
            {
                telemetryEvent[SourcesWithVulnerabilitiesCount] = SourcesWithVulnerabilities;
            }

            if (Packages is not null)
            {
                List<TelemetryEvent> result = new List<TelemetryEvent>(Packages.Count);
                foreach (var package in Packages)
                {
                    TelemetryEvent packageData = new TelemetryEvent(eventName: string.Empty);
                    packageData.AddPiiData("id", package.Id.ToLowerInvariant());
                    packageData["version"] = package.Version;
                    result.Add(packageData);
                }
                telemetryEvent.ComplexData[AuditVulnerabilitiesPackages] = result;
            }
        }
    }
}

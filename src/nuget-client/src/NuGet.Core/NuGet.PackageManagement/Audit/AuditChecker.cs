// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Model;
using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.PackageManagement
{
    public class AuditChecker(
        List<SourceRepository> packageSources,
        IReadOnlyList<SourceRepository>? auditSources,
        SourceCacheContext sourceCacheContext,
        ILogger logger)
    {
        private readonly List<SourceRepository> _packageSources = packageSources;
        private readonly IReadOnlyList<SourceRepository>? _auditSources = auditSources;
        private readonly ILogger _logger = logger;
        private readonly SourceCacheContext _sourceCacheContext = sourceCacheContext;

        public AuditChecker(
             List<SourceRepository> sourceRepositories,
             SourceCacheContext sourceCacheContext,
             ILogger logger)
            : this(sourceRepositories, auditSources: null, sourceCacheContext, logger)
        {
        }

        public async Task<AuditCheckResult> CheckPackageVulnerabilitiesAsync(IEnumerable<PackageRestoreData> packages, Dictionary<string, RestoreAuditProperties> restoreAuditProperties, CancellationToken cancellationToken)
        {
            if (packages == null) throw new ArgumentNullException(nameof(packages));
            if (restoreAuditProperties == null) throw new ArgumentNullException(nameof(restoreAuditProperties));

            // Before fetching vulnerability data, check if any projects are enabled for audit
            // If there are no settings, then run the audit for all packages
            bool anyProjectsEnabledForAudit = restoreAuditProperties.Count == 0;
            var auditSettings = new Dictionary<string, ProjectAuditSettings>(restoreAuditProperties.Count);
            foreach (var (projectPath, restoreAuditProperty) in restoreAuditProperties)
            {
                _ = restoreAuditProperty.TryParseEnableAudit(out bool isAuditEnabled);
                _ = restoreAuditProperty.TryParseAuditLevel(out PackageVulnerabilitySeverity minimumAuditSeverity);
                auditSettings.Add(projectPath, new ProjectAuditSettings(isAuditEnabled, minimumAuditSeverity, restoreAuditProperty.SuppressedAdvisories));
                anyProjectsEnabledForAudit |= isAuditEnabled;
            }

            if (!anyProjectsEnabledForAudit)
            {
                return new AuditCheckResult(Array.Empty<ILogMessage>())
                {
                    IsAuditEnabled = false,
                };
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            (int sourceWithVulnerabilityCount, GetVulnerabilityInfoResult? allVulnerabilityData) = await GetAllVulnerabilityDataAsync(_packageSources, _auditSources, _sourceCacheContext, _logger, cancellationToken);
            stopwatch.Stop();
            double downloadDurationInSeconds = stopwatch.Elapsed.TotalSeconds;

            if (allVulnerabilityData?.Exceptions is not null)
            {
                foreach (Exception exception in allVulnerabilityData.Exceptions.InnerExceptions)
                {
                    var messageText = string.Format(Strings.Error_VulnerabilityDataFetch, exception.Message);
                    var logMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1900, messageText);
                    _logger.Log(logMessage);
                }
            }

            if (allVulnerabilityData is null || !IsAnyVulnerabilityDataFound(allVulnerabilityData.KnownVulnerabilities))
            {
                return new AuditCheckResult(Array.Empty<ILogMessage>())
                {
                    DownloadDurationInSeconds = downloadDurationInSeconds,
                    SourcesWithVulnerabilities = sourceWithVulnerabilityCount,
                };
            }

            stopwatch.Restart();
            Dictionary<PackageIdentity, PackageAuditInfo>? packagesWithKnownVulnerabilities = FindPackagesWithKnownVulnerabilities(allVulnerabilityData.KnownVulnerabilities!, packages);
            int Sev0Matches = 0, Sev1Matches = 0, Sev2Matches = 0, Sev3Matches = 0, InvalidSevMatches = 0, TotalWarningsSuppressedCount = 0, DistinctAdvisoriesSuppressedCount = 0;

            List<PackageIdentity> packagesWithReportedAdvisories = new(packagesWithKnownVulnerabilities?.Count ?? 0);

            IReadOnlyList<ILogMessage> warnings = packagesWithKnownVulnerabilities is not null
                ? CreateWarnings(packagesWithKnownVulnerabilities, auditSettings, ref Sev0Matches, ref Sev1Matches, ref Sev2Matches, ref Sev3Matches, ref InvalidSevMatches,
                                    ref TotalWarningsSuppressedCount, ref DistinctAdvisoriesSuppressedCount, ref packagesWithReportedAdvisories)
                : Array.Empty<ILogMessage>();

            foreach (var warning in warnings.NoAllocEnumerate())
            {
                _logger.Log(warning);
            }

            stopwatch.Stop();
            double checkPackagesDurationInSeconds = stopwatch.Elapsed.TotalSeconds;

            return new AuditCheckResult(warnings)
            {
                Severity0VulnerabilitiesFound = Sev0Matches,
                Severity1VulnerabilitiesFound = Sev1Matches,
                Severity2VulnerabilitiesFound = Sev2Matches,
                Severity3VulnerabilitiesFound = Sev3Matches,
                InvalidSeverityVulnerabilitiesFound = InvalidSevMatches,
                TotalWarningsSuppressedCount = TotalWarningsSuppressedCount,
                DistinctAdvisoriesSuppressedCount = DistinctAdvisoriesSuppressedCount,
                Packages = packagesWithReportedAdvisories,
                DownloadDurationInSeconds = downloadDurationInSeconds,
                CheckPackagesDurationInSeconds = checkPackagesDurationInSeconds,
                SourcesWithVulnerabilities = sourceWithVulnerabilityCount,
            };

            static bool IsAnyVulnerabilityDataFound(IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>? knownVulnerabilities)
            {
                if (knownVulnerabilities is null || knownVulnerabilities.Count == 0)
                {
                    return false;
                }

                for (var i = 0; i < knownVulnerabilities.Count; i++)
                {
                    if (knownVulnerabilities[i].Count > 0) { return true; }
                }
                return false;
            }
        }

        internal static async Task<(int, GetVulnerabilityInfoResult?)> GetAllVulnerabilityDataAsync(
            List<SourceRepository> packageSources,
            IReadOnlyList<SourceRepository>? auditSources,
            SourceCacheContext sourceCacheContext,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            int SourcesWithVulnerabilityData = 0;
            List<Task<GetVulnerabilityInfoResult?>> results;
            IReadOnlyList<SourceRepository> vulnerabilitySources;
            bool usingAuditSources;
            if (auditSources?.Count > 0)
            {
                results = new(auditSources.Count);
                vulnerabilitySources = auditSources;
                usingAuditSources = true;
            }
            else
            {
                results = new(packageSources.Count);
                vulnerabilitySources = packageSources;
                usingAuditSources = false;
            }

            for (int i = 0; i < vulnerabilitySources.Count; i++)
            {
                SourceRepository source = vulnerabilitySources[i];
                Task<GetVulnerabilityInfoResult?> getVulnerabilityInfoResult = GetVulnerabilityInfoAsync(source, sourceCacheContext, logger);
                results.Add(getVulnerabilityInfoResult);
            }

            await Task.WhenAll(results);
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            List<Exception>? errors = null;
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>? knownVulnerabilities = null;
            for (int i = 0; i < results.Count; i++)
            {
                Task<GetVulnerabilityInfoResult?> resultTask = results[i];
                GetVulnerabilityInfoResult? result = await resultTask;

                if (result is null)
                {
                    if (usingAuditSources)
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, Strings.Warning_AuditSourceWithoutData, vulnerabilitySources[i].PackageSource.Name);
                        RestoreLogMessage restoreLogMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1905, message);
                        logger.Log(restoreLogMessage);
                    }
                    continue;
                }

                if (result.KnownVulnerabilities != null)
                {
                    SourcesWithVulnerabilityData++;
                    knownVulnerabilities ??= new();

                    knownVulnerabilities.AddRange(result.KnownVulnerabilities);
                }

                if (result.Exceptions != null)
                {
                    errors ??= new();

                    errors.AddRange(result.Exceptions.InnerExceptions);
                }
            }

            GetVulnerabilityInfoResult? final =
                knownVulnerabilities != null || errors != null
                ? new(knownVulnerabilities, errors != null ? new AggregateException(errors) : null)
                : null;
            return (SourcesWithVulnerabilityData, final);

            static async Task<GetVulnerabilityInfoResult?> GetVulnerabilityInfoAsync(SourceRepository source, SourceCacheContext cacheContext, ILogger logger)
            {
                try
                {
                    IVulnerabilityInfoResource vulnerabilityInfoResource =
                        await source.GetResourceAsync<IVulnerabilityInfoResource>(CancellationToken.None);
                    if (vulnerabilityInfoResource is null)
                    {
                        return null;
                    }
                    return await vulnerabilityInfoResource.GetVulnerabilityInfoAsync(cacheContext, logger, CancellationToken.None);
                }
                catch (Exception exception)
                {
                    AggregateException aggregateException = new(exception);
                    GetVulnerabilityInfoResult result = new(knownVulnerabilities: null, exceptions: aggregateException);
                    return result;
                }
            }
        }

        internal static List<LogMessage> CreateWarnings(Dictionary<PackageIdentity, PackageAuditInfo> packagesWithKnownVulnerabilities,
            Dictionary<string, ProjectAuditSettings> auditSettings,
            ref int Sev0Matches,
            ref int Sev1Matches,
            ref int Sev2Matches,
            ref int Sev3Matches,
            ref int InvalidSevMatches,
            ref int TotalWarningsSuppressedCount,
            ref int DistinctAdvisoriesSuppressedCount,
            ref List<PackageIdentity> packagesWithReportedAdvisories)
        {
            var warnings = new List<LogMessage>();
            foreach ((PackageIdentity package, PackageAuditInfo auditInfo) in packagesWithKnownVulnerabilities.OrderBy(p => p.Key.Id))
            {
                bool isVulnerabilityReported = false;

                foreach (PackageVulnerabilityInfo vulnerability in auditInfo.Vulnerabilities)
                {
                    (var severityLabel, NuGetLogCode logCode) = GetSeverityLabelAndCode(vulnerability.Severity);
                    var message = string.Format(Strings.Warning_PackageWithKnownVulnerability,
                        package.Id,
                        package.Version.ToNormalizedString(),
                        severityLabel,
                        vulnerability.Url);

                    bool counted = false;
                    for (int i = 0; i < auditInfo.Projects.Count; i++)
                    {
                        string projectPath = auditInfo.Projects[i];
                        auditSettings.TryGetValue(projectPath, out ProjectAuditSettings? auditSetting);

                        if (auditSetting == default || auditSetting.IsAuditEnabled && (int)vulnerability.Severity >= (int)auditSetting.MinimumSeverity)
                        {
                            if (CheckIfAdvisoryHasBeenSuppressed(auditSetting?.SuppressedAdvisories, vulnerability.Url.OriginalString, ref TotalWarningsSuppressedCount, ref DistinctAdvisoriesSuppressedCount))
                            {
                                continue;
                            }

                            isVulnerabilityReported = true;
                            if (!counted)
                            {
                                switch (vulnerability.Severity)
                                {
                                    case PackageVulnerabilitySeverity.Low:
                                        Sev0Matches++;
                                        break;
                                    case PackageVulnerabilitySeverity.Moderate:
                                        Sev1Matches++;
                                        break;
                                    case PackageVulnerabilitySeverity.High:
                                        Sev2Matches++;
                                        break;
                                    case PackageVulnerabilitySeverity.Critical:
                                        Sev3Matches++;
                                        break;
                                    default:
                                        InvalidSevMatches++;
                                        break;
                                }
                            }
                            counted = true;

                            var restoreLogMessage = LogMessage.CreateWarning(logCode, message);
                            restoreLogMessage.ProjectPath = projectPath;
                            warnings.Add(restoreLogMessage);
                        }
                    }
                }
                if (isVulnerabilityReported)
                {
                    packagesWithReportedAdvisories.Add(package);
                }
            }
            return warnings;
        }

        internal static Dictionary<PackageIdentity, PackageAuditInfo>? FindPackagesWithKnownVulnerabilities(
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities,
            IEnumerable<PackageRestoreData> packages)
        {
            Dictionary<PackageIdentity, PackageAuditInfo>? result = null;

            foreach (PackageRestoreData packageRestoreData in packages.NoAllocEnumerate())
            {
                PackageIdentity packageIdentity = packageRestoreData.PackageReference.PackageIdentity;
                List<PackageVulnerabilityInfo>? knownVulnerabilitiesForPackage = GetKnownVulnerabilities(packageIdentity.Id, packageIdentity.Version, knownVulnerabilities);

                if (knownVulnerabilitiesForPackage?.Count > 0)
                {
                    foreach (PackageVulnerabilityInfo knownVulnerability in knownVulnerabilitiesForPackage)
                    {
                        result ??= new();

                        if (!result.TryGetValue(packageIdentity, out PackageAuditInfo? auditInfo))
                        {
                            auditInfo = new(packageIdentity, packageRestoreData.ProjectNames.AsList());
                            result.Add(packageIdentity, auditInfo);
                        }

                        auditInfo.Vulnerabilities.Add(knownVulnerability);
                    }
                }
            }

            return result;
        }

        internal static List<PackageVulnerabilityInfo>? GetKnownVulnerabilities(
            string name,
            NuGetVersion version,
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities)
        {
            HashSet<PackageVulnerabilityInfo>? vulnerabilities = null;

            foreach (var file in knownVulnerabilities)
            {
                if (file.TryGetValue(name, out var packageVulnerabilities))
                {
                    foreach (var vulnerabilityInfo in packageVulnerabilities)
                    {
                        if (vulnerabilityInfo.Versions.Satisfies(version))
                        {
                            vulnerabilities ??= new();
                            vulnerabilities.Add(vulnerabilityInfo);
                        }
                    }
                }
            }

            return vulnerabilities?.ToList();
        }

        internal static (string severityLabel, NuGetLogCode code) GetSeverityLabelAndCode(PackageVulnerabilitySeverity severity)
        {
            switch (severity)
            {
                case PackageVulnerabilitySeverity.Low:
                    return ("low", NuGetLogCode.NU1901);
                case PackageVulnerabilitySeverity.Moderate:
                    return ("moderate", NuGetLogCode.NU1902);
                case PackageVulnerabilitySeverity.High:
                    return ("high", NuGetLogCode.NU1903);
                case PackageVulnerabilitySeverity.Critical:
                    return ("critical", NuGetLogCode.NU1904);
                default:
                    return ("unknown", NuGetLogCode.NU1900);
            }
        }

        private static bool CheckIfAdvisoryHasBeenSuppressed(Dictionary<string, bool>? suppressedAdvisories, string advisoryUrl, ref int totalWarningsSuppressedCount, ref int distinctAdvisoriesSuppressedCount)
        {
            if (suppressedAdvisories?.TryGetValue(advisoryUrl, out bool advisoryUsed) == true)
            {
                totalWarningsSuppressedCount++;

                if (!advisoryUsed)
                {
                    suppressedAdvisories[advisoryUrl] = true;
                    distinctAdvisoriesSuppressedCount++;
                }

                return true;
            }

            return false;
        }

        internal class PackageAuditInfo
        {
            public PackageIdentity Identity { get; }

            public IList<string> Projects { get; }

            public List<PackageVulnerabilityInfo> Vulnerabilities { get; }

            public PackageAuditInfo(PackageIdentity identity, IList<string> projects)
            {
                Identity = identity;
                Vulnerabilities = new();
                Projects = projects;
            }
        }

        internal class ProjectAuditSettings
        {
            public bool IsAuditEnabled { get; }

            public PackageVulnerabilitySeverity MinimumSeverity { get; }

            public Dictionary<string, bool>? SuppressedAdvisories { get; }

            public ProjectAuditSettings(bool enableAudit, PackageVulnerabilitySeverity auditLevel, HashSet<string>? suppressedAdvisories)
            {
                IsAuditEnabled = enableAudit;
                MinimumSeverity = auditLevel;

                if (suppressedAdvisories != null)
                {
                    SuppressedAdvisories = new Dictionary<string, bool>(suppressedAdvisories.Count);

                    foreach (string advisory in suppressedAdvisories)
                    {
                        SuppressedAdvisories.Add(advisory, false);
                    }
                }
            }
        }
    }
}

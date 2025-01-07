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
using NuGet.DependencyResolver;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Model;
using NuGet.Versioning;

namespace NuGet.Commands.Restore.Utility
{
    internal class AuditUtility
    {
        private readonly RestoreAuditProperties? _restoreAuditProperties;
        private readonly string _projectFullPath;
        private readonly IEnumerable<RestoreTargetGraph> _targetGraphs;
        private readonly IReadOnlyList<IVulnerabilityInformationProvider> _vulnerabilityInfoProviders;
        private readonly ILogger _logger;
        private readonly IList<TargetFrameworkInformation> _targetFrameworks;

        internal PackageVulnerabilitySeverity MinSeverity { get; }
        internal NuGetAuditMode AuditMode { get; }
        internal Dictionary<string, bool>? SuppressedAdvisories { get; }
        internal List<string>? DirectPackagesWithAdvisory { get; private set; }
        internal List<string>? PackageDownloadPackagesWithAdvisory { get; private set; }
        internal List<string>? TransitivePackagesWithAdvisory { get; private set; }
        internal int Sev0DirectMatches { get; private set; }
        internal int Sev1DirectMatches { get; private set; }
        internal int Sev2DirectMatches { get; private set; }
        internal int Sev3DirectMatches { get; private set; }
        internal int InvalidSevDirectMatches { get; private set; }
        internal int Sev0TransitiveMatches { get; private set; }
        internal int Sev1TransitiveMatches { get; private set; }
        internal int Sev2TransitiveMatches { get; private set; }
        internal int Sev3TransitiveMatches { get; private set; }
        internal int InvalidSevTransitiveMatches { get; private set; }
        internal double? DownloadDurationSeconds { get; private set; }
        internal double? CheckPackagesDurationSeconds { get; private set; }
        internal double? GenerateOutputDurationSeconds { get; private set; }
        internal int SourcesWithVulnerabilityData { get; private set; }
        internal int DistinctAdvisoriesSuppressedCount { get; private set; }
        internal int TotalWarningsSuppressedCount { get; private set; }
        internal int TotalPackageDownloadWarningsSuppressedCount { get; private set; }
        internal int DistinctPackageDownloadAdvisoriesSuppressedCount { get; private set; }

        internal int Sev0PackageDownloadMatches { get; private set; }
        internal int Sev1PackageDownloadMatches { get; private set; }
        internal int Sev2PackageDownloadMatches { get; private set; }
        internal int Sev3PackageDownloadMatches { get; private set; }
        internal int InvalidSevPackageDownloadMatches { get; private set; }

        public AuditUtility(
            RestoreAuditProperties? restoreAuditProperties,
            string projectFullPath,
            IEnumerable<RestoreTargetGraph> graphs,
            IReadOnlyList<IVulnerabilityInformationProvider> vulnerabilityInformationProviders,
            IList<TargetFrameworkInformation> targetFrameworks,
            ILogger logger)
        {
            _targetFrameworks = targetFrameworks;
            _restoreAuditProperties = restoreAuditProperties;
            _projectFullPath = projectFullPath;
            _targetGraphs = graphs;
            _vulnerabilityInfoProviders = vulnerabilityInformationProviders;
            _logger = logger;

            MinSeverity = ParseAuditLevel();
            AuditMode = ParseAuditMode();

            if (restoreAuditProperties?.SuppressedAdvisories != null)
            {
                SuppressedAdvisories = new Dictionary<string, bool>(restoreAuditProperties.SuppressedAdvisories.Count);

                foreach (string advisory in restoreAuditProperties.SuppressedAdvisories)
                {
                    SuppressedAdvisories.Add(advisory, false);
                }
            }
        }

        private int CountPackageDownloads()
        {
            int count = 0;
            foreach (var targetFramework in _targetFrameworks.NoAllocEnumerate())
            {
                count += targetFramework.DownloadDependencies.Length;
            }
            return count;
        }

        private void CheckPackageDownloadVulnerabilities(IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities)
        {
            Dictionary<DownloadDependency, PackageDownloadAuditInfo>? packagesWithKnownVulnerabilities = FindPackageDownloadsWithKnownVulnerabilities(knownVulnerabilities);

            if (packagesWithKnownVulnerabilities == null)
            {
                return;
            }

            PackageDownloadPackagesWithAdvisory = new(capacity: packagesWithKnownVulnerabilities.Values.Count);

            foreach ((DownloadDependency package, PackageDownloadAuditInfo auditInfo) in packagesWithKnownVulnerabilities)
            {
                PackageDownloadPackagesWithAdvisory.Add(package.Name);

                foreach (var advisory in auditInfo.GraphsPerVulnerability.Keys)
                {
                    PackageVulnerabilitySeverity severity = advisory.Severity;
                    if (severity == PackageVulnerabilitySeverity.Low) { Sev0PackageDownloadMatches++; }
                    else if (severity == PackageVulnerabilitySeverity.Moderate) { Sev1PackageDownloadMatches++; }
                    else if (severity == PackageVulnerabilitySeverity.High) { Sev2PackageDownloadMatches++; }
                    else if (severity == PackageVulnerabilitySeverity.Critical) { Sev3PackageDownloadMatches++; }
                    else { InvalidSevPackageDownloadMatches++; }
                }
            }
        }

        private Dictionary<DownloadDependency, PackageDownloadAuditInfo>? FindPackageDownloadsWithKnownVulnerabilities(
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities)
        {
            Dictionary<DownloadDependency, PackageDownloadAuditInfo>? result = null;

            foreach (var targetFramework in _targetFrameworks.NoAllocEnumerate())
            {
                foreach (var downloadDependency in targetFramework.DownloadDependencies)
                {
                    List<PackageVulnerabilityInfo>? knownVulnerabilitiesForPackage = GetKnownVulnerabilities(downloadDependency.Name, downloadDependency.VersionRange.MaxVersion!, knownVulnerabilities);

                    if (knownVulnerabilitiesForPackage?.Count > 0)
                    {
                        foreach (PackageVulnerabilityInfo knownVulnerability in knownVulnerabilitiesForPackage)
                        {
                            if ((int)knownVulnerability.Severity < (int)MinSeverity && knownVulnerability.Severity != PackageVulnerabilitySeverity.Unknown)
                            {
                                continue;
                            }

                            if (SuppressedAdvisories?.TryGetValue(knownVulnerability.Url.OriginalString, out bool advisoryUsed) == true)
                            {
                                TotalPackageDownloadWarningsSuppressedCount++;

                                if (!advisoryUsed)
                                {
                                    SuppressedAdvisories[knownVulnerability.Url.OriginalString] = true;
                                    DistinctPackageDownloadAdvisoriesSuppressedCount++;
                                }

                                continue;
                            }

                            result ??= new();

                            if (!result.TryGetValue(downloadDependency, out PackageDownloadAuditInfo? auditInfo))
                            {
                                auditInfo = new(downloadDependency);
                                result.Add(downloadDependency, auditInfo);
                            }

                            if (!auditInfo.GraphsPerVulnerability.TryGetValue(knownVulnerability, out List<string>? affectedGraphs))
                            {
                                affectedGraphs = new();
                                auditInfo.GraphsPerVulnerability.Add(knownVulnerability, affectedGraphs);
                            }

                            // Multiple package sources might list the same known vulnerability, so de-dupe those too.
                            if (!affectedGraphs.Contains(downloadDependency.Name))
                            {
                                affectedGraphs.Add(downloadDependency.Name);
                            }
                        }
                    }
                }
            }

            return result;
        }


        public async Task<bool> CheckPackageVulnerabilitiesAsync(CancellationToken cancellationToken)
        {
            // Performance: Early exit if restore graph does not contain any packages.
            if (!HasPackages())
            {
                // No packages means we've validated there are none with known vulnerabilities.
                return true;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>? allVulnerabilityData = await GetAllVulnerabilityDataAsync(cancellationToken);
            stopwatch.Stop();
            DownloadDurationSeconds = stopwatch.Elapsed.TotalSeconds;

            // Performance: Early exit if there's no vulnerability data to check packages against.
            if (allVulnerabilityData is null || !AnyVulnerabilityDataFound(allVulnerabilityData))
            {
                return false;
            }

            CheckPackageVulnerabilities(allVulnerabilityData);
            CheckPackageDownloadVulnerabilities(allVulnerabilityData);
            return true;

            bool HasPackages()
            {
                if (CountPackageDownloads() > 0)
                {
                    return true;
                }

                foreach (RestoreTargetGraph graph in _targetGraphs)
                {
                    if (graph.Flattened.Any(r => r.Key.Type == LibraryType.Package))
                    {
                        return true;
                    }
                }

                return false;
            }

            bool AnyVulnerabilityDataFound(IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>? knownVulnerabilities)
            {
                if (knownVulnerabilities is null || knownVulnerabilities.Count == 0)
                {
                    return false;
                }

                for (int i = 0; i < knownVulnerabilities.Count; i++)
                {
                    if (knownVulnerabilities[i].Count > 0) { return true; }
                }

                return false;
            }
        }

        private void ReplayErrors(AggregateException exceptions)
        {
            foreach (Exception exception in exceptions.InnerExceptions)
            {
                var messageText = string.Format(Strings.Error_VulnerabilityDataFetch, exception.Message);
                RestoreLogMessage logMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1900, messageText);
                _logger.Log(logMessage);
            }
        }

        private void CheckPackageVulnerabilities(IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Dictionary<PackageIdentity, PackageAuditInfo>? packagesWithKnownVulnerabilities = FindPackagesWithKnownVulnerabilities(knownVulnerabilities);
            stopwatch.Stop();
            CheckPackagesDurationSeconds = stopwatch.Elapsed.TotalSeconds;

            if (packagesWithKnownVulnerabilities == null) return;

            stopwatch.Restart();

            int directPackageCount = packagesWithKnownVulnerabilities.Values.Count(p => p.IsDirect);
            DirectPackagesWithAdvisory = new(capacity: directPackageCount);
            TransitivePackagesWithAdvisory = new(capacity: packagesWithKnownVulnerabilities.Count - directPackageCount);

            // no-op checks DGSpec hash, which means the order of everything must be deterministic.
            foreach ((PackageIdentity package, PackageAuditInfo auditInfo) in packagesWithKnownVulnerabilities.OrderBy(p => p.Key.Id))
            {
                if (auditInfo.IsDirect || AuditMode == NuGetAuditMode.All)
                {
                    foreach (var kvp2 in auditInfo.GraphsPerVulnerability.OrderBy(v => v.Key.Url.OriginalString))
                    {
                        PackageVulnerabilityInfo vulnerability = kvp2.Key;
                        List<string> affectedGraphs = kvp2.Value;
                        (string severityLabel, NuGetLogCode logCode) = GetSeverityLabelAndCode(vulnerability.Severity);
                        string message = string.Format(Strings.Warning_PackageWithKnownVulnerability,
                            package.Id,
                            package.Version.ToNormalizedString(),
                            severityLabel,
                            vulnerability.Url);
                        RestoreLogMessage restoreLogMessage =
                            RestoreLogMessage.CreateWarning(logCode,
                            message,
                            package.Id,
                            affectedGraphs.OrderBy(s => s).ToArray());
                        _logger.Log(restoreLogMessage);
                    }
                }

                if (auditInfo.IsDirect)
                {
                    DirectPackagesWithAdvisory.Add(package.Id);

                    foreach (var advisory in auditInfo.GraphsPerVulnerability.Keys)
                    {
                        PackageVulnerabilitySeverity severity = advisory.Severity;
                        if (severity == PackageVulnerabilitySeverity.Low) { Sev0DirectMatches++; }
                        else if (severity == PackageVulnerabilitySeverity.Moderate) { Sev1DirectMatches++; }
                        else if (severity == PackageVulnerabilitySeverity.High) { Sev2DirectMatches++; }
                        else if (severity == PackageVulnerabilitySeverity.Critical) { Sev3DirectMatches++; }
                        else { InvalidSevDirectMatches++; }
                    }
                }
                else
                {
                    TransitivePackagesWithAdvisory.Add(package.Id);

                    foreach (var advisory in auditInfo.GraphsPerVulnerability.Keys)
                    {
                        PackageVulnerabilitySeverity severity = advisory.Severity;
                        if (severity == PackageVulnerabilitySeverity.Low) { Sev0TransitiveMatches++; }
                        else if (severity == PackageVulnerabilitySeverity.Moderate) { Sev1TransitiveMatches++; }
                        else if (severity == PackageVulnerabilitySeverity.High) { Sev2TransitiveMatches++; }
                        else if (severity == PackageVulnerabilitySeverity.Critical) { Sev3TransitiveMatches++; }
                        else { InvalidSevTransitiveMatches++; }
                    }
                }
            }

            stopwatch.Stop();
            GenerateOutputDurationSeconds = stopwatch.Elapsed.TotalSeconds;
        }

        private static List<PackageVulnerabilityInfo>? GetKnownVulnerabilities(
            string name,
            NuGetVersion version,
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities)
        {
            HashSet<PackageVulnerabilityInfo>? vulnerabilities = null;

            foreach (var file in knownVulnerabilities)
            {
                if (file.TryGetValue(name, out var packageVulnerabilities))
                {
                    foreach (var vulnInfo in packageVulnerabilities)
                    {
                        if (vulnInfo.Versions.Satisfies(version))
                        {
                            vulnerabilities ??= new();
                            vulnerabilities.Add(vulnInfo);
                        }
                    }
                }
            }

            return vulnerabilities?.ToList();
        }

        private static (string severityLabel, NuGetLogCode code) GetSeverityLabelAndCode(PackageVulnerabilitySeverity severity)
        {
            switch (severity)
            {
                case PackageVulnerabilitySeverity.Low:
                    return (Strings.Vulnerability_Severity_Low, NuGetLogCode.NU1901);
                case PackageVulnerabilitySeverity.Moderate:
                    return (Strings.Vulnerability_Severity_Moderate, NuGetLogCode.NU1902);
                case PackageVulnerabilitySeverity.High:
                    return (Strings.Vulnerability_Severity_High, NuGetLogCode.NU1903);
                case PackageVulnerabilitySeverity.Critical:
                    return (Strings.Vulnerability_Severity_Critical, NuGetLogCode.NU1904);
                default:
                    return (Strings.Vulnerability_Severity_unknown, NuGetLogCode.NU1900);
            }
        }

        private Dictionary<PackageIdentity, PackageAuditInfo>? FindPackagesWithKnownVulnerabilities(
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities)
        {
            // multi-targeting projects often use the same package across multiple TFMs, so group to reduce output spam.
            Dictionary<PackageIdentity, PackageAuditInfo>? result = null;

            foreach (RestoreTargetGraph graph in _targetGraphs)
            {
                GraphItem<RemoteResolveResult>? currentProject = graph.Graphs.FirstOrDefault()?.Item;

                foreach (GraphItem<RemoteResolveResult>? node in graph.Flattened.Where(r => r.Key.Type == LibraryType.Package))
                {
                    LibraryIdentity package = node.Key;
                    List<PackageVulnerabilityInfo>? knownVulnerabilitiesForPackage = GetKnownVulnerabilities(package.Name, package.Version, knownVulnerabilities);

                    if (knownVulnerabilitiesForPackage?.Count > 0)
                    {
                        PackageIdentity packageIdentity = new(package.Name, package.Version);

                        foreach (PackageVulnerabilityInfo knownVulnerability in knownVulnerabilitiesForPackage)
                        {
                            if ((int)knownVulnerability.Severity < (int)MinSeverity && knownVulnerability.Severity != PackageVulnerabilitySeverity.Unknown)
                            {
                                continue;
                            }

                            if (SuppressedAdvisories?.TryGetValue(knownVulnerability.Url.OriginalString, out bool advisoryUsed) == true)
                            {
                                TotalWarningsSuppressedCount++;

                                if (!advisoryUsed)
                                {
                                    SuppressedAdvisories[knownVulnerability.Url.OriginalString] = true;
                                    DistinctAdvisoriesSuppressedCount++;
                                }

                                continue;
                            }

                            result ??= new();

                            if (!result.TryGetValue(packageIdentity, out PackageAuditInfo? auditInfo))
                            {
                                auditInfo = new(packageIdentity);
                                result.Add(packageIdentity, auditInfo);
                            }

                            if (!auditInfo.GraphsPerVulnerability.TryGetValue(knownVulnerability, out List<string>? affectedGraphs))
                            {
                                affectedGraphs = new();
                                auditInfo.GraphsPerVulnerability.Add(knownVulnerability, affectedGraphs);
                            }

                            // Multiple package sources might list the same known vulnerability, so de-dupe those too.
                            if (!affectedGraphs.Contains(graph.TargetGraphName))
                            {
                                affectedGraphs.Add(graph.TargetGraphName);
                            }

                            if (!auditInfo.IsDirect &&
                                currentProject?.Data.Dependencies.Any(d => string.Equals(d.Name, packageIdentity.Id, StringComparison.OrdinalIgnoreCase)) == true)
                            {
                                auditInfo.IsDirect = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private async Task<List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>?> GetAllVulnerabilityDataAsync(CancellationToken cancellationToken)
        {
            var results = new Task<GetVulnerabilityInfoResult?>[_vulnerabilityInfoProviders.Count];
            for (int i = 0; i < _vulnerabilityInfoProviders.Count; i++)
            {
                IVulnerabilityInformationProvider provider = _vulnerabilityInfoProviders[i];
                results[i] = provider.GetVulnerabilityInformationAsync(cancellationToken);
            }

            await Task.WhenAll(results);
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>? knownVulnerabilities = null;
            for (int i = 0; i < results.Length; i++)
            {
                GetVulnerabilityInfoResult? result = await results[i];

                if (_vulnerabilityInfoProviders[i].IsAuditSource && (result?.KnownVulnerabilities is null || result.KnownVulnerabilities.Count == 0))
                {
                    string message = string.Format(CultureInfo.CurrentCulture, Strings.Warning_AuditSourceWithoutVulnerabilityData, _vulnerabilityInfoProviders[i].SourceName);
                    RestoreLogMessage logMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1905, message);
                    _logger.Log(logMessage);
                }

                if (result is null) continue;

                if (result.KnownVulnerabilities != null)
                {
                    SourcesWithVulnerabilityData++;
                    if (knownVulnerabilities == null)
                    {
                        knownVulnerabilities = new();
                    }

                    knownVulnerabilities.AddRange(result.KnownVulnerabilities);
                }

                if (result.Exceptions != null)
                {
                    ReplayErrors(result.Exceptions);
                }
            }

            return knownVulnerabilities;
        }

        private PackageVulnerabilitySeverity ParseAuditLevel()
        {
            string? auditLevel = _restoreAuditProperties?.AuditLevel?.Trim();

            if (auditLevel == null)
            {
                return PackageVulnerabilitySeverity.Low;
            }

            if (_restoreAuditProperties!.TryParseAuditLevel(out PackageVulnerabilitySeverity result))
            {
                return result;
            }

            string messageText = string.Format(Strings.Error_InvalidNuGetAuditLevelValue, auditLevel, "low, moderate, high, critical");
            RestoreLogMessage message = RestoreLogMessage.CreateError(NuGetLogCode.NU1014, messageText);
            _logger.Log(message);
            return PackageVulnerabilitySeverity.Low;
        }

        internal enum NuGetAuditMode { Unknown, Direct, All }

        // Enum parsing and ToString are a magnitude of times slower than a naive implementation. 
        private NuGetAuditMode ParseAuditMode()
        {
            string? auditMode = _restoreAuditProperties?.AuditMode?.Trim();

            if (auditMode == null)
            {
                return NuGetAuditMode.Unknown;
            }
            else if (string.Equals("direct", auditMode, StringComparison.OrdinalIgnoreCase))
            {
                return NuGetAuditMode.Direct;
            }
            else if (string.Equals("all", auditMode, StringComparison.OrdinalIgnoreCase))
            {
                return NuGetAuditMode.All;
            }

            string messageText = string.Format(Strings.Error_InvalidNuGetAuditModeValue, auditMode, "direct, all");
            RestoreLogMessage message = RestoreLogMessage.CreateError(NuGetLogCode.NU1014, messageText);
            message.ProjectPath = _projectFullPath;
            _logger.Log(message);
            return NuGetAuditMode.Unknown;
        }

        // Enum parsing and ToString are a magnitude of times slower than a naive implementation.
        public static bool ParseEnableValue(RestoreAuditProperties? value, string projectFullPath, ILogger logger)
        {
            if (value == null)
            {
                return true;
            }

            if (!value.TryParseEnableAudit(out bool result))
            {
                string messageText = string.Format(Strings.Error_InvalidNuGetAuditValue, value, "true, false");
                RestoreLogMessage message = RestoreLogMessage.CreateError(NuGetLogCode.NU1014, messageText);
                message.ProjectPath = projectFullPath;
                logger.Log(message);
            }

            return result;
        }

        // Enum parsing and ToString are a magnitude of times slower than a naive implementation.
        internal static string GetString(NuGetAuditMode auditMode)
        {
            return auditMode switch
            {
                NuGetAuditMode.All => nameof(NuGetAuditMode.All),
                NuGetAuditMode.Direct => nameof(NuGetAuditMode.Direct),
                NuGetAuditMode.Unknown => nameof(NuGetAuditMode.Unknown),
                _ => auditMode.ToString()
            };
        }

        private class PackageAuditInfo
        {
            public PackageIdentity Identity { get; }
            public bool IsDirect { get; set; }
            public Dictionary<PackageVulnerabilityInfo, List<string>> GraphsPerVulnerability { get; }

            public PackageAuditInfo(PackageIdentity identity)
            {
                Identity = identity;
                IsDirect = false;
                GraphsPerVulnerability = new();
            }
        }

        private class PackageDownloadAuditInfo
        {
            public DownloadDependency Identity { get; }
            public Dictionary<PackageVulnerabilityInfo, List<string>> GraphsPerVulnerability { get; }

            public PackageDownloadAuditInfo(DownloadDependency identity)
            {
                Identity = identity;
                GraphsPerVulnerability = new();
            }
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Model;
using NuGet.Versioning;

namespace NuGet.Commands.Restore.Utility
{
    internal class AuditUtility
    {
        private readonly ProjectModel.RestoreAuditProperties? _restoreAuditProperties;
        private readonly string _projectFullPath;
        private readonly IEnumerable<RestoreTargetGraph> _targetGraphs;
        private readonly IReadOnlyList<IVulnerabilityInformationProvider> _vulnerabilityInfoProviders;
        private readonly ILogger _logger;

        internal PackageVulnerabilitySeverity MinSeverity { get; }
        internal NuGetAuditMode AuditMode { get; }
        internal List<string>? DirectPackagesWithAdvisory { get; private set; }
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

        public AuditUtility(
            ProjectModel.RestoreAuditProperties? restoreAuditProperties,
            string projectFullPath,
            IEnumerable<RestoreTargetGraph> graphs,
            IReadOnlyList<IVulnerabilityInformationProvider> vulnerabilityInformationProviders,
            ILogger logger)
        {
            _restoreAuditProperties = restoreAuditProperties;
            _projectFullPath = projectFullPath;
            _targetGraphs = graphs;
            _vulnerabilityInfoProviders = vulnerabilityInformationProviders;
            _logger = logger;

            MinSeverity = ParseAuditLevel();
            AuditMode = ParseAuditMode();
        }

        public async Task CheckPackageVulnerabilitiesAsync(CancellationToken cancellationToken)
        {
            // Performance: Early exit if restore graph does not contain any packages.
            if (!HasPackages())
            {
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            GetVulnerabilityInfoResult? allVulnerabilityData = await GetAllVulnerabilityDataAsync(cancellationToken);
            stopwatch.Stop();
            DownloadDurationSeconds = stopwatch.Elapsed.TotalSeconds;

            if (allVulnerabilityData?.Exceptions is not null)
            {
                ReplayErrors(allVulnerabilityData.Exceptions);
            }

            // Performance: Early exit if there's no vulnerability data to check packages against.
            if (allVulnerabilityData is null || !AnyVulnerabilityDataFound(allVulnerabilityData.KnownVulnerabilities))
            {
                return;
            }

            if (allVulnerabilityData.KnownVulnerabilities is not null)
            {
                CheckPackageVulnerabilities(allVulnerabilityData.KnownVulnerabilities);
            }

            bool HasPackages()
            {
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
                logMessage.ProjectPath = _projectFullPath;
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
                        restoreLogMessage.ProjectPath = _projectFullPath;
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

        private async Task<GetVulnerabilityInfoResult?> GetAllVulnerabilityDataAsync(CancellationToken cancellationToken)
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

            List<Exception>? errors = null;
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>? knownVulnerabilities = null;
            foreach (var resultTask in results)
            {
                GetVulnerabilityInfoResult? result = await resultTask;
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
                    if (errors == null)
                    {
                        errors = new();
                    }

                    errors.AddRange(result.Exceptions.InnerExceptions);
                }
            }

            GetVulnerabilityInfoResult? final =
                knownVulnerabilities != null || errors != null
                ? new(knownVulnerabilities, errors != null ? new AggregateException(errors) : null)
                : null;
            return final;
        }

        private PackageVulnerabilitySeverity ParseAuditLevel()
        {
            string? auditLevel = _restoreAuditProperties?.AuditLevel?.Trim();

            if (auditLevel == null)
            {
                return PackageVulnerabilitySeverity.Low;
            }

            if (string.Equals("low", auditLevel, StringComparison.OrdinalIgnoreCase))
            {
                return PackageVulnerabilitySeverity.Low;
            }
            if (string.Equals("moderate", auditLevel, StringComparison.OrdinalIgnoreCase))
            {
                return PackageVulnerabilitySeverity.Moderate;
            }
            if (string.Equals("high", auditLevel, StringComparison.OrdinalIgnoreCase))
            {
                return PackageVulnerabilitySeverity.High;
            }
            if (string.Equals("critical", auditLevel, StringComparison.OrdinalIgnoreCase))
            {
                return PackageVulnerabilitySeverity.Critical;
            }

            string messageText = string.Format(Strings.Error_InvalidNuGetAuditLevelValue, auditLevel, "low, moderate, high, critical");
            RestoreLogMessage message = RestoreLogMessage.CreateError(NuGetLogCode.NU1014, messageText);
            message.ProjectPath = _projectFullPath;
            _logger.Log(message);
            return 0;
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
        public static bool ParseEnableValue(string? value, string projectFullPath, ILogger logger)
        {
            // Earlier versions allowed "enable" and "default" to opt-in
            if (string.IsNullOrEmpty(value)
                || string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "enable", StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "default", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (string.Equals(value, bool.FalseString, StringComparison.OrdinalIgnoreCase)
                || string.Equals(value, "disable", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string messageText = string.Format(Strings.Error_InvalidNuGetAuditValue, value, "true, false");
            RestoreLogMessage message = RestoreLogMessage.CreateError(NuGetLogCode.NU1014, messageText);
            message.ProjectPath = projectFullPath;
            logger.Log(message);
            return true;
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
    }
}

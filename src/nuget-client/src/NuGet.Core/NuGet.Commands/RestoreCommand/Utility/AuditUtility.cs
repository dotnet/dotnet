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
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.Protocol.Model;
using NuGet.Versioning;

namespace NuGet.Commands.Restore.Utility
{
    internal struct AuditUtility
    {
        private readonly ProjectModel.RestoreAuditProperties _restoreAuditProperties;
        private readonly string _projectFullPath;
        private readonly IEnumerable<RestoreTargetGraph> _targetGraphs;
        private readonly IReadOnlyList<IVulnerabilityInformationProvider> _vulnerabilityInfoProviders;
        private readonly ILogger _logger;

        public AuditUtility(
            ProjectModel.RestoreAuditProperties restoreAuditProperties,
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
        }

        public async Task CheckPackageVulnerabilitiesAsync(CancellationToken cancellationToken)
        {
            GetVulnerabilityInfoResult? allVulnerabilityData = await GetAllVulnerabilityDataAsync(cancellationToken);
            if (allVulnerabilityData == null) return;

            if (allVulnerabilityData.Exceptions != null)
            {
                ReplayErrors(allVulnerabilityData.Exceptions);
            }

            if (allVulnerabilityData.KnownVulnerabilities != null)
            {
                CheckPackageVulnerabilities(allVulnerabilityData.KnownVulnerabilities);
            }
        }

        private void ReplayErrors(AggregateException exceptions)
        {
            foreach (Exception exception in exceptions.InnerExceptions)
            {
                var messageText = string.Format(Strings.Error_VulnerabilityDataFetch, exception.Message);
                RestoreLogMessage logMessage = RestoreLogMessage.CreateError(NuGetLogCode.NU1900, messageText);
                _logger.Log(logMessage);
            }
        }

        private void CheckPackageVulnerabilities(IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities)
        {
            Dictionary<PackageIdentity, Dictionary<PackageVulnerabilityInfo, List<string>>>? packagesWithKnownVulnerabilities =
                FindPackagesWithKnownVulnerabilities(knownVulnerabilities);

            if (packagesWithKnownVulnerabilities == null) return;

            // no-op checks DGSpec hash, which means the order of everything must be deterministic.
            // .NET Framework and .NET Standard don't have Deconstructor methods for KeyValuePair
            foreach (var kvp1 in packagesWithKnownVulnerabilities.OrderBy(p => p.Key.Id))
            {
                PackageIdentity package = kvp1.Key;
                Dictionary<PackageVulnerabilityInfo, List<string>> vulnerabilities = kvp1.Value;
                foreach (var kvp2 in vulnerabilities.OrderBy(v => v.Key.Url.OriginalString))
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
        }

        private static List<PackageVulnerabilityInfo>? GetKnownVulnerabilities(
            string name,
            NuGetVersion version,
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>? knownVulnerabilities)
        {
            HashSet<PackageVulnerabilityInfo>? vulnerabilities = null;

            if (knownVulnerabilities == null) return null;

            foreach (var file in knownVulnerabilities)
            {
                if (file.TryGetValue(name, out var packageVulnerabilities))
                {
                    foreach (var vulnInfo in packageVulnerabilities)
                    {
                        if (vulnInfo.Versions.Satisfies(version))
                        {
                            if (vulnerabilities == null)
                            {
                                vulnerabilities = new();
                            }
                            vulnerabilities.Add(vulnInfo);
                        }
                    }
                }
            }

            return vulnerabilities != null ? vulnerabilities.ToList() : null;
        }

        private static (string severityLabel, NuGetLogCode code) GetSeverityLabelAndCode(int severity)
        {
            switch (severity)
            {
                case 1:
                    return (Strings.Vulnerability_Severity_1, NuGetLogCode.NU1901);
                case 2:
                    return (Strings.Vulnerability_Severity_2, NuGetLogCode.NU1902);
                case 3:
                    return (Strings.Vulnerability_Severity_3, NuGetLogCode.NU1903);
                case 4:
                    return (Strings.Vulnerability_Severity_4, NuGetLogCode.NU1904);
                default:
                    return (Strings.Vulnerability_Severity_unknown, NuGetLogCode.NU1900);
            }
        }

        private Dictionary<PackageIdentity, Dictionary<PackageVulnerabilityInfo, List<string>>>? FindPackagesWithKnownVulnerabilities(
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities)
        {
            // multi-targeting projects often use the same package across multiple TFMs, so group to reduce output spam.
            Dictionary<PackageIdentity, Dictionary<PackageVulnerabilityInfo, List<string>>>? result = null;

            int minSeverity = ParseAuditLevel();
            NuGetAuditMode auditMode = ParseAuditMode();

            foreach (RestoreTargetGraph graph in _targetGraphs)
            {
                foreach (ResolvedDependencyKey resolvedDependency in GetDependenciesToAudit(graph, auditMode))
                {
                    LibraryIdentity package = resolvedDependency.Child;
                    List<PackageVulnerabilityInfo>? knownVulerabilitiesForPackage = GetKnownVulnerabilities(package.Name, package.Version, knownVulnerabilities);

                    if (knownVulerabilitiesForPackage?.Count() > 0)
                    {
                        PackageIdentity packageIdentity = new(package.Name, package.Version);

                        foreach (PackageVulnerabilityInfo knownVulnerability in knownVulerabilitiesForPackage)
                        {
                            if (knownVulnerability.Severity < minSeverity)
                            {
                                continue;
                            }

                            if (result == null)
                            {
                                result = new();
                            }

                            if (!result.TryGetValue(packageIdentity, out Dictionary<PackageVulnerabilityInfo, List<string>>? knownPackageVulnerabilities))
                            {
                                knownPackageVulnerabilities = new();
                                result.Add(packageIdentity, knownPackageVulnerabilities);
                            }

                            if (!knownPackageVulnerabilities.TryGetValue(knownVulnerability, out List<string>? affectedGraphs))
                            {
                                affectedGraphs = new();
                                knownPackageVulnerabilities.Add(knownVulnerability, affectedGraphs);
                            }

                            // Multiple package sources might list the same known vulnerability, so de-dupe those too.
                            if (!affectedGraphs.Contains(graph.TargetGraphName))
                            {
                                affectedGraphs.Add(graph.TargetGraphName);
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

        private int ParseAuditLevel()
        {
            string? auditLevel = _restoreAuditProperties.AuditLevel?.Trim();

            if (auditLevel == null)
            {
                return 1;
            }

            if (string.Equals("low", auditLevel, StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }
            if (string.Equals("moderate", auditLevel, StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }
            if (string.Equals("high", auditLevel, StringComparison.OrdinalIgnoreCase))
            {
                return 3;
            }
            if (string.Equals("critical", auditLevel, StringComparison.OrdinalIgnoreCase))
            {
                return 4;
            }

            string messageText = string.Format(Strings.Error_InvalidNuGetAuditLevelValue, auditLevel, "low, moderate, high, critical");
            RestoreLogMessage message = RestoreLogMessage.CreateError(NuGetLogCode.NU1014, messageText);
            message.ProjectPath = _projectFullPath;
            _logger.Log(message);
            return 1;
        }

        private enum NuGetAuditMode { Unknown, Direct, All }

        private NuGetAuditMode ParseAuditMode()
        {
            string? auditMode = _restoreAuditProperties.AuditMode?.Trim();

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

        IEnumerable<ResolvedDependencyKey> GetDependenciesToAudit(RestoreTargetGraph graph, NuGetAuditMode auditMode)
        {
            if (auditMode == NuGetAuditMode.All)
            {
                return graph.ResolvedDependencies.Where(dep => dep.Child.Type == LibraryType.Package);
            }

            Debug.Assert(graph.Graphs.Count() <= 1);
            LibraryIdentity? thisProject = graph.Graphs.FirstOrDefault()?.Item.Key;

            if (thisProject == null)
            {
                return Enumerable.Empty<ResolvedDependencyKey>();
            }

            Debug.Assert(thisProject.Type == LibraryType.Project);
            return graph.ResolvedDependencies.Where(dep => dep.Parent == thisProject && dep.Child.Type == LibraryType.Package);
        }
    }
}

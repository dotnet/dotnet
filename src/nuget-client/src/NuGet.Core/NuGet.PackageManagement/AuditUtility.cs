// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Model;
using NuGet.Versioning;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    internal class AuditUtility
    {
        private readonly IEnumerable<PackageRestoreData> _packages;
        private readonly List<SourceRepository> _sourceRepositories;
        private readonly ILogger _logger;
        private readonly SourceCacheContext _sourceCacheContext;
        private readonly PackageVulnerabilitySeverity _minSeverity;

        public AuditUtility(
            PackageVulnerabilitySeverity minSeverity,
            IEnumerable<PackageRestoreData> packages,
            List<SourceRepository> sourceRepositories,
            SourceCacheContext sourceCacheContext,
            ILogger logger)
        {
            _minSeverity = minSeverity;
            _packages = packages;
            _sourceRepositories = sourceRepositories;
            _sourceCacheContext = sourceCacheContext;
            _logger = logger;
        }

        public async Task CheckPackageVulnerabilitiesAsync(CancellationToken cancellationToken)
        {
            GetVulnerabilityInfoResult? allVulnerabilityData = await GetAllVulnerabilityDataAsync(_sourceRepositories, _sourceCacheContext, _logger, cancellationToken);

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
                return;
            }

            Dictionary<PackageIdentity, PackageAuditInfo>? packagesWithKnownVulnerabilities =
                FindPackagesWithKnownVulnerabilities(allVulnerabilityData.KnownVulnerabilities!,
                                                    _packages,
                                                    _minSeverity);
            if (packagesWithKnownVulnerabilities is not null)
            {
                CreateWarningsForPackagesWithVulnerabilities(packagesWithKnownVulnerabilities, _logger);
            }

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

        internal static async Task<GetVulnerabilityInfoResult?> GetAllVulnerabilityDataAsync(List<SourceRepository> sourceRepositories, SourceCacheContext sourceCacheContext, ILogger logger, CancellationToken cancellationToken)
        {
            List<Task<GetVulnerabilityInfoResult?>>? results = new(sourceRepositories.Count);

            foreach (SourceRepository source in sourceRepositories)
            {
                Task<GetVulnerabilityInfoResult?> getVulnerabilityInfoResult = GetVulnerabilityInfoAsync(source, sourceCacheContext, logger);
                if (getVulnerabilityInfoResult != null)
                {
                    results.Add(getVulnerabilityInfoResult);
                }
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

        internal static void CreateWarningsForPackagesWithVulnerabilities(Dictionary<PackageIdentity, PackageAuditInfo> packagesWithKnownVulnerabilities, ILogger logger)
        {
            foreach ((PackageIdentity package, PackageAuditInfo auditInfo) in packagesWithKnownVulnerabilities.OrderBy(p => p.Key.Id))
            {
                foreach (PackageVulnerabilityInfo vulnerability in auditInfo.Vulnerabilities)
                {
                    (var severityLabel, NuGetLogCode logCode) = GetSeverityLabelAndCode(vulnerability.Severity);
                    var message = string.Format(Strings.Warning_PackageWithKnownVulnerability,
                        package.Id,
                        package.Version.ToNormalizedString(),
                        severityLabel,
                        vulnerability.Url);
                    var restoreLogMessage =
                        RestoreLogMessage.CreateWarning(logCode,
                        message,
                        package.Id);
                    logger.Log(restoreLogMessage);
                }
            }
        }

        internal static Dictionary<PackageIdentity, PackageAuditInfo>? FindPackagesWithKnownVulnerabilities(
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities,
            IEnumerable<PackageRestoreData> packages, PackageVulnerabilitySeverity minSeverity)
        {
            Dictionary<PackageIdentity, PackageAuditInfo>? result = null;

            foreach (PackageRestoreData packageRestoreData in packages)
            {
                PackageIdentity packageIdentity = packageRestoreData.PackageReference.PackageIdentity;
                List<PackageVulnerabilityInfo>? knownVulnerabilitiesForPackage = GetKnownVulnerabilities(packageIdentity.Id, packageIdentity.Version, knownVulnerabilities);

                if (knownVulnerabilitiesForPackage?.Count > 0)
                {
                    foreach (PackageVulnerabilityInfo knownVulnerability in knownVulnerabilitiesForPackage)
                    {
                        if ((int)knownVulnerability.Severity < (int)minSeverity && knownVulnerability.Severity != PackageVulnerabilitySeverity.Unknown)
                        {
                            continue;
                        }

                        result ??= new();

                        if (!result.TryGetValue(packageIdentity, out PackageAuditInfo? auditInfo))
                        {
                            auditInfo = new(packageIdentity);
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

        internal class PackageAuditInfo
        {
            public PackageIdentity Identity { get; }
            public List<PackageVulnerabilityInfo> Vulnerabilities { get; }

            public PackageAuditInfo(PackageIdentity identity)
            {
                Identity = identity;
                Vulnerabilities = new();
            }
        }
    }
}

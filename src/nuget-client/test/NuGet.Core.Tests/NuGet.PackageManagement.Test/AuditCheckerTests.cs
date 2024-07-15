// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Model;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;

namespace NuGet.PackageManagement.Test
{
    public class AuditCheckerTests
    {
        [Fact]
        public void GetKnownVulnerability_WithPackageNotVulnerable_ReturnsNull()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities =
                new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>
                {
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "anotherPackageId",
                        new List<PackageVulnerabilityInfo> {
                            new PackageVulnerabilityInfo(
                                new Uri("https://contoso.com/random-vulnerability1"),
                                PackageVulnerabilitySeverity.Low,
                                VersionRange.Parse("[1.0.0, 2.0.0)")) }
                        }
                    }
                };

            AuditChecker.GetKnownVulnerabilities("packageId", new NuGetVersion(1, 0, 0), knownVulnerabilities).Should().BeNull();
        }

        [Fact]
        public void GetKnownVulnerability_WithPackageIdVulnerableButPackageVersionNotInRange_ReturnsNull()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities =
                new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>
                {
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "packageId",
                        new List<PackageVulnerabilityInfo> {
                            new PackageVulnerabilityInfo(
                                new Uri("https://contoso.com/random-vulnerability1"),
                                PackageVulnerabilitySeverity.Low,
                                VersionRange.Parse("[1.0.0, 2.0.0)")) }
                        }
                    }
                };

            AuditChecker.GetKnownVulnerabilities("packageId", new NuGetVersion(2, 0, 0), knownVulnerabilities).Should().BeNull();
        }

        [Fact]
        public void GetKnownVulnerability_WithPackageVulnerable_ReturnsOnlyAppropriateVulnerabilities()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities =
                new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>
                {
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "packageId",
                        new List<PackageVulnerabilityInfo> {
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Low,
                                    VersionRange.Parse("[1.0.0, 2.0.0)")),
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability2"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[2.0.0, 2.0.0]")),
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability3"),
                                    PackageVulnerabilitySeverity.High,
                                    VersionRange.Parse("[2.0.0, 3.0.0)"))
                            }
                        }
                    }
                };

            List<PackageVulnerabilityInfo>? vulnerabilities = AuditChecker.GetKnownVulnerabilities("packageId", new NuGetVersion(2, 0, 0), knownVulnerabilities);

            vulnerabilities!.Should().HaveCount(2);
            var moderateVulnerability = vulnerabilities![0];
            moderateVulnerability.Severity.Should().Be(PackageVulnerabilitySeverity.Moderate);
            moderateVulnerability.Url.Should().Be(new Uri("https://contoso.com/random-vulnerability2"));
            moderateVulnerability.Versions.Should().Be(VersionRange.Parse("[2.0.0, 2.0.0]"));

            var highVulnerability = vulnerabilities[1];
            highVulnerability.Severity.Should().Be(PackageVulnerabilitySeverity.High);
            highVulnerability.Url.Should().Be(new Uri("https://contoso.com/random-vulnerability3"));
            highVulnerability.Versions.Should().Be(VersionRange.Parse("[2.0.0, 3.0.0)"));
        }

        [Fact]
        public void GetKnownVulnerability_WithPackageVulnerableFromMultipleSources_ReturnsAppropriateVulnerabilities()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities =
                new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>
                {
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "packageId",
                        new List<PackageVulnerabilityInfo> {
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Low,
                                    VersionRange.Parse("[1.0.0, 2.0.0)")),
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability2"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[2.0.0, 2.0.0]")),
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability3"),
                                    PackageVulnerabilitySeverity.High,
                                    VersionRange.Parse("[3.0.0, 4.0.0)"))
                            }
                        }
                    },
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "packageId",
                        new List<PackageVulnerabilityInfo> {
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability4"),
                                    PackageVulnerabilitySeverity.Critical,
                                    VersionRange.Parse("[2.0.0, 2.5.0)"))
                            }
                        }
                    }
                };

            List<PackageVulnerabilityInfo>? vulnerabilities = AuditChecker.GetKnownVulnerabilities("packageId", new NuGetVersion(2, 0, 0), knownVulnerabilities);

            vulnerabilities!.Should().HaveCount(2);
            var moderateVulnerability = vulnerabilities![0];
            moderateVulnerability.Severity.Should().Be(PackageVulnerabilitySeverity.Moderate);
            moderateVulnerability.Url.Should().Be(new Uri("https://contoso.com/random-vulnerability2"));
            moderateVulnerability.Versions.Should().Be(VersionRange.Parse("[2.0.0, 2.0.0]"));

            var highVulnerability = vulnerabilities[1];
            highVulnerability.Severity.Should().Be(PackageVulnerabilitySeverity.Critical);
            highVulnerability.Url.Should().Be(new Uri("https://contoso.com/random-vulnerability4"));
            highVulnerability.Versions.Should().Be(VersionRange.Parse("[2.0.0, 2.5.0)"));
        }

        [Fact]
        public void GetKnownVulnerability_WithPackageVulnerableFromMultipleSources_ReturnsDedupedVulnerabilities()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities =
                new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>
                {
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "packageId",
                        new List<PackageVulnerabilityInfo> {
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Low,
                                    VersionRange.Parse("[1.0.0, 2.0.0)")),
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability2"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[2.0.0, 2.0.0]")),
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability3"),
                                    PackageVulnerabilitySeverity.High,
                                    VersionRange.Parse("[3.0.0, 4.0.0)"))
                            }
                        }
                    },
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "packageId",
                        new List<PackageVulnerabilityInfo> {
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability2"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[2.0.0, 2.0.0]"))
                            }
                        }
                    }
                };

            List<PackageVulnerabilityInfo>? vulnerabilities = AuditChecker.GetKnownVulnerabilities("packageId", new NuGetVersion(2, 0, 0), knownVulnerabilities);

            vulnerabilities.Should().HaveCount(1);
            var moderateVulnerability = vulnerabilities![0];
            moderateVulnerability.Severity.Should().Be(PackageVulnerabilitySeverity.Moderate);
            moderateVulnerability.Url.Should().Be(new Uri("https://contoso.com/random-vulnerability2"));
            moderateVulnerability.Versions.Should().Be(VersionRange.Parse("[2.0.0, 2.0.0]"));
        }

        [Fact]
        public void FindPackagesWithKnownVulnerabilities_WithPackageIdWithVulnerabilitiesButVersionNotVulnerable_ReturnsNull()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities =
                new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>
                {
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "packageId",
                        new List<PackageVulnerabilityInfo> {
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Low,
                                    VersionRange.Parse("[1.0.0, 2.0.0)"))
                            }
                        }
                    }
                };

            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            var packages = new List<PackageRestoreData>
            {
                new PackageRestoreData(new PackageReference(packageIdentity, CommonFrameworks.Net472), new string[]{ }, isMissing: true)
            };

            AuditChecker.FindPackagesWithKnownVulnerabilities(knownVulnerabilities, packages).Should().BeNull();
        }

        [Fact]
        public void FindPackagesWithKnownVulnerabilities_WithVulnerablePackage_ReturnsAppropriatePackages()
        {
            var projectPath = "C:\\solution\\project\\project.csproj";
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities =
                new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>
                {
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "packageId",
                        new List<PackageVulnerabilityInfo> {
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Low,
                                    VersionRange.Parse("[1.0.0, 2.0.0)")),
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability2"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[2.0.0, 2.0.0]")),
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability3"),
                                    PackageVulnerabilitySeverity.High,
                                    VersionRange.Parse("[3.0.0, 4.0.0)"))
                            }
                        }
                    },
                    new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>()
                    {
                        { "packageId",
                        new List<PackageVulnerabilityInfo> {
                                new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability4"),
                                    PackageVulnerabilitySeverity.High,
                                    VersionRange.Parse("[2.0.0, 3.0.0)"))
                            }
                        }
                    }
                };

            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            var packages = new List<PackageRestoreData>
            {
                new PackageRestoreData(new PackageReference(packageIdentity, CommonFrameworks.Net472), new string[]{ projectPath }, isMissing: true)
            };

            var packagesWithVulnerabilities = AuditChecker.FindPackagesWithKnownVulnerabilities(knownVulnerabilities, packages);
            packagesWithVulnerabilities.Should().HaveCount(1);
            (PackageIdentity vulnerablePackage, AuditChecker.PackageAuditInfo auditInfo) = packagesWithVulnerabilities!.Single();
            vulnerablePackage.Should().Be(packageIdentity);
            auditInfo.Identity.Should().Be(packageIdentity);
            auditInfo.Vulnerabilities.Should().HaveCount(2);
            auditInfo.Projects.Should().HaveCount(1);
            auditInfo.Projects.Should().Contain(projectPath);

            var moderateVulnerability = auditInfo.Vulnerabilities[0];
            moderateVulnerability.Severity.Should().Be(PackageVulnerabilitySeverity.Moderate);
            moderateVulnerability.Url.Should().Be(new Uri("https://contoso.com/random-vulnerability2"));
            moderateVulnerability.Versions.Should().Be(VersionRange.Parse("[2.0.0, 2.0.0]"));

            var highVulnerability = auditInfo.Vulnerabilities[1];
            highVulnerability.Severity.Should().Be(PackageVulnerabilitySeverity.High);
            highVulnerability.Url.Should().Be(new Uri("https://contoso.com/random-vulnerability4"));
            highVulnerability.Versions.Should().Be(VersionRange.Parse("[2.0.0, 3.0.0)"));
        }

        [Theory]
        [InlineData(PackageVulnerabilitySeverity.Low, "low", NuGetLogCode.NU1901)]
        [InlineData(PackageVulnerabilitySeverity.Moderate, "moderate", NuGetLogCode.NU1902)]
        [InlineData(PackageVulnerabilitySeverity.High, "high", NuGetLogCode.NU1903)]
        [InlineData(PackageVulnerabilitySeverity.Critical, "critical", NuGetLogCode.NU1904)]
        [InlineData(PackageVulnerabilitySeverity.Unknown, "unknown", NuGetLogCode.NU1900)]
        public void GetSeverityLabelAndCode_ReturnCorrectLabelAndCode(PackageVulnerabilitySeverity severity, string expectedLabel, NuGetLogCode expectedCode)
        {
            AuditChecker.GetSeverityLabelAndCode(severity).Should().Be((expectedLabel, expectedCode));
        }

        internal class VulnerabilityInfoResourceImplementation : IVulnerabilityInfoResource
        {
            internal GetVulnerabilityInfoResult Result { get; }
            internal SourceRepository SourceRepository { get; }
            public VulnerabilityInfoResourceImplementation(SourceRepository sourceRepository, GetVulnerabilityInfoResult result)
            {
                SourceRepository = sourceRepository;
                Result = result;
            }
            public Task<GetVulnerabilityInfoResult> GetVulnerabilityInfoAsync(SourceCacheContext cacheContext, ILogger logger, CancellationToken cancellationToken)
            {
                return Task.FromResult(Result);
            }
        }

        internal class VulnerabilityInfoResourceProvider : ResourceProvider
        {
            private readonly Dictionary<string, GetVulnerabilityInfoResult> _vulnerabilityInfoResults;

            public VulnerabilityInfoResourceProvider(Dictionary<string, GetVulnerabilityInfoResult> vulnerabilityInfoResults)
                : base(typeof(IVulnerabilityInfoResource), nameof(VulnerabilityInfoResourceProvider))
            {
                _vulnerabilityInfoResults = vulnerabilityInfoResults ?? throw new ArgumentNullException(nameof(vulnerabilityInfoResults));
            }

            public override Task<Tuple<bool, INuGetResource?>> TryCreate(SourceRepository source, CancellationToken token)
            {
                if (_vulnerabilityInfoResults.TryGetValue(source.PackageSource.Source, out GetVulnerabilityInfoResult? value))
                {
                    var resource = new VulnerabilityInfoResourceImplementation(source, value);
                    var result = new Tuple<bool, INuGetResource?>(true, resource);
                    return Task.FromResult(result);
                }
                return Task.FromResult(new Tuple<bool, INuGetResource?>(false, null));
            }
        }

        [Fact]
        public async Task GetAllVulnerabilityDataAsync_WithNoSourcesProvidingVulnerabilities_ReturnsNull()
        {
            Dictionary<string, GetVulnerabilityInfoResult> vulnerabilityResults = new();

            var sourceRepositories = new List<SourceRepository>
            {
                new SourceRepository(new PackageSource("https://contoso.com/v3/index.json"), new List<INuGetResourceProvider>{new VulnerabilityInfoResourceProvider(vulnerabilityResults) })
            };
            (int count, GetVulnerabilityInfoResult? vulnerabilityData) = await AuditChecker.GetAllVulnerabilityDataAsync(sourceRepositories, auditSources: null, Mock.Of<SourceCacheContext>(), NullLogger.Instance, CancellationToken.None);
            vulnerabilityData.Should().BeNull();
            count.Should().Be(0);
        }

        [Fact]
        public async Task GetAllVulnerabilityDataAsync_WithMultipleSources_AndSingleVulnerabilityInfo_ProvidesAllData()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities = new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>()
            {
                new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>
                {
                    {
                        "A",
                        new PackageVulnerabilityInfo[] {
                            new PackageVulnerabilityInfo(new Uri("https://vulnerability1"), PackageVulnerabilitySeverity.Low, VersionRange.Parse("[1.0.0,2.0.0)"))
                        }
                    }
                }
            };

            string sourceWithVulnerabilityData = "https://contoso.com/vulnerability/v3/index.json";
            Dictionary<string, GetVulnerabilityInfoResult> vulnerabilityResults = new()
            {
                { sourceWithVulnerabilityData, new GetVulnerabilityInfoResult(knownVulnerabilities, exceptions: null) }
            };

            var providers = new List<INuGetResourceProvider> { new VulnerabilityInfoResourceProvider(vulnerabilityResults) };
            var sourceRepositories = new List<SourceRepository>
            {
                new SourceRepository(new PackageSource("https://contoso.com/v3/index.json"), providers),
                new SourceRepository(new PackageSource(sourceWithVulnerabilityData), providers)
            };

            (int count, GetVulnerabilityInfoResult? vulnerabilityData) = await AuditChecker.GetAllVulnerabilityDataAsync(sourceRepositories, auditSources: null, Mock.Of<SourceCacheContext>(), NullLogger.Instance, CancellationToken.None);
            vulnerabilityData.Should().NotBeNull();
            vulnerabilityData!.Exceptions.Should().BeNull();
            vulnerabilityData.KnownVulnerabilities.Should().HaveCount(1);
            vulnerabilityData.KnownVulnerabilities!.Single().Keys.Should().Contain("A");
            vulnerabilityData.KnownVulnerabilities!.Single().Values.Single().Should().HaveCount(1);
            count.Should().Be(1);
        }

        [Fact]
        public async Task GetAllVulnerabilityDataAsync_WithMultipleSources_MergesDataAndExceptions()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities = new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>()
            {
                new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>
                {
                    {
                        "A",
                        new PackageVulnerabilityInfo[] {
                            new PackageVulnerabilityInfo(new Uri("https://vulnerability1"), PackageVulnerabilitySeverity.Low, VersionRange.Parse("[1.0.0,2.0.0)"))
                        }
                    }
                }
            };

            string sourceWithVulnerabilityData = "https://contoso.com/vulnerability/v3/index.json";
            string sourceWithBadVulnerabilityData = "https://contoso.com/vulnerability/broken/v3/index.json";
            string failureMessage = "Failed getting vulnerability data";
            Dictionary<string, GetVulnerabilityInfoResult> vulnerabilityResults = new()
            {
                { sourceWithVulnerabilityData, new GetVulnerabilityInfoResult(knownVulnerabilities, exceptions: null) },
                { sourceWithBadVulnerabilityData, new GetVulnerabilityInfoResult(null, exceptions: new AggregateException(new Exception(failureMessage))) }
            };

            var providers = new List<INuGetResourceProvider> { new VulnerabilityInfoResourceProvider(vulnerabilityResults) };
            var sourceRepositories = new List<SourceRepository>
            {
                new SourceRepository(new PackageSource(sourceWithBadVulnerabilityData), providers),
                new SourceRepository(new PackageSource(sourceWithVulnerabilityData), providers)
            };

            (int count, GetVulnerabilityInfoResult? vulnerabilityData) = await AuditChecker.GetAllVulnerabilityDataAsync(sourceRepositories, auditSources: null, Mock.Of<SourceCacheContext>(), NullLogger.Instance, CancellationToken.None);
            count.Should().Be(1);
            vulnerabilityData.Should().NotBeNull();
            vulnerabilityData!.KnownVulnerabilities.Should().HaveCount(1);
            vulnerabilityData.KnownVulnerabilities!.Single().Keys.Should().Contain("A");
            vulnerabilityData.KnownVulnerabilities!.Single().Values.Single().Should().HaveCount(1);
            vulnerabilityData.Exceptions.Should().NotBeNull();
            vulnerabilityData.Exceptions!.InnerException.Should().NotBeNull();
            vulnerabilityData.Exceptions.InnerException!.Message.Should().Be(failureMessage);
        }

        [Fact]
        public async Task GetAllVulnerabilityDataAsync_WithMultipleSources_MergesDataInOneResult()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> firstKnownVulnerabilities = new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>()
            {
                new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>
                {
                    {
                        "A",
                        new PackageVulnerabilityInfo[] {
                            new PackageVulnerabilityInfo(new Uri("https://vulnerability1"), PackageVulnerabilitySeverity.Low, VersionRange.Parse("[1.0.0,2.0.0)"))
                        }
                    }
                }
            };
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> secondKnownVulnerabilities = new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>()
            {
                new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>
                {
                    {
                        "B",
                        new PackageVulnerabilityInfo[] {
                            new PackageVulnerabilityInfo(new Uri("https://vulnerability2"), PackageVulnerabilitySeverity.Low, VersionRange.Parse("[2.0.0,3.0.0)"))
                        }
                    }
                }
            };

            string sourceWithVulnerabilityData = "https://contoso.com/vulnerability/v3/index.json";
            string secondarySourceWithVulnerability = "https://contoso.com/vulnerability/secondary/v3/index.json";
            Dictionary<string, GetVulnerabilityInfoResult> vulnerabilityResults = new()
            {
                { sourceWithVulnerabilityData, new GetVulnerabilityInfoResult(firstKnownVulnerabilities, exceptions: null) },
                { secondarySourceWithVulnerability, new GetVulnerabilityInfoResult(secondKnownVulnerabilities, exceptions: null) }
            };

            var providers = new List<INuGetResourceProvider> { new VulnerabilityInfoResourceProvider(vulnerabilityResults) };
            var sourceRepositories = new List<SourceRepository>
            {
                new SourceRepository(new PackageSource(secondarySourceWithVulnerability), providers),
                new SourceRepository(new PackageSource(sourceWithVulnerabilityData), providers)
            };

            (int count, GetVulnerabilityInfoResult? vulnerabilityData) = await AuditChecker.GetAllVulnerabilityDataAsync(sourceRepositories, auditSources: null, Mock.Of<SourceCacheContext>(), NullLogger.Instance, CancellationToken.None);
            vulnerabilityData.Should().NotBeNull();
            vulnerabilityData!.KnownVulnerabilities.Should().HaveCount(2);
            vulnerabilityData.KnownVulnerabilities!.First().Keys.Should().Contain("B");
            vulnerabilityData.KnownVulnerabilities!.First().Values.Single().Should().HaveCount(1);
            vulnerabilityData.KnownVulnerabilities!.Last().Keys.Should().Contain("A");
            vulnerabilityData.KnownVulnerabilities!.Last().Values.Single().Should().HaveCount(1);
            count.Should().Be(2);
        }

        [Fact]
        public async Task GetAllVulnerabilityDataAsync_SourceWithInvalidHost_ReturnResultWithException()
        {
            // Arrange
            // .test is a reserved TLD, so we know it will never exist
            var packageSource = new PackageSource("https://nuget.test/v3/index.json");
            SourceRepository source = Repository.Factory.GetCoreV3(packageSource);
            List<SourceRepository> sourceRepositories = new List<SourceRepository>() { source };
            using SourceCacheContext cacheContext = new();

            // Act
            (int count, GetVulnerabilityInfoResult? result) = await AuditChecker.GetAllVulnerabilityDataAsync(sourceRepositories, auditSources: null, cacheContext, NullLogger.Instance, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.KnownVulnerabilities.Should().BeNull();
            result.Exceptions.Should().NotBeNull();
            count.Should().Be(0);
        }

        [Fact]
        public async Task GetAllVulnerabilityDataAsync_PackageAndAuditSource_ReturnsAuditSourceVulnerabilitiesOnly()
        {
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> firstKnownVulnerabilities = new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>()
            {
                new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>
                {
                    {
                        "A",
                        new PackageVulnerabilityInfo[] {
                            new PackageVulnerabilityInfo(new Uri("https://vulnerability1"), PackageVulnerabilitySeverity.Low, VersionRange.Parse("[1.0.0,2.0.0)"))
                        }
                    }
                }
            };
            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> secondKnownVulnerabilities = new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>()
            {
                new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>
                {
                    {
                        "B",
                        new PackageVulnerabilityInfo[] {
                            new PackageVulnerabilityInfo(new Uri("https://vulnerability2"), PackageVulnerabilitySeverity.Low, VersionRange.Parse("[2.0.0,3.0.0)"))
                        }
                    }
                }
            };

            string packageSourceUrl = "https://contoso.test/vulnerability/v3/index.json";
            string auditSourceUrl = "https://contoso.test/vulnerability/secondary/v3/index.json";
            Dictionary<string, GetVulnerabilityInfoResult> vulnerabilityResults = new()
            {
                { packageSourceUrl, new GetVulnerabilityInfoResult(firstKnownVulnerabilities, exceptions: null) },
                { auditSourceUrl, new GetVulnerabilityInfoResult(secondKnownVulnerabilities, exceptions: null) }
            };

            var providers = new List<INuGetResourceProvider> { new VulnerabilityInfoResourceProvider(vulnerabilityResults) };
            var packageSources = new List<SourceRepository>
            {
                new SourceRepository(new PackageSource(packageSourceUrl), providers),
            };
            var auditSources = new List<SourceRepository>
            {
                new SourceRepository(new PackageSource(auditSourceUrl), providers)
            };

            (int count, GetVulnerabilityInfoResult? vulnerabilityData) = await AuditChecker.GetAllVulnerabilityDataAsync(packageSources, auditSources, Mock.Of<SourceCacheContext>(), NullLogger.Instance, CancellationToken.None);
            vulnerabilityData.Should().NotBeNull();
            vulnerabilityData!.KnownVulnerabilities.Should().HaveCount(1);
            vulnerabilityData.KnownVulnerabilities![0].Keys.Should().HaveCount(1);
            vulnerabilityData.KnownVulnerabilities[0].Keys.First().Should().Be("B");
        }

        [Fact]
        public async Task GetAllVulnerabilityDataAsync_AuditSourceWithoutVulnerabilityDB_RaisesWarning()
        {
            // Arrange
            string auditSourceUrl = "https://contoso.test/nuget/v3/index.json";
            Dictionary<string, GetVulnerabilityInfoResult> vulnerabilityResults = new();

            var providers = new List<INuGetResourceProvider> { new VulnerabilityInfoResourceProvider(vulnerabilityResults) };
            var packageSources = new List<SourceRepository>();
            var auditSources = new List<SourceRepository>
            {
                new SourceRepository(new PackageSource(auditSourceUrl), providers)
            };

            var logger = new TestLogger();

            // Act
            (int count, GetVulnerabilityInfoResult? vulnerabilityData) = await AuditChecker.GetAllVulnerabilityDataAsync(packageSources, auditSources, Mock.Of<SourceCacheContext>(), logger, CancellationToken.None);

            // Assert
            logger.LogMessages.Should().ContainSingle();
            ILogMessage logMessage = logger.LogMessages.Single();
            logMessage.Code.Should().Be(NuGetLogCode.NU1905);
            logMessage.Message.Should().Contain(auditSourceUrl);
        }

        [Fact]
        public void CreateWarnings_WithoutAuditSettings_WithoutProjectsInformation_ReturnsNull()
        {
            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            AuditChecker.PackageAuditInfo packageAuditInfo = new(packageIdentity, Array.Empty<string>());

            packageAuditInfo.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));

            Dictionary<PackageIdentity, AuditChecker.PackageAuditInfo> result = new()
            {
                {packageIdentity, packageAuditInfo }
            };

            var auditSettings = new Dictionary<string, AuditChecker.ProjectAuditSettings>();
            int Sev0Matches = 0, Sev1Matches = 0, Sev2Matches = 0, Sev3Matches = 0, InvalidSevMatches = 0, TotalWarningsSuppressedCount = 0, DistinctAdvisoriesSuppressedCount = 0;
            List<PackageIdentity> packagesWithReportedAdvisories = new List<PackageIdentity>();

            List<LogMessage> warnings = AuditChecker.CreateWarnings(result!,
                       auditSettings,
                       ref Sev0Matches,
                       ref Sev1Matches,
                       ref Sev2Matches,
                       ref Sev3Matches,
                       ref InvalidSevMatches,
                       ref TotalWarningsSuppressedCount,
                       ref DistinctAdvisoriesSuppressedCount,
                       ref packagesWithReportedAdvisories);

            warnings.Should().BeEmpty();
            Sev0Matches.Should().Be(0);
            Sev1Matches.Should().Be(0);
            Sev2Matches.Should().Be(0);
            Sev3Matches.Should().Be(0);
            InvalidSevMatches.Should().Be(0);
            packagesWithReportedAdvisories.Should().BeEmpty();
        }

        [Fact]
        public void CreateWarnings_WithoutAuditSettings_RaisesWarningsForAllProjects()
        {
            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            var projectPath1 = "C:\\solution\\project\\project1.csproj";
            var projectPath2 = "C:\\solution\\project\\project2.csproj";
            AuditChecker.PackageAuditInfo packageAuditInfo = new(packageIdentity, new string[] { projectPath1, projectPath2 });

            packageAuditInfo.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));

            Dictionary<PackageIdentity, AuditChecker.PackageAuditInfo> result = new()
            {
                {packageIdentity, packageAuditInfo }
            };

            var auditSettings = new Dictionary<string, AuditChecker.ProjectAuditSettings>();
            int Sev0Matches = 0, Sev1Matches = 0, Sev2Matches = 0, Sev3Matches = 0, InvalidSevMatches = 0, TotalWarningsSuppressedCount = 0, DistinctAdvisoriesSuppressedCount = 0;
            List<PackageIdentity> packagesWithReportedAdvisories = new List<PackageIdentity>();

            List<LogMessage> warnings = AuditChecker.CreateWarnings(result!,
                       auditSettings,
                       ref Sev0Matches,
                       ref Sev1Matches,
                       ref Sev2Matches,
                       ref Sev3Matches,
                       ref InvalidSevMatches,
                       ref TotalWarningsSuppressedCount,
                       ref DistinctAdvisoriesSuppressedCount,
                       ref packagesWithReportedAdvisories);

            warnings.Should().HaveCount(2);
            warnings[0].Code.Should().Be(NuGetLogCode.NU1902);
            warnings[0].Message.Should().Be(string.Format(Strings.Warning_PackageWithKnownVulnerability,
                               packageIdentity.Id,
                               packageIdentity.Version.ToNormalizedString(),
                               PackageVulnerabilitySeverity.Moderate.ToString().ToLowerInvariant(),
                               packageAuditInfo.Vulnerabilities[0].Url));
            warnings[0].ProjectPath.Should().Be(projectPath1);

            warnings[1].Code.Should().Be(NuGetLogCode.NU1902);
            warnings[1].Message.Should().Be(string.Format(Strings.Warning_PackageWithKnownVulnerability,
                               packageIdentity.Id,
                               packageIdentity.Version.ToNormalizedString(),
                               PackageVulnerabilitySeverity.Moderate.ToString().ToLowerInvariant(),
                               packageAuditInfo.Vulnerabilities[0].Url));
            warnings[1].ProjectPath.Should().Be(projectPath2);

            Sev0Matches.Should().Be(0);
            Sev1Matches.Should().Be(1);
            Sev2Matches.Should().Be(0);
            Sev3Matches.Should().Be(0);
            InvalidSevMatches.Should().Be(0);
            packagesWithReportedAdvisories.Should().HaveCount(1);
            packagesWithReportedAdvisories[0].Should().Be(packageIdentity);
        }

        [Fact]
        public void CreateWarnings_WithoutAuditSettings_RaisesWarningsForAllSeverities()
        {
            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            var projectPath = "C:\\solution\\project\\project.csproj";
            AuditChecker.PackageAuditInfo packageAuditInfo = new(packageIdentity, new string[] { projectPath });

            packageAuditInfo.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));

            Dictionary<PackageIdentity, AuditChecker.PackageAuditInfo> result = new()
            {
                {packageIdentity, packageAuditInfo }
            };

            var auditSettings = new Dictionary<string, AuditChecker.ProjectAuditSettings>();
            int Sev0Matches = 0, Sev1Matches = 0, Sev2Matches = 0, Sev3Matches = 0, InvalidSevMatches = 0, TotalWarningsSuppressedCount = 0, DistinctAdvisoriesSuppressedCount = 0;
            List<PackageIdentity> packagesWithReportedAdvisories = new List<PackageIdentity>();

            List<LogMessage> warnings = AuditChecker.CreateWarnings(result!,
                       auditSettings,
                       ref Sev0Matches,
                       ref Sev1Matches,
                       ref Sev2Matches,
                       ref Sev3Matches,
                       ref InvalidSevMatches,
                       ref TotalWarningsSuppressedCount,
                       ref DistinctAdvisoriesSuppressedCount,
                       ref packagesWithReportedAdvisories);

            warnings.Should().HaveCount(1);
            warnings[0].Code.Should().Be(NuGetLogCode.NU1902);
            warnings[0].Message.Should().Be(string.Format(Strings.Warning_PackageWithKnownVulnerability,
                               packageIdentity.Id,
                               packageIdentity.Version.ToNormalizedString(),
                               PackageVulnerabilitySeverity.Moderate.ToString().ToLowerInvariant(),
                               packageAuditInfo.Vulnerabilities[0].Url));
            warnings[0].ProjectPath.Should().Be(projectPath);

            Sev0Matches.Should().Be(0);
            Sev1Matches.Should().Be(1);
            Sev2Matches.Should().Be(0);
            Sev3Matches.Should().Be(0);
            InvalidSevMatches.Should().Be(0);
            packagesWithReportedAdvisories.Should().HaveCount(1);
            packagesWithReportedAdvisories[0].Should().Be(packageIdentity);
        }

        [Fact]
        public void CreateWarnings_WithVariousVulnerabilties_CountsSeverityMatchesCorrectly()
        {
            PackageIdentity packageA = new("a", new NuGetVersion(2, 0, 0));
            PackageIdentity packageB = new("b", new NuGetVersion(1, 0, 0));
            PackageIdentity packageC = new("c", new NuGetVersion(2, 5, 0));
            var projectPath = "C:\\solution\\project\\project.csproj";

            AuditChecker.PackageAuditInfo packageAuditInfoA = new(packageA, new string[] { projectPath });
            packageAuditInfoA.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.High));
            packageAuditInfoA.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.Low));
            packageAuditInfoA.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.Low));
            packageAuditInfoA.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.Critical));

            AuditChecker.PackageAuditInfo packageAuditInfoB = new(packageB, new string[] { projectPath });
            packageAuditInfoB.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.High));
            packageAuditInfoB.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.Moderate));
            packageAuditInfoB.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.Moderate));

            AuditChecker.PackageAuditInfo packageAuditInfoC = new(packageC, new string[] { projectPath });
            packageAuditInfoC.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.Low));
            packageAuditInfoC.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.Moderate));
            packageAuditInfoC.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.Low));
            packageAuditInfoC.Vulnerabilities.Add(GetVulnerability(PackageVulnerabilitySeverity.Unknown));

            Dictionary<PackageIdentity, AuditChecker.PackageAuditInfo> result = new()
            {
                {packageA, packageAuditInfoA },
                {packageB, packageAuditInfoB },
                {packageC, packageAuditInfoC },
            };

            var auditSettings = new Dictionary<string, AuditChecker.ProjectAuditSettings>();
            int Sev0Matches = 0, Sev1Matches = 0, Sev2Matches = 0, Sev3Matches = 0, InvalidSevMatches = 0, TotalWarningsSuppressedCount = 0, DistinctAdvisoriesSuppressedCount = 0;
            List<PackageIdentity> packagesWithReportedAdvisories = new List<PackageIdentity>();

            List<LogMessage> warnings = AuditChecker.CreateWarnings(result!,
                       auditSettings,
                       ref Sev0Matches,
                       ref Sev1Matches,
                       ref Sev2Matches,
                       ref Sev3Matches,
                       ref InvalidSevMatches,
                       ref TotalWarningsSuppressedCount,
                       ref DistinctAdvisoriesSuppressedCount,
                       ref packagesWithReportedAdvisories);

            warnings.Should().HaveCount(11);

            Sev0Matches.Should().Be(4);
            Sev1Matches.Should().Be(3);
            Sev2Matches.Should().Be(2);
            Sev3Matches.Should().Be(1);
            InvalidSevMatches.Should().Be(1);
            packagesWithReportedAdvisories.Should().HaveCount(3);
            packagesWithReportedAdvisories[0].Should().Be(packageA);
            packagesWithReportedAdvisories[1].Should().Be(packageB);
            packagesWithReportedAdvisories[2].Should().Be(packageC);
            static PackageVulnerabilityInfo GetVulnerability(PackageVulnerabilitySeverity packageVulnerabilitySeverity)
            {
                return new PackageVulnerabilityInfo(
                                                    new Uri($"https://contoso.com/{Guid.NewGuid()}"),
                                                    packageVulnerabilitySeverity,
                                                    VersionRange.Parse("[1.0.0, 3.0.0)"));
            }
        }

        [Fact]
        public void CreateWarnings_WithAuditSettings_RaisesWarningsForEnabledProjectsOnly()
        {
            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            var projectPath1 = "C:\\solution\\project\\project1.csproj";
            var projectPath2 = "C:\\solution\\project\\project2.csproj";
            AuditChecker.PackageAuditInfo packageAuditInfo = new(packageIdentity, new string[] { projectPath1, projectPath2 });

            packageAuditInfo.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));

            Dictionary<PackageIdentity, AuditChecker.PackageAuditInfo> result = new()
            {
                {packageIdentity, packageAuditInfo }
            };

            var auditSettings = new Dictionary<string, AuditChecker.ProjectAuditSettings>()
            {
                { projectPath1 , new AuditChecker.ProjectAuditSettings(true, PackageVulnerabilitySeverity.Moderate, suppressedAdvisories: null) },
                { projectPath2 , new AuditChecker.ProjectAuditSettings(false, PackageVulnerabilitySeverity.Moderate, suppressedAdvisories: null) }
            };

            int Sev0Matches = 0, Sev1Matches = 0, Sev2Matches = 0, Sev3Matches = 0, InvalidSevMatches = 0, TotalWarningsSuppressedCount = 0, DistinctAdvisoriesSuppressedCount = 0;
            List<PackageIdentity> packagesWithReportedAdvisories = new List<PackageIdentity>();

            List<LogMessage> warnings = AuditChecker.CreateWarnings(result!,
                       auditSettings,
                       ref Sev0Matches,
                       ref Sev1Matches,
                       ref Sev2Matches,
                       ref Sev3Matches,
                       ref InvalidSevMatches,
                       ref TotalWarningsSuppressedCount,
                       ref DistinctAdvisoriesSuppressedCount,
                       ref packagesWithReportedAdvisories);

            warnings.Should().HaveCount(1);
            warnings[0].Code.Should().Be(NuGetLogCode.NU1902);
            warnings[0].Message.Should().Be(string.Format(Strings.Warning_PackageWithKnownVulnerability,
                               packageIdentity.Id,
                               packageIdentity.Version.ToNormalizedString(),
                               PackageVulnerabilitySeverity.Moderate.ToString().ToLowerInvariant(),
                               packageAuditInfo.Vulnerabilities[0].Url));
            warnings[0].ProjectPath.Should().Be(projectPath1);

            Sev0Matches.Should().Be(0);
            Sev1Matches.Should().Be(1);
            Sev2Matches.Should().Be(0);
            Sev3Matches.Should().Be(0);
            InvalidSevMatches.Should().Be(0);
            packagesWithReportedAdvisories.Should().HaveCount(1);
            packagesWithReportedAdvisories[0].Should().Be(packageIdentity);
        }

        [Fact]
        public void CreateWarnings_WithAuditSettings_RaisesWarningsForProjectsWithMatchingSeverityOnly()
        {
            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            var projectPath1 = "C:\\solution\\project\\project1.csproj";
            var projectPath2 = "C:\\solution\\project\\project2.csproj";
            AuditChecker.PackageAuditInfo packageAuditInfo = new(packageIdentity, new string[] { projectPath1, projectPath2 });

            packageAuditInfo.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri("https://contoso.com/random-vulnerability1"),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));

            Dictionary<PackageIdentity, AuditChecker.PackageAuditInfo> result = new()
            {
                {packageIdentity, packageAuditInfo }
            };

            var auditSettings = new Dictionary<string, AuditChecker.ProjectAuditSettings>()
            {
                { projectPath1 , new AuditChecker.ProjectAuditSettings(true, PackageVulnerabilitySeverity.Moderate, suppressedAdvisories: null) },
                { projectPath2 , new AuditChecker.ProjectAuditSettings(true, PackageVulnerabilitySeverity.High, suppressedAdvisories: null) }
            };

            int Sev0Matches = 0, Sev1Matches = 0, Sev2Matches = 0, Sev3Matches = 0, InvalidSevMatches = 0, TotalWarningsSuppressedCount = 0, DistinctAdvisoriesSuppressedCount = 0;
            List<PackageIdentity> packagesWithReportedAdvisories = new List<PackageIdentity>();

            List<LogMessage> warnings = AuditChecker.CreateWarnings(result!,
                       auditSettings,
                       ref Sev0Matches,
                       ref Sev1Matches,
                       ref Sev2Matches,
                       ref Sev3Matches,
                       ref InvalidSevMatches,
                       ref TotalWarningsSuppressedCount,
                       ref DistinctAdvisoriesSuppressedCount,
                       ref packagesWithReportedAdvisories);

            warnings.Should().HaveCount(1);
            warnings[0].Code.Should().Be(NuGetLogCode.NU1902);
            warnings[0].Message.Should().Be(string.Format(Strings.Warning_PackageWithKnownVulnerability,
                               packageIdentity.Id,
                               packageIdentity.Version.ToNormalizedString(),
                               PackageVulnerabilitySeverity.Moderate.ToString().ToLowerInvariant(),
                               packageAuditInfo.Vulnerabilities[0].Url));
            warnings[0].ProjectPath.Should().Be(projectPath1);

            Sev0Matches.Should().Be(0);
            Sev1Matches.Should().Be(1);
            Sev2Matches.Should().Be(0);
            Sev3Matches.Should().Be(0);
            InvalidSevMatches.Should().Be(0);
            TotalWarningsSuppressedCount.Should().Be(0);
            DistinctAdvisoriesSuppressedCount.Should().Be(0);
            packagesWithReportedAdvisories.Should().HaveCount(1);
            packagesWithReportedAdvisories[0].Should().Be(packageIdentity);
        }

        [Fact]
        public async Task CheckVulnerabiltiesAsync_WithoutEnabledProjects_SkipsVulnerabilityCheckingAltogether()
        {
            SetupPrerequisites(out AuditChecker auditChecker, out string projectPath, out List<PackageRestoreData> packages);

            var restoreAuditProperties = new Dictionary<string, RestoreAuditProperties>()
            {
                { projectPath, new RestoreAuditProperties()
                    {
                        EnableAudit = "false"
                    }
                }
            };

            AuditCheckResult result = await auditChecker.CheckPackageVulnerabilitiesAsync(packages, restoreAuditProperties, CancellationToken.None);

            result.Should().NotBeNull();
            result.Warnings.Should().BeEmpty();
            result.DownloadDurationInSeconds.Should().BeNull();
            result.CheckPackagesDurationInSeconds.Should().BeNull(); ;

            result.IsAuditEnabled.Should().BeFalse();
            result.SourcesWithVulnerabilities.Should().BeNull();
            result.Severity0VulnerabilitiesFound.Should().Be(0);
            result.Severity1VulnerabilitiesFound.Should().Be(0);
            result.Severity2VulnerabilitiesFound.Should().Be(0);
            result.Severity3VulnerabilitiesFound.Should().Be(0);
            result.InvalidSeverityVulnerabilitiesFound.Should().Be(0);
        }

        [Fact]
        public async Task CheckVulnerabiltiesAsync_WithSeverityLowerThanVulnerabilitiesReported_RaisesWarnings()
        {
            SetupPrerequisites(out AuditChecker auditChecker, out string projectPath, out List<PackageRestoreData> packages);

            var restoreAuditProperties = new Dictionary<string, RestoreAuditProperties>()
            {
                { projectPath, new RestoreAuditProperties()
                    {
                        EnableAudit = "true",
                        AuditLevel = "low",
                    }
                }
            };

            AuditCheckResult result = await auditChecker.CheckPackageVulnerabilitiesAsync(packages, restoreAuditProperties, CancellationToken.None);

            result.Should().NotBeNull();
            result.Warnings.Should().HaveCount(1);
            result.DownloadDurationInSeconds.Should().NotBeNull();
            result.CheckPackagesDurationInSeconds.Should().NotBeNull(); ;

            result.IsAuditEnabled.Should().BeTrue();
            result.SourcesWithVulnerabilities.Should().Be(1);
            result.Severity0VulnerabilitiesFound.Should().Be(0);
            result.Severity1VulnerabilitiesFound.Should().Be(1);
            result.Severity2VulnerabilitiesFound.Should().Be(0);
            result.Severity3VulnerabilitiesFound.Should().Be(0);
            result.InvalidSeverityVulnerabilitiesFound.Should().Be(0);
        }

        [Fact]
        public async Task CheckVulnerabiltiesAsync_WithSeverityHigherThanVulnerabilitiesReported_DoesNotRaiseWarnings()
        {
            SetupPrerequisites(out AuditChecker auditChecker, out string projectPath, out List<PackageRestoreData> packages);

            var restoreAuditProperties = new Dictionary<string, RestoreAuditProperties>()
            {
                { projectPath, new RestoreAuditProperties()
                    {
                        EnableAudit = "true",
                        AuditLevel = "high",
                    }
                }
            };

            AuditCheckResult result = await auditChecker.CheckPackageVulnerabilitiesAsync(packages, restoreAuditProperties, CancellationToken.None);

            result.Should().NotBeNull();
            result.Warnings.Should().HaveCount(0);
            result.DownloadDurationInSeconds.Should().NotBeNull();
            result.CheckPackagesDurationInSeconds.Should().NotBeNull(); ;

            result.IsAuditEnabled.Should().BeTrue();
            result.SourcesWithVulnerabilities.Should().Be(1);
            result.Severity0VulnerabilitiesFound.Should().Be(0);
            result.Severity1VulnerabilitiesFound.Should().Be(0);
            result.Severity2VulnerabilitiesFound.Should().Be(0);
            result.Severity3VulnerabilitiesFound.Should().Be(0);
            result.InvalidSeverityVulnerabilitiesFound.Should().Be(0);
        }

        [Fact]
        public void CreateWarnings_WithAuditSettings_WithSuppressedAdvisories_SuppressesExpectedVulnerabilities()
        {
            string cveUrl1 = "https://cve.test/suppressed/1";
            string cveUrl2 = "https://cve.test/suppressed/2";

            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            var projectPath = "C:\\solution\\project\\project1.csproj";
            AuditChecker.PackageAuditInfo packageAuditInfo = new(packageIdentity, new string[] { projectPath });

            packageAuditInfo.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri(cveUrl1),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));
            packageAuditInfo.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri(cveUrl2),
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));

            var suppressedAdvisories = new HashSet<string> { cveUrl1 }; // suppress one of the two advisories

            Dictionary<PackageIdentity, AuditChecker.PackageAuditInfo> result = new()
            {
                {packageIdentity, packageAuditInfo }
            };

            var auditSettings = new Dictionary<string, AuditChecker.ProjectAuditSettings>()
            {
                { projectPath , new AuditChecker.ProjectAuditSettings(true, PackageVulnerabilitySeverity.Moderate, suppressedAdvisories) }
            };

            int Sev0Matches = 0, Sev1Matches = 0, Sev2Matches = 0, Sev3Matches = 0, InvalidSevMatches = 0, TotalWarningsSuppressedCount = 0, DistinctAdvisoriesSuppressedCount = 0;
            List<PackageIdentity> packagesWithReportedAdvisories = new List<PackageIdentity>();

            List<LogMessage> warnings = AuditChecker.CreateWarnings(result!,
                       auditSettings,
                       ref Sev0Matches,
                       ref Sev1Matches,
                       ref Sev2Matches,
                       ref Sev3Matches,
                       ref InvalidSevMatches,
                       ref TotalWarningsSuppressedCount,
                       ref DistinctAdvisoriesSuppressedCount,
                       ref packagesWithReportedAdvisories);

            warnings.Should().HaveCount(1);
            warnings.Any(m => m.Message.Contains(cveUrl1)).Should().BeFalse(); // cveUrl1 should be suppressed
            warnings.Any(m => m.Message.Contains(cveUrl2)).Should().BeTrue(); // cveUrl2 should not be suppressed

            Sev0Matches.Should().Be(0);
            Sev1Matches.Should().Be(1);
            Sev2Matches.Should().Be(0);
            Sev3Matches.Should().Be(0);
            InvalidSevMatches.Should().Be(0);
            TotalWarningsSuppressedCount.Should().Be(1);
            DistinctAdvisoriesSuppressedCount.Should().Be(1);
            packagesWithReportedAdvisories.Should().HaveCount(1);
            packagesWithReportedAdvisories[0].Should().Be(packageIdentity);
        }

        [Fact]
        public void CreateWarnings_WithAuditSettings_WithSuppressedAdvisories_CountsSuppressedAdvisoriesCorrectly()
        {
            string cveUrl1 = "https://cve.test/suppressed/1";
            string cveUrl2 = "https://cve.test/suppressed/2";
            string cveUrl3 = "https://cve.test/suppressed/3";

            var projectPath = "C:\\solution\\project\\project1.csproj";

            PackageIdentity packageIdentityA = new("packageA", new NuGetVersion(2, 0, 0));
            AuditChecker.PackageAuditInfo packageAuditInfoA = new(packageIdentityA, new string[] { projectPath });
            packageAuditInfoA.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri(cveUrl1), // this will be suppressed
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));
            packageAuditInfoA.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri(cveUrl2), // this will be suppressed
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));

            PackageIdentity packageIdentityB = new("packageB", new NuGetVersion(2, 0, 0));
            AuditChecker.PackageAuditInfo packageAuditInfoB = new(packageIdentityB, new string[] { projectPath });
            packageAuditInfoB.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri(cveUrl1), // this will be suppressed
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));
            packageAuditInfoB.Vulnerabilities.Add(new PackageVulnerabilityInfo(
                                    new Uri(cveUrl3), // this will not be suppressed
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)")));

            var suppressedAdvisories = new HashSet<string> { cveUrl1, cveUrl2 };

            Dictionary<PackageIdentity, AuditChecker.PackageAuditInfo> result = new()
            {
                { packageIdentityA, packageAuditInfoA },
                { packageIdentityB, packageAuditInfoB }
            };

            var auditSettings = new Dictionary<string, AuditChecker.ProjectAuditSettings>()
            {
                { projectPath , new AuditChecker.ProjectAuditSettings(true, PackageVulnerabilitySeverity.Moderate, suppressedAdvisories) }
            };

            int Sev0Matches = 0, Sev1Matches = 0, Sev2Matches = 0, Sev3Matches = 0, InvalidSevMatches = 0, TotalWarningsSuppressedCount = 0, DistinctAdvisoriesSuppressedCount = 0;
            List<PackageIdentity> packagesWithReportedAdvisories = new List<PackageIdentity>();

            List<LogMessage> warnings = AuditChecker.CreateWarnings(result!,
                       auditSettings,
                       ref Sev0Matches,
                       ref Sev1Matches,
                       ref Sev2Matches,
                       ref Sev3Matches,
                       ref InvalidSevMatches,
                       ref TotalWarningsSuppressedCount,
                       ref DistinctAdvisoriesSuppressedCount,
                       ref packagesWithReportedAdvisories);

            warnings.Should().HaveCount(1);
            warnings.Any(m => m.Message.Contains(cveUrl1)).Should().BeFalse();
            warnings.Any(m => m.Message.Contains(cveUrl2)).Should().BeFalse();
            warnings.Any(m => m.Message.Contains(cveUrl3)).Should().BeTrue();

            Sev0Matches.Should().Be(0);
            Sev1Matches.Should().Be(1);
            Sev2Matches.Should().Be(0);
            Sev3Matches.Should().Be(0);
            InvalidSevMatches.Should().Be(0);
            TotalWarningsSuppressedCount.Should().Be(3);
            DistinctAdvisoriesSuppressedCount.Should().Be(2);
            packagesWithReportedAdvisories.Should().HaveCount(1);
            packagesWithReportedAdvisories[0].Should().Be(packageIdentityB);
        }

        // Setup a test bed with multiple sources, 1 source with vulnerabilities, 1 vulnerable package wih a moderate vulnerability.
        private static void SetupPrerequisites(out AuditChecker auditChecker, out string projectPath, out List<PackageRestoreData> packages)
        {
            string packageId = "A";
            PackageIdentity packageIdentity = new(packageId, new NuGetVersion(1, 0, 0));
            projectPath = "C:\\solution\\project\\project.csproj";

            List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities = new List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>()
            {
                new Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>
                {
                    {
                        packageId,
                        new PackageVulnerabilityInfo[] {
                            new PackageVulnerabilityInfo(new Uri("https://vulnerability1"), PackageVulnerabilitySeverity.Moderate, VersionRange.Parse("[1.0.0,2.0.0)"))
                        }
                    }
                }
            };

            string sourceWithVulnerabilityData = "https://contoso.com/vulnerability/v3/index.json";
            Dictionary<string, GetVulnerabilityInfoResult> vulnerabilityResults = new()
            {
                { sourceWithVulnerabilityData, new GetVulnerabilityInfoResult(knownVulnerabilities, exceptions: null) }
            };

            var providers = new List<INuGetResourceProvider> { new VulnerabilityInfoResourceProvider(vulnerabilityResults) };
            var sourceRepositories = new List<SourceRepository>
            {
                new SourceRepository(new PackageSource("https://contoso.com/v3/index.json"), providers),
                new SourceRepository(new PackageSource(sourceWithVulnerabilityData), providers)
            };

            var logger = new TestLogger();

            auditChecker = new AuditChecker(sourceRepositories, new SourceCacheContext(), logger);

            packages = new List<PackageRestoreData>
            {
                new PackageRestoreData(new PackageReference(packageIdentity, CommonFrameworks.Net472), new string[]{ projectPath }, isMissing: true)
            };
        }
    }
}

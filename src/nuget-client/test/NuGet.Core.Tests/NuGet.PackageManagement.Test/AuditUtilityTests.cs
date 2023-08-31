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
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Model;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;
using static NuGet.PackageManagement.AuditUtility;

namespace NuGet.PackageManagement.Test
{
    public class AuditUtilityTests
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

            AuditUtility.GetKnownVulnerabilities("packageId", new NuGetVersion(1, 0, 0), knownVulnerabilities).Should().BeNull();
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

            AuditUtility.GetKnownVulnerabilities("packageId", new NuGetVersion(2, 0, 0), knownVulnerabilities).Should().BeNull();
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

            List<PackageVulnerabilityInfo>? vulnerabilities = AuditUtility.GetKnownVulnerabilities("packageId", new NuGetVersion(2, 0, 0), knownVulnerabilities);

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

            List<PackageVulnerabilityInfo>? vulnerabilities = AuditUtility.GetKnownVulnerabilities("packageId", new NuGetVersion(2, 0, 0), knownVulnerabilities);

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

            List<PackageVulnerabilityInfo>? vulnerabilities = AuditUtility.GetKnownVulnerabilities("packageId", new NuGetVersion(2, 0, 0), knownVulnerabilities);

            vulnerabilities.Should().HaveCount(1);
            var moderateVulnerability = vulnerabilities![0];
            moderateVulnerability.Severity.Should().Be(PackageVulnerabilitySeverity.Moderate);
            moderateVulnerability.Url.Should().Be(new Uri("https://contoso.com/random-vulnerability2"));
            moderateVulnerability.Versions.Should().Be(VersionRange.Parse("[2.0.0, 2.0.0]"));
        }

        [Fact]
        public void FindPackagesWithKnownVulnerabilities_WithoutPackageNotVulnerable_ReturnsNull()
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

            AuditUtility.FindPackagesWithKnownVulnerabilities(knownVulnerabilities, packages, PackageVulnerabilitySeverity.Low).Should().BeNull();
        }

        [Fact]
        public void FindPackagesWithKnownVulnerabilities_WithoutVulnerablePackageButLowerSeverity_ReturnsNull()
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
                                    PackageVulnerabilitySeverity.Moderate,
                                    VersionRange.Parse("[1.0.0, 3.0.0)"))
                            }
                        }
                    }
                };

            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            var packages = new List<PackageRestoreData>
            {
                new PackageRestoreData(new PackageReference(packageIdentity, CommonFrameworks.Net472), new string[]{ }, isMissing: true)
            };

            AuditUtility.FindPackagesWithKnownVulnerabilities(knownVulnerabilities, packages, PackageVulnerabilitySeverity.High).Should().BeNull();
        }

        [Fact]
        public void FindPackagesWithKnownVulnerabilities_WithVulnerablePackageWithinSpecifiedSeverity_ReturnsAppropriatePackages()
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
                                    PackageVulnerabilitySeverity.High,
                                    VersionRange.Parse("[2.0.0, 3.0.0)"))
                            }
                        }
                    }
                };

            PackageIdentity packageIdentity = new("packageId", new NuGetVersion(2, 0, 0));
            var packages = new List<PackageRestoreData>
            {
                new PackageRestoreData(new PackageReference(packageIdentity, CommonFrameworks.Net472), new string[]{ }, isMissing: true)
            };

            var packagesWithVulnerabilities = AuditUtility.FindPackagesWithKnownVulnerabilities(knownVulnerabilities, packages, PackageVulnerabilitySeverity.Moderate);
            packagesWithVulnerabilities.Should().HaveCount(1);
            (PackageIdentity vulnerablePackage, AuditUtility.PackageAuditInfo auditInfo) = packagesWithVulnerabilities.Single();
            vulnerablePackage.Should().Be(packageIdentity);
            auditInfo.Identity.Should().Be(packageIdentity);
            auditInfo.Vulnerabilities.Should().HaveCount(2);

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
            AuditUtility.GetSeverityLabelAndCode(severity).Should().Be((expectedLabel, expectedCode));
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
            GetVulnerabilityInfoResult? vulnerabilityData = await AuditUtility.GetAllVulnerabilityDataAsync(sourceRepositories, Mock.Of<SourceCacheContext>(), NullLogger.Instance, CancellationToken.None);
            vulnerabilityData.Should().BeNull();
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

            GetVulnerabilityInfoResult? vulnerabilityData = await AuditUtility.GetAllVulnerabilityDataAsync(sourceRepositories, Mock.Of<SourceCacheContext>(), NullLogger.Instance, CancellationToken.None);
            vulnerabilityData.Should().NotBeNull();
            vulnerabilityData!.Exceptions.Should().BeNull();
            vulnerabilityData.KnownVulnerabilities.Should().HaveCount(1);
            vulnerabilityData.KnownVulnerabilities.Single().Keys.Should().Contain("A");
            vulnerabilityData.KnownVulnerabilities.Single().Values.Single().Should().HaveCount(1);
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

            GetVulnerabilityInfoResult? vulnerabilityData = await AuditUtility.GetAllVulnerabilityDataAsync(sourceRepositories, Mock.Of<SourceCacheContext>(), NullLogger.Instance, CancellationToken.None);
            vulnerabilityData.Should().NotBeNull();
            vulnerabilityData!.KnownVulnerabilities.Should().HaveCount(1);
            vulnerabilityData.KnownVulnerabilities.Single().Keys.Should().Contain("A");
            vulnerabilityData.KnownVulnerabilities.Single().Values.Single().Should().HaveCount(1);
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

            GetVulnerabilityInfoResult? vulnerabilityData = await AuditUtility.GetAllVulnerabilityDataAsync(sourceRepositories, Mock.Of<SourceCacheContext>(), NullLogger.Instance, CancellationToken.None);
            vulnerabilityData.Should().NotBeNull();
            vulnerabilityData!.KnownVulnerabilities.Should().HaveCount(2);
            vulnerabilityData.KnownVulnerabilities.First().Keys.Should().Contain("B");
            vulnerabilityData.KnownVulnerabilities.First().Values.Single().Should().HaveCount(1);
            vulnerabilityData.KnownVulnerabilities.Last().Keys.Should().Contain("A");
            vulnerabilityData.KnownVulnerabilities.Last().Values.Single().Should().HaveCount(1);
        }

        [Fact]
        public void CreateWarningsForPackagesWithVulnerabilities_CreatesWarningsForAllVulnerabilities()
        {
            var packageA = new PackageIdentity("A", new NuGetVersion(1, 0, 0));
            var packageB = new PackageIdentity("B", new NuGetVersion(2, 0, 0));

            var pva = new PackageAuditInfo(packageA);
            pva.Vulnerabilities.Add(new PackageVulnerabilityInfo(new Uri("https://vulnerability1"), PackageVulnerabilitySeverity.Low, VersionRange.Parse("[1.0.0,2.0.0)")));
            pva.Vulnerabilities.Add(new PackageVulnerabilityInfo(new Uri("https://vulnerability2"), PackageVulnerabilitySeverity.Moderate, VersionRange.Parse("[1.0.0,1.1.0)")));
            var pvb = new PackageAuditInfo(packageB);
            pvb.Vulnerabilities.Add(new PackageVulnerabilityInfo(new Uri("https://vulnerability3"), PackageVulnerabilitySeverity.High, VersionRange.Parse("[2.0.0,3.0.0)")));
            pvb.Vulnerabilities.Add(new PackageVulnerabilityInfo(new Uri("https://vulnerability4"), PackageVulnerabilitySeverity.Critical, VersionRange.Parse("[2.0.0,2.1.0)")));

            Dictionary<PackageIdentity, PackageAuditInfo> packagesWithKnownVulnerabilities = new()
            {
                { packageA, pva },
                { packageB, pvb }
            };

            var testLogger = new TestLogger();
            AuditUtility.CreateWarningsForPackagesWithVulnerabilities(packagesWithKnownVulnerabilities, testLogger);
            testLogger.WarningMessages.Should().HaveCount(4);
            testLogger.WarningMessages.Should().Contain(string.Format(Strings.Warning_PackageWithKnownVulnerability,
                        packageA.Id,
                        packageA.Version.ToNormalizedString(),
                        pva.Vulnerabilities[0].Severity.ToString().ToLower(),
                        pva.Vulnerabilities[0].Url));
            testLogger.WarningMessages.Should().Contain(string.Format(Strings.Warning_PackageWithKnownVulnerability,
                        packageA.Id,
                        packageA.Version.ToNormalizedString(),
                        pva.Vulnerabilities[1].Severity.ToString().ToLower(),
                        pva.Vulnerabilities[1].Url));
            testLogger.WarningMessages.Should().Contain(string.Format(Strings.Warning_PackageWithKnownVulnerability,
                        packageB.Id,
                        packageB.Version.ToNormalizedString(),
                        pvb.Vulnerabilities[0].Severity.ToString().ToLower(),
                        pvb.Vulnerabilities[0].Url));
            testLogger.WarningMessages.Should().Contain(string.Format(Strings.Warning_PackageWithKnownVulnerability,
                        packageB.Id,
                        packageB.Version.ToNormalizedString(),
                        pvb.Vulnerabilities[1].Severity.ToString().ToLower(),
                        pvb.Vulnerabilities[1].Url));
        }
    }
}

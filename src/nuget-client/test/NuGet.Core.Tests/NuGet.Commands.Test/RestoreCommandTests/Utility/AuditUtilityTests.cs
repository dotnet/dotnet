// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Commands.Restore.Utility;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Model;
using NuGet.RuntimeModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Commands.Test.RestoreCommandTests.Utility;
public class AuditUtilityTests
{
    private static Uri CveUrl = new Uri("https://cve.test/1");
    private static VersionRange UpToV2 = new VersionRange(maxVersion: new NuGetVersion(2, 0, 0), includeMaxVersion: false);

    [Fact]
    public async Task Check_VulnerabilityProviderWithExceptions_WarningsReplayedToLogger()
    {
        // Arrange
        var context = new AuditTestContext();
        var exception1 = new AggregateException(new HttpRequestException("404"));
        context.WithVulnerabilityProvider().WithException(exception1);
        var exception2 = new AggregateException(new HttpRequestException("401"));
        context.WithVulnerabilityProvider().WithException(exception2);

        context.WithRestoreTarget();

        // Act
        _ = await context.CheckPackageVulnerabilitiesAsync(CancellationToken.None);

        // Assert
        context.Log.LogMessages.Count.Should().Be(2);
        context.Log.LogMessages.All(m => m.Code == NuGetLogCode.NU1900).Should().BeTrue();
        context.Log.LogMessages.All(m => m.ProjectPath == context.ProjectFullPath).Should().BeTrue();
        context.Log.LogMessages.Where(m => m.Message.Contains("404")).Should().ContainSingle();
        context.Log.LogMessages.Where(m => m.Message.Contains("401")).Should().ContainSingle();
        context.Log.LogMessages.All(m => m.Level == LogLevel.Warning).Should().BeTrue();
    }

    [Theory]
    [InlineData("default", false)]
    [InlineData("true", true)]
    public async Task Check_WithNoVulnerabilitySources_NU1905Warning(string enable, bool expectWarning)
    {
        // Arrange
        var context = new AuditTestContext();
        context.Enabled = enable;

        context.WithRestoreTarget();

        // Act
        var auditUtility = await context.CheckPackageVulnerabilitiesAsync(CancellationToken.None);

        // Assert
        if (expectWarning)
        {
            context.Log.LogMessages.Count.Should().Be(1);
            var message = (RestoreLogMessage)context.Log.LogMessages.Single();
            message.Code.Should().Be(NuGetLogCode.NU1905);
            message.ProjectPath.Should().Be(context.ProjectFullPath);
            message.Level.Should().Be(LogLevel.Warning);
        }
        else
        {
            context.Log.LogMessages.Count.Should().Be(0);
        }

        // for perf, when we don't have data to check, we shouldn't waste time checking
        auditUtility.DownloadDurationSeconds.Should().NotBeNull();
        auditUtility.CheckPackagesDurationSeconds.Should().BeNull();
        auditUtility.GenerateOutputDurationSeconds.Should().BeNull();
    }

    [Fact]
    public async Task Check_ProjectWithoutVulnerablePackages_NoWarnings()
    {
        // Arrange
        var context = new AuditTestContext();

        var packageVulnerabilities = context.WithVulnerabilityProvider().WithPackageVulnerability("SomePackage");
        packageVulnerabilities.Add(new PackageVulnerabilityInfo(CveUrl, PackageVulnerabilitySeverity.Moderate, UpToV2));

        context.WithRestoreTarget();

        // Act
        var auditUtil = await context.CheckPackageVulnerabilitiesAsync(CancellationToken.None);

        // Assert
        context.Log.LogMessages.Count.Should().Be(0);

        // time to output should be zero since there are no messages, for perf.
        auditUtil.DownloadDurationSeconds.Should().NotBeNull();
        auditUtil.CheckPackagesDurationSeconds.Should().NotBeNull();
        auditUtil.GenerateOutputDurationSeconds.Should().BeNull();
    }

    [Theory]
    [InlineData(PackageVulnerabilitySeverity.Low, NuGetLogCode.NU1901)]
    [InlineData(PackageVulnerabilitySeverity.Moderate, NuGetLogCode.NU1902)]
    [InlineData(PackageVulnerabilitySeverity.High, NuGetLogCode.NU1903)]
    [InlineData(PackageVulnerabilitySeverity.Critical, NuGetLogCode.NU1904)]
    [InlineData(PackageVulnerabilitySeverity.Unknown, NuGetLogCode.NU1900)]
    public async Task Check_ProjectReferencingPackageWithVulnerability_WarningLogged(PackageVulnerabilitySeverity severity, NuGetLogCode expectedCode)
    {
        // Arrange
        var context = new AuditTestContext();
        context.Mode = "all";

        var vulnerabilityProvider = context.WithVulnerabilityProvider();
        var knownVulnerabilities = vulnerabilityProvider.WithPackageVulnerability("pkga");
        knownVulnerabilities.Add(
            new PackageVulnerabilityInfo(
                CveUrl,
                severity,
                UpToV2));
        knownVulnerabilities = vulnerabilityProvider.WithPackageVulnerability("pkgb");
        knownVulnerabilities.Add(
            new PackageVulnerabilityInfo(
                CveUrl,
                severity,
                UpToV2));

        context.WithRestoreTarget()
            .DependsOn("pkga", "1.0.0");

        context.PackagesDependencyProvider.Package("pkga", "1.0.0").DependsOn("pkgb", "1.0.0");
        context.PackagesDependencyProvider.Package("pkgb", "1.0.0");

        // Act
        var auditUtility = await context.CheckPackageVulnerabilitiesAsync(CancellationToken.None);

        // Assert
        context.Log.LogMessages.Count.Should().Be(2);

        context.Log.LogMessages.Where(m => m.Message.Contains("pkga")).Should().NotBeNullOrEmpty();
        RestoreLogMessage message = (RestoreLogMessage)context.Log.LogMessages.Where(m => m.Message.Contains("pkga")).Single();
        ValidateRestoreLogMessage(message, "pkga", expectedCode, context);

        context.Log.LogMessages.Where(m => m.Message.Contains("pkgb")).Should().NotBeNullOrEmpty();
        message = (RestoreLogMessage)context.Log.LogMessages.Where(m => m.Message.Contains("pkgb")).Single();
        ValidateRestoreLogMessage(message, "pkgb", expectedCode, context);

        auditUtility.DownloadDurationSeconds.Should().NotBeNull();
        auditUtility.CheckPackagesDurationSeconds.Should().NotBeNull();
        auditUtility.GenerateOutputDurationSeconds.Should().NotBeNull();

        auditUtility.DirectPackagesWithAdvisory.Should().NotBeNull();
        auditUtility.DirectPackagesWithAdvisory!.Should().BeEquivalentTo(new[] { "pkga" });

        auditUtility.TransitivePackagesWithAdvisory.Should().NotBeNull();
        auditUtility.TransitivePackagesWithAdvisory!.Should().BeEquivalentTo(new[] { "pkgb" });

        int expectedCount = severity == PackageVulnerabilitySeverity.Low ? 1 : 0;
        auditUtility.Sev0DirectMatches.Should().Be(expectedCount);
        auditUtility.Sev0TransitiveMatches.Should().Be(expectedCount);

        expectedCount = severity == PackageVulnerabilitySeverity.Moderate ? 1 : 0;
        auditUtility.Sev1DirectMatches.Should().Be(expectedCount);
        auditUtility.Sev1TransitiveMatches.Should().Be(expectedCount);

        expectedCount = severity == PackageVulnerabilitySeverity.High ? 1 : 0;
        auditUtility.Sev2DirectMatches.Should().Be(expectedCount);
        auditUtility.Sev2TransitiveMatches.Should().Be(expectedCount);

        expectedCount = severity == PackageVulnerabilitySeverity.Critical ? 1 : 0;
        auditUtility.Sev3DirectMatches.Should().Be(expectedCount);
        auditUtility.Sev3TransitiveMatches.Should().Be(expectedCount);

        expectedCount = severity == PackageVulnerabilitySeverity.Unknown ? 1 : 0;
        auditUtility.InvalidSevDirectMatches.Should().Be(expectedCount);
        auditUtility.InvalidSevTransitiveMatches.Should().Be(expectedCount);

        static void ValidateRestoreLogMessage(RestoreLogMessage message, string packageId, NuGetLogCode expectedCode, AuditTestContext context)
        {
            message.Message.Should().Contain("1.0.0", "Message doesn't contain package version");
            message.Message.Should().Contain(CveUrl.OriginalString, "Message doesn't contain CVE URL");
            message.Code.Should().Be(expectedCode);
            message.ProjectPath.Should().Be(context.ProjectFullPath);
            message.LibraryId.Should().Be(packageId);
            message.TargetGraphs.Should().BeEquivalentTo(new[] { "net6.0" });
        }
    }

    [Fact]
    public async Task Check_TwoVulnerabilityProviders_MergesKnownVulnerabilities()
    {
        // Arrange
        var context = new AuditTestContext();
        context.Mode = "all";

        PackageVulnerabilityInfo commonKnownVulnerability = new PackageVulnerabilityInfo(CveUrl, PackageVulnerabilitySeverity.Moderate, UpToV2);
        Uri cve2Url = new("https://cve.test/2");
        Uri cve3Url = new("https://cve.test/3");

        // provider 1 knows about vulnerabilities 1 and 2
        var vulnerabilityProvider = context.WithVulnerabilityProvider();
        var knownVulnerabilities = vulnerabilityProvider.WithPackageVulnerability("pkga");
        knownVulnerabilities.Add(commonKnownVulnerability);
        knownVulnerabilities.Add(new PackageVulnerabilityInfo(cve2Url, PackageVulnerabilitySeverity.Moderate, UpToV2));

        // provider 2 knows about vulnerabilities 1 and 3
        vulnerabilityProvider = context.WithVulnerabilityProvider();
        knownVulnerabilities = vulnerabilityProvider.WithPackageVulnerability("pkga");
        knownVulnerabilities.Add(commonKnownVulnerability);
        knownVulnerabilities.Add(new PackageVulnerabilityInfo(cve3Url, PackageVulnerabilitySeverity.Moderate, UpToV2));

        context.WithRestoreTarget().DependsOn("pkga", "1.0.0");
        context.PackagesDependencyProvider.Package("pkga", "1.0.0");

        // Act
        var auditUtility = await context.CheckPackageVulnerabilitiesAsync(CancellationToken.None);

        // Assert
        // the common cve both vulnerability providers know about should be deduplicated
        context.Log.LogMessages.Count.Should().Be(3);

        List<RestoreLogMessage> messages = new(3);
        messages.AddRange(context.Log.LogMessages.Cast<RestoreLogMessage>());

        messages.All(m => m.LibraryId == "pkga").Should().BeTrue();
        messages.Any(m => m.Message.Contains(CveUrl.OriginalString)).Should().BeTrue();
        messages.Any(m => m.Message.Contains(cve2Url.OriginalString)).Should().BeTrue();
        messages.Any(m => m.Message.Contains(cve3Url.OriginalString)).Should().BeTrue();
    }

    /// <summary>
    /// Diamond dependency pkga has a known vulnerability on the lower version, but none on the higher version.
    /// Therefore, no warnings or vulnerable packages should be detected.
    /// </summary>
    [Fact]
    public async Task Check_RejectedTransitivePackageInGraphHasKnownVulnerability_NoWarningsOrErrors()
    {
        // Arrange
        var context = new AuditTestContext();
        context.Mode = "all";

        // project -> pkgb 1.0.0 -> pkga 1.0.0
        //         -> pkgc 1.0.0 -> pkga 2.0.0
        context.PackagesDependencyProvider.Package("pkga", "1.0.0");
        context.PackagesDependencyProvider.Package("pkga", "2.0.0");
        context.PackagesDependencyProvider.Package("pkgb", "1.0.0").DependsOn("pkga", "1.0.0");
        context.PackagesDependencyProvider.Package("pkgc", "1.0.0").DependsOn("pkga", "2.0.0");

        context.WithRestoreTarget()
            .DependsOn("pkgb", "1.0.0")
            .DependsOn("pkgc", "1.0.0");

        var pkgaVulnerabilities = context
            .WithVulnerabilityProvider()
            .WithPackageVulnerability("pkga");
        pkgaVulnerabilities.Add(
            new PackageVulnerabilityInfo(
                new Uri("https://cve.test/cve1"),
                PackageVulnerabilitySeverity.Moderate,
                new VersionRange(maxVersion: new NuGetVersion(2, 0, 0), includeMaxVersion: false)));

        // Act
        AuditUtility auditUtility = await context.CheckPackageVulnerabilitiesAsync(CancellationToken.None);

        //Assert
        auditUtility.CheckPackagesDurationSeconds.Should().NotBeNull("audit utility early exit before checking graph for known vulnerabilities");
        context.Log.Messages.Should().BeEmpty();
        auditUtility.DirectPackagesWithAdvisory.Should().BeNullOrEmpty();
        auditUtility.TransitivePackagesWithAdvisory.Should().BeNullOrEmpty();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Check_TransitivePackageHasKnownVulnerability_WarningInAllMode(bool auditModeAll)
    {
        // Arrange
        var context = new AuditTestContext();
        if (auditModeAll)
        {
            context.Mode = "all";
        }

        string vulnerablePackage = "pkga";
        string vulnerableVersion = "1.2.3";

        context.PackagesDependencyProvider.Package(vulnerablePackage, vulnerableVersion);
        context.PackagesDependencyProvider.Package("pkgb", "1.0.0").DependsOn(vulnerablePackage, vulnerableVersion);

        context.WithRestoreTarget()
            .DependsOn("pkgb", "1.0.0");

        var pkgaVulnerabilities = context
            .WithVulnerabilityProvider()
            .WithPackageVulnerability(vulnerablePackage);
        pkgaVulnerabilities.Add(
            new PackageVulnerabilityInfo(
                new Uri("https://cve.test/cve1"),
                PackageVulnerabilitySeverity.Moderate,
                new VersionRange(maxVersion: new NuGetVersion(2, 0, 0), includeMaxVersion: false)));

        // Act
        AuditUtility auditUtility = await context.CheckPackageVulnerabilitiesAsync(CancellationToken.None);

        //Assert
        auditUtility.CheckPackagesDurationSeconds.Should().NotBeNull("audit utility early exit before checking graph for known vulnerabilities");

        if (auditModeAll)
        {
            context.Log.Messages.Count.Should().Be(1);
            RestoreLogMessage message = (RestoreLogMessage)context.Log.LogMessages.Single();
            message.Message.Should().Contain(vulnerablePackage).And.Contain(vulnerableVersion);
            message.ProjectPath.Should().Be(context.ProjectFullPath);
        }
        else
        {
            context.Log.Messages.Count().Should().Be(0);
        }

        auditUtility.DirectPackagesWithAdvisory.Should().BeNullOrEmpty();
        auditUtility.TransitivePackagesWithAdvisory.Should().BeEquivalentTo(new[] { vulnerablePackage });
    }

    [Fact]
    public async Task Check_MultiTargetingProjectFile_WarningsHaveExpectedProperties()
    {
        // Arrange

        DependencyProvider packageDependencyProvider = new();
        packageDependencyProvider.Package("pkga", "1.0.0");
        packageDependencyProvider.Package("pkgb", "1.0.0");

        Task<RestoreTargetGraph>[] createGraphTasks =
        {
            CreateGraphAsync(packageDependencyProvider, "pkga", FrameworkConstants.CommonFrameworks.Net60),
            CreateGraphAsync(packageDependencyProvider, "pkgb", FrameworkConstants.CommonFrameworks.Net50)
        };

        List<VulnerabilityProviderTestContext> vulnerabilityProviderContexts = new(1)
        {
            new VulnerabilityProviderTestContext()
        };
        vulnerabilityProviderContexts[0].WithPackageVulnerability("pkga").Add(new PackageVulnerabilityInfo(CveUrl, 0, UpToV2));
        vulnerabilityProviderContexts[0].WithPackageVulnerability("pkgb").Add(new PackageVulnerabilityInfo(CveUrl, 0, UpToV2));

        var vulnerabilityProviders = AuditTestContext.CreateVulnerabilityInformationProviders(vulnerabilityProviderContexts);

        RestoreTargetGraph[] graphs =
        {
            await createGraphTasks[0],
            await createGraphTasks[1]
        };

        RestoreAuditProperties restoreAuditProperties = new()
        {
            EnableAudit = "default",
        };

        var log = new TestLogger();

        // Act
        var audit = new AuditUtility(
            AuditUtility.ParseEnableValue(restoreAuditProperties.EnableAudit),
            restoreAuditProperties,
            "/path/proj.csproj",
            graphs,
            vulnerabilityProviders,
            log);
        await audit.CheckPackageVulnerabilitiesAsync(CancellationToken.None);

        // Assert
        log.LogMessages.Count.Should().Be(2);

        RestoreLogMessage message = log.LogMessages.Cast<RestoreLogMessage>().Single(m => m.LibraryId == "pkga");
        message.TargetGraphs.Should().BeEquivalentTo(new[] { "net6.0" });

        message = log.LogMessages.Cast<RestoreLogMessage>().Single(m => m.LibraryId == "pkgb");
        message.TargetGraphs.Should().BeEquivalentTo(new[] { "net5.0" });

        static async Task<RestoreTargetGraph> CreateGraphAsync(DependencyProvider packageProvider, string dependencyId, NuGetFramework targetFramework)
        {
            DependencyProvider projectProvider = new();
            projectProvider.Package("proj", "1.0.0", LibraryType.Project).DependsOn(dependencyId, "1.0.0");

            var walkContext = new TestRemoteWalkContext();
            walkContext.LocalLibraryProviders.Add(packageProvider);
            walkContext.ProjectLibraryProviders.Add(projectProvider);
            var walker = new RemoteDependencyWalker(walkContext);

            LibraryRange restoreTarget = new("proj", new VersionRange(NuGetVersion.Parse("1.0.0")), LibraryDependencyTarget.Project);

            var walkResult = await walker.WalkAsync(restoreTarget, targetFramework, "", RuntimeGraph.Empty, true);

            var graph = RestoreTargetGraph.Create(new[] { walkResult }, walkContext, NullLogger.Instance, targetFramework);

            return graph;
        }
    }

    private class AuditTestContext
    {
        public string ProjectFullPath { get; set; } = RuntimeEnvironmentHelper.IsWindows ? @"n:\proj\proj.csproj" : "/src/proj/proj.csproj";
        public string? Enabled { get; set; } = "default";
        public string? Level { get; set; }
        public string? Mode { get; set; }

        public TestLogger Log { get; } = new();

        public DependencyProvider ProjectDependencyProvider { get; } = new();
        public DependencyProvider PackagesDependencyProvider { get; } = new();

        private LibraryRange? _walkTarget;

        private List<VulnerabilityProviderTestContext>? _vulnerabilityProviders;

        /// <summary>
        /// Set up the project that is being restored (not just a project reference)
        /// </summary>
        public DependencyProvider.TestPackage WithRestoreTarget(string projectName = "proj", string version = "1.0.0")
        {
            if (_walkTarget != null)
            {
                throw new InvalidOperationException($"Cannot set more than 1 restore target");
            }

            var projectVersion = NuGetVersion.Parse(version);

            _walkTarget = new LibraryRange(projectName, new VersionRange(projectVersion), LibraryDependencyTarget.Project);

            var testProject = ProjectDependencyProvider.Package(projectName, projectVersion, LibraryType.Project);
            return testProject;
        }

        public VulnerabilityProviderTestContext WithVulnerabilityProvider()
        {
            if (_vulnerabilityProviders is null)
            {
                _vulnerabilityProviders = new();
            }

            VulnerabilityProviderTestContext provider = new();
            _vulnerabilityProviders.Add(provider);
            return provider;
        }

        public async Task<AuditUtility> CheckPackageVulnerabilitiesAsync(CancellationToken cancellationToken)
        {
            AuditUtility.EnabledValue enabled;
            if (Enabled is null
                || (enabled = AuditUtility.ParseEnableValue(Enabled)) == AuditUtility.EnabledValue.Undefined
                || enabled == AuditUtility.EnabledValue.ExplicitOptOut)
            {
                throw new InvalidOperationException($"{nameof(Enabled)} must have a value that does not disable NuGetAudit.");
            }

            if (_walkTarget is null)
            {
                throw new InvalidOperationException($"{nameof(WithRestoreTarget)} must be called once");
            }

            RestoreAuditProperties restoreAuditProperties = new()
            {
                EnableAudit = Enabled,
                AuditLevel = Level,
                AuditMode = Mode,
            };

            var graphs = await CreateGraphsAsync();

            var vulnProviders = CreateVulnerabilityInformationProviders(_vulnerabilityProviders);

            var audit = new AuditUtility(enabled, restoreAuditProperties, ProjectFullPath, graphs, vulnProviders, Log);
            await audit.CheckPackageVulnerabilitiesAsync(CancellationToken.None);

            return audit;

            async Task<RestoreTargetGraph[]> CreateGraphsAsync()
            {
                var walkContext = new TestRemoteWalkContext();
                walkContext.LocalLibraryProviders.Add(PackagesDependencyProvider);
                walkContext.ProjectLibraryProviders.Add(ProjectDependencyProvider);
                var walker = new RemoteDependencyWalker(walkContext);

                var targetFramework = FrameworkConstants.CommonFrameworks.Net60;
                var graph = await walker.WalkAsync(_walkTarget, targetFramework, "", RuntimeGraph.Empty, true);

                RestoreTargetGraph[] graphs = new[]
                {
                    RestoreTargetGraph.Create(new[] { graph }, walkContext, NullLogger.Instance, targetFramework)
                };

                return graphs;
            }
        }

        public static List<IVulnerabilityInformationProvider> CreateVulnerabilityInformationProviders(List<VulnerabilityProviderTestContext>? providers)
        {
            List<IVulnerabilityInformationProvider> result = new();

            if (providers is null)
            {
                return result;
            }

            foreach (var provider in providers)
            {
                List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>>? knownVulnerabilities =
                    provider.KnownVulnerabilities is not null ? new() { provider.KnownVulnerabilities } : null;
                GetVulnerabilityInfoResult getVulnerabilityInfoResult = new(knownVulnerabilities, provider.Exceptions);
                var vulnProvider = new Mock<IVulnerabilityInformationProvider>();
                vulnProvider.Setup(p => p.GetVulnerabilityInformationAsync(It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult<GetVulnerabilityInfoResult?>(getVulnerabilityInfoResult));
                result.Add(vulnProvider.Object);
            }

            return result;
        }
    }

    private class VulnerabilityProviderTestContext
    {
        public Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>? KnownVulnerabilities { get; private set; }
        public AggregateException? Exceptions { get; private set; }

        public List<PackageVulnerabilityInfo> WithPackageVulnerability(string packageId)
        {
            List<PackageVulnerabilityInfo> packageVulnerabilities = new();

            if (KnownVulnerabilities is null)
            {
                KnownVulnerabilities = new();
            }

            KnownVulnerabilities.Add(packageId, packageVulnerabilities);

            return packageVulnerabilities;
        }

        internal void WithException(AggregateException exceptions)
        {
            if (Exceptions is not null)
            {
                throw new InvalidOperationException("Vulnerability provider exceptions cannot be set more than once");
            }

            Exceptions = exceptions;
        }
    }
}

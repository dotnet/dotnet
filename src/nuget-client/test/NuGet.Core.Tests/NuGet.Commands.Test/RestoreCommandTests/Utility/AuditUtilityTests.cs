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
using NuGet.Commands.Restore.Utility;
using NuGet.Common;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Protocol.Model;
using NuGet.RuntimeModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Commands.Test.RestoreCommandTests.Utility;
public class AuditUtilityTests
{
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

        var pkgaVulnerabilities = context.WithPackageVulnerability("pkga");
        pkgaVulnerabilities.Add(
            new PackageVulnerabilityInfo(
                new Uri("https://cve.test/cve1"),
                severity: 1,
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

        var pkgaVulnerabilities = context.WithPackageVulnerability(vulnerablePackage);
        pkgaVulnerabilities.Add(
            new PackageVulnerabilityInfo(
                new Uri("https://cve.test/cve1"),
                severity: 1,
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

        private Dictionary<string, IReadOnlyList<PackageVulnerabilityInfo>> _knownVulnerabilities = new();

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

        public List<PackageVulnerabilityInfo> WithPackageVulnerability(string packageId)
        {
            List<PackageVulnerabilityInfo> packageVulnerabilities = new();

            _knownVulnerabilities.Add(packageId, packageVulnerabilities);

            return packageVulnerabilities;
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

            var vulnProviders = CreateVulnerabilityInformationProviders();

            var audit = new AuditUtility(AuditUtility.EnabledValue.ImplicitOptIn, restoreAuditProperties, ProjectFullPath, graphs, vulnProviders, Log);
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

            List<IVulnerabilityInformationProvider> CreateVulnerabilityInformationProviders()
            {
                List<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> allKnownVulnerabilities = new() { _knownVulnerabilities };
                GetVulnerabilityInfoResult getVulnerabilityInfoResult = new(allKnownVulnerabilities, exceptions: null);

                var vulnProvider = new Mock<IVulnerabilityInformationProvider>();
                vulnProvider.Setup(p => p.GetVulnerabilityInformationAsync(It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult<GetVulnerabilityInfoResult?>(getVulnerabilityInfoResult));
                var vulnProviders = new List<IVulnerabilityInformationProvider>(1) { vulnProvider.Object };

                return vulnProviders;
            }
        }
    }
}

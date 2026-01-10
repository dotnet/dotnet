// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest.Commands.Package.Update.PackageUpdateIOTests;

public class GetKnownVulnerabilitiesAsyncTests
{
    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance);
        var packageUpdateIO = new PackageUpdateIO(solutionRoot, msbuildUtility, TestEnvironmentVariableReader.EmptyInstance);
        return packageUpdateIO;
    }

    [Fact]
    public async Task GetKnownVulnerabilitiesAsync_WithNoSources_ReturnsEmptyList()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();

        var nugetConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <clear />
  </packageSources>
</configuration>";
        File.WriteAllText(Path.Combine(testContext.SolutionRoot, "nuget.config"), nugetConfig);

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = await packageUpdateIO.GetKnownVulnerabilitiesAsync(
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetKnownVulnerabilitiesAsync_WithMockServerSource_ReturnsVulnerabilities()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        var packageId = "TestPackage.Vulnerable";

        using var mockServer = new FileSystemBackedV3MockServer(
            testContext.PackageSource,
            isPrivateFeed: false,
            sourceReportsVulnerabilities: true);

        mockServer.Vulnerabilities.Add(packageId, new List<(Uri, PackageVulnerabilitySeverity, VersionRange)>
        {
            (new Uri("https://example.com/vuln1"), PackageVulnerabilitySeverity.High, VersionRange.Parse("[1.0.0, 1.1.0]")),
            (new Uri("https://example.com/vuln2"), PackageVulnerabilitySeverity.Moderate, VersionRange.Parse("[2.0.0]"))
        });

        mockServer.Start();

        var nugetConfig = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <clear />
    <add key=""testSource"" value=""{mockServer.ServiceIndexUri}"" allowInsecureConnections=""true"" />
  </packageSources>
</configuration>";
        File.WriteAllText(Path.Combine(testContext.SolutionRoot, "nuget.config"), nugetConfig);

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = await packageUpdateIO.GetKnownVulnerabilitiesAsync(
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);

        var vulnerabilityDict = result[0];
        vulnerabilityDict.Should().ContainKey(packageId);

        var packageVulnerabilities = vulnerabilityDict[packageId];
        packageVulnerabilities.Should().HaveCount(2);

        packageVulnerabilities[0].Url.Should().Be(new Uri("https://example.com/vuln1"));
        packageVulnerabilities[0].Severity.Should().Be(PackageVulnerabilitySeverity.High);
        packageVulnerabilities[0].Versions.Should().Be(VersionRange.Parse("[1.0.0, 1.1.0]"));

        packageVulnerabilities[1].Url.Should().Be(new Uri("https://example.com/vuln2"));
        packageVulnerabilities[1].Severity.Should().Be(PackageVulnerabilitySeverity.Moderate);
        packageVulnerabilities[1].Versions.Should().Be(VersionRange.Parse("[2.0.0]"));
    }

    [Fact]
    public async Task GetKnownVulnerabilitiesAsync_WithAuditSource_ReturnsVulnerabilities()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();
        var packageId = "TestPackage.WithAudit";

        var packages = new[]
        {
            new SimpleTestPackageContext(packageId, "1.0.0"),
            new SimpleTestPackageContext(packageId, "2.0.0")
        };

        await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packages);

        using var mockServer = new FileSystemBackedV3MockServer(
            testContext.PackageSource,
            isPrivateFeed: false,
            sourceReportsVulnerabilities: true);

        mockServer.Vulnerabilities.Add(packageId, new List<(Uri, PackageVulnerabilitySeverity, VersionRange)>
        {
            (new Uri("https://example.com/security-advisory"), PackageVulnerabilitySeverity.Critical, VersionRange.Parse("[1.0.0]"))
        });

        mockServer.Start();

        var nugetConfig = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <clear />
    <add key=""mainSource"" value=""{testContext.PackageSource}"" />
  </packageSources>
  <auditSources>
    <clear />
    <add key=""auditSource"" value=""{mockServer.ServiceIndexUri}"" allowInsecureConnections=""true"" />
  </auditSources>
</configuration>";
        File.WriteAllText(Path.Combine(testContext.SolutionRoot, "nuget.config"), nugetConfig);

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = await packageUpdateIO.GetKnownVulnerabilitiesAsync(
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();

        var vulnerabilityDict = result[0];
        vulnerabilityDict.Should().ContainKey(packageId);

        var packageVulnerabilities = vulnerabilityDict[packageId];
        packageVulnerabilities.Should().HaveCount(1);
        packageVulnerabilities[0].Url.Should().Be(new Uri("https://example.com/security-advisory"));
        packageVulnerabilities[0].Severity.Should().Be(PackageVulnerabilitySeverity.Critical);
        packageVulnerabilities[0].Versions.Should().Be(VersionRange.Parse("[1.0.0]"));
    }
}

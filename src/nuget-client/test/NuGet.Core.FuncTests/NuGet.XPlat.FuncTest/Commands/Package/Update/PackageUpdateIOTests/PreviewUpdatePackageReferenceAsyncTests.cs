// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest.Commands.Package.Update.PackageUpdateIOTests;

public class PreviewUpdatePackageReferenceAsyncTests
{
    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance);
        var packageUpdateIO = new PackageUpdateIO(solutionRoot, msbuildUtility, TestEnvironmentVariableReader.EmptyInstance);
        return packageUpdateIO;
    }

    [Fact]
    public async Task PreviewUpdatePackageReferenceAsync_WithValidProjects_ReturnsSuccessfulResult()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();

        var packageA = new SimpleTestPackageContext("PackageA", "1.0.0");
        await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packageA);

        var projectA = SimpleTestProjectContext.CreateNETCore(
            "ProjectA",
            testContext.SolutionRoot,
            "net9.0");
        projectA.AddPackageToAllFrameworks(packageA);
        projectA.GlobalPackagesFolder = testContext.UserPackagesFolder;
        projectA.Sources = new List<NuGet.Configuration.PackageSource> { new NuGet.Configuration.PackageSource(testContext.PackageSource) };
        projectA.FallbackFolders = new List<string> { testContext.FallbackFolder };
        projectA.Save();

        var projectB = SimpleTestProjectContext.CreateNETCore(
            "ProjectB",
            testContext.SolutionRoot,
            "net9.0");
        projectB.AddPackageToAllFrameworks(packageA);
        projectB.GlobalPackagesFolder = testContext.UserPackagesFolder;
        projectB.Sources = new List<NuGet.Configuration.PackageSource> { new NuGet.Configuration.PackageSource(testContext.PackageSource) };
        projectB.FallbackFolders = new List<string> { testContext.FallbackFolder };
        projectB.Save();

        var dgSpec = new NuGet.ProjectModel.DependencyGraphSpec();
        var projectSpecA = projectA.PackageSpec;
        var projectSpecB = projectB.PackageSpec;
        dgSpec.AddProject(projectSpecA);
        dgSpec.AddProject(projectSpecB);
        dgSpec.AddRestore(projectSpecA.RestoreMetadata.ProjectUniqueName);
        dgSpec.AddRestore(projectSpecB.RestoreMetadata.ProjectUniqueName);

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = await packageUpdateIO.PreviewUpdatePackageReferenceAsync(
            dgSpec,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var restoreResult = result.Should().BeOfType<PackageUpdateIO.RestoreResult>().Subject;
        restoreResult.RestoreResultPairs.Should().HaveCount(2);
        restoreResult.RestoreResultPairs.Should().Contain(pair =>
            pair.SummaryRequest.Request.Project.Name == "ProjectA");
        restoreResult.RestoreResultPairs.Should().Contain(pair =>
            pair.SummaryRequest.Request.Project.Name == "ProjectB");
    }

    [Fact]
    public async Task PreviewUpdatePackageReferenceAsync_WithMissingPackage_ReturnsFailedResult()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();

        var packageA = new SimpleTestPackageContext("MissingPackage", "1.0.0");

        var project = SimpleTestProjectContext.CreateNETCore(
            "TestProject",
            testContext.SolutionRoot,
            "net9.0");

        project.AddPackageToAllFrameworks(packageA);
        project.GlobalPackagesFolder = testContext.UserPackagesFolder;
        project.Sources = new List<NuGet.Configuration.PackageSource> { new NuGet.Configuration.PackageSource(testContext.PackageSource) };
        project.FallbackFolders = new List<string> { testContext.FallbackFolder };
        project.Save();

        var dgSpec = new NuGet.ProjectModel.DependencyGraphSpec();
        var projectSpec = project.PackageSpec;
        dgSpec.AddProject(projectSpec);
        dgSpec.AddRestore(projectSpec.RestoreMetadata.ProjectUniqueName);

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        // Act
        var result = await packageUpdateIO.PreviewUpdatePackageReferenceAsync(
            dgSpec,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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

public class GetProjectAssetsFileAsyncTests
{
    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance);
        var packageUpdateIO = new PackageUpdateIO(solutionRoot, msbuildUtility, TestEnvironmentVariableReader.EmptyInstance);
        return packageUpdateIO;
    }

    [Fact]
    public async Task GetProjectAssetsFileAsync_WithValidProject_ReturnsLockFile()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();

        var packageA = new SimpleTestPackageContext("PackageA", "1.0.0");
        await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packageA);

        var project = SimpleTestProjectContext.CreateNETCore(
            "TestProject",
            testContext.SolutionRoot,
            "net9.0");

        project.AddPackageToAllFrameworks(packageA);
        project.Save();

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);
        var dgSpec = packageUpdateIO.GetDependencyGraphSpec(project.ProjectPath);
        dgSpec.Should().NotBeNull();

        // Act
        var lockFile = await packageUpdateIO.GetProjectAssetsFileAsync(
            dgSpec!,
            project.ProjectPath,
            NullLogger.Instance,
            CancellationToken.None);

        // Assert
        lockFile.Should().NotBeNull();
        lockFile.Libraries.Should().ContainSingle(lib => lib.Name == "PackageA" && lib.Version.ToString() == "1.0.0");
        lockFile.PackageSpec.Should().NotBeNull();
        lockFile.PackageSpec.Name.Should().Be("TestProject");
    }
}

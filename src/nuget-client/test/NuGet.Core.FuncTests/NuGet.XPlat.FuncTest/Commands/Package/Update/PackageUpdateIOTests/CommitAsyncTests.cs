// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;

namespace NuGet.XPlat.FuncTest.Commands.Package.Update.PackageUpdateIOTests;

public class CommitAsyncTests
{
    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance);
        var packageUpdateIO = new PackageUpdateIO(solutionRoot, msbuildUtility, TestEnvironmentVariableReader.EmptyInstance);
        return packageUpdateIO;
    }

    [Fact]
    public async Task CommitAsync_AfterSuccessfulPreview_CreatesAssetsFile()
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

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);
        var dgSpec = new DependencyGraphSpec();
        dgSpec.AddProject(projectA.PackageSpec);
        dgSpec.AddProject(projectB.PackageSpec);
        dgSpec.AddRestore(projectA.PackageSpec.RestoreMetadata.ProjectUniqueName);
        dgSpec.AddRestore(projectB.PackageSpec.RestoreMetadata.ProjectUniqueName);

        var previewResult = await packageUpdateIO.PreviewUpdatePackageReferenceAsync(
            dgSpec,
            NullLogger.Instance,
            CancellationToken.None);
        previewResult.Success.Should().BeTrue();

        var assetsFilePathA = projectA.AssetsFileOutputPath;
        if (File.Exists(assetsFilePathA))
        {
            File.Delete(assetsFilePathA);
        }

        var assetsFilePathB = projectB.AssetsFileOutputPath;
        if (File.Exists(assetsFilePathB))
        {
            File.Delete(assetsFilePathB);
        }

        // Act
        await packageUpdateIO.CommitAsync(previewResult, CancellationToken.None);

        // Assert
        File.Exists(assetsFilePathA).Should().BeTrue();

        var lockFileA = new LockFileFormat().Read(assetsFilePathA);
        lockFileA.Should().NotBeNull();
        lockFileA.Libraries.Should().ContainSingle(lib => lib.Name == "PackageA" && lib.Version.ToString() == "1.0.0");

        File.Exists(assetsFilePathB).Should().BeTrue();

        var lockFileB = new LockFileFormat().Read(assetsFilePathB);
        lockFileB.Should().NotBeNull();
        lockFileB.Libraries.Should().ContainSingle(lib => lib.Name == "PackageA" && lib.Version.ToString() == "1.0.0");
    }
}

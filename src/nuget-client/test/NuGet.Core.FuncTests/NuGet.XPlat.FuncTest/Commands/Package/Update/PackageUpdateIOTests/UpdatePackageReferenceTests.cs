// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.Commands.Package.Update;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;
using static NuGet.CommandLine.XPlat.Commands.Package.Update.PackageUpdateCommandRunner;

namespace NuGet.XPlat.FuncTest.Commands.Package.Update.PackageUpdateIOTests;

// PackageUpdateIO uses MSBuildAPIUtility, so just use a few integration tests here, and assume that
// MSBuildAPIUtility is tested extensively elsewhere.
[Collection(XPlatCollection.Name)]
public class UpdatePackageReferenceTests
{
    private static PackageUpdateIO CreatePackageUpdateIO(string solutionRoot)
    {
        var msbuildUtility = new MSBuildAPIUtility(NullLogger.Instance);
        var packageUpdateIO = new PackageUpdateIO(solutionRoot, msbuildUtility, TestEnvironmentVariableReader.EmptyInstance);
        return packageUpdateIO;
    }

    [Fact]
    public async Task UpdatePackageReference_DgSpecWithTwoProjects_UpdatesOneProject()
    {
        // Arrange
        using var testContext = new SimpleTestPathContext();

        var packageA_v1 = new SimpleTestPackageContext("PackageA", "1.0.0");
        var packageA_v2 = new SimpleTestPackageContext("PackageA", "2.0.0");
        await SimpleTestPackageUtility.CreatePackagesAsync(testContext.PackageSource, packageA_v1, packageA_v2);

        var project1 = SimpleTestProjectContext.CreateNETCore(
            "TestProject1",
            testContext.SolutionRoot,
            "net9.0");

        project1.AddPackageToAllFrameworks(packageA_v1);
        project1.Properties.Add("RestorePackagesPath", testContext.UserPackagesFolder);
        project1.Sources = new List<PackageSource> { new PackageSource(testContext.PackageSource) };
        project1.FallbackFolders = new List<string> { testContext.FallbackFolder };
        project1.GlobalPackagesFolder = testContext.UserPackagesFolder;
        project1.Save();

        var project2 = SimpleTestProjectContext.CreateNETCore(
            "TestProject2",
            testContext.SolutionRoot,
            "net9.0");

        project2.AddPackageToAllFrameworks(packageA_v1);
        project2.Properties.Add("RestorePackagesPath", testContext.UserPackagesFolder);
        project2.Sources = new List<PackageSource> { new PackageSource(testContext.PackageSource) };
        project2.FallbackFolders = new List<string> { testContext.FallbackFolder };
        project2.GlobalPackagesFolder = testContext.UserPackagesFolder;
        project2.Save();

        using var packageUpdateIO = CreatePackageUpdateIO(testContext.SolutionRoot);

        var updatedDgSpec = new ProjectModel.DependencyGraphSpec();
        var originalProjectSpec1 = project1.PackageSpec;
        var projectSpec1 = originalProjectSpec1.Clone();
        projectSpec1.RestoreMetadata.Sources = new List<PackageSource> { new PackageSource(testContext.PackageSource) };
        projectSpec1.RestoreMetadata.FallbackFolders = new List<string> { testContext.FallbackFolder };
        projectSpec1.RestoreMetadata.PackagesPath = testContext.UserPackagesFolder;
        var framework1 = projectSpec1.TargetFrameworks.First();
        framework1.Dependencies.Clear();
        framework1.Dependencies.Add(new LibraryModel.LibraryDependency
        {
            LibraryRange = new LibraryModel.LibraryRange(
                "PackageA",
                VersionRange.Parse("2.0.0"),
                NuGet.LibraryModel.LibraryDependencyTarget.Package)
        });
        updatedDgSpec.AddProject(projectSpec1);
        updatedDgSpec.AddRestore(projectSpec1.RestoreMetadata.ProjectUniqueName);

        var originalProjectSpec2 = project2.PackageSpec;
        var projectSpec2 = originalProjectSpec2.Clone();
        projectSpec2.RestoreMetadata.Sources = new List<PackageSource> { new PackageSource(testContext.PackageSource) };
        projectSpec2.RestoreMetadata.FallbackFolders = new List<string> { testContext.FallbackFolder };
        projectSpec2.RestoreMetadata.PackagesPath = testContext.UserPackagesFolder;
        updatedDgSpec.AddProject(projectSpec2);
        updatedDgSpec.AddRestore(projectSpec2.RestoreMetadata.ProjectUniqueName);

        var previewResult = await packageUpdateIO.PreviewUpdatePackageReferenceAsync(
            updatedDgSpec,
            NullLogger.Instance,
            CancellationToken.None);
        previewResult.Success.Should().BeTrue();

        var packageToUpdate = new PackageToUpdate
        {
            Id = "PackageA",
            CurrentVersion = VersionRange.Parse("1.0.0"),
            NewVersion = VersionRange.Parse("2.0.0")
        };

        var tfmAliases = projectSpec1.TargetFrameworks.Select(tf => tf.TargetAlias).ToList();

        // Act
        packageUpdateIO.UpdatePackageReference(
            projectSpec1,
            previewResult,
            tfmAliases,
            packageToUpdate,
            NullLogger.Instance);

        // Assert
        var project1Xml = XDocument.Load(project1.ProjectPath);
        var ns = project1Xml.Root!.GetDefaultNamespace();
        var project1PackageReferences = project1Xml.Descendants(ns + "PackageReference")
            .Where(e => e.Attribute("Include")?.Value == "PackageA")
            .ToList();

        project1PackageReferences.Should().ContainSingle();
        var project1PackageRef = project1PackageReferences.First();
        project1PackageRef.Attribute("Version")?.Value.Should().Be("2.0.0");

        var project2Xml = XDocument.Load(project2.ProjectPath);
        var project2PackageReferences = project2Xml.Descendants(ns + "PackageReference")
            .Where(e => e.Attribute("Include")?.Value == "PackageA")
            .ToList();

        project2PackageReferences.Should().ContainSingle();
        var project2PackageRef = project2PackageReferences.First();
        project2PackageRef.Attribute("Version")?.Value.Should().Be("1.0.0");
    }
}

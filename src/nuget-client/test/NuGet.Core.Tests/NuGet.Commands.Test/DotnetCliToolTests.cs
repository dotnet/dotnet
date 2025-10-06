// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class DotnetCliToolTests
    {
        [Fact]
        public async Task DotnetCliTool_VerifyProjectsAreNotAllowed()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var dgFile = new DependencyGraphSpec();

                var spec = ToolRestoreUtility.GetSpec(
                    Path.Combine(pathContext.SolutionRoot, "tool", "fake.csproj"),
                    "a",
                    VersionRange.Parse("1.0.0"),
                    NuGetFramework.Parse("netcoreapp1.0"),
                    pathContext.UserPackagesFolder,
                    new List<string>() { pathContext.FallbackFolder },
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    projectWideWarningProperties: null);

                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.Name);

                var pathResolver = new ToolPathResolver(pathContext.UserPackagesFolder);
                var path = pathResolver.GetLockFilePath(
                    "a",
                    NuGetVersion.Parse("1.0.0"),
                    NuGetFramework.Parse("netcoreapp1.0"));

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "a",
                    Version = "1.0.0"
                };

                var packageB = new SimpleTestPackageContext()
                {
                    Id = "b",
                    Version = "1.0.0"
                };

                packageA.Dependencies.Add(packageB);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageA,
                    packageB);

                var projectYRoot = Path.Combine(pathContext.SolutionRoot, "b");
                Directory.CreateDirectory(projectYRoot);
                var projectYJson = Path.Combine(projectYRoot, "project.json");

                var projectJsonContent = JObject.Parse(@"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                        'netstandard1.0': {
                                                    }
                                                  }
                                               }");

                File.WriteAllText(projectYJson, projectJsonContent.ToString());

                // Act
                var result = await CommandsTestUtility.RunSingleRestore(dgFile, pathContext, logger);

                // Assert
                Assert.True(result.Success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.True(File.Exists(path));

                var lockFormat = new LockFileFormat();
                var lockFile = lockFormat.Read(path);

                // Verify only packages
                Assert.DoesNotContain(lockFile.Libraries, e => e.Type != "package");
            }
        }

        [Fact]
        public void DotnetCliTool_VerifyPackageSpecWithoutWarningProperties()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Act
                var spec = ToolRestoreUtility.GetSpec(
                    Path.Combine(pathContext.SolutionRoot, "tool", "fake.csproj"),
                    "a",
                    VersionRange.Parse("1.0.0"),
                    NuGetFramework.Parse("netcoreapp1.0"),
                    pathContext.UserPackagesFolder,
                    new List<string>() { pathContext.FallbackFolder },
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    projectWideWarningProperties: null);

                // Assert
                spec.RestoreMetadata.ProjectWideWarningProperties.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task DotnetCliTool_BasicToolRestore()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var dgFile = new DependencyGraphSpec();

                var spec = ToolRestoreUtility.GetSpec(
                    Path.Combine(pathContext.SolutionRoot, "fake.csproj"),
                    "a",
                    VersionRange.Parse("1.0.0"),
                    NuGetFramework.Parse("netcoreapp1.0"),
                    pathContext.UserPackagesFolder,
                    new List<string>() { pathContext.FallbackFolder },
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    projectWideWarningProperties: null);

                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.Name);

                var pathResolver = new ToolPathResolver(pathContext.UserPackagesFolder);
                var path = pathResolver.GetLockFilePath(
                    "a",
                    NuGetVersion.Parse("1.0.0"),
                    NuGetFramework.Parse("netcoreapp1.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                // Act
                var result = await CommandsTestUtility.RunSingleRestore(dgFile, pathContext, logger);

                // Assert
                Assert.True(result.Success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.True(File.Exists(path));
            }
        }

        [Fact]
        public async Task DotnetCliTool_ToolRestoreNoOpsRegardlessOfProject()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger1 = new TestLogger();
                var logger2 = new TestLogger();

                var spec1 = ToolRestoreUtility.GetSpec(
                    Path.Combine(pathContext.SolutionRoot, "fake1.csproj"),
                    "a",
                    VersionRange.Parse("1.0.0"),
                    NuGetFramework.Parse("netcoreapp1.0"),
                                        pathContext.UserPackagesFolder,
                    new List<string>() { pathContext.FallbackFolder },
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    projectWideWarningProperties: null);

                var spec2 = ToolRestoreUtility.GetSpec(
                    Path.Combine(pathContext.SolutionRoot, "fake2.csproj"),
                    "a",
                    VersionRange.Parse("1.0.0"),
                    NuGetFramework.Parse("netcoreapp1.0"),
                                        pathContext.UserPackagesFolder,
                    new List<string>() { pathContext.FallbackFolder },
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    projectWideWarningProperties: null);

                var dgFile1 = new DependencyGraphSpec();
                dgFile1.AddProject(spec1);
                dgFile1.AddRestore(spec1.Name);

                var dgFile2 = new DependencyGraphSpec();
                dgFile2.AddProject(spec2);
                dgFile2.AddRestore(spec2.Name);

                var pathResolver = new ToolPathResolver(pathContext.UserPackagesFolder);
                var path = pathResolver.GetLockFilePath(
                    "a",
                    NuGetVersion.Parse("1.0.0"),
                    NuGetFramework.Parse("netcoreapp1.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                // Act
                var results1 = await CommandsTestUtility.RunRestore(dgFile1, pathContext, logger1);

                // Assert
                Assert.Equal(1, results1.Count);
                var result1 = results1.Single();

                Assert.True(result1.Success, "Failed: " + string.Join(Environment.NewLine, logger1.Messages));
                Assert.False(result1.NoOpRestore, "Should not no-op: " + string.Join(Environment.NewLine, logger1.Messages));
                Assert.True(File.Exists(path));

                // Act
                var results2 = await CommandsTestUtility.RunRestore(dgFile2, pathContext, logger2);

                // Assert
                Assert.Equal(1, results2.Count);
                var result2 = results2.Single();

                Assert.True(result2.Success, "Failed: " + string.Join(Environment.NewLine, logger2.Messages));
                Assert.True(result2.NoOpRestore, "Should no-op: " + string.Join(Environment.NewLine, logger2.Messages));
                Assert.True(File.Exists(path));
            }
        }

        [Fact]
        public async Task DotnetCliTool_BasicToolRestore_WithDuplicates()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var dgFile = new DependencyGraphSpec();

                for (int i = 0; i < 10; i++)
                {
                    var spec = ToolRestoreUtility.GetSpec(
                        Path.Combine(pathContext.SolutionRoot, "fake.csproj"),
                        "a",
                        VersionRange.Parse("1.0.0"),
                        NuGetFramework.Parse("netcoreapp1.0"),
                                            pathContext.UserPackagesFolder,
                    new List<string>() { pathContext.FallbackFolder },
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    projectWideWarningProperties: null);

                    dgFile.AddProject(spec);
                    dgFile.AddRestore(spec.Name);
                }

                var pathResolver = new ToolPathResolver(pathContext.UserPackagesFolder);
                var path = pathResolver.GetLockFilePath(
                    "a",
                    NuGetVersion.Parse("1.0.0"),
                    NuGetFramework.Parse("netcoreapp1.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, new PackageIdentity("a", NuGetVersion.Parse("1.0.0")));

                // Act
                var results = await CommandsTestUtility.RunRestore(dgFile, pathContext, logger);

                // Assert
                // This should have been de-duplicated
                Assert.Equal(1, results.Count);
                var result = results.Single();

                Assert.True(result.Success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                Assert.True(File.Exists(path));
            }
        }

        [Fact]
        public async Task DotnetCliTool_BasicToolRestore_DifferentVersionRanges()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var dgFile = new DependencyGraphSpec();

                var versions = new List<VersionRange>();

                var limit = 100;

                for (int i = 0; i < limit; i++)
                {
                    var version = VersionRange.Parse($"{i + 1}.0.0");
                    versions.Add(version);

                    var spec = ToolRestoreUtility.GetSpec(
                        Path.Combine(pathContext.SolutionRoot, $"fake{i}.csproj"),
                        "a",
                        version,
                        NuGetFramework.Parse("netcoreapp1.0"),
                                            pathContext.UserPackagesFolder,
                    new List<string>() { pathContext.FallbackFolder },
                    new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                    projectWideWarningProperties: null);

                    dgFile.AddProject(spec);
                    dgFile.AddRestore(spec.Name);
                }

                var pathResolver = new ToolPathResolver(pathContext.UserPackagesFolder);

                foreach (var version in versions)
                {
                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, new PackageIdentity("a", version.MinVersion));
                }

                // Act
                var results = await CommandsTestUtility.RunRestore(dgFile, pathContext, logger);

                // Assert
                Assert.Equal(limit, results.Count);

                foreach (var result in results)
                {
                    Assert.True(result.Success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));
                }

                foreach (var version in versions)
                {
                    var path = pathResolver.GetLockFilePath(
                        "a",
                        version.MinVersion,
                        NuGetFramework.Parse("netcoreapp1.0"));

                    Assert.True(File.Exists(path), $"{path} does not exist!");
                }
            }
        }

        [Theory]
        [InlineData("tool", "netcoreapp1.0", "1.0.0", "tool-netcoreapp1.0-[1.0.0, )")]
        [InlineData("Tool", "netcoreapp1.0", "1.0.0", "tool-netcoreapp1.0-[1.0.0, )")]
        [InlineData("tOOl", "NetCoreapp1.0", "1.0.0", "tool-netcoreapp1.0-[1.0.0, )")]
        public void DotnetCliTool_TestGetUniqueName(string name, string framework, string version, string expected)
        {
            Assert.Equal(expected, ToolRestoreUtility.GetUniqueName(name, framework, VersionRange.Parse(version)));
        }

        [Fact]
        public void DotnetCliTool_TestGetUniqueName_VersionRangeAll()
        {
            Assert.Equal("tool-netcoreapp1.0-(, )", ToolRestoreUtility.GetUniqueName("tool", "netcoreapp1.0", VersionRange.All));
        }
    }
}

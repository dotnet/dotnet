// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.CommandLineUtils;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    [Collection("NuGet XPlat Test Collection")]
    public class XPlatAddPkgTests
    {
        private static readonly string ProjectName = "test_project_addpkg";

        private readonly ITestOutputHelper _testOutputHelper;

        public XPlatAddPkgTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        // Argument parsing related tests

        [Theory]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "--dg-file", "dgfile_foo", "--project", "project_foo.csproj", "", "", "", "", "", "", "", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "", "", "", "", "", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "--framework", "net46;netcoreapp1.0", "", "", "", "", "", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "-f", "net46 ; netcoreapp1.0 ; ", "", "", "", "", "", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "-f", "net46", "", "", "", "", "", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "--source", "a;b", "", "", "", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "-s", "a ; b ;", "", "", "", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "-s", "a", "", "", "", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "", "", "--package-directory", @"foo\dir", "", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "", "", "", "", "--no-restore", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "", "", "", "", "-n", "", "")]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "", "", "", "", "-n", "--interactive", "")]
        [InlineData("--package", "package_foo", "", "", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "", "", "", "", "", "", "--prerelease")]
        public void AddPkg_ArgParsing(string packageOption, string package, string versionOption, string version, string dgFileOption,
            string dgFilePath, string projectOption, string project, string frameworkOption, string frameworkString, string sourceOption,
            string sourceString, string packageDirectoryOption, string packageDirectory, string noRestoreSwitch, string interactiveSwitch, string prereleaseOption)
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectPath = Path.Combine(testDirectory, project);
                var frameworks = MSBuildStringUtility.Split(frameworkString);
                var sources = MSBuildStringUtility.Split(sourceString);
                File.Create(projectPath).Dispose();

                var argList = new List<string>() {
                "add",
                packageOption,
                package,
                dgFileOption,
                dgFilePath,
                projectOption,
                projectPath};

                if (!string.IsNullOrEmpty(versionOption))
                {
                    argList.Add(versionOption);
                }
                if (!string.IsNullOrEmpty(version))
                {
                    argList.Add(version);
                }
                if (!string.IsNullOrEmpty(frameworkOption))
                {
                    foreach (var framework in frameworks)
                    {
                        argList.Add(frameworkOption);
                        argList.Add(framework);
                    }
                }
                if (!string.IsNullOrEmpty(sourceOption))
                {
                    foreach (var source in sources)
                    {
                        argList.Add(sourceOption);
                        argList.Add(source);
                    }
                }
                if (!string.IsNullOrEmpty(packageDirectoryOption))
                {
                    argList.Add(packageDirectoryOption);
                    argList.Add(packageDirectory);
                }
                if (!string.IsNullOrEmpty(noRestoreSwitch))
                {
                    argList.Add(noRestoreSwitch);
                }
                if (!string.IsNullOrEmpty(interactiveSwitch))
                {
                    argList.Add(interactiveSwitch);
                }
                if (!string.IsNullOrEmpty(prereleaseOption))
                {
                    argList.Add(prereleaseOption);
                }

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var testApp = new CommandLineApplication();
                var mockCommandRunner = new Mock<IPackageReferenceCommandRunner>();
                mockCommandRunner
                    .Setup(m => m.ExecuteCommand(It.IsAny<PackageReferenceArgs>(), It.IsAny<MSBuildAPIUtility>()))
                    .ReturnsAsync(0);

                testApp.Name = "dotnet nuget_test";
                AddPackageReferenceCommand.Register(testApp,
                    () => logger,
                    () => mockCommandRunner.Object);

                // Act
                var result = testApp.Execute(argList.ToArray());

                XPlatTestUtils.DisposeTemporaryFile(projectPath);

                // Assert
                mockCommandRunner.Verify(m => m.ExecuteCommand(It.Is<PackageReferenceArgs>(p => p.PackageId == package &&
                (!string.IsNullOrEmpty(versionOption) == string.IsNullOrEmpty(prereleaseOption) ||
                    (string.IsNullOrEmpty(versionOption) == !string.IsNullOrEmpty(prereleaseOption) && p.PackageVersion == version)) &&
                p.ProjectPath == projectPath &&
                p.DgFilePath == dgFilePath &&
                p.NoRestore == !string.IsNullOrEmpty(noRestoreSwitch) &&
                (string.IsNullOrEmpty(frameworkOption) || !string.IsNullOrEmpty(frameworkOption) && p.Frameworks.SequenceEqual(frameworks)) &&
                (string.IsNullOrEmpty(sourceOption) || !string.IsNullOrEmpty(sourceOption) && p.Sources.SequenceEqual(MSBuildStringUtility.Split(sourceString))) &&
                (string.IsNullOrEmpty(packageDirectoryOption) || !string.IsNullOrEmpty(packageDirectoryOption) && p.PackageDirectory == packageDirectory) &&
                p.Interactive == !string.IsNullOrEmpty(interactiveSwitch) &&
                p.Prerelease == !string.IsNullOrEmpty(prereleaseOption)),
                It.IsAny<MSBuildAPIUtility>()));

                Assert.Equal(0, result);
            }
        }

        [Theory]
        [InlineData("--package", "package_foo", "--version", "1.0.0-foo", "-d", "dgfile_foo", "-p", "project_foo.csproj", "", "", "", "", "", "", "", "", "--prerelease")]
        public void AddPkg_Error_ArgParsingPrerelease(string packageOption, string package, string versionOption, string version, string dgFileOption,
            string dgFilePath, string projectOption, string project, string frameworkOption, string frameworkString, string sourceOption,
            string sourceString, string packageDirectoryOption, string packageDirectory, string noRestoreSwitch, string interactiveSwitch, string prereleaseOption)
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectPath = Path.Combine(testDirectory, project);
                var frameworks = MSBuildStringUtility.Split(frameworkString);
                var sources = MSBuildStringUtility.Split(sourceString);
                File.Create(projectPath).Dispose();

                var argList = new List<string>() {
                "add",
                packageOption,
                package,
                dgFileOption,
                dgFilePath,
                projectOption,
                projectPath};

                if (!string.IsNullOrEmpty(versionOption))
                {
                    argList.Add(versionOption);
                }
                if (!string.IsNullOrEmpty(version))
                {
                    argList.Add(version);
                }
                if (!string.IsNullOrEmpty(frameworkOption))
                {
                    foreach (var framework in frameworks)
                    {
                        argList.Add(frameworkOption);
                        argList.Add(framework);
                    }
                }
                if (!string.IsNullOrEmpty(sourceOption))
                {
                    foreach (var source in sources)
                    {
                        argList.Add(sourceOption);
                        argList.Add(source);
                    }
                }
                if (!string.IsNullOrEmpty(packageDirectoryOption))
                {
                    argList.Add(packageDirectoryOption);
                    argList.Add(packageDirectory);
                }
                if (!string.IsNullOrEmpty(noRestoreSwitch))
                {
                    argList.Add(noRestoreSwitch);
                }
                if (!string.IsNullOrEmpty(interactiveSwitch))
                {
                    argList.Add(interactiveSwitch);
                }
                if (!string.IsNullOrEmpty(prereleaseOption))
                {
                    argList.Add(prereleaseOption);
                }

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var testApp = new CommandLineApplication();
                var mockCommandRunner = new Mock<IPackageReferenceCommandRunner>();
                mockCommandRunner
                    .Setup(m => m.ExecuteCommand(It.IsAny<PackageReferenceArgs>(), It.IsAny<MSBuildAPIUtility>()))
                    .ReturnsAsync(0);

                testApp.Name = "dotnet nuget_test";
                AddPackageReferenceCommand.Register(testApp,
                    () => logger,
                    () => mockCommandRunner.Object);

                // Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => testApp.Execute(argList.ToArray()));
                Assert.Equal(Strings.Error_PrereleaseWhenVersionSpecified, exception.Message);
                XPlatTestUtils.DisposeTemporaryFile(projectPath);
            }
        }

        // Add Related Tests

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("*")]
        [InlineData("1.*")]
        [InlineData("1.0.*")]
        [InlineData("[1.0.*, )")]
        public async Task AddPkg_UnconditionalAdd_Success(string userInputVersion)
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");
                var packageX = XPlatTestUtils.CreatePackage();

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, userInputVersion, projectA);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                var itemGroup = XPlatTestUtils.GetItemGroupForAllFrameworks(projectXmlRoot);

                // Assert
                Assert.Equal(0, result);
                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageX.Id, userInputVersion));
                Assert.True(XPlatTestUtils.ValidateAssetsFile(projectA, packageX.Id));
            }
        }

        public static readonly List<object[]> AddPkg_PackageVersionsLatestPrereleaseSucessData
            = new List<object[]>
            {
                    new object[] { new string[] { "0.0.5", "0.9.0", "1.0.0-preview.3" }, "1.0.0-preview.3", true },
                    new object[] { new string[] { "0.0.5", "0.9.0", "1.0.0-preview.3", "1.1.1-preview.7" }, "1.1.1-preview.7", true },
                    new object[] { new string[] { "0.0.5", "0.9.0", "1.0.0" }, "1.0.0", true },
                    new object[] { new string[] { "0.0.5", "0.9.0", "1.0.0-preview.3", "2.0.0" }, "2.0.0", true },
                    new object[] { new string[] { "0.0.5", "0.9.0", "1.0.0-preview.3" }, "0.9.0", false },
                    new object[] { new string[] { "0.0.5", "0.9.0", "1.0.0-preview.3", "1.0.0" }, "1.0.0", false },
                    new object[] { new string[] { "0.0.5", "0.9.0", "1.0.0" }, "1.0.0", false },
                    new object[] { new string[] { "1.0.0-preview.1", "1.0.0-preview.2", "1.0.0-preview.3" }, "1.0.0-preview.3", true },
            };

        [Theory]
        [MemberData(nameof(AddPkg_PackageVersionsLatestPrereleaseSucessData))]
        public async Task AddPkg_UnconditionalAddPrereleaseSuccess(string[] inputVersions, string expectedVersion, bool prerelease)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46; netcoreapp1.0");
                var packages = inputVersions.Select(e => XPlatTestUtils.CreatePackage(packageVersion: e)).ToArray();
                var logger = new TestCommandOutputLogger(_testOutputHelper);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packages);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packages[0].Id, "*", projectA, noVersion: true, prerelease: prerelease);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                // Assert
                Assert.Equal(0, result);
                Assert.True(XPlatTestUtils.ValidateReference(projectXmlRoot, packages[0].Id, expectedVersion), projectXmlRoot.ToString());
            }
        }

        public static readonly List<object[]> AddPkg_PackageVersionsLatestPrereleasNoStableAvailableData
            = new List<object[]>
            {
                    new object[] { new string[] { "1.0.0-preview.1", "1.0.0-preview.2", "1.0.0-preview.3" }, false },
            };

        [Theory]
        [MemberData(nameof(AddPkg_PackageVersionsLatestPrereleasNoStableAvailableData))]
        public async Task AddPkg_NoStablePackageAvailable(string[] inputVersions, bool prerelease)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46; netcoreapp1.0");
                var packages = inputVersions.Select(e => XPlatTestUtils.CreatePackage(packageVersion: e)).ToArray();

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packages);

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packages[0].Id, "*", projectA, noVersion: true, prerelease: prerelease);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act & Assert
                var result = await Assert.ThrowsAsync<CommandException>(() => commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger)));
                Assert.Equal(string.Format(CultureInfo.CurrentCulture, Strings.PrereleaseVersionsAvailable, packages.Max(x => x.Identity.Version)), result.Message);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task AddPkg_NoVersionsAvailable(bool prerelease)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46; netcoreapp1.0");
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    XPlatTestUtils.CreatePackage(packageVersion: "1.0.0"));


                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, "packageY", "*", projectA, noVersion: true, prerelease: prerelease);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act & Assert
                var result = await Assert.ThrowsAsync<CommandException>(() => commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger)));
                Assert.Equal(string.Format(CultureInfo.CurrentCulture, Strings.Error_NoVersionsAvailable, "packageY"), result.Message);
            }
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("*")]
        [InlineData("1.*")]
        [InlineData("1.0.*")]
        public async Task AddPkg_UnconditionalAddIntoExeProject_Success(string userInputVersion)
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");
                projectA.Properties.Add("OutputType", "exe");
                projectA.Save();

                var packageX = XPlatTestUtils.CreatePackage();
                var logger = new TestCommandOutputLogger(_testOutputHelper);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, userInputVersion, projectA);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                var itemGroup = XPlatTestUtils.GetItemGroupForAllFrameworks(projectXmlRoot);

                // Assert
                Assert.Equal(0, result);
                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageX.Id, userInputVersion));
                Assert.True(XPlatTestUtils.ValidateAssetsFile(projectA, packageX.Id));
            }
        }

        [Theory]
        [InlineData("1.0.0")]
        [InlineData("*")]
        [InlineData("1.*")]
        [InlineData("1.0.*")]
        public async Task AddPkg_UnconditionalAddWithDotnetCliTool_Success(string userInputVersion)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Generate DotNetCliToolReference Package
                var packageDotnetCliToolX = XPlatTestUtils.CreatePackage(packageId: "PackageDotnetCliToolX",
                    packageType: PackageType.DotnetCliTool);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageDotnetCliToolX);

                // Generate test package
                var packageY = XPlatTestUtils.CreatePackage(packageId: "PackageY");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageY);

                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");

                projectA.DotnetCLIToolReferences.Add(packageDotnetCliToolX);

                projectA.Save();

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                // Verify that the package reference exists before removing.
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                var itemGroup = XPlatTestUtils.GetItemGroupForAllFrameworks(projectXmlRoot, packageType: PackageType.DotnetCliTool);

                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageDotnetCliToolX.Id, "1.0.0", PackageType.DotnetCliTool));

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageY.Id, userInputVersion, projectA, noRestore: false);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                itemGroup = XPlatTestUtils.GetItemGroupForAllFrameworks(projectXmlRoot);

                // Assert
                Assert.Equal(0, result);
                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageY.Id, userInputVersion));
            }
        }

        [Theory]
        [InlineData("net46", "net46; netcoreapp1.0", "1.*")]
        [InlineData("net46; netcoreapp1.0", "net46; netcoreapp1.0", "1.*")]
        [InlineData("netcoreapp1.0", "net46; netcoreapp1.0", "1.*")]
        [InlineData("net46", "net46; netcoreapp2.0", "1.*")]
        [InlineData("net46; netcoreapp2.0", "net46; netcoreapp2.0", "1.*")]
        [InlineData("netcoreapp2.0", "net46; netcoreapp2.0", "1.*")]
        [InlineData("net46", "net46; netstandard2.0", "1.*")]
        [InlineData("net46; netstandard2.0", "net46; netstandard2.0", "1.*")]
        [InlineData("netstandard2.0", "net46; netstandard2.0", "1.*")]
        public async Task AddPkg_UnconditionalAddWithNoRestore_Success(string packageFrameworks,
            string projectFrameworks,
            string userInputVersion)
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var packageX = XPlatTestUtils.CreatePackage(frameworkString: packageFrameworks);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, userInputVersion, projectA, noRestore: true);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                // If noRestore is set, then we do not perform compatibility check.
                // The added package reference will be unconditional
                var itemGroup = XPlatTestUtils.GetItemGroupForAllFrameworks(projectXmlRoot);

                // Assert
                Assert.Equal(0, result);
                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageX.Id, userInputVersion));
            }
        }

        [Theory]
        [InlineData("net46; netcoreapp1.0", "1.*")]
        [InlineData("net46; netcoreapp2.0", "1.*")]
        [InlineData("net46; netstandard2.0", "1.*")]
        public async Task AddPkg_UnconditionalAddWithDotnetCliToolAndNoRestore_Success(string projectFrameworks,
            string userInputVersion)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Generate DotNetCliToolReference Package
                var packageDotnetCliToolX = XPlatTestUtils.CreatePackage(packageId: "PackageDotnetCliToolX",
                    packageType: PackageType.DotnetCliTool);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageDotnetCliToolX);

                // Generate test package
                var packageY = XPlatTestUtils.CreatePackage(packageId: "PackageY");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageY);

                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);

                projectA.DotnetCLIToolReferences.Add(packageDotnetCliToolX);

                projectA.Save();

                // Verify that the package reference exists before removing.
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                var itemGroup = XPlatTestUtils.GetItemGroupForAllFrameworks(projectXmlRoot, packageType: PackageType.DotnetCliTool);
                var logger = new TestCommandOutputLogger(_testOutputHelper);

                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageDotnetCliToolX.Id, "1.0.0", PackageType.DotnetCliTool));

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageY.Id, userInputVersion, projectA, noRestore: true);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                itemGroup = XPlatTestUtils.GetItemGroupForAllFrameworks(projectXmlRoot);

                // Assert
                Assert.Equal(0, result);
                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageY.Id, userInputVersion));
            }
        }

        [Fact]
        public async Task AddPkg_UnconditionalAddWithoutVersion_Success()
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");
                var packageX = XPlatTestUtils.CreatePackage();

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                // Since user is not inputing a version, it is converted to a "*"
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, "*", projectA, noVersion: true);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                // Assert
                Assert.Equal(0, result);

                // Since user did not specify a version, the package reference will contain the resolved version
                Assert.True(XPlatTestUtils.ValidateReference(projectXmlRoot, packageX.Id, "1.0.0"));
            }
        }

        [Theory]
        [InlineData("net46", "net46; netcoreapp1.0", "1.0.0")]
        [InlineData("net46", "net46; netcoreapp1.0", "*")]
        [InlineData("net46", "net46; netcoreapp2.0", "1.0.0")]
        [InlineData("net46", "net46; netcoreapp2.0", "*")]
        [InlineData("net46", "net46; netstandard2.0", "1.0.0")]
        [InlineData("net46", "net46; netstandard2.0", "*")]
        public async Task AddPkg_ConditionalAddWithoutUserInputFramework_Success(string packageFrameworks,
            string projectFrameworks, string userInputVersion)
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var packageX = XPlatTestUtils.CreatePackage(frameworkString: packageFrameworks);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, userInputVersion, projectA);
                var commandRunner = new AddPackageReferenceCommandRunner();
                var commonFramework = XPlatTestUtils.GetCommonFramework(packageFrameworks, projectFrameworks);

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                var itemGroup = XPlatTestUtils.GetItemGroupForFramework(projectXmlRoot, commonFramework);

                // Assert
                Assert.Equal(0, result);
                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageX.Id, userInputVersion));
            }
        }

        [Theory]
        [InlineData("net46", "net46; netcoreapp1.0", "net46")]
        [InlineData("net46; netcoreapp1.0", "net46; netcoreapp1.0", "net46")]
        [InlineData("net46; netcoreapp1.0", "net46; netcoreapp1.0", "netcoreapp1.0")]
        [InlineData("net46", "net46; netcoreapp1.0", "net46; netcoreapp1.0")]
        [InlineData("netcoreapp1.0", "net46; netcoreapp1.0", "net46; netcoreapp1.0")]
        [InlineData("net46", "net46; netcoreapp2.0", "net46")]
        [InlineData("net46; netcoreapp2.0", "net46; netcoreapp2.0", "net46")]
        [InlineData("net46; netcoreapp2.0", "net46; netcoreapp2.0", "netcoreapp2.0")]
        [InlineData("net46", "net46; netcoreapp2.0", "net46; netcoreapp2.0")]
        [InlineData("netcoreapp2.0", "net461; netcoreapp2.0", "net461;  netcoreapp2.0")]
        [InlineData("net46", "net46; netstandard2.0", "net46")]
        [InlineData("net46; netstandard2.0", "net46; netstandard2.0", "net46")]
        [InlineData("net46; netstandard2.0", "net46; netstandard2.0", "netstandard2.0")]
        [InlineData("net46", "net46; netstandard2.0", "net46; netstandard2.0")]
        public async Task AddPkg_ConditionalAddWithUserInputFramework_Success(string packageFrameworks, string projectFrameworks, string userInputFrameworks)
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var packageX = XPlatTestUtils.CreatePackage(frameworkString: packageFrameworks);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, packageX.Version, projectA,
                    frameworks: userInputFrameworks);
                var commandRunner = new AddPackageReferenceCommandRunner();
                var commonFramework = XPlatTestUtils.GetCommonFramework(packageFrameworks, projectFrameworks, userInputFrameworks);

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                var itemGroup = XPlatTestUtils.GetItemGroupForFramework(projectXmlRoot, commonFramework);

                // Assert
                Assert.Equal(0, result);
                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageX.Id, packageX.Version));
            }
        }

        [Theory]
        [InlineData("net46", "net46; netcoreapp1.0", ".NETFramework,Version=v4.7.2;NetCoreApp,Version=v1.0", "Windows,Version=7.0;,Version=", "net46")]
        [InlineData("netcoreapp2.0", "net46;netcoreapp20;net50", ".NETFramework,Version=v4.6;NetCoreApp,Version=v2.0;NetCoreApp,Version=v5.0", "Windows,Version=7.0;,Version=;,Version=", "netcoreapp20;net50")]
        public async Task AddPkg_ConditionalWithAlias_Success(
            string packageFrameworks,
            string projectFrameworks,
            string projectTargetFrameworkMonikers,
            string projectTargetPlatforMonikers,
            string expectedConditions)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var userInputVersion = "1.0.0";
                var actualProjectFrameworks = MSBuildStringUtility.Split(projectFrameworks);
                var actualProjectTargetFrameworkMonikers = MSBuildStringUtility.Split(projectTargetFrameworkMonikers);
                var actualProjectTargetPlatforMonikers = MSBuildStringUtility.Split(projectTargetPlatforMonikers);
                var settings = Settings.LoadDefaultSettings(Path.GetDirectoryName(pathContext.NuGetConfig), Path.GetFileName(pathContext.NuGetConfig), null);
                var project = SimpleTestProjectContext.CreateNETCoreWithSDK(
                        projectName: ProjectName,
                        solutionRoot: pathContext.SolutionRoot,
                        frameworks: MSBuildStringUtility.Split(projectFrameworks));

                for (int i = 0; i < actualProjectFrameworks.Length; i++)
                {
                    var framework = project.Frameworks.Single(e => e.TargetAlias.Equals(actualProjectFrameworks[i]));
                    framework.Properties.Add("TargetFrameworkMoniker", actualProjectTargetFrameworkMonikers[i]);
                    framework.Properties.Add("TargetPlatformMoniker", actualProjectTargetPlatforMonikers[i]);
                }

                project.FallbackFolders = (IList<string>)SettingsUtility.GetFallbackPackageFolders(settings);
                project.GlobalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(settings);
                var packageSourceProvider = new PackageSourceProvider(settings);
                project.Sources = packageSourceProvider.LoadPackageSources();

                project.Save();

                var packageX = XPlatTestUtils.CreatePackage(frameworkString: packageFrameworks);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, userInputVersion, project);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(project.ProjectPath).Root;

                // Assert
                Assert.Equal(0, result);

                foreach (var framework in MSBuildStringUtility.Split(expectedConditions))
                {
                    var itemGroup = XPlatTestUtils.GetItemGroupForFramework(projectXmlRoot, framework);
                    Assert.NotNull(itemGroup);
                    Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageX.Id, userInputVersion));
                }
            }
        }

        [Fact]
        public async Task AddPkg_V3LocalSourceFeed_WithAbsolutePath_NoVersionSpecified_Success()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectFrameworks = "net472";
                var packageFrameworks = "net472; netcoreapp2.0";
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var packageX = "packageX";
                var packageX_V1 = new PackageIdentity(packageX, new NuGetVersion("1.0.0"));
                var packageX_V2 = new PackageIdentity(packageX, new NuGetVersion("2.0.0"));
                var packageX_V1_Context = XPlatTestUtils.CreatePackage(packageX_V1.Id, packageX_V1.Version.Version.ToString(), frameworkString: packageFrameworks);
                var packageX_V2_Context = XPlatTestUtils.CreatePackage(packageX_V2.Id, packageX_V2.Version.Version.ToString(), frameworkString: packageFrameworks);
                var customSourcePath = Path.Combine(pathContext.WorkingDirectory, "Custompackages");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    customSourcePath,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext[] { packageX_V1_Context, packageX_V2_Context });

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                // Since user is not inputing a version, it is converted to a " * " in the command
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX_V1.Id, "*",
                    projectA,
                    sources: customSourcePath);

                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));

                // Assert
                Assert.Equal(0, result);

                // Make sure source is replaced in generated dgSpec file.
                PackageSpec packageSpec = projectA.AssetsFile.PackageSpec;
                string[] sources = packageSpec.RestoreMetadata.Sources.Select(s => s.Name).ToArray();
                Assert.Equal(sources.Count(), 1);
                Assert.Equal(sources[0], customSourcePath);

                var ridlessTarget = projectA.AssetsFile.Targets.Single(e => string.IsNullOrEmpty(e.RuntimeIdentifier));
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "package" && e.Name == packageX);
                // Should resolve to highest available version.
                ridlessTarget.Libraries.Should().Contain(e => e.Version.Equals(packageX_V2.Version));
            }
        }

        [Fact]
        public async Task AddPkg_V3LocalSourceFeed_WithAbsolutePath_NoVersionSpecified_Fail()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectFrameworks = "net472";
                var packageFrameworks = "net472; netcoreapp2.0";
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var packageX = "packageX";
                var packageY = "packageY";
                var packageX_V1_Context = XPlatTestUtils.CreatePackage(packageX, frameworkString: packageFrameworks);
                var packageY_V1_Context = XPlatTestUtils.CreatePackage(packageY, frameworkString: packageFrameworks);
                var customSourcePath = Path.Combine(pathContext.WorkingDirectory, "Custompackages");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    customSourcePath,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext[] { packageY_V1_Context });

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                // Since user is not inputing a version, it is converted to a " * " in the command
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX, "*",
                    projectA,
                    sources: customSourcePath);

                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));

                // Assert
                Assert.Equal(1, result);
            }
        }

        [Fact]
        public async Task AddPkg_V3LocalSourceFeed_WithAbsolutePath_VersionSpecified_Success()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectFrameworks = "net472";
                var packageFrameworks = "net472; netcoreapp2.0";
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var packageX = "packageX";
                var packageX_V1 = new PackageIdentity(packageX, new NuGetVersion("1.0.0"));
                var packageX_V2 = new PackageIdentity(packageX, new NuGetVersion("2.0.0"));
                var packageX_V1_Context = XPlatTestUtils.CreatePackage(packageX_V1.Id, packageX_V1.Version.Version.ToString(), frameworkString: packageFrameworks);
                var packageX_V2_Context = XPlatTestUtils.CreatePackage(packageX_V2.Id, packageX_V2.Version.Version.ToString(), frameworkString: packageFrameworks);
                var customSourcePath = Path.Combine(pathContext.WorkingDirectory, "Custompackages");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    customSourcePath,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext[] { packageX_V1_Context, packageX_V2_Context });

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX_V1.Id, packageX_V1.Version.ToString(),
                    projectA,
                    sources: customSourcePath);

                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));

                // Assert
                Assert.Equal(0, result);

                // Make sure source is replaced in generated dgSpec file.
                PackageSpec packageSpec = projectA.AssetsFile.PackageSpec;
                string[] sources = packageSpec.RestoreMetadata.Sources.Select(s => s.Name).ToArray();
                Assert.Equal(sources.Count(), 1);
                Assert.Equal(sources[0], customSourcePath);

                var ridlessTarget = projectA.AssetsFile.Targets.Single(e => string.IsNullOrEmpty(e.RuntimeIdentifier));
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "package" && e.Name == packageX);
                // Should resolve to specified version.
                ridlessTarget.Libraries.Should().Contain(e => e.Version.Equals(packageX_V1.Version));
            }
        }

        [Fact]
        public async Task AddPkg_V3LocalSourceFeed_WithAbsolutePath_VersionSpecified_Fail()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectFrameworks = "net472";
                var packageFrameworks = "net472; netcoreapp2.0";
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var packageX = "packageX";
                var packageX_V1 = new PackageIdentity(packageX, new NuGetVersion("1.0.0"));
                var packageX_V2 = new PackageIdentity(packageX, new NuGetVersion("2.0.0"));
                var packageX_V3 = new PackageIdentity(packageX, new NuGetVersion("3.0.0"));
                var packageX_V1_Context = XPlatTestUtils.CreatePackage(packageX, packageX_V1.Version.Version.ToString(), frameworkString: packageFrameworks);
                var packageX_V2_Context = XPlatTestUtils.CreatePackage(packageX, packageX_V2.Version.Version.ToString(), frameworkString: packageFrameworks);
                var customSourcePath = Path.Combine(pathContext.WorkingDirectory, "Custompackages");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    customSourcePath,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext[] { packageX_V1_Context, packageX_V2_Context });

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger,
                    packageX_V3.Id,
                    packageX_V3.Version.ToString(),
                    projectA,
                    sources: customSourcePath);

                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));

                // Assert
                Assert.Equal(1, result);
            }
        }

        [Theory]
        [InlineData("net46", "net46; netcoreapp1.0", "net46")]
        [InlineData("net46; netcoreapp1.0", "net46; netcoreapp1.0", "net46")]
        [InlineData("net46; netcoreapp1.0", "net46; netcoreapp1.0", "netcoreapp1.0")]
        [InlineData("net46", "net46; netcoreapp1.0", "net46; netcoreapp1.0")]
        [InlineData("netcoreapp1.0", "net46; netcoreapp1.0", "net46; netcoreapp1.0")]
        [InlineData("net46", "net46; netcoreapp2.0", "net46")]
        [InlineData("net46; netcoreapp2.0", "net46; netcoreapp2.0", "net46")]
        [InlineData("net46; netcoreapp2.0", "net46; netcoreapp2.0", "netcoreapp2.0")]
        [InlineData("net46", "net46; netcoreapp2.0", "net46; netcoreapp2.0")]
        [InlineData("netcoreapp2.0", "net46; netcoreapp2.0", "net46; netcoreapp2.0")]
        [InlineData("net46", "net46; netstandard2.0", "net46")]
        [InlineData("net46; netstandard2.0", "net46; netstandard2.0", "net46")]
        [InlineData("net46; netstandard2.0", "net46; netstandard2.0", "netstandard2.0")]
        [InlineData("net46", "net46; netstandard2.0", "net46; netstandard2.0")]
        public async Task AddPkg_ConditionalAddWithoutVersion_Success(string packageFrameworks,
            string projectFrameworks,
            string userInputFrameworks)
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var packageX = XPlatTestUtils.CreatePackage(frameworkString: packageFrameworks);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                // Since user is not inputing a version, it is converted to a "*" in the command
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger,
                    packageX.Id,
                    "*",
                    projectA,
                    frameworks: userInputFrameworks,
                    noVersion: true);

                var commandRunner = new AddPackageReferenceCommandRunner();
                var commonFramework = XPlatTestUtils.GetCommonFramework(packageFrameworks, projectFrameworks, userInputFrameworks);

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                var itemGroup = XPlatTestUtils.GetItemGroupForFramework(projectXmlRoot, commonFramework);

                // Assert
                Assert.Equal(0, result);
                Assert.NotNull(itemGroup);

                // Since user did not specify a version, the package reference will contain the resolved version
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageX.Id, "1.0.0"));
            }
        }

        [Theory]
        [InlineData("net46", "netcoreapp1.0")]
        [InlineData("netcoreapp1.0", "net46")]
        [InlineData("net46", "unknown_framework")]
        [InlineData("netcoreapp1.0", "unknown_framework")]
        [InlineData("net46; netcoreapp1.0", "unknown_framework")]
        public async Task AddPkg_FailureIncompatibleFrameworks(string packageFrameworks, string userInputFrameworks)
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46; netcoreapp1.0");
                var packageX = XPlatTestUtils.CreatePackage(frameworkString: packageFrameworks);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, packageX.Version, projectA,
                    frameworks: userInputFrameworks);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                // Assert
                Assert.Equal(1, result);
                Assert.True(XPlatTestUtils.ValidateNoReference(projectXmlRoot, packageX.Id));
            }
        }

        [Fact]
        public async Task AddPkg_FailureUnknownPackage()
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46; netcoreapp1.0");
                var packageX = XPlatTestUtils.CreatePackage();

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, "unknown_package_id", "1.0.0", projectA);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                // Assert
                Assert.Equal(1, result);
                Assert.True(XPlatTestUtils.ValidateNoReference(projectXmlRoot, packageX.Id));
                Assert.True(XPlatTestUtils.ValidateNoReference(projectXmlRoot, "unknown_package_id"));
            }
        }

        [Fact]
        public async Task AddPkg_UnconditionalAddTwoPackages_Success()
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");
                var packageX = XPlatTestUtils.CreatePackage("PkgX");
                var packageY = XPlatTestUtils.CreatePackage("PkgY");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageY);

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var msbuild = new MSBuildAPIUtility(logger);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, packageX.Version, projectA);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, msbuild);
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageY.Id, packageY.Version, projectA);

                // Act
                result = await commandRunner.ExecuteCommand(packageArgs, msbuild);
                projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                // Assert
                Assert.Equal(0, result);
                Assert.True(XPlatTestUtils.ValidateTwoReferences(projectXmlRoot, packageX, packageY));
            }
        }

        [Theory]
        [InlineData("net46", "net46; netcoreapp1.0", "net46")]
        [InlineData("net46; netcoreapp1.0", "net46; netcoreapp1.0", "net46")]
        [InlineData("net46; netcoreapp1.0", "net46; netcoreapp1.0", "netcoreapp1.0")]
        [InlineData("net46", "net46; netcoreapp1.0", "net46; netcoreapp1.0")]
        [InlineData("netcoreapp1.0", "net46; netcoreapp1.0", "net46; netcoreapp1.0")]
        [InlineData("net46", "net46; netcoreapp2.0", "net46")]
        [InlineData("net46; netcoreapp2.0", "net46; netcoreapp2.0", "net46")]
        [InlineData("net46; netcoreapp2.0", "net46; netcoreapp2.0", "netcoreapp2.0")]
        [InlineData("net46", "net46; netcoreapp2.0", "net46; netcoreapp2.0")]
        [InlineData("netcoreapp2.0", "net46; netcoreapp2.0", "net46; netcoreapp2.0")]
        [InlineData("net46", "net46; netstandard2.0", "net46")]
        [InlineData("net46; netstandard2.0", "net46; netstandard2.0", "net46")]
        [InlineData("net46; netstandard2.0", "net46; netstandard2.0", "netstandard2.0")]
        [InlineData("net46", "net46; netstandard2.0", "net46; netstandard2.0")]
        [InlineData("netstandard2.0", "net46; netstandard2.0", "net46; netstandard2.0")]
        public async Task AddPkg_ConditionalAddTwoPackages_Success(string packageFrameworks, string projectFrameworks, string userInputFrameworks)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var packageX = XPlatTestUtils.CreatePackage("PkgX", frameworkString: packageFrameworks);
                var packageY = XPlatTestUtils.CreatePackage("PkgY", frameworkString: packageFrameworks);

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var msBuild = new MSBuildAPIUtility(logger);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageY);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(
                    logger,
                    packageX.Id,
                    packageX.Version,
                    projectA,
                    frameworks: userInputFrameworks);
                var commandRunner = new AddPackageReferenceCommandRunner();
                var commonFramework = XPlatTestUtils.GetCommonFramework(packageFrameworks,
                    projectFrameworks,
                    userInputFrameworks);

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, msBuild);
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageY.Id, packageY.Version, projectA);

                // Act
                result = await commandRunner.ExecuteCommand(packageArgs, msBuild);
                projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                var itemGroup = XPlatTestUtils.GetItemGroupForFramework(projectXmlRoot, commonFramework);

                // Assert
                Assert.Equal(0, result);
                Assert.True(XPlatTestUtils.ValidateTwoReferences(projectXmlRoot, packageX, packageY));
            }
        }

        [Fact]
        public async Task AddPkg_UnconditionalAddWithPackageDirectory_Success()
        {
            // Arrange

            using (var tempGlobalPackagesDirectory = TestDirectory.Create())
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");
                var packageX = XPlatTestUtils.CreatePackage();

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger,
                    packageX.Id,
                    packageX.Version,
                    projectA,
                    packageDirectory: tempGlobalPackagesDirectory.Path);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;
                var itemGroup = XPlatTestUtils.GetItemGroupForAllFrameworks(projectXmlRoot);

                // Assert
                Assert.Equal(0, result);
                Assert.NotNull(itemGroup);
                Assert.True(XPlatTestUtils.ValidateReference(itemGroup, packageX.Id, packageX.Version));

                // Since user provided packge directory, assert if package is present
                Assert.True(XPlatTestUtils.ValidatePackageDownload(tempGlobalPackagesDirectory.Path, packageX));
            }
        }

        // Update Related Tests

        [Theory]
        [InlineData("0.0.5", "1.0.0", false)]
        [InlineData("0.0.5", "0.9", false)]
        [InlineData("0.0.5", "*", false)]
        [InlineData("*", "1.0.0", false)]
        [InlineData("*", "0.9", false)]
        [InlineData("*", "1.*", false)]
        public async Task AddPkg_UnconditionalAddAsUpdate_Succcess(string userInputVersionOld, string userInputVersionNew, bool noVersion)
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46; netcoreapp1.0");
                var latestVersion = "1.0.0";
                var packages = new SimpleTestPackageContext[] { XPlatTestUtils.CreatePackage(packageVersion: latestVersion),
                        XPlatTestUtils.CreatePackage(packageVersion: "0.0.5"), XPlatTestUtils.CreatePackage(packageVersion: "0.0.9") };

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packages);

                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packages[0].Id, userInputVersionOld, projectA);
                var commandRunner = new AddPackageReferenceCommandRunner();
                var msBuild = new MSBuildAPIUtility(logger);

                // Create a package ref with the old version
                var result = await commandRunner.ExecuteCommand(packageArgs, msBuild);
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                //Preconditions
                Assert.True(XPlatTestUtils.ValidateReference(projectXmlRoot, packages[0].Id, userInputVersionOld));

                //The model fom which the args are generated needs updated as well
                projectA.AddPackageToAllFrameworks(new SimpleTestPackageContext(packages[0].Id, userInputVersionOld));

                packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packages[0].Id, userInputVersionNew, projectA, noVersion: noVersion);
                commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                // Create a package ref with the new version
                result = await commandRunner.ExecuteCommand(packageArgs, msBuild);
                projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                // Assert
                // Verify that the only package reference is with the new version
                Assert.Equal(0, result);
                Assert.True(XPlatTestUtils.ValidateReference(projectXmlRoot, packages[0].Id, noVersion ? latestVersion : userInputVersionNew));
            }
        }

        [Theory]
        [InlineData("net46; netcoreapp1.0", "0.0.5", "1.0.0", false)]
        [InlineData("net46; netcoreapp1.0", "0.0.5", "0.9", false)]
        [InlineData("net46; netcoreapp1.0", "0.0.5", "*", false)]
        [InlineData("net46; netcoreapp1.0", "*", "1.0.0", false)]
        [InlineData("net46; netcoreapp1.0", "*", "0.9", false)]
        [InlineData("net46; netcoreapp1.0", "*", "1.*", false)]
        [InlineData("net46; netcoreapp2.0", "0.0.5", "1.0.0", false)]
        [InlineData("net46; netcoreapp2.0", "0.0.5", "0.9", false)]
        [InlineData("net46; netcoreapp2.0", "0.0.5", "*", false)]
        [InlineData("net46; netcoreapp2.0", "*", "1.0.0", false)]
        [InlineData("net46; netcoreapp2.0", "*", "0.9", false)]
        [InlineData("net46; netcoreapp2.0", "*", "1.*", false)]
        [InlineData("net46; netstandard2.0", "0.0.5", "1.0.0", false)]
        [InlineData("net46; netstandard2.0", "0.0.5", "0.9", false)]
        [InlineData("net46; netstandard2.0", "0.0.5", "*", false)]
        [InlineData("net46; netstandard2.0", "*", "1.0.0", false)]
        [InlineData("net46; netstandard2.0", "*", "0.9", false)]
        [InlineData("net46; netstandard2.0", "*", "1.*", false)]
        [InlineData("net46; netstandard2.0", "0.0.5", "*", true)]
        public async Task AddPkg_ConditionalAddAsUpdate_Success(string projectFrameworks, string userInputVersionOld, string userInputVersionNew, bool noVersion)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, projectFrameworks);
                var latestVersion = "1.0.0";
                var packages = new SimpleTestPackageContext[] { XPlatTestUtils.CreatePackage(packageVersion: latestVersion),
                        XPlatTestUtils.CreatePackage(packageVersion: "0.0.5"), XPlatTestUtils.CreatePackage(packageVersion: "0.0.9") };

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packages);

                var logger = new TestCommandOutputLogger(_testOutputHelper);
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packages[0].Id, userInputVersionOld, projectA);
                var commandRunner = new AddPackageReferenceCommandRunner();

                var msBuild = new MSBuildAPIUtility(logger);

                // Create a package ref with old version
                var result = await commandRunner.ExecuteCommand(packageArgs, msBuild);
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                //Preconditions
                Assert.True(XPlatTestUtils.ValidateReference(projectXmlRoot, packages[0].Id, userInputVersionOld));
                //The model fom which the args are generated needs updated as well - not 100% correct, but does the job
                projectA.AddPackageToAllFrameworks(new SimpleTestPackageContext(packages[0].Id, userInputVersionOld));

                packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packages[0].Id, userInputVersionNew, projectA, noVersion: noVersion);
                commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                // Create a package ref with new version
                result = await commandRunner.ExecuteCommand(packageArgs, msBuild);
                projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                // Assert
                // Verify that the only package reference is with the new version
                Assert.Equal(0, result);
                Assert.True(XPlatTestUtils.ValidateReference(projectXmlRoot, packages[0].Id, noVersion ? latestVersion : userInputVersionNew), projectXmlRoot.ToString());
            }
        }

        [Fact]
        public async Task AddPkg_DevelopmentDependency()
        {
            // Arrange

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");
                var packageX = XPlatTestUtils.CreatePackage(developmentDependency: true);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var logger = new TestCommandOutputLogger(_testOutputHelper);

                // Since user is not specifying a version, it is converted to a "*"
                var packageArgs = XPlatTestUtils.GetPackageReferenceArgs(logger, packageX.Id, "*", projectA, noVersion: true);
                var commandRunner = new AddPackageReferenceCommandRunner();

                // Act
                var result = await commandRunner.ExecuteCommand(packageArgs, new MSBuildAPIUtility(logger));
                var projectXmlRoot = XPlatTestUtils.LoadCSProj(projectA.ProjectPath).Root;

                // Assert
                Assert.Equal(0, result);

                // Since user did not specify a version, the package reference will contain the resolved version
                Assert.True(XPlatTestUtils.ValidateReference(projectXmlRoot, packageX.Id, "1.0.0", developmentDependency: true));
            }
        }
    }
}

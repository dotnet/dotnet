// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.XPlat.FuncTest;
using Xunit;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetAddPackageTests
    {
        private readonly DotnetIntegrationTestFixture _fixture;

        public DotnetAddPackageTests(DotnetIntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AddPkg_V3LocalSourceFeed_WithRelativePath_NoVersionSpecified_Success()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectName = "projectA";
                var targetFrameworks = "net7.0";
                SimpleTestProjectContext projectA = XPlatTestUtils.CreateProject(projectName, pathContext, targetFrameworks);
                var packageX = "packageX";
                var packageX_V1 = new PackageIdentity(packageX, new NuGetVersion("1.0.0"));
                var packageX_V2 = new PackageIdentity(packageX, new NuGetVersion("2.0.0"));
                var packageFrameworks = "net472; netcoreapp2.0";
                var packageX_V1_Context = XPlatTestUtils.CreatePackage(packageX_V1.Id, packageX_V1.Version.Version.ToString(), frameworkString: packageFrameworks);
                var packageX_V2_Context = XPlatTestUtils.CreatePackage(packageX_V2.Id, packageX_V2.Version.Version.ToString(), frameworkString: packageFrameworks);
                var customSourcePath = Path.Combine(pathContext.WorkingDirectory, "Custompackages");
                var sourceRelativePath = Path.Combine("..", "..", "Custompackages");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    customSourcePath,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext[] { packageX_V1_Context, packageX_V2_Context });

                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var projectFilePath = Path.Combine(projectDirectory, $"{projectName}.csproj");

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(projectDirectory, $"add {projectFilePath} package {packageX} -s {sourceRelativePath}");

                // Assert
                // Make sure source is replaced in generated dgSpec file.
                PackageSpec packageSpec = projectA.AssetsFile.PackageSpec;
                string[] sources = packageSpec.RestoreMetadata.Sources.Select(s => s.Name).ToArray();
                Assert.Equal(sources.Count(), 1);
                Assert.Equal(sources[0], customSourcePath);

                var ridlessTarget = projectA.AssetsFile.Targets.Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Single();
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "package" && e.Name == packageX);
                // Should resolve to specified package.
                ridlessTarget.Libraries.Should().Contain(e => e.Version.Equals(packageX_V2.Version));
            }
        }

        [Fact]
        public async Task AddPkg_V3LocalSourceFeed_WithRelativePath_NoVersionSpecified_Fail()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectName = "projectA";
                var targetFrameworks = "net7.0";
                XPlatTestUtils.CreateProject(projectName, pathContext, targetFrameworks);
                var packageX = "packageX";
                var packageY = "packageY";
                var packageY_V1 = new PackageIdentity(packageY, new NuGetVersion("1.0.0"));
                var packageFrameworks = "net472; netcoreapp2.0";
                var packageY_V1_Context = XPlatTestUtils.CreatePackage(packageY_V1.Id, packageY_V1.Version.Version.ToString(), frameworkString: packageFrameworks);
                var customSourcePath = Path.Combine(pathContext.WorkingDirectory, "Custompackages");
                var sourceRelativePath = Path.Combine("..", "..", "Custompackages");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    customSourcePath,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext[] { packageY_V1_Context });

                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var projectFilePath = Path.Combine(projectDirectory, $"{projectName}.csproj");

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectFailure(projectDirectory, $"add {projectFilePath} package {packageX} -s {sourceRelativePath}");
            }
        }

        [Fact]
        public async Task AddPkg_V3LocalSourceFeed_WithRelativePath_VersionSpecified_Success()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectName = "projectA";
                var targetFrameworks = "net7.0";
                SimpleTestProjectContext projectA = XPlatTestUtils.CreateProject(projectName, pathContext, targetFrameworks);
                var packageX = "packageX";
                var packageX_V1 = new PackageIdentity(packageX, new NuGetVersion("1.0.0"));
                var packageX_V2 = new PackageIdentity(packageX, new NuGetVersion("2.0.0"));
                var packageFrameworks = "net472; netcoreapp2.0";
                var packageX_V1_Context = XPlatTestUtils.CreatePackage(packageX_V1.Id, packageX_V1.Version.Version.ToString(), frameworkString: packageFrameworks);
                var packageX_V2_Context = XPlatTestUtils.CreatePackage(packageX_V2.Id, packageX_V2.Version.Version.ToString(), frameworkString: packageFrameworks);
                var customSourcePath = Path.Combine(pathContext.WorkingDirectory, "Custompackages");
                var sourceRelativePath = Path.Combine("..", "..", "Custompackages");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    customSourcePath,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext[] { packageX_V1_Context, packageX_V2_Context });

                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var projectFilePath = Path.Combine(projectDirectory, $"{projectName}.csproj");

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(projectDirectory, $"add {projectFilePath} package {packageX} -s {sourceRelativePath} -v {packageX_V1.Version}");

                // Assert
                // Make sure source is replaced in generated dgSpec file.
                PackageSpec packageSpec = projectA.AssetsFile.PackageSpec;
                string[] sources = packageSpec.RestoreMetadata.Sources.Select(s => s.Name).ToArray();
                Assert.Equal(sources.Count(), 1);
                Assert.Equal(sources[0], customSourcePath);

                var ridlessTarget = projectA.AssetsFile.Targets.Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Single();
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "package" && e.Name == packageX);
                // Should resolve to specified package.
                ridlessTarget.Libraries.Should().Contain(e => e.Version.Equals(packageX_V1.Version));
            }
        }

        [Fact]
        public async Task AddPkg_V3LocalSourceFeed_WithRelativePath_VersionSpecified_Fail()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectName = "projectA";
                var targetFrameworks = "net7.0";
                SimpleTestProjectContext projectA = XPlatTestUtils.CreateProject(projectName, pathContext, targetFrameworks);
                var packageX = "packageX";
                var packageX_V1 = new PackageIdentity(packageX, new NuGetVersion("1.0.0"));
                var packageX_V2 = new PackageIdentity(packageX, new NuGetVersion("2.0.0"));
                var packageFrameworks = "net472; netcoreapp2.0";
                var packageX_V1_Context = XPlatTestUtils.CreatePackage(packageX_V1.Id, packageX_V1.Version.Version.ToString(), frameworkString: packageFrameworks);
                var customSourcePath = Path.Combine(pathContext.WorkingDirectory, "Custompackages");
                var sourceRelativePath = Path.Combine("..", "..", "Custompackages");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    customSourcePath,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext[] { packageX_V1_Context });

                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var projectFilePath = Path.Combine(projectDirectory, $"{projectName}.csproj");

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectFailure(projectDirectory, $"add {projectFilePath} package {packageX} -s {sourceRelativePath} -v {packageX_V2.Version}");
            }
        }

        [Fact]
        public async Task AddPkg_V3LocalSourceFeed_WithDefaultSolutiuonSource_NoVersionSpecified_Success()
        {
            using (SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext(addTemplateFeed: false))
            {
                var projectName = "projectA";
                var targetFrameworks = "net7.0";
                SimpleTestProjectContext projectA = XPlatTestUtils.CreateProject(projectName, pathContext, targetFrameworks);
                var packageY = "packageY";
                var packageY_V1 = new PackageIdentity(packageY, new NuGetVersion("1.0.0"));
                var packageY_V2 = new PackageIdentity(packageY, new NuGetVersion("2.0.0"));
                var packageFrameworks = "net472; netcoreapp2.0";
                var packageY_V1_Context = XPlatTestUtils.CreatePackage(packageY_V1.Id, packageY_V1.Version.Version.ToString(), frameworkString: packageFrameworks);
                var packageY_V2_Context = XPlatTestUtils.CreatePackage(packageY_V2.Id, packageY_V2.Version.Version.ToString(), frameworkString: packageFrameworks);

                // Generate V3 Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource, // using default solution source folder, not passing source as parameter.
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext[] { packageY_V1_Context, packageY_V2_Context });

                var projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var projectFilePath = Path.Combine(projectDirectory, $"{projectName}.csproj");

                // Act
                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(projectDirectory, $"add {projectFilePath} package {packageY}");

                // Assert

                // Make sure source is replaced in generated dgSpec file.
                PackageSpec packageSpec = projectA.AssetsFile.PackageSpec;

                packageSpec.RestoreMetadata.Sources.Select(s => s.Name).Should().ContainSingle()
                    .Which.Should().Be(pathContext.PackageSource);

                var ridlessTarget = projectA.AssetsFile.Targets.Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier)).Single();
                // Should resolve to specified package.
                ridlessTarget.Libraries.Should().Contain(e => e.Type == "package" && e.Name == packageY);
                // Should resolve to highest available version.
                ridlessTarget.Libraries.Should().Contain(e => e.Version.Equals(packageY_V2.Version));
            }
        }

        [Fact]
        public async Task AddPkg_WhenPackageSourceMappingConfiguredAndNoMatchingSourceFound_Fails()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution, and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectName = "projectA";
            var projectA = XPlatTestUtils.CreateProject(projectName, pathContext, "net7.0");

            const string version = "1.0.0";
            const string packageX = "X", packageZ = "Z";

            var packageFrameworks = "net472; net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version, frameworkString: packageFrameworks);
            var packageZ100 = XPlatTestUtils.CreatePackage(packageZ, version, frameworkString: packageFrameworks);

            packageX100.Dependencies.Add(packageZ100);

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var packageSource2 = new DirectoryInfo(Path.Combine(pathContext.WorkingDirectory, "source2"));
            packageSource2.Create();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource2.FullName,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            var configFile = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""source2"" value=""{packageSource2.FullName}"" />
    </packageSources>
        <packageSourceMapping>
            <packageSource key=""source2"">
                <package pattern=""{packageX}*"" />
            </packageSource>
    </packageSourceMapping>
</configuration>
";
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);
            File.WriteAllText(Path.Combine(projectADirectory, "NuGet.Config"), configFile);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version}");

            // Assert
            Assert.Contains($"Installed {packageX} {version} from {packageSource2.FullName}", result.AllOutput);
            Assert.Contains($"NU1100: Unable to resolve '{packageZ} (>= {version})'", result.AllOutput);
        }

        [Fact]
        public async Task AddPkg_WhenPackageSourceMappingConfiguredInstallsPackagesFromExpectedSources_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution, and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectName = "projectA";
            var projectA = XPlatTestUtils.CreateProject(projectName, pathContext, "net7.0");

            const string version = "1.0.0";
            const string packageX = "X", packageZ = "Z";

            var packageFrameworks = "net472; net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version, frameworkString: packageFrameworks);
            var packageZ100 = XPlatTestUtils.CreatePackage(packageZ, version, frameworkString: packageFrameworks);

            packageX100.Dependencies.Add(packageZ100);

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var packageSource2 = new DirectoryInfo(Path.Combine(pathContext.WorkingDirectory, "source2"));
            packageSource2.Create();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource2.FullName,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            var configFile = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""source2"" value=""{packageSource2.FullName}"" />
    </packageSources>
        <packageSourceMapping>
            <packageSource key=""source"">
                <package pattern=""{packageZ}*"" />
            </packageSource>
            <packageSource key=""source2"">
                <package pattern=""{packageX}*"" />
            </packageSource>
    </packageSourceMapping>
</configuration>
";
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);
            File.WriteAllText(Path.Combine(projectADirectory, "NuGet.Config"), configFile);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version}");

            // Assert
            Assert.Contains($"Installed {packageX} {version} from {packageSource2.FullName}", result.AllOutput);
            Assert.Contains($"Installed {packageZ} {version} from {pathContext.PackageSource}", result.AllOutput);
        }

        [Fact]
        public async Task AddPkg_WhenPackageSourceMappingConfiguredInstallsPackagesFromSourcesUriOption_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution, and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectName = "projectA";
            var projectA = XPlatTestUtils.CreateProject(projectName, pathContext, "net7.0");

            const string version = "1.0.0";
            const string packageX = "X", packageZ = "Z";

            var packageFrameworks = "net472; net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version, frameworkString: packageFrameworks);
            var packageZ100 = XPlatTestUtils.CreatePackage(packageZ, version, frameworkString: packageFrameworks);

            packageX100.Dependencies.Add(packageZ100);

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var packageSource2 = new DirectoryInfo(Path.Combine(pathContext.WorkingDirectory, "source2"));
            packageSource2.Create();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource2.FullName,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            var configFile = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""source2"" value=""{packageSource2.FullName}"" />
    </packageSources>
        <packageSourceMapping>
            <packageSource key=""source"">
                <package pattern=""{packageZ}*"" />
                <package pattern=""{packageX}*"" />
            </packageSource>
            <packageSource key=""source2"">
                <package pattern=""{packageZ}*"" />
                <package pattern=""{packageX}*"" />
            </packageSource>
    </packageSourceMapping>
</configuration>
";
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);
            File.WriteAllText(Path.Combine(projectADirectory, "NuGet.Config"), configFile);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version} -s {pathContext.PackageSource}");

            // Assert
            Assert.Contains($"Installed {packageX} {version} from {pathContext.PackageSource}", result.AllOutput);
            Assert.Contains($"Installed {packageZ} {version} from {pathContext.PackageSource}", result.AllOutput);
        }

        [Fact]
        public async Task AddPkg_WhenPackageSourceMappingConfiguredCanotInstallsPackagesFromSourcesUriOption_Fails()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution, and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectName = "projectA";
            var projectA = XPlatTestUtils.CreateProject(projectName, pathContext, "net7.0");

            const string version = "1.0.0";
            const string packageX = "X", packageZ = "Z";

            var packageFrameworks = "net472; net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version, frameworkString: packageFrameworks);
            var packageZ100 = XPlatTestUtils.CreatePackage(packageZ, version, frameworkString: packageFrameworks);

            packageX100.Dependencies.Add(packageZ100);

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var packageSource2 = new DirectoryInfo(Path.Combine(pathContext.WorkingDirectory, "source2"));
            packageSource2.Create();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource2.FullName,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            var configFile = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""source2"" value=""{packageSource2.FullName}"" />
    </packageSources>
        <packageSourceMapping>
            <packageSource key=""source"">
                <package pattern=""{packageZ}*"" />
                <package pattern=""{packageX}*"" />
            </packageSource>
            <packageSource key=""source2"">
                <package pattern=""{packageX}*"" />
            </packageSource>
    </packageSourceMapping>
</configuration>
";
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);
            File.WriteAllText(Path.Combine(projectADirectory, "NuGet.Config"), configFile);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version} -s {packageSource2}");

            // Assert
            Assert.Contains($"Installed {packageX} {version} from {packageSource2}", result.AllOutput);
            Assert.Contains($"NU1100: Unable to resolve '{packageZ} (>= {version})' for 'net7.0'", result.AllOutput);
        }

        [Fact]
        public async Task AddPkg_WhenPackageSourceMappingConfiguredInstallsPackagesFromRestoreSources_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution, and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectName = "projectA";
            var projectA = XPlatTestUtils.CreateProject(projectName, pathContext, "net7.0");

            const string version = "1.0.0";
            const string packageX = "X", packageZ = "Z";

            var packageFrameworks = "net472;net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version, frameworkString: packageFrameworks);
            var packageZ100 = XPlatTestUtils.CreatePackage(packageZ, version, frameworkString: packageFrameworks);

            packageX100.Dependencies.Add(packageZ100);

            var packageSource2 = new DirectoryInfo(Path.Combine(pathContext.WorkingDirectory, "source2"));
            packageSource2.Create();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource2.FullName,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            var configFile = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""source2"" value=""{packageSource2.FullName}"" />
    </packageSources>
        <packageSourceMapping>
            <packageSource key=""source"">
                <package pattern=""*"" />
            </packageSource>
            <packageSource key=""source2"">
                <package pattern=""{packageZ}*"" />
                <package pattern=""{packageX}*"" />
            </packageSource>
    </packageSourceMapping>
</configuration>
";

            // Add RestoreSources
            projectA.Properties.Add("RestoreSources", $"{packageSource2.FullName}");

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);
            File.WriteAllText(Path.Combine(projectADirectory, "NuGet.Config"), configFile);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version}");

            // Assert
            Assert.Contains($"Installed {packageX} {version} from {packageSource2.FullName}", result.AllOutput);
            Assert.Contains($"Installed {packageZ} {version} from {packageSource2.FullName}", result.AllOutput);
        }

        [Fact]
        public async Task AddPkg_WhenPackageSourceMappingConfiguredCanotInstallsPackagesFromRestoreSources_Fails()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution, and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectName = "projectA";
            var projectA = XPlatTestUtils.CreateProject(projectName, pathContext, "net7.0");

            const string version = "1.0.0";
            const string packageX = "X", packageZ = "Z";

            var packageFrameworks = "net472; net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version, frameworkString: packageFrameworks);
            var packageZ100 = XPlatTestUtils.CreatePackage(packageZ, version, frameworkString: packageFrameworks);

            packageX100.Dependencies.Add(packageZ100);

            var packageSource2 = new DirectoryInfo(Path.Combine(pathContext.WorkingDirectory, "source2"));
            packageSource2.Create();

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource2.FullName,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageZ100);

            var configFile = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""source2"" value=""{packageSource2.FullName}"" />
    </packageSources>
        <packageSourceMapping>
            <packageSource key=""source2"">
                <package pattern=""{packageX}*"" />
            </packageSource>
    </packageSourceMapping>
</configuration>
";
            // Add RestoreSources
            projectA.Properties.Add("RestoreSources", $"{packageSource2.FullName};{pathContext.PackageSource}");

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);
            File.WriteAllText(Path.Combine(projectADirectory, "NuGet.Config"), configFile);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version} -s {packageSource2}");

            // Assert
            Assert.Contains($"Installed {packageX} {version} from {packageSource2}", result.AllOutput);
            Assert.Contains($"NU1100: Unable to resolve '{packageZ} (>= {version})' for 'net7.0'", result.AllOutput);
        }

        [Fact]
        public void AddPkg_WhenSourceIsSignedPackageWithExpiredCertificatesAndWithTimestamps_Success()
        {
            using (SimpleTestPathContext pathContext = new())
            {
                var projectName = "project";
                string targetFrameworks = Constants.DefaultTargetFramework.GetShortFolderName();
                SimpleTestProjectContext projectA = XPlatTestUtils.CreateProject(projectName, pathContext, targetFrameworks);

                // This package is important because:
                //    * it has no package dependencies and thus simplifies the test scenario
                //    * it is a signed package and thus verifies signed package verification, if enabled
                //    * the author- and repository-signing certificates have expired
                //    * the author and repository timestamps may be untrusted on Linux/macOS if a valid certificate bundle isn't found
                PackageIdentity package = new("NuGet.Versioning", new NuGetVersion("5.0.0"));
                DirectoryInfo packageSourceDirectory = new(Path.Combine(pathContext.WorkingDirectory, "PackageSource"));
                var packageFileName = $"{package.Id.ToLowerInvariant()}.{package.Version}.nupkg";

                CopyResourceToDirectory(packageFileName, packageSourceDirectory);

                string projectDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                string projectFilePath = Path.Combine(projectDirectory, $"{projectName}.csproj");

                CommandRunnerResult result = _fixture.RunDotnetExpectSuccess(
                    projectDirectory,
                    $"add {projectFilePath} package {package.Id} -s {packageSourceDirectory.FullName} -v {package.Version}");

                if (RuntimeEnvironmentHelper.IsWindows)
                {
                    result.AllOutput.Should()
                        .Contain(
                            Strings.ChainBuilding_UsingDefaultTrustStoreForCodeSigning,
                            because: result.AllOutput);

                    result.AllOutput.Should()
                        .Contain(
                            Strings.ChainBuilding_UsingDefaultTrustStoreForTimestamping,
                            because: result.AllOutput);
                }
                else
                {
                    result.AllOutput.Should()
                        .ContainAny(
                            new string[] {
                                "X.509 certificate chain validation will use the fallback certificate bundle at ",
                                "X.509 certificate chain validation will use the system certificate bundle at "
                            },
                            because: result.AllOutput);
                }

                LockFileTarget ridlessTarget = projectA.AssetsFile.Targets
                    .Where(e => string.IsNullOrEmpty(e.RuntimeIdentifier))
                    .Single();

                ridlessTarget.Libraries.Should().Contain(e => e.Type == "package" && e.Name == package.Id);
                ridlessTarget.Libraries.Should().Contain(e => e.Version.Equals(package.Version));
            }
        }

        private void CopyResourceToDirectory(string resourceName, DirectoryInfo directory)
        {
            string fullResourceName = $"Dotnet.Integration.Test.compiler.resources.{resourceName}";
            string destinationFilePath = Path.Combine(directory.FullName, resourceName);

            directory.Create();

            using (Stream stream = GetType().Assembly.GetManifestResourceStream(fullResourceName))
            {
                stream.CopyToFile(destinationFilePath);
            }
        }

        [Fact]
        public async Task AddPkg_WithCPM_WhenPackageVersionDoesNotExistAndVersionCLIArgNotPassed_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution, and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);

            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX}");

            // Assert
            Assert.Contains(@$"<ItemGroup>
    <PackageVersion Include=""X"" Version=""2.0.0"" />
  </ItemGroup", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.DoesNotContain(@$"<ItemGroup> <PackageReference Include=""X"" Version=""2.0.0"" /> </ItemGroup",
                File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WhenPackageVersionDoesNotExistAndVersionCLIArgPassed_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution, and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version1}");

            // Assert
            Assert.Contains(@$"<ItemGroup>
    <PackageVersion Include=""X"" Version=""1.0.0"" />
  </ItemGroup", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.DoesNotContain(@$"<ItemGroup> <PackageVersion Include=""X"" Version=""1.0.0"" /> </ItemGroup",
                File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WhenPackageVersionExistsAndVersionCLIArgNotPassed_NoOp()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                                <ItemGroup>
                                <PackageVersion Include=""X"" Version=""1.0.0"" />
                                </ItemGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            //Act
            //By default the package version used will be 2.0.0 since no version CLI argument is passed in the CLI command.
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX}");

            // Assert
            // Checking that the PackageVersion is not updated.
            Assert.Contains(@$"<PackageVersion Include=""X"" Version=""1.0.0"" />", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));

            var projectFileFromDisk = File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj"));

            // Checking that version metadata is not added to the project files.
            Assert.Contains(@$"Include=""X""", projectFileFromDisk);
            Assert.DoesNotContain(@$"Include=""X"" Version=""1.0.0""", projectFileFromDisk);
        }

        [Fact]
        public async Task AddPkg_WithCPM_WhenPackageVersionExistsAndVersionCLIArgPassed_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                                <ItemGroup>
                                <PackageVersion Include=""X"" Version=""1.0.0"" />
                                </ItemGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version}");

            // Assert
            Assert.Contains(@$"<PackageVersion Include=""X"" Version=""2.0.0"" />", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));

            var projectFileFromDisk = File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj"));

            // Checking that version metadata is not added to the project files.
            Assert.Contains(@$"Include=""X""", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
            Assert.DoesNotContain(@$"Include=""X"" Version=""1.0.0""", projectFileFromDisk);
            Assert.DoesNotContain(@$"Include=""X"" Version=""2.0.0""", projectFileFromDisk);
        }

        [Fact]
        public async Task AddPkg_WithCPM_WhenMultipleItemGroupsExist_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version = "1.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100);

            var propsFile = @$"<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <Content Include=""SomeFile"" />
  </ItemGroup>
  <ItemGroup>
    <PackageVersion Include=""Y"" Version=""1.0.0"" />
  </ItemGroup>
</Project>";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version}");

            // Assert
            var propsFileFromDisk = File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"));

            Assert.Contains(@$"<ItemGroup>
    <PackageVersion Include=""X"" Version=""1.0.0"" />
    <PackageVersion Include=""Y"" Version=""1.0.0"" />
  </ItemGroup>", propsFileFromDisk);

            Assert.DoesNotContain($@"< ItemGroup >
    < Content Include = ""SomeFile"" />
    <PackageVersion Include=""X"" Version=""1.0.0"" />
  </ ItemGroup >", propsFileFromDisk);
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_NoVersionOverride_NoPackageVersion_NoVersionCLI_Errors()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>{packageFrameworks}</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" Version=""1.0.0""/>
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX} ");

            // Assert
            Assert.Contains("error: Projects that use central package version management should not define the version on the PackageReference items but on the PackageVersion items: X", result.Output);
            Assert.DoesNotContain(@$"<ItemGroup>
    <PackageVersion Include=""X"" Version=""2.0.0"" />
  </ItemGroup>", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.Contains(@$" <ItemGroup>
        <PackageReference Include=""X"" Version=""1.0.0""/>
    </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_NoVersionOverride_NoPackageVersion_WithVersionCLI_Errors()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>{packageFrameworks}</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" Version=""1.0.0""/>
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version2}");

            // Assert
            Assert.Contains("error: Projects that use central package version management should not define the version on the PackageReference items but on the PackageVersion items: X", result.Output);
            Assert.DoesNotContain(@$"<ItemGroup>
    <PackageVersion Include=""X"" Version=""2.0.0"" />
  </ItemGroup>", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.Contains(@$"<ItemGroup>
        <PackageReference Include=""X"" Version=""1.0.0""/>
    </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_NoVersionOverride_WithPackageVersion_NoVersionCLI_NoOps()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                    <ItemGroup>
                                        <PackageVersion Include=""X"" Version=""1.0.0"" />
                                    </ItemGroup>
                                </PropertyGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>{packageFrameworks}</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" />
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX} ");

            // Assert
            Assert.DoesNotContain("error: Projects that use central package version management should not define the version on the PackageReference items but on the PackageVersion items: X", result.Output);
            Assert.Contains(@$"<ItemGroup>
                                        <PackageVersion Include=""X"" Version=""1.0.0"" />
                                    </ItemGroup>", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.Contains(@$"<ItemGroup>
        <PackageReference Include=""X"" />
    </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_NoVersionOverride_WithPackageVersion_WithVersionCLI_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                                <ItemGroup>
                                <PackageVersion Include=""X"" Version=""1.0.0"" />
                                </ItemGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
    <PropertyGroup>                   
	    <TargetFramework>{packageFrameworks}</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" />
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version2}");

            // Assert
            Assert.Contains(@$"<ItemGroup>
    <PackageVersion Include=""X"" Version=""2.0.0"" />
  </ItemGroup>", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_WithVersionOverride_NoPackageVersion_NoVersionCLI_NoOps()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>{packageFrameworks}</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX}");

            // Assert
            Assert.DoesNotContain("error: Projects that use central package version management should not define the version on the PackageReference items but on the PackageVersion items: X", result.Output);
            Assert.DoesNotContain(@$"<ItemGroup>
    <PackageVersion Include=""X"" Version=""2.0.0"" />
  </ItemGroup>", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.Contains(@$"<ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
    </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_WithVersionOverride_NoPackageVersion_WithVersionCLI_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>{packageFrameworks}</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version2}");

            // Assert
            Assert.DoesNotContain(@$"<ItemGroup>
                                    <PackageVersion Include=""X"" Version=""2.0.0"" />
                                </ItemGroup>", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.Contains(@$"<ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""2.0.0"" />
    </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_WithVersionOverride_WithPackageVersion_NoVersionCLI_NoOps()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                                <ItemGroup>
                                <PackageVersion Include=""X"" Version=""1.0.0"" />
                                </ItemGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>{packageFrameworks}</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX}");

            // Assert
            Assert.DoesNotContain("error: Projects that use central package version management should not define the version on the PackageReference items but on the PackageVersion items: X", result.Output);
            Assert.Contains(@$"<ItemGroup>
                                <PackageVersion Include=""X"" Version=""1.0.0"" />
                                </ItemGroup>", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.Contains(@$"<ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
    </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_WithVersionOverride_WithPackageVersion_WithVersionCLI_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                                <ItemGroup>
                                <PackageVersion Include=""X"" Version=""1.0.0"" />
                                </ItemGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>{packageFrameworks}</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version2}");

            // Assert
            Assert.DoesNotContain(@$"<ItemGroup>
                                    <PackageVersion Include=""X"" Version=""2.0.0"" />
                                </ItemGroup>", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.Contains(@$"<ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""2.0.0"" />
    </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_EmptyVersionOverride_WithPackageVersion_WithVersionCLI_IgnoreEmptyVersionOverride_Success()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                <PropertyGroup>
                                <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                </PropertyGroup>
                                <ItemGroup>
                                <PackageVersion Include=""X"" Version=""1.0.0"" />
                                </ItemGroup>
                            </Project>
                            ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>{packageFrameworks}</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""   ""/>
    </ItemGroup>
</Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectSuccess(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version2}");

            // Assert
            Assert.Contains(@$"<ItemGroup>
    <PackageVersion Include=""X"" Version=""2.0.0"" />
  </ItemGroup>", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.DoesNotContain(@$"<ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""2.0.0"" />
    </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WrongPackageReference_WithVersionOverride_WithPackageVersion_WithVersionCLI_WrongConfiguration_Error()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                    <PropertyGroup>
                                    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                    </PropertyGroup>
                                    <ItemGroup>
                                    <PackageVersion Include=""X"" Version=""1.0.0"" />
                                    <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
                                    </ItemGroup>
                                </Project>
                                ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
    @$"<Project  Sdk=""Microsoft.NET.Sdk"">
    <PropertyGroup>                   
	    <TargetFramework>{packageFrameworks}</TargetFramework>
	    </PropertyGroup>
    </Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version2}");

            // Assert
            Assert.Contains("error: Package reference for package 'X' defined in incorrect location, PackageReference should be defined in project file.", result.Output);
            Assert.Contains(@$"<PackageVersion Include=""X"" Version=""1.0.0"" />", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.DoesNotContain(@$"<ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""2.0.0"" />
    </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_WithVersionOverride_WrongPackageVersion_WithVersionCLI_WrongConfiguration_Error()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                    <PropertyGroup>
                                    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                    </PropertyGroup>
                                </Project>
                                ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
    @$"<Project  Sdk=""Microsoft.NET.Sdk"">
    <PropertyGroup>                   
	    <TargetFramework>{packageFrameworks}</TargetFramework>
	    </PropertyGroup>
        <ItemGroup>
            <PackageVersion Include=""X"" Version=""1.0.0"" />
            <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
        </ItemGroup>
    </Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version2}");

            // Assert
            Assert.Contains(" PackageVersion for package 'X' defined in incorrect location, PackageVersion should be defined in Directory.Package.props.", result.Output);
            Assert.DoesNotContain(@$"<PackageVersion Include=""X"" Version=""2.0.0"" />", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.Contains(@$"<ItemGroup>
            <PackageVersion Include=""X"" Version=""1.0.0"" />
            <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
        </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }

        [Fact]
        public async Task AddPkg_WithCPM_WithPackageReference_DisabledVersionOverride_WrongPackageVersion_WithVersionCLI_WrongConfiguration_Error()
        {
            using SimpleTestPathContext pathContext = _fixture.CreateSimpleTestPathContext();

            // Set up solution and project
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var projectA = XPlatTestUtils.CreateProject("projectA", pathContext, "net7.0");

            const string version1 = "1.0.0";
            const string version2 = "2.0.0";
            const string packageX = "X";

            var packageFrameworks = "net7.0";
            var packageX100 = XPlatTestUtils.CreatePackage(packageX, version1, frameworkString: packageFrameworks);
            var packageX200 = XPlatTestUtils.CreatePackage(packageX, version2, frameworkString: packageFrameworks);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX100,
                    packageX200);

            var propsFile = @$"<Project>
                                    <PropertyGroup>
                                    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
                                    </PropertyGroup>
                                </Project>
                                ";

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);


            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);
            var projectADirectory = Path.Combine(pathContext.SolutionRoot, projectA.ProjectName);

            // Arrange project file
            string projectContent =
    @$"<Project  Sdk=""Microsoft.NET.Sdk"">
    <PropertyGroup>                   
	    <TargetFramework>{packageFrameworks}</TargetFramework>
        <CentralPackageVersionOverrideEnabled>false</CentralPackageVersionOverrideEnabled>
	    </PropertyGroup>
        <ItemGroup>
            <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
        </ItemGroup>
    </Project>";
            File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "projectA", "projectA.csproj"), projectContent);

            //Act
            var result = _fixture.RunDotnetExpectFailure(projectADirectory, $"add {projectA.ProjectPath} package {packageX} -v {version2}");

            // Assert
            Assert.Contains("The package reference X specifies a VersionOverride but the ability to override a centrally defined version is currently disabled.", result.Output);
            Assert.DoesNotContain(@$"<PackageVersion Include=""X"" Version=""2.0.0"" />", File.ReadAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props")));
            Assert.Contains(@$"<ItemGroup>
            <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
        </ItemGroup>", File.ReadAllText(Path.Combine(projectADirectory, "projectA.csproj")));
        }
    }
}

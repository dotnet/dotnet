// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Packaging;
using NuGet.Test.Utility;
using NuGet.XPlat.FuncTest;
using Test.Utility;
using Xunit;
using Strings = NuGet.CommandLine.XPlat.Strings;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetListPackageTests
    {
        private static readonly string ProjectName = "test_project_listpkg";

        private readonly DotnetIntegrationTestFixture _fixture;

        public DotnetListPackageTests(DotnetIntegrationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_Succeed()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");

                var packageX = XPlatTestUtils.CreatePackage();

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --version 1.0.0 --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package");

                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, "packageX1.0.01.0.0"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_NoRestore_Fail()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");

                var packageX = XPlatTestUtils.CreatePackage();

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);


                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --no-restore");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectFailure(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package");

                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, "No assets file was found".Replace(" ", "")));
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_VersionRanges_WithCPM()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net7.0");

                var packageX = XPlatTestUtils.CreatePackage("X", "1.0.0");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var propsFile =
@$"<Project>
   <ItemGroup>
        <PackageVersion Include=""X"" Version=""[0.1.0,)"" />
    </ItemGroup>
</Project>";

                File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);

                string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>net46</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X""/>
    </ItemGroup>
</Project>";
                File.WriteAllText(Path.Combine(pathContext.SolutionRoot, ProjectName, string.Concat(ProjectName, ".csproj")), projectContent);

                _fixture.RunDotnetExpectSuccess(Path.Combine(pathContext.SolutionRoot, projectA.ProjectName),
                    $"restore {projectA.ProjectName}.csproj");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package");

                // Assert Requested version is [0.1.0,), but 1.0.0 was resolved
                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, "[0.1.0,)"));
                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, "1.0.0"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_WithCPM_WithVersionOverride()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net7.0");

                var packageX = XPlatTestUtils.CreatePackage("X", "1.0.0");
                var packageX2 = XPlatTestUtils.CreatePackage("X", "2.0.0");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageX2);

                var propsFile =
@$"<Project>
    <ItemGroup>
        <PackageVersion Include=""X"" Version=""2.0.0"" />
    </ItemGroup>
</Project>";

                File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);

                string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>net46</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X"" VersionOverride=""1.0.0""/>
    </ItemGroup>
</Project>";
                File.WriteAllText(Path.Combine(pathContext.SolutionRoot, ProjectName, string.Concat(ProjectName, ".csproj")), projectContent);

                _fixture.RunDotnetExpectSuccess(Path.Combine(pathContext.SolutionRoot, projectA.ProjectName),
                    $"restore {projectA.ProjectName}.csproj");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package");

                // Assert Resolved version is 1.0.0 and Requested version is 1.0.0, since it was overridden by VersionOverride tag
                Assert.False(ContainsIgnoringSpaces(listResult.AllOutput, "2.0.0"));
                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, "1.0.0"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_WithCPM_GlobalPackageReference()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net7.0");

                var packageX = XPlatTestUtils.CreatePackage("X", "1.0.0");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var propsFile =
@$"<Project>
    <ItemGroup>
        <GlobalPackageReference Include=""X"" Version=""0.1.0"" />
    </ItemGroup>
</Project>";

                File.WriteAllText(Path.Combine(pathContext.SolutionRoot, "Directory.Packages.props"), propsFile);

                string projectContent =
@$"<Project  Sdk=""Microsoft.NET.Sdk"">
<PropertyGroup>                   
	<TargetFramework>net46</TargetFramework>
	</PropertyGroup>
    <ItemGroup>
        <PackageReference Include=""X""/>
    </ItemGroup>
</Project>";
                File.WriteAllText(Path.Combine(pathContext.SolutionRoot, ProjectName, string.Concat(ProjectName, ".csproj")), projectContent);

                var projectDirectory = Path.Combine(pathContext.SolutionRoot, ProjectName);
                var projectFilePath = Path.Combine(projectDirectory, $"{ProjectName}.csproj");

                _fixture.RunDotnetExpectSuccess(projectDirectory,
                    $"restore {projectFilePath}");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package");

                // Assert Requested version is 0.1.0, but the resolved version is 1.0.0
                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, "0.1.0"));
                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, "1.0.0"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_Transitive()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");

                var packageX = XPlatTestUtils.CreatePackage();
                var packageY = XPlatTestUtils.CreatePackage(packageId: "packageY");
                packageX.Dependencies.Add(packageY);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);


                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package");

                Assert.False(ContainsIgnoringSpaces(listResult.AllOutput, "packageY"));

                listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package --include-transitive");

                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, "packageY"));

            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("")]
        [InlineData(" --outdated")]
        [InlineData(" --vulnerable")]
        [InlineData(" --deprecated")]
        public async Task DotnetListPackage_DoesNotReturnProjects(string args)
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                string directDependencyProjectName = $"{ProjectName}Dependency";
                string transitiveDependencyProjectName = $"{ProjectName}TransitiveDependency";
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");
                var projectB = XPlatTestUtils.CreateProject(directDependencyProjectName, pathContext, "net46");
                var projectC = XPlatTestUtils.CreateProject(transitiveDependencyProjectName, pathContext, "net46");

                var packageX = XPlatTestUtils.CreatePackage(packageId: "packageX");
                var packageY = XPlatTestUtils.CreatePackage(packageId: "packageY");
                var packageZ = XPlatTestUtils.CreatePackage(packageId: "packageZ");
                var packageT = XPlatTestUtils.CreatePackage(packageId: "packageT");
                packageX.Dependencies.Add(packageT);
                packageY.Dependencies.Add(packageT);
                packageZ.Dependencies.Add(packageT);

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY,
                    packageZ,
                    packageT);

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} reference {projectB.ProjectPath}");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectB.ProjectPath} reference {projectC.ProjectPath}");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectB.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectB.ProjectPath).FullName,
                    $"add {projectB.ProjectPath} package packageY --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectB.ProjectPath).FullName,
                    $"add {projectC.ProjectPath} package packageZ --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package{args}");

                Assert.False(ContainsIgnoringSpaces(listResult.AllOutput, projectB.ProjectName));
                Assert.False(ContainsIgnoringSpaces(listResult.AllOutput, projectC.ProjectName));

                listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package{args} --include-transitive");

                Assert.False(ContainsIgnoringSpaces(listResult.AllOutput, projectB.ProjectName));
                Assert.False(ContainsIgnoringSpaces(listResult.AllOutput, projectC.ProjectName));
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("", "net48", null)]
        [InlineData("", "net46", null)]
        [InlineData("--framework net46 --framework net48", "net48", null)]
        [InlineData("--framework net46 --framework net48", "net46", null)]
        [InlineData("--framework net46", "net46", "net48")]
        [InlineData("--framework net48", "net48", "net46")]
        public async Task DotnetListPackage_FrameworkSpecific_Success(string args, string shouldInclude, string shouldntInclude)
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46;net48");

                var packageX = XPlatTestUtils.CreatePackage(frameworkString: "net46;net48");

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);


                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package {args}");

                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, shouldInclude.Replace(" ", "")));

                if (shouldntInclude != null)
                {
                    Assert.False(ContainsIgnoringSpaces(listResult.AllOutput, shouldntInclude.Replace(" ", "")));
                }

            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_InvalidFramework_Fail()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");

                var packageX = XPlatTestUtils.CreatePackage();

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);


                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                _fixture.RunDotnetExpectFailure(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package --framework net46 --framework invalidFramework");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void DotnetListPackage_DeprecatedAndOutdated_Fail()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");

                var listResult = _fixture.RunDotnetExpectFailure(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package --deprecated --outdated");

                Assert.Contains(string.Format(Strings.ListPkg_InvalidOptions, "--outdated", "--deprecated"), listResult.Errors);
            }
        }

        // In 2.2.100 of CLI. DotNet list package would show a section for each TFM and for each TFM/RID.
        // This is testing to ensure that it only shows one section for each TFM.
        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_ShowFrameworksOnly_SDK()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {

                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net461");

                projectA.Properties.Add("RuntimeIdentifiers", "win;win-x86;win-x64");

                var packageX = XPlatTestUtils.CreatePackage();

                projectA.AddPackageToAllFrameworks(packageX);

                projectA.Save();

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                //the assets file should generate 4 sections in Targets: 1 for TFM only , and 3 for TFM + RID combinations 
                var assetsFile = projectA.AssetsFile;
                Assert.Equal(4, assetsFile.Targets.Count);

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package");

                //make sure there is no duplicate in output
                Assert.True(NoDuplicateSection(listResult.AllOutput), listResult.AllOutput);

            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("1.0.0", "", "2.1.0")]
        [InlineData("1.0.0", "--include-prerelease", "2.2.0-beta")]
        [InlineData("1.0.0-beta", "", "2.1.0")]
        [InlineData("1.0.0", "--highest-patch", "1.0.9")]
        [InlineData("1.0.0", "--highest-minor", "1.9.0")]
        [InlineData("1.0.0", "--highest-patch --include-prerelease", "1.0.10-beta")]
        [InlineData("1.0.0", "--highest-minor --include-prerelease", "1.10.0-beta")]
        public async Task DotnetListPackage_Outdated_Succeed(string currentVersion, string args, string expectedVersion)
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net472");
                var versions = new List<string> { "1.0.0-beta", "1.0.0", "1.0.9", "1.0.10-beta", "1.9.0", "1.10.0-beta", "2.1.0", "2.2.0-beta" };
                foreach (var version in versions)
                {
                    var packageX = XPlatTestUtils.CreatePackage(packageId: "packageX", packageVersion: version);

                    // Generate Package
                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packageX);
                }

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --version {currentVersion} --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package --outdated {args}");

                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, $"packageX{currentVersion}{currentVersion}{expectedVersion}"));

            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_OutdatedWithNoVersionsFound_Succeeds()
        {
            // Arrange
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject("ProjectA", pathContext, "net472");
                var packageX = XPlatTestUtils.CreatePackage(packageId: "packageX", packageVersion: "1.0.0");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packageX);

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --version 1.0.0 --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                foreach (var nupkg in Directory.EnumerateDirectories(pathContext.PackageSource))
                {
                    Directory.Delete(nupkg, recursive: true);
                }

                // Act
                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package --outdated");

                // Assert
                string[] lines = listResult.AllOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                Assert.True(lines.Any(l => l.Contains("packageX") && l.Contains("Not found at the sources")), "Line containing 'packageX' and 'Not found at the sources' not found: " + listResult.AllOutput);
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("1.1.0-beta", "")]
        [InlineData("2.0.0-beta", "--highest-patch")]
        [InlineData("2.0.0-beta", "--highest-minor")]
        public async Task DotnetListPackage_OutdatedWithNoAvailableVersion_Succeeds(string currentVersion, string args)
        {
            // Arrange
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject("ProjectA", pathContext, "net472");
                var packageX = XPlatTestUtils.CreatePackage(packageId: "packageX", packageVersion: currentVersion);
                var packageY = XPlatTestUtils.CreatePackage(packageId: "packageY", packageVersion: "1.0.0");
                var packageY2 = XPlatTestUtils.CreatePackage(packageId: "packageY", packageVersion: "1.0.1");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                        pathContext.PackageSource,
                        PackageSaveMode.Defaultv3,
                        packageX,
                        packageY,
                        packageY2);

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --version {currentVersion} --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageY --version 1.0.0 --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                // Act
                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package --outdated {args}");

                // Assert
                string[] lines = listResult.AllOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                Assert.True(lines.Any(l => l.Contains("packageY") && l.Contains("1.0.1")), "Line containing 'packageY' and '1.0.1' not found: " + listResult.AllOutput);
                Assert.True(lines.Any(l => l.Contains("packageX") && l.Contains("Not found at the sources")), "Line containing 'packageX' and 'Not found at the sources' not found: " + listResult.AllOutput);
            }
        }

        // We read the original PackageReference items by calling the CollectPackageReference target.
        // If a project has InitialTargets we need to deal with the response properly in the C# based invocation.
        [PlatformFact(Platform.Windows)]
        public async Task DotnetListPackage_ProjectWithInitialTargets_Succeeds()
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var projectA = XPlatTestUtils.CreateProject(ProjectName, pathContext, "net46");

                var doc = XDocument.Parse(File.ReadAllText(projectA.ProjectPath));

                doc.Root.Add(new XAttribute("InitialTargets", "FirstTarget"));

                doc.Root.Add(new XElement(XName.Get("Target"),
                    new XAttribute(XName.Get("Name"), "FirstTarget"),
                    new XElement(XName.Get("Message"),
                        new XAttribute(XName.Get("Text"), "I am the first target invoked every time a target is called on this project!"))));

                File.WriteAllText(projectA.ProjectPath, doc.ToString());

                var packageX = XPlatTestUtils.CreatePackage();

                // Generate Package
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"add {projectA.ProjectPath} package packageX --version 1.0.0 --no-restore");

                _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"restore {projectA.ProjectName}.csproj");

                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName,
                    $"list {projectA.ProjectPath} package");

                Assert.True(ContainsIgnoringSpaces(listResult.AllOutput, "packageX1.0.01.0.0"));
            }
        }

        [PlatformTheory(Platform.Windows, Skip = "Enabled after .NET CLI integration. Tracked by https://github.com/NuGet/Home/issues/9800.")]
        [InlineData("", false)]
        [InlineData("--verbosity minimal", false)]
        [InlineData("--verbosity normal", true)]
        public void DotnetListPackage_VerbositySwitchTogglesHttpVisibility(string args, bool showsHttp)
        {
            using (var pathContext = _fixture.CreateSimpleTestPathContext())
            {
                var source = "https://api.nuget.org/v3/index.json";
                var emptyHttpCache = new Dictionary<string, string>
                {
                    { "NUGET_HTTP_CACHE_PATH", pathContext.HttpCacheFolder },
                };

                ProjectFileBuilder
                    .Create()
                    .WithProjectName(ProjectName)
                    .WithProperty("targetFramework", "net46")
                    .WithItem("PackageReference", "BaseTestPackage.SearchFilters", version: "1.1.0")
                    .WithProperty("RestoreSources", source)
                    .Build(_fixture, pathContext.SolutionRoot);

                var workingDirectory = Path.Combine(pathContext.SolutionRoot, ProjectName);
                _fixture.RunDotnetExpectSuccess(workingDirectory, $"restore {ProjectName}.csproj");

                // Act
                CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(
                    workingDirectory,
                    $"list package --outdated --source {source} {args}",
                    environmentVariables: emptyHttpCache);

                // Assert
                if (showsHttp)
                {
                    Assert.Contains("GET http", CollapseSpaces(listResult.AllOutput));
                }
                else
                {
                    Assert.DoesNotContain("GET http", CollapseSpaces(listResult.AllOutput));
                }

                Assert.Contains("BaseTestPackage.SearchFilters 1.1.0 1.1.0 1.3.0", CollapseSpaces(listResult.AllOutput));
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("true", false)]
        [InlineData("false", true)]
        public async Task ListPackage_WithHttpSourceAndAllowInsecureConnections_WarnsCorrectly(string allowInsecureConnections, bool isHttpWarningExpected)
        {
            // Arrange
            using var pathContext = _fixture.CreateSimpleTestPathContext();
            var emptyHttpCache = new Dictionary<string, string>
                {
                    { "NUGET_HTTP_CACHE_PATH", pathContext.HttpCacheFolder },
                };

            var packageA100 = new SimpleTestPackageContext("A", "1.0.0");
            var packageA200 = new SimpleTestPackageContext("A", "2.0.0");

            var projectA = XPlatTestUtils.CreateProject("ProjectA", pathContext, "net472");

            await SimpleTestPackageUtility.CreatePackagesAsync(
                    pathContext.PackageSource,
                    packageA100,
                    packageA200);

            using var mockServer = new FileSystemBackedV3MockServer(pathContext.PackageSource);
            mockServer.Start();
            pathContext.Settings.AddSource("http-source", mockServer.ServiceIndexUri, allowInsecureConnections);

            _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName, $"add package A --version 1.0.0");

            // Act
            CommandRunnerResult listResult = _fixture.RunDotnetExpectSuccess(Directory.GetParent(projectA.ProjectPath).FullName, $"list package --outdated");
            mockServer.Stop();

            // Assert
            var lines = listResult.AllOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(lines.Any(l => l.Contains("> A                    1.0.0       1.0.0      2.0.0")), listResult.AllOutput);
            if (isHttpWarningExpected)
            {
                Assert.Contains("warn : You are running the 'list package' operation with an 'HTTP' source", listResult.AllOutput);
            }
            else
            {
                Assert.DoesNotContain("warn : You are running the 'list package' operation with an 'HTTP' source", listResult.AllOutput);
            }
        }

        private static string CollapseSpaces(string input)
        {
            return Regex.Replace(input, " +", " ");
        }

        private static bool ContainsIgnoringSpaces(string output, string pattern)
        {
            var commandResultNoSpaces = output.Replace(" ", "");

            return commandResultNoSpaces.ToLowerInvariant().Contains(pattern.ToLowerInvariant());
        }

        private static bool NoDuplicateSection(string output)
        {
            var sections = output.Replace(" ", "").Replace("\r", "").Replace("\n", "").Split("[");
            if (sections.Length == 1)
            {
                return false;
            }

            for (var i = 1; i <= sections.Length - 2; i++)
            {
                for (var j = i + 1; j <= sections.Length - 1; j++)
                {
                    if (sections[i].Equals(sections[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

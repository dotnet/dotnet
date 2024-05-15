// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Msbuild.Integration.Test
{
    public class MsbuildRestoreTaskPackageSourceMappingTests : IClassFixture<MsbuildIntegrationTestFixture>
    {
        private readonly MsbuildIntegrationTestFixture _msbuildFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public MsbuildRestoreTaskPackageSourceMappingTests(MsbuildIntegrationTestFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _msbuildFixture = fixture;
            _testOutputHelper = testOutputHelper;
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_PackageSourceMappingFull_Succeed()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = new SimpleTestProjectContext(
                    "a",
                    ProjectStyle.PackagesConfig,
                    pathContext.SolutionRoot);
                projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                using (var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "packages.config")))
                {
                    writer.Write(
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
  <package id=""Contoso.Opensource.Buffers"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");
                }

                var opensourceRepositoryPath = pathContext.PackageSource;
                Directory.CreateDirectory(opensourceRepositoryPath);

                var packageOpenSourceContosoMvc = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",  // Package Id conflict with internally created package
                    Version = "1.0.0"
                };
                packageOpenSourceContosoMvc.Files.Clear();
                packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageOpenSourceContosoMvc);

                var packageContosoBuffersOpenSource = new SimpleTestPackageContext()
                {
                    Id = "Contoso.Opensource.Buffers",
                    Version = "1.0.0"
                };
                packageContosoBuffersOpenSource.Files.Clear();
                packageContosoBuffersOpenSource.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageContosoBuffersOpenSource);

                var sharedRepositoryPath = pathContext.UserPackagesFolder;
                Directory.CreateDirectory(sharedRepositoryPath);

                var packageContosoMvcReal = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",
                    Version = "1.0.0"
                };
                packageContosoMvcReal.Files.Clear();
                packageContosoMvcReal.AddFile("lib/net461/realA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    sharedRepositoryPath,
                    packageContosoMvcReal);

                // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
                // so we go ahead and replace that config before running MSBuild.
                var configPath = Path.Combine(Path.GetDirectoryName(pathContext.SolutionRoot), "NuGet.Config");
                var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
    <add key=""SharedRepository"" value=""{sharedRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PublicRepository"">
            <package pattern=""Contoso.Opensource.*"" />
        </packageSource>
        <packageSource key=""SharedRepository"">
            <package pattern=""Contoso.MVC.*"" /> <!--Contoso.MVC.ASP package exist in both repository but it'll restore from this one -->
        </packageSource>
    </packageSourceMapping>
</configuration>";
                using (var writer = new StreamWriter(configPath))
                {
                    writer.Write(configText);
                }

                // Act
                var result = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore {pathContext.SolutionRoot} /p:RestorePackagesConfig=true", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 0);
                var contosoRestorePath = Path.Combine(projectAPackages, packageOpenSourceContosoMvc.ToString(), packageOpenSourceContosoMvc.ToString() + ".nupkg");
                using (var nupkgReader = new PackageArchiveReader(contosoRestorePath))
                {
                    var allFiles = nupkgReader.GetFiles().ToList();
                    // Assert correct Contoso package was restored.
                    Assert.Contains("lib/net461/realA.dll", allFiles);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_PackageSourceMappingFull_Fails()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = new SimpleTestProjectContext(
                    "a",
                    ProjectStyle.PackagesConfig,
                    pathContext.SolutionRoot);
                projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                using (var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "packages.config")))
                {
                    writer.Write(
@"<packages>
<package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
<package id=""Contoso.Opensource.Buffers"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");
                }

                var opensourceRepositoryPath = pathContext.PackageSource;
                Directory.CreateDirectory(opensourceRepositoryPath);

                var packageOpenSourceContosoMvc = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",  // Package Id conflict with internally created package
                    Version = "1.0.0"
                };
                packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageOpenSourceContosoMvc);

                var packageContosoBuffersOpenSource = new SimpleTestPackageContext()
                {
                    Id = "Contoso.Opensource.Buffers",
                    Version = "1.0.0"
                };
                packageContosoBuffersOpenSource.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageContosoBuffersOpenSource);

                var sharedRepositoryPath = pathContext.UserPackagesFolder;
                Directory.CreateDirectory(sharedRepositoryPath);

                // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
                // so we go ahead and replace that config before running MSBuild.
                var configPath = Path.Combine(Path.GetDirectoryName(pathContext.SolutionRoot), "NuGet.Config");
                var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
<packageSources>
<!--To inherit the global NuGet package sources remove the <clear/> line below -->
<clear />
<add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
<add key=""SharedRepository"" value=""{sharedRepositoryPath}"" />
</packageSources>
<packageSourceMapping>
    <packageSource key=""PublicRepository"">
        <package pattern=""Contoso.Opensource.*"" />
    </packageSource>
    <packageSource key=""SharedRepository"">
        <package pattern=""Contoso.MVC.*"" /> <!--Contoso.MVC.ASP package doesn't exist in this repostiry, so it'll fail to restore -->
    </packageSource>
</packageSourceMapping>
</configuration>";
                using (var writer = new StreamWriter(configPath))
                {
                    writer.Write(configText);
                }

                // Act
                var result = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore {pathContext.SolutionRoot} /p:RestorePackagesConfig=true", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 1);
                var packageContosoBuffersPath = Path.Combine(projectAPackages, packageContosoBuffersOpenSource.ToString(), packageContosoBuffersOpenSource.ToString() + ".nupkg");
                Assert.True(File.Exists(packageContosoBuffersPath));
                // Assert Contoso.MVC.ASP is not restored.
                Assert.Contains("Unable to find version '1.0.0' of package 'Contoso.MVC.ASP'.", result.Output);
                var packageContosoMvcPath = Path.Combine(projectAPackages, packageOpenSourceContosoMvc.ToString(), packageOpenSourceContosoMvc.ToString() + ".nupkg");
                Assert.False(File.Exists(packageContosoMvcPath));
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_PackageSourceMappingPartial_Succeed()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = new SimpleTestProjectContext(
                    "a",
                    ProjectStyle.PackagesConfig,
                    pathContext.SolutionRoot);
                projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                using (var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "packages.config")))
                {
                    writer.Write(
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
  <package id=""Contoso.Opensource.Buffers"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");
                }

                var opensourceRepositoryPath = pathContext.PackageSource;
                Directory.CreateDirectory(opensourceRepositoryPath);

                var packageOpenSourceContosoMvc = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",  // Package Id conflict with internally created package
                    Version = "1.0.0"
                };
                packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageOpenSourceContosoMvc);

                var packageContosoBuffersOpenSource = new SimpleTestPackageContext()
                {
                    Id = "Contoso.Opensource.Buffers",
                    Version = "1.0.0"
                };
                packageContosoBuffersOpenSource.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageContosoBuffersOpenSource);

                var sharedRepositoryPath = pathContext.UserPackagesFolder;
                Directory.CreateDirectory(sharedRepositoryPath);

                var packageContosoMvcReal = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",
                    Version = "1.0.0"
                };
                packageContosoMvcReal.AddFile("lib/net461/realA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    sharedRepositoryPath,
                    packageContosoMvcReal);

                // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
                // so we go ahead and replace that config before running MSBuild.
                var configPath = Path.Combine(Path.GetDirectoryName(pathContext.SolutionRoot), "NuGet.Config");
                var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
    <add key=""SharedRepository"" value=""{sharedRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PublicRepository"">
            <package pattern=""Contoso.O*"" />
        </packageSource>
        <packageSource key=""SharedRepository"">
            <package pattern=""Contoso.M*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>";
                using (var writer = new StreamWriter(configPath))
                {
                    writer.Write(configText);
                }

                // Act
                var result = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore {pathContext.SolutionRoot} /p:RestorePackagesConfig=true", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 0);
                var contosoRestorePath = Path.Combine(projectAPackages, packageOpenSourceContosoMvc.ToString(), packageOpenSourceContosoMvc.ToString() + ".nupkg");
                using (var nupkgReader = new PackageArchiveReader(contosoRestorePath))
                {
                    var allFiles = nupkgReader.GetFiles().ToList();
                    // Assert correct Contoso package was restored.
                    Assert.Contains("lib/net461/realA.dll", allFiles);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_PackageSourceMappingPartial_Fails()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = new SimpleTestProjectContext(
                    "a",
                    ProjectStyle.PackagesConfig,
                    pathContext.SolutionRoot);
                projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                using (var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "packages.config")))
                {
                    writer.Write(
@"<packages>
<package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
<package id=""Contoso.Opensource.Buffers"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");
                }

                var opensourceRepositoryPath = pathContext.PackageSource;
                Directory.CreateDirectory(opensourceRepositoryPath);

                var packageOpenSourceContosoMvc = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",  // Package Id conflict with internally created package
                    Version = "1.0.0"
                };
                packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageOpenSourceContosoMvc);

                var packageContosoBuffersOpenSource = new SimpleTestPackageContext()
                {
                    Id = "Contoso.Opensource.Buffers",
                    Version = "1.0.0"
                };
                packageContosoBuffersOpenSource.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageContosoBuffersOpenSource);

                var sharedRepositoryPath = pathContext.UserPackagesFolder;
                Directory.CreateDirectory(sharedRepositoryPath);

                // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
                // so we go ahead and remove that config before running MSBuild.
                var configPath = Path.Combine(Path.GetDirectoryName(pathContext.SolutionRoot), "NuGet.Config");
                var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
<packageSources>
<!--To inherit the global NuGet package sources remove the <clear/> line below -->
<clear />
<add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
<add key=""SharedRepository"" value=""{sharedRepositoryPath}"" />
</packageSources>
<packageSourceMapping>
    <packageSource key=""PublicRepository"">
        <package pattern=""Contoso.O*"" />
    </packageSource>
    <packageSource key=""SharedRepository"">
        <package pattern=""Contoso.M*"" />  <!--Contoso.MVC.ASP package doesn't exist in this repostiry, so it'll fail to restore -->
    </packageSource>
</packageSourceMapping>
</configuration>";
                using (var writer = new StreamWriter(configPath))
                {
                    writer.Write(configText);
                }

                // Act
                var result = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore {pathContext.SolutionRoot} /p:RestorePackagesConfig=true", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 1);
                var packageContosoBuffersPath = Path.Combine(projectAPackages, packageContosoBuffersOpenSource.ToString(), packageContosoBuffersOpenSource.ToString() + ".nupkg");
                Assert.True(File.Exists(packageContosoBuffersPath));
                // Assert Contoso.MVC.ASP is not restored.
                Assert.Contains("Unable to find version '1.0.0' of package 'Contoso.MVC.ASP'.", result.Output);
                var packageContosoMvcPath = Path.Combine(projectAPackages, packageOpenSourceContosoMvc.ToString(), packageOpenSourceContosoMvc.ToString() + ".nupkg");
                Assert.False(File.Exists(packageContosoMvcPath));
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_PackageSourceMappingLongerMatches_Succeed()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = new SimpleTestProjectContext(
                    "a",
                    ProjectStyle.PackagesConfig,
                    pathContext.SolutionRoot);
                projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                using (var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "packages.config")))
                {
                    writer.Write(
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
  <package id=""Contoso.Opensource.Buffers"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");
                }

                var opensourceRepositoryPath = pathContext.PackageSource;
                Directory.CreateDirectory(opensourceRepositoryPath);

                var packageOpenSourceContosoMvc = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",  // Package Id conflict with internally created package
                    Version = "1.0.0"
                };
                packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageOpenSourceContosoMvc);

                var packageContosoBuffersOpenSource = new SimpleTestPackageContext()
                {
                    Id = "Contoso.Opensource.Buffers",
                    Version = "1.0.0"
                };
                packageContosoBuffersOpenSource.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageContosoBuffersOpenSource);

                var sharedRepositoryPath = pathContext.UserPackagesFolder;
                Directory.CreateDirectory(sharedRepositoryPath);

                var packageContosoMvcReal = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",
                    Version = "1.0.0"
                };
                packageContosoMvcReal.AddFile("lib/net461/realA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    sharedRepositoryPath,
                    packageContosoMvcReal);

                // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
                // so we go ahead and remove that config before running MSBuild.
                var configPath = Path.Combine(Path.GetDirectoryName(pathContext.SolutionRoot), "NuGet.Config");
                var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
    <add key=""SharedRepository"" value=""{sharedRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PublicRepository"">
            <package pattern=""Contoso.Opensource.*"" />
            <package pattern=""Contoso.MVC.*"" />
        </packageSource>
        <packageSource key=""SharedRepository"">
            <package pattern=""Contoso.MVC.ASP"" />   <!-- Longer prefix prevails over Contoso.MVC.* -->
        </packageSource>
    </packageSourceMapping>
</configuration>";

                using (var writer = new StreamWriter(configPath))
                {
                    writer.Write(configText);
                }

                // Act
                var result = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore -v:d {pathContext.SolutionRoot} /p:RestorePackagesConfig=true", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 0);
                var contosoRestorePath = Path.Combine(projectAPackages, packageOpenSourceContosoMvc.ToString(), packageOpenSourceContosoMvc.ToString() + ".nupkg");
                using (var nupkgReader = new PackageArchiveReader(contosoRestorePath))
                {
                    var allFiles = nupkgReader.GetFiles().ToList();
                    // Assert correct Contoso package was restored.
                    Assert.Contains("lib/net461/realA.dll", allFiles);
                }

                Assert.True(result.ExitCode == 0);
                Assert.Contains("Package source mapping matches found for package ID 'Contoso.MVC.ASP' are: 'SharedRepository'", result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_PackageSourceMappingLongerMatches_NoNamespaceMatchesLog()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = new SimpleTestProjectContext(
                    "a",
                    ProjectStyle.PackagesConfig,
                    pathContext.SolutionRoot);
                projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                using (var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "packages.config")))
                {
                    writer.Write(
@"<packages>
  <package id=""My.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");
                }

                var opensourceRepositoryPath = pathContext.PackageSource;
                Directory.CreateDirectory(opensourceRepositoryPath);

                var packageOpenSourceContosoMvc = new SimpleTestPackageContext()
                {
                    Id = "My.MVC.ASP",  // Package Id conflict with internally created package
                    Version = "1.0.0"
                };
                packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageOpenSourceContosoMvc);

                var packageContosoBuffersOpenSource = new SimpleTestPackageContext()
                {
                    Id = "Contoso.Opensource.Buffers",
                    Version = "1.0.0"
                };
                packageContosoBuffersOpenSource.AddFile("lib/net461/openA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    opensourceRepositoryPath,
                    packageContosoBuffersOpenSource);

                // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
                // so we go ahead and remove that config before running MSBuild.
                var configPath = Path.Combine(Path.GetDirectoryName(pathContext.SolutionRoot), "NuGet.Config");
                var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PublicRepository"">
            <package pattern=""Contoso.MVC.ASP"" />   <!-- My.MVC.ASP doesn't match any package name spaces -->
        </packageSource>
    </packageSourceMapping>
</configuration>";

                using (var writer = new StreamWriter(configPath))
                {
                    writer.Write(configText);
                }

                // Act
                var result = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore -v:d {pathContext.SolutionRoot} /p:RestorePackagesConfig=true", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.Equal(1, result.ExitCode);
                Assert.Contains("Package source mapping match not found for package ID 'My.MVC.ASP'", result.Output);
                Assert.Contains("error : Unable to find version '1.0.0' of package 'My.MVC.ASP'.", result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_PackageSourceMappingPatternMatchesMultipleSources_Succeed()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = new SimpleTestProjectContext(
                    "a",
                    ProjectStyle.PackagesConfig,
                    pathContext.SolutionRoot);
                projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                using (var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "packages.config")))
                {
                    writer.Write(
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");
                }

                var sharedRepositoryPath1 = pathContext.UserPackagesFolder;
                Directory.CreateDirectory(sharedRepositoryPath1);

                var packageContosoMvcReal1 = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",
                    Version = "1.0.0"
                };
                packageContosoMvcReal1.AddFile("lib/net461/realA1.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    sharedRepositoryPath1,
                    packageContosoMvcReal1);

                var sharedRepositoryPath2 = pathContext.UserPackagesFolder;
                Directory.CreateDirectory(sharedRepositoryPath2);

                var packageContosoMvcReal2 = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",
                    Version = "1.0.0"
                };
                packageContosoMvcReal2.AddFile("lib/net461/realA2.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    sharedRepositoryPath2,
                    packageContosoMvcReal2);

                // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
                // so we go ahead and remove that config before running MSBuild.
                var configPath = Path.Combine(Path.GetDirectoryName(pathContext.SolutionRoot), "NuGet.Config");
                var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""SharedRepository1"" value=""{sharedRepositoryPath1}"" />
    <add key=""SharedRepository2"" value=""{sharedRepositoryPath2}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""SharedRepository1"">
            <package pattern=""Contoso.MVC.*"" /> <!--Same package source mapping prefix matches both repository -->
        </packageSource>
        <packageSource key=""SharedRepository2"">
            <package pattern=""Contoso.MVC.*"" /> <!--Same package source mapping prefix matches both repository -->
        </packageSource>
    </packageSourceMapping>
</configuration>";
                using (var writer = new StreamWriter(configPath))
                {
                    writer.Write(configText);
                }

                // Act
                var result = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore {pathContext.SolutionRoot} /p:RestorePackagesConfig=true", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 0);
                var contosoRestorePath = Path.Combine(projectAPackages, packageContosoMvcReal1.ToString(), packageContosoMvcReal1.ToString() + ".nupkg");
                using (var nupkgReader = new PackageArchiveReader(contosoRestorePath))
                {
                    var allFiles = nupkgReader.GetFiles().ToList();
                    // Assert correct Contoso package was restored.
                    Assert.True(allFiles.Contains("lib/net461/realA1.dll") || allFiles.Contains("lib/net461/realA2.dll"));
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_NoPackageSourceMappingsection_NoSourceRelatedLogMessage()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = new SimpleTestProjectContext(
                    "a",
                    ProjectStyle.PackagesConfig,
                    pathContext.SolutionRoot);
                projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                using (var writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "packages.config")))
                {
                    writer.Write(
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");
                }

                var packageContosoMvcReal = new SimpleTestPackageContext()
                {
                    Id = "Contoso.MVC.ASP",
                    Version = "1.0.0"
                };
                packageContosoMvcReal.AddFile("lib/net461/realA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    packageContosoMvcReal);

                // Act
                var result = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore {pathContext.SolutionRoot} /p:RestorePackagesConfig=true", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

                // Assert
                result.Success.Should().BeTrue(because: result.AllOutput);
                result.AllOutput.Should().NotContain("source mapping");
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_PR_PackageSourceMapping_WithAllRestoreSourceProperies_Succeed()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            // Set up solution, project, and packages
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var workingPath = pathContext.WorkingDirectory;
            var opensourceRepositoryPath = Path.Combine(workingPath, "PublicRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            var privateRepositoryPath = Path.Combine(workingPath, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var net461 = NuGetFramework.Parse("net461");
            var projectA = new SimpleTestProjectContext(
                "a",
                ProjectStyle.PackageReference,
                pathContext.SolutionRoot);
            projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));

            // Add both repositories as RestoreSources
            projectA.Properties.Add("RestoreSources", $"{opensourceRepositoryPath};{privateRepositoryPath}");

            var packageOpenSourceA = new SimpleTestPackageContext("Contoso.Opensource.A", "1.0.0");
            packageOpenSourceA.AddFile("lib/net461/openA.dll");

            var packageOpenSourceContosoMvc = new SimpleTestPackageContext("Contoso.MVC.ASP", "1.0.0"); // Package Id conflict with internally created package
            packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

            var packageContosoMvcReal = new SimpleTestPackageContext("Contoso.MVC.ASP", "1.0.0");
            packageContosoMvcReal.AddFile("lib/net461/realA.dll");

            projectA.AddPackageToAllFrameworks(packageOpenSourceA);
            projectA.AddPackageToAllFrameworks(packageOpenSourceContosoMvc);
            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
            // so we go ahead and replace that config before running MSBuild.
            var configAPath = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "NuGet.Config");
            var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PublicRepository"">
            <package pattern=""Contoso.Opensource.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.MVC.*"" /> <!--Contoso.MVC.ASP package exist in both repository but it'll restore from this one -->
        </packageSource>
    </packageSourceMapping>
</configuration>";
            using (var writer = new StreamWriter(configAPath))
            {
                writer.Write(configText);
            }

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                opensourceRepositoryPath,
                packageOpenSourceA,
                packageOpenSourceContosoMvc);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                privateRepositoryPath,
                packageContosoMvcReal);

            // Act
            var r = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore -v:d {pathContext.SolutionRoot}", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

            // Assert
            Assert.True(r.ExitCode == 0);
            Assert.Contains($"Package source mapping matches found for package ID 'Contoso.MVC.ASP' are: 'PrivateRepository'", r.Output);
            Assert.Contains($"Package source mapping matches found for package ID 'Contoso.Opensource.A' are: 'PublicRepository'", r.Output);
            var localResolver = new VersionFolderPathResolver(pathContext.UserPackagesFolder);
            var contosoMvcMetadataPath = localResolver.GetNupkgMetadataPath(packageContosoMvcReal.Identity.Id, packageContosoMvcReal.Identity.Version);
            NupkgMetadataFile contosoMvcmetadata = NupkgMetadataFileFormat.Read(contosoMvcMetadataPath);
            Assert.Equal(privateRepositoryPath, contosoMvcmetadata.Source);
        }

        [PlatformFact(Platform.Windows)]
        public async Task MsbuildRestore_PR_PackageSourceMapping_WithNotEnoughRestoreSourceProperty_Fails()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            // Set up solution, project, and packages
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var workingPath = pathContext.WorkingDirectory;
            var opensourceRepositoryPath = Path.Combine(workingPath, "PublicRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);

            var privateRepositoryPath = Path.Combine(workingPath, "PrivateRepository");
            Directory.CreateDirectory(privateRepositoryPath);

            var net461 = NuGetFramework.Parse("net461");
            var projectA = new SimpleTestProjectContext(
                "a",
                ProjectStyle.PackageReference,
                pathContext.SolutionRoot);
            projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));

            // Add only 1 repository as RestoreSources
            projectA.Properties.Add("RestoreSources", $"{opensourceRepositoryPath}");

            var packageOpenSourceA = new SimpleTestPackageContext("Contoso.Opensource.A", "1.0.0");
            packageOpenSourceA.AddFile("lib/net461/openA.dll");

            var packageOpenSourceContosoMvc = new SimpleTestPackageContext("Contoso.MVC.ASP", "1.0.0"); // Package Id conflict with internally created package
            packageOpenSourceContosoMvc.AddFile("lib/net461/openA.dll");

            var packageContosoMvcReal = new SimpleTestPackageContext("Contoso.MVC.ASP", "1.0.0");
            packageContosoMvcReal.AddFile("lib/net461/realA.dll");

            projectA.AddPackageToAllFrameworks(packageOpenSourceA);
            projectA.AddPackageToAllFrameworks(packageOpenSourceContosoMvc);
            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            // SimpleTestPathContext adds a NuGet.Config with a repositoryPath,
            // so we go ahead and replace that config before running MSBuild.
            var configAPath = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "NuGet.Config");
            var configText =
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{opensourceRepositoryPath}"" />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""PublicRepository"">
            <package pattern=""Contoso.Opensource.*"" />
        </packageSource>
        <packageSource key=""PrivateRepository"">
            <package pattern=""Contoso.MVC.*"" /> <!--Contoso.MVC.ASP package exist in both repository but it'll restore from this one -->
        </packageSource>
    </packageSourceMapping>
</configuration>";
            using (var writer = new StreamWriter(configAPath))
            {
                writer.Write(configText);
            }

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                opensourceRepositoryPath,
                packageOpenSourceA,
                packageOpenSourceContosoMvc);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                privateRepositoryPath,
                packageContosoMvcReal);

            // Act
            var r = _msbuildFixture.RunMsBuild(pathContext.WorkingDirectory, $"/t:restore -v:d {pathContext.SolutionRoot}", ignoreExitCode: true, testOutputHelper: _testOutputHelper);

            // Assert
            Assert.True(r.ExitCode == 1);
            Assert.Contains($"Package source mapping match not found for package ID 'Contoso.MVC.ASP'.", r.Output);
            Assert.Contains($"Package source mapping matches found for package ID 'Contoso.Opensource.A' are: 'PublicRepository'", r.Output);
        }
    }
}

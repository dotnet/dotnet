// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetRestoreCommandTest
    {
        private const int _failureCode = 1;
        private const int _successCode = 0;

        [Theory]
        [InlineData("bad/pat.h/myfile.blah")]
        [InlineData("**/*.sln")]
        public void RestoreCommand_BadInputPath(string solutionPath)
        {
            using (var randomTestFolder = TestDirectory.Create())
            {
                // Arrange
                var nugetexe = Util.GetNuGetExePath();

                var args = new string[]
                {
                    "restore",
                    solutionPath,
                    "-PackagesDirectory",
                    randomTestFolder
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Assert.NotEqual(_successCode, r.ExitCode);
                var error = r.Errors;
                Assert.Contains("Input file does not exist: " + solutionPath, r.Errors, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void RestoreCommand_MissingSolutionFile()
        {
            using (var randomTestFolder = TestDirectory.Create())
            {
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var solutionPath = Path.Combine(randomTestFolder, "solution.sln");

                var args = new string[]
                {
                    "restore",
                    solutionPath,
                    "-PackagesDirectory",
                    randomTestFolder
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Assert.NotEqual(_successCode, r.ExitCode);
                var error = r.Errors;
                Assert.Contains("Input file does not exist:", r.Errors, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void TestVerbosityQuiet_ShowsErrorMessages()
        {
            using (var randomTestFolder = TestDirectory.Create())
            {
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var solutionPath = Path.Combine(randomTestFolder, "solution.sln");

                var args = new string[]
                {
                    "restore",
                    solutionPath,
                    "-PackagesDirectory",
                    randomTestFolder,
                    "-Verbosity",
                    "Quiet"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Assert.NotEqual(_successCode, r.ExitCode);
                var error = r.Errors;
                Assert.Contains("Input file does not exist:", r.Errors, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void RestoreCommand_MissingPackagesConfigFile()
        {
            using (var randomTestFolder = TestDirectory.Create())
            {
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var packagesConfigPath = Path.Combine(randomTestFolder, "packages.config");

                var args = new string[]
                {
                    "restore",
                    packagesConfigPath,
                    "-PackagesDirectory",
                    randomTestFolder
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Assert.NotEqual(_successCode, r.ExitCode);
                var error = r.Errors;
                Assert.Contains("input file does not exist", r.Errors, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void RestoreCommand_FromPackagesConfigFile()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = pathContext.PackageSource;
                Directory.CreateDirectory(repositoryPath);
                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", repositoryPath);
                Util.CreateFile(workingPath, "packages.config",
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                string[] args = new string[] { "restore", "-PackagesDirectory", "outputDir", "-Source", repositoryPath };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var packageFileA = Path.Combine(workingPath, @"outputDir", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(workingPath, @"outputDir", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public async Task RestoreCommand_MissingNuspecFileInPackage_FailsWithNU5037()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = pathContext.PackageSource;
                var a = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
                await SimpleTestPackageUtility.CreateFolderFeedV2Async(repositoryPath, a);
                await SimpleTestPackageUtility.DeleteNuspecFileFromPackageAsync(Path.Combine(repositoryPath, a.ToString() + NuGetConstants.PackageExtension));
                Util.CreateFile(workingPath, "packages.config",
@"<packages>
  <package id=""a"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");

                string[] args = new string[] { "restore", "-PackagesDirectory", "outputDir", "-Source", repositoryPath };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                var packageFileA = Path.Combine(workingPath, @"outputDir", "a.1.1.0", "a.1.1.0.nupkg");
                Assert.False(File.Exists(packageFileA));
                Assert.Equal(_failureCode, r.ExitCode);
                Assert.Contains("The package is missing the required nuspec file.", r.AllOutput);
            }
        }

        [Fact(Skip = "Inconsistent")]
        public void RestoreCommand_NoCancelledOrNotFoundMessages()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var nugetexe = Util.GetNuGetExePath();

                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                var sourcePath = Path.Combine(workingPath, "source");
                Directory.CreateDirectory(sourcePath);

                var packagesPath = Path.Combine(workingPath, "packages");
                Directory.CreateDirectory(packagesPath);

                var packageA = new FileInfo(Util.CreateTestPackage("PackageA", "1.1.0", sourcePath));
                var packageB = new FileInfo(Util.CreateTestPackage("PackageB", "2.2.0", sourcePath));

                Util.CreateFile(workingPath, "packages.config",
@"<packages>
  <package id=""PackageA"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");

                using (var serverWithPackage = Util.CreateMockServer(new[] { packageA }))
                using (var serverWithoutPackage = Util.CreateMockServer(new[] { packageB }))
                using (var slowServer = new MockServer())
                {
                    slowServer.Get.Add("/", request =>
                    {
                        return new Action<HttpListenerResponse>(response =>
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(5));
                            response.StatusCode = 500;
                        });
                    });

                    serverWithPackage.Start();
                    serverWithoutPackage.Start();
                    slowServer.Start();

                    string[] args =
                    {
                        "restore",
                        "-PackagesDirectory", packagesPath,
                        "-Source", serverWithPackage.Uri + "nuget",
                        "-Source", serverWithoutPackage.Uri + "nuget",
                        "-Source", slowServer.Uri + "nuget"
                    };

                    // Act
                    var result = CommandRunner.Run(
                        nugetexe,
                        workingPath,
                        string.Join(" ", args));

                    // Assert
                    Assert.True(result.Errors == string.Empty, $"There should not be any STDERR:{Environment.NewLine}{result.Errors}");
                    Assert.True(result.Output != string.Empty, $"There should be some STDOUT.");
                    Assert.DoesNotContain("cancel", result.Output, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain("not found", result.Output, StringComparison.OrdinalIgnoreCase);
                    Assert.Equal(_successCode, result.ExitCode);
                    Assert.True(File.Exists(Path.Combine(packagesPath, @"PackageA.1.1.0", "PackageA.1.1.0.nupkg")));
                }
            }
        }

        [Theory]
        [InlineData("packages.config")]
        [InlineData("packages.proj2.config")]
        public void RestoreCommand_FromSolutionFile(string configFileName)
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();


            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, "packages.config", configFileName);

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public void RestoreCommand_FromFilteredSolutionFile()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Util.CreateBasicTwoProjectSolutionWithSolutionFilters(workingPath, "packages.config", "packages.config");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore " + Path.Combine(workingPath, "a.proj1.slnf") + " -Source " + repositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.False(File.Exists(packageFileB));
            }

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Util.CreateBasicTwoProjectSolutionWithSolutionFilters(workingPath, "packages.config", "packages.config");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore " + Path.Combine(workingPath, "a.proj2.slnf") + " -Source " + repositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.False(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Util.CreateBasicTwoProjectSolutionWithSolutionFilters(workingPath, "packages.config", "packages.config");

                // Act
                var r = CommandRunner.Run(nugetexe,
                    workingPath, "restore " + Path.Combine(workingPath, "filter", "filterinsubfolder.slnf") + " -Source " + repositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public void RestoreCommand_FromProjectFile()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = pathContext.PackageSource;

                var proj1Directory = Path.Combine(workingPath, "proj1");
                Directory.CreateDirectory(proj1Directory);

                var currentDirectory = Directory.GetCurrentDirectory();
                var nugetexe = Util.GetNuGetExePath();

                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", repositoryPath);

                var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
                File.WriteAllText(
                    proj1File,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                File.WriteAllText(
                    Path.Combine(proj1Directory, "packages.config"),
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                string[] args = new string[]
                    {
                        "restore",
                        proj1File,
                        "-Source",
                        repositoryPath,
                        "-solutionDir",
                        workingPath
                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public void RestoreCommand_FromSolutionFileNoProjects_ReportsNothingToDoWithoutError()
        {
            // Verify we display a simple informational message, no errors

            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                Directory.CreateDirectory(workingPath);

                Util.CreateFile(workingPath, "a.sln",
                    @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 14
");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore");

                // Assert
                Assert.True(string.IsNullOrEmpty(r.Errors)); // No error
                Assert.Contains("Nothing to do", r.Output); // Informative message
            }
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "packages.config")]
        [InlineData("project.json", "")]
        public void RestoreCommand_FromSolutionFile_ReportsNothingToDoWithoutError(string proj1ConfigFileName, string proj2ConfigFileName)
        {
            // Verify we display a simple informational message if we don't encounter any projects with project.json
            // or packages.config, no errors.

            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, proj1ConfigFileName, proj2ConfigFileName, redirectGlobalPackagesFolder: false);
                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + "" + r.Errors);
                Assert.True(string.IsNullOrEmpty(r.Errors)); // No error

                if (string.IsNullOrEmpty(proj1ConfigFileName) && string.IsNullOrEmpty(proj2ConfigFileName))
                {
                    Assert.Contains("Nothing to do", r.Output); // Informative message
                }
                else
                {
                    Assert.DoesNotContain("Nothing to do", r.Output);
                }
            }

        }

        [CIOnlyTheory(Skip = "https://github.com/NuGet/Home/issues/9303")]
        [InlineData("packages.config")]
        [InlineData("packages.proj2.config")]
        public void RestoreCommand_FromSolutionFileWithMsbuild12(string configFileName)
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, "packages.config", configFileName);

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath + @" -msbuildversion 12");

                // Assert
                Assert.True(_successCode == r.ExitCode, $"Expected: {_successCode} - Actual: {r.ExitCode}{Environment.NewLine} {r.AllOutput}");
                var packageFileA = Path.Combine(workingPath, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(workingPath, @"packages", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public void RestoreCommand_FromSolutionFileWithMsbuildPath()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();


            var msbuildPath = Util.GetMsbuildPathOnWindows();
            if (RuntimeEnvironmentHelper.IsMono && RuntimeEnvironmentHelper.IsMacOSX)
            {
                msbuildPath = @"/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/msbuild/15.0/bin/";
            }

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, "packages.config", "packages.config");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath + $@" -MSBuildPath ""{msbuildPath}"" ");

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output);
                Assert.True(r.Output.Contains($"Using Msbuild from '{msbuildPath}'."));
                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public void RestoreCommand_FromSolutionFileWithNonExistMsBuildPath()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();
            var msbuildPath = @"not exist path";

            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, "packages.config", "packages.config");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath + $@" -MSBuildPath ""{msbuildPath}"" ");

                // Assert
                Assert.True(_failureCode == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(r.Errors.Contains($"MSBuildPath : {msbuildPath} does not exist."));
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/9303")]
        public void RestoreCommand_FromSolutionFileWithMsbuildPathAndMsbuildVersion()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();


            var msbuildPath = Util.GetMsbuildPathOnWindows();
            if (RuntimeEnvironmentHelper.IsMono && RuntimeEnvironmentHelper.IsMacOSX)
            {
                msbuildPath = @"/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/msbuild/15.0/bin/";
            }

            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, "packages.config", "packages.config");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath + $@" -MSBuildPath ""{msbuildPath}"" -MSBuildVersion 12");

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                Assert.True(r.Output.Contains($"Using Msbuild from '{msbuildPath}'."));
                Assert.True(r.Output.Contains($"MsbuildPath : {msbuildPath} is using, ignore MsBuildVersion: 12."));

                var packageFileA = Path.Combine(workingPath, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(workingPath, @"packages", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        // Tests that if the project file cannot be loaded, i.e. InvalidProjectFileException is thrown,
        // Then packages listed in packages.config file will be restored.
        [Fact]
        public void RestoreCommand_ProjectCannotBeLoaded()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;

                var repositoryPath = pathContext.PackageSource;
                var proj1Directory = Path.Combine(workingPath, "proj1");

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(proj1Directory);

                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);

                Util.CreateFile(workingPath, "a.sln",
                    @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj1"", ""proj1\proj1.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject");

                // The project contains an import statement to import an non-existing file.
                // Thus, this project cannot be loaded successfully.
                Util.CreateFile(proj1Directory, "proj1.csproj",
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <Import Project='..\packages\a.targets' />
</Project>");
                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");
                string[] args = new string[] { "restore", "-Source", repositoryPath };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath);

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                Assert.True(File.Exists(packageFileA));
            }
        }

        // Tests that when -solutionDir is specified, the $(SolutionDir)\.nuget\NuGet.Config file
        // will be used.
        [Fact]
        public void RestoreCommand_FromPackagesConfigFileWithOptionSolutionDir()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = pathContext.PackageSource;

                Directory.CreateDirectory(repositoryPath);

                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", repositoryPath);
                Util.CreateFile(workingPath, "packages.config",
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                string[] args = new string[] { "restore", "-Source", repositoryPath, "-solutionDir", workingPath };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        // Tests that when package restore is enabled and -RequireConsent is specified,
        // the opt out message is displayed.
        [Theory]
        [InlineData("packages.config")]
        [InlineData("packages.proj1.config")]
        public void RestoreCommand_OptOutMessage(string configFileName)
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                Util.CreateFile(workingPath, "my.config",
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageRestore>
    <add key=""enabled"" value=""True"" />
  </packageRestore>
</configuration>");
                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, configFileName, "packages.config");


                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath + " -ConfigFile my.config -RequireConsent");

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                string optOutMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    NuGetResources.RestoreCommandPackageRestoreOptOutMessage,
                    NuGetResources.PackageRestoreConsentCheckBoxText.Replace("&", ""));
                Assert.Contains(optOutMessage.Replace("\r\n", "\n"), r.Output.Replace("\r\n", "\n"));
                var packageFileA = Path.Combine(workingPath, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(workingPath, @"packages", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        // Tests that when package restore is enabled and -RequireConsent is not specified,
        // the opt out message is not displayed.
        [Fact]
        public void RestoreCommand_NoOptOutMessage()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, "packages.config", "packages.config");
                Util.CreateFile(workingPath, "my.config",
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageRestore>
    <add key=""enabled"" value=""True"" />
  </packageRestore>
</configuration>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath + " -ConfigFile my.config");

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                string optOutMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    NuGetResources.RestoreCommandPackageRestoreOptOutMessage,
                    NuGetResources.PackageRestoreConsentCheckBoxText.Replace("&", ""));
                Assert.DoesNotContain(optOutMessage, r.Output);
                var packageFileA = Path.Combine(workingPath, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(workingPath, @"packages", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        // Test that when a directory is passed to nuget.exe restore, and the directory contains
        // just one solution file, restore will work on that solution file.
        [Theory]
        [InlineData("packages.config")]
        [InlineData("packages.proj2.config")]
        public void RestoreCommand_OneSolutionFileInDirectory(string configFileName)
        {
            // Arrang
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var randomTestFolder = Path.Combine(pathContext.WorkingDirectory, "random");
                Directory.CreateDirectory(randomTestFolder);
                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, "packages.config", configFileName);

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomTestFolder,
                    "restore " + workingPath + " -Source " + repositoryPath);

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        // Test that when a directory is passed to nuget.exe restore, and the directory contains
        // multiple solution files, nuget.exe will generate an error.
        [Fact]
        public void RestoreCommand_MultipleSolutionFilesInDirectory()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            using (var randomTestFolder = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var proj1Directory = Path.Combine(workingPath, "proj1");
                var proj2Directory = Path.Combine(workingPath, "proj2");

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(proj1Directory);
                Directory.CreateDirectory(proj2Directory);

                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", repositoryPath);

                Util.CreateFile(workingPath, "a.sln",
                    @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj1"", ""proj1\proj1.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj2"", ""proj2\proj2.csproj"", ""{42641DAE-D6C4-49D4-92EA-749D2573554A}""
EndProject");
                Util.CreateFile(workingPath, "b.sln",
                    @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj1"", ""proj1\proj1.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj2"", ""proj2\proj2.csproj"", ""{42641DAE-D6C4-49D4-92EA-749D2573554A}""
EndProject");

                Util.CreateFile(proj1Directory, "proj1.csproj",
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");
                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");

                Util.CreateFile(proj2Directory, "proj2.csproj",
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");
                Util.CreateFile(proj2Directory, "packages.config",
@"<packages>
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomTestFolder,
                    "restore " + workingPath + " -Source " + repositoryPath);

                // Assert
                Assert.Equal(_failureCode, r.ExitCode);
                Assert.Contains("This folder contains more than one solution file.", r.Errors);
                var packageFileA = Path.Combine(workingPath, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(workingPath, @"packages", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.False(File.Exists(packageFileA));
                Assert.False(File.Exists(packageFileB));
            }
        }

        // Test that when a directory is passed to nuget.exe restore, and the directory contains
        // no solution files, nuget.exe will generate an error.
        [Fact]
        public void RestoreCommand_NoSolutionFilesInDirectory()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            using (var randomTestFolder = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var proj1Directory = Path.Combine(workingPath, "proj1");
                var proj2Directory = Path.Combine(workingPath, "proj2");

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(proj1Directory);
                Directory.CreateDirectory(proj2Directory);

                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", repositoryPath);

                Util.CreateFile(proj1Directory, "proj1.csproj",
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");
                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");

                Util.CreateFile(proj2Directory, "proj2.csproj",
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");
                Util.CreateFile(proj2Directory, "packages.config",
@"<packages>
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomTestFolder,
                    "restore " + workingPath + " -Source " + repositoryPath);

                // Assert
                Assert.Equal(_failureCode, r.ExitCode);
                Assert.Contains("does not contain an msbuild solution", r.Errors);
                var packageFileA = Path.Combine(workingPath, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(workingPath, @"packages", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.False(File.Exists(packageFileA));
                Assert.False(File.Exists(packageFileB));
            }
        }

        // Tests that package restore loads the correct config file when -ConfigFile
        // is specified.
        [Fact]
        public void RestoreCommand_ConfigFile()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var proj1Directory = Path.Combine(workingPath, "proj1");

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(proj1Directory);

                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);

                Util.CreateFile(workingPath, "a.sln",
                    @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""proj1"", ""proj1\proj1.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject");
                Util.CreateFile(workingPath, "my.config",
                    string.Format(CultureInfo.InvariantCulture,
@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
        <add key=""mysouce"" value=""{0}"" />
    </packageSources>
</configuration>", repositoryPath));

                Util.CreateFile(proj1Directory, "proj1.csproj",
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");
                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");

                // Act
                // the package source listed in my.config will be used.
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -ConfigFile my.config");

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var packageFileA = Path.Combine(workingPath, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                Assert.True(File.Exists(packageFileA));
            }
        }

        /// <summary>
        /// Tests two subsequent restores, with different combinations of -PackageSaveMode. This
        /// test should try all possible combinations.
        /// </summary>
        [Theory]
        [InlineData(PackageSaveMode.Defaultv2, PackageSaveMode.Defaultv2)]
        [InlineData(PackageSaveMode.Defaultv2, PackageSaveMode.Nupkg)]
        [InlineData(PackageSaveMode.Defaultv2, PackageSaveMode.Nuspec)]
        [InlineData(PackageSaveMode.Defaultv2, PackageSaveMode.Nupkg | PackageSaveMode.Nuspec)]

        [InlineData(PackageSaveMode.Nupkg, PackageSaveMode.Defaultv2)]
        [InlineData(PackageSaveMode.Nupkg, PackageSaveMode.Nupkg)]
        [InlineData(PackageSaveMode.Nupkg, PackageSaveMode.Nuspec)]
        [InlineData(PackageSaveMode.Nupkg, PackageSaveMode.Nupkg | PackageSaveMode.Nuspec)]

        [InlineData(PackageSaveMode.Nuspec, PackageSaveMode.Defaultv2)]
        [InlineData(PackageSaveMode.Nuspec, PackageSaveMode.Nupkg)]
        [InlineData(PackageSaveMode.Nuspec, PackageSaveMode.Nuspec)]
        [InlineData(PackageSaveMode.Nuspec, PackageSaveMode.Nupkg | PackageSaveMode.Nuspec)]

        [InlineData(PackageSaveMode.Nupkg | PackageSaveMode.Nuspec, PackageSaveMode.Defaultv2)]
        [InlineData(PackageSaveMode.Nupkg | PackageSaveMode.Nuspec, PackageSaveMode.Nupkg)]
        [InlineData(PackageSaveMode.Nupkg | PackageSaveMode.Nuspec, PackageSaveMode.Nuspec)]
        [InlineData(PackageSaveMode.Nupkg | PackageSaveMode.Nuspec, PackageSaveMode.Nupkg | PackageSaveMode.Nuspec)]
        public void RestoreCommand_WithSubsequentRestores_PackageSaveModeIsObserved(PackageSaveMode firstRestore, PackageSaveMode secondRestore)
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;

                var repositoryPath = Util.CreateBasicTwoProjectSolution(workingPath, "packages.config", "packages.config");

                var expectedPackageFileAExists = false;
                var expectedNuspecFileAExists = false;
                var expectedContentAExists = false;
                var expectedPackageFileBExists = false;
                var expectedNuspecFileBExists = false;
                var expectedContentBExists = false;

                var packageFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var nuspecFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "packageA.nuspec");
                var contentFileA = Path.Combine(pathContext.PackagesV2, "packageA.1.1.0", "content", "test1.txt");
                var packageFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.2.2.0.nupkg");
                var nuspecFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "packageB.nuspec");
                var contentFileB = Path.Combine(pathContext.PackagesV2, "packageB.2.2.0", "content", "test1.txt");

                // Act & Assert
                // None of the files should exist before any restore.
                Assert.Equal(expectedPackageFileAExists, File.Exists(packageFileA));
                Assert.Equal(expectedNuspecFileAExists, File.Exists(nuspecFileA));
                Assert.Equal(expectedContentAExists, File.Exists(contentFileA));
                Assert.Equal(expectedPackageFileBExists, File.Exists(packageFileB));
                Assert.Equal(expectedNuspecFileBExists, File.Exists(nuspecFileB));
                Assert.Equal(expectedContentBExists, File.Exists(contentFileB));

                // First restore.
                if (firstRestore.HasFlag(PackageSaveMode.Nupkg))
                {
                    expectedPackageFileAExists = true;
                    expectedPackageFileBExists = true;
                }

                if (firstRestore.HasFlag(PackageSaveMode.Nuspec))
                {
                    expectedNuspecFileAExists = true;
                    expectedNuspecFileBExists = true;
                }

                expectedContentAExists = true;
                expectedContentBExists = true;

                var packageSaveMode1 = firstRestore == PackageSaveMode.Defaultv2 ?
                    string.Empty :
                    " -PackageSaveMode " + firstRestore.ToString().Replace(", ", ";");
                var r1 = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath + packageSaveMode1);

                Assert.Equal(_successCode, r1.ExitCode);
                Assert.Equal(expectedPackageFileAExists, File.Exists(packageFileA));
                Assert.Equal(expectedNuspecFileAExists, File.Exists(nuspecFileA));
                Assert.Equal(expectedContentAExists, File.Exists(contentFileA));
                Assert.Equal(expectedPackageFileBExists, File.Exists(packageFileB));
                Assert.Equal(expectedNuspecFileBExists, File.Exists(nuspecFileB));
                Assert.Equal(expectedContentBExists, File.Exists(contentFileB));

                // Second restore.
                if (secondRestore.HasFlag(PackageSaveMode.Nupkg))
                {
                    expectedPackageFileAExists = true;
                    expectedPackageFileBExists = true;
                }

                if (secondRestore.HasFlag(PackageSaveMode.Nuspec))
                {
                    expectedNuspecFileAExists = true;
                    expectedNuspecFileBExists = true;
                }

                expectedContentAExists = true;
                expectedContentBExists = true;

                // Second restore
                var packageSaveMode2 = secondRestore == PackageSaveMode.Defaultv2 ?
                    string.Empty :
                    " -PackageSaveMode " + secondRestore.ToString().Replace(", ", ";");
                var r2 = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    "restore -Source " + repositoryPath + packageSaveMode2);

                Assert.Equal(_successCode, r2.ExitCode);
                Assert.Equal(expectedPackageFileAExists, File.Exists(packageFileA));
                Assert.Equal(expectedNuspecFileAExists, File.Exists(nuspecFileA));
                Assert.Equal(expectedContentAExists, File.Exists(contentFileA));
                Assert.Equal(expectedPackageFileBExists, File.Exists(packageFileB));
                Assert.Equal(expectedNuspecFileBExists, File.Exists(nuspecFileB));
                Assert.Equal(expectedContentBExists, File.Exists(contentFileB));
            }
        }

        // Tests restore from an http source.
        [Fact]
        public void RestoreCommand_FromHttpSource()
        {
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var packageDirectory = Path.Combine(pathContext.WorkingDirectory, "packageDirectory");
                var workingDirectory = pathContext.WorkingDirectory;
                // Arrange
                var packageFileName = Util.CreateTestPackage("testPackage1", "1.1.0", packageDirectory);
                var package = new FileInfo(packageFileName);

                Util.CreateFile(
                    workingDirectory,
                    "packages.config",
                    @"
<packages>
  <package id=""testPackage1"" version=""1.1.0"" />
</packages>");

                using (var server = new MockServer())
                {
                    bool getPackageByVersionIsCalled = false;
                    bool packageDownloadIsCalled = false;

                    server.Get.Add("/nuget/$metadata", r =>
                       Util.GetMockServerResource());
                    server.Get.Add("/nuget/Packages(Id='testPackage1',Version='1.1.0')", r =>
                        new Action<HttpListenerResponse>(response =>
                        {
                            getPackageByVersionIsCalled = true;
                            response.ContentType = "application/atom+xml;type=entry;charset=utf-8";
                            var odata = server.ToOData(new PackageArchiveReader(package.OpenRead()));
                            MockServer.SetResponseContent(response, odata);
                        }));

                    server.Get.Add("/package/testPackage1", r =>
                        new Action<HttpListenerResponse>(response =>
                        {
                            packageDownloadIsCalled = true;
                            response.ContentType = "application/zip";
                            using (var stream = package.OpenRead())
                            {
                                var content = stream.ReadAllBytes();
                                MockServer.SetResponseContent(response, content);
                            }
                        }));

                    server.Get.Add("/nuget", r => "OK");

                    server.Start();

                    // Act
                    var args = "restore packages.config -PackagesDirectory . -Source " + server.Uri + "nuget";
                    var r1 = CommandRunner.Run(
                        nugetexe,
                        workingDirectory,
                        args);
                    server.Stop();

                    // Assert
                    r1.Success.Should().BeTrue(r1.AllOutput);
                    Assert.True(getPackageByVersionIsCalled, "getPackageByVersionIsCalled");
                    Assert.True(packageDownloadIsCalled, "getPackageByVersionIsCalled");
                }
            }
        }

        [Fact]
        public void RestoreCommand_FromProjectJson_RelativeGlobalPackagesFolder()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = Path.Combine(pathContext.WorkingDirectory, "sub1", "sub2");

                Directory.CreateDirectory(workingPath);

                var repositoryPath = Path.Combine(workingPath, "Repository");

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));

                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", repositoryPath);


                var projectJson = @"{
                    'dependencies': {
                    'packageA': '1.1.0',
                    'packageB': '2.2.0'
                    },
                    'frameworks': {
                                'netcore50': { }
                            }
                }";

                var projectFile = Util.CreateUAPProject(workingPath, projectJson);

                var nugetConfigDir = Path.Combine(workingPath, ".nuget");

                var repoPath = (RuntimeEnvironmentHelper.IsMono && !RuntimeEnvironmentHelper.IsWindows) ?
                   @"../../GlobalPackages2" : @"..\..\GlobalPackages2";
                Util.CreateFile(nugetConfigDir, "nuget.config",
$@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <config>
    <add key=""globalPackagesFolder"" value=""{repoPath}"" />
  </config>
</configuration>");

                string[] args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    projectFile,
                    "-verbosity detailed"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(
                    nugetConfigDir,
                    @"..", "..", "GlobalPackages2", "packageA", "1.1.0", "packageA.1.1.0.nupkg");

                var packageFileB = Path.Combine(
                    nugetConfigDir,
                    @"..", "..", "GlobalPackages2", "packageB", "2.2.0", "packageB.2.2.0.nupkg");

                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public void RestoreCommand_InvalidPackagesConfigFile()
        {
            using (var randomTestFolder = TestDirectory.Create())
            {
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var packagesConfigPath = Path.Combine(randomTestFolder, "packages.config");
                var contents = @"blah <packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>";
                File.WriteAllText(packagesConfigPath, contents);

                var args = new string[]
                {
                    "restore",
                    packagesConfigPath,
                    "-PackagesDirectory",
                    randomTestFolder
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    Directory.GetCurrentDirectory(),
                    string.Join(" ", args));

                // Assert
                Assert.NotEqual(_successCode, r.ExitCode);
                var error = r.Errors;
                Assert.True(error.Contains("Error parsing packages.config file"));
            }
        }

        [Fact]
        public void RestoreCommand_InvalidSolutionFile()
        {
            using (var randomTestFolder = TestDirectory.Create())
            {
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                var solutionFile = Path.Combine(randomTestFolder, "A.sln");
                var contents = @"Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(asdfasdf""{ FAE04EC0 - 301F - 11D3 - BF4B - 00C04F79EFBC}
                "") = ""proj1"", ""proj1\proj1.csproj"", ""{ A04C59CC - 7622 - 4223 - B16B - CDF2ECAD438D}
                ""
EndProjectblah
Project(""{ FAE04EC0 - 301F - 11D3 - BF4B - 00C04F79EFBC}
                "") = ""proj2"", ""proj2\proj2.csproj"", ""{ 42641DAE - D6C4 - 49D4 - 92EA - 749D2573554A}
                ""
EndProject";
                File.WriteAllText(solutionFile, contents);

                var args = new string[]
                {
                    "restore",
                    solutionFile,
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomTestFolder,
                    string.Join(" ", args));

                // Assert
                Assert.NotEqual(_successCode, r.ExitCode);
                var error = r.Errors;
                Assert.True(error.Contains("Error parsing solution file"));
                Assert.True(error.Contains("Error parsing a project section"));
            }
        }

        // return code should be 1 when restore failed
        [Fact]
        public void RestoreCommand_FromPackagesConfigFileFailed()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                Directory.CreateDirectory(repositoryPath);

                Util.CreateFile(workingPath, "packages.config",
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                var args = new string[] { "restore", "-PackagesDirectory", "outputDir", "-Source", repositoryPath, "-nocache" };

                // Act
                var envVars = new Dictionary<string, string>()
                {
                    { "PATH", null }
                };

                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args),
                    environmentVariables: envVars);

                var output = r.Output + " " + r.Errors;

                // Assert
                Assert.Equal(_failureCode, r.ExitCode);
                Assert.False(output.IndexOf("exception", StringComparison.OrdinalIgnoreCase) > -1);
                Assert.False(output.IndexOf("exception", StringComparison.OrdinalIgnoreCase) > -1);

                var firstIndex = output.IndexOf(
                    "Unable to find version '1.1.0' of package 'packageA'.",
                    StringComparison.OrdinalIgnoreCase);
                Assert.True(firstIndex > -1);
                var secondIndex = output.IndexOf(
                    "Unable to find version '1.1.0' of package 'packageA'.",
                    StringComparison.OrdinalIgnoreCase);
                Assert.True(secondIndex > -1);
            }
        }

        [Fact]
        public void RestoreCommand_NoFeedAvailable()
        {
            var nugetexe = Util.GetNuGetExePath();
            using (var randomTestFolder = TestDirectory.Create())
            {
                // Create an empty config file and pass it as -ConfigFile switch.
                // This imitates the scenario where there is a machine without a default nuget.config under %APPDATA%
                // In this case, nuget will not create default nuget.config for user.
                var config = string.Format(
    @"<?xml version='1.0' encoding='utf - 8'?>
<configuration/>
");
                var configFileName = Path.Combine(randomTestFolder, "nuget.config");
                File.WriteAllText(configFileName, config);

                var packagesConfigFileName = Path.Combine(randomTestFolder, "packages.config");
                File.WriteAllText(
                    packagesConfigFileName,
@"<packages>
  <package id=""Newtonsoft.Json"" version=""7.0.1"" targetFramework=""net45"" />
</packages>");

                string[] args
                    = new string[]
                    {
                        "restore",
                        "-PackagesDirectory",
                        ".",
                        "-ConfigFile",
                        configFileName
                    };

                // Act
                var envVars = new Dictionary<string, string>()
                {
                    { "PATH", null }
                };

                var r = CommandRunner.Run(
                    nugetexe,
                    randomTestFolder,
                    string.Join(" ", args),
                    environmentVariables: envVars);

                // Assert
                var expectedPath = Path.Combine(
                    randomTestFolder,
                    "Newtonsoft.Json.7.0.1",
                    "Newtonsoft.Json.7.0.1.nupkg");

                Assert.False(File.Exists(expectedPath));
            }
        }

        [Fact]
        public void RestoreCommand_LegacySolutionLevelPackages_SolutionDirectory()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var randomRepositoryPath = pathContext.PackageSource;
                var randomSolutionFolder = pathContext.SolutionRoot;
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                Util.CreateTestPackage("packageA", "1.1.0", randomRepositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", randomRepositoryPath);

                var solutionFile = Path.Combine(randomSolutionFolder, "A.sln");

                var solutionFileContents
                    = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") " +
@"= ""proj"", ""proj\proj.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject";

                File.WriteAllText(solutionFile, solutionFileContents);

                var nugetFolderAtSolutionDirectory
                    = Path.Combine(randomSolutionFolder, NuGetConstants.NuGetSolutionSettingsFolder);
                Directory.CreateDirectory(nugetFolderAtSolutionDirectory);

                File.WriteAllText(
                    Path.Combine(nugetFolderAtSolutionDirectory, ProjectManagement.Constants.PackageReferenceFile),
@"<packages>
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                var projectDirectory = Path.Combine(randomSolutionFolder, "proj");
                Directory.CreateDirectory(projectDirectory);

                File.WriteAllText(
                    Path.Combine(projectDirectory, "proj.csproj"),
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                File.WriteAllText(
                    Path.Combine(projectDirectory, "packages.config"),
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomSolutionFolder,
                    "restore " + randomSolutionFolder + " -Source " + randomRepositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(randomSolutionFolder, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(randomSolutionFolder, @"packages", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public void RestoreCommand_LegacySolutionLevelPackages_SolutionFile()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var randomRepositoryPath = pathContext.PackageSource;
                var randomSolutionFolder = pathContext.SolutionRoot;
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                Util.CreateTestPackage("packageA", "1.1.0", randomRepositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", randomRepositoryPath);

                var solutionFile = Path.Combine(randomSolutionFolder, "A.sln");

                var solutionFileContents
                    = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") " +
@"= ""proj"", ""proj\proj.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject";

                File.WriteAllText(solutionFile, solutionFileContents);

                var nugetFolderAtSolutionDirectory
                    = Path.Combine(randomSolutionFolder, NuGetConstants.NuGetSolutionSettingsFolder);
                Directory.CreateDirectory(nugetFolderAtSolutionDirectory);

                File.WriteAllText(
                    Path.Combine(nugetFolderAtSolutionDirectory, ProjectManagement.Constants.PackageReferenceFile),
@"<packages>
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                var projectDirectory = Path.Combine(randomSolutionFolder, "proj");
                Directory.CreateDirectory(projectDirectory);

                File.WriteAllText(
                    Path.Combine(projectDirectory, "proj.csproj"),
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                File.WriteAllText(
                    Path.Combine(projectDirectory, "packages.config"),
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomSolutionFolder,
                    "restore " + solutionFile + " -Source " + randomRepositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(randomSolutionFolder, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(randomSolutionFolder, @"packages", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public void RestoreCommand_LegacySolutionLevelPackages_NoArgument()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var randomRepositoryPath = pathContext.PackageSource;
                var randomSolutionFolder = pathContext.SolutionRoot;
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                Util.CreateTestPackage("packageA", "1.1.0", randomRepositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", randomRepositoryPath);

                var solutionFile = Path.Combine(randomSolutionFolder, "A.sln");

                var solutionFileContents
                    = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") " +
@"= ""proj"", ""proj\proj.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject";

                File.WriteAllText(solutionFile, solutionFileContents);

                var nugetFolderAtSolutionDirectory
                    = Path.Combine(randomSolutionFolder, NuGetConstants.NuGetSolutionSettingsFolder);
                Directory.CreateDirectory(nugetFolderAtSolutionDirectory);

                File.WriteAllText(
                    Path.Combine(nugetFolderAtSolutionDirectory, ProjectManagement.Constants.PackageReferenceFile),
@"<packages>
  <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                var projectDirectory = Path.Combine(randomSolutionFolder, "proj");
                Directory.CreateDirectory(projectDirectory);

                File.WriteAllText(
                    Path.Combine(projectDirectory, "proj.csproj"),
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                File.WriteAllText(
                    Path.Combine(projectDirectory, "packages.config"),
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomSolutionFolder,
                    "restore  -Source " + randomRepositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(randomSolutionFolder, @"packages", "packageA.1.1.0", "packageA.1.1.0.nupkg");
                var packageFileB = Path.Combine(randomSolutionFolder, @"packages", "packageB.2.2.0", "packageB.2.2.0.nupkg");
                Assert.True(File.Exists(packageFileA));
                Assert.True(File.Exists(packageFileB));
            }
        }

        [Fact]
        public void RestoreCommand_LegacySolutionLevelPackages_DuplicatePackageIds()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var randomRepositoryPath = pathContext.PackageSource;
                var randomSolutionFolder = pathContext.SolutionRoot;
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                Util.CreateTestPackage("packageA", "1.0.0", randomRepositoryPath);
                Util.CreateTestPackage("packageA", "2.0.0", randomRepositoryPath);
                Util.CreateTestPackage("packageA", "3.0.0", randomRepositoryPath);
                Util.CreateTestPackage("packageB", "1.0.0", randomRepositoryPath);
                Util.CreateTestPackage("packageB", "2.0.0", randomRepositoryPath);
                Util.CreateTestPackage("packageB", "3.0.0", randomRepositoryPath);
                Util.CreateTestPackage("packageC", "1.0.0", randomRepositoryPath);

                var solutionFile = Path.Combine(randomSolutionFolder, "A.sln");

                var solutionFileContents
                    = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") " +
@"= ""proj"", ""proj\proj.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject";

                File.WriteAllText(solutionFile, solutionFileContents);

                var nugetFolderAtSolutionDirectory
                    = Path.Combine(randomSolutionFolder, NuGetConstants.NuGetSolutionSettingsFolder);
                Directory.CreateDirectory(nugetFolderAtSolutionDirectory);

                File.WriteAllText(
                    Path.Combine(nugetFolderAtSolutionDirectory, ProjectManagement.Constants.PackageReferenceFile),
@"<packages>
  <package id=""packageB"" version=""1.0.0"" targetFramework=""net45"" />
  <package id=""packageB"" version=""2.0.0"" targetFramework=""net45"" />
  <package id=""packageB"" version=""3.0.0"" targetFramework=""net45"" />
  <package id=""packageA"" version=""1.0.0"" targetFramework=""net45"" />
  <package id=""packageA"" version=""2.0.0"" targetFramework=""net45"" />
  <package id=""packageA"" version=""3.0.0"" targetFramework=""net45"" />
</packages>");

                var projectDirectory = Path.Combine(randomSolutionFolder, "proj");
                Directory.CreateDirectory(projectDirectory);

                File.WriteAllText(
                    Path.Combine(projectDirectory, "proj.csproj"),
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                File.WriteAllText(
                    Path.Combine(projectDirectory, "packages.config"),
@"<packages>
  <package id=""packageC"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomSolutionFolder,
                    "restore  -Source " + randomRepositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);

                Assert.True(File.Exists(Path.Combine(randomSolutionFolder,
                    @"packages", "packageA.1.0.0", "packageA.1.0.0.nupkg")));

                Assert.True(File.Exists(Path.Combine(randomSolutionFolder,
                    @"packages", "packageA.2.0.0", "packageA.2.0.0.nupkg")));

                Assert.True(File.Exists(Path.Combine(randomSolutionFolder,
                    @"packages", "packageA.3.0.0", "packageA.3.0.0.nupkg")));

                Assert.True(File.Exists(Path.Combine(randomSolutionFolder,
                    @"packages", "packageB.1.0.0", "packageB.1.0.0.nupkg")));

                Assert.True(File.Exists(Path.Combine(randomSolutionFolder,
                    @"packages", "packageB.2.0.0", "packageB.2.0.0.nupkg")));

                Assert.True(File.Exists(Path.Combine(randomSolutionFolder,
                    @"packages", "packageB.3.0.0", "packageB.3.0.0.nupkg")));

                Assert.True(File.Exists(Path.Combine(randomSolutionFolder,
                    @"packages", "packageC.1.0.0", "packageC.1.0.0.nupkg")));
            }
        }

        [Fact]
        public void RestoreCommand_LegacySolutionLevelPackages_DuplicatePackageIdentities()
        {
            using (var randomRepositoryPath = TestDirectory.Create())
            using (var randomSolutionFolder = TestDirectory.Create())
            {
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                Util.CreateTestPackage("packageA", "1.0.0", randomRepositoryPath);
                Util.CreateTestPackage("packageA", "3.0.0", randomRepositoryPath);
                Util.CreateTestPackage("packageC", "1.0.0", randomRepositoryPath);

                var solutionFile = Path.Combine(randomSolutionFolder, "A.sln");

                var solutionFileContents
                    = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") " +
@"= ""proj"", ""proj\proj.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject";

                File.WriteAllText(solutionFile, solutionFileContents);

                var nugetFolderAtSolutionDirectory
                    = Path.Combine(randomSolutionFolder, NuGetConstants.NuGetSolutionSettingsFolder);
                Directory.CreateDirectory(nugetFolderAtSolutionDirectory);

                File.WriteAllText(
                    Path.Combine(nugetFolderAtSolutionDirectory, ProjectManagement.Constants.PackageReferenceFile),
@"<packages>
  <package id=""packageA"" version=""1.0.0"" targetFramework=""net45"" />
  <package id=""packageA"" version=""1.0.0"" targetFramework=""net45"" />
  <package id=""packageA"" version=""1.0.0"" targetFramework=""net45"" />
  <package id=""packageA"" version=""3.0.0"" targetFramework=""net45"" />
  <package id=""packageA"" version=""3.0.0"" targetFramework=""net45"" />
</packages>");

                var projectDirectory = Path.Combine(randomSolutionFolder, "proj");
                Directory.CreateDirectory(projectDirectory);

                File.WriteAllText(
                    Path.Combine(projectDirectory, "proj.csproj"),
@"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                File.WriteAllText(
                    Path.Combine(projectDirectory, "packages.config"),
@"<packages>
  <package id=""packageC"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomSolutionFolder,
                    "restore  -Source " + randomRepositoryPath);

                // Assert
                Assert.False(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Contains("There are duplicate packages: packageA.1.0.0, packageA.3.0.0", r.Errors);
            }
        }

        [Fact]
        public async Task RestoreCommand_PreservesPackageFilesTimestamps()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = pathContext.PackageSource;

                var entryModifiedTime = new DateTimeOffset(1985, 11, 20, 12, 0, 0, TimeSpan.FromHours(-7.0)).DateTime;

                var packageFileFullPath = Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                using (var zip = new ZipArchive(File.Open(packageFileFullPath, FileMode.Open, FileAccess.ReadWrite), ZipArchiveMode.Update))
                {
                    var entry = await zip.AddEntryAsync("lib/net45/A.dll", string.Empty);
                    entry.LastWriteTime = entryModifiedTime;
                }

                Util.CreateFile(workingPath, "packages.config",
@"<packages>
  <package id=""packageA"" version=""1.1.0"" targetFramework=""net45"" />
</packages>");

                string[] args = new string[] { "restore", "-PackagesDirectory", "outputDir", "-Source", repositoryPath };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var dllPath = Path.Combine(workingPath, "outputDir", "packageA.1.1.0", "lib", "net45", "A.dll");
                var dllFileInfo = new FileInfo(dllPath);
                Assert.True(File.Exists(dllFileInfo.FullName));
                Assert.Equal(entryModifiedTime, dllFileInfo.LastWriteTime);
            }
        }

        /// <summary>
        /// Test proper handling of project in parent directories. The solution A\A.sln contains A\A.Util\A.Util.csproj
        /// and B\B.csproj. B.csproj depends on ..\A\A.Util\A.Util.csproj.
        /// </summary>
        [Fact]
        public void RestoreCommand_FromSolutionFile_ProjectsInParentDir()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var basePath = pathContext.WorkingDirectory;
                Directory.CreateDirectory(Path.Combine(basePath, "A"));
                Directory.CreateDirectory(Path.Combine(basePath, "A", "A.Util"));
                Directory.CreateDirectory(Path.Combine(basePath, "B"));

                var repositoryPath = Path.Combine(basePath, "Repository");

                Directory.CreateDirectory(repositoryPath);

                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", repositoryPath);

                Util.CreateFile(Path.Combine(basePath, "A", "A.Util"), "A.Util.csproj",
@"<Project ToolsVersion='14.0' DefaultTargets='Build' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='project.json' />
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>");

                Util.CreateFile(Path.Combine(basePath, "A", "A.Util"), "project.json",
@"{
  'dependencies': {
    'packageA': '1.1.0',
    'packageB': '2.2.0'
  },
  'frameworks': {
                'netcore50': { }
            }
}");
                Util.CreateFile(Path.Combine(basePath, "B"), "B.csproj",
@"<Project ToolsVersion='14.0' DefaultTargets='Build' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include=""..\A\A.Util\A.Util.csproj"">
      <Project>{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</Project>
      <Name>A.Util</Name>
    </ProjectReference>
  </ItemGroup>
  <ItemGroup>
    <None Include='project.json' />
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>");

                Util.CreateFile(Path.Combine(basePath, "B"), "project.json",
@"{
  'dependencies': {
  },
  'frameworks': {
                'netcore50': { }
            }
}");

                Util.CreateFile(Path.Combine(basePath, "A"), "A.sln",
                    @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""A.Util"", ""A.Util\A.Util.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""B"", ""..\B\B.csproj"", ""{42641DAE-D6C4-49D4-92EA-749D2573554A}""
EndProject");

                var args = new[] {
                    "restore",
                    Path.Combine(basePath, "A", "A.sln"),
                    "-verbosity detailed",
                    "-Source", repositoryPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    basePath,
                    string.Join(" ", args));

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var bProjectLockJsonFile = Path.Combine(basePath, "B", "project.lock.json");
                Assert.True(File.Exists(bProjectLockJsonFile));
                var bProjectLockJson = new LockFileFormat().Read(bProjectLockJsonFile);
                var bLibraries = bProjectLockJson.Libraries;
                var bLibraryNames = bLibraries.Select(lib => lib.Name).ToList();
                Assert.Contains("packageA", bLibraryNames);
                Assert.Contains("packageB", bLibraryNames);
            }
        }

        [Fact]
        public void RestoreCommand_SourceLoggingFileSource()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();
            var identity = new PackageIdentity("packageA", new NuGetVersion("1.1.0"));

            using (var pathContext = new SimpleTestPathContext())
            {
                var repositoryPath = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;
                Directory.CreateDirectory(repositoryPath);
                Util.CreateTestPackage(identity.Id, identity.Version.ToNormalizedString(), repositoryPath);
                Util.CreateFile(workingPath, "packages.config",
@"<packages>
  <package id=""" + identity.Id + @""" version=""" + identity.Version.ToNormalizedString() + @""" targetFramework=""net45"" />
</packages>");

                string[] args = new string[] { "restore", "-PackagesDirectory", "outputDir", "-Source", repositoryPath, "-Verbosity detailed" };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var target = new PackagePathResolver(Path.Combine(workingPath, @"outputDir"), true);
                var packageFilePath = target.GetInstalledPackageFilePath(identity);

                Assert.True(File.Exists(packageFilePath));
                Assert.Contains(" from source ", r.Output); // source logging present in verbose log
            }
        }

        [Fact]
        public void RestoreCommand_SourceLoggingFromV3FeedNoGlobalPackageCache()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();
            var identity = new PackageIdentity("Newtonsoft.Json", new Versioning.NuGetVersion("7.0.1"));
            var source = @"https://api.nuget.org/v3/index.json";
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                Util.CreateFile(workingPath, "packages.config",
@"<packages>
  <package id=""" + identity.Id + @""" version=""" + identity.Version.ToNormalizedString() + @""" targetFramework=""net45"" />
</packages>");

                string[] args = new string[] { "restore", "-PackagesDirectory", "outputDir", "-Source ", source, "-Verbosity detailed" };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var target = new PackagePathResolver(Path.Combine(workingPath, @"outputDir"), true);
                var packageFilePath = target.GetInstalledPackageFilePath(identity);
                Assert.True(File.Exists(packageFilePath));

                Assert.Contains(" from source ", r.Output); // source logging present in verbose log

                // verify sorce logging reported the correct source
                var match = Regex.Match(r.Output, @" from source '(.*)'");
                Assert.True(match.Success);
                Assert.Contains(source, match.Groups[1].Value);
            }
        }

        [Fact]
        public void RestoreCommand_SourceLoggingFromGlobalPackageCache()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();
            var identity = new PackageIdentity("Newtonsoft.Json", new NuGetVersion("7.0.1"));

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                Util.CreateFile(workingPath, "packages.config",
@"<packages>
  <package id=""" + identity.Id + @""" version=""" + identity.Version.ToNormalizedString() + @""" targetFramework=""net45"" />
</packages>");
                var globalPackagesFolder = Path.Combine(workingPath, @"globalPackages");
                // Prime Cache
                string[] args1 = new string[] { "restore", "-PackagesDirectory", "primeOutputDir", "-Source https://api.nuget.org/v3/index.json", "-Verbosity detailed" };
                CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args1));
                //Verify primed
                Assert.True(File.Exists(Path.Combine(globalPackagesFolder, @"newtonsoft.json", "7.0.1", "newtonsoft.json.7.0.1.nupkg")));

                // Act
                string[] args2 = new string[] { "restore", "-PackagesDirectory", "outputDir", "-Source https://api.nuget.org/v3/index.json", "-Verbosity detailed" };
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args2));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var target = new PackagePathResolver(Path.Combine(workingPath, @"outputDir"), true);
                var packageFilePath = target.GetInstalledPackageFilePath(identity);
                Assert.True(File.Exists(packageFilePath));
                Assert.Contains(" from source ", r.Output); // source logging present in verbose log

                //verify source logging reported the globalPackages folder
                var match = Regex.Match(r.Output, @" from source '(.*)'");
                Assert.True(match.Success);
                Assert.Contains(globalPackagesFolder, match.Groups[1].Value);
            }
        }

        [Fact]
        public void RestoreCommand_ProjectContainsSolutionDirs()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var randomRepositoryPath = pathContext.PackageSource;
                var randomSolutionFolder = pathContext.SolutionRoot;
                // Arrange
                var nugetexe = Util.GetNuGetExePath();
                Util.CreateTestPackage("packageA", "1.1.0", randomRepositoryPath);

                var solutionFile = Path.Combine(randomSolutionFolder, "A.sln");
                var targetFile = Path.Combine(randomSolutionFolder, "MSBuild.Community.Tasks.Targets");
                var solutionFileContents
                    = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2012
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") " +
@"= ""proj"", ""proj\proj.csproj"", ""{A04C59CC-7622-4223-B16B-CDF2ECAD438D}""
EndProject";
                var targetFileContents
                    = @"
<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
<Target Name=""test"">
    <Message Text = ""test"" />
</Target>
</Project>";

                File.WriteAllText(solutionFile, solutionFileContents);
                File.WriteAllText(targetFile, targetFileContents);

                var projectDirectory = Path.Combine(randomSolutionFolder, "proj");
                Directory.CreateDirectory(projectDirectory);

                File.WriteAllText(
                    Path.Combine(projectDirectory, "proj.csproj"),
@"<Project ToolsVersion='15.0' xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
<Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props""
 Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <ProjectGuid>{AA9CA553-8E25-477C-824F-0E5DFE3703DC}</ProjectGuid>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
  </PropertyGroup>
   <ItemGroup>
    <PackageReference Include=""packageA"">
      <Version>1.1.0</Version>
    </PackageReference>
  </ItemGroup>
  <Import Project=""$(SolutionDir)\MSBuild.Community.Tasks.Targets"" />
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    randomSolutionFolder,
                    "restore  -Source " + randomRepositoryPath);

                // Assert
                Assert.True(_successCode == r.ExitCode, r.Output + " " + r.Errors);
                var packageFileA = Path.Combine(pathContext.UserPackagesFolder, "packagea", "1.1.0", "packageA.1.1.0.nupkg");
                Assert.True(File.Exists(packageFileA));
            }
        }

        [Fact]
        public void RestoreCommand_WithAuthorSignedPackage_Succeeds()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var packageDestinationFolder = Path.Combine(pathContext.WorkingDirectory, "outDir");
                var packageFile = new FileInfo(Path.Combine(pathContext.PackageSource, "TestPackage.AuthorSigned.1.0.0.nupkg"));
                var package = GetResource(packageFile.Name);

                File.WriteAllBytes(packageFile.FullName, package);

                var projectFile = new FileInfo(Path.Combine(pathContext.SolutionRoot, "ClassLibrary1.csproj"));
                File.WriteAllText(
                    projectFile.FullName,
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{8586D895-886A-41C9-AAE0-B5510BFA50FC}</ProjectGuid>
    <OutputType>Library</OutputType>
    <RootNamespace>ClassLibrary1</RootNamespace>
    <AssemblyName>ClassLibrary1</AssemblyName>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <OutputPath>bin\Debug\</OutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""System"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""Microsoft.CSharp"" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include=""TestPackage.AuthorSigned"">
      <Version>1.0.0</Version>
    </PackageReference>
  </ItemGroup>
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>
                    ");

                var expectedFilePath = Path.Combine(packageDestinationFolder, "testpackage.authorsigned", "1.0.0", packageFile.Name);
                var nugetExe = Util.GetNuGetExePath();

                var args = new string[]
                    {
                        "restore",
                        projectFile.Name,
                        "-Source",
                        pathContext.PackageSource,
                        "-PackagesDirectory",
                        packageDestinationFolder
                    };

                Assert.False(File.Exists(expectedFilePath));

                var result = CommandRunner.Run(
                    nugetExe,
                    pathContext.SolutionRoot,
                    string.Join(" ", args));

                Assert.True(_successCode == result.ExitCode, result.AllOutput);
                Assert.True(result.Success);
                Assert.True(File.Exists(expectedFilePath));
            }
        }

        [Fact]
        public void RestoreCommand_LongPathPackage()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var repositoryPath = pathContext.PackageSource;
                Directory.CreateDirectory(repositoryPath);
                var aPackage = Util.CreateTestPackage(
                    "packageA",
                    "1.0.0",
                    repositoryPath,
                    new List<NuGetFramework> { NuGetFramework.Parse("net45") },
                    @"2.5.6/core/store/x64/netcoreapp2.0/microsoft.extensions.configuration.environmentvariables/2.0.0/lib/netstandard2.0/Microsoft.Extensions.Configuration.EnvironmentVariables.dll"
                    );
                Util.CreateFile(pathContext.WorkingDirectory, "packages.config",
@"<packages>
  <package id=""packageA"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");

                var args = new string[] { "restore", "-PackagesDirectory", "outputDir", "-Source", repositoryPath };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    pathContext.WorkingDirectory,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var packageFileA = Path.Combine(pathContext.WorkingDirectory, @"outputDir", "packageA.1.0.0", "packageA.1.0.0.nupkg");
                Assert.True(File.Exists(packageFileA));
            }
        }

        [Fact]
        public void RestoreCommand_FromInvalidPackagesConfigFile_ThrowsException()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var outputDir = "outputDir";
                var outputPath = Path.Combine(workingPath, outputDir);
                Directory.CreateDirectory(repositoryPath);
                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", repositoryPath);
                Util.CreateFile(workingPath, "packages.config",
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<!DOCTYPE package [
   <!ENTITY greeting ""Hello"">
   <!ENTITY name ""NuGet Client "">
   <!ENTITY sayhello ""&greeting; &name;"">
]>
<packages>
    <package id=""&sayhello;"" version=""1.1.0"" targetFramework=""net45"" />
    <package id=""packageB"" version=""2.2.0"" targetFramework=""net45"" />
</packages>");

                string[] args = new string[] { "restore", "-PackagesDirectory", outputDir, "-Source", repositoryPath };

                // Act
                CommandRunnerResult result = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.False(result.Success);
                Assert.Contains("Error parsing packages.config file", result.AllOutput);
                Assert.False(Directory.Exists(outputPath));
            }
        }

        [Theory]
        [InlineData("restore a b -PackagesDirectory x")]
        [InlineData("restore a b")]
        public void RestoreCommand_Failure_InvalidArguments(string cmd)
        {
            Util.TestCommandInvalidArguments(cmd);
        }

        [Fact]
        public void RestoreCommand_PackageSourceMappingFilter_Succeed()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingPath, "proj1");
                Directory.CreateDirectory(proj1Directory);

                var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
                File.WriteAllText(
                    proj1File,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                var publicRepositoryPath = Path.Combine(workingPath, "PublicRepository");
                Directory.CreateDirectory(publicRepositoryPath);
                Util.CreateTestPackage("测试更新包", "1.0.0", publicRepositoryPath);
                Util.CreateTestPackage("Microsoft.AspNet.Mvc", "5.2.7", publicRepositoryPath);
                Util.CreateTestPackage("Microsoft.AspNet.WebApi.Cors", "5.2.7", publicRepositoryPath);
                Util.CreateTestPackage("Microsoft.Extensions.Configuration.Abstractions", "5.0.0", publicRepositoryPath);
                Util.CreateTestPackage("Moq", "4.16.1", publicRepositoryPath);
                Util.CreateTestPackage("Moq.AutoMock", "2.3.0", publicRepositoryPath);
                Util.CreateTestPackage("NerdBank.Algorithms", "1.0.0", publicRepositoryPath);
                Util.CreateTestPackage("Nerdbank.GitVersioning", "1.1.64", publicRepositoryPath);
                Util.CreateTestPackage("System.Buffers", "4.5.1", publicRepositoryPath);
                Util.CreateTestPackage("System.Memory", "4.5.4", publicRepositoryPath);
                Util.CreateTestPackage("System.Numerics.Vectors", "4.5.0", publicRepositoryPath);
                Util.CreateTestPackage("System.Runtime.CompilerServices.Unsafe", "6.0.0-preview.1.21102.12", publicRepositoryPath);
                Util.CreateTestPackage("System.Runtime.InteropServices.RuntimeInformation", "4.0.0", publicRepositoryPath);
                Util.CreateTestPackage("System.Threading.Tasks.Extensions", "4.5.4", publicRepositoryPath);
                Util.CreateTestPackage("xunit", "2.0.0", publicRepositoryPath);
                Util.CreateTestPackage("xunit.abstractions", "2.0.0", publicRepositoryPath);
                Util.CreateTestPackage("xunit.assert", "2.0.0", publicRepositoryPath);
                Util.CreateTestPackage("xunit.core", "2.0.0", publicRepositoryPath);

                var sharedRepository = Path.Combine(workingPath, "SharedRepository");
                Directory.CreateDirectory(sharedRepository);
                Util.CreateTestPackage("Castle.Core", "4.4.0", sharedRepository);
                Util.CreateTestPackage("Microsoft.Extensions.Configuration", "5.0.0", sharedRepository);
                Util.CreateTestPackage("Microsoft.Extensions.DependencyInjection.Abstractions", "5.0.0", sharedRepository);
                Util.CreateTestPackage("Microsoft.Extensions.Logging", "5.0.0", sharedRepository);
                Util.CreateTestPackage("Microsoft.Extensions.Primitives", "5.0.0", sharedRepository);
                Util.CreateTestPackage("xunit.extensibility.core", "2.0.0", sharedRepository);
                // Name squatting package on shared repo.
                Util.CreateTestPackage("TestPackage.AuthorSigned", "1.0.0", publicRepositoryPath);

                var signedRepository = Path.Combine(workingPath, "Signed");
                Directory.CreateDirectory(signedRepository);
                var authoredSignedPackage = new FileInfo(Path.Combine(signedRepository, "TestPackage.AuthorSigned.1.0.0.nupkg"));
                File.WriteAllBytes(authoredSignedPackage.FullName, GetResource(authoredSignedPackage.Name));
                var expectedSignedPackageHash = GetHash(authoredSignedPackage.FullName);

                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""测试更新包"" version=""1.0.0"" targetFramework=""net48"" />
  <package id=""Castle.Core"" version=""4.4.0"" targetFramework=""net48"" />
  <package id=""Microsoft.Extensions.Primitives"" version=""5.0.0"" targetFramework=""net48"" />
  <package id=""Microsoft.Extensions.DependencyInjection.Abstractions"" version=""5.0.0"" targetFramework=""net48"" />
  <package id=""Microsoft.Extensions.Configuration"" version=""5.0.0"" targetFramework=""net48"" />
  <package id=""Microsoft.Extensions.Configuration.Abstractions"" version=""5.0.0"" targetFramework=""net48"" />
  <package id=""Microsoft.Extensions.Logging"" version=""5.0.0"" targetFramework=""net48"" />
  <package id=""Moq"" version=""4.16.1"" targetFramework=""net48"" />
  <package id=""Moq.AutoMock"" version=""2.3.0"" targetFramework=""net47"" />
  <package id=""Nerdbank.GitVersioning"" version=""1.1.64"" targetFramework=""net48"" developmentDependency=""true"" />
  <package id=""NerdBank.Algorithms"" version=""1.0.0"" targetFramework=""net48"" developmentDependency=""true"" />
  <package id=""TestPackage.AuthorSigned"" version=""1.0.0"" targetFramework=""net48"" />
  <package id=""System.Buffers"" version=""4.5.1"" targetFramework=""net48"" />
  <package id=""System.Memory"" version=""4.5.4"" targetFramework=""net48"" />
  <package id=""System.Numerics.Vectors"" version=""4.5.0"" targetFramework=""net48"" />
  <package id=""System.Runtime.CompilerServices.Unsafe"" version=""6.0.0-preview.1.21102.12"" targetFramework=""net48"" />
  <package id=""System.Runtime.InteropServices.RuntimeInformation"" version=""4.0.0"" targetFramework=""net48"" />
  <package id=""System.Threading.Tasks.Extensions"" version=""4.5.4"" targetFramework=""net48"" />
  <package id=""xunit"" version=""2.0.0"" targetFramework=""net472"" />
  <package id=""xunit.abstractions"" version=""2.0.0"" targetFramework=""net472"" />
  <package id=""xunit.assert"" version=""2.0.0"" targetFramework=""net472"" />
  <package id=""xunit.core"" version=""2.0.0"" targetFramework=""net472"" />
  <package id=""xunit.extensibility.core"" version=""2.0.0"" targetFramework=""net472"" />
</packages>");

                var configPath = Path.Combine(workingPath, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{publicRepositoryPath}"" />
    <add key=""SharedRepository"" value=""{sharedRepository}"" />
    <add key=""signed"" value=""{signedRepository}"" />
    </packageSources>
    <packageSourceMapping>
        <clear />
        <packageSource key=""PublicRepository"">
            <package pattern=""Moq*"" />
            <package pattern=""Nerdbank.*"" />
            <package pattern=""Microsoft.Asp.*"" />
            <package pattern=""Microsoft.AspNet.*"" />
            <package pattern=""Microsoft.Extensions.Configuration.*"" />
            <package pattern=""System.*"" />
            <package pattern=""xunit*"" />
            <package pattern=""测试更新包"" />
        </packageSource>
        <packageSource key=""SharedRepository"">
            <package pattern=""Castle.Cor*"" />
            <package pattern=""Moq*"" />
            <package pattern=""Microsoft.Extensions.*"" />
            <package pattern=""Microsoft.Extensions.Logging"" />
            <package pattern=""Nerd*"" />
            <package pattern=""Test*"" />
            <package pattern=""xunit.extensibility.core"" />
        </packageSource>
        <packageSource key=""signed"">
            <package pattern=""TestPackage.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

                var packagePath = Path.Combine(workingPath, "packages");

                string[] args = new string[]
                    {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath
                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                r.Success.Should().BeTrue(r.AllOutput);
                Assert.Equal(_successCode, r.ExitCode);
                var packageNerdBankAlgorithms = Path.Combine(packagePath, "NerdBank.Algorithms.1.0.0", "NerdBank.Algorithms.1.0.0.nupkg");
                Assert.True(File.Exists(packageNerdBankAlgorithms));
                var packageCastleCore = Path.Combine(packagePath, "Castle.Core.4.4.0", "Castle.Core.4.4.0.nupkg");
                Assert.True(File.Exists(packageCastleCore));
                // Assert correct signed package is restored from correct repository
                var restoredSignedPackagePath = Path.Combine(packagePath, "TestPackage.AuthorSigned.1.0.0", "TestPackage.AuthorSigned.1.0.0.nupkg");
                using (var nupkgReader = new PackageArchiveReader(restoredSignedPackagePath))
                {
                    var allFiles = nupkgReader.GetFiles().ToList();
                    Assert.Contains(".signature.p7s", allFiles);
                }

                Assert.Equal(expectedSignedPackageHash, GetHash(restoredSignedPackagePath));
            }
        }

        [Fact]
        public void RestoreCommand_PackageSourceMappingFilter_Fails()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingPath, "proj1");
                Directory.CreateDirectory(proj1Directory);

                var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
                File.WriteAllText(
                    proj1File,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                var publicRepositoryPath = Path.Combine(workingPath, "PublicRepository");
                Directory.CreateDirectory(publicRepositoryPath);
                Util.CreateTestPackage("Great.Calc", "1.0.0", publicRepositoryPath);
                Util.CreateTestPackage("Another.Calc", "1.0.0", publicRepositoryPath);

                var sharedRepository = Path.Combine(workingPath, "SharedRepository");
                Directory.CreateDirectory(sharedRepository);
                Util.CreateTestPackage("Newton.Calc", "1.0.0", sharedRepository);

                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""Newton.Calc"" version=""1.0.0"" targetFramework=""net48"" />
  <package id=""Great.Calc"" version=""1.0.0"" targetFramework=""net48"" />
  <package id=""Another.Calc"" version=""1.0.0"" targetFramework=""net48"" />
</packages>");

                var configPath1 = Path.Combine(workingPath, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath1, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PublicRepository"" value=""{publicRepositoryPath}"" />
    <add key=""SharedRepository"" value=""{sharedRepository}"" />
    </packageSources>
    <packageSourceMapping>
        <clear />
        <packageSource key=""PublicRepository"">
            <package pattern=""Newton.*"" />
            <package pattern=""Great.*"" />
            <package pattern=""Another.*"" />
        </packageSource>
        <packageSource key=""SharedRepository"">
            <package pattern=""Some.*"" /> <!--Newton.* prefix exist in other repository, not this one. -->
        </packageSource>
    </packageSourceMapping>
</configuration>");

                var packagePath = Path.Combine(workingPath, "packages");

                string[] args = new string[]
                    {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath
                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_failureCode, r.ExitCode);
                // Since Newton.* prefix direct 'Newton.Calc.1.0.0' to PublicRepository it should fail to find it, it only exist in SharedRepository.
                Assert.True(r.Errors.Contains("Package 'Newton.Calc.1.0.0' is not found"));
            }
        }

        [Fact]
        public void RestoreCommand_LongestPackageSourceMappingPatternMatches_Succeed()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingPath, "proj1");
                Directory.CreateDirectory(proj1Directory);

                var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
                File.WriteAllText(
                    proj1File,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                var opensourceRepositoryPath = Path.Combine(workingPath, "PublicRepository");
                Directory.CreateDirectory(opensourceRepositoryPath);
                Util.CreateTestPackage("测试更新包", "1.0.0", opensourceRepositoryPath);
                Util.CreateTestPackage("Contoso.Opensource.Buffers", "1.0.0", opensourceRepositoryPath);

                var sharedRepositoryPath = Path.Combine(workingPath, "SharedRepository");
                Directory.CreateDirectory(sharedRepositoryPath);
                Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath);

                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
  <package id=""Contoso.Opensource.Buffers"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                var configPath = Path.Combine(workingPath, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
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
            <package pattern=""Contoso.MVC.ASP"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

                var packagePath = Path.Combine(workingPath, "packages");

                string[] args = new string[]
                    {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath,
                        "-Verbosity",
                        "d"
                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                Assert.Contains("Package source mapping matches found for package ID 'Contoso.MVC.ASP' are: 'SharedRepository'", r.Output);
            }
        }

        [Fact]
        public void RestoreCommand_PackageSourceMappingLongerMatches_NoPatternMatchesLog()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingPath, "proj1");
                Directory.CreateDirectory(proj1Directory);

                var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
                File.WriteAllText(
                    proj1File,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                var opensourceRepositoryPath = Path.Combine(workingPath, "PublicRepository");
                Directory.CreateDirectory(opensourceRepositoryPath);
                Util.CreateTestPackage("My.MVC.ASP", "1.0.0", opensourceRepositoryPath);
                Util.CreateTestPackage("Contoso.Opensource.Buffers", "1.0.0", opensourceRepositoryPath);

                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""My.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                var configPath = Path.Combine(workingPath, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
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
</configuration>");

                var packagePath = Path.Combine(workingPath, "packages");

                string[] args = new string[]
                    {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath,
                        "-verbosity detailed"                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_failureCode, r.ExitCode);
                Assert.Contains("Package source mapping match not found for package ID 'My.MVC.ASP'", r.AllOutput);
                Assert.Contains("Unable to find version '1.0.0' of package 'My.MVC.ASP'.", r.Errors);
            }
        }

        [Fact]
        public void RestoreCommand_PackageNamespaceMatchesMultipleSources_Succeed()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingPath, "proj1");
                Directory.CreateDirectory(proj1Directory);

                var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
                File.WriteAllText(
                    proj1File,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                var sharedRepositoryPath1 = Path.Combine(workingPath, "SharedRepository1");
                Directory.CreateDirectory(sharedRepositoryPath1);
                Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath1);

                var sharedRepositoryPath2 = Path.Combine(workingPath, "SharedRepository2");
                Directory.CreateDirectory(sharedRepositoryPath2);
                Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath2);

                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                var configPath = Path.Combine(workingPath, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""SharedRepositoryPath1"" value=""{sharedRepositoryPath1}"" />
    <add key=""SharedRepositoryPath2"" value=""{sharedRepositoryPath2}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""SharedRepositoryPath1"">
            <package pattern=""Contoso.MVC.*"" /> <!--Same package pattern prefix matches both repository -->
        </packageSource>
        <packageSource key=""SharedRepositoryPath2"">
            <package pattern=""Contoso.MVC.*"" /> <!--Same package pattern prefix matches both repository -->
        </packageSource>
    </packageSourceMapping>
</configuration>");

                var packagePath = Path.Combine(workingPath, "packages");

                string[] args = new string[]
                    {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath
                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var contosoRestorePath = Path.Combine(packagePath, "Contoso.MVC.ASP.1.0.0", "Contoso.MVC.ASP.1.0.0.nupkg");
                Assert.True(File.Exists(contosoRestorePath));
            }
        }

        [Fact]
        public void RestoreCommand_PackageSourceMappingEmptyPackageSource_Succeed()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.SolutionRoot;
                var proj1Directory = Path.Combine(workingPath, "proj1");
                Directory.CreateDirectory(proj1Directory);

                var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
                File.WriteAllText(
                    proj1File,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                var sharedRepositoryPath1 = Path.Combine(workingPath, "SharedRepository1");
                Directory.CreateDirectory(sharedRepositoryPath1);
                Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath1);

                var sharedRepositoryPath2 = Path.Combine(workingPath, "SharedRepository2");
                Directory.CreateDirectory(sharedRepositoryPath2);
                Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath2);

                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                var configPath = Path.Combine(workingPath, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""SharedRepositoryPath1"" value=""{sharedRepositoryPath1}"" />
    <add key=""SharedRepositoryPath2"" value=""{sharedRepositoryPath2}"" />
    </packageSources>
    <packageSourceMapping>
        <!--Empty packageSourceMapping -->
    </packageSourceMapping>
</configuration>");

                var packagePath = Path.Combine(workingPath, "packages");

                string[] args = new string[]
                    {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath
                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var contosoRestorePath = Path.Combine(packagePath, "Contoso.MVC.ASP.1.0.0", "Contoso.MVC.ASP.1.0.0.nupkg");
                Assert.True(File.Exists(contosoRestorePath));
            }
        }

        [Fact]
        public void RestoreCommand_NoPackageSourceMappingsection_NoSourceRelatedLogMessage()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.SolutionRoot;
                var proj1Directory = Path.Combine(workingPath, "proj1");
                Directory.CreateDirectory(proj1Directory);

                var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
                File.WriteAllText(
                    proj1File,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");
                var sharedRepositoryPath = Path.Combine(workingPath, "SharedRepository");
                Directory.CreateDirectory(sharedRepositoryPath);
                Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath);

                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                var configPath = Path.Combine(workingPath, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""SharedRepository"" value=""{sharedRepositoryPath}"" />
    </packageSources>
</configuration>");

                var packagePath = Path.Combine(workingPath, "packages");

                string[] args = new string[]
                    {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath,
                        "-Verbosity",
                        "d"
                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                Assert.DoesNotContain("pattern", r.Output);
            }
        }

        [Fact]
        public void RestoreCommand_PackageSourceMappingFilter_Cli_WithAllSourceOptions_Succeed()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();
            using var workingPath = TestDirectory.Create();
            var proj1Directory = Path.Combine(workingPath, "proj1");
            Directory.CreateDirectory(proj1Directory);

            var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
            File.WriteAllText(
                proj1File,
                @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

            var opensourceRepositoryPath = Path.Combine(workingPath, "PublicRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);
            Util.CreateTestPackage("Contoso.Opensource.A", "1.0.0", opensourceRepositoryPath);

            var sharedRepositoryPath = Path.Combine(workingPath, "SharedRepository");
            Directory.CreateDirectory(sharedRepositoryPath);
            Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath);

            Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""Contoso.Opensource.A"" version=""1.0.0"" targetFramework=""net461"" />
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

            var configPath = Path.Combine(workingPath, "nuget.config");
            SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
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
            <package pattern=""Contoso.MVC.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            var packagePath = Path.Combine(workingPath, "packages");

            string[] args = new string[]
                {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath,
                        "-Verbosity",
                        "d",
                        "-source",
                        sharedRepositoryPath + ';' + opensourceRepositoryPath  //We pass both repositories as source
                };

            // Act
            var r = CommandRunner.Run(
                nugetexe,
                workingPath,
                string.Join(" ", args));

            // Assert
            Assert.Equal(_successCode, r.ExitCode);
            // If we pass source then log include actual path to repository instead of repository name.
            Assert.Contains($"Package source mapping matches found for package ID 'Contoso.MVC.ASP' are: 'SharedRepository'", r.Output);
            Assert.Contains($"Package source mapping matches found for package ID 'Contoso.Opensource.A' are: 'PublicRepository'", r.Output);
        }

        [Fact]
        public void RestoreCommand_PackageSourceMappingFilter_Cli_WithNotEnoughSourceOption_Fails()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();
            using var workingPath = TestDirectory.Create();
            var proj1Directory = Path.Combine(workingPath, "proj1");
            Directory.CreateDirectory(proj1Directory);

            var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
            File.WriteAllText(
                proj1File,
                @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

            var opensourceRepositoryPath = Path.Combine(workingPath, "PublicRepository");
            Directory.CreateDirectory(opensourceRepositoryPath);
            Util.CreateTestPackage("Contoso.Opensource", "1.0.0", opensourceRepositoryPath);
            Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", opensourceRepositoryPath);

            var sharedRepositoryPath = Path.Combine(workingPath, "SharedRepository");
            Directory.CreateDirectory(sharedRepositoryPath);
            Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath);

            Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""Contoso.Opensource"" version=""1.0.0"" targetFramework=""net461"" />
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

            var configPath = Path.Combine(workingPath, "nuget.config");
            SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
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
            <package pattern=""Contoso.MVC.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

            var packagePath = Path.Combine(workingPath, "packages");

            string[] args = new string[]
                {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath,
                        "-source",
                        opensourceRepositoryPath, // We only pass 1 source here
                        "-Verbosity",
                        "d"
                };

            // Act
            var r = CommandRunner.Run(
                nugetexe,
                workingPath,
                string.Join(" ", args));

            // Assert
            Assert.Contains($"Package source mapping matches found for package ID 'Contoso.MVC.ASP' are: 'SharedRepository'", r.Output);
            // Even though there is eligible source SharedRepository exist but only opensourceRepositoryPath passed as option it'll fail to restore.
            Assert.Equal(_failureCode, r.ExitCode);
            Assert.Contains("Unable to find version '1.0.0' of package 'Contoso.Opensource'.", r.Errors);
            Assert.Contains("Unable to find version '1.0.0' of package 'Contoso.MVC.ASP'.", r.Errors);
        }

        [Fact]
        public void RestoreCommand_PackageSourceMapping_InternationalSources_SearchMatchWithOneSource()
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();

            using (var workingPath = TestDirectory.Create())
            {
                var proj1Directory = Path.Combine(workingPath, "proj1");
                Directory.CreateDirectory(proj1Directory);

                var proj1File = Path.Combine(proj1Directory, "proj1.csproj");
                File.WriteAllText(
                    proj1File,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='packages.config' />
  </ItemGroup>
</Project>");

                var sharedRepositoryPath1 = Path.Combine(workingPath, "SharedRepository1");
                Directory.CreateDirectory(sharedRepositoryPath1);
                Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath1);

                var sharedRepositoryPath2 = Path.Combine(workingPath, "SharedRepository2");
                Directory.CreateDirectory(sharedRepositoryPath2);
                Util.CreateTestPackage("Contoso.MVC.ASP", "1.0.0", sharedRepositoryPath2);

                Util.CreateFile(proj1Directory, "packages.config",
@"<packages>
  <package id=""Contoso.MVC.ASP"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                var configPath = Path.Combine(workingPath, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""encyclopaedia"" value=""{sharedRepositoryPath1}"" />
    <add key=""encyclopædia"" value=""{sharedRepositoryPath2}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""encyclopædia"">
            <package pattern=""Contoso.MVC.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

                var packagePath = Path.Combine(workingPath, "packages");

                string[] args = new string[]
                    {
                        "restore",
                        proj1File,
                        "-solutionDir",
                        workingPath,
                        "-Verbosity",
                        "d"
                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.Equal(_successCode, r.ExitCode);
                var contosoRestorePath = Path.Combine(packagePath, "Contoso.MVC.ASP.1.0.0", "Contoso.MVC.ASP.1.0.0.nupkg");
                Assert.True(File.Exists(contosoRestorePath));
                Assert.Contains($"Added package 'Contoso.MVC.ASP.1.0.0' to folder '{packagePath}' from source '{sharedRepositoryPath2}'", r.Output);
            }
        }

        private static byte[] GetResource(string name)
        {
            return ResourceTestUtility.GetResourceBytes(
                $"NuGet.CommandLine.Test.compiler.resources.{name}",
                typeof(NuGetRestoreCommandTest));
        }

        private static string GetHash(string packagePath)
        {
            using (var stream = new FileStream(packagePath, FileMode.Open, FileAccess.Read))
            {
                using (var sha512 = SHA512.Create())
                {
                    return Convert.ToBase64String(sha512.ComputeHash(stream));
                }
            }
        }
    }
}

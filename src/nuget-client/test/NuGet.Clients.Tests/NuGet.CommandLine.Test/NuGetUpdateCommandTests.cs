// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class NuGetUpdateCommandTests
    {
        [Fact]
        public async Task UpdateCommand_Success_Update_DeletedFile()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;
                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));
                var b1 = new PackageIdentity("B", new NuGetVersion("1.0.0"));
                var b2 = new PackageIdentity("B", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework> { NuGetFramework.Parse("net45") },
                    "test.txt"
                    );

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework> { NuGetFramework.Parse("net45") },
                   "test.txt"
                    );

                var b1Package = Util.CreateTestPackage(
                    b1.Id,
                    b1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework> { NuGetFramework.Parse("net45") },
                    "test.txt"
                    );

                var b2Package = Util.CreateTestPackage(
                    b2.Id,
                    b2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework> { NuGetFramework.Parse("net45") },
                    "test.txt"
                    );

                Directory.CreateDirectory(projectDirectory);

                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent(contentFiles: new[] { "test.txt" }));

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);

                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                using (var stream = File.OpenRead(b1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        b1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory,
                    "-NonInteractive"
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                var content = File.ReadAllText(projectFile);

                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "B.2.0.0", "lib", "net45", "B.dll"))));
                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "A.dll"))));
                Assert.True(content.Contains(@"<Content Include=""test.txt"" />"));
            }
        }

        [Fact]
        public async Task UpdateCommand_Success_References()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;
                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });


                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var packagesFolder = PathUtility.GetRelativePath(projectDirectory, packagesDirectory);

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                var content = File.ReadAllText(projectFile);
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "file.dll"))));
            }
        }

        [Fact]
        public async Task UpdateCommand_Success_References_MultipleProjects()
        {

            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;
                var projectDirectory1 = Path.Combine(solutionDirectory, "proj1");
                var projectDirectory2 = Path.Combine(solutionDirectory, "proj2");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var b1 = new PackageIdentity("B", new NuGetVersion("1.0.0"));
                var b2 = new PackageIdentity("B", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var b1Package = Util.CreateTestPackage(
                    b1.Id,
                    b1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var b2Package = Util.CreateTestPackage(
                    b2.Id,
                    b2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                Directory.CreateDirectory(projectDirectory1);

                Util.CreateFile(
                    projectDirectory1,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Directory.CreateDirectory(projectDirectory2);

                Util.CreateFile(
                    projectDirectory2,
                    "proj2.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile1 = Path.Combine(projectDirectory1, "proj1.csproj");
                var projectFile2 = Path.Combine(projectDirectory2, "proj2.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem1 = new MSBuildProjectSystem(msbuildDirectory, projectFile1, testNuGetProjectContext);
                var projectSystem2 = new MSBuildProjectSystem(msbuildDirectory, projectFile2, testNuGetProjectContext);
                var msBuildProject1 = new MSBuildNuGetProject(projectSystem1, packagesDirectory, projectDirectory1);
                var msBuildProject2 = new MSBuildNuGetProject(projectSystem2, packagesDirectory, projectDirectory2);

                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject1.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                using (var stream = File.OpenRead(b1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject2.InstallPackageAsync(
                        b1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                projectSystem1.Save();
                projectSystem2.Save();

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory,
                    "-Id",
                    "A"
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                var content1 = File.ReadAllText(projectFile1);
                Assert.False(content1.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.True(content1.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content1.Contains(Util.GetHintPath(Path.Combine("packages", "B.1.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content1.Contains(Util.GetHintPath(Path.Combine("packages", "B.2.0.0", "lib", "net45", "file.dll"))));

                var content2 = File.ReadAllText(projectFile2);
                Assert.True(content2.Contains(Util.GetHintPath(Path.Combine("packages", "B.1.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content2.Contains(Util.GetHintPath(Path.Combine("packages", "B.2.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content2.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content2.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "file.dll"))));
            }
        }

        [Fact]
        public async Task UpdateCommand_Fails_References_MultipleProjectsInSameDirectory()
        {
            // Arrange
            using (var packagesSourceDirectory = TestDirectory.Create())
            using (var solutionDirectory = TestDirectory.Create())
            using (var workingPath = TestDirectory.Create())
            {
                var projectDirectory = Path.Combine(solutionDirectory, "proj");
                var packagesDirectory = Path.Combine(solutionDirectory, "packages");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                Directory.CreateDirectory(projectDirectory);

                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile1 = Path.Combine(projectDirectory, "proj1.csproj");
                var packagesConfigFile = Path.Combine(projectDirectory, "packages.config");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem1 = new MSBuildProjectSystem(msbuildDirectory, projectFile1, testNuGetProjectContext);
                var msBuildProject1 = new MSBuildNuGetProject(projectSystem1, packagesDirectory, projectDirectory);

                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject1.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                projectSystem1.Save();

                var projectFile2 = Path.Combine(projectDirectory, "proj2.csproj");
                File.Copy(projectFile1, projectFile2);

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory,
                    "-Id",
                    "A"
                };

                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                Assert.Contains("Scanning for projects...", r.Output);
                Assert.Contains($"WARNING: Found multiple project files for '{packagesConfigFile}'.", r.Output);
                Assert.Contains("No projects found with packages.config.", r.Output);

                var content1 = File.ReadAllText(projectFile1);
                Assert.True(content1.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content1.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "file.dll"))));

                var content2 = File.ReadAllText(projectFile2);
                Assert.True(content2.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content2.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "file.dll"))));
            }
        }

        [Fact]
        public async Task UpdateCommand_Success_NOPrerelease()
        {
            using (var packagesSourceDirectory = TestDirectory.Create())
            using (var solutionDirectory = TestDirectory.Create())
            using (var workingPath = TestDirectory.Create())
            {
                var projectDirectory = Path.Combine(solutionDirectory, "proj1");
                var packagesDirectory = Path.Combine(solutionDirectory, "packages");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0-beta"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });


                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var packagesFolder = PathUtility.GetRelativePath(projectDirectory, packagesDirectory);

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory,
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                var content = File.ReadAllText(projectFile);
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0-beta", "lib", "net45", "file.dll"))));
            }
        }


        [Fact]
        public async Task UpdateCommand_Success_Prerelease()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;
                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0-BETA"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });


                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var packagesFolder = PathUtility.GetRelativePath(projectDirectory, packagesDirectory);

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory,
                    "-Prerelease",
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                var content = File.ReadAllText(projectFile);
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0-BETA", "lib", "net45", "file.dll"))));
            }
        }

        [Theory]
        [InlineData("1.0.0", "2.0.0-BETA")]
        [InlineData("1.0.0-BETA", "2.0.0-BETA")]
        [InlineData("2.0.0-BETA", "2.0.0")]
        [InlineData("2.0.0-BETA", "2.0.0-BETA2")]
        [InlineData("2.0.0-BETA", "2.0.1")]
        public async Task UpdateCommand_Success_Prerelease_With_Id(string oldVersion, string newVersion)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;
                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                var a1 = new PackageIdentity("A", new NuGetVersion(oldVersion));
                var a2 = new PackageIdentity("A", new NuGetVersion(newVersion));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });


                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var packagesFolder = PathUtility.GetRelativePath(projectDirectory, packagesDirectory);

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Id",
                    "A",
                    "-Source",
                    packagesSourceDirectory,
                    "-Prerelease",
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                var content = File.ReadAllText(projectFile);
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A." + oldVersion.ToString(), "lib", "net45", "file.dll"))));
                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "A." + newVersion.ToString(), "lib", "net45", "file.dll"))));
            }
        }


        [Fact]
        public async Task UpdateCommand_Success_Version_Upgrade()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;
                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));
                var a3 = new PackageIdentity("A", new NuGetVersion("3.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var a3Package = Util.CreateTestPackage(
                    a3.Id,
                    a3.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var packagesFolder = PathUtility.GetRelativePath(projectDirectory, packagesDirectory);

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory,
                    "-Version",
                    "2.0.0",
                    "-Id",
                    "A"
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                var content = File.ReadAllText(projectFile);
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.3.0.0", "lib", "net45", "file.dll"))));
            }
        }

        [Fact]
        public async Task UpdateCommand_Success_Version_Downgrade()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;
                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));
                var a3 = new PackageIdentity("A", new NuGetVersion("3.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var a3Package = Util.CreateTestPackage(
                    a3.Id,
                    a3.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var packagesFolder = PathUtility.GetRelativePath(projectDirectory, packagesDirectory);

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a2Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a2,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory,
                    "-Version",
                    "1.0.0",
                    "-Id",
                    "A"
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                var content = File.ReadAllText(projectFile);
                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "file.dll"))));
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.3.0.0", "lib", "net45", "file.dll"))));
            }
        }

        [Fact]
        public async Task UpdateCommand_Success_ProjectFile_References()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;

                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });


                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var packagesFolder = PathUtility.GetRelativePath(projectDirectory, packagesDirectory);

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                var args = new[]
                {
                    "update",
                    projectFile,
                    "-Source",
                    packagesSourceDirectory
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);

                var content = File.ReadAllText(projectFile);
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "file.dll"))));
            }
        }

        [Fact]
        public async Task UpdateCommand_Success_PackagesConfig_References()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var workingPath = pathContext.WorkingDirectory;

                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });


                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var packagesFolder = PathUtility.GetRelativePath(projectDirectory, packagesDirectory);

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");
                var packagesConfigFile = Path.Combine(projectDirectory, "packages.config");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                var args = new[]
                {
                    "update",
                    projectFile,
                    "-Source",
                    packagesSourceDirectory
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);
                System.Console.WriteLine(r.Output);

                var content = File.ReadAllText(projectFile);
                Assert.False(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "A.2.0.0", "lib", "net45", "file.dll"))));
            }
        }

        [Fact]
        public async Task UpdateCommand_Success_ContentFiles()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var workingPath = pathContext.WorkingDirectory;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;

                var projectDirectory = Path.Combine(solutionDirectory, "proj1");
                var packagesDirectory = Path.Combine(solutionDirectory, "packages");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    licenseUrl: null,
                    contentFiles: "test1.txt");

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    licenseUrl: null,
                    contentFiles: "test2.txt");

                var packagesFolder = PathUtility.GetRelativePath(projectDirectory, packagesDirectory);

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                // Since, content files do not get added by MSBuildProjectSystem used by nuget.exe only,
                // the csproj file will not have the entries for content files.
                // Note that MSBuildProjectSystem used by nuget.exe only, can add or remove references from csproj.
                // Replace proj1.csproj with content files on it, like Install-Package from VS would have.
                File.Delete(projectFile);
                File.WriteAllText(projectFile,
                    Util.CreateProjFileContent(
                        "proj1",
                        "v4.5",
                        references: null,
                        contentFiles: new[] { "test1.txt" }));

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory
                };

                var test1textPath = Path.Combine(projectDirectory, "test1.txt");
                var test2textPath = Path.Combine(projectDirectory, "test2.txt");
                var packagesConfigPath = Path.Combine(projectDirectory, "packages.config");

                Assert.True(File.Exists(test1textPath), "Content file test1.txt should exist but does not.");
                Assert.False(File.Exists(test2textPath), "Content file test2.txt should not exist but does.");
                using (var packagesConfigStream = File.OpenRead(packagesConfigPath))
                {
                    var packagesConfigReader = new PackagesConfigReader(packagesConfigStream);
                    var packages = packagesConfigReader.GetPackages().ToList();
                    Assert.Equal(1, packages.Count);
                    Assert.Equal(a1, packages[0].PackageIdentity);
                }

                // Act
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                // Assert
                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);
                Assert.False(File.Exists(test1textPath), "Content file test1.txt should not exist but does.");
                Assert.True(File.Exists(test2textPath), "Content file test2.txt should exist but does not.");

                using (var packagesConfigStream = File.OpenRead(packagesConfigPath))
                {
                    var packagesConfigReader = new PackagesConfigReader(packagesConfigStream);
                    var packages = packagesConfigReader.GetPackages().ToList();
                    Assert.Equal(1, packages.Count);
                    Assert.Equal(a2, packages[0].PackageIdentity);
                }
            }
        }


        [Fact]
        public async Task UpdateCommand_Success_CustomPackagesFolder_RelativePath()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                // Use a different folder name instead of 'packages'
                var packagesDirectory = Path.Combine(solutionDirectory, "custom-pcks");

                // Create a couple of packages
                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                // Create a nuget.config file with a relative 'repositoryPath' setting
                Util.CreateNuGetConfig(solutionDirectory, new[] { packagesSourceDirectory }.ToList(), "custom-pcks");

                var projectDirectory = Path.Combine(solutionDirectory, "proj1");
                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();

                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;

                var projectSystem1 = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject1 = new MSBuildNuGetProject(projectSystem1, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject1.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                // The install should create custom the packages folder
                Assert.True(Directory.Exists(packagesDirectory));

                // Run the update command on the solution, which should update package 'A' to v2 into the custom packages folder.
                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    solutionDirectory,
                    string.Join(" ", args),
                    waitForExit: true);

                // Should be no errors returned - used to fail as update command assumed folder was <solutiondir>\packages.
                Assert.Empty(r.Errors);

                // Check that the new version is installed into the custom folder
                Assert.True(Directory.Exists(Path.Combine(packagesDirectory, "A.2.0.0")));

                // Check the custom package folder is used in the assembly reference
                var content1 = File.ReadAllText(projectFile);
                Assert.False(content1.Contains(Util.GetHintPath(Path.Combine("custom-pcks", "A.1.0.0", "lib", "net45", "file.dll"))));
                Assert.True(content1.Contains(Util.GetHintPath(Path.Combine("custom-pcks", "A.2.0.0", "lib", "net45", "file.dll"))));
            }
        }

        [Theory]
        [InlineData("packages.config")]
        [InlineData("packages.proj1.config")]
        public void UpdateCommand_FromProjectConfig(string configFileName)
        {
            // Arrange
            var nugetexe = Util.GetNuGetExePath();
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    licenseUrl: null,
                    contentFiles: "test1.txt");

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    licenseUrl: null,
                    contentFiles: "test2.txt");

                //Create solution file
                Directory.CreateDirectory(projectDirectory);
                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                //Create project file
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    string.Format(CultureInfo.InvariantCulture,
                    @"<Project ToolsVersion='4.0' DefaultTargets='Build'
    xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>out</OutputPath>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
  </PropertyGroup>
  <ItemGroup>
    <None Include='{0}' />
  </ItemGroup>
</Project>", configFileName));

                //Create packages config file
                Util.CreateFile(
                    projectDirectory,
                    configFileName,
                    @"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id=""A"" version=""1.0.0"" targetFramework=""net45"" />
</packages>");

                var restoreArgs = new[]
                {
                    "restore",
                    Path.Combine(projectDirectory, "proj1.csproj"),
                    "-Source",
                    packagesSourceDirectory,
                    "-SolutionDirectory",
                    solutionDirectory
                };

                var restoreResult = CommandRunner.Run(
                    nugetexe,
                    solutionDirectory,
                    string.Join(" ", restoreArgs),
                    waitForExit: true);

                // Act
                var args = new[]
                {
                    "update",
                    Path.Combine(projectDirectory, configFileName),
                    "-Source",
                    packagesSourceDirectory
                };

                var r = CommandRunner.Run(
                nugetexe,
                solutionDirectory,
                string.Join(" ", args),
                waitForExit: true);

                // Assert
                Assert.True(r.Success, r.Output + " " + r.Errors);
            }
        }

        [Fact]
        public async Task UpdateCommand_Success_CustomPackagesFolder_AbsolutePath()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var packagesDirectory = pathContext.PackagesV2;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;

                // Create some packages
                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { NuGetFramework.Parse("net45") },
                    new List<PackageDependencyGroup>() { });

                var projectDirectory = Path.Combine(solutionDirectory, "proj1");

                Directory.CreateDirectory(projectDirectory);
                // create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem1 = new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject1 = new MSBuildNuGetProject(projectSystem1, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject1.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                // The install should create custom the packages folder
                Assert.True(Directory.Exists(packagesDirectory));

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory
                };

                // Run the update command on the solution
                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    solutionDirectory,
                    string.Join(" ", args),
                    waitForExit: true);

                // Should be no errors returned - used to fail as update command assumed folder was <solutiondir>\packages.
                Assert.Empty(r.Errors);

                // Check that the new version is installed into the custom folder
                Assert.True(Directory.Exists(Path.Combine(packagesDirectory, "A.2.0.0")));

                // Check the custom package folder is used in the assembly reference
                var content1 = File.ReadAllText(projectFile);
                var customPackageFolderName = new DirectoryInfo(packagesDirectory).Name;
                var a1Path = Path.DirectorySeparatorChar + customPackageFolderName + Path.DirectorySeparatorChar +
                    Path.Combine("A.1.0.0", "lib", "net45", "file.dll");
                var a2Path = Path.DirectorySeparatorChar + customPackageFolderName + Path.DirectorySeparatorChar +
                    Path.Combine("A.2.0.0", "lib", "net45", "file.dll");
                Assert.DoesNotContain(a1Path, content1);
                Assert.Contains(a2Path, content1);
            }
        }


        [Fact]
        public async Task UpdateCommand_Native_JS_Projects_Success()
        {
            using (var packagesSourceDirectory = TestDirectory.Create())
            using (var solutionDirectory = TestDirectory.Create())
            using (var workingPath = TestDirectory.Create())
            {
                var projectDirectory1 = Path.Combine(solutionDirectory, "proj1");
                var projectDirectory2 = Path.Combine(solutionDirectory, "proj2");
                var packagesDirectory = Path.Combine(solutionDirectory, "packages");

                var a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                var a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                var b1 = new PackageIdentity("B", new NuGetVersion("1.0.0"));
                var b2 = new PackageIdentity("B", new NuGetVersion("2.0.0"));

                var a1Package = Util.CreateTestPackage(
                    a1.Id,
                    a1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { },
                    "test.txt");

                var a2Package = Util.CreateTestPackage(
                    a2.Id,
                    a2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { },
                    "test.txt");

                var b1Package = Util.CreateTestPackage(
                    b1.Id,
                    b1.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { },
                    "test.txt");

                var b2Package = Util.CreateTestPackage(
                    b2.Id,
                    b2.Version.ToString(),
                    packagesSourceDirectory,
                    new List<NuGetFramework>() { },
                    "test.txt");

                Directory.CreateDirectory(projectDirectory1);

                Util.CreateFile(
                    projectDirectory1,
                    "proj1.jsproj",
                    Util.CreateProjFileContent());

                Directory.CreateDirectory(projectDirectory2);

                Util.CreateFile(
                    projectDirectory2,
                    "proj2.vcxproj",
                    Util.CreateProjFileContent());

                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());

                var projectFile1 = Path.Combine(projectDirectory1, "proj1.jsproj");
                var projectFile2 = Path.Combine(projectDirectory2, "proj2.vcxproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem1 = new MSBuildProjectSystem(msbuildDirectory, projectFile1, testNuGetProjectContext);
                var projectSystem2 = new MSBuildProjectSystem(msbuildDirectory, projectFile2, testNuGetProjectContext);
                var msBuildProject1 = new MSBuildNuGetProject(projectSystem1, packagesDirectory, projectDirectory1);
                var msBuildProject2 = new MSBuildNuGetProject(projectSystem2, packagesDirectory, projectDirectory2);

                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject1.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                using (var stream = File.OpenRead(b1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject2.InstallPackageAsync(
                        b1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                projectSystem1.Save();
                projectSystem2.Save();

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory
                };

                var r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.Output + ". Error is " + r.Errors);
            }
        }

        [Theory]
        [InlineData(null, "2.0.0")]
        [InlineData("Lowest", "1.0.0")]
        [InlineData("Highest", "2.0.0")]
        [InlineData("HighestMinor", "1.2.0")]
        [InlineData("HighestPatch", "1.0.1")]
        public async Task UpdateCommand_DependencyResolution_Success(string dependencyVersion, string expectedVersion)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var workingPath = pathContext.WorkingDirectory;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                var projectDirectory = Path.Combine(solutionDirectory, "proj1");
                var packagesDirectory = pathContext.PackagesV2;
                var nugetFramework = NuGetFramework.Parse("net45");
                // version installed will be the 1.1.0  - Create Package a1
                var a1PackageIdentity = new PackageIdentity("A", new NuGetVersion("1.1.0"));
                var a1Package = Util.CreateTestPackage(a1PackageIdentity.Id, a1PackageIdentity.Version.ToString(), packagesSourceDirectory, new List<NuGetFramework>() { nugetFramework }, new List<PackageDependencyGroup>()
                    {
                        new PackageDependencyGroup(nugetFramework,new List<Packaging.Core.PackageDependency>(){new Packaging.Core.PackageDependency("dep",new VersionRange(new NuGetVersion("1.0.0")))})
                    });
                //Create package a2
                var a2PackageIdentity = new PackageIdentity("A", new NuGetVersion("2.0.0"));
                var a2Package = Util.CreateTestPackage(a1PackageIdentity.Id, a1PackageIdentity.Version.ToString(), packagesSourceDirectory, new List<NuGetFramework>() { nugetFramework }, new List<PackageDependencyGroup>()                     {
                        new PackageDependencyGroup(nugetFramework,new List<Packaging.Core.PackageDependency>(){new Packaging.Core.PackageDependency("dep",new VersionRange(new NuGetVersion("1.0.0")))})
                    });
                //Create all the test packages
                Util.CreateTestPackage("dep", "1.0.0", packagesSourceDirectory, new List<NuGetFramework>() { nugetFramework }, new List<PackageDependencyGroup>() { });
                Util.CreateTestPackage("dep", "1.0.1", packagesSourceDirectory, new List<NuGetFramework>() { nugetFramework }, new List<PackageDependencyGroup>() { });
                Util.CreateTestPackage("dep", "1.2.0", packagesSourceDirectory, new List<NuGetFramework>() { nugetFramework }, new List<PackageDependencyGroup>() { });
                Util.CreateTestPackage("dep", "2.0.0", packagesSourceDirectory, new List<NuGetFramework>() { nugetFramework }, new List<PackageDependencyGroup>() { });
                Directory.CreateDirectory(projectDirectory);
                //Create project 1
                Util.CreateFile(
                    projectDirectory,
                    "proj1.csproj",
                    Util.CreateProjFileContent());
                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateSolutionFileContent());
                var projectFile = Path.Combine(projectDirectory, "proj1.csproj");
                var solutionFile = Path.Combine(solutionDirectory, "a.sln");
                var testNuGetProjectContext = new TestNuGetProjectContext();
                var msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem =
                    new MSBuildProjectSystem(msbuildDirectory, projectFile, testNuGetProjectContext);
                var msBuildProject = new MSBuildNuGetProject(projectSystem, packagesDirectory, projectDirectory);
                using (var stream = File.OpenRead(a1Package))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject.InstallPackageAsync(
                        a1PackageIdentity,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }
                string[] args;
                //Test the case where the code is not provided to ensure it works as expected. (Highest by default)
                if (string.IsNullOrEmpty(dependencyVersion))
                {
                    args = new[]
                    {
                            "update",
                            solutionFile,
                            "-Source",
                            packagesSourceDirectory,
                        };
                }
                else
                {
                    args = new[]
                    {
                            "update",
                            solutionFile,
                            "-Source",
                            packagesSourceDirectory,
                            "-DependencyVersion",
                            dependencyVersion
                        };
                }

                //Act
                var commandRunResult = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                //Assert
                Assert.True(commandRunResult.ExitCode == 0, "Output is " + commandRunResult.Output + ". Error is " + commandRunResult.Errors);
                var content = File.ReadAllText(projectFile);
                // Assert no error
                Assert.Equal(0, commandRunResult.ExitCode);
                Assert.True(content.Contains(Util.GetHintPath(Path.Combine("packages", "dep." + expectedVersion, "lib", "net45", "file.dll"))));
            }
        }

        [Fact]
        public async Task UpdateCommand_Self_UpdateFromCustomSource()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var expectedFileContent = new byte[1];
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "NuGet.CommandLine",
                    Version = "99.99.99"
                };

                packageX.Files.Clear();
                packageX.Files.Add(new KeyValuePair<string, byte[]>(@"NuGet.exe", expectedFileContent));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Copy NuGet to a new location, and run update self.
                var nugetExe = Path.Combine(pathContext.WorkingDirectory, "NuGet.exe");
                File.Copy(Util.GetNuGetExePath(), nugetExe);

                Util.RunCommand(pathContext, nugetExe, 0, "update", "-self", "-source", pathContext.PackageSource);

                Assert.Equal(expectedFileContent, File.ReadAllBytes(nugetExe));
            }
        }

        [Fact]
        public void UpdateCommand_Self_FailsWithMoreThanOneSource()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Copy NuGet to a new location, and run update self.
                var nugetExe = Path.Combine(pathContext.WorkingDirectory, "NuGet.exe");
                File.Copy(Util.GetNuGetExePath(), nugetExe);

                CommandRunnerResult result = Util.RunCommand(pathContext, nugetExe, 1, "update", "-self", "-source", pathContext.PackageSource, "-source", pathContext.HttpCacheFolder);
                result.ExitCode.Equals(1);
                result.AllOutput.Contains(NuGetResources.Error_UpdateSelf_Source);
            }
        }

        [Fact]
        public async Task UpdateCommand_NF_Project_Success()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                //Arrange
                var workingPath = pathContext.WorkingDirectory;
                var solutionDirectory = pathContext.SolutionRoot;
                var packagesSourceDirectory = pathContext.PackageSource;
                // setup directories
                string projectDirectory1 = Path.Combine(solutionDirectory, "proj1");
                string projectDirectory2 = Path.Combine(solutionDirectory, "proj2");
                string packagesDirectory = Path.Combine(solutionDirectory, "packages");

                // create packages IDs
                PackageIdentity a1 = new PackageIdentity("A", new NuGetVersion("1.0.0"));
                PackageIdentity a2 = new PackageIdentity("A", new NuGetVersion("2.0.0"));

                PackageIdentity b1 = new PackageIdentity("B", new NuGetVersion("1.0.0"));
                PackageIdentity b2 = new PackageIdentity("B", new NuGetVersion("2.0.0"));

                // create packages
                var a1Package = new SimpleTestPackageContext()
                {
                    Id = a1.Id,
                    Version = a1.Version.ToString()
                };
                a1Package.Files.Clear();
                a1Package.AddFile($"lib/{a1.Id}.dll");

                var a2Package = new SimpleTestPackageContext()
                {
                    Id = a2.Id,
                    Version = a2.Version.ToString()
                };
                a2Package.Files.Clear();
                a2Package.AddFile($"lib/{a2.Id}.dll");

                var b1Package = new SimpleTestPackageContext()
                {
                    Id = b1.Id,
                    Version = b1.Version.ToString()
                };
                b1Package.Files.Clear();
                b1Package.AddFile($"lib/{b1.Id}.dll");

                var b2Package = new SimpleTestPackageContext()
                {
                    Id = b2.Id,
                    Version = b2.Version.ToString()
                };
                b2Package.Files.Clear();
                b2Package.AddFile($"lib/{b2.Id}.dll");

                await SimpleTestPackageUtility.CreatePackagesAsync(packagesSourceDirectory, a1Package, a2Package, b1Package, b2Package);

                // build list of packages (initial versions on the project files)
                var packages = new List<(string, string)>();
                packages.Add((a1.Id, a1.Version.ToString()));
                packages.Add((b1.Id, b1.Version.ToString()));

                // create everything related with project 1
                Directory.CreateDirectory(projectDirectory1);

                Util.CreateFile(
                    projectDirectory1,
                    "proj1.nfproj",
                    Util.GetNFProjXML(
                        "proj1",
                        packages));

                Util.CreateFile(
                    projectDirectory1,
                    "packages.config",
                    Util.GetNFPackageConfig(packages));

                // create everything related with project 2
                Directory.CreateDirectory(projectDirectory2);

                Util.CreateFile(
                    projectDirectory2,
                    "proj2.nfproj",
                    Util.GetNFProjXML(
                        "proj2",
                        packages));

                Util.CreateFile(
                    projectDirectory1,
                    "packages.config",
                    Util.GetNFPackageConfig(packages));

                List<string> projectList = new string[] { "proj1", "proj2" }.ToList();

                // create solution file
                Util.CreateFile(solutionDirectory, "a.sln",
                    Util.CreateNFSolutionFileContent(projectList));

                // get paths for projects and solutions
                string projectFile1 = Path.Combine(projectDirectory1, "proj1.nfproj");
                string projectFile2 = Path.Combine(projectDirectory2, "proj2.nfproj");
                string solutionFile = Path.Combine(solutionDirectory, "a.sln");

                var testNuGetProjectContext = new TestNuGetProjectContext();
                string msbuildDirectory = MsBuildUtility.GetMsBuildToolset(null, null).Path;
                var projectSystem1 = new MSBuildProjectSystem(msbuildDirectory, projectFile1, testNuGetProjectContext);
                var projectSystem2 = new MSBuildProjectSystem(msbuildDirectory, projectFile2, testNuGetProjectContext);
                var msBuildProject1 = new MSBuildNuGetProject(projectSystem1, packagesDirectory, projectDirectory1);
                var msBuildProject2 = new MSBuildNuGetProject(projectSystem2, packagesDirectory, projectDirectory2);

                var packagesInSource = LocalFolderUtility.GetPackagesV2(pathContext.PackageSource, Common.NullLogger.Instance);
                var a1File = packagesInSource.Single(e => e.Identity.Equals(a1Package.Identity));
                var b1File = packagesInSource.Single(e => e.Identity.Equals(b1Package.Identity));

                using (FileStream stream = File.OpenRead(a1File.Path))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject1.InstallPackageAsync(
                        a1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                using (FileStream stream = File.OpenRead(b1File.Path))
                {
                    var downloadResult = new DownloadResourceResult(stream, packagesSourceDirectory);
                    await msBuildProject2.InstallPackageAsync(
                        b1,
                        downloadResult,
                        testNuGetProjectContext,
                        CancellationToken.None);
                }

                projectSystem1.Save();
                projectSystem2.Save();

                var args = new[]
                {
                    "update",
                    solutionFile,
                    "-Source",
                    packagesSourceDirectory
                };

                CommandRunnerResult r = CommandRunner.Run(
                    Util.GetNuGetExePath(),
                    workingPath,
                    string.Join(" ", args),
                    waitForExit: true);

                Assert.True(r.ExitCode == 0, "Output is " + r.AllOutput + ". Error is " + r.Errors);
                Assert.Contains($"Successfully installed '{a2.Id} {a2.Version}'", r.AllOutput);
                Assert.Contains($"Successfully installed '{b2.Id} {b2.Version}'", r.AllOutput);
            }
        }

        [Fact]
        public async Task UpdateCommand_WithHttpSource_Warns()
        {
            //Arrange
            using var pathContext = new SimpleTestPathContext();
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var httpSourceDirectory = Path.Combine(pathContext.WorkingDirectory, "http-source");
            var packageA100 = new SimpleTestPackageContext("a", "1.0.0");
            var packageA200 = new SimpleTestPackageContext("a", "2.0.0");
            await SimpleTestPackageUtility.CreatePackagesAsync(httpSourceDirectory, packageA100, packageA200);
            var packageA100FileInfo = new FileInfo(Path.Combine(httpSourceDirectory, packageA100.PackageName));
            var packageA200FileInfo = new FileInfo(Path.Combine(httpSourceDirectory, packageA200.PackageName));

            using var server = Util.CreateMockServer(new[] { packageA100FileInfo, packageA200FileInfo });
            server.Start();

            var sourceUri = $"{server.Uri}nuget";

            var projectA = new SimpleTestProjectContext(
                  "a",
                  ProjectStyle.PackagesConfig,
                  pathContext.SolutionRoot);

            Util.CreateFile(Path.GetDirectoryName(projectA.ProjectPath), "packages.config",
@"<packages>
  <package id=""A"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            var args = new[]
            {
                    "restore",
                    solution.SolutionPath,
                    "-Source",
                    sourceUri
            };

            var restoreResult = CommandRunner.Run(
                Util.GetNuGetExePath(),
                pathContext.WorkingDirectory,
                string.Join(" ", args),
                waitForExit: true);
            restoreResult.Success.Should().BeTrue(restoreResult.AllOutput);
            args = new[]
            {
                    "update",
                    solution.SolutionPath,
                    "-Source",
                    sourceUri
            };

            // Act
            var r = CommandRunner.Run(
                Util.GetNuGetExePath(),
                pathContext.WorkingDirectory,
                string.Join(" ", args),
                waitForExit: true);
            server.Stop();

            // Assert
            r.Success.Should().BeTrue(r.AllOutput);
            r.AllOutput.Should().Contain("You are running the 'update' operation with an 'http' source");
        }
    }
}

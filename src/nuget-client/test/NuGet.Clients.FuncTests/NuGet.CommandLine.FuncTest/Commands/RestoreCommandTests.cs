// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.CommandLine.Test;
using NuGet.Common;
using NuGet.Configuration.Test;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.FuncTest.Commands
{
    public class RestoreCommandTests
    {
        private const int _successExitCode = 0;
        private const int _failureExitCode = 1;

        [Fact]
        public async Task Restore_LegacyPackageReference_WithNuGetLockFile()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                projectA.Properties.Add("RestorePackagesWithLockFile", "true");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var result = RunRestore(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                // There will be a "ridless" target, then one target per whichever RIDs the project system enables by default
                lockFile.Targets.All(t => t.Name.StartsWith(".NETFramework,Version=v4.6.1")).Should().BeTrue();

                var targets = lockFile.Targets.Where(t => t.Dependencies.Count > 0).ToList();
                Assert.Equal(1, targets.Count);
                Assert.Equal(".NETFramework,Version=v4.6.1", targets[0].Name);
                Assert.Equal(1, targets[0].Dependencies.Count);
                Assert.Equal("x", targets[0].Dependencies[0].Id);
            }
        }

        [Fact]
        public async Task Restore_LegacyPackageReference_WithNuGetLockFilePath()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = "net461";

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse(net461));

                var projectB = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "b",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse(net461));

                // set up packages
                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile($"lib/{0}/x.dll", net461);

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile($"lib/{0}/y.dll", net461);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                   pathContext.PackageSource,
                   PackageSaveMode.Defaultv3,
                   packageX,
                   packageY);

                // set up projects and solution
                projectB.AddPackageToAllFrameworks(packageY);
                projectA.Properties.Add("RestorePackagesWithLockFile", "true");
                var packagesLockFilePath = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "packages.custom.lock.json");
                projectA.Properties.Add("NuGetLockFilePath", packagesLockFilePath);
                projectA.AddProjectToAllFrameworks(projectB);
                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var result = RunRestore(pathContext);

                // Assert
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                Assert.Equal(packagesLockFilePath, projectA.NuGetLockFileOutputPath);

                var lockFile = PackagesLockFileFormat.Read(projectA.NuGetLockFileOutputPath);
                // There will be a "ridless" target, then one target per whichever RIDs the project system enables by default
                lockFile.Targets.All(t => t.Name.StartsWith(".NETFramework,Version=v4.6.1")).Should().BeTrue();

                var targets = lockFile.Targets.Where(t => t.Dependencies.Count > 0).ToList();
                Assert.Equal(1, targets.Count);
                Assert.Equal(".NETFramework,Version=v4.6.1", targets[0].Name);
                Assert.Equal(3, targets[0].Dependencies.Count);
                Assert.Equal("x", targets[0].Dependencies[0].Id);
                Assert.Equal(PackageDependencyType.Direct, targets[0].Dependencies[0].Type);
                Assert.Equal("y", targets[0].Dependencies[1].Id);
                Assert.Equal(PackageDependencyType.Transitive, targets[0].Dependencies[1].Type);
                Assert.Equal("b", targets[0].Dependencies[2].Id);
                Assert.Equal(PackageDependencyType.Project, targets[0].Dependencies[2].Type);
            }
        }

        [Fact]
        public async Task Restore_LegacyPackageReference_WithNuGetLockFileArgument()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var result = RunRestore(pathContext, _successExitCode, "-UseLockFile");

                // Assert
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
            }
        }

        [Fact]
        public async Task Restore_LegacyPackageReference_WithCustomNameNuGetLockFileArgument()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var result = RunRestore(pathContext, _successExitCode, "-UseLockFile", "-LockFilePath", "custom.lock.json");

                // Assert
                var expectedLockFileName = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "custom.lock.json");
                Assert.True(File.Exists(expectedLockFileName));
            }
        }

        [Fact]
        public async Task Restore_LegacyPackageReference_WithNuGetLockFileLockedMode()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile("lib/net461/y.dll");

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                var result = RunRestore(pathContext, _successExitCode, "-UseLockFile");
                Assert.True(result.Success);
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                projectA.AddPackageToAllFrameworks(packageY);
                projectA.Save();

                // Act
                result = RunRestore(pathContext, _failureExitCode, "-LockedMode");

                // Assert
                Assert.Contains("NU1004:", result.Errors);
                var logCodes = projectA.AssetsFile.LogMessages.Select(e => e.Code);
                Assert.Contains(NuGetLogCode.NU1004, logCodes);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Restore_LegacyPackageReference_ForceEvaluateNuGetLockFile(bool forceEvaluate)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                var result = RunRestore(pathContext, _successExitCode, "-UseLockFile");
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Act
                result = forceEvaluate
                    ? RunRestore(pathContext, _successExitCode, "-UseLockFile", "-ForceEvaluate")
                    : RunRestore(pathContext, _successExitCode, "-UseLockFile");

                // Assert
                Assert.Contains("Assets file has not changed.", result.AllOutput);
                if (forceEvaluate)
                {
                    Assert.Contains("Writing packages lock file at disk.", result.AllOutput);
                }
                else
                {
                    Assert.Contains("No-Op restore.", result.AllOutput);
                }
            }
        }

        [Fact]
        public async Task Restore_LegacyPackagesConfig_WithNuGetLockFileArgument()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");

                Util.CreateFile(Path.GetDirectoryName(projectA.ProjectPath), "packages.config",
@"<packages>
  <package id=""x"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var result = RunRestore(pathContext, _successExitCode, "-UseLockFile");

                // Assert
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
            }
        }

        [Fact]
        public async Task Restore_LegacyPackagesConfig_WithCustomNamedNuGetLockFile()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");

                Util.CreateFile(Path.GetDirectoryName(projectA.ProjectPath), "packages.config",
@"<packages>
  <package id=""x"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var result = RunRestore(pathContext, _successExitCode, "-UseLockFile", "-LockFilePath", "custom.lock.json");

                // Assert
                var expectedLockFileName = Path.Combine(Path.GetDirectoryName(projectA.ProjectPath), "custom.lock.json");
                Assert.True(File.Exists(expectedLockFileName));
            }
        }

        [Fact]
        public async Task Restore_LegacyPackagesConfig_WithNuGetLockFileLockedMode()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/x.dll");

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile("lib/net461/y.dll");

                Util.CreateFile(Path.GetDirectoryName(projectA.ProjectPath), "packages.config",
@"<packages>
  <package id=""x"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                // Restore to set up lock file
                var result = RunRestore(pathContext, _successExitCode, "-UseLockFile");
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));

                // Change packages to cause lock file difference
                Util.CreateFile(Path.GetDirectoryName(projectA.ProjectPath), "packages.config",
@"<packages>
  <package id=""y"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                // Act
                result = RunRestore(pathContext, _failureExitCode, "-UseLockFile", "-LockedMode");

                // Assert
                Assert.Contains("NU1004:", result.Errors);
            }
        }

        [Fact]
        public async Task RestorePackagesConfig_WithExistingLockFile_LockedMode_Succeeds()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/x.dll");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);
                Util.CreateFile(Path.GetDirectoryName(projectA.ProjectPath), "packages.config",
@"<packages>
  <package id=""x"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Preconditions, regular restore
                var result = RunRestore(pathContext, _successExitCode);
                result.Success.Should().BeTrue(because: result.AllOutput);
                new FileInfo(projectA.NuGetLockFileOutputPath).Exists.Should().BeFalse();

                // Write expected lock file
                var packagePath = LocalFolderUtility.GetPackagesV3(pathContext.PackageSource, NullLogger.Instance).Single().Path;

                string contentHash = null;
                using (var reader = new PackageArchiveReader(packagePath))
                {
                    contentHash = reader.GetContentHash(CancellationToken.None);
                }

                var expectedLockFile = GetResource("NuGet.CommandLine.FuncTest.compiler.resources.pc.packages.lock.json").Replace("TEMPLATE", contentHash);
                File.WriteAllText(projectA.NuGetLockFileOutputPath, expectedLockFile);

                // Run lockedmode restore.
                result = RunRestore(pathContext, _successExitCode, "-LockedMode");
                result.Success.Should().BeTrue(because: result.AllOutput);
                new FileInfo(projectA.NuGetLockFileOutputPath).Exists.Should().BeTrue();
            }
        }

        [Fact]
        public async Task Restore_LegacyPackageReference_EmbedInteropPackage()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");
                packageX.AddFile("embed/net461/a.dll");

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX);

                // Act
                var result = RunRestore(pathContext);

                // Assert
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);

                foreach (var target in assetsFile.Targets)
                {
                    var library = target.Libraries.FirstOrDefault(lib => lib.Name.Equals("x"));
                    Assert.NotNull(library);
                    Assert.True(library.EmbedAssemblies.Any(embed => embed.Path.Equals("embed/net461/a.dll")));
                    Assert.True(library.CompileTimeAssemblies.Any(embed => embed.Path.Equals("lib/net461/a.dll")));
                    Assert.True(library.RuntimeAssemblies.Any(embed => embed.Path.Equals("lib/net461/a.dll")));
                }
            }
        }

        [Fact]
        public async Task Restore_LegacyPackageReference_BuildTransitive()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile("lib/net461/y.dll");
                packageY.AddFile("build/y.targets");
                packageY.AddFile("buildCrossTargeting/y.targets");
                packageY.AddFile("build/y.props");
                packageY.AddFile("buildCrossTargeting/y.props");
                packageY.AddFile("buildTransitive/y.targets");
                packageY.Exclude = "build,analyzers";

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/x.dll");
                packageX.Dependencies.Add(packageY);

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                // Act
                var result = RunRestore(pathContext);

                // Assert
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);

                foreach (var target in assetsFile.Targets)
                {
                    var library = target.Libraries.FirstOrDefault(lib => lib.Name.Equals("y"));
                    Assert.NotNull(library);
                    Assert.True(library.Build.Any(build => build.Path.Equals("buildTransitive/y.targets")), $"All build assets: {string.Join(", ", library.Build.Select(e => e.Path))}");
                    Assert.False(library.Build.Any(build => build.Path.Equals("build/y.props")), $"All build assets: {string.Join(", ", library.Build.Select(e => e.Path))}");
                }
            }
        }

        [Fact]
        public async Task Restore_LegacyPackageReference_SkipBuildTransitive()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile("lib/net461/y.dll");
                packageY.AddFile("build/y.targets");
                packageY.AddFile("buildCrossTargeting/y.targets");
                packageY.AddFile("build/y.props");
                packageY.AddFile("buildCrossTargeting/y.props");
                packageY.AddFile("buildTransitive/y.targets");
                packageY.Exclude = "buildTransitive";

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/x.dll");
                packageX.Dependencies.Add(packageY);

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                // Act
                var result = RunRestore(pathContext);

                // Assert
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);

                foreach (var target in assetsFile.Targets)
                {
                    var library = target.Libraries.FirstOrDefault(lib => lib.Name.Equals("y"));
                    Assert.NotNull(library);
                    Assert.False(library.Build.Any(build => build.Path.Equals("buildTransitive/y.targets")));
                    Assert.False(library.Build.Any(build => build.Path.Equals("build/y.props")));
                }
            }
        }

        [Fact]
        public async Task Restore_LegacyPackageReference_P2P_BuildTransitive()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);
                projectA.Properties.Add("RestoreProjectStyle", "PackageReference");

                var projectB = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "b",
                    pathContext.SolutionRoot,
                    net461);

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile("lib/net461/y.dll");
                packageY.AddFile("build/y.targets");
                packageY.AddFile("buildCrossTargeting/y.targets");
                packageY.AddFile("buildTransitive/y.targets");

                projectB.AddPackageToAllFrameworks(packageY);
                projectA.AddProjectToAllFrameworks(projectB);
                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageY);

                // Act
                var result = RunRestore(pathContext);

                // Assert
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);

                foreach (var target in assetsFile.Targets)
                {
                    var library = target.Libraries.FirstOrDefault(lib => lib.Name.Equals("y"));
                    Assert.NotNull(library);
                    Assert.True(library.Build.Any(build => build.Path.Equals("buildTransitive/y.targets")));
                }
            }
        }

        [Fact]
        public async Task Restore_LegacyPackageReference_P2P_SkipBuildTransitive()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);
                projectA.Properties.Add("RestoreProjectStyle", "PackageReference");

                var projectB = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "b",
                    pathContext.SolutionRoot,
                    net461);

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile("lib/net461/y.dll");
                packageY.AddFile("build/y.targets");
                packageY.AddFile("buildCrossTargeting/y.targets");
                packageY.AddFile("buildTransitive/y.targets");
                packageY.PrivateAssets = "buildTransitive";

                projectB.AddPackageToAllFrameworks(packageY);
                projectA.AddProjectToAllFrameworks(projectB);
                solution.Projects.Add(projectA);
                solution.Projects.Add(projectB);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageY);

                // Act
                var result = RunRestore(pathContext);

                // Assert
                var assetsFile = projectA.AssetsFile;
                Assert.NotNull(assetsFile);

                foreach (var target in assetsFile.Targets)
                {
                    var library = target.Libraries.FirstOrDefault(lib => lib.Name.Equals("y"));
                    Assert.NotNull(library);
                    Assert.False(library.Build.Any(build => build.Path.Equals("buildTransitive/y.targets")));
                }
            }
        }

        [Fact]
        public async Task Restore_WithLockedModeAndNoObjFolder_RestoreFailsAndWritesOutRestoreResultFiles()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);
                projectA.Properties.Add("RuntimeIdentifier", "win");

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };
                packageX.Files.Clear();
                packageX.AddFile("lib/net461/a.dll");

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };
                packageY.Files.Clear();
                packageY.AddFile("lib/net461/y.dll");

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageX,
                    packageY);

                var result = RunRestore(pathContext, _successExitCode, "-UseLockFile");
                Assert.True(result.Success);
                Assert.True(File.Exists(projectA.NuGetLockFileOutputPath));
                var originalPackagesLockFileWriteTime = new FileInfo(projectA.NuGetLockFileOutputPath).LastWriteTimeUtc;

                projectA.AddPackageToAllFrameworks(packageY);
                projectA.Save();

                // Remove old obj folders.
                Directory.Delete(Path.GetDirectoryName(projectA.AssetsFileOutputPath), recursive: true);

                // Act
                result = RunRestore(pathContext, _failureExitCode, "-LockedMode");

                // Assert
                Assert.Contains("NU1004:", result.Errors);
                var assetsFile = projectA.AssetsFile;
                var logCodes = assetsFile.LogMessages.Select(e => e.Code);
                Assert.Contains(NuGetLogCode.NU1004, logCodes);
                Assert.Equal(2, assetsFile.Targets.Count);
                var ridlessMainTarget = assetsFile.Targets.FirstOrDefault(e => string.IsNullOrEmpty(e.RuntimeIdentifier));
                Assert.Equal(net461, ridlessMainTarget.TargetFramework);
                var ridMainTarget = assetsFile.Targets.FirstOrDefault(e => "win".Equals(e.RuntimeIdentifier));
                Assert.Equal("win", ridMainTarget.RuntimeIdentifier);
                Assert.True(File.Exists(projectA.PropsOutput));
                Assert.True(File.Exists(projectA.TargetsOutput));
                Assert.True(File.Exists(projectA.CacheFileOutputPath));
                var packagesLockFileWriteTime = new FileInfo(projectA.NuGetLockFileOutputPath).LastWriteTimeUtc;
                packagesLockFileWriteTime.Should().Be(originalPackagesLockFileWriteTime, because: "Locked mode must not overwrite the lock file");
            }
        }

        public static CommandRunnerResult RunRestore(SimpleTestPathContext pathContext, int expectedExitCode = 0, params string[] additionalArguments)
        {
            var nugetExe = Util.GetNuGetExePath();

            // Store the dg file for debugging
            var envVars = new Dictionary<string, string>()
            {
                { "NUGET_HTTP_CACHE_PATH", pathContext.HttpCacheFolder }
            };

            var args = new string[4 + additionalArguments.Length];
            args[0] = "restore";
            args[1] = pathContext.SolutionRoot;
            args[2] = "-Verbosity";
            args[3] = "detailed";
            for (int i = 0; i < additionalArguments.Length; i++)
            {
                args[4 + i] = additionalArguments[i];
            }

            // Act
            var r = CommandRunner.Run(
                nugetExe,
                pathContext.WorkingDirectory.Path,
                string.Join(" ", args),
                environmentVariables: envVars);

            // Assert
            Assert.True(expectedExitCode == r.ExitCode, r.AllOutput);

            return r;
        }

        [Fact]
        public async Task Restore_PackageSourceMapping_Succeed()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                var externalRepositoryPath = Path.Combine(pathContext.SolutionRoot, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var contosoRepositoryPath = Path.Combine(pathContext.SolutionRoot, "ContosoRepository");
                Directory.CreateDirectory(contosoRepositoryPath);

                var configPath = Path.Combine(pathContext.WorkingDirectory, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""ContosoRepository"" value=""{contosoRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""externalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""contosoRepository"">
            <package pattern=""Contoso.*"" />
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

                var ContosoReal = new SimpleTestPackageContext()
                {
                    Id = "Contoso.A",
                    Version = "1.0.0"
                };
                ContosoReal.AddFile("lib/net461/contosoA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    contosoRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ContosoReal);

                var ExternalA = new SimpleTestPackageContext()
                {
                    Id = "Contoso.A",  // Initial version had package id conflict with Contoso repository
                    Version = "1.0.0"
                };
                ExternalA.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA);

                var ExternalB = new SimpleTestPackageContext()
                {
                    Id = "External.B",  // name conflict resolved.
                    Version = "2.0.0"
                };
                ExternalB.AddFile("lib/net461/externalB.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalB);

                Util.CreateFile(Path.GetDirectoryName(projectA.ProjectPath), "packages.config",
@"<packages>
  <package id=""Contoso.A"" version=""1.0.0"" targetFramework=""net461"" />
  <package id=""External.B"" version=""2.0.0"" targetFramework=""net461"" />
</packages>");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var result = RunRestore(pathContext, _successExitCode);

                // Assert
                var contosoRestorePath = Path.Combine(projectAPackages, ContosoReal.ToString(), ContosoReal.ToString() + ".nupkg");
                using (var nupkgReader = new PackageArchiveReader(contosoRestorePath))
                {
                    var allFiles = nupkgReader.GetFiles().ToList();
                    // Assert correct Contoso package from Contoso repository was restored.
                    Assert.Contains("lib/net461/contosoA.dll", allFiles);
                }
                var externalRestorePath = Path.Combine(projectAPackages, ExternalB.ToString(), ExternalB.ToString() + ".nupkg");
                Assert.True(File.Exists(externalRestorePath));
            }
        }


        [Fact]
        public async Task Restore_PackageSourceMapping_Fails()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var net461 = NuGetFramework.Parse("net461");

                var projectA = SimpleTestProjectContext.CreateLegacyPackageReference(
                    "a",
                    pathContext.SolutionRoot,
                    net461);
                var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

                var externalRepositoryPath = Path.Combine(pathContext.SolutionRoot, "ExternalRepository");
                Directory.CreateDirectory(externalRepositoryPath);

                var contosoRepositoryPath = Path.Combine(pathContext.SolutionRoot, "ContosoRepository");
                Directory.CreateDirectory(contosoRepositoryPath);

                var configPath = Path.Combine(pathContext.WorkingDirectory, "nuget.config");
                SettingsTestUtils.CreateConfigurationFile(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""ExternalRepository"" value=""{externalRepositoryPath}"" />
    <add key=""ContosoRepository"" value=""{contosoRepositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
        <packageSource key=""ExternalRepository"">
            <package pattern=""External.*"" />
            <package pattern=""Others.*"" />
        </packageSource>
        <packageSource key=""ContosoRepository"">
            <package pattern=""Contoso.*"" />  <!--Contoso.A package doesn't exist Contoso repository, so restore should fail-->
            <package pattern=""Test.*"" />
        </packageSource>
    </packageSourceMapping>
</configuration>");

                var ExternalA = new SimpleTestPackageContext()
                {
                    Id = "Contoso.A",  // Initial version had package id conflict with Contoso repository
                    Version = "1.0.0"
                };
                ExternalA.AddFile("lib/net461/externalA.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalA);

                var ExternalB = new SimpleTestPackageContext()
                {
                    Id = "External.B",  // name conflict resolved.
                    Version = "2.0.0"
                };
                ExternalB.AddFile("lib/net461/externalB.dll");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    externalRepositoryPath,
                    PackageSaveMode.Defaultv3,
                    ExternalB);

                Util.CreateFile(Path.GetDirectoryName(projectA.ProjectPath), "packages.config",
@"<packages>
  <package id=""Contoso.A"" version=""1.0.0"" targetFramework=""net461"" />
  <package id=""External.B"" version=""2.0.0"" targetFramework=""net461"" />
</packages>");

                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                // Act
                var result = RunRestore(pathContext, _failureExitCode);

                // Assert
                Assert.Contains("Unable to find version '1.0.0' of package 'Contoso.A'", result.Errors);
            }
        }


        [Fact]
        public async Task Restore_WithHttpSource_Warns()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            // Set up solution, project, and packages
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var packageA = new SimpleTestPackageContext("a", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageA);
            pathContext.Settings.AddSource("http-feed", "http://api.source/index.json");
            pathContext.Settings.AddSource("https-feed", "https://api.source/index.json");

            var net461 = NuGetFramework.Parse("net461");
            var projectA = new SimpleTestProjectContext(
                "a",
                ProjectStyle.PackagesConfig,
                pathContext.SolutionRoot);
            projectA.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
            var projectAPackages = Path.Combine(pathContext.SolutionRoot, "packages");

            Util.CreateFile(Path.GetDirectoryName(projectA.ProjectPath), "packages.config",
@"<packages>
  <package id=""A"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

            solution.Projects.Add(projectA);
            solution.Create(pathContext.SolutionRoot);

            // Act
            var result = RunRestore(pathContext, _successExitCode);

            // Assert
            result.Success.Should().BeTrue();
            Assert.Contains($"Added package 'A.1.0.0' to folder '{projectAPackages}'", result.Output);
            Assert.Contains("You are running the 'restore' operation with an 'HTTP' source, 'http://api.source/index.json'. Non-HTTPS access will be removed in a future version. Consider migrating to an 'HTTPS' source.", result.Output);
        }

        [Theory]
        [InlineData("false", true)]
        [InlineData("FALSE", true)]
        [InlineData("invalidString", true)]
        [InlineData("", true)]
        [InlineData("true", false)]
        [InlineData("TRUE", false)]
        public async Task Restore_WithHttpSourceAndFalseAllowInsecureConnections_WarnsCorrectly(string allowInsecureConnections, bool hasHttpWarning)
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            // Set up solution, project, and packages
            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);
            var packageA = new SimpleTestPackageContext("a", "1.0.0");
            await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, packageA);
            pathContext.Settings.AddSource("http-feed", "http://api.source/index.json", allowInsecureConnections);
            pathContext.Settings.AddSource("https-feed", "https://api.source/index.json", allowInsecureConnections);

            var net461 = NuGetFramework.Parse("net461");
            var projectB = new SimpleTestProjectContext(
                "b",
                ProjectStyle.PackagesConfig,
                pathContext.SolutionRoot);
            projectB.Frameworks.Add(new SimpleTestProjectFrameworkContext(net461));
            var projectBPackages = Path.Combine(pathContext.SolutionRoot, "packages");

            Util.CreateFile(Path.GetDirectoryName(projectB.ProjectPath), "packages.config",
@"<packages>
  <package id=""A"" version=""1.0.0"" targetFramework=""net461"" />
</packages>");

            solution.Projects.Add(projectB);
            solution.Create(pathContext.SolutionRoot);

            // Act
            CommandRunnerResult result = RunRestore(pathContext, _successExitCode);

            // Assert
            string formatString = "You are running the 'restore' operation with an 'HTTP' source, '{0}'. Non-HTTPS access will be removed in a future version. Consider migrating to an 'HTTPS'";
            string warningForHttpSource = string.Format(formatString, "http://api.source/index.json");
            string warningForHttpsSource = string.Format(formatString, "https://api.source/index.json");

            result.Success.Should().BeTrue();
            Assert.Contains($"Added package 'A.1.0.0' to folder '{projectBPackages}'", result.Output);
            Assert.DoesNotContain(warningForHttpsSource, result.Output);
            if (hasHttpWarning)
            {
                Assert.Contains(warningForHttpSource, result.Output);
            }
            else
            {
                Assert.DoesNotContain(warningForHttpSource, result.Output);
            }
        }

        public static string GetResource(string name)
        {
            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(name)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

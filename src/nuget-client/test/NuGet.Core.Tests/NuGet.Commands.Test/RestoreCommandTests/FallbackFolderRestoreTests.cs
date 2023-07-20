// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test.RestoreCommandTests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class FallbackFolderRestoreTests
    {
        [Fact]
        public async Task FallbackFolderRestore_AllPackagesFoundInFallbackAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""netstandard1.3"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var fallbackFolder = new DirectoryInfo(Path.Combine(workingDir, "fallbackFolder"));
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(
                    spec1,
                    sources,
                    packagesDir.FullName,
                    new List<string>() { fallbackFolder.FullName },
                    logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageBContext = new SimpleTestPackageContext()
                {
                    Id = "packageB",
                    Version = "1.0.0"
                };

                var packageAContext = new SimpleTestPackageContext()
                {
                    Id = "packageA",
                    Version = "1.0.0"
                };

                packageAContext.Dependencies.Add(packageBContext);

                var saveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files | PackageSaveMode.Nupkg;

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    fallbackFolder.FullName,
                    saveMode,
                    packageAContext,
                    packageBContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var lockFormat = new LockFileFormat();
                var fromDisk = lockFormat.Read(result.LockFilePath);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(2, lockFile.Libraries.Count);
                Assert.Equal(0, result.GetAllInstalled().Count);
                Assert.False(Directory.Exists(packagesDir.FullName));

                Assert.Equal(2, lockFile.PackageFolders.Count);
                Assert.Equal(packagesDir.FullName, lockFile.PackageFolders[0].Path);
                Assert.Equal(fallbackFolder.FullName, lockFile.PackageFolders[1].Path);

                // Verify folders are round tripped
                Assert.Equal(2, fromDisk.PackageFolders.Count);
                Assert.Equal(packagesDir.FullName, fromDisk.PackageFolders[0].Path);
                Assert.Equal(fallbackFolder.FullName, fromDisk.PackageFolders[1].Path);
            }
        }

        [Fact]
        public async Task FallbackFolderRestore_AllPackagesFoundInFallback_NuspecModeAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""netstandard1.3"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var fallbackFolder = new DirectoryInfo(Path.Combine(workingDir, "fallbackFolder"));
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(
                    spec1,
                    sources,
                    packagesDir.FullName,
                    new List<string>() { fallbackFolder.FullName },
                    logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageBContext = new SimpleTestPackageContext()
                {
                    Id = "packageB",
                    Version = "1.0.0"
                };

                var packageAContext = new SimpleTestPackageContext()
                {
                    Id = "packageA",
                    Version = "1.0.0"
                };

                packageAContext.Dependencies.Add(packageBContext);

                var saveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files;

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    fallbackFolder.FullName,
                    saveMode,
                    packageAContext,
                    packageBContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(2, lockFile.Libraries.Count);
                Assert.Equal(0, result.GetAllInstalled().Count);
                Assert.False(Directory.Exists(packagesDir.FullName));
                Assert.Equal(2, lockFile.PackageFolders.Count);
                Assert.Equal(packagesDir.FullName, lockFile.PackageFolders[0].Path);
                Assert.Equal(fallbackFolder.FullName, lockFile.PackageFolders[1].Path);
            }
        }

        [Fact]
        public async Task FallbackFolderRestore_SinglePackageFoundInFallbackAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""netstandard1.3"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0"",
                        ""packageB"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var fallbackFolder = new DirectoryInfo(Path.Combine(workingDir, "fallbackFolder"));
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(
                    spec1,
                    sources,
                    packagesDir.FullName,
                    new List<string>() { fallbackFolder.FullName },
                    logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageBContext = new SimpleTestPackageContext()
                {
                    Id = "packageB",
                    Version = "1.0.0"
                };

                var packageAContext = new SimpleTestPackageContext()
                {
                    Id = "packageA",
                    Version = "1.0.0"
                };

                var saveMode = PackageSaveMode.Nuspec | PackageSaveMode.Files | PackageSaveMode.Nupkg;

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    fallbackFolder.FullName,
                    saveMode,
                    packageAContext);

                await SimpleTestPackageUtility.CreatePackagesAsync(
                    packageSource.FullName,
                    packageBContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(2, lockFile.Libraries.Count);
                Assert.Equal(1, result.GetAllInstalled().Count);
                Assert.Equal("package/packageB 1.0.0", result.GetAllInstalled().Single().ToString());
                Assert.Equal("packageb", Path.GetFileName(Directory.GetDirectories(packagesDir.FullName).Single()));
                Assert.Equal(2, lockFile.PackageFolders.Count);
                Assert.Equal(packagesDir.FullName, lockFile.PackageFolders[0].Path);
                Assert.Equal(fallbackFolder.FullName, lockFile.PackageFolders[1].Path);
            }
        }

        [Fact]
        public async Task FallbackFolderRestore_NoPackagesFoundInFallbackAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""netstandard1.3"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0"",
                        ""packageB"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var fallbackFolder = new DirectoryInfo(Path.Combine(workingDir, "fallbackFolder"));
                var fallbackFolder2 = new DirectoryInfo(Path.Combine(workingDir, "fallbackFolder2"));
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packageSource.Create();
                project1.Create();
                fallbackFolder.Create();
                fallbackFolder2.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(
                    spec1,
                    sources,
                    packagesDir.FullName,
                    new List<string>() { fallbackFolder.FullName, fallbackFolder2.FullName },
                    logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageBContext = new SimpleTestPackageContext()
                {
                    Id = "packageB",
                    Version = "1.0.0"
                };

                var packageAContext = new SimpleTestPackageContext()
                {
                    Id = "packageA",
                    Version = "1.0.0"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(
                    packageSource.FullName,
                    packageAContext,
                    packageBContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(2, lockFile.Libraries.Count);
                Assert.Equal(2, result.GetAllInstalled().Count);
                Assert.Equal(2, Directory.GetDirectories(packagesDir.FullName).Length);
                Assert.Equal(3, lockFile.PackageFolders.Count);
                Assert.Equal(packagesDir.FullName, lockFile.PackageFolders[0].Path);
                Assert.Equal(fallbackFolder.FullName, lockFile.PackageFolders[1].Path);
                Assert.Equal(fallbackFolder2.FullName, lockFile.PackageFolders[2].Path);
            }
        }

        [Fact]
        public async Task FallbackFolderRestore_VerifyMissingFallbackFolderFailsAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""netstandard1.3"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0"",
                        ""packageB"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var fallbackFolder = new DirectoryInfo(Path.Combine(workingDir, "fallbackFolder"));
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(
                    spec1,
                    sources,
                    packagesDir.FullName,
                    new List<string>() { fallbackFolder.FullName },
                    logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageBContext = new SimpleTestPackageContext()
                {
                    Id = "packageB",
                    Version = "1.0.0"
                };

                var packageAContext = new SimpleTestPackageContext()
                {
                    Id = "packageA",
                    Version = "1.0.0"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(
                    packageSource.FullName,
                    packageAContext,
                    packageBContext);

                // Act & Assert
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                result.Success.Should().BeFalse();
                result.LockFile.LogMessages.Select(e => e.Code).Should().AllBeEquivalentTo(NuGetLogCode.NU1301);
            }
        }
    }
}

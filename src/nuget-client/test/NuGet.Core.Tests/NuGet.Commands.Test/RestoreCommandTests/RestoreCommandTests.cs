// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Test;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Test.Utility.Commands;
using Test.Utility.ProjectManagement;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Commands.Test.RestoreCommandTests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class RestoreCommandTests
    {
        private static SignedPackageVerifierSettings DefaultSettings = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);

        [Fact]
        public async Task RestoreCommand_VerifyRuntimeSpecificAssetsAreNotIncludedForCompile_RuntimeOnlyAsync()
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
                ""netstandard1.5"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              },
              ""runtimes"": {
                ""win7-x64"": {}
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("runtimes/win7-x64/lib/netstandard1.5/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.5"), "win7-x64")
                    .Libraries
                    .Single(library => library.Name == "packageA");

                // Assert
                Assert.True(result.Success);
                Assert.Equal(0, targetLib.CompileTimeAssemblies.Count);
                Assert.Equal("runtimes/win7-x64/lib/netstandard1.5/a.dll", targetLib.RuntimeAssemblies.Single());
            }
        }

        [Fact]
        public async Task RestoreCommand_VerifyRuntimeSpecificAssetsAreNotIncludedForCompile_RuntimeAndRefAsync()
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
                ""netstandard1.5"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              },
              ""runtimes"": {
                ""win7-x64"": {}
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("ref/netstandard1.5/a.dll");
                packageAContext.AddFile("runtimes/win7-x64/lib/netstandard1.5/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.5"), "win7-x64")
                    .Libraries
                    .Single(library => library.Name == "packageA");

                // Assert
                Assert.True(result.Success);
                Assert.Equal("ref/netstandard1.5/a.dll", targetLib.CompileTimeAssemblies.Single());
                Assert.Equal("runtimes/win7-x64/lib/netstandard1.5/a.dll", targetLib.RuntimeAssemblies.Single());
            }
        }

        [Fact]
        public async Task RestoreCommand_CompileAssetsWithBothRefAndLib_VerifyRefWinsAsync()
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
                ""netstandard1.5"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              },
              ""runtimes"": {
                ""win7-x64"": {}
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("ref/netstandard1.5/a.dll");
                packageAContext.AddFile("lib/netstandard1.5/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.5"), "win7-x64")
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.OrderBy(s => s).ToArray();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1, compile.Length);
                Assert.Equal("ref/netstandard1.5/a.dll", compile[0]);
                Assert.Equal("lib/netstandard1.5/a.dll", targetLib.RuntimeAssemblies.Single());
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RestoreCommand_ObservesLowercaseFlagAsync(bool isLowercase)
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var sourceDir = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var projectDir = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                sourceDir.Create();
                projectDir.Create();

                var resolver = new VersionFolderPathResolver(packagesDir.FullName, isLowercase);

                var sources = new List<string>
                {
                    sourceDir.FullName
                };

                var projectJson = @"
                {
                  ""frameworks"": {
                    ""netstandard1.0"": {
                      ""dependencies"": {
                        ""PackageA"": ""1.0.0-Beta""
                      }
                    }
                  }
                }";

                File.WriteAllText(Path.Combine(projectDir.FullName, "project.json"), projectJson);

                var specPath = Path.Combine(projectDir.FullName, "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(projectJson, "project1", specPath).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(
                    spec,
                    sources.Select(x => Repository.Factory.GetCoreV3(x)),
                    packagesDir.FullName,
                    Enumerable.Empty<string>(),
                    logger)
                {
                    IsLowercasePackagesDirectory = isLowercase
                };
                request.LockFilePath = Path.Combine(projectDir.FullName, "project.lock.json");

                var packageId = "PackageA";
                var packageVersion = "1.0.0-Beta";
                var packageAContext = new SimpleTestPackageContext(packageId, packageVersion);
                packageAContext.AddFile("lib/netstandard1.0/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(sourceDir.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;

                // Assert
                Assert.True(result.Success);

                var library = lockFile
                    .Libraries
                    .FirstOrDefault(l => l.Name == packageId && l.Version.ToNormalizedString() == packageVersion);

                Assert.NotNull(library);
                Assert.Equal(
                    PathUtility.GetPathWithForwardSlashes(resolver.GetPackageDirectory(packageId, library.Version)),
                    library.Path);
                Assert.True(File.Exists(resolver.GetPackageFilePath(packageId, library.Version)));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RestoreCommand_WhenSwitchingBetweenLowercaseSettings_LockFileAlwaysRespectsLatestSettingAsync(bool isLowercase)
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var sourceDir = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var projectDir = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                sourceDir.Create();
                projectDir.Create();

                var resolverA = new VersionFolderPathResolver(packagesDir.FullName, !isLowercase);
                var resolverB = new VersionFolderPathResolver(packagesDir.FullName, isLowercase);

                var sources = new List<string>
                {
                    sourceDir.FullName
                };

                var projectJson = @"
                {
                  ""frameworks"": {
                    ""netstandard1.0"": {
                      ""dependencies"": {
                        ""PackageA"": ""1.0.0-Beta""
                      }
                    }
                  }
                }";

                File.WriteAllText(Path.Combine(projectDir.FullName, "project.json"), projectJson);

                var specPath = Path.Combine(projectDir.FullName, "project.json");
                var spec = JsonPackageSpecReader.GetPackageSpec(projectJson, "project1", specPath).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var lockFilePath = Path.Combine(projectDir.FullName, "project.lock.json");
                var lockFileFormat = new LockFileFormat();

                var packageId = "PackageA";
                var packageVersion = "1.0.0-Beta";
                var packageAContext = new SimpleTestPackageContext(packageId, packageVersion);
                packageAContext.AddFile("lib/netstandard1.0/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(sourceDir.FullName, packageAContext);

                // Act
                // Execute the first restore with the opposite lowercase setting.
                var requestA = new TestRestoreRequest(
                    spec,
                    sources.Select(x => Repository.Factory.GetCoreV3(x)),
                    packagesDir.FullName,
                    Enumerable.Empty<string>(),
                    logger)
                {
                    LockFilePath = lockFilePath,
                    IsLowercasePackagesDirectory = !isLowercase,
                    AllowNoOp = false,
                };
                var commandA = new RestoreCommand(requestA);
                var resultA = await commandA.ExecuteAsync();
                await resultA.CommitAsync(logger, CancellationToken.None);

                // Execute the second restore with the request lowercase setting.
                var requestB = new TestRestoreRequest(
                    spec,
                    sources.Select(x => Repository.Factory.GetCoreV3(x)),
                    packagesDir.FullName,
                    Enumerable.Empty<string>(),
                    logger)
                {
                    LockFilePath = lockFilePath,
                    IsLowercasePackagesDirectory = isLowercase,
                    ExistingLockFile = lockFileFormat.Read(lockFilePath),
                    AllowNoOp = false,
                };
                var commandB = new RestoreCommand(requestB);
                var resultB = await commandB.ExecuteAsync();
                await resultB.CommitAsync(logger, CancellationToken.None);

                // Assert
                // Commands should have succeeded.
                Assert.True(resultA.Success);
                Assert.True(resultB.Success);

                // The lock file library path should match the requested case.
                var libraryA = resultA
                    .LockFile
                    .Libraries
                    .FirstOrDefault(l => l.Name == packageId && l.Version.ToNormalizedString() == packageVersion);
                Assert.NotNull(libraryA);
                Assert.Equal(
                    PathUtility.GetPathWithForwardSlashes(resolverA.GetPackageDirectory(packageId, libraryA.Version)),
                    libraryA.Path);
                Assert.True(File.Exists(resolverA.GetPackageFilePath(packageId, libraryA.Version)));

                var libraryB = resultB
                    .LockFile
                    .Libraries
                    .FirstOrDefault(l => l.Name == packageId && l.Version.ToNormalizedString() == packageVersion);
                Assert.NotNull(libraryB);
                Assert.Equal(
                    PathUtility.GetPathWithForwardSlashes(resolverB.GetPackageDirectory(packageId, libraryB.Version)),
                    libraryB.Path);
                Assert.True(File.Exists(resolverB.GetPackageFilePath(packageId, libraryB.Version)));

                // The lock file on disk should match the second restore's library.
                var diskLockFile = lockFileFormat.Read(lockFilePath);
                var lockFileLibrary = diskLockFile
                    .Libraries
                    .FirstOrDefault(l => l.Name == packageId && l.Version.ToNormalizedString() == packageVersion);
                Assert.NotNull(lockFileLibrary);
                Assert.Equal(
                    PathUtility.GetPathWithForwardSlashes(resolverB.GetPackageDirectory(packageId, libraryB.Version)),
                    libraryB.Path);
                Assert.Equal(libraryB, lockFileLibrary);
            }
        }

        [Fact]
        public async Task RestoreCommand_FileUriV3FolderAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            // Both TxMs reference packageA, but they are different types.
            // Verify that the reference does not show up under libraries.
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
                        ""packageA"": ""4.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(UriUtility.CreateSourceUri(packageSource.FullName).AbsoluteUri));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    packageSource.FullName,
                    new PackageIdentity("packageA", NuGetVersion.Parse("4.0.0")));

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1, lockFile.Libraries.Count);
            }
        }

        [Fact]
        public async Task RestoreCommand_FileUriV2FolderAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            // Both TxMs reference packageA, but they are different types.
            // Verify that the reference does not show up under libraries.
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
                        ""packageA"": ""4.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(UriUtility.CreateSourceUri(packageSource.FullName).AbsoluteUri));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "4.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1, lockFile.Libraries.Count);
            }
        }

        [Fact]
        public async Task RestoreCommand_FindInV2FolderWithDifferentCasingAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            // Both TxMs reference packageA, but they are different types.
            // Verify that the reference does not show up under libraries.
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
                        ""PACKAGEA"": ""4.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "4.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1, lockFile.Libraries.Count);
            }
        }

        [Fact]
        public async Task RestoreCommand_ReferenceWithSameNameDifferentCasingAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            // Both TxMs reference packageA, but they are different types.
            // Verify that the reference does not show up under libraries.
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
                        ""packageA"": ""4.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "PROJECT1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "PROJECT1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var aContext = new SimpleTestPackageContext()
                {
                    Id = "packageA",
                    Version = "4.0.0"
                };

                aContext.Dependencies.Add(new SimpleTestPackageContext("proJect1"));

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "projeCt1", "4.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                // Verify no stack overflows from circular dependencies
                Assert.False(result.Success);
            }
        }

        [Fact]
        public async Task RestoreCommand_ImportsWithHigherVersion_NoFallbackAsync()
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
                    ""imports"": [ ""netstandard1.5"" ],
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("lib/netstandard1.0/a.dll");
                packageAContext.AddFile("lib/netstandard1.5/a.dll");
                packageAContext.AddFile("lib/net46/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.3"), null)
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("lib/netstandard1.0/a.dll", compile.Path);
            }
        }

        [Fact]
        public async Task RestoreCommand_ImportsWithHigherVersion_FallbackAsync()
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
                    ""imports"": [ ""netstandard1.5"" ],
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("lib/netstandard1.5/a.dll");
                packageAContext.AddFile("lib/netstandard1.6/a.dll");
                packageAContext.AddFile("lib/net46/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.3"), null)
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("lib/netstandard1.5/a.dll", compile.Path);
            }
        }

        [Fact]
        public async Task RestoreCommand_ImportsWithHigherVersion_MultiFallbackAsync()
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
                ""netstandard1.0"": {
                    ""imports"": [ ""netstandard1.1"", ""netstandard1.2"", ""dnxcore50""  ],
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("lib/netstandard1.2/a.dll");
                packageAContext.AddFile("lib/netstandard1.5/a.dll");
                packageAContext.AddFile("lib/net46/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.0"), null)
                    .Libraries
                    .Single(library => library.Name == "packageA");

                var compile = targetLib.CompileTimeAssemblies.Single();

                // Assert
                Assert.True(result.Success);
                Assert.Equal("lib/netstandard1.2/a.dll", compile.Path);
            }
        }

        [Fact]
        public async Task RestoreCommand_ImportsNoMatchAsync()
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
                ""netstandard1.0"": {
                    ""imports"": [ ""netstandard1.1"", ""netstandard1.2""  ],
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("lib/netstandard1.5/a.dll");
                packageAContext.AddFile("lib/netstandard1.6/a.dll");
                packageAContext.AddFile("lib/net46/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                var targetLib = lockFile.GetTarget(NuGetFramework.Parse("netstandard1.0"), null)
                                    .Libraries
                                    .Single(library => library.Name == "packageA");

                var compileItems = targetLib.CompileTimeAssemblies;

                // Assert
                Assert.False(result.Success);
                Assert.Equal(0, compileItems.Count);
            }
        }

#if IS_SIGNING_SUPPORTED
        [PlatformFact(Platform.Windows)]
        public async Task RestoreCommand_InvalidSignedPackageAsync_FailsAsync()
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
                ""net46"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, DefaultSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(NullSettings.Instance, logger);
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, clientPolicyContext, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json"),
                    SignedPackageVerifier = signedPackageVerifier.Object
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("lib/net46/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();

                // Assert
                Assert.False(result.Success);
            }
        }

        [PlatformFact(Platform.Darwin)]
        public async Task RestoreCommand_InvalidSignedPackageAsync_SuccessAsync()
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
                ""net46"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                PackageSpec spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, DefaultSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: false, isSigned: true));

                ClientPolicyContext clientPolicyContext = ClientPolicyContext.GetClientPolicy(NullSettings.Instance, logger);
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, clientPolicyContext, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json"),
                    SignedPackageVerifier = signedPackageVerifier.Object
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("lib/net46/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                RestoreResult result = await command.ExecuteAsync();

                // Assert
                Assert.True(result.Success);
            }
        }

        [Fact]
        public async Task RestoreCommand_SignedPackageAsync()
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
                ""net46"": {
                    ""dependencies"": {
                        ""packageA"": ""1.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();

                var signedPackageVerifier = new Mock<IPackageSignatureVerifier>(MockBehavior.Strict);

                signedPackageVerifier.Setup(x => x.VerifySignaturesAsync(
                    It.IsAny<ISignedPackageReader>(),
                    It.Is<SignedPackageVerifierSettings>(s => SigningTestUtility.AreVerifierSettingsEqual(s, DefaultSettings)),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Guid>())).
                    ReturnsAsync(new VerifySignaturesResult(isValid: true, isSigned: true));

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(NullSettings.Instance, logger);
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, clientPolicyContext, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json"),
                    SignedPackageVerifier = signedPackageVerifier.Object
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("lib/net46/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();

                // Assert
                Assert.True(result.Success);
            }
        }
#endif

        [Fact]
        public async Task RestoreCommand_PathInPackageLibraryAsync()
        {
            // Arrange
            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();

                var sources = new List<PackageSource>
                {
                    new PackageSource(packageSource.FullName)
                };

                var project1Json = @"
                {
                  ""frameworks"": {
                    ""netstandard1.0"": {
                      ""dependencies"": {
                        ""packageA"": ""1.0.0""
                      }
                    }
                  }
                }";

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                var packageAContext = new SimpleTestPackageContext("packageA");
                packageAContext.AddFile("lib/netstandard1.0/a.dll");

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, packageAContext);

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;

                // Assert
                Assert.True(result.Success);
                var library = lockFile.Libraries.FirstOrDefault(l => l.Name == "packageA");
                Assert.NotNull(library);
                Assert.Equal("packagea/1.0.0", library.Path);
            }
        }

        [Fact]
        public async Task RestoreCommand_PackageWithSameNameAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            var project1Json = @"
            {
              ""version"": ""1.0.0-*"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""dependencies"": {
                ""project1"": { ""version"": ""1.0.0"", ""target"": ""package"" }
              },
              ""frameworks"": {
                ""net45"": {
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "project1", "1.0.0");

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.False(result.Success);
                Assert.True(logger.ErrorMessages.Any(s => s.Contains("Cycle detected")));
            }
        }

        [Fact]
        public async Task RestoreCommand_PackageAndReferenceWithSameNameAndVersionAsync()
        {
            // Arrange
            var sources = new List<PackageSource>();

            // Both TxMs reference packageA, but they are different types.
            // Verify that the reference does not show up under libraries.
            var project1Json = @"
            {
              ""version"": ""1.0.0"",
              ""description"": """",
              ""authors"": [ ""author"" ],
              ""tags"": [ """" ],
              ""projectUrl"": """",
              ""licenseUrl"": """",
              ""frameworks"": {
                ""net45"": {
                    ""frameworkAssemblies"": {
                         ""packageA"": ""4.0.0""
                    }
                },
                ""netstandard1.3"": {
                    ""dependencies"": {
                        ""packageA"": ""4.0.0""
                    }
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                await SimpleTestPackageUtility.CreateFullPackageAsync(packageSource.FullName, "packageA", "4.0.0");

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1, lockFile.Libraries.Count);
            }
        }

        [Fact]
        public async Task RestoreCommand_RestoreProjectWithNoDependenciesAsync()
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
                ""net45"": {
                }
              }
            }";

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                File.WriteAllText(Path.Combine(project1.FullName, "project.json"), project1Json);

                var specPath1 = Path.Combine(project1.FullName, "project.json");
                var spec1 = JsonPackageSpecReader.GetPackageSpec(project1Json, "project1", specPath1).EnsureProjectJsonRestoreMetadata();

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    LockFilePath = Path.Combine(project1.FullName, "project.lock.json")
                };

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(0, lockFile.Libraries.Count);
            }
        }

        [Fact]
        public async Task RestoreCommand_MinimalProjectWithAdditionalMessages_WritesAssetsFileWithMessages()
        {
            // Arrange
            var sources = new List<PackageSource>();

            using (var workingDir = TestDirectory.Create())
            {
                var packagesDir = new DirectoryInfo(Path.Combine(workingDir, "globalPackages"));
                var packageSource = new DirectoryInfo(Path.Combine(workingDir, "packageSource"));
                var project1 = new DirectoryInfo(Path.Combine(workingDir, "projects", "project1"));
                var project1Obj = new DirectoryInfo(Path.Combine(project1.FullName, "obj"));
                packagesDir.Create();
                packageSource.Create();
                project1.Create();
                sources.Add(new PackageSource(packageSource.FullName));

                var dgspec1 = DependencyGraphSpecTestUtilities.CreateMinimalDependencyGraphSpec(Path.Combine(project1.FullName, "project1.csproj"), project1Obj.FullName);
                var spec1 = dgspec1.Projects[0];

                var logger = new TestLogger();
                var request = new TestRestoreRequest(spec1, sources, packagesDir.FullName, logger)
                {
                    AdditionalMessages = new List<IAssetsLogMessage>()
                    {
                        new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1105, "Test error")
                    }
                };

                // Act
                var command = new RestoreCommand(request);
                var result = await command.ExecuteAsync();
                var lockFile = result.LockFile;
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.False(result.Success);
                Assert.Equal(1, lockFile.LogMessages.Count);
            }
        }

        [Fact]
        public async Task RestoreCommand_CentralVersion_ErrorWhenDependenciesHaveVersion()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var outputPath = Path.Combine(projectPath, "obj");
                var dependencyBar = new LibraryDependency(new LibraryRange("bar", VersionRange.Parse("3.0.0"), LibraryDependencyTarget.All),
                        LibraryIncludeFlags.All,
                        LibraryIncludeFlags.All,
                        new List<NuGetLogCode>(),
                        autoReferenced: false,
                        generatePathProperty: true,
                        versionCentrallyManaged: false,
                        LibraryDependencyReferenceType.Direct,
                        aliases: null,
                        versionOverride: null);

                var centralVersionFoo = new CentralPackageVersion("foo", VersionRange.Parse("1.0.0"));
                var centralVersionBar = new CentralPackageVersion("bar", VersionRange.Parse("2.0.0"));

                var tfi = CreateTargetFrameworkInformation(new List<LibraryDependency>() { dependencyBar }, new List<CentralPackageVersion>() { centralVersionFoo, centralVersionBar });
                var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>() { tfi });
                packageSpec.RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectUniqueName = projectName,
                    CentralPackageVersionsEnabled = true,
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                };
                packageSpec.FilePath = projectPath;

                var sources = new List<PackageSource>();
                var logger = new TestLogger();

                var request = new TestRestoreRequest(packageSpec, sources, "", logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var restoreCommand = new RestoreCommand(request);

                var result = await restoreCommand.ExecuteAsync();

                // Assert
                Assert.False(result.Success);
                Assert.Equal(1, logger.ErrorMessages.Count);
                logger.ErrorMessages.TryDequeue(out var errorMessage);
                Assert.True(errorMessage.Contains("Projects that use central package version management should not define the version on the PackageReference items but on the PackageVersion"));
                Assert.True(errorMessage.Contains("bar"));
                var NU1801Messages = result.LockFile.LogMessages.Where(m => m.Code == NuGetLogCode.NU1008);
                Assert.Equal(1, NU1801Messages.Count());
            }
        }

        [Theory]
        [InlineData("bar")]
        [InlineData("Bar")]
        public async Task RestoreCommand_CentralVersion_ErrorWhenCentralPackageVersionFileContainsAutoReferencedReferences(string autoreferencedpackageId)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var outputPath = Path.Combine(projectPath, "obj");
                var dependencyBar = new LibraryDependency(
                    new LibraryRange(
                        autoreferencedpackageId,
                        VersionRange.Parse("3.0.0"),
                        LibraryDependencyTarget.All),
                    LibraryIncludeFlags.All,
                    LibraryIncludeFlags.All,
                    new List<NuGetLogCode>(),
                    autoReferenced: true,
                    generatePathProperty: true,
                    versionCentrallyManaged: false,
                    LibraryDependencyReferenceType.Direct,
                    aliases: null,
                    versionOverride: null);

                var centralVersionFoo = new CentralPackageVersion("foo", VersionRange.Parse("1.0.0"));
                var centralVersionBar = new CentralPackageVersion(autoreferencedpackageId.ToLowerInvariant(), VersionRange.Parse("2.0.0"));

                var tfi = CreateTargetFrameworkInformation(new List<LibraryDependency>() { dependencyBar }, new List<CentralPackageVersion>() { centralVersionFoo, centralVersionBar });
                var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>() { tfi });
                packageSpec.RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectUniqueName = projectName,
                    CentralPackageVersionsEnabled = true,
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                };
                packageSpec.FilePath = projectPath;

                var sources = new List<PackageSource>();
                var logger = new TestLogger();

                var request = new TestRestoreRequest(packageSpec, sources, "", logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var restoreCommand = new RestoreCommand(request);

                var result = await restoreCommand.ExecuteAsync();

                // Assert
                Assert.False(result.Success);
                Assert.Equal(1, logger.ErrorMessages.Count);
                logger.ErrorMessages.TryDequeue(out var errorMessage);
                Assert.True(errorMessage.Contains("You do not typically need to reference them from your project or in your central package versions management file. For more information, see https://aka.ms/sdkimplicitrefs"));
                Assert.True(errorMessage.Contains(autoreferencedpackageId));
                var NU1009Messages = result.LockFile.LogMessages.Where(m => m.Code == NuGetLogCode.NU1009);
                Assert.Equal(1, NU1009Messages.Count());
            }
        }

        [Fact]
        public async Task RestoreCommand_CentralVersion_NoWarningWhenOnlyOneFeedAndPackageSourceMappingNotUsed()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext("foo", "1.0.0"));

                using var context = new SourceCacheContext();

                var packageSources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource),
                    new PackageSource("https://feed1"),
                };

                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var outputPath = Path.Combine(projectPath, "obj");
                var dependencyBar = new LibraryDependency(
                    new LibraryRange(
                        "foo",
                        null,
                        LibraryDependencyTarget.All),
                    LibraryIncludeFlags.All,
                    LibraryIncludeFlags.All,
                    new List<NuGetLogCode>(),
                    autoReferenced: false,
                    generatePathProperty: false,
                    versionCentrallyManaged: false,
                    LibraryDependencyReferenceType.Direct,
                    aliases: null,
                    versionOverride: null);

                var centralVersionFoo = new CentralPackageVersion("foo", VersionRange.Parse("1.0.0"));

                var tfi = CreateTargetFrameworkInformation(new List<LibraryDependency>() { dependencyBar }, new List<CentralPackageVersion>() { centralVersionFoo });
                var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>() { tfi })
                {
                    FilePath = projectPath,
                    Name = projectName,
                    RestoreMetadata = new ProjectRestoreMetadata()
                    {
                        ProjectName = projectName,
                        ProjectUniqueName = projectName,
                        CentralPackageVersionsEnabled = true,
                        ProjectStyle = ProjectStyle.PackageReference,
                        OutputPath = outputPath,
                        Sources = packageSources
                    }
                };

                var logger = new TestLogger();

                PackageSourceMapping packageSourceMappingConfiguration = PackageSourceMapping.GetPackageSourceMapping(NullSettings.Instance);

                var request = new TestRestoreRequest(packageSpec, packageSources, packagesDirectory: "", cacheContext: context, packageSourceMappingConfiguration: packageSourceMappingConfiguration, logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference,

                };

                var restoreCommand = new RestoreCommand(request);

                var result = await restoreCommand.ExecuteAsync();

                // Assert
                Assert.True(result.Success);

                logger.Errors.Should().Be(0);

                logger.WarningMessages.Should().NotContain(i => i.Contains(NuGetLogCode.NU1507.ToString()));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RestoreCommand_CentralVersion_WarningWhenMoreThanOneFeedAndPackageSourceMappingNotUsed(bool enablePackageSourceMapping)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext("foo", "1.0.0"));

                using var context = new SourceCacheContext();

                var packageSources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource),
                    new PackageSource("https://feed1"),
                    new PackageSource("https://feed2"),
                };

                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var outputPath = Path.Combine(projectPath, "obj");
                var dependencyBar = new LibraryDependency(
                    new LibraryRange(
                        "foo",
                        null,
                        LibraryDependencyTarget.All),
                    LibraryIncludeFlags.All,
                    LibraryIncludeFlags.All,
                    new List<NuGetLogCode>(),
                    autoReferenced: false,
                    generatePathProperty: false,
                    versionCentrallyManaged: false,
                    LibraryDependencyReferenceType.Direct,
                    aliases: null,
                    versionOverride: null);

                var centralVersionFoo = new CentralPackageVersion("foo", VersionRange.Parse("1.0.0"));

                var tfi = CreateTargetFrameworkInformation(new List<LibraryDependency>() { dependencyBar }, new List<CentralPackageVersion>() { centralVersionFoo });
                var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>() { tfi })
                {
                    FilePath = projectPath,
                    Name = projectName,
                    RestoreMetadata = new ProjectRestoreMetadata()
                    {
                        ProjectName = projectName,
                        ProjectUniqueName = projectName,
                        CentralPackageVersionsEnabled = true,
                        ProjectStyle = ProjectStyle.PackageReference,
                        OutputPath = outputPath,
                        Sources = packageSources
                    }
                };

                var logger = new TestLogger();

                PackageSourceMapping packageSourceMappingConfiguration = enablePackageSourceMapping
                    ? new PackageSourceMapping(new Dictionary<string, IReadOnlyList<string>>
                    {
                        [pathContext.PackageSource] = new List<string> { "foo" },
                        ["https://feed1"] = new List<string> { "bar" },
                        ["https://feed2"] = new List<string> { "baz" },
                    })
                    : PackageSourceMapping.GetPackageSourceMapping(NullSettings.Instance);

                var request = new TestRestoreRequest(packageSpec, packageSources, packagesDirectory: "", cacheContext: context, packageSourceMappingConfiguration: packageSourceMappingConfiguration, logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference,

                };

                var restoreCommand = new RestoreCommand(request);

                var result = await restoreCommand.ExecuteAsync();

                // Assert
                Assert.True(result.Success);

                logger.Errors.Should().Be(0);

                if (enablePackageSourceMapping)
                {
                    logger.WarningMessages.Should().NotContain(i => i.Contains(NuGetLogCode.NU1507.ToString()));
                }
                else
                {
                    // NU1507: There are 3 package sources defined in your configuration. When using central package management, please map your package sources with package source mapping (https://aka.ms/nuget-package-source-mapping) or specify a single package source. The following sources are defined: D:\NuGet\.test\work\298ed94f\653dd6db\source, https://feed1, https://feed2
                    logger.WarningMessages.Should()
                        .Contain(i => i.Contains(NuGetLogCode.NU1507.ToString()))
                        .Which.Should().Contain("There are 2 package sources defined in your configuration")
                        .And.Contain($"The following sources are defined: https://feed1, https://feed2");
                }
            }
        }

        [Fact]
        public async Task RestoreCommand_LogDowngradeWarningsOrErrorsAsync_ErrorWhenCpvmEnabled()
        {
            // Arrange
            // create graph with a downgrade
            var centralPackageName = "D";
            var centralPackageVersion = "2.0.0";
            var otherVersion = "3.0.0";
            NuGetFramework framework = NuGetFramework.Parse("net45");
            var logger = new TestLogger();

            var context = new TestRemoteWalkContext();
            var provider = new DependencyProvider();
            // D is a transitive dependency for package A through package B -> C -> D
            // D is defined as a Central Package Version
            // In this context Package D with version centralPackageVersion will be added as inner node of Node A, next to B

            // Input
            // A -> B (version = 3.0.0) -> C (version = 3.0.0) -> D (version = 3.0.0)
            // A ~> D (version = 2.0.0
            //         the dependency is not direct,
            //         it simulates the fact that there is a centrally defined "D" package
            //         the information is added to the provider)

            // The expected output graph
            //    -> B (version = 3.0.0) -> C (version = 3.0.0)
            // A
            //    -> D (version = 2.0.0)
            provider.Package("A", otherVersion)
                    .DependsOn("B", otherVersion)
                    .DependsOn(centralPackageName, centralPackageVersion, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            provider.Package("B", otherVersion)
                   .DependsOn("C", otherVersion);

            provider.Package("C", otherVersion)
                  .DependsOn(centralPackageName, otherVersion);

            // Simulates the existence of a D centrally defined package that is not direct dependency
            provider.Package("A", otherVersion)
                     .DependsOn(centralPackageName, centralPackageVersion, LibraryDependencyTarget.Package, versionCentrallyManaged: true, libraryDependencyReferenceType: LibraryDependencyReferenceType.None);

            // Add central package to the source with multiple versions
            provider.Package(centralPackageName, "1.0.0");
            provider.Package(centralPackageName, centralPackageVersion);
            provider.Package(centralPackageName, "3.0.0");

            context.LocalLibraryProviders.Add(provider);
            var walker = new RemoteDependencyWalker(context);

            // Act
            var rootNode = await DoWalkAsync(walker, "A", framework);
            RestoreTargetGraph restoreTargetGraph = RestoreTargetGraph.Create(new List<GraphNode<RemoteResolveResult>>() { rootNode }, context, logger, framework);

            await RestoreCommand.LogDowngradeWarningsOrErrorsAsync(new List<RestoreTargetGraph>() { restoreTargetGraph }, logger);

            // Assert
            Assert.Equal(1, logger.Errors);
            Assert.Equal(1, logger.LogMessages.Count);
            var logMessage = logger.LogMessages.First();
            Assert.Equal(LogLevel.Error, logMessage.Level);
            Assert.True(logMessage.Message.Contains("Detected package downgrade: D from 3.0.0 to centrally defined 2.0.0. "));
            Assert.Equal(NuGetLogCode.NU1109, logMessage.Code);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task RestoreCommand_DowngradeIsErrorWhen_DowngradedByCentralTransitiveDependency(bool CentralPackageTransitivePinningEnabled)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var sources = new List<PackageSource> { new PackageSource(pathContext.PackageSource) };
                var tdpString = CentralPackageTransitivePinningEnabled ? "true" : "false";
                var project1Json = @"
                {
                  ""version"": ""1.0.0"",
                    ""restore"": {
                                    ""projectUniqueName"": ""TestProject"",
                                    ""centralPackageVersionsManagementEnabled"": true,
                                    ""CentralPackageTransitivePinningEnabled"": " + tdpString + @",
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""[2.0.0,)"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                }
                        },
                        ""centralPackageVersions"": {
                            ""packageA"": ""[2.0.0,)"",
                            ""packageB"": ""[1.0.0,)""
                        }
                    }
                  }
                }";

                var packageA_Version200 = new SimpleTestPackageContext("packageA", "2.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");

                packageA_Version200.Dependencies.Add(packageB_Version200);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version200,
                    packageB_Version100,
                    packageB_Version200
                    );

                // set up the project
                var spec = JsonPackageSpecReader.GetPackageSpec(project1Json, projectName, Path.Combine(projectPath, $"{projectName}.json")).WithTestRestoreMetadata();

                var request = new TestRestoreRequest(spec, sources, pathContext.UserPackagesFolder, logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var command = new RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();

                // Assert
                if (CentralPackageTransitivePinningEnabled)
                {
                    Assert.False(result.Success);
                    var downgradeErrorMessages = logger.Messages.Where(s => s.Contains("Detected package downgrade: packageB from 2.0.0 to centrally defined 1.0.0.")).ToList();
                    Assert.Equal(1, downgradeErrorMessages.Count);
                }
                else
                {
                    Assert.True(result.Success);
                }
            }
        }

        [Fact]
        public async Task RestoreCommand_DowngradeIsNotErrorWhen_DowngradedByCentralDirectDependency()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var sources = new List<PackageSource> { new PackageSource(pathContext.PackageSource) };

                var project1Json = @"
                {
                  ""version"": ""1.0.0"",
                    ""restore"": {
                                    ""projectUniqueName"": ""TestProject"",
                                    ""centralPackageVersionsManagementEnabled"": true
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""[2.0.0]"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                },
                                ""packageB"": {
                                    ""version"": ""[1.0.0]"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                }
                        },
                        ""centralPackageVersions"": {
                            ""packageA"": ""[2.0.0]"",
                            ""packageB"": ""[1.0.0]""
                        }
                    }
                  }
                }";

                var packageA_Version200 = new SimpleTestPackageContext("packageA", "2.0.0");
                var packageB_Version100 = new SimpleTestPackageContext("packageB", "1.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");

                packageA_Version200.Dependencies.Add(packageB_Version200);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version200,
                    packageB_Version100,
                    packageB_Version200
                    );

                // set up the project
                var spec = JsonPackageSpecReader.GetPackageSpec(project1Json, projectName, Path.Combine(projectPath, $"{projectName}.json")).WithTestRestoreMetadata();

                var request = new TestRestoreRequest(spec, sources, pathContext.UserPackagesFolder, logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var command = new RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();

                // Assert
                Assert.True(result.Success);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task RestoreCommand_CentralVersion_ErrorWhenFloatingCentralVersions(bool enabled)
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    new SimpleTestPackageContext("foo", "1.0.0"));

                using var context = new SourceCacheContext();

                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var outputPath = Path.Combine(projectPath, "obj");
                // Package Bar does not have a corresponding PackageVersion
                var packageRefDependecyFoo = new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("foo", versionRange: null, typeConstraint: LibraryDependencyTarget.Package),
                };

                var centralVersionFoo = new CentralPackageVersion("foo", VersionRange.Parse("1.*", allowFloating: true));

                var tfi = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>() { packageRefDependecyFoo },
                    new List<CentralPackageVersion>() { centralVersionFoo });

                var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>() { tfi })
                {
                    FilePath = projectPath,
                    Name = projectName,
                    RestoreMetadata = new ProjectRestoreMetadata()
                    {
                        ProjectUniqueName = projectName,
                        CentralPackageVersionsEnabled = true,
                        CentralPackageFloatingVersionsEnabled = enabled,
                        ProjectStyle = ProjectStyle.PackageReference,
                        OutputPath = outputPath,
                    }
                };

                var dgspec = new DependencyGraphSpec();
                dgspec.AddProject(packageSpec);

                var sources = new List<PackageSource> { new PackageSource(pathContext.PackageSource) };
                var logger = new TestLogger();

                var request = new TestRestoreRequest(packageSpec, sources, "", logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var restoreCommand = new RestoreCommand(request);

                var result = await restoreCommand.ExecuteAsync();

                // Assert
                if (enabled)
                {
                    Assert.True(result.Success);
                }
                else
                {
                    Assert.False(result.Success);
                    Assert.Equal(1, logger.ErrorMessages.Count);
                    logger.ErrorMessages.TryDequeue(out var errorMessage);
                    Assert.True(errorMessage.Contains("Centrally defined floating package versions are not allowed."));
                    var messagesForNU1011 = result.LockFile.LogMessages.Where(m => m.Code == NuGetLogCode.NU1011);
                    Assert.Equal(1, messagesForNU1011.Count());
                }
            }
        }

        [Fact]
        public async Task RestoreCommand_CentralVersion_ErrorWhenNotAllPRItemsHaveCorespondingPackageVersion()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var outputPath = Path.Combine(projectPath, "obj");
                // Package Bar does not have a corresponding PackageVersion
                var packageRefDependecyBar = new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("bar", versionRange: null, typeConstraint: LibraryDependencyTarget.Package),
                };

                var centralVersionFoo = new CentralPackageVersion("foo", VersionRange.Parse("1.0.0"));

                var tfi = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>() { packageRefDependecyBar },
                    new List<CentralPackageVersion>() { centralVersionFoo });

                var packageSpec = new PackageSpec(new List<TargetFrameworkInformation>() { tfi });
                packageSpec.RestoreMetadata = new ProjectRestoreMetadata()
                {
                    ProjectUniqueName = projectName,
                    CentralPackageVersionsEnabled = true,
                    ProjectStyle = ProjectStyle.PackageReference,
                    OutputPath = outputPath,
                };
                packageSpec.FilePath = projectPath;

                var dgspec = new DependencyGraphSpec();
                dgspec.AddProject(packageSpec);

                var sources = new List<PackageSource>();
                var logger = new TestLogger();

                var request = new TestRestoreRequest(dgspec.GetProjectSpec(projectName), sources, "", logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var restoreCommand = new RestoreCommand(request);

                var result = await restoreCommand.ExecuteAsync();

                // Assert
                Assert.False(result.Success);
                Assert.Equal(1, logger.ErrorMessages.Count);
                logger.ErrorMessages.TryDequeue(out var errorMessage);
                Assert.True(errorMessage.Contains("The PackageReference items bar do not have corresponding PackageVersion."));
                var messagesForNU1010 = result.LockFile.LogMessages.Where(m => m.Code == NuGetLogCode.NU1010);
                Assert.Equal(1, messagesForNU1010.Count());
            }
        }

        [Fact]
        public async Task RestoreCommand_CentralVersion_Multitargeting_NoFailureSamePackageInTwoFrameworsDirectAndTransitive()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var projectName = "TestProject";
                var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                var sources = new List<PackageSource> { new PackageSource(pathContext.PackageSource) };

                // net472 will have packageA as direct dependency that has packageB as transitive
                // netstandard1.1 will have packageB as direct dependency
                var project1Json = @"
                {
                  ""version"": ""1.0.0"",
                    ""restore"": {
                                    ""projectUniqueName"": ""TestProject"",
                                    ""centralPackageVersionsManagementEnabled"": true
                    },
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""[2.0.0]"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                }
                        },
                        ""centralPackageVersions"": {
                            ""packageA"": ""[2.0.0]"",
                            ""packageB"": ""[2.0.0]""
                        }
                    },
                    ""netstandard1.1"": {
                        ""dependencies"": {
                                ""packageB"": {
                                    ""version"": ""[2.0.0]"",
                                    ""target"": ""Package"",
                                    ""versionCentrallyManaged"": true
                                }
                        },
                        ""centralPackageVersions"": {
                            ""packageA"": ""[2.0.0]"",
                            ""packageB"": ""[2.0.0]""
                        }
                    }
                  }
                }";

                var packageA_Version200 = new SimpleTestPackageContext("packageA", "2.0.0");
                var packageB_Version200 = new SimpleTestPackageContext("packageB", "2.0.0");

                packageA_Version200.Dependencies.Add(packageB_Version200);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    packageA_Version200,
                    packageB_Version200
                    );

                // set up the project
                var spec = JsonPackageSpecReader.GetPackageSpec(project1Json, projectName, Path.Combine(projectPath, $"{projectName}.json")).WithTestRestoreMetadata();

                var request = new TestRestoreRequest(spec, sources, pathContext.UserPackagesFolder, logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var command = new RestoreCommand(request);

                // Act
                var result = await command.ExecuteAsync();

                // Assert
                Assert.True(result.Success);
            }
        }

        [Fact]
        public async Task RestoreCommand_CentralVersion_AssetsFile_VerifyProjectsReferencesInTargets()
        {
            // Arrange
            var framework = new NuGetFramework("net46");
            var projectName1 = "TestProject1";
            var projectName2 = "TestProject2";
            var packageName = "foo";
            var dummyPackageName = "dummy";
            var packageVersion = "1.0.0";

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectPath1 = Path.Combine(pathContext.SolutionRoot, projectName1, $"{projectName1}.csproj");
                var projectPath2 = Path.Combine(pathContext.SolutionRoot, projectName2, $"{projectName2}.csproj");
                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(pathContext.PackageSource));
                var logger = new TestLogger();

                var dependencyFoo = new LibraryDependency()
                {
                    LibraryRange = new LibraryRange() { Name = packageName }
                };
                var centralVersionFoo = new CentralPackageVersion(packageName, VersionRange.Parse(packageVersion));
                var centralVersionDummy = new CentralPackageVersion(dummyPackageName, VersionRange.Parse(packageVersion));

                var packageFooContext = new SimpleTestPackageContext(packageName, packageVersion);
                packageFooContext.AddFile("runtimes/win7-x64/lib/net46/foo.dll");
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageFooContext);

                var packageDummyontext = new SimpleTestPackageContext(dummyPackageName, packageVersion);
                packageDummyontext.AddFile("runtimes/win7-x64/lib/net46/dummy.dll");
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageDummyontext);

                var tfi = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>() { dependencyFoo },
                    new List<CentralPackageVersion>() { centralVersionFoo, centralVersionDummy },
                    framework);

                PackageSpec packageSpec2 = CreatePackageSpec(new List<TargetFrameworkInformation>() { tfi }, framework, projectName2, projectPath2, centralPackageManagementEnabled: true);
                PackageSpec packageSpec1 = CreatePackageSpec(new List<TargetFrameworkInformation>() { tfi }, framework, projectName1, projectPath1, centralPackageManagementEnabled: true);
                packageSpec1 = packageSpec1.WithTestProjectReference(packageSpec2);

                var dgspec = new DependencyGraphSpec();
                dgspec.AddProject(packageSpec1);

                var request = new TestRestoreRequest(dgspec.GetProjectSpec(projectName1), sources, pathContext.PackagesV2, logger)
                {
                    LockFilePath = Path.Combine(projectPath1, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };
                request.ExternalProjects.Add(new ExternalProjectReference(
                   projectName1,
                   packageSpec1,
                   projectPath1,
                   new string[] { projectName2 }));

                request.ExternalProjects.Add(new ExternalProjectReference(
                   projectName2,
                   packageSpec2,
                   projectPath2,
                   Array.Empty<string>()));

                var restoreCommand = new RestoreCommand(request);
                var result = await restoreCommand.ExecuteAsync();
                var lockFile = result.LockFile;

                var targetLib = lockFile.Targets.First().Libraries.Where(l => l.Name == projectName2).FirstOrDefault();

                // Assert
                Assert.True(result.Success);
                Assert.NotNull(targetLib);
                Assert.Equal(1, targetLib.Dependencies.Count);
                Assert.True(targetLib.Dependencies.Any(d => d.Id == packageName));
            }
        }

        /// <summary>
        /// Verifies that when a transitive package version is pinned, the PrivateAssets flow from the package that pulled it into the graph to the pinned dependency.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RestoreCommand_CentralVersion_AssetsFile_PrivateAssetsFlowsToPinnedDependenciesWithTopLevelDependency()
        {
            // Arrange
            var framework = new NuGetFramework("net46");

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectInfo = new
                {
                    Name = "ProjectA",
                    Directory = Directory.CreateDirectory(Path.Combine(pathContext.SolutionRoot, "ProjectA")),
                    Path = new FileInfo(Path.Combine(pathContext.SolutionRoot, "ProjectA", "Project1.csproj")).FullName
                };

                var logger = new TestLogger();

                // PackageA 1.0.0 -> PackageB 1.0.0 -> PackageC 1.0.0 -> PackageD 1.0.0
                var packageA = new PackageIdentity("PackageA", new NuGetVersion("1.0.0"));
                var packageB = new PackageIdentity("PackageB", new NuGetVersion("1.0.0"));
                var packageC = new PackageIdentity("PackageC", new NuGetVersion("1.0.0"));
                var packageD = new PackageIdentity("PackageD", new NuGetVersion("1.0.0"));

                var packageDContext = new SimpleTestPackageContext(packageD);
                packageDContext.AddFile("lib/net46/PackageD.dll");
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageDContext);

                var packageCContext = new SimpleTestPackageContext(packageC);
                packageCContext.AddFile("lib/net46/PackageC.dll");
                packageCContext.Dependencies.Add(packageDContext);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageCContext);

                var packageBContext = new SimpleTestPackageContext(packageB);
                packageBContext.AddFile("lib/net46/PackageB.dll");
                packageBContext.Dependencies.Add(packageCContext);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageBContext);

                var packageAContext = new SimpleTestPackageContext(packageA);
                packageAContext.AddFile("lib/net46/PackageA.dll");
                packageAContext.Dependencies.Add(packageBContext);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageAContext);

                var tfi = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>
                    {
                        new LibraryDependency()
                        {
                            LibraryRange = new LibraryRange()
                            {
                                Name = packageA.Id,
                                TypeConstraint = LibraryDependencyTarget.Package,
                            },
                            VersionCentrallyManaged = true,
                            SuppressParent = LibraryIncludeFlags.Runtime | LibraryIncludeFlags.Compile
                        },
                        new LibraryDependency()
                        {
                            LibraryRange = new LibraryRange()
                            {
                                Name = packageB.Id,
                                TypeConstraint = LibraryDependencyTarget.Package,
                            },
                            VersionCentrallyManaged = true,
                            SuppressParent = LibraryIncludeFlags.Build
                        }
                    },
                    new List<CentralPackageVersion>
                    {
                        new CentralPackageVersion(packageA.Id, new VersionRange(packageA.Version)),
                        new CentralPackageVersion(packageB.Id, new VersionRange(packageB.Version)),
                        new CentralPackageVersion(packageD.Id, new VersionRange(packageD.Version))
                    },
                    framework);

                PackageSpec packageSpec = CreatePackageSpec(
                    new List<TargetFrameworkInformation>() { tfi },
                    framework,
                    projectInfo.Name,
                    projectInfo.Path,
                    centralPackageManagementEnabled: true,
                    centralPackageTransitivePinningEnabled: true);


                var dgspec = new DependencyGraphSpec();
                dgspec.AddProject(packageSpec);

                var request = new TestRestoreRequest(dgspec.GetProjectSpec(projectInfo.Name), new[] { new PackageSource(pathContext.PackageSource) }, pathContext.PackagesV2, logger)
                {
                    LockFilePath = Path.Combine(projectInfo.Directory.FullName, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var restoreCommand = new RestoreCommand(request);
                var result = await restoreCommand.ExecuteAsync();
                var lockFile = result.LockFile;

                var targetLib = lockFile.Targets.First().Libraries.Where(l => l.Name == packageA.Id).FirstOrDefault();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1, lockFile.CentralTransitiveDependencyGroups.Count);

                List<LibraryDependency> transitiveDependencies = lockFile.CentralTransitiveDependencyGroups.First().TransitiveDependencies.ToList();

                // Only PackageD should be in the transitive pinned items
                Assert.Equal(1, transitiveDependencies.Count);

                LibraryDependency transitiveDependency = transitiveDependencies.First();

                Assert.Equal(packageD.Id, transitiveDependency.Name);
                Assert.Equal(LibraryIncludeFlags.Build, transitiveDependency.SuppressParent);
            }
        }

        /// <summary>
        /// Verifies that when a transitive package version is pinned, the PrivateAssets flow from the package that pulled it into the graph to the pinned dependency.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RestoreCommand_CentralVersion_AssetsFile_PrivateAssetsFlowsToPinnedDependenciesWithSingleParent()
        {
            // Arrange
            var framework = new NuGetFramework("net46");

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectInfo = new
                {
                    Name = "ProjectA",
                    Directory = Directory.CreateDirectory(Path.Combine(pathContext.SolutionRoot, "ProjectA")),
                    Path = new FileInfo(Path.Combine(pathContext.SolutionRoot, "ProjectA", "Project1.csproj")).FullName
                };

                var logger = new TestLogger();

                // PackageA 1.0.0 -> PackageB 1.0.0 -> PackageC 1.0.0 -> PackageD 1.0.0
                var packageA = new PackageIdentity("PackageA", new NuGetVersion("1.0.0"));
                var packageB = new PackageIdentity("PackageB", new NuGetVersion("1.0.0"));
                var packageC = new PackageIdentity("PackageC", new NuGetVersion("1.0.0"));
                var packageD = new PackageIdentity("PackageD", new NuGetVersion("1.0.0"));

                var packageDContext = new SimpleTestPackageContext(packageD);
                packageDContext.AddFile("lib/net46/PackageD.dll");
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageDContext);

                var packageCContext = new SimpleTestPackageContext(packageC);
                packageCContext.AddFile("lib/net46/PackageC.dll");
                packageCContext.Dependencies.Add(packageDContext);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageCContext);

                var packageBContext = new SimpleTestPackageContext(packageB);
                packageBContext.AddFile("lib/net46/PackageB.dll");
                packageBContext.Dependencies.Add(packageCContext);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageBContext);

                var packageAContext = new SimpleTestPackageContext(packageA);
                packageAContext.AddFile("lib/net46/PackageA.dll");
                packageAContext.Dependencies.Add(packageBContext);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageAContext);

                var tfi = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>
                    {
                        new LibraryDependency()
                        {
                            LibraryRange = new LibraryRange()
                            {
                                Name = packageA.Id,
                                TypeConstraint = LibraryDependencyTarget.Package,
                            },
                            VersionCentrallyManaged = true,
                            SuppressParent = LibraryIncludeFlags.Runtime | LibraryIncludeFlags.Compile
                        },
                    },
                    new List<CentralPackageVersion>
                    {
                        new CentralPackageVersion(packageA.Id, new VersionRange(packageA.Version)),
                        new CentralPackageVersion(packageB.Id, new VersionRange(packageB.Version)),
                        new CentralPackageVersion(packageD.Id, new VersionRange(packageD.Version))
                    },
                    framework);

                PackageSpec packageSpec = CreatePackageSpec(
                    new List<TargetFrameworkInformation>() { tfi },
                    framework,
                    projectInfo.Name,
                    projectInfo.Path,
                    centralPackageManagementEnabled: true,
                    centralPackageTransitivePinningEnabled: true);


                var dgspec = new DependencyGraphSpec();
                dgspec.AddProject(packageSpec);

                var request = new TestRestoreRequest(dgspec.GetProjectSpec(projectInfo.Name), new[] { new PackageSource(pathContext.PackageSource) }, pathContext.PackagesV2, logger)
                {
                    LockFilePath = Path.Combine(projectInfo.Directory.FullName, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var restoreCommand = new RestoreCommand(request);
                var result = await restoreCommand.ExecuteAsync();
                var lockFile = result.LockFile;

                var targetLib = lockFile.Targets.First().Libraries.Where(l => l.Name == packageA.Id).FirstOrDefault();

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1, lockFile.CentralTransitiveDependencyGroups.Count);

                List<LibraryDependency> transitiveDependencies = lockFile.CentralTransitiveDependencyGroups.First().TransitiveDependencies.ToList();

                Assert.Equal(2, transitiveDependencies.Count);

                LibraryDependency transitiveDependencyB = transitiveDependencies.Single(i => i.Name.Equals(packageB.Id));

                Assert.Equal(LibraryIncludeFlags.Runtime | LibraryIncludeFlags.Compile, transitiveDependencyB.SuppressParent);

                LibraryDependency transitiveDependencyD = transitiveDependencies.Single(i => i.Name.Equals(packageD.Id));

                Assert.Equal(LibraryIncludeFlags.Runtime | LibraryIncludeFlags.Compile, transitiveDependencyD.SuppressParent);
            }
        }

        [Theory]
        [InlineData(LibraryIncludeFlags.All, 0)]
        [InlineData(LibraryIncludeFlags.None, 1)]
        public async Task RestoreCommand_CentralVersion_AssetsFile_PrivateAssetsFlowsToPinnedDependenciesWithSingleParentProject(LibraryIncludeFlags privateAssets, int expectedCount)
        {
            // Arrange
            using (var testPathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();

                var project1Directory = new DirectoryInfo(Path.Combine(testPathContext.SolutionRoot, "Project1"));
                var project2Directory = new DirectoryInfo(Path.Combine(testPathContext.SolutionRoot, "Project2"));

                // Project1 -> Project2 -> PackageA 1.0.0
                var packageA = new SimpleTestPackageContext { Id = "PackageA", Version = "1.0.0", };
                await SimpleTestPackageUtility.CreateFullPackageAsync(testPathContext.PackageSource, packageA);

                var project2Spec = PackageReferenceSpecBuilder.Create("Project2", project2Directory.FullName)
                    .WithTargetFrameworks(new[]
                    {
                        new TargetFrameworkInformation
                        {
                            FrameworkName = NuGetFramework.Parse("net46"),
                            Dependencies = new List<LibraryDependency>(new[]
                            {
                                new LibraryDependency
                                {
                                    LibraryRange = new LibraryRange("PackageA", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.All),
                                    VersionCentrallyManaged = true,
                                },
                            }),
                            CentralPackageVersions = { new KeyValuePair<string, CentralPackageVersion>("PackageA", new CentralPackageVersion("PackageA", VersionRange.Parse("1.0.0"))) },
                        }
                    })
                    .WithCentralPackageVersionsEnabled()
                    .WithCentralPackageTransitivePinningEnabled()
                    .Build()
                    .WithTestRestoreMetadata();

                var project1Spec = PackageReferenceSpecBuilder.Create("Project1", project1Directory.FullName)
                    .WithTargetFrameworks(new[]
                    {
                        new TargetFrameworkInformation
                        {
                            FrameworkName = NuGetFramework.Parse("net46"),
                            Dependencies = new List<LibraryDependency>(),
                            CentralPackageVersions = { new KeyValuePair<string, CentralPackageVersion>("PackageA", new CentralPackageVersion("PackageA", VersionRange.Parse("1.0.0"))) },
                        }
                    })
                    .WithCentralPackageVersionsEnabled()
                    .WithCentralPackageTransitivePinningEnabled()
                    .Build()
                    .WithTestRestoreMetadata()
                    .WithTestProjectReference(project2Spec, privateAssets: privateAssets);

                var restoreContext = new RestoreArgs()
                {
                    Sources = new List<string> { testPathContext.PackageSource },
                    GlobalPackagesFolder = testPathContext.UserPackagesFolder,
                    Log = logger,
                    CacheContext = new TestSourceCacheContext(),
                };

                var request = await ProjectTestHelpers.GetRequestAsync(restoreContext, project1Spec, project2Spec);
                var restoreCommand = new RestoreCommand(request);
                var result = await restoreCommand.ExecuteAsync();
                var lockFile = result.LockFile;

                // Assert
                Assert.True(result.Success);
                Assert.Equal(expectedCount, lockFile.CentralTransitiveDependencyGroups.Count);
            }
        }

        /// <summary>
        /// Verifies that when a transitive package version is pinned and is referenced by multiple parents, the PrivateAssets flow from the package that pulled it into the graph to the pinned dependency.
        /// </summary>
        [Theory]
        [InlineData(
            LibraryIncludeFlags.All, // PrivateAssets="All"
            LibraryIncludeFlags.Build | LibraryIncludeFlags.ContentFiles | LibraryIncludeFlags.Analyzers, // Default PrivateAssets
            LibraryIncludeFlags.Build | LibraryIncludeFlags.ContentFiles | LibraryIncludeFlags.Analyzers)] // Expect only the intersection, in this case the default
        [InlineData(
            LibraryIncludeFlags.Compile | LibraryIncludeFlags.Runtime, // PrivateAssets="Compile;Runtime"
            LibraryIncludeFlags.Compile, // PrivateAssets="Compile"
            LibraryIncludeFlags.Compile)] // The intersection is Compile
        [InlineData(LibraryIncludeFlags.All, LibraryIncludeFlags.All, LibraryIncludeFlags.All)] // When both parents have PrivateAssets="All", expect that the dependency does not flow
        [InlineData(LibraryIncludeFlags.None, LibraryIncludeFlags.None, LibraryIncludeFlags.None)] // When both parents have PrivateAssets="None", expect all assets of the dependency to flow
        [InlineData(LibraryIncludeFlags.None, LibraryIncludeFlags.Runtime | LibraryIncludeFlags.Compile, LibraryIncludeFlags.None)] // When both parents have PrivateAssets="None", expect that the dependency is completely suppressed
        public async Task RestoreCommand_CentralVersion_AssetsFile_PrivateAssetsFlowsToPinnedDependenciesWithMultipleParents(LibraryIncludeFlags suppressParent1, LibraryIncludeFlags suppressParent2, LibraryIncludeFlags expected)
        {
            // Arrange
            var framework = new NuGetFramework("net46");

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectInfo = new
                {
                    Name = "ProjectA",
                    Directory = Directory.CreateDirectory(Path.Combine(pathContext.SolutionRoot, "ProjectA")),
                    Path = new FileInfo(Path.Combine(pathContext.SolutionRoot, "ProjectA", "Project1.csproj")).FullName
                };

                var logger = new TestLogger();

                var packageA = new PackageIdentity("PackageA", new NuGetVersion("1.0.0"));
                var packageB = new PackageIdentity("PackageB", new NuGetVersion("1.0.0"));
                var packageC1_0 = new PackageIdentity("PackageC", new NuGetVersion("1.0.0"));
                var packageC2_0 = new PackageIdentity("PackageC", new NuGetVersion("2.0.0"));

                var packageC2_0Context = new SimpleTestPackageContext(packageC2_0);
                packageC2_0Context.AddFile("lib/net46/PackageC.dll");
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageC2_0Context);

                var packageC1_0Context = new SimpleTestPackageContext(packageC1_0);
                packageC1_0Context.AddFile("lib/net46/PackageC.dll");
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageC1_0Context);

                // PackageB 1.0.0 -> PackageC 2.0.0
                var packageBContext = new SimpleTestPackageContext(packageB);
                packageBContext.AddFile("lib/net46/PackageB.dll");
                packageBContext.Dependencies.Add(packageC2_0Context);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageBContext);

                // PackageA 1.0.0 -> PackageC 1.0.0
                var packageAContext = new SimpleTestPackageContext(packageA);
                packageAContext.AddFile("lib/net46/PackageA.dll");
                packageAContext.Dependencies.Add(packageC1_0Context);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, packageAContext);

                var tfi = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>
                    {
                        new LibraryDependency()
                        {
                            LibraryRange = new LibraryRange()
                            {
                                Name = packageA.Id,
                                TypeConstraint = LibraryDependencyTarget.Package,
                            },
                            VersionCentrallyManaged = true,
                            SuppressParent = suppressParent1,
                        },
                        new LibraryDependency()
                        {
                            LibraryRange = new LibraryRange()
                            {
                                Name = packageB.Id,
                                TypeConstraint = LibraryDependencyTarget.Package,
                            },
                            VersionCentrallyManaged = true,
                            SuppressParent = suppressParent2,
                        },
                    },
                    new List<CentralPackageVersion>
                    {
                        new CentralPackageVersion(packageA.Id, new VersionRange(packageA.Version)),
                        new CentralPackageVersion(packageB.Id, new VersionRange(packageB.Version)),
                        new CentralPackageVersion(packageC2_0.Id, new VersionRange(packageC2_0.Version))
                    },
                    framework);

                PackageSpec packageSpec = CreatePackageSpec(
                    new List<TargetFrameworkInformation>() { tfi },
                    framework,
                    projectInfo.Name,
                    projectInfo.Path,
                    centralPackageManagementEnabled: true,
                    centralPackageTransitivePinningEnabled: true);


                var dgspec = new DependencyGraphSpec();
                dgspec.AddProject(packageSpec);

                var request = new TestRestoreRequest(dgspec.GetProjectSpec(projectInfo.Name), new[] { new PackageSource(pathContext.PackageSource) }, pathContext.PackagesV2, logger)
                {
                    LockFilePath = Path.Combine(projectInfo.Directory.FullName, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference
                };

                var restoreCommand = new RestoreCommand(request);
                var result = await restoreCommand.ExecuteAsync();
                var lockFile = result.LockFile;

                // Assert
                Assert.True(result.Success);
                if (expected == LibraryIncludeFlags.All)
                {
                    Assert.Equal(0, lockFile.CentralTransitiveDependencyGroups.Count);
                }
                else
                {
                    Assert.Equal(1, lockFile.CentralTransitiveDependencyGroups.Count);

                    List<LibraryDependency> transitiveDependencies = lockFile.CentralTransitiveDependencyGroups.First().TransitiveDependencies.ToList();

                    Assert.Equal(1, transitiveDependencies.Count);

                    LibraryDependency transitiveDependencyC = transitiveDependencies.Single(i => i.Name.Equals(packageC2_0.Id));

                    Assert.Equal(expected, transitiveDependencyC.SuppressParent);
                }
            }
        }

        /// <summary>
        /// Verifies an error is logged when a user attempts to specify a VersionOverride but the feature is disabled and that restore succeeds if the feature is enabled.
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task RestoreCommand_CentralVersion_ErrorWhenVersionOverrideUsedButIsDisabled(bool isCentralPackageVersionOverrideDisabled, bool isVersionOverrideUsed)
        {
            const string projectName = "TestProject";

            const string packageName = "PackageA";

            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
                string outputPath = Path.Combine(projectPath, "obj");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.PackageSource, PackageSaveMode.Defaultv3, new SimpleTestPackageContext(packageName, "1.0.0"), new SimpleTestPackageContext(packageName, "2.0.0"));

                var packageRefDependencyFoo = new LibraryDependency()
                {
                    LibraryRange = new LibraryRange(packageName, versionRange: null, typeConstraint: LibraryDependencyTarget.Package),
                };
                if (isVersionOverrideUsed)
                {
                    packageRefDependencyFoo.VersionOverride = new VersionRange(NuGetVersion.Parse("2.0.0"));
                }

                var packageVersion = new CentralPackageVersion(packageName, VersionRange.Parse("1.0.0"));

                TargetFrameworkInformation targetFrameworkInformation = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>
                    {
                        packageRefDependencyFoo
                    },
                    new List<CentralPackageVersion>
                    {
                        packageVersion
                    });

                PackageSpec packageSpec = CreatePackageSpec(new List<TargetFrameworkInformation>() { targetFrameworkInformation }, targetFrameworkInformation.FrameworkName, projectName, projectPath, centralPackageManagementEnabled: true);

                packageSpec.RestoreMetadata.CentralPackageVersionOverrideDisabled = isCentralPackageVersionOverrideDisabled;

                var dgspec = new DependencyGraphSpec();

                dgspec.AddProject(packageSpec);

                var sources = new List<PackageSource>();
                var logger = new TestLogger();

                var request = new TestRestoreRequest(packageSpec, new PackageSource[] { new PackageSource(pathContext.PackageSource) }, pathContext.UserPackagesFolder, logger)
                {
                    LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference,
                };

                var restoreCommand = new RestoreCommand(request);

                var result = await restoreCommand.ExecuteAsync();

                // Assert

                if (isCentralPackageVersionOverrideDisabled && isVersionOverrideUsed)
                {
                    Assert.False(result.Success);

                    Assert.Equal(1, logger.ErrorMessages.Count);

                    logger.ErrorMessages.TryDequeue(out var errorMessage);

                    Assert.True(errorMessage.Contains(NuGetLogCode.NU1013.ToString()));

                    Assert.True(result.LockFile.LogMessages.Any(m => m.Code == NuGetLogCode.NU1013), "Lockfile should contain an error with code NU1013");
                }
                else
                {
                    Assert.True(result.Success);

                    Assert.Equal(0, logger.ErrorMessages.Count);
                }
            }
        }

        [Fact]
        public async Task ExecuteAsync_WithSinglePackage_PopulatesCorrectTelemetry()
        {
            // Arrange
            using var context = new SourceCacheContext();
            using var pathContext = new SimpleTestPathContext();
            var projectName = "TestProject";
            var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
            PackageSpec packageSpec = ProjectTestHelpers.GetPackageSpec(projectName, pathContext.SolutionRoot, "net472", "a");
            packageSpec.RestoreMetadata.RestoreAuditProperties.EnableAudit = bool.TrueString;

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("a", "1.0.0"));
            var logger = new TestLogger();

            var request = new TestRestoreRequest(packageSpec, new PackageSource[] { new PackageSource(pathContext.PackageSource) }, pathContext.UserPackagesFolder, logger)
            {
                LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                ProjectStyle = ProjectStyle.PackageReference,
            };

            // Set-up telemetry service - Important to set-up the service *after* the package source creation call as that emits telemetry!
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            var _telemetryService = new Mock<INuGetTelemetryService>(MockBehavior.Loose);
            _telemetryService
                .Setup(x => x.EmitTelemetryEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            TelemetryActivity.NuGetTelemetryService = _telemetryService.Object;

            var restoreCommand = new RestoreCommand(request);
            RestoreResult result = await restoreCommand.ExecuteAsync();

            // Assert
            result.Success.Should().BeTrue(because: logger.ShowMessages());
            IEnumerable<string> telEventNames = telemetryEvents.Select(e => e.Name);
            telemetryEvents.Should().HaveCountLessOrEqualTo(3);
            telEventNames.Should().Contain("ProjectRestoreInformation");

            var projectInformationEvent = telemetryEvents.Single(e => e.Name.Equals("ProjectRestoreInformation"));

            var expectedProperties = new Dictionary<string, Action<object>>()
            {
                ["RestoreSuccess"] = value => value.Should().Be(true),
                ["NoOpResult"] = value => value.Should().Be(false),
                ["IsCentralVersionManagementEnabled"] = value => value.Should().Be(false),
                ["NoOpCacheFileEvaluationResult"] = value => value.Should().Be(false),
                ["IsLockFileEnabled"] = value => value.Should().Be(false),
                ["IsLockFileValidForRestore"] = value => value.Should().Be(false),
                ["LockFileEvaluationResult"] = value => value.Should().Be(true),
                ["NoOpDuration"] = value => value.Should().NotBeNull(),
                ["TotalUniquePackagesCount"] = value => value.Should().Be(1),
                ["NewPackagesInstalledCount"] = value => value.Should().Be(1),
                ["EvaluateLockFileDuration"] = value => value.Should().NotBeNull(),
                ["CreateRestoreTargetGraphDuration"] = value => value.Should().NotBeNull(),
                ["GenerateRestoreGraphDuration"] = value => value.Should().NotBeNull(),
                ["CreateRestoreResultDuration"] = value => value.Should().NotBeNull(),
                ["WalkFrameworkDependencyDuration"] = value => value.Should().NotBeNull(),
                ["GenerateAssetsFileDuration"] = value => value.Should().NotBeNull(),
                ["ValidateRestoreGraphsDuration"] = value => value.Should().NotBeNull(),
                ["EvaluateDownloadDependenciesDuration"] = value => value.Should().NotBeNull(),
                ["NoOpCacheFileEvaluateDuration"] = value => value.Should().NotBeNull(),
                ["StartTime"] = value => value.Should().NotBeNull(),
                ["EndTime"] = value => value.Should().NotBeNull(),
                ["OperationId"] = value => value.Should().NotBeNull(),
                ["Duration"] = value => value.Should().NotBeNull(),
                ["PackageSourceMapping.IsMappingEnabled"] = value => value.Should().Be(false),
                ["SourcesCount"] = value => value.Should().Be(1),
                ["HttpSourcesCount"] = value => value.Should().Be(0),
                ["LocalSourcesCount"] = value => value.Should().Be(1),
                ["FallbackFoldersCount"] = value => value.Should().Be(0),
                ["Audit.Enabled"] = value => value.Should().Be("enabled"),
                ["Audit.Level"] = value => value.Should().Be(0),
                ["Audit.Mode"] = value => value.Should().Be("Unknown"),
                ["Audit.Vulnerability.Direct.Count"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Direct.Severity0"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Direct.Severity1"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Direct.Severity2"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Direct.Severity3"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Direct.SeverityInvalid"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Transitive.Count"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Transitive.Severity0"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Transitive.Severity1"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Transitive.Severity2"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Transitive.Severity3"] = value => value.Should().Be(0),
                ["Audit.Vulnerability.Transitive.SeverityInvalid"] = value => value.Should().Be(0),
                ["Audit.DataSources"] = value => value.Should().Be(0),
                ["Audit.Duration.Download"] = value => value.Should().BeOfType<double>(),
                ["Audit.Duration.Total"] = value => value.Should().BeOfType<double>(),
            };

            HashSet<string> actualProperties = new();
            foreach (var eventProperty in projectInformationEvent)
            {
                actualProperties.Add(eventProperty.Key);
            }

            expectedProperties.Keys.Except(actualProperties).Should().BeEmpty();
            actualProperties.Except(expectedProperties.Keys).Should().BeEmpty();

            foreach (var kvp in expectedProperties)
            {
                object value = projectInformationEvent[kvp.Key];
                kvp.Value(value);
            }
        }

        [Fact]
        public async Task ExecuteAsync_WithSinglePackage_WhenNoOping_PopulatesCorrectTelemetry()
        {
            // Arrange
            using var context = new SourceCacheContext();
            using var pathContext = new SimpleTestPathContext();
            var projectName = "TestProject";
            var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
            PackageSpec packageSpec = ProjectTestHelpers.GetPackageSpec(projectName, pathContext.SolutionRoot, "net472", "a");

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                new SimpleTestPackageContext("a", "1.0.0"));
            var logger = new TestLogger();

            var request = new TestRestoreRequest(packageSpec, new PackageSource[] { new PackageSource(pathContext.PackageSource) }, pathContext.UserPackagesFolder, logger)
            {
                LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                ProjectStyle = ProjectStyle.PackageReference
            };

            // Set-up telemetry service - Important to set-up the service *after* the package source creation call as that emits telemetry!
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            var _telemetryService = new Mock<INuGetTelemetryService>(MockBehavior.Loose);
            _telemetryService
                .Setup(x => x.EmitTelemetryEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            TelemetryActivity.NuGetTelemetryService = _telemetryService.Object;

            var restoreCommand = new RestoreCommand(request);
            RestoreResult result = await restoreCommand.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);
            // Pre-conditions
            result.Success.Should().BeTrue(because: logger.ShowMessages());
            IEnumerable<string> telEventNames = telemetryEvents.Select(e => e.Name);
            telemetryEvents.Should().HaveCountLessOrEqualTo(3);
            telEventNames.Should().Contain("ProjectRestoreInformation");

            while (telemetryEvents.TryDequeue(out _))
            {
                // Clear telemetry
            }

            // Act!
            restoreCommand = new RestoreCommand(request);
            result = await restoreCommand.ExecuteAsync();

            telemetryEvents.Should().HaveCount(1);

            var projectInformationEvent = telemetryEvents.Single(e => e.Name.Equals("ProjectRestoreInformation"));

            projectInformationEvent.Count.Should().Be(22);
            projectInformationEvent["RestoreSuccess"].Should().Be(true);
            projectInformationEvent["NoOpResult"].Should().Be(true);
            projectInformationEvent["IsCentralVersionManagementEnabled"].Should().Be(false);
            projectInformationEvent["NoOpCacheFileEvaluationResult"].Should().Be(true);
            projectInformationEvent["NoOpRestoreOutputEvaluationResult"].Should().Be(true);
            projectInformationEvent["NoOpDuration"].Should().NotBeNull();
            projectInformationEvent["TotalUniquePackagesCount"].Should().Be(1);
            projectInformationEvent["NewPackagesInstalledCount"].Should().Be(0);
            projectInformationEvent["NoOpCacheFileEvaluateDuration"].Should().NotBeNull();
            projectInformationEvent["StartTime"].Should().NotBeNull();
            projectInformationEvent["EndTime"].Should().NotBeNull();
            projectInformationEvent["OperationId"].Should().NotBeNull();
            projectInformationEvent["Duration"].Should().NotBeNull();
            projectInformationEvent["NoOpRestoreOutputEvaluationDuration"].Should().NotBeNull();
            projectInformationEvent["NoOpReplayLogsDuration"].Should().NotBeNull();
            projectInformationEvent["PackageSourceMapping.IsMappingEnabled"].Should().Be(false);
            projectInformationEvent["SourcesCount"].Should().Be(1);
            projectInformationEvent["HttpSourcesCount"].Should().Be(0);
            projectInformationEvent["LocalSourcesCount"].Should().Be(1);
            projectInformationEvent["FallbackFoldersCount"].Should().Be(0);
            projectInformationEvent["IsLockFileEnabled"].Should().Be(false);
            projectInformationEvent["NoOpCacheFileAgeDays"].Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_WithPartiallyPopulatedGlobalPackagesFolder_PopulatesNewlyInstalledPackagesTelemetry()
        {
            // Arrange
            using var context = new SourceCacheContext();
            using var pathContext = new SimpleTestPathContext();
            var projectName = "TestProject";
            var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
            PackageSpec packageSpec = ProjectTestHelpers.GetPackageSpec(projectName, pathContext.SolutionRoot, "net472", "a");
            var packageA = new SimpleTestPackageContext("a", "1.0.0");
            var packageB = new SimpleTestPackageContext("b", "1.0.0");
            packageA.Dependencies.Add(packageB);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                packageA,
                packageB);

            // package B should be installed in the global packages folder.

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.UserPackagesFolder,
                PackageSaveMode.Defaultv3,
                packageB);

            var logger = new TestLogger();

            var request = new TestRestoreRequest(packageSpec, new PackageSource[] { new PackageSource(pathContext.PackageSource) }, pathContext.UserPackagesFolder, logger)
            {
                LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                ProjectStyle = ProjectStyle.PackageReference
            };

            // Set-up telemetry service - Important to set-up the service *after* the package source creation call as that emits telemetry!
            var telemetryEvents = new ConcurrentQueue<TelemetryEvent>();
            var _telemetryService = new Mock<INuGetTelemetryService>(MockBehavior.Loose);
            _telemetryService
                .Setup(x => x.EmitTelemetryEvent(It.IsAny<TelemetryEvent>()))
                .Callback<TelemetryEvent>(x => telemetryEvents.Enqueue(x));

            TelemetryActivity.NuGetTelemetryService = _telemetryService.Object;

            var restoreCommand = new RestoreCommand(request);
            RestoreResult result = await restoreCommand.ExecuteAsync();
            await result.CommitAsync(logger, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue(because: logger.ShowMessages());
            IEnumerable<string> telEventNames = telemetryEvents.Select(e => e.Name);
            telemetryEvents.Should().HaveCountLessOrEqualTo(3);
            telEventNames.Should().Contain("ProjectRestoreInformation");

            var projectInformationEvent = telemetryEvents.Single(e => e.Name.Equals("ProjectRestoreInformation"));

            projectInformationEvent.Count.Should().Be(29);
            projectInformationEvent["RestoreSuccess"].Should().Be(true);
            projectInformationEvent["NoOpResult"].Should().Be(false);
            projectInformationEvent["TotalUniquePackagesCount"].Should().Be(2);
            projectInformationEvent["NewPackagesInstalledCount"].Should().Be(1);
            projectInformationEvent["PackageSourceMapping.IsMappingEnabled"].Should().Be(false);
        }

        /// A 1.0.0 -> C 1.0.0 -> D 1.1.0
        /// B 1.0.0 -> C 1.1.0 -> D 1.0.0
        /// D 1.0.0
        [Fact]
        public async Task ExecuteAsync_WithDowngradesInPrunedSubgraph_DoesNotRaiseNU1605()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();
            var logger = new TestLogger();
            var projectName = "TestProject";
            var projectPath = Path.Combine(pathContext.SolutionRoot, projectName);
            var sources = new List<PackageSource> { new PackageSource(pathContext.PackageSource) };

            var project1Json = @"
                {
                  ""version"": ""1.0.0"",
                  ""frameworks"": {
                    ""net472"": {
                        ""dependencies"": {
                            ""A"": ""1.0.0"",
                            ""B"": ""1.0.0"",
                            ""D"" : ""1.0.0""
                        }
                    }
                  }
                }";

            var A = new SimpleTestPackageContext("A", "1.0.0");
            var B = new SimpleTestPackageContext("B", "1.0.0");
            var D100 = new SimpleTestPackageContext("D", "1.0.0");
            var D110 = new SimpleTestPackageContext("D", "1.1.0");
            var C100 = new SimpleTestPackageContext("C", "1.0.0");
            var C110 = new SimpleTestPackageContext("C", "1.1.0");

            A.Dependencies.Add(C100);
            B.Dependencies.Add(C110);
            C100.Dependencies.Add(D110);
            C110.Dependencies.Add(D100);

            await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                pathContext.PackageSource,
                PackageSaveMode.Defaultv3,
                A,
                B,
                D100,
                D110,
                C100,
                C110
                );

            // set up the project
            var spec = JsonPackageSpecReader.GetPackageSpec(project1Json, projectName, Path.Combine(projectPath, $"{projectName}.json")).WithTestRestoreMetadata();

            var request = new TestRestoreRequest(spec, sources, pathContext.UserPackagesFolder, logger)
            {
                LockFilePath = Path.Combine(projectPath, "project.assets.json"),
                ProjectStyle = ProjectStyle.PackageReference
            };

            var command = new RestoreCommand(request);

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            result.Success.Should().BeTrue();
            result.LogMessages.Should().HaveCount(0);
            result.LockFile.Libraries.Count.Should().Be(4);
        }

        /// <summary>
        /// A 1.0 -> D 1.0 (Central transitive)
        ///       -> B 1.0 -> D 3.0 (Central transitive - should be ignored because it is not at root)
        ///                -> C 1.0 -> D 2.0
        /// </summary>
        [Fact]
        public async Task ExecuteAsync_TransitiveDependenciesFromNonRootLibraries_AreIgnored()
        {
            // Arrange
            var framework = new NuGetFramework("net46");
            var projectNameA = "ProjectA";
            var projectNameB = "ProjectB";
            var projectNameC = "ProjectC";
            var packageName = "PackageD";

            using (var pathContext = new SimpleTestPathContext())
            {
                var projectPathA = Path.Combine(pathContext.SolutionRoot, projectNameA, $"{projectNameA}.csproj");
                var projectPathB = Path.Combine(pathContext.SolutionRoot, projectNameB, $"{projectNameB}.csproj");
                var projectPathC = Path.Combine(pathContext.SolutionRoot, projectNameC, $"{projectNameC}.csproj");
                var sources = new List<PackageSource>();
                sources.Add(new PackageSource(pathContext.PackageSource));
                var logger = new TestLogger();

                var dependencyD = new LibraryDependency
                {
                    LibraryRange = new LibraryRange { Name = packageName }
                };

                var centralVersion1 = new CentralPackageVersion(packageName, VersionRange.Parse("1.0.0"));
                var centralVersion2 = new CentralPackageVersion(packageName, VersionRange.Parse("2.0.0"));
                var centralVersion3 = new CentralPackageVersion(packageName, VersionRange.Parse("3.0.0"));

                var package1Context = new SimpleTestPackageContext(packageName, "1.0.0");
                var package2Context = new SimpleTestPackageContext(packageName, "2.0.0");
                var package3Context = new SimpleTestPackageContext(packageName, "3.0.0");
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, package1Context);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, package2Context);
                await SimpleTestPackageUtility.CreateFullPackageAsync(pathContext.PackageSource, package3Context);

                var tfiA = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>(), // no direct dependencies
                    new List<CentralPackageVersion>() { centralVersion1 },
                    framework);

                var tfiB = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>(), // no direct dependencies
                    new List<CentralPackageVersion>() { centralVersion3 },
                    framework);

                var tfiC = CreateTargetFrameworkInformation(
                    new List<LibraryDependency>() { dependencyD }, // direct dependency
                    new List<CentralPackageVersion>() { centralVersion2 },
                    framework);

                PackageSpec packageSpecA = CreatePackageSpec(new List<TargetFrameworkInformation>() { tfiA }, framework, projectNameA, projectPathA, centralPackageManagementEnabled: true);
                PackageSpec packageSpecB = CreatePackageSpec(new List<TargetFrameworkInformation>() { tfiB }, framework, projectNameB, projectPathB, centralPackageManagementEnabled: true);
                PackageSpec packageSpecC = CreatePackageSpec(new List<TargetFrameworkInformation>() { tfiC }, framework, projectNameC, projectPathC, centralPackageManagementEnabled: true);
                packageSpecA = packageSpecA.WithTestProjectReference(packageSpecB);
                packageSpecB = packageSpecB.WithTestProjectReference(packageSpecC);
                packageSpecA.RestoreMetadata.CentralPackageTransitivePinningEnabled = true;
                packageSpecB.RestoreMetadata.CentralPackageTransitivePinningEnabled = true;
                packageSpecC.RestoreMetadata.CentralPackageTransitivePinningEnabled = true;

                var dgspec = new DependencyGraphSpec();
                dgspec.AddProject(packageSpecA);

                var request = new TestRestoreRequest(dgspec.GetProjectSpec(projectNameA), sources, pathContext.PackagesV2, logger)
                {
                    LockFilePath = Path.Combine(projectPathA, "project.assets.json"),
                    ProjectStyle = ProjectStyle.PackageReference,
                };

                var externalProjectA = new ExternalProjectReference(projectNameA, packageSpecA, projectPathA, new[] { projectNameB });
                var externalProjectB = new ExternalProjectReference(projectNameB, packageSpecB, projectPathB, new[] { projectNameC });
                var externalProjectC = new ExternalProjectReference(projectNameC, packageSpecC, projectPathC, new string[] { });
                request.ExternalProjects.Add(externalProjectA);
                request.ExternalProjects.Add(externalProjectB);
                request.ExternalProjects.Add(externalProjectC);
                var restoreCommand = new RestoreCommand(request);
                var result = await restoreCommand.ExecuteAsync();

                // Assert
                Assert.False(result.Success);
                var downgrades = result.RestoreGraphs.Single().AnalyzeResult.Downgrades;
                downgrades.Count.Should().Be(1);
                var d = downgrades.Single();
                d.DowngradedFrom.Key.ToString().Should().Be("PackageD (>= 2.0.0)");
                d.DowngradedTo.Key.ToString().Should().Be("PackageD (>= 1.0.0)");
            }
        }

        private static PackageSpec GetPackageSpec(string projectName, string testDirectory, string referenceSpec)
        {
            return JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, testDirectory).WithTestRestoreMetadata();
        }

        private static TargetFrameworkInformation CreateTargetFrameworkInformation(List<LibraryDependency> dependencies, List<CentralPackageVersion> centralVersionsDependencies, NuGetFramework framework = null)
        {
            NuGetFramework nugetFramework = framework ?? new NuGetFramework("net40");
            TargetFrameworkInformation tfi = new TargetFrameworkInformation()
            {
                AssetTargetFallback = true,
                Warn = false,
                FrameworkName = nugetFramework,
                Dependencies = dependencies,
            };

            foreach (var cvd in centralVersionsDependencies)
            {
                tfi.CentralPackageVersions.Add(cvd.Name, cvd);
            }
            LibraryDependency.ApplyCentralVersionInformation(tfi.Dependencies, tfi.CentralPackageVersions);

            return tfi;
        }

        private static PackageSpec CreatePackageSpec(List<TargetFrameworkInformation> tfis, NuGetFramework framework, string projectName, string projectPath, bool centralPackageManagementEnabled, bool centralPackageTransitivePinningEnabled = false)
        {
            var packageSpec = new PackageSpec(tfis);
            packageSpec.RestoreMetadata = new ProjectRestoreMetadata()
            {
                ProjectUniqueName = projectName,
                CentralPackageVersionsEnabled = centralPackageManagementEnabled,
                CentralPackageTransitivePinningEnabled = centralPackageTransitivePinningEnabled,
                ProjectStyle = ProjectStyle.PackageReference,
                TargetFrameworks = new List<ProjectRestoreMetadataFrameworkInfo>() { new ProjectRestoreMetadataFrameworkInfo(framework) },
                OutputPath = Path.Combine(projectPath, "obj"),
                ProjectPath = projectPath,
            };
            packageSpec.Name = projectName;
            packageSpec.FilePath = projectPath;

            return packageSpec;
        }

        private Task<GraphNode<RemoteResolveResult>> DoWalkAsync(RemoteDependencyWalker walker, string name, NuGetFramework framework)
        {
            var range = new LibraryRange
            {
                Name = name,
                VersionRange = new VersionRange(new NuGetVersion("1.0"))
            };

            return walker.WalkAsync(range, framework, runtimeIdentifier: null, runtimeGraph: null, recursive: true);
        }
    }
}

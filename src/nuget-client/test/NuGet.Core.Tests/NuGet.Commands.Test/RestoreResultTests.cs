// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class RestoreResultTests
    {
        [Fact]
        public async Task RestoreResult_CommitAsync_WithCancellationToken_ThrowsAsync()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var path = Path.Combine(td, "project.lock.json");
                var logger = new TestLogger();
                var result = new RestoreResult(
                    success: true,
                    restoreGraphs: null,
                    compatibilityCheckResults: null,
                    lockFile: new LockFile(),
                    previousLockFile: null, // different lock file
                    lockFilePath: path,
                    msbuildFiles: Enumerable.Empty<MSBuildOutputFile>(),
                    cacheFile: null,
                    cacheFilePath: null,
                    packagesLockFilePath: null,
                    packagesLockFile: null,
                    dependencyGraphSpecFilePath: null,
                    dependencyGraphSpec: null,
                    projectStyle: ProjectStyle.Unknown,
                    elapsedTime: TimeSpan.MinValue);

                // Act and Assert
                await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                {
                    var cts = new CancellationTokenSource();
                    cts.Cancel();
                    await result.CommitAsync(logger, cts.Token);
                });
            }
        }

        [Fact]
        public async Task RestoreResult_WritesCommitToInformation()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var path = Path.Combine(td, "project.lock.json");
                var logger = new TestLogger();
                var result = new RestoreResult(
                    success: true,
                    restoreGraphs: null,
                    compatibilityCheckResults: null,
                    lockFile: new LockFile(),
                    previousLockFile: null, // different lock file
                    lockFilePath: path,
                    msbuildFiles: Enumerable.Empty<MSBuildOutputFile>(),
                    cacheFile: null,
                    cacheFilePath: null,
                    packagesLockFilePath: null,
                    packagesLockFile: null,
                    dependencyGraphSpecFilePath: null,
                    dependencyGraphSpec: null,
                    projectStyle: ProjectStyle.Unknown,
                    elapsedTime: TimeSpan.MinValue);

                // Act
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.Empty(logger.MinimalMessages);
                Assert.Contains(
                    $"Writing assets file to disk. Path: {path}",
                    logger.Messages);
                Assert.True(File.Exists(path), $"The lock file should have been written: {path}");
                Assert.Equal(1, logger.Messages.Count);
            }
        }

        [Fact]
        public async Task RestoreResult_WritesSkipCommitToInformation()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var path = Path.Combine(td, "project.lock.json");
                var logger = new TestLogger();
                var result = new RestoreResult(
                    success: true,
                    restoreGraphs: null,
                    compatibilityCheckResults: null,
                    lockFile: new LockFile(),
                    previousLockFile: new LockFile(), // same lock file
                    lockFilePath: path,
                    msbuildFiles: Enumerable.Empty<MSBuildOutputFile>(),
                    cacheFile: null,
                    cacheFilePath: null,
                    packagesLockFilePath: null,
                    packagesLockFile: null,
                    dependencyGraphSpecFilePath: null,
                    dependencyGraphSpec: null,
                    projectStyle: ProjectStyle.Unknown,
                    elapsedTime: TimeSpan.MinValue);

                // Act
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.Contains(
                    $"Assets file has not changed. Skipping assets file writing. Path: {path}",
                    logger.Messages);
                Assert.False(File.Exists(path), $"The lock file should not have been written: {path}");
                Assert.Equal(1, logger.Messages.Count);
            }
        }

        [Fact]
        public async Task RestoreResult_WritesCommitToInformation_AssetsAndCache()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var path = Path.Combine(td, "project.lock.json");
                var cachePath = Path.Combine(td, "project.csproj.nuget.cache");
                var logger = new TestLogger();
                var result = new RestoreResult(
                    success: true,
                    restoreGraphs: null,
                    compatibilityCheckResults: null,
                    lockFile: new LockFile(),
                    previousLockFile: null, // different lock file
                    lockFilePath: path,
                    msbuildFiles: Enumerable.Empty<MSBuildOutputFile>(),
                    cacheFile: new CacheFile("NotSoRandomString"),
                    cacheFilePath: cachePath,
                    packagesLockFilePath: null,
                    packagesLockFile: null,
                    dependencyGraphSpecFilePath: null,
                    dependencyGraphSpec: null,
                    projectStyle: ProjectStyle.Unknown,
                    elapsedTime: TimeSpan.MinValue);

                // Act
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.Empty(logger.MinimalMessages);
                Assert.Contains(
                    $"Writing assets file to disk. Path: {path}",
                    logger.Messages);
                Assert.Contains(
                    $"Writing cache file to disk. Path: {cachePath}",
                    logger.VerboseMessages);

                Assert.True(File.Exists(path), $"The lock file should have been written: {path}");
                Assert.True(File.Exists(cachePath), $"The cache file should have been written: {cachePath}");
                Assert.Equal(2, logger.Messages.Count);
            }
        }


        [Fact]
        public async Task NoOpRestoreResultTest_SkipsFileWriting()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var path = Path.Combine(td, "project.lock.json");
                var cachePath = Path.Combine(td, "project.csproj.nuget.cache");
                var logger = new TestLogger();
                var result = new NoOpRestoreResult(
                    success: true,
                    lockFilePath: path,
                    new Lazy<LockFile>(() => new LockFile()),
                    cacheFile: new CacheFile("NotSoRandomString"),
                    cacheFilePath: cachePath,
                    projectStyle: ProjectStyle.Unknown,
                    elapsedTime: TimeSpan.MinValue);

                // Act
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.Contains(
                    $"Assets file has not changed. Skipping assets file writing. Path: {path}",
                    logger.Messages);
                Assert.Contains(
                    $"No-Op restore. The cache will not be updated. Path: {cachePath}",
                    logger.VerboseMessages);

                Assert.False(File.Exists(path), $"The lock file should not have been written: {path}");
                Assert.False(File.Exists(cachePath), $"The cache file should not have been written: {cachePath}");
                Assert.Equal(2, logger.Messages.Count);
            }
        }

        [Fact]
        public void NoOpRestoreResult_IsLazy()
        {
            const string lockFileContent = @"{
  ""version"": 1,
  ""targets"": {
    "".NETPlatform,Version=v5.0"": {
      ""System.Runtime/4.0.10-beta-23008"": {
        ""compile"": {
          ""ref/dotnet/System.Runtime.dll"": {}
        }
      }
    }
  },
  ""libraries"": {
    ""System.Runtime/4.0.10-beta-23008"": {
      ""sha512"": ""JkGp8sCzxxRY1GS+p1SEk8WcaT8pu++/5b94ar2i/RaUN/OzkcGP/6OLFUxUf1uar75pUvotpiMawVt1dCEUVA=="",
      ""type"": ""Package"",
      ""files"": [
        ""_rels/.rels"",
        ""System.Runtime.nuspec"",
        ""License.rtf"",
        ""ref/dotnet/System.Runtime.dll"",
        ""ref/net451/_._"",
        ""lib/net451/_._"",
        ""ref/win81/_._"",
        ""lib/win81/_._"",
        ""ref/netcore50/System.Runtime.dll"",
        ""package/services/metadata/core-properties/cdec43993f064447a2d882cbfd022539.psmdcp"",
        ""[Content_Types].xml""
      ]
    }
  },
  ""projectFileDependencyGroups"": {
    """": [
      ""System.Runtime >= 4.0.10-beta-*""
    ],
    "".NETPlatform,Version=v5.0"": []
  }
}
";

            // Arrange
            using (var td = TestDirectory.Create())
            {
                var path = Path.Combine(td, "project.lock.json");
                File.WriteAllText(path, lockFileContent);
                var logger = new TestLogger();
                var result = new NoOpRestoreResult(
                    success: true,
                    lockFilePath: path,
                    new Lazy<LockFile>(() => LockFileUtilities.GetLockFile(path, logger)),
                    cacheFile: new CacheFile("NotSoRandomString"),
                    cacheFilePath: null,
                    projectStyle: ProjectStyle.Unknown,
                    elapsedTime: TimeSpan.MinValue);

                // Act
                var actual = result.LockFile;

                // Assert
                Assert.Equal(1, actual.Libraries.Count);
                Assert.Equal("System.Runtime", actual.Libraries[0].Name);
            }
        }

        [Fact]
        public async Task RestoreResult_Commit_WritesDependencyGraphSpec()
        {
            // Arrange
            using (var td = TestDirectory.Create())
            {
                var path = Path.Combine(td, "project.assets.json");
                var cachePath = Path.Combine(td, "project.csproj.nuget.cache");
                var dgSpecPath = Path.Combine(td, "project1.nuget.g.dgspec.json");
                var dgSpec = new DependencyGraphSpec();
                var configJson = @"
                {
                    ""dependencies"": {
                    },
                     ""frameworks"": {
                        ""net45"": { }
                    }
                }";

                var spec = JsonPackageSpecReader.GetPackageSpec(configJson, "TestProject", Path.Combine(td, "project.csproj")).WithTestRestoreMetadata();
                dgSpec.AddProject(spec);
                dgSpec.AddRestore(spec.Name);

                var logger = new TestLogger();
                var result = new RestoreResult(
                    success: true,
                    restoreGraphs: null,
                    compatibilityCheckResults: null,
                    lockFile: new LockFile(),
                    previousLockFile: null, // different lock file
                    lockFilePath: path,
                    msbuildFiles: Enumerable.Empty<MSBuildOutputFile>(),
                    cacheFile: new CacheFile("NotSoRandomString"),
                    cacheFilePath: cachePath,
                    packagesLockFilePath: null,
                    packagesLockFile: null,
                    dependencyGraphSpecFilePath: dgSpecPath,
                    dependencyGraphSpec: dgSpec,
                    projectStyle: ProjectStyle.Unknown,
                    elapsedTime: TimeSpan.MinValue);

                // Act
                await result.CommitAsync(logger, CancellationToken.None);

                // Assert
                Assert.Empty(logger.MinimalMessages);
                Assert.Contains(
                    $"Persisting dg to {dgSpecPath}",
                    logger.VerboseMessages);
                Assert.True(File.Exists(dgSpecPath));
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CommitAsync_WithExistingDGSpecAndDidDGHashChange_WritesDependencyGraphSpecWhenDGHashChanged(bool didDGHashChange)
        {
            // Arrange
            using var td = TestDirectory.Create();
            var path = Path.Combine(td, "project.assets.json");
            var cachePath = Path.Combine(td, "project.csproj.nuget.cache");
            var dgSpecPath = Path.Combine(td, "project1.nuget.g.dgspec.json");
            File.WriteAllText(dgSpecPath, "{}");

            var dgSpec = new DependencyGraphSpec();
            var configJson = @"
                {
                    ""dependencies"": {
                    },
                     ""frameworks"": {
                        ""net45"": { }
                    }
                }";

            var spec = JsonPackageSpecReader.GetPackageSpec(configJson, "TestProject", Path.Combine(td, "project.csproj")).WithTestRestoreMetadata();
            dgSpec.AddProject(spec);
            dgSpec.AddRestore(spec.Name);

            var logger = new TestLogger();
            var result = new RestoreResult(
                success: true,
                restoreGraphs: null,
                compatibilityCheckResults: null,
                lockFile: new LockFile(),
                previousLockFile: null, // different lock file
                lockFilePath: path,
                msbuildFiles: Enumerable.Empty<MSBuildOutputFile>(),
                cacheFile: new CacheFile("NotSoRandomString"),
                cacheFilePath: cachePath,
                packagesLockFilePath: null,
                packagesLockFile: null,
                dependencyGraphSpecFilePath: dgSpecPath,
                dependencyGraphSpec: dgSpec,
                projectStyle: ProjectStyle.Unknown,
                elapsedTime: TimeSpan.MinValue)
            {
                DidDGHashChange = didDGHashChange
            };

            // Act
            await result.CommitAsync(logger, CancellationToken.None);

            // Assert
            Assert.Empty(logger.MinimalMessages);
            Assert.Equal(didDGHashChange, logger.VerboseMessages.Contains($"Persisting dg to {dgSpecPath}"));
            Assert.True(File.Exists(dgSpecPath));
        }

        [Fact]
        public void WhenRestoreResult_LogMessagesAreSourcedFromTheAssetsFile()
        {
            // Arrange
            var expectedLogLevel = NuGetLogCode.NU1500;
            var assetsLogMessage = new AssetsLogMessage(LogLevel.Error, expectedLogLevel, "a");
            var cacheLogMessage = new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1501, "a");

            var lockFile = new LockFile();
            lockFile.LogMessages.Add(assetsLogMessage);

            var cacheFile = new CacheFile("hash")
            {
                Success = true,
                LogMessages = new List<IAssetsLogMessage>() { cacheLogMessage },
            };

            // Act 
            var result = new RestoreResult(
                success: true,
                restoreGraphs: null,
                compatibilityCheckResults: null,
                lockFile: lockFile,
                previousLockFile: null,
                lockFilePath: "project.assets.json",
                msbuildFiles: Enumerable.Empty<MSBuildOutputFile>(),
                cacheFile: cacheFile,
                cacheFilePath: null,
                packagesLockFilePath: null,
                packagesLockFile: null,
                dependencyGraphSpecFilePath: null,
                dependencyGraphSpec: null,
                projectStyle: ProjectStyle.PackageReference,
                elapsedTime: TimeSpan.MinValue);

            // Assert
            result.LogMessages.Should().NotBeNull();
            result.LogMessages.Should().HaveCount(1);
            result.LogMessages.Single().Code.Should().Be(expectedLogLevel);
        }

        [Fact]
        public void WhenNoOpRestoreResult_LogMessagesAreSourcedFromTheCacheFile()
        {
            // Arrange
            var expectedLogLevel = NuGetLogCode.NU1500;
            var assetsLogMessage = new AssetsLogMessage(LogLevel.Error, NuGetLogCode.NU1501, "a");
            var cacheLogMessage = new AssetsLogMessage(LogLevel.Error, expectedLogLevel, "a");

            var lockFile = new LockFile();
            lockFile.LogMessages.Add(assetsLogMessage);

            var cacheFile = new CacheFile("hash")
            {
                Success = true,
                LogMessages = new List<IAssetsLogMessage>() { cacheLogMessage },
            };

            // Act
            var result = new NoOpRestoreResult(
                   success: true,
                   lockFilePath: "project.assets.json",
                   new Lazy<LockFile>(() => lockFile),
                   cacheFile: cacheFile,
                   cacheFilePath: "cachepath",
                   projectStyle: ProjectStyle.PackageReference,
                   elapsedTime: TimeSpan.MinValue);

            // Assert
            result.LogMessages.Should().NotBeNull();
            result.LogMessages.Should().HaveCount(1);
            result.LogMessages.Single().Code.Should().Be(expectedLogLevel);
        }

        [Fact]
        public void WhenRestoreResult_WithNullAssetsFile_LogMessagesAreEmpty()
        {
            // Arrange & Act
            var result = new RestoreResult(
                success: true,
                restoreGraphs: null,
                compatibilityCheckResults: null,
                lockFile: null,
                previousLockFile: null,
                lockFilePath: "project.assets.json",
                msbuildFiles: Enumerable.Empty<MSBuildOutputFile>(),
                cacheFile: null,
                cacheFilePath: null,
                packagesLockFilePath: null,
                packagesLockFile: null,
                dependencyGraphSpecFilePath: null,
                dependencyGraphSpec: null,
                projectStyle: ProjectStyle.PackageReference,
                elapsedTime: TimeSpan.MinValue);

            // Assert
            result.LogMessages.Should().NotBeNull();
            result.LogMessages.Should().HaveCount(0);
        }

        [Fact]
        public void WhenNoOpRestoreResult_WithNullCacheFile_LogMessagesAreEmpty()
        {
            // Arrange & Act
            var result = new NoOpRestoreResult(
                   success: true,
                   lockFilePath: "project.assets.json",
                   new Lazy<LockFile>(() => null),
                   cacheFile: null,
                   cacheFilePath: "cachepath",
                   projectStyle: ProjectStyle.PackageReference,
                   elapsedTime: TimeSpan.MinValue);

            // Assert
            result.LogMessages.Should().NotBeNull();
            result.LogMessages.Should().HaveCount(0);
        }
    }
}

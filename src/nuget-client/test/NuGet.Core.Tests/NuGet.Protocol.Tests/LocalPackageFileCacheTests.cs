// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class LocalPackageFileCacheTests
    {
        [Fact]
        public async Task LocalPackageFileCache_GetFilesTwiceVerifySameInstance()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var cache = new LocalPackageFileCache();
                var pathResolver = new VersionFolderPathResolver(pathContext.PackageSource);

                var identity = new PackageIdentity("X", NuGetVersion.Parse("1.0.0"));
                var path = pathResolver.GetInstallPath(identity.Id, identity.Version);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    identity);

                var filesA = cache.GetOrAddFiles(path);
                var filesB = cache.GetOrAddFiles(path);

                // Verify both file lists are the exact same instance
                Assert.Same(filesA.Value, filesB.Value);
                filesA.Value.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task LocalPackageFileCache_GetShaTwiceVerifyMissingFilesAreNotCached()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var cache = new LocalPackageFileCache();
                var pathResolver = new VersionFolderPathResolver(pathContext.PackageSource);

                var identity = new PackageIdentity("X", NuGetVersion.Parse("1.0.0"));
                var shaPath = pathResolver.GetNupkgMetadataPath(identity.Id, identity.Version);

                var exists1 = cache.Sha512Exists(shaPath);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    identity);

                var exists2 = cache.Sha512Exists(shaPath);
                var sha512 = cache.GetOrAddSha512(shaPath);
                var sha512B = cache.GetOrAddSha512(shaPath);

                // Verify original value was not found
                exists1.Should().BeFalse();

                // Verify false was not cached
                exists2.Should().BeTrue();

                // Verify both hashes are the exact same instance
                Assert.Same(sha512.Value, sha512B.Value);
            }
        }

        [Fact]
        public async Task LocalPackageFileCache_GetNuspecTwiceVerifySameInstance()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var cache = new LocalPackageFileCache();
                var pathResolver = new VersionFolderPathResolver(pathContext.PackageSource);

                var identity = new PackageIdentity("X", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    identity);

                var nuspec = pathResolver.GetManifestFilePath(identity.Id, identity.Version);
                var expanded = pathResolver.GetInstallPath(identity.Id, identity.Version);

                var result1 = cache.GetOrAddNuspec(nuspec, expanded);
                var result2 = cache.GetOrAddNuspec(nuspec, expanded);

                Assert.Same(result1.Value, result2.Value);
                result1.Value.GetIdentity().Should().Be(identity);
            }
        }

        [Fact]
        public async Task LocalPackageFileCache_FallbackToFolderReaderVerifyResult()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var cache = new LocalPackageFileCache();
                var pathResolver = new VersionFolderPathResolver(pathContext.PackageSource);

                var identity = new PackageIdentity("X", NuGetVersion.Parse("1.0.0"));

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    identity);

                var nuspec = pathResolver.GetManifestFilePath(identity.Id, identity.Version) + "invalid.nuspec";
                var expanded = pathResolver.GetInstallPath(identity.Id, identity.Version);

                var result = cache.GetOrAddNuspec(nuspec, expanded);

                result.Value.GetIdentity().Should().Be(identity);
            }
        }

        [Fact]
        public void LocalPackageFileCache_NuspecNotFoundVerifyFailure()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var cache = new LocalPackageFileCache();
                var pathResolver = new VersionFolderPathResolver(pathContext.PackageSource);

                var identity = new PackageIdentity("X", NuGetVersion.Parse("1.0.0"));

                var nuspec = pathResolver.GetManifestFilePath(identity.Id, identity.Version);
                var expanded = pathResolver.GetInstallPath(identity.Id, identity.Version);
                Directory.CreateDirectory(expanded);

                // Verify does not throw
                var result = cache.GetOrAddNuspec(nuspec, expanded);

                // This should throw
                Assert.Throws<PackagingException>(() => result.Value);
            }
        }

        [Fact]
        public void LocalPackageFileCache_DirNotFoundVerifyFailure()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var cache = new LocalPackageFileCache();
                var pathResolver = new VersionFolderPathResolver(pathContext.PackageSource);

                var identity = new PackageIdentity("X", NuGetVersion.Parse("1.0.0"));

                var nuspec = pathResolver.GetManifestFilePath(identity.Id, identity.Version);
                var expanded = pathResolver.GetInstallPath(identity.Id, identity.Version);

                // Verify does not throw
                var result = cache.GetOrAddNuspec(nuspec, expanded);

                // This should throw
                Assert.Throws<DirectoryNotFoundException>(() => result.Value);
            }
        }

        [Fact]
        public async Task LocalPackageFileCache_UpdateLastAccessTimestampVerifyOnce()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                var cache = new LocalPackageFileCache();
                var pathResolver = new VersionFolderPathResolver(pathContext.PackageSource);

                var identity = new PackageIdentity("X", NuGetVersion.Parse("1.0.0"));
                var path = pathResolver.GetInstallPath(identity.Id, identity.Version);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    identity);

                var metadataPath = pathResolver.GetNupkgMetadataPath(identity.Id, identity.Version);

                cache.UpdateLastAccessTime(metadataPath);

                var lastAccess = File.GetLastAccessTimeUtc(metadataPath);

                cache.UpdateLastAccessTime(metadataPath);

                // Verify the last access timestamp was cached
                Assert.Equal(lastAccess, File.GetLastAccessTimeUtc(metadataPath));
            }
        }
    }
}

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.CommandLine.XPlat.Tests
{
    public class InstallPackagesTests
    {
        [Fact]
        public async Task InstallPackageFromAnotherProcessVerifyCacheIsClearedAsync()
        {
            // Arrange
            var logger = new TestLogger();

            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var tfi = new List<TargetFrameworkInformation>
                {
                    new TargetFrameworkInformation()
                    {
                        FrameworkName = NuGetFramework.Parse("net462")
                    }
                };

                var spec = NETCoreRestoreTestUtility.GetProject(projectName: "projectA", framework: "net46");
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("a", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                var project = NETCoreRestoreTestUtility.CreateProjectsFromSpecs(pathContext, spec).Single();

                var packageA = new SimpleTestPackageContext("a");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageA);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);

                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var providerCache = new RestoreCommandProvidersCache();
                var sources = new List<string>() { pathContext.PackageSource };

                var restoreContext = new RestoreArgs()
                {
                    CacheContext = cacheContext,
                    DisableParallel = true,
                    GlobalPackagesFolder = pathContext.UserPackagesFolder,
                    Sources = sources,
                    Log = logger,
                    CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(new List<PackageSource>() { new PackageSource(pathContext.PackageSource) })),
                    PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                    {
                        new DependencyGraphSpecRequestProvider(providerCache, dgFile)
                    }
                };

                var request = (await RestoreRunner.GetRequests(restoreContext)).Single();
                var providers = providerCache.GetOrCreate(pathContext.UserPackagesFolder, sources, new List<SourceRepository>(), cacheContext, logger, false);
                var command = new NuGet.Commands.RestoreCommand(request.Request);

                // Add to cache before install on all providers
                var globalPackages = providers.GlobalPackages;
                var packages = globalPackages.FindPackagesById("a");
                packages.Should().BeEmpty("has not been installed yet");

                foreach (var local in providers.LocalProviders)
                {
                    await local.GetDependenciesAsync(new LibraryIdentity("a", NuGetVersion.Parse("1.0.0"), LibraryType.Package), NuGetFramework.Parse("net46"), cacheContext, logger, CancellationToken.None);
                }

                // Install the package without updating the cache
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(pathContext.UserPackagesFolder, PackageSaveMode.Defaultv3, packageA);

                // Run restore using an incorrect cache
                var result = await command.ExecuteAsync();

                // Verify a is in the output assets file
                result.Success.Should().BeTrue();
                result.LockFile.GetLibrary("a", new NuGetVersion(1, 0, 0)).Should().NotBeNull();
            }
        }
    }
}

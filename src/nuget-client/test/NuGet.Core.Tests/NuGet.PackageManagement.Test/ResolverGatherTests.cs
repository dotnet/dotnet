// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.Test
{
    public class ResolverGatherTests
    {
        [Fact]
        public async Task ResolverGather_TimeoutFromPrimaryRepositoryThrows()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var primaryRepo = CreateTimeoutRepo("primary");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("a", "2.0.0")
                };

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));
            repos.Add(primaryRepo);

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0")
                };

            var context = new GatherContext()
            {
                PrimaryTargets = targets.ToList(),
                InstalledPackages = installedPackages,
                TargetFramework = framework,
                PrimarySources = new List<SourceRepository>() { primaryRepo },
                AllSources = repos,
                PackagesFolderSource = CreateRepo("installed", new List<SourcePackageDependencyInfo>()),
                ResolutionContext = new ResolutionContext()
            };

            var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

            // Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await ResolverGather.GatherAsync(context, cts.Token);
            });
        }

        [Fact]
        public async Task ResolverGather_TimeoutFromSecondaryRepositoryIgnored()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var secondaryRepo = CreateTimeoutRepo("secondary");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("a", "2.0.0")
                };

            var allRepos = new List<SourceRepository>();
            allRepos.Add(CreateRepo("a", repoA));
            allRepos.Add(secondaryRepo);

            var primaryRepos = new List<SourceRepository>();
            primaryRepos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0")
                };

            var context = new GatherContext()
            {
                PrimaryTargets = targets.ToList(),
                InstalledPackages = installedPackages,
                TargetFramework = framework,
                AllSources = allRepos,
                PrimarySources = primaryRepos,
                PackagesFolderSource = CreateRepo("installed", new List<SourcePackageDependencyInfo>()),
                ResolutionContext = new ResolutionContext()
            };

            var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(5000));

            // Act
            var results = await ResolverGather.GatherAsync(context, cts.Token);

            // Assert
            Assert.Equal(1, results.Count);
        }

        [Fact]
        public async Task ResolverGather_ThrowExceptionFromSecondaryRepositoryIgnored()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var secondaryRepo = CreateThrowingRepo("secondary", new InvalidOperationException("failed"));

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("a", "2.0.0")
                };

            var allRepos = new List<SourceRepository>();
            allRepos.Add(CreateRepo("a", repoA));
            allRepos.Add(secondaryRepo);

            var primaryRepos = new List<SourceRepository>();
            primaryRepos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0")
                };

            var context = new GatherContext()
            {
                PrimaryTargets = targets.ToList(),
                InstalledPackages = installedPackages,
                TargetFramework = framework,
                AllSources = allRepos,
                PrimarySources = primaryRepos,
                PackagesFolderSource = CreateRepo("installed", new List<SourcePackageDependencyInfo>()),
                ResolutionContext = new ResolutionContext()
            };

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            // Assert
            Assert.Equal(1, results.Count);
        }

        [Fact]
        public async Task ResolverGather_ThrowExceptionFromPrimaryRepository()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var primaryRepo = CreateThrowingRepo("primary", new InvalidOperationException("failed"));

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("a", "2.0.0")
                };

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0")
                };

            var context = new GatherContext()
            {
                PrimaryTargets = targets.ToList(),
                InstalledPackages = installedPackages,
                TargetFramework = framework,
                PrimarySources = new List<SourceRepository>() { primaryRepo },
                AllSources = repos,
                PackagesFolderSource = CreateRepo("installed", new List<SourcePackageDependencyInfo>()),
                ResolutionContext = new ResolutionContext()
            };

            // Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await ResolverGather.GatherAsync(context, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ResolverGather_VerifyCacheWorksWithCachedNulls()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>();
            var repoB = new List<SourcePackageDependencyInfo>();
            var repoInstalled = new List<SourcePackageDependencyInfo>();

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));
            repos.Add(CreateRepo("b", repoB));

            var installedPackages = new List<PackageIdentity>();

            // this contains the cache
            var resolutionContext = new ResolutionContext();

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = resolutionContext;

            var contextAOnly = new GatherContext();
            contextAOnly.PrimaryTargets = targets.ToList();
            contextAOnly.InstalledPackages = installedPackages;
            contextAOnly.TargetFramework = framework;
            contextAOnly.PrimarySources = primaryRepo;
            contextAOnly.AllSources = repos;
            contextAOnly.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            contextAOnly.ResolutionContext = resolutionContext;

            // Run the first time
            try
            {
                await ResolverGather.GatherAsync(context, CancellationToken.None);
            }
            catch (InvalidOperationException)
            {

            }

            // Act
            // Run again
            Exception result = null;

            try
            {
                await ResolverGather.GatherAsync(contextAOnly, CancellationToken.None);
            }
            catch (InvalidOperationException ex)
            {
                result = ex;
            }

            // Assert
            // If this fails there will be a null ref or argument exception
            Assert.Contains("Package 'a 2.0.0' is not found", result.Message);
        }

        [Fact]
        public async Task ResolverGather_VerifyCacheIsUsed()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "2.0.0", "b"),
                };

            var repoB = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("b", "2.0.0"),
                };

            var repoBEmpty = new List<SourcePackageDependencyInfo>
            {

            };

            var repoInstalled = new List<SourcePackageDependencyInfo>()
            {

            };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));
            repos.Add(CreateRepo("b", repoB));

            var reposAOnly = new List<SourceRepository>();
            reposAOnly.Add(CreateRepo("a", repoA));
            reposAOnly.Add(CreateRepo("b", repoBEmpty));

            var installedPackages = new List<PackageIdentity>
            {

            };

            // this contains the cache
            var resolutionContext = new ResolutionContext();

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = resolutionContext;

            var contextAOnly = new GatherContext();
            contextAOnly.PrimaryTargets = targets.ToList();
            contextAOnly.InstalledPackages = installedPackages;
            contextAOnly.TargetFramework = framework;
            contextAOnly.PrimarySources = primaryRepo;
            contextAOnly.AllSources = reposAOnly;
            contextAOnly.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            contextAOnly.ResolutionContext = resolutionContext;

            // Run the first time
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            // Act
            // Run again
            results = await ResolverGather.GatherAsync(contextAOnly, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(2, check.Count);
            Assert.Equal("a", check[0].Key);

            // B can only come from repoB, which should only be in the cache
            Assert.Equal("b", check[1].Key);
        }

        [Fact]
        public async Task ResolverGather_VerifyPackageMissingWithNoCache()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "2.0.0", "b"),
                };

            var repoB = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("b", "2.0.0"),
                };

            var repoBEmpty = new List<SourcePackageDependencyInfo>
            {

            };

            var repoInstalled = new List<SourcePackageDependencyInfo>()
            {

            };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));
            repos.Add(CreateRepo("b", repoB));

            var reposAOnly = new List<SourceRepository>();
            reposAOnly.Add(CreateRepo("a", repoA));
            reposAOnly.Add(CreateRepo("b", repoBEmpty));

            var installedPackages = new List<PackageIdentity>
            {

            };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = new ResolutionContext();

            var contextAOnly = new GatherContext();
            contextAOnly.PrimaryTargets = targets.ToList();
            contextAOnly.InstalledPackages = installedPackages;
            contextAOnly.TargetFramework = framework;
            contextAOnly.PrimarySources = primaryRepo;
            contextAOnly.AllSources = reposAOnly;
            contextAOnly.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            contextAOnly.ResolutionContext = new ResolutionContext();

            // Run the first time
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            // Act
            // Run again
            results = await ResolverGather.GatherAsync(contextAOnly, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(1, check.Count);
            Assert.Equal("a", check[0].Key);
        }

        [Fact]
        public async Task ResolverGather_MissingInstalledPackageFromPackagesFolder()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("a", "2.0.0"),
                    CreateDependencyInfo("b", "2.0.0"),
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>()
            {
                // missing packages
            };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "2.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(2, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal("b", check[1].Key);
        }

        [Fact]
        public async Task ResolverGather_IgnoreDependenciesSetToTrueShouldSkipChildren()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0", "b"),
                    CreateDependencyInfo("a", "2.0.0", "c"),
                    CreateDependencyInfo("b", "2.0.0"),
                    CreateDependencyInfo("c", "2.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>()
            {
                CreateDependencyInfo("a", "1.0.0", "b"),
                CreateDependencyInfo("b", "2.0.0"),
            };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "2.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.AllowDowngrades = false;
            context.ResolutionContext = new ResolutionContext(DependencyBehavior.Ignore, true, true, VersionConstraints.None);

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(2, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal("b", check[1].Key);
            // Skip C
        }


        [Fact]
        public async Task ResolverGather_AllowDowngradesTrueShouldIncludeDowngradeDependencies()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0", "b"),
                    CreateDependencyInfo("a", "2.0.0", "b"),
                    CreateDependencyInfo("a", "3.0.0", "b"),
                    CreateDependencyInfo("b", "1.0.0", "c"), // should be ignored
                    CreateDependencyInfo("b", "2.0.0"),
                    CreateDependencyInfo("c", "2.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>()
            {
                // missing packages
            };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "2.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.AllowDowngrades = true;
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(3, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal("b", check[1].Key);
            Assert.Equal("c", check[2].Key);
        }

        [Fact]
        public async Task ResolverGather_AllowDowngradesFalseShouldIgnoreDowngradeDependencies()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0", "b"),
                    CreateDependencyInfo("a", "2.0.0", "b"),
                    CreateDependencyInfo("a", "3.0.0", "b"),
                    CreateDependencyInfo("b", "1.0.0", "c"), // should be ignored
                    CreateDependencyInfo("b", "2.0.0"),
                    CreateDependencyInfo("c", "2.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>()
            {
                // missing packages
            };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "2.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.AllowDowngrades = false;
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(2, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal("b", check[1].Key);
            // c should not be collected
        }

        [Fact]
        public async Task ResolverGather_MissingPrimaryPackage()
        {
            // Arrange
            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("a", "3.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>()
            {
                // missing packages
            };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = new ResolutionContext();

            // Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                {
                    try
                    {
                        await ResolverGather.GatherAsync(context, CancellationToken.None);
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                });
        }

        [Fact]
        public async Task ResolverGather_UpdateAllWithMissingPrimaryPackage()
        {
            // Arrange
            var targetA = CreatePackage("a", "2.0.0");
            var targetB = CreatePackage("b", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { targetA, targetB };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("a", "2.0.0")
                };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "1.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.IsUpdateAll = true;
            context.PackagesFolderSource = CreateRepo("installed", new List<SourcePackageDependencyInfo>());
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            // Assert
            Assert.Equal(1, results.Count);
        }

        [Fact]
        public async Task ResolverGather_MissingPackageGatheredFromSource()
        {
            // Arrange

            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("a", "2.0.0"),
                    CreateDependencyInfo("a", "3.0.0"),
                    CreateDependencyInfo("b", "1.0.0"),
                    CreateDependencyInfo("b", "2.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>()
            {
                // missing packages
            };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "1.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(2, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal(1, check[0].Count());
            Assert.Equal("b", check[1].Key);
            Assert.Equal(1, check[1].Count());
        }

        [Fact]
        public async Task ResolverGather_VerifyUnrelatedPackageIsIgnored()
        {
            // Arrange

            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("a", "2.0.0"),
                    CreateDependencyInfo("a", "3.0.0"),
                    CreateDependencyInfo("b", "1.0.0"),
                    CreateDependencyInfo("b", "2.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0"),
                    CreateDependencyInfo("b", "1.0.0")
                };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "1.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(2, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal(1, check[0].Count());
            Assert.Equal("b", check[1].Key);
            Assert.Equal(1, check[1].Count());
        }

        [Fact]
        public async Task ResolverGather_VerifyParentDependencyIsExpanded()
        {
            // Arrange

            var target = CreatePackage("c", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "2.0.0", "d"),
                    CreateDependencyInfo("a", "1.0.0", "b"),
                    CreateDependencyInfo("b", "1.0.0", "c"),
                    CreateDependencyInfo("b", "2.0.0", "c"),
                    CreateDependencyInfo("c", "1.0.0"),
                    CreateDependencyInfo("c", "2.0.0"),
                    CreateDependencyInfo("d", "1.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0", "b"),
                    CreateDependencyInfo("b", "1.0.0", "c"),
                    CreateDependencyInfo("c", "1.0.0")
                };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "1.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(4, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal("b", check[1].Key);
            Assert.Equal("c", check[2].Key);
            Assert.Equal("d", check[3].Key);
        }

        [Fact]
        public async Task ResolverGather_VerifyDependencyIsExpanded()
        {
            // Arrange

            var target = CreatePackage("a", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "2.0.0", "b"),
                    CreateDependencyInfo("a", "1.0.0", "b"),
                    CreateDependencyInfo("b", "1.0.0"),
                    CreateDependencyInfo("b", "2.0.0", "c"),
                    CreateDependencyInfo("c", "2.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0", "b"),
                    CreateDependencyInfo("b", "1.0.0")
                };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "1.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(3, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal("b", check[1].Key);

            // when a is updated, b is expanded and c is found
            Assert.Equal("c", check[2].Key);
        }

        [Fact]
        public async Task ResolverGather_VerifyParentIsExpanded()
        {
            // Arrange

            var target = CreatePackage("b", "2.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "2.0.0", "b", "c"),
                    CreateDependencyInfo("a", "1.0.0", "b"),
                    CreateDependencyInfo("b", "1.0.0"),
                    CreateDependencyInfo("b", "2.0.0"),
                    CreateDependencyInfo("c", "2.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0", "b"),
                    CreateDependencyInfo("b", "1.0.0")
                };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("a", "1.0.0"),
                    CreatePackage("b", "1.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(3, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal("b", check[1].Key);

            // when update b, a is expanded which then retrieves c
            Assert.Equal("c", check[2].Key);
        }

        [Fact]
        public async Task ResolverGather_ComplexGraphNeedingMultiplePasses()
        {
            // Arrange

            var target = CreatePackage("a", "1.0.0");
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var repoA = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("a", "1.0.0", "b", "d"),
                    CreateDependencyInfo("a", "2.0.0", "z"),
                    CreateDependencyInfo("b", "1.0.0", "c"),
                    CreateDependencyInfo("b", "2.0.0", "c"),
                    CreateDependencyInfo("c", "1.0.0"),
                    CreateDependencyInfo("d", "1.0.0", "f"),
                    CreateDependencyInfo("f", "1.0.0"),
                    CreateDependencyInfo("g", "1.0.0"),
                    CreateDependencyInfo("g", "2.0.0"),
                    CreateDependencyInfo("c", "2.0.0"),
                    CreateDependencyInfo("j", "2.0.0"),
                    CreateDependencyInfo("z", "1.0.0"),
                    CreateDependencyInfo("g", "1.0.0", "a"),
                    CreateDependencyInfo("h", "1.0.0", "c", "g"),
                    CreateDependencyInfo("h", "2.0.0", "c"),
                    CreateDependencyInfo("h", "3.0.0", "j"),
                    CreateDependencyInfo("i", "1.0.0", "a", "b"),
                    CreateDependencyInfo("y", "1.0.0", "c"),
                    CreateDependencyInfo("y", "2.0.0", "c")
                };

            var repoB = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("b", "3.0.0", "c"),
                    CreateDependencyInfo("h", "3.0.0", "c"),
                    CreateDependencyInfo("c", "3.0.0"),
                    CreateDependencyInfo("g", "1.0.0")
                };

            var repoInstalled = new List<SourcePackageDependencyInfo>
                {
                    CreateDependencyInfo("y", "1.0.0", "c"),
                    CreateDependencyInfo("c", "1.0.0"),
                    CreateDependencyInfo("h", "1.0.0", "c", "g"),
                    CreateDependencyInfo("x", "1.0.0"),
                    CreateDependencyInfo("g", "1.0.0")
                };

            var primaryRepo = new List<SourceRepository>();
            primaryRepo.Add(CreateRepo("a", repoA));

            var repos = new List<SourceRepository>();
            repos.Add(CreateRepo("a", repoA));
            repos.Add(CreateRepo("b", repoB));

            var installedPackages = new List<PackageIdentity>
                {
                    CreatePackage("y", "1.0.0"),
                    CreatePackage("c", "1.0.0"),
                    CreatePackage("h", "1.0.0"),
                    CreatePackage("g", "1.0.0")
                };

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = installedPackages;
            context.TargetFramework = framework;
            context.PrimarySources = primaryRepo;
            context.AllSources = repos;
            context.PackagesFolderSource = CreateRepo("installed", repoInstalled);
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.GroupBy(e => e.Id).OrderBy(e => e.Key).ToList();

            // Assert
            Assert.Equal(9, check.Count);
            Assert.Equal("a", check[0].Key);
            Assert.Equal(1, check[0].Count());
            Assert.Equal("b", check[1].Key);
            Assert.Equal(3, check[1].Count());
            Assert.Equal("c", check[2].Key);
            Assert.Equal(3, check[2].Count());
            Assert.Equal("d", check[3].Key);
            Assert.Equal(1, check[3].Count());
            Assert.Equal("f", check[4].Key);
            Assert.Equal(1, check[4].Count());
            Assert.Equal("g", check[5].Key);
            Assert.Equal(2, check[5].Count());
            Assert.Equal("h", check[6].Key);
            Assert.Equal(3, check[6].Count());
            Assert.Equal("j", check[7].Key);
            Assert.Equal(1, check[7].Count());
            Assert.Equal("y", check[8].Key);
            Assert.Equal(2, check[8].Count());
        }

        /// <summary>
        /// Verify packages can be found across repos
        /// </summary>
        [Fact]
        public async Task ResolverGather_Basic()
        {
            // Arrange

            var target = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var packagesA = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("d", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("e", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                    new SourcePackageDependencyInfo("notpartofthis", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var packagesB = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("notpartofthis2", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var providersA = new List<Lazy<INuGetResourceProvider>>();
            providersA.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packagesA)));

            var providersB = new List<Lazy<INuGetResourceProvider>>();
            providersB.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packagesB)));

            var providersC = new List<Lazy<INuGetResourceProvider>>();
            providersC.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(new List<SourcePackageDependencyInfo>())));

            var repos = new List<SourceRepository>();
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://a"), providersA));
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://b"), providersB));
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://c"), providersC));

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = new List<PackageIdentity>();
            context.TargetFramework = framework;
            context.PrimarySources = repos;
            context.AllSources = repos;
            context.PackagesFolderSource = repos[2];
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.OrderBy(e => e.Id).ToList();

            // Assert
            Assert.Equal(5, check.Count);
            Assert.Equal("a", check[0].Id);
            Assert.Equal("b", check[1].Id);
            Assert.Equal("c", check[2].Id);
            Assert.Equal("d", check[3].Id);
            Assert.Equal("e", check[4].Id);
        }

        /// <summary>
        /// Verify packages can be found across repos
        /// </summary>
        [Fact]
        public async Task ResolverGather_BasicGatherWithExtraPackages()
        {
            // Arrange

            var target = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var packagesA = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("d", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("e", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                    new SourcePackageDependencyInfo("d", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("e", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("notpartofthis", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var packagesB = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("notpartofthis2", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var providersC = new List<Lazy<INuGetResourceProvider>>();
            providersC.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(new List<SourcePackageDependencyInfo>())));

            var providersA = new List<Lazy<INuGetResourceProvider>>();
            providersA.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packagesA)));

            var providersB = new List<Lazy<INuGetResourceProvider>>();
            providersB.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packagesB)));

            var repos = new List<SourceRepository>();
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://a"), providersA));
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://b"), providersB));
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://c"), providersC));

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = new List<PackageIdentity>();
            context.TargetFramework = framework;
            context.PrimarySources = repos;
            context.AllSources = repos;
            context.PackagesFolderSource = repos[2];
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.OrderBy(e => e.Id).ToList();

            // Assert
            Assert.Equal(5, check.Count);
            Assert.Equal("a", check[0].Id);
            Assert.Equal("b", check[1].Id);
            Assert.Equal("c", check[2].Id);
            Assert.Equal("d", check[3].Id);
            Assert.Equal("e", check[4].Id);
        }

        /// <summary>
        /// Verify packages can be found across repos
        /// </summary>
        [Fact]
        public async Task ResolverGather_GatherWithNotFoundPackages()
        {
            // Arrange

            var target = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var packagesA = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("d", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("e", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null),
                    new SourcePackageDependencyInfo("notpartofthis", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var packagesB = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null),
                    new SourcePackageDependencyInfo("notpartofthis2", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var providersA = new List<Lazy<INuGetResourceProvider>>();
            providersA.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packagesA)));

            var providersB = new List<Lazy<INuGetResourceProvider>>();
            providersB.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packagesB)));

            var providersC = new List<Lazy<INuGetResourceProvider>>();
            providersC.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(new List<SourcePackageDependencyInfo>())));

            var repos = new List<SourceRepository>();
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://a"), providersA));
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://b"), providersB));
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://c"), providersC));

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = new List<PackageIdentity>();
            context.TargetFramework = framework;
            context.PrimarySources = repos;
            context.AllSources = repos;
            context.PackagesFolderSource = repos[2];
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.OrderBy(e => e.Id).ToList();

            // Assert
            Assert.Equal(3, check.Count);
            Assert.Equal("a", check[0].Id);
            Assert.Equal("b", check[1].Id);
            Assert.Equal("c", check[2].Id);
        }

        /// <summary>
        /// Verify packages can be found across repos
        /// </summary>
        [Fact]
        public async Task ResolverGather_DependenciesSpreadAcrossRepos()
        {
            // Arrange
            var target = new PackageIdentity("a", new NuGetVersion(1, 0, 0));
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var packages1 = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo("c", new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var packages2 = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo("b", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("c", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null)
                };

            var packages3 = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo("a", new NuGetVersion(1, 0, 0), new[] { new Packaging.Core.PackageDependency("b", new VersionRange(new NuGetVersion(1, 0, 0))) }, true, null)
                };

            var providers1 = new List<Lazy<INuGetResourceProvider>>();
            providers1.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages1)));

            var providers2 = new List<Lazy<INuGetResourceProvider>>();
            providers2.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages2)));

            var providers3 = new List<Lazy<INuGetResourceProvider>>();
            providers3.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages3)));

            var providersPackagesFolder = new List<Lazy<INuGetResourceProvider>>();
            providersPackagesFolder.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(new List<SourcePackageDependencyInfo>())));

            var repos = new List<SourceRepository>();
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://1"), providers1));
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://2"), providers2));
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://3"), providers3));
            repos.Add(new SourceRepository(new Configuration.PackageSource("http://4"), providersPackagesFolder));

            var context = new GatherContext();
            context.PrimaryTargets = targets.ToList();
            context.InstalledPackages = new List<PackageIdentity>();
            context.TargetFramework = framework;
            context.PrimarySources = repos;
            context.AllSources = repos;
            context.PackagesFolderSource = repos[2];
            context.ResolutionContext = new ResolutionContext();

            // Act
            var results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            var check = results.OrderBy(e => e.Id).ToList();

            // Assert
            Assert.Equal(3, check.Count);
            Assert.Equal("a", check[0].Id);
            Assert.Equal("b", check[1].Id);
            Assert.Equal("c", check[2].Id);
        }

        /// <summary>
        /// Test package patterns filters are respected and succeeds.
        /// </summary>
        [Theory]
        [InlineData("public,Nuget", "Nuget")]
        [InlineData("public,nuget", "Nuget")]
        [InlineData("public,Nuget", "nuget")]
        [InlineData("public,nuget", "nuget")]
        [InlineData("public,Contoso.Opensource.*", "Contoso.Opensource.")]
        [InlineData("public,Contoso.Opensource.*", "Contoso.Opensource.MVC")]
        [InlineData("public,Contoso.Opensource.*", "Contoso.Opensource.MVC.ASP")]
        [InlineData("public,Contoso.Opensource.* ", "Contoso.Opensource.MVC.ASP")]
        [InlineData("nuget.org,nuget|privateRepository,private*", "nuget")]
        [InlineData("nuget.org,nuget|privateRepository,private*", "private1")]
        public async Task ResolverGather_PackageSourceMapping_Succeed(string packagePatterns, string packageId)
        {
            // Arrange
            var sourceMappingConfiguration = PackageSourceMappingUtility.GetPackageSourceMapping(packagePatterns);
            IReadOnlyList<string> configuredSources = sourceMappingConfiguration.GetConfiguredPackageSources(packageId);
            var target = new PackageIdentity(packageId, new NuGetVersion(1, 0, 0));
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var packages1 = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo(packageId, new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var packages2 = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo(packageId, new NuGetVersion(1, 0, 0),  new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var packages3 = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo(packageId, new NuGetVersion(1, 0, 0),  new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var providers1 = new List<Lazy<INuGetResourceProvider>>();
            providers1.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages1)));

            var providers2 = new List<Lazy<INuGetResourceProvider>>();
            providers2.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages2)));

            var providers3 = new List<Lazy<INuGetResourceProvider>>();
            providers3.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages3)));

            var repos = new List<SourceRepository>();
            repos.Add(new SourceRepository(new PackageSource("http://1", "someRepository1"), providers1));
            repos.Add(new SourceRepository(new PackageSource("http://2", "someRepository2"), providers2));
            repos.Add(new SourceRepository(new PackageSource("http://3", configuredSources[0]), providers3));

            var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };

            var context = new GatherContext(sourceMappingConfiguration)
            {
                PrimaryTargets = targets.ToList(),
                InstalledPackages = new List<PackageIdentity>(),
                TargetFramework = framework,
                PrimarySources = repos,
                AllSources = repos,
                PackagesFolderSource = repos[2], // Since it's configuredSource it'll succeed finding the packageId.
                ResolutionContext = new ResolutionContext(),
                ProjectContext = testNuGetProjectContext
            };

            // Act
            HashSet<SourcePackageDependencyInfo> results = await ResolverGather.GatherAsync(context, CancellationToken.None);

            List<SourcePackageDependencyInfo> check = results.OrderBy(e => e.Id).ToList();

            // Assert
            Assert.True(sourceMappingConfiguration.IsEnabled);
            Assert.Equal(1, configuredSources.Count);
            Assert.Equal(1, check.Count);
            // Only match to repository set in Package source mapping filter.
            Assert.Equal(configuredSources[0], check[0].Source.PackageSource.Name);

            // Assert log.
            Assert.Equal($"Package source mapping matches found for package ID '{packageId}' are: '{configuredSources[0]}' ", testNuGetProjectContext.Logs.Value[0]);
        }

        /// <summary>
        /// Test package source mapping filters are respected and fails if not found.
        /// </summary>
        [Theory]
        [InlineData("public,nuget", "nuge")]
        [InlineData("public,nuget", "nuget1")]
        [InlineData("public,Contoso.Opensource.*", "Cont")]
        [InlineData("public,Contoso.Opensource.*", "Contoso.Opensource")]
        [InlineData("nuget.org,nuget|privateRepository,private*", "nuge")]
        [InlineData("nuget.org,nuget|privateRepository,private*", "nuget1")]
        [InlineData("nuget.org,nuget|privateRepository,private*", "privat")]
        [InlineData("-", "privat")]
        [InlineData("public,nuget", "-")]
        public async Task ResolverGather_PackageSourceMapping_Fails(string packagePatterns, string packageId)
        {
            // Arrange
            var sourceMappingConfiguration = PackageSourceMappingUtility.GetPackageSourceMapping(packagePatterns);
            IReadOnlyList<string> configuredSources = sourceMappingConfiguration.GetConfiguredPackageSources(packageId);
            var target = new PackageIdentity(packageId, new NuGetVersion(1, 0, 0));
            IEnumerable<PackageIdentity> targets = new[] { target };

            var framework = NuGetFramework.Parse("net451");

            var packages1 = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo(packageId, new NuGetVersion(1, 0, 0), new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var packages2 = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo(packageId, new NuGetVersion(1, 0, 0),  new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var packages3 = new List<SourcePackageDependencyInfo>
                {
                    new SourcePackageDependencyInfo(packageId, new NuGetVersion(1, 0, 0),  new Packaging.Core.PackageDependency[] { }, true, null)
                };

            var providers1 = new List<Lazy<INuGetResourceProvider>>();
            providers1.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages1)));

            var providers2 = new List<Lazy<INuGetResourceProvider>>();
            providers2.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages2)));

            var providers3 = new List<Lazy<INuGetResourceProvider>>();
            providers3.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages3)));

            var repos = new List<SourceRepository>();
            repos.Add(new SourceRepository(new PackageSource("http://1", "nuget.org"), providers1));
            repos.Add(new SourceRepository(new PackageSource("http://2", "nuget.org"), providers2));
            repos.Add(new SourceRepository(new PackageSource("http://3", "privateRepository"), providers3));

            var testNuGetProjectContext = new TestNuGetProjectContext() { EnableLogging = true };

            var context = new GatherContext(sourceMappingConfiguration)
            {
                PrimaryTargets = targets.ToList(),
                InstalledPackages = new List<PackageIdentity>(),
                TargetFramework = framework,
                PrimarySources = repos,
                AllSources = repos,
                PackagesFolderSource = repos[2],// Since it's not configuredSource it'll fail finding the packageId.
                ResolutionContext = new ResolutionContext(),
                ProjectContext = testNuGetProjectContext
            };

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => ResolverGather.GatherAsync(context, CancellationToken.None));

            // Assert
            Assert.True(sourceMappingConfiguration.IsEnabled);
            Assert.Empty(configuredSources);

            // Assert log.
            Assert.Contains($"Package '{packageId} 1.0.0' is not found in the following primary source(s)", exception.Message);
        }

        private static SourceRepository CreateTimeoutRepo(string source)
        {
            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.Add(new Lazy<INuGetResourceProvider>(() => new TestTimeoutDependencyInfoProvider()));

            return new SourceRepository(new Configuration.PackageSource(source), providers);
        }

        private static SourceRepository CreateThrowingRepo(string source, Exception exception)
        {
            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.Add(new Lazy<INuGetResourceProvider>(() => new TestThrowingDependencyInfoProvider(exception)));

            return new SourceRepository(new Configuration.PackageSource(source), providers);
        }

        private static SourceRepository CreateRepo(string source, List<SourcePackageDependencyInfo> packages)
        {
            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.Add(new Lazy<INuGetResourceProvider>(() => new TestDependencyInfoProvider(packages)));

            return new SourceRepository(new Configuration.PackageSource(source), providers);
        }

        private static SourcePackageDependencyInfo CreateDependencyInfo(string id, string version, params string[] dependencyIds)
        {
            return new SourcePackageDependencyInfo(CreatePackage(id, version),
                dependencyIds.Select(depId => new Packaging.Core.PackageDependency(depId, new VersionRange(NuGetVersion.Parse("1.0.0")))),
                true,
                source: null,
                downloadUri: null,
                packageHash: null);
        }

        private static PackageIdentity CreatePackage(string id, string version)
        {
            return new PackageIdentity(id, NuGetVersion.Parse(version));
        }
    }

    internal class TestThrowingDependencyInfoProvider : ResourceProvider
    {
        public Exception Exception { get; set; }

        public TestThrowingDependencyInfoProvider(Exception ex)
            : base(typeof(DependencyInfoResource))
        {
            Exception = ex;
        }

        public override Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            var nuGetResource = new TestThrowingDependencyInfo(source, Exception);
            return Task.FromResult(new Tuple<bool, INuGetResource>(true, nuGetResource));
        }
    }

    internal class TestTimeoutDependencyInfoProvider : ResourceProvider
    {
        public TestTimeoutDependencyInfoProvider()
            : base(typeof(DependencyInfoResource))
        {

        }

        public override Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
            var nuGetResource = new TestTimeoutDependencyInfo(source);
            return Task.FromResult(new Tuple<bool, INuGetResource>(true, nuGetResource));
        }
    }

    /// <summary>
    /// Resolves against a local set of packages
    /// </summary>
    internal class TestThrowingDependencyInfo : DependencyInfoResource
    {
        public Exception Exception { get; set; }

        public TestThrowingDependencyInfo(SourceRepository source, Exception ex)
        {
            Exception = ex;
        }

        public override Task<SourcePackageDependencyInfo> ResolvePackage(PackageIdentity package, NuGetFramework projectFramework, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token)
        {
            throw Exception;
        }

        public override Task<IEnumerable<SourcePackageDependencyInfo>> ResolvePackages(string packageId, NuGetFramework projectFramework, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token)
        {
            throw Exception;
        }
    }

    /// <summary>
    /// Resolves against a local set of packages
    /// </summary>
    internal class TestTimeoutDependencyInfo : DependencyInfoResource
    {
        public TestTimeoutDependencyInfo(SourceRepository source)
        {

        }

        public override async Task<SourcePackageDependencyInfo> ResolvePackage(PackageIdentity package, NuGetFramework projectFramework, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                await Task.Delay(20);
            }
        }

        public override async Task<IEnumerable<SourcePackageDependencyInfo>> ResolvePackages(string packageId, NuGetFramework projectFramework, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                await Task.Delay(20);
            }
        }
    }
}

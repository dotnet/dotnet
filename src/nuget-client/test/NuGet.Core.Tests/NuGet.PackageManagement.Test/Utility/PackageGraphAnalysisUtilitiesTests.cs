// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using static NuGet.Frameworks.FrameworkConstants;
using static NuGet.Protocol.Core.Types.Repository;

namespace NuGet.PackageManagement.Test
{
    public class PackageGraphAnalysisUtilitiesTests
    {
        // A 1.0.0 =>
        //      B 1.0.0
        //      C 1.0.0
        // C 1.1.0
        // B 1.0.0 =>
        //      D 1.0.0
        // Installed list: A 1.0.0, B 1.0.0, C 1.1.0, D 1.0.0
        // The scenario is a bump in the version of C
        [Fact]
        public void PackageGraphAnalysisUtilities_GetPackagesWithDependants_Succeeds()
        {
            // Set up
            var packageIdentityA100 = new PackageIdentity("a", NuGetVersion.Parse("1.0.0"));
            var packageIdentityB100 = new PackageIdentity("b", NuGetVersion.Parse("1.0.0"));
            var packageIdentityC100 = new PackageIdentity("c", NuGetVersion.Parse("1.0.0"));
            var packageIdentityC110 = new PackageIdentity("c", NuGetVersion.Parse("1.1.0"));
            var packageIdentityD100 = new PackageIdentity("d", NuGetVersion.Parse("1.0.0"));

            var packageDependencyInfos = new List<PackageDependencyInfo>();
            var packageDependencyInfoA = new PackageDependencyInfo(packageIdentityA100,
                new PackageDependency[] {
                    new PackageDependency(packageIdentityB100.Id, VersionRange.Parse(packageIdentityB100.Version.OriginalVersion)),
                    new PackageDependency(packageIdentityC100.Id, VersionRange.Parse(packageIdentityC100.Version.OriginalVersion)),
                });
            var packageDependencyInfoB = new PackageDependencyInfo(packageIdentityB100,
                new PackageDependency[] {
                    new PackageDependency(packageIdentityD100.Id, VersionRange.Parse(packageIdentityD100.Version.OriginalVersion)),
                });
            var packageDependencyInfoC = new PackageDependencyInfo(packageIdentityC110, Enumerable.Empty<PackageDependency>());
            var packageDependencyInfoD = new PackageDependencyInfo(packageIdentityD100, Enumerable.Empty<PackageDependency>());

            packageDependencyInfos.Add(packageDependencyInfoA);
            packageDependencyInfos.Add(packageDependencyInfoB);
            packageDependencyInfos.Add(packageDependencyInfoC);
            packageDependencyInfos.Add(packageDependencyInfoD);
            // Act

            var packageWithDependants = PackageGraphAnalysisUtilities.GetPackagesWithDependants(packageDependencyInfos);

            // Assert

            foreach (var package in packageWithDependants)
            {
                switch (package.Identity.Id)
                {
                    case "a":
                        {
                            Assert.Equal(0, package.DependantPackages.Count);
                            Assert.True(package.IsTopLevelPackage);
                            break;
                        }
                    case "b":
                        {
                            Assert.Equal(1, package.DependantPackages.Count);
                            Assert.Equal(packageIdentityA100.Id, package.DependantPackages.Single().Id);
                            Assert.False(package.IsTopLevelPackage);
                            break;
                        }
                    case "c":
                        {
                            Assert.Equal(0, package.DependantPackages.Count);
                            Assert.True(package.IsTopLevelPackage);
                            break;
                        }
                    case "d":
                        {
                            Assert.Equal(1, package.DependantPackages.Count);
                            Assert.Equal(packageIdentityB100.Id, package.DependantPackages.Single().Id);
                            Assert.False(package.IsTopLevelPackage);
                            break;
                        }
                    default:
                        {
                            Assert.Fail($"Unexpected package {package.Identity}");
                            break;
                        }
                }
            }
        }

        // A 1.0.0 =>
        //      B 1.0.0
        //      C 1.0.0
        // C 1.1.0
        // B 1.0.0 =>
        //      D 1.0.0
        // Installed list: A 1.0.0, B 1.0.0, C 1.1.0, D 1.0.0
        // The scenario is a bump in the version of C
        [Fact]
        public async Task PackageGraphAnalysisUtilities_GetDependencyInfoForPackageIdentitiesAsync_SucceedsAsync()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Set up packages
                var packageA100 = new SimpleTestPackageContext("a", "1.0.0");
                var packageB100 = new SimpleTestPackageContext("b", "1.0.0");
                var packageC100 = new SimpleTestPackageContext("c", "1.0.0");
                var packageC110 = new SimpleTestPackageContext("c", "1.1.0");
                var packageD100 = new SimpleTestPackageContext("d", "1.0.0");

                // Set up dependency relationships
                packageA100.Dependencies.Add(packageB100);
                packageA100.Dependencies.Add(packageC100);
                packageB100.Dependencies.Add(packageD100);

                // Create the packages
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageA100, packageB100, packageC100, packageC110, packageD100);

                var sourceReposistory = Factory.GetCoreV3(pathContext.PackageSource);

                var installedList = new PackageIdentity[]
                {
                    new PackageIdentity("a", NuGetVersion.Parse("1.0.0")),
                    new PackageIdentity("b", NuGetVersion.Parse("1.0.0")),
                    new PackageIdentity("c", NuGetVersion.Parse("1.1.0")),
                    new PackageIdentity("d", NuGetVersion.Parse("1.0.0")),

                };
                // Act
                var packageDependencyInfos = await PackageGraphAnalysisUtilities.GetDependencyInfoForPackageIdentitiesAsync(
                    packageIdentities: installedList,
                    nuGetFramework: CommonFrameworks.Net45,
                    dependencyInfoResource: await sourceReposistory.GetResourceAsync<DependencyInfoResource>(CancellationToken.None),
                    sourceCacheContext: new SourceCacheContext(),
                    includeUnresolved: true,
                    logger: NullLogger.Instance,
                    cancellationToken: CancellationToken.None
                    );

                // Assert
                foreach (var package in packageDependencyInfos)
                {
                    switch (package.Id)
                    {
                        case "a":
                            {
                                Assert.Equal(2, package.Dependencies.Count());
                                Assert.Contains(package.Dependencies, e => e.Id == packageB100.Id && e.VersionRange.MinVersion.Equals(NuGetVersion.Parse(packageB100.Version)));
                                Assert.Contains(package.Dependencies, e => e.Id == packageC100.Id && e.VersionRange.MinVersion.Equals(NuGetVersion.Parse(packageC100.Version)));

                                break;
                            }
                        case "b":
                            {
                                Assert.Equal(1, package.Dependencies.Count());
                                Assert.Contains(package.Dependencies, e => e.Id == packageD100.Id && e.VersionRange.MinVersion.Equals(NuGetVersion.Parse(packageD100.Version)));
                                break;
                            }
                        case "c":
                            {
                                Assert.Equal(0, package.Dependencies.Count());
                                break;
                            }
                        case "d":
                            {
                                Assert.Equal(0, package.Dependencies.Count());
                                break;
                            }
                        default:
                            {
                                Assert.Fail($"Unexpected package {package.Id}");
                                break;
                            }
                    }
                }

            }
        }
    }
}

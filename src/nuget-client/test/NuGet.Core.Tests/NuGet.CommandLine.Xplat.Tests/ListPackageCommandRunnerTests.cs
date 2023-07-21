// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.ListPackage;
using NuGet.CommandLine.XPlat.Utility;
using NuGet.Common;
using NuGet.Configuration;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests
{
    public class ListPackageCommandRunnerTests
    {
        public class TopLevelPackagesFilterForOutdated
        {
            [Fact]
            public void FiltersAutoReferencedPackages()
            {
                // Arrange
                Func<InstalledPackageReference, bool> filter = ListPackageHelper.TopLevelPackagesFilterForOutdated;
                var installedPackageReference = ListPackageTestHelper.CreateInstalledPackageReference(autoReference: true);

                // Act
                bool result = filter.Invoke(installedPackageReference);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void DoesNotFilterPackagesWithLatestMetadataNull()
            {
                // Arrange
                Func<InstalledPackageReference, bool> filter = ListPackageHelper.TopLevelPackagesFilterForOutdated;
                var installedPackageReference = ListPackageTestHelper.CreateInstalledPackageReference();
                installedPackageReference.LatestPackageMetadata = null;

                // Act
                bool result = filter.Invoke(installedPackageReference);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void DoesNotFilterPackagesWithNewerVersionAvailable()
            {
                // Arrange
                Func<InstalledPackageReference, bool> filter = ListPackageHelper.TopLevelPackagesFilterForOutdated;
                var installedPackageReference = ListPackageTestHelper.CreateInstalledPackageReference(
                    latestPackageVersionString: "2.0.0");

                // Act
                bool result = filter.Invoke(installedPackageReference);

                // Assert
                Assert.True(result);
            }
        }

        public class TransitivePackagesFilterForOutdated
        {
            [Fact]
            public void DoesNotFilterPackagesWithLatestMetadataNull()
            {
                // Arrange
                Func<InstalledPackageReference, bool> filter = ListPackageHelper.TransitivePackagesFilterForOutdated;
                var installedPackageReference = ListPackageTestHelper.CreateInstalledPackageReference();
                installedPackageReference.LatestPackageMetadata = null;

                // Act
                bool result = filter.Invoke(installedPackageReference);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void DoesNotFilterPackagesWithNewerVersionAvailable()
            {
                // Arrange
                Func<InstalledPackageReference, bool> filter = ListPackageHelper.TransitivePackagesFilterForOutdated;
                var installedPackageReference = ListPackageTestHelper.CreateInstalledPackageReference(
                    latestPackageVersionString: "2.0.0");

                // Act
                bool result = filter.Invoke(installedPackageReference);

                // Assert
                Assert.True(result);
            }

            [Theory]
            [InlineData(false, false)]
            [InlineData(false, true)]
            [InlineData(true, false)]
            [InlineData(true, true)]
            public void FiltersFrameworkPackagesCollectionWithOutdatedMetadata(
                bool includeTopLevelPositives,
                bool includeTransitivePositives)
            {
                // Arrange
                var packages = new FrameworkPackages("net40");
                var topLevelPackages =
                    new List<InstalledPackageReference>
                    {
                        ListPackageTestHelper.CreateInstalledPackageReference(resolvedPackageVersionString: "2.0.0",
                            latestPackageVersionString: "2.0.0")
                    };
                var transitivePackages =
                    new List<InstalledPackageReference>
                    {
                        ListPackageTestHelper.CreateInstalledPackageReference(resolvedPackageVersionString: "2.0.0",
                            latestPackageVersionString: "2.0.0")
                    };

                if (includeTopLevelPositives)
                {
                    topLevelPackages.Add(ListPackageTestHelper.CreateInstalledPackageReference(
                        resolvedPackageVersionString: "2.0.0", latestPackageVersionString: "3.0.0"));
                }

                if (includeTransitivePositives)
                {
                    transitivePackages.Add(ListPackageTestHelper.CreateInstalledPackageReference(
                        resolvedPackageVersionString: "2.0.0", latestPackageVersionString: "3.0.0"));
                }

                packages.TopLevelPackages = topLevelPackages;
                packages.TransitivePackages = transitivePackages;
                var allPackages = new List<FrameworkPackages> { packages };
                var listPackageArgs = new ListPackageArgs(path: "", packageSources: new List<PackageSource>(),
                    frameworks: new List<string>(),
                    ReportType.Outdated,
                    new ListPackageConsoleRenderer(),
                    includeTransitive: true, prerelease: false, highestPatch: false, highestMinor: false,
                    logger: new Mock<ILogger>().Object,
                    CancellationToken.None);

                // Act
                var isFilteredSetNonEmpty = ListPackageCommandRunner.FilterPackages(allPackages, listPackageArgs);

                // Assert
                Assert.Equal(includeTopLevelPositives || includeTransitivePositives, isFilteredSetNonEmpty);
                Assert.Equal(includeTopLevelPositives ? 1 : 0, allPackages.First().TopLevelPackages.Count());
                Assert.Equal(includeTransitivePositives ? 1 : 0, allPackages.First().TransitivePackages.Count());
            }
        }

        public class PackagesFilterForDeprecated
        {
            [Fact]
            public void FiltersPackagesWithoutDeprecationMetadata()
            {
                // Arrange
                Func<InstalledPackageReference, bool> filter = ListPackageHelper.PackagesFilterForDeprecated;
                var installedPackageReference = ListPackageTestHelper.CreateInstalledPackageReference();

                // Act
                bool result = filter.Invoke(installedPackageReference);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void DoesNotFilterPackagesWithDeprecationMetadata()
            {
                // Arrange
                Func<InstalledPackageReference, bool> filter = ListPackageHelper.PackagesFilterForDeprecated;
                var installedPackageReference = ListPackageTestHelper.CreateInstalledPackageReference(isDeprecated: true);

                // Act
                bool result = filter.Invoke(installedPackageReference);

                // Assert
                Assert.True(result);
            }

            [Theory]
            [InlineData(false, false)]
            [InlineData(false, true)]
            [InlineData(true, false)]
            [InlineData(true, true)]
            public void FiltersFrameworkPackagesCollectionWithDeprecationMetadata(
                bool includeTopLevelPositives,
                bool includeTransitivePositives)
            {
                // Arrange
                var packages = new FrameworkPackages("net40");
                var topLevelPackages =
                    new List<InstalledPackageReference> { ListPackageTestHelper.CreateInstalledPackageReference() };
                var transitivePackages =
                    new List<InstalledPackageReference> { ListPackageTestHelper.CreateInstalledPackageReference() };
                if (includeTopLevelPositives)
                {
                    topLevelPackages.Add(ListPackageTestHelper.CreateInstalledPackageReference(isDeprecated: true));
                };
                if (includeTransitivePositives)
                {
                    transitivePackages.Add(ListPackageTestHelper.CreateInstalledPackageReference(isDeprecated: true));
                }

                packages.TopLevelPackages = topLevelPackages;
                packages.TransitivePackages = transitivePackages;
                var allPackages = new List<FrameworkPackages> { packages };
                var listPackageArgs = new ListPackageArgs(path: "", packageSources: new List<PackageSource>(),
                    frameworks: new List<string>(),
                    ReportType.Deprecated,
                    new ListPackageConsoleRenderer(),
                    includeTransitive: true, prerelease: false, highestPatch: false, highestMinor: false, logger: new Mock<ILogger>().Object,
                    CancellationToken.None);

                // Act
                var isFilteredSetNonEmpty = ListPackageCommandRunner.FilterPackages(allPackages, listPackageArgs);

                // Assert
                Assert.Equal(includeTopLevelPositives || includeTransitivePositives, isFilteredSetNonEmpty);
                Assert.Equal(includeTopLevelPositives ? 1 : 0, allPackages.First().TopLevelPackages.Count());
                Assert.Equal(includeTransitivePositives ? 1 : 0, allPackages.First().TransitivePackages.Count());
            }
        }

        public class PackagesFilterForVulnerable
        {
            [Fact]
            public void FiltersPackagesWithoutVulnerableMetadata()
            {
                // Arrange
                Func<InstalledPackageReference, bool> filter = ListPackageHelper.PackagesFilterForVulnerable;
                var installedPackageReference = ListPackageTestHelper.CreateInstalledPackageReference();

                // Act
                bool result = filter.Invoke(installedPackageReference);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void DoesNotFilterPackagesWithVulnerableMetadata()
            {
                // Arrange
                Func<InstalledPackageReference, bool> filter = ListPackageHelper.PackagesFilterForVulnerable;
                var installedPackageReference = ListPackageTestHelper.CreateInstalledPackageReference(vulnerabilityCount: 1);

                // Act
                bool result = filter.Invoke(installedPackageReference);

                // Assert
                Assert.True(result);
            }

            [Theory]
            [InlineData(false, false)]
            [InlineData(false, true)]
            [InlineData(true, false)]
            [InlineData(true, true)]
            public void FiltersFrameworkPackagesCollectionWithVulnerableMetadata(
                bool includeTopLevelPositives,
                bool includeTransitivePositives)
            {
                // Arrange
                var packages = new FrameworkPackages("net40");
                var topLevelPackages =
                    new List<InstalledPackageReference> { ListPackageTestHelper.CreateInstalledPackageReference() };
                var transitivePackages =
                    new List<InstalledPackageReference> { ListPackageTestHelper.CreateInstalledPackageReference() };
                if (includeTopLevelPositives)
                {
                    topLevelPackages.Add(ListPackageTestHelper.CreateInstalledPackageReference(vulnerabilityCount: 1));
                };
                if (includeTransitivePositives)
                {
                    transitivePackages.Add(ListPackageTestHelper.CreateInstalledPackageReference(vulnerabilityCount: 1));
                }

                packages.TopLevelPackages = topLevelPackages;
                packages.TransitivePackages = transitivePackages;
                var allPackages = new List<FrameworkPackages> { packages };
                var listPackageArgs = new ListPackageArgs(path: "", packageSources: new List<PackageSource>(),
                    frameworks: new List<string>(),
                    ReportType.Vulnerable,
                    new ListPackageConsoleRenderer(),
                    includeTransitive: true, prerelease: false, highestPatch: false, highestMinor: false, logger: new Mock<ILogger>().Object,
                    CancellationToken.None);

                // Act
                var isFilteredSetNonEmpty = ListPackageCommandRunner.FilterPackages(allPackages, listPackageArgs);

                // Assert
                Assert.Equal(includeTopLevelPositives || includeTransitivePositives, isFilteredSetNonEmpty);
                Assert.Equal(includeTopLevelPositives ? 1 : 0, allPackages.First().TopLevelPackages.Count());
                Assert.Equal(includeTransitivePositives ? 1 : 0, allPackages.First().TransitivePackages.Count());
            }
        }
    }
}

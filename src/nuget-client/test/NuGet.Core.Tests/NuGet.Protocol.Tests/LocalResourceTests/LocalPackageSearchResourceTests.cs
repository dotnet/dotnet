// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Tests.Plugins.Helpers;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class LocalPackageSearchResourceTests
    {
        [Fact]
        public async Task LocalPackageSearchResource_MatchOnIdAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myPackage</id>
                            <version>1.0.0-alpha.1.2+5</version>
                            <description>package description</description>
                            <tags>a b c</tags>
                        </metadata>
                        </package>");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "myPackage",
                    Version = "1.0.0-alpha.1.2+5",
                    Nuspec = nuspec
                };

                var nuspec2 = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myOtherPackage</id>
                            <version>1.0.0-alpha.1.3+5</version>
                            <description>package description</description>
                            <tags>a b c</tags>
                        </metadata>
                        </package>");

                var packageA2 = new SimpleTestPackageContext()
                {
                    Id = "myOtherPackage",
                    Version = "1.0.0-alpha.1.3+5",
                    Nuspec = nuspec2
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageA2
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: true);

                // Act
                var packages = (await resource.SearchAsync(
                        "mypackage",
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity.Id)
                        .ToList();

                var package = packages.First();

                // Assert
                Assert.Equal(1, packages.Count);
                Assert.Equal("myPackage", package.Identity.Id);
                Assert.Equal("1.0.0-alpha.1.2+5", package.Identity.Version.ToFullString());
            }
        }

        [Fact]
        public async Task LocalPackageSearchResource_RelativePathIsRejectedAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();
                var source = Path.Combine("..", "packages");
                var localResource = new FindLocalPackagesResourceV2(source);
                var resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: true);

                // Act & Assert
                var actual = await Assert.ThrowsAsync<InvalidOperationException>(
                    () => resource.SearchAsync(
                        "mypackage",
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None));
                Assert.Equal(
                    $"The path '{source}' for the selected source could not be resolved.",
                    actual.Message);
            }
        }

        [Fact]
        public async Task LocalPackageSearchResource_MatchOnTagAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myPackage</id>
                            <version>1.0.0-alpha.1.2+5</version>
                            <description>package description</description>
                            <tags>apple orange</tags>
                        </metadata>
                        </package>");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "myPackage",
                    Version = "1.0.0-alpha.1.2+5",
                    Nuspec = nuspec
                };

                var nuspec2 = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myOtherPackage</id>
                            <version>1.0.0-alpha.1.3+5</version>
                            <description>package description</description>
                            <tags>grape</tags>
                        </metadata>
                        </package>");

                var packageA2 = new SimpleTestPackageContext()
                {
                    Id = "myOtherPackage",
                    Version = "1.0.0-alpha.1.3+5",
                    Nuspec = nuspec2
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageA2
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: true);

                // Act
                var packages = (await resource.SearchAsync(
                        "apple",
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity.Id)
                        .ToList();

                var package = packages.First();

                // Assert
                Assert.Equal(1, packages.Count);
                Assert.Equal("myPackage", package.Identity.Id);
                Assert.Equal("1.0.0-alpha.1.2+5", package.Identity.Version.ToFullString());
            }
        }

        [Fact]
        public async Task LocalPackageSearchResource_MatchOnDescriptionAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myPackage</id>
                            <version>1.0.0-alpha.1.2+5</version>
                            <description>package blue description</description>
                            <tags>apple orange</tags>
                        </metadata>
                        </package>");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "myPackage",
                    Version = "1.0.0-alpha.1.2+5",
                    Nuspec = nuspec
                };

                var nuspec2 = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myOtherPackage</id>
                            <version>1.0.0-alpha.1.3+5</version>
                            <description>package red description</description>
                            <tags>grape</tags>
                        </metadata>
                        </package>");

                var packageA2 = new SimpleTestPackageContext()
                {
                    Id = "myOtherPackage",
                    Version = "1.0.0-alpha.1.3+5",
                    Nuspec = nuspec2
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageA2
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: true);

                // Act
                var packages = (await resource.SearchAsync(
                        "rEd",
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity.Id)
                        .ToList();

                var package = packages.First();

                // Assert
                Assert.Equal(1, packages.Count);
                Assert.Equal("myOtherPackage", package.Identity.Id);
                Assert.Equal("1.0.0-alpha.1.3+5", package.Identity.Version.ToFullString());
            }
        }

        [Fact]
        public async Task LocalPackageSearchResource_MatchOnPartialIdAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myPackage</id>
                            <version>1.0.0-alpha.1.2+5</version>
                            <description>package description</description>
                            <tags>a b c</tags>
                        </metadata>
                        </package>");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "myPackage",
                    Version = "1.0.0-alpha.1.2+5",
                    Nuspec = nuspec
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: true);

                // Act
                var packages = (await resource.SearchAsync(
                        "ypac",
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity.Id)
                        .ToList();

                var package = packages.First();

                // Assert
                Assert.Equal(1, packages.Count);
                Assert.Equal("myPackage", package.Identity.Id);
                Assert.Equal("1.0.0-alpha.1.2+5", package.Identity.Version.ToFullString());
            }
        }

        [Fact]
        public async Task LocalPackageSearchResource_MatchNoneAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myPackage</id>
                            <version>1.0.0-alpha.1.2+5</version>
                            <description>package description</description>
                            <tags>a b c</tags>
                        </metadata>
                        </package>");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "myPackage",
                    Version = "1.0.0-alpha.1.2+5",
                    Nuspec = nuspec
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: true);

                // Act
                var packages = (await resource.SearchAsync(
                        "test",
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity.Id)
                        .ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        [Fact]
        public async Task LocalPackageSearchResource_SearchStableNoMatchAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myPackage</id>
                            <version>1.0.0-alpha.1.2+5</version>
                            <description>package description</description>
                            <tags>a b c</tags>
                        </metadata>
                        </package>");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "myPackage",
                    Version = "1.0.0-alpha.1.2+5",
                    Nuspec = nuspec
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: false);

                // Act
                var packages = (await resource.SearchAsync(
                        "ypac",
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity.Id)
                        .ToList();

                // Assert
                Assert.Equal(0, packages.Count);
            }
        }

        [Fact]
        public async Task LocalPackageSearchResource_SearchStableMatchAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myPackage</id>
                            <version>2.0.0-alpha.1.2+5</version>
                            <description>package description</description>
                            <tags>a b c</tags>
                        </metadata>
                        </package>");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "myPackage",
                    Version = "1.0.0-alpha.1.2+5",
                    Nuspec = nuspec
                };

                var nuspec2 = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myPackageB</id>
                            <version>1.0.0</version>
                            <description>package description</description>
                            <tags>a b c</tags>
                        </metadata>
                        </package>");

                var packageA2 = new SimpleTestPackageContext()
                {
                    Id = "myPackageB",
                    Version = "1.0.0",
                    Nuspec = nuspec2
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageA2
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var localResource = new FindLocalPackagesResourceV2(root);
                var resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: false);

                // Act
                var packages = (await resource.SearchAsync(
                        "mypackage",
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity.Id)
                        .ToList();

                var package = packages.First();

                // Assert
                Assert.Equal(1, packages.Count);
                Assert.Equal("myPackageB", package.Identity.Id);
                Assert.Equal("1.0.0", package.Identity.Version.ToFullString());
            }
        }

        [Fact]
        public async Task LocalPackageSearchResource_FileSourceAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var testLogger = new TestLogger();

                var nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myPackage</id>
                            <version>1.0.0-alpha.1.2+5</version>
                            <description>package description</description>
                            <tags>a b c</tags>
                        </metadata>
                        </package>");

                var packageA = new SimpleTestPackageContext()
                {
                    Id = "myPackage",
                    Version = "1.0.0-alpha.1.2+5",
                    Nuspec = nuspec
                };

                var nuspec2 = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>myOtherPackage</id>
                            <version>1.0.0-alpha.1.3+5</version>
                            <description>package description</description>
                            <tags>a b c</tags>
                        </metadata>
                        </package>");

                var packageA2 = new SimpleTestPackageContext()
                {
                    Id = "myOtherPackage",
                    Version = "1.0.0-alpha.1.3+5",
                    Nuspec = nuspec2
                };

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageA2
                };

                var fileUrl = "file://" + root.Path.Replace(@"\", @"/");

                await SimpleTestPackageUtility.CreatePackagesAsync(root, packageContexts);

                var localResource = new FindLocalPackagesResourceV2(fileUrl);
                var resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: true);

                // Act
                var packages = (await resource.SearchAsync(
                        "mypackage",
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity.Id)
                        .ToList();

                var package = packages.First();

                // Assert
                Assert.Equal(1, packages.Count);
                Assert.Equal("myPackage", package.Identity.Id);
                Assert.Equal("1.0.0-alpha.1.2+5", package.Identity.Version.ToFullString());
            }
        }

        [Fact]
        public async Task LocalPackageSearch_SearchAsync_WithCancellationToken_ImmediatelyThrowsAsync()
        {
            using (var root = TestDirectory.Create())
            {
                // Arrange
                var localResource = new FindLocalPackagesResourceV2(root);
                LocalPackageSearchResource resource = new LocalPackageSearchResource(localResource);

                // Act & Assert
                await Assert.ThrowsAsync<TaskCanceledException>(
                    async () => await resource.SearchAsync("", null, 0, 1, NullLogger.Instance, new CancellationToken(canceled: true)));
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/11650")]
        public async Task LocalPackageSearch_SearchAsync_SlowLocalRepository_WithCancellationToken_ThrowsAsync()
        {
            using var pathContext = new SimpleTestPathContext();

            // Arrange
            using CancellationTokenSource cts = new();
            var slowLocalRepository = new DelayedFindLocalPackagesResourceV2(pathContext.PackageSource, 2000);

            // Act
            Task delayTask = Task.Delay(TimeSpan.FromMilliseconds(200), cts.Token);
            LocalPackageSearchResource slowResource = new LocalPackageSearchResource(slowLocalRepository);
            Task searchTask = slowResource.SearchAsync(searchTerm: "", filters: null, skip: 0, take: 1, log: NullLogger.Instance, token: cts.Token);

            // Assert
            // To simulate real world scenario I added delayed cancellation logic check in DelayedFindLocalPackagesResourceV2 localRepository.
            // We're expecting delay Task finish before search Task since 2000 > 200.
            Task completed = await Task.WhenAny(searchTask, delayTask);
            if (completed != delayTask)
            {
                // Search task completed before shorter delay Task which is unexpected.
                throw new TimeoutException();
            }
            // Trigger cancellation after 200 milsec.
            cts.Cancel();
            // During execution of long search task cancellation is triggered from localRepository.
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await searchTask);
        }

        [Theory]
        [InlineData(SearchFilterType.IsAbsoluteLatestVersion, true, "2.0.0-alpha.1.2+5")]
        [InlineData(SearchFilterType.IsLatestVersion, false, "1.0.0")]
        public async Task LocalPackageSearchResource_SearchWithFilter_OnlyLatestVersionMatch(SearchFilterType searchFilter, bool includePrerelease, string expectedVersion)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange
                string workingPath = pathContext.WorkingDirectory;
                var testLogger = new TestLogger();
                string repositoryPath = Path.Combine(workingPath, "mypackages");
                string packageId = "myPackage";
                var packageA = new SimpleTestPackageContext(packageId, "2.0.0-alpha.1.2+5");
                var packageA2 = new SimpleTestPackageContext(packageId, "1.0.0");

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA,
                    packageA2
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageContexts);

                FindLocalPackagesResourceV2 localResource = new FindLocalPackagesResourceV2(repositoryPath);
                LocalPackageSearchResource resource = new LocalPackageSearchResource(localResource);

                var filter = new SearchFilter(includePrerelease: includePrerelease, filter: searchFilter);

                // Act
                var matchingPackages = (await resource.SearchAsync(
                        packageId,
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity)
                        .ToList();

                var matchPackage = matchingPackages.First();

                // Assert
                Assert.Equal(1, matchingPackages.Count);
                Assert.Equal(packageId, matchPackage.Identity.Id);
                Assert.Equal(expectedVersion, matchPackage.Identity.Version.ToFullString());
                Assert.Equal(0, testLogger.Warnings);
                Assert.Equal(0, testLogger.Errors);
            }
        }

        [Theory]
        [InlineData(false, new[] { "1.0.0", "1.1.0" })]
        [InlineData(true, new[] { "1.0.0", "1.1.0", "2.0.0-alpha.1.2+5" })]
        public async Task LocalPackageSearchResource_SearchNoFilter_AllversionsMatch(bool includePrerelease, string[] expected)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange
                string workingPath = pathContext.WorkingDirectory;
                var testLogger = new TestLogger();
                string repositoryPath = Path.Combine(workingPath, "mypackages");
                string packageId = "myPackage";
                var packageA1 = new SimpleTestPackageContext(packageId, "1.0.0");
                var packageA2 = new SimpleTestPackageContext(packageId, "1.1.0");
                var packageA3 = new SimpleTestPackageContext(packageId, "2.0.0-alpha.1.2+5");

                var packageContexts = new SimpleTestPackageContext[]
                {
                    packageA1,
                    packageA2,
                    packageA3
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(workingPath, packageContexts);

                var localResource = new FindLocalPackagesResourceV2(workingPath);
                var resource = new LocalPackageSearchResource(localResource);

                // Mimic setup for AllVersions request
                var filter = new SearchFilter(includePrerelease: includePrerelease, filter: null)
                {
                    OrderBy = SearchOrderBy.Id
                };

                // Act
                var matchingPackages = (await resource.SearchAsync(
                        packageId,
                        filter,
                        skip: 0,
                        take: 30,
                        log: testLogger,
                        token: CancellationToken.None))
                        .OrderBy(p => p.Identity)
                        .ToList();

                // Assert
                Assert.Equal(true, matchingPackages.All(p => p.Identity.Id == packageId));
                Assert.Equal(expected, matchingPackages.Select(p => p.Identity.Version.ToFullString()).ToArray());
                Assert.Equal(0, testLogger.Warnings);
                Assert.Equal(0, testLogger.Errors);
            }
        }
    }
}

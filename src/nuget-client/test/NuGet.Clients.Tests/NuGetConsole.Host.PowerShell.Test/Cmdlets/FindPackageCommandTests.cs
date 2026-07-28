// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.PowerShellCmdlets;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.VisualStudio;
using Test.Utility;
using Xunit;

namespace NuGetConsole.Host.PowerShell.Test
{
    [Collection(MockedVS.Collection)]
    public class FindPackageCommandTests : IAsyncServiceProvider
    {
        private readonly Dictionary<Type, Task<object>> _services = new Dictionary<Type, Task<object>>();
        private readonly Mock<IComponentModel> _componentModel;
        private readonly Mock<IVsSolutionManager> _solutionManager;

        public FindPackageCommandTests(GlobalServiceProvider globalServiceProvider)
        {
            globalServiceProvider.Reset();

            _solutionManager = new Mock<IVsSolutionManager>();
            _solutionManager.SetupGet(x => x.SolutionDirectory).Returns(@"C:\test");
            _solutionManager.SetupGet(x => x.IsSolutionOpen).Returns(true);
            _solutionManager.Setup(x => x.IsSolutionAvailableAsync()).ReturnsAsync(true);

            // FindPackageCommand does not call GetNuGetProjectAsync, but the base command constructor
            // resolves IVsSolutionManager, so we still need a valid mock.
            var defaultProject = new Mock<NuGetProject>(
                new Dictionary<string, object> { { NuGetProjectMetadataKeys.Name, "TestProject" } });
            _solutionManager.Setup(x => x.GetDefaultNuGetProjectAsync())
                .ReturnsAsync(defaultProject.Object);

            _componentModel = new Mock<IComponentModel>();
            _componentModel.Setup(x => x.GetService<ISettings>()).Returns(Mock.Of<ISettings>());
            _componentModel.Setup(x => x.GetService<IVsSolutionManager>()).Returns(_solutionManager.Object);
            _componentModel.Setup(x => x.GetService<ISourceControlManagerProvider>()).Returns(Mock.Of<ISourceControlManagerProvider>());
            _componentModel.Setup(x => x.GetService<ICommonOperations>()).Returns(Mock.Of<ICommonOperations>());
            _componentModel.Setup(x => x.GetService<IPackageRestoreManager>()).Returns(Mock.Of<IPackageRestoreManager>());
            _componentModel.Setup(x => x.GetService<IDeleteOnRestartManager>()).Returns(Mock.Of<IDeleteOnRestartManager>());
            _componentModel.Setup(x => x.GetService<IRestoreProgressReporter>()).Returns(Mock.Of<IRestoreProgressReporter>());

            globalServiceProvider.AddService(typeof(SComponentModel), _componentModel.Object);

            ServiceLocator.InitializePackageServiceProvider(this);
        }

        /// <summary>
        /// Verifies that Find-Package returns packages matching an ID keyword.
        /// </summary>
        [Fact]
        public async Task FindPackage_ById_ReturnsMatchingPackagesAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("FindByIdPackageA", "1.0.0"),
                new SimpleTestPackageContext("FindByIdPackageB", "2.0.0"),
                new SimpleTestPackageContext("OtherPackage", "1.0.0"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act — search for packages whose ID contains "FindById"
            var results = fixture.Invoke(
                "Find-Package",
                new Dictionary<string, object>
                {
                    { "Id", "FindById" },
                    { "Source", pathContext.PackageSource },
                });

            // Assert — exactly the two matching packages are returned, ordered ascending by id
            // (the local source enumerates package id folders in ascending order).
            var ids = results.Select(r => ((PowerShellPackage)r.BaseObject).Id).ToList();
            ids.Should().Equal("FindByIdPackageA", "FindByIdPackageB");
        }

        /// <summary>
        /// Verifies that Find-Package returns a result with the expected version.
        /// </summary>
        [Fact]
        public async Task FindPackage_ByExactId_ReturnsPackageWithVersionAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("VersionTestPackage", "1.2.3"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act
            var results = fixture.Invoke(
                "Find-Package",
                new Dictionary<string, object>
                {
                    { "Id", "VersionTestPackage" },
                    { "Source", pathContext.PackageSource },
                });

            // Assert
            results.Should().ContainSingle();
            var package = (PowerShellPackage)results[0].BaseObject;
            package.Id.Should().Be("VersionTestPackage");
            package.Version.ToString().Should().Be("1.2.3");
        }

        /// <summary>
        /// Verifies that Find-Package -AllVersions returns multiple versions for a package.
        /// </summary>
        [Fact]
        public async Task FindPackage_WithAllVersions_ReturnsMultipleVersionsAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("AllVersionsPackage", "1.0.0"),
                new SimpleTestPackageContext("AllVersionsPackage", "2.0.0"),
                new SimpleTestPackageContext("AllVersionsPackage", "3.0.0"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act
            var results = fixture.Invoke(
                "Find-Package",
                new Dictionary<string, object>
                {
                    { "Id", "AllVersionsPackage" },
                    { "AllVersions", true },
                    { "Source", pathContext.PackageSource },
                });

            // Assert — all three versions are returned, ordered descending by version
            // (LocalPackageSearchResource orders versions with OrderByDescending).
            results.Should().ContainSingle();
            var package = (PowerShellPackage)results[0].BaseObject;
            package.Id.Should().Be("AllVersionsPackage");
            var versions = package.Versions.Select(v => v.ToNormalizedString()).ToList();
            versions.Should().Equal("3.0.0", "2.0.0", "1.0.0");
        }

        /// <summary>
        /// Verifies that Find-Package without -IncludePrerelease hides prerelease-only packages.
        /// </summary>
        [Fact]
        public async Task FindPackage_WithoutPrerelease_HidesPrereleaseOnlyPackagesAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            // Only a prerelease version exists
            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PrereleaseOnlyPackage", "1.0.0-beta"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act — no -IncludePrerelease flag
            var results = fixture.Invoke(
                "Find-Package",
                new Dictionary<string, object>
                {
                    { "Id", "PrereleaseOnlyPackage" },
                    { "Source", pathContext.PackageSource },
                });

            // Assert — prerelease-only package should not appear
            results.Should().BeEmpty(because: "the package only has a prerelease version and -IncludePrerelease was not specified");
        }

        /// <summary>
        /// Verifies that Find-Package -IncludePrerelease returns prerelease packages.
        /// </summary>
        [Fact]
        public async Task FindPackage_WithPrerelease_IncludesPrereleasePackagesAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PrereleaseOnlyPackage", "1.0.0-beta"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act — with -IncludePrerelease
            var results = fixture.Invoke(
                "Find-Package",
                new Dictionary<string, object>
                {
                    { "Id", "PrereleaseOnlyPackage" },
                    { "IncludePrerelease", true },
                    { "Source", pathContext.PackageSource },
                });

            // Assert — prerelease version should be returned
            results.Should().ContainSingle();
            var package = (PowerShellPackage)results[0].BaseObject;
            package.Id.Should().Be("PrereleaseOnlyPackage");
            package.Version.ToString().Should().Be("1.0.0-beta");
        }

        /// <summary>
        /// Verifies that Find-Package -First caps the number of returned packages.
        /// </summary>
        [Fact]
        public async Task FindPackage_WithFirst_LimitsNumberOfResultsAsync()
        {
            // Arrange — six distinct package ids share a common keyword
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("FirstKeywordPackage1", "1.0.0"),
                new SimpleTestPackageContext("FirstKeywordPackage2", "1.0.0"),
                new SimpleTestPackageContext("FirstKeywordPackage3", "1.0.0"),
                new SimpleTestPackageContext("FirstKeywordPackage4", "1.0.0"),
                new SimpleTestPackageContext("FirstKeywordPackage5", "1.0.0"),
                new SimpleTestPackageContext("FirstKeywordPackage6", "1.0.0"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act — Find-Package FirstKeywordPackage -First 5
            var results = fixture.Invoke(
                "Find-Package",
                new Dictionary<string, object>
                {
                    { "Id", "FirstKeywordPackage" },
                    { "Source", pathContext.PackageSource },
                    { "First", 5 },
                });

            // Assert — -First caps the count to the first 5 ids in ascending order
            var ids = results.Select(r => ((PowerShellPackage)r.BaseObject).Id).ToList();
            ids.Should().Equal(
                "FirstKeywordPackage1",
                "FirstKeywordPackage2",
                "FirstKeywordPackage3",
                "FirstKeywordPackage4",
                "FirstKeywordPackage5");
        }

        /// <summary>
        /// Verifies that Find-Package -Skip past the available results returns nothing.
        /// </summary>
        [Fact]
        public async Task FindPackage_WithSkipBeyondResultCount_ReturnsEmptyAsync()
        {
            // Arrange — a single matching package
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("SkipKeywordPackage", "1.0.0"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act 1 — without -Skip, the single package is returned
            var withoutSkip = fixture.Invoke(
                "Find-Package",
                new Dictionary<string, object>
                {
                    { "Id", "SkipKeywordPackage" },
                    { "Source", pathContext.PackageSource },
                });

            // Act 2 — skipping past the only result returns nothing
            var withSkip = fixture.Invoke(
                "Find-Package",
                new Dictionary<string, object>
                {
                    { "Id", "SkipKeywordPackage" },
                    { "Source", pathContext.PackageSource },
                    { "Skip", 1 },
                });

            // Assert
            withoutSkip.Should().ContainSingle();
            withSkip.Should().BeEmpty();
        }

        /// <summary>
        /// Verifies that Find-Package -First combined with -Skip returns a full page of results
        /// after skipping into a larger result set.
        /// </summary>
        [Fact]
        public async Task FindPackage_WithFirstAndSkip_ReturnsLimitedResultsAsync()
        {
            // Arrange — six matching packages; skip into the set and take a full page
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PageKeywordPackage1", "1.0.0"),
                new SimpleTestPackageContext("PageKeywordPackage2", "1.0.0"),
                new SimpleTestPackageContext("PageKeywordPackage3", "1.0.0"),
                new SimpleTestPackageContext("PageKeywordPackage4", "1.0.0"),
                new SimpleTestPackageContext("PageKeywordPackage5", "1.0.0"),
                new SimpleTestPackageContext("PageKeywordPackage6", "1.0.0"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act — Find-Package PageKeywordPackage -First 3 -Skip 2
            var results = fixture.Invoke(
                "Find-Package",
                new Dictionary<string, object>
                {
                    { "Id", "PageKeywordPackage" },
                    { "Source", pathContext.PackageSource },
                    { "First", 3 },
                    { "Skip", 2 },
                });

            // Assert — skipping 2 into the ascending set and taking 3 returns packages 3, 4, 5
            var ids = results.Select(r => ((PowerShellPackage)r.BaseObject).Id).ToList();
            ids.Should().Equal(
                "PageKeywordPackage3",
                "PageKeywordPackage4",
                "PageKeywordPackage5");
        }

        private void SetupSourceRepositoryProvider(string localSourcePath)
        {
            var localSource = new PackageSource(localSourcePath);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(localSource);
            _componentModel.Setup(x => x.GetService<ISourceRepositoryProvider>()).Returns(sourceRepositoryProvider);
        }

        public Task<object?> GetServiceAsync(Type serviceType)
        {
            if (_services.TryGetValue(serviceType, out Task<object>? task))
            {
                return task!;
            }

            return Task.FromResult<object?>(null);
        }
    }
}

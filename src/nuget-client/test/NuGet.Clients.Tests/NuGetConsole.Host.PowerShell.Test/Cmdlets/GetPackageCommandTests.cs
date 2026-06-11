// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Moq;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.PackageManagement.PowerShellCmdlets;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using Test.Utility;
using Xunit;
using PSCommand = System.Management.Automation.Runspaces.Command;

namespace NuGetConsole.Host.PowerShell.Test
{
    [Collection(MockedVS.Collection)]
    public class GetPackageCommandTests : IAsyncServiceProvider
    {
        private readonly Dictionary<Type, Task<object>> _services = new Dictionary<Type, Task<object>>();
        private readonly Mock<IComponentModel> _componentModel;
        private readonly Mock<IVsSolutionManager> _solutionManager;

        public GetPackageCommandTests(GlobalServiceProvider globalServiceProvider)
        {
            globalServiceProvider.Reset();

            _solutionManager = new Mock<IVsSolutionManager>();
            _solutionManager.SetupGet(x => x.SolutionDirectory).Returns(@"C:\test");
            _solutionManager.SetupGet(x => x.IsSolutionOpen).Returns(true);
            _solutionManager.Setup(x => x.IsSolutionAvailableAsync()).ReturnsAsync(true);

            // Preprocess() always calls GetNuGetProjectAsync(), which requires a non-null default project
            var defaultProject = new Mock<NuGetProject>(
                new Dictionary<string, object> { { NuGetProjectMetadataKeys.Name, "TestProject" } });
            defaultProject.Setup(p => p.GetInstalledPackagesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<PackageReference>());

            _solutionManager.Setup(x => x.GetNuGetProjectsAsync())
                .ReturnsAsync(new[] { defaultProject.Object });
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

        [Fact]
        public async Task GetPackageListAvailable_WithLocalSource_ReturnsPackagesAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var packageA = new SimpleTestPackageContext("TestPackageA", "1.0.0");
            await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageA);

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ListAvailable", true },
                    { "Source", pathContext.PackageSource },
                });

            // Assert
            results.Should().ContainSingle();
            var package = (PowerShellRemotePackage)results[0].BaseObject;
            package.Id.Should().Be("TestPackageA");
        }

        [Fact]
        public async Task GetPackageListAvailable_WithFilter_ReturnsMatchingPackageAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("TargetPackage", "1.0.0"),
                new SimpleTestPackageContext("OtherPackage", "1.0.0"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ListAvailable", true },
                    { "Source", pathContext.PackageSource },
                    { "Filter", "TargetPackage" },
                });

            // Assert
            results.Should().ContainSingle();
            var package = (PowerShellRemotePackage)results[0].BaseObject;
            package.Id.Should().Be("TargetPackage");
        }

        [Fact]
        public async Task GetPackageListAvailable_WithoutPrerelease_HidesPrereleaseVersionsAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0-a"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0-b"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.1-a"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - no -Prerelease switch
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ListAvailable", true },
                    { "Source", pathContext.PackageSource },
                    { "Filter", "PreReleaseTestPackage" },
                });

            // Assert - only stable version shown
            results.Should().ContainSingle();
            var package = (PowerShellRemotePackage)results[0].BaseObject;
            package.Id.Should().Be("PreReleaseTestPackage");
            package.Version.ToString().Should().Be("1.0.0");
        }

        [Fact]
        public async Task GetPackageListAvailable_AllVersionsWithoutPrerelease_HidesPrereleaseVersionsAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0-a"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0-b"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.1-a"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - -AllVersions but no -Prerelease
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ListAvailable", true },
                    { "AllVersions", true },
                    { "Source", pathContext.PackageSource },
                    { "Filter", "PreReleaseTestPackage" },
                });

            // Assert - only stable versions shown
            results.Should().ContainSingle();
            var package = (PowerShellRemotePackage)results[0].BaseObject;
            package.Id.Should().Be("PreReleaseTestPackage");
            package.Version.ToString().Should().Be("1.0.0");
        }

        [Fact]
        public async Task GetPackageListAvailable_WithPrerelease_ShowsPrereleaseVersionAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0-a"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0-b"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.1-a"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - with -Prerelease
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ListAvailable", true },
                    { "IncludePrerelease", true },
                    { "Source", pathContext.PackageSource },
                    { "Filter", "PreReleaseTestPackage" },
                });

            // Assert - latest version is 1.0.1-a (prerelease)
            results.Should().ContainSingle();
            var package = (PowerShellRemotePackage)results[0].BaseObject;
            package.Id.Should().Be("PreReleaseTestPackage");
            package.Version.ToString().Should().Be("1.0.1-a");
        }

        [Fact]
        public async Task GetPackageListAvailable_AllVersionsWithPrerelease_ShowsAllVersionsAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0-a"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0-b"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.0"),
                new SimpleTestPackageContext("PreReleaseTestPackage", "1.0.1-a"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - with -AllVersions -Prerelease
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ListAvailable", true },
                    { "AllVersions", true },
                    { "IncludePrerelease", true },
                    { "Source", pathContext.PackageSource },
                    { "Filter", "PreReleaseTestPackage" },
                });

            // Assert - all versions returned, descending order
            results.Should().ContainSingle();
            var package = (PowerShellRemotePackage)results[0].BaseObject;
            package.Id.Should().Be("PreReleaseTestPackage");
            var versions = package.Versions.Select(v => v.ToNormalizedString()).ToList();
            versions.Should().Equal("1.0.1-a", "1.0.0", "1.0.0-b", "1.0.0-a");
        }

        [Fact]
        public async Task GetPackageUpdates_WithoutPrerelease_DoesNotReturnPrereleaseUpdateAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0-a"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0-b"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.1-a"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);
            SetupProjectWithInstalledPackage("PrereleaseTestPackage", "1.0.0-b");

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - Get-Package -Updates (no -Prerelease)
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "Updates", true },
                    { "Source", pathContext.PackageSource },
                });

            // Assert - only stable update (1.0.0) returned, not prerelease 1.0.1-a
            results.Should().ContainSingle();
            var package = (PowerShellUpdatePackage)results[0].BaseObject;
            package.Id.Should().Be("PrereleaseTestPackage");
            package.Version.ToString().Should().Be("1.0.0");
        }

        [Fact]
        public async Task GetPackageUpdates_WithPrerelease_ReturnsPrereleaseUpdateAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0-a"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0-b"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.1-a"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);
            SetupProjectWithInstalledPackage("PrereleaseTestPackage", "1.0.0-a");

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - Get-Package -Updates -Prerelease
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "Updates", true },
                    { "IncludePrerelease", true },
                    { "Source", pathContext.PackageSource },
                });

            // Assert - prerelease update 1.0.1-a returned
            results.Should().ContainSingle();
            var package = (PowerShellUpdatePackage)results[0].BaseObject;
            package.Id.Should().Be("PrereleaseTestPackage");
            package.Version.ToString().Should().Be("1.0.1-a");
        }

        [Fact]
        public async Task GetPackageUpdates_AllVersionsWithoutPrerelease_ReturnsStableVersionAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0-a"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0-b"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.1-a"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);
            SetupProjectWithInstalledPackage("PrereleaseTestPackage", "1.0.0-a");

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - Get-Package -Updates -AllVersions (no -Prerelease)
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "Updates", true },
                    { "AllVersions", true },
                    { "Source", pathContext.PackageSource },
                });

            // Assert - only stable update (1.0.0) returned
            results.Should().ContainSingle();
            var package = (PowerShellUpdatePackage)results[0].BaseObject;
            package.Id.Should().Be("PrereleaseTestPackage");
            package.Version.ToString().Should().Be("1.0.0");
        }

        [Fact]
        public async Task GetPackageUpdates_AllVersionsWithPrerelease_ReturnsAllEligibleUpdatesAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0-a"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0-b"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.0"),
                new SimpleTestPackageContext("PrereleaseTestPackage", "1.0.1-a"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);
            SetupProjectWithInstalledPackage("PrereleaseTestPackage", "1.0.0-b");

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - Get-Package -Updates -AllVersions -Prerelease
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "Updates", true },
                    { "AllVersions", true },
                    { "IncludePrerelease", true },
                    { "Source", pathContext.PackageSource },
                });

            // Assert - all versions > 1.0.0-b returned
            results.Should().ContainSingle();
            var package = (PowerShellUpdatePackage)results[0].BaseObject;
            package.Id.Should().Be("PrereleaseTestPackage");
            var versions = package.Versions.Select(v => v.ToNormalizedString()).ToList();
            versions.Should().Equal("1.0.1-a", "1.0.0");
        }

        [Fact]
        public async Task GetPackageListAvailable_WithAbsolutePathSource_ReturnsPackagesAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            var packageA = new SimpleTestPackageContext("PathSourcePackage", "1.0.0");
            await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageA);

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - use absolute path as -Source
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ListAvailable", true },
                    { "Source", pathContext.PackageSource },
                });

            // Assert
            results.Should().ContainSingle();
            var package = (PowerShellRemotePackage)results[0].BaseObject;
            package.Id.Should().Be("PathSourcePackage");
        }

        [Fact]
        public async Task GetPackageInstalled_ListsInstalledPackagesAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            SetupSourceRepositoryProvider(pathContext.PackageSource);
            SetupMultipleInstalledPackages("TestProject", new[]
            {
                ("TestPackageA", "1.0.0"),
                ("TestPackageB", "2.0.0"),
            });

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - Get-Package (no switches = list installed)
            var results = fixture.Invoke("Get-Package", new Dictionary<string, object>());

            // Assert
            results.Should().HaveCount(2);
            var ids = results.Select(r => ((PowerShellInstalledPackage)r.BaseObject).Id).ToList();
            ids.Should().Contain("TestPackageA");
            ids.Should().Contain("TestPackageB");
        }

        [Fact]
        public async Task GetPackageInstalled_ForProject_ReturnsCorrectPackagesAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            SetupSourceRepositoryProvider(pathContext.PackageSource);
            SetupNamedProjectWithInstalledPackages("ProjectA", new[] { ("TestPackage", "1.5.0") });

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - Get-Package -ProjectName ProjectA
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ProjectName", "ProjectA" },
                });

            // Assert
            results.Should().ContainSingle();
            var package = (PowerShellInstalledPackage)results[0].BaseObject;
            package.Id.Should().Be("TestPackage");
            package.Versions.First().ToString().Should().Be("1.5.0");
        }

        [Fact]
        public async Task GetPackageInstalled_ForOtherProject_ReturnsEmptyAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            // ProjectA has a package, ProjectB has none
            var projectA = CreateMockProject("ProjectA", new[] { ("TestPackage", "1.0.0") });
            var projectB = CreateMockProject("ProjectB", Array.Empty<(string, string)>());

            _solutionManager.Setup(x => x.GetNuGetProjectsAsync())
                .ReturnsAsync(new[] { projectA, projectB });
            _solutionManager.Setup(x => x.GetDefaultNuGetProjectAsync())
                .ReturnsAsync(projectA);
            _solutionManager.Setup(x => x.GetNuGetProjectAsync("ProjectB"))
                .ReturnsAsync(projectB);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - Get-Package -ProjectName ProjectB
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ProjectName", "ProjectB" },
                });

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPackageInstalled_WithFilter_ReturnsMatchingPackageAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            SetupSourceRepositoryProvider(pathContext.PackageSource);
            SetupMultipleInstalledPackages("TestProject", new[]
            {
                ("TestFilterPackage", "1.0.0"),
                ("OtherPackage", "1.0.0"),
            });

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act - Get-Package 'TestFilter'
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "Filter", "TestFilter" },
                });

            // Assert
            results.Should().ContainSingle();
            var package = (PowerShellInstalledPackage)results[0].BaseObject;
            package.Id.Should().Be("TestFilterPackage");
        }

        [Fact]
        public async Task GetPackageListAvailable_WithFilter_FindsReleaseNotesPackageAsync()
        {
            // Arrange
            using var pathContext = new SimpleTestPathContext();

            await SimpleTestPackageUtility.CreatePackagesAsync(
                pathContext.PackageSource,
                new SimpleTestPackageContext("ReleaseNotesPackage", "1.0.0"));

            SetupSourceRepositoryProvider(pathContext.PackageSource);

            using var fixture = new CmdletRunspaceFixture(activeSource: pathContext.PackageSource);

            // Act
            var results = fixture.Invoke(
                "Get-Package",
                new Dictionary<string, object>
                {
                    { "ListAvailable", true },
                    { "Source", pathContext.PackageSource },
                    { "Filter", "ReleaseNotesPackage" },
                });

            // Assert
            results.Should().ContainSingle();
            var package = (PowerShellRemotePackage)results[0].BaseObject;
            package.Id.Should().Be("ReleaseNotesPackage");
        }

        #region Helpers

        private void SetupSourceRepositoryProvider(string localSourcePath)
        {
            var localSource = new PackageSource(localSourcePath);
            var sourceRepositoryProvider = TestSourceRepositoryUtility.CreateSourceRepositoryProvider(localSource);
            _componentModel.Setup(x => x.GetService<ISourceRepositoryProvider>()).Returns(sourceRepositoryProvider);
        }

        private NuGetProject CreateMockProject(string projectName, (string id, string version)[] packages)
        {
            var packageRefs = packages.Select(p => new PackageReference(
                new PackageIdentity(p.id, NuGetVersion.Parse(p.version)),
                NuGetFramework.AnyFramework)).ToArray();

            var project = new Mock<NuGetProject>(
                new Dictionary<string, object> { { NuGetProjectMetadataKeys.Name, projectName } });
            project.Setup(p => p.GetInstalledPackagesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(packageRefs);

            return project.Object;
        }

        private void SetupMultipleInstalledPackages(string projectName, (string id, string version)[] packages)
        {
            var projectInstance = CreateMockProject(projectName, packages);

            _solutionManager.Setup(x => x.GetNuGetProjectsAsync())
                .ReturnsAsync(new[] { projectInstance });
            _solutionManager.Setup(x => x.GetDefaultNuGetProjectAsync())
                .ReturnsAsync(projectInstance);
        }

        private void SetupNamedProjectWithInstalledPackages(string projectName, (string id, string version)[] packages)
        {
            var projectInstance = CreateMockProject(projectName, packages);

            _solutionManager.Setup(x => x.GetNuGetProjectsAsync())
                .ReturnsAsync(new[] { projectInstance });
            _solutionManager.Setup(x => x.GetDefaultNuGetProjectAsync())
                .ReturnsAsync(projectInstance);
            _solutionManager.Setup(x => x.GetNuGetProjectAsync(projectName))
                .ReturnsAsync(projectInstance);
        }

        private void SetupProjectWithInstalledPackage(string packageId, string packageVersion)
        {
            SetupMultipleInstalledPackages("TestProject", new[] { (packageId, packageVersion) });
        }

        public Task<object?> GetServiceAsync(Type serviceType)
        {
            if (_services.TryGetValue(serviceType, out Task<object>? task))
            {
                return task!;
            }

            return Task.FromResult<object?>(null);
        }

        #endregion

        #region Test Infrastructure

        /// <summary>
        /// Encapsulates runspace and host setup for invoking NuGet PowerShell cmdlets in tests.
        /// </summary>
        private sealed class CmdletRunspaceFixture : IDisposable
        {
            private readonly Runspace _runspace;

            public CmdletRunspaceFixture(string activeSource = "https://api.nuget.org/v3/index.json")
            {
                var host = new TestPSHost(activeSource);
                var initialSessionState = InitialSessionState.CreateDefault();
                initialSessionState.Commands.Add(
                    new SessionStateCmdletEntry("Get-Package", typeof(GetPackageCommand), null));

                _runspace = RunspaceFactory.CreateRunspace(host, initialSessionState);
                _runspace.Open();
            }

            public IList<PSObject> Invoke(string cmdletName, Dictionary<string, object> parameters)
            {
                using var pipeline = _runspace.CreatePipeline();
                var cmd = new PSCommand(cmdletName);
                foreach (var kvp in parameters)
                {
                    cmd.Parameters.Add(kvp.Key, kvp.Value);
                }
                pipeline.Commands.Add(cmd);
                return pipeline.Invoke().ToList();
            }

            public void Dispose()
            {
                _runspace.Close();
                _runspace.Dispose();
            }
        }

        /// <summary>
        /// Minimal PSHost that provides PrivateData with properties expected by NuGet cmdlets.
        /// </summary>
        private sealed class TestPSHost : PSHost
        {
            private readonly Guid _instanceId = Guid.NewGuid();
            private readonly PSObject _privateData;

            public TestPSHost(string activeSource)
            {
                _privateData = new PSObject();
                _privateData.Properties.Add(new PSNoteProperty("activePackageSource", activeSource));
                _privateData.Properties.Add(new PSNoteProperty("CancellationTokenKey", CancellationToken.None));
            }

            public override CultureInfo CurrentCulture => CultureInfo.InvariantCulture;
            public override CultureInfo CurrentUICulture => CultureInfo.InvariantCulture;
            public override Guid InstanceId => _instanceId;
            public override string Name => "TestNuGetHost";
            public override PSObject PrivateData => _privateData;
            public override PSHostUserInterface? UI => null;
            public override Version Version => new Version(1, 0);

            public override void EnterNestedPrompt() { }
            public override void ExitNestedPrompt() { }
            public override void NotifyBeginApplication() { }
            public override void NotifyEndApplication() { }
            public override void SetShouldExit(int exitCode) { }
        }

        #endregion
    }
}

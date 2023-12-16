// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.PackageManagement.VisualStudio;
using NuGet.PackageManagement.VisualStudio.Exceptions;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Versioning;
using NuGet.VisualStudio.Contracts;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using Test.Utility.ProjectManagement;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test.Extensibility
{
    public class NuGetProjectServiceTests
    {
        [Fact]
        public async Task GetInstalledPackagesAsync_CpsProjectNotNominated_ReturnsProjectNotReadyResult()
        {
            // Arrange
            var projectGuid = Guid.NewGuid();

            var settings = new Mock<ISettings>();
            var telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

            // emulate the ProjectNotNominatedException when we have a CpsPackageReference project, but there's no nomination data yet.
            var project = new Mock<BuildIntegratedNuGetProject>();
            project.Setup(p => p.GetPackageSpecsAndAdditionalMessagesAsync(It.IsAny<DependencyGraphCacheContext>()))
                .Throws<ProjectNotNominatedException>();

            var solutionManager = new Mock<IVsSolutionManager>();
            solutionManager.Setup(sm => sm.GetNuGetProjectAsync(projectGuid.ToString()))
                .Returns(() => Task.FromResult<NuGetProject>(project.Object));

            // Act
            var target = new NuGetProjectService(solutionManager.Object, settings.Object, telemetryProvider.Object);
            InstalledPackagesResult actual = await target.GetInstalledPackagesAsync(projectGuid, CancellationToken.None);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(InstalledPackageResultStatus.ProjectNotReady, actual.Status);
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_LegacyProjectInvalidDataException_ReturnsProjectInvalidResultAsync()
        {
            // Arrange
            var projectGuid = Guid.NewGuid();

            var settings = new Mock<ISettings>();
            var telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

            // Emulate the InvalidDataException thrown by LegacyPackageReferenceProject when the project doesn't have MSBuildProjectExtensionsPath
            var project = new Mock<BuildIntegratedNuGetProject>();
            project.Setup(p => p.GetPackageSpecsAndAdditionalMessagesAsync(It.IsAny<DependencyGraphCacheContext>()))
                .Throws<InvalidDataException>();

            var solutionManager = new Mock<IVsSolutionManager>();
            solutionManager.Setup(sm => sm.GetNuGetProjectAsync(projectGuid.ToString()))
                .Returns(() => Task.FromResult<NuGetProject>(project.Object));

            // Act
            var target = new NuGetProjectService(solutionManager.Object, settings.Object, telemetryProvider.Object);
            InstalledPackagesResult actual = await target.GetInstalledPackagesAsync(projectGuid, CancellationToken.None);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(InstalledPackageResultStatus.ProjectInvalid, actual.Status);
        }

        [Fact]
        public async Task GetInstalledPackagesAsync_PackageReferenceProject_ReturnsTransitiveAsync()
        {
            // Arrange
            var projectGuid = Guid.NewGuid();

            var settings = new Mock<ISettings>();
            var telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

            var installedPackages = new List<PackageReference>()
            {
                new PackageReference(new PackageIdentity("a", new NuGetVersion(1, 0, 0)), FrameworkConstants.CommonFrameworks.Net50)
            };
            var transitivePackages = new List<PackageReference>()
            {
                new PackageReference(new PackageIdentity("b", new NuGetVersion(1, 2, 3)), FrameworkConstants.CommonFrameworks.Net50)
            };
            var transitiveProjectPackages = transitivePackages.Select(p => new TransitivePackageReference(p)).ToList();
            var projectPackages = new ProjectPackages(installedPackages, transitiveProjectPackages);
            var project = new TestPackageReferenceProject("ProjectA", @"src\ProjectA\Project.csproj", @"c:\path\to\src\ProjectA\ProjectA.csproj",
                installedPackages, transitivePackages);

            var solutionManager = new Mock<IVsSolutionManager>();
            solutionManager.Setup(sm => sm.GetNuGetProjectAsync(projectGuid.ToString()))
                .Returns(() => Task.FromResult<NuGetProject>(project));

            // Act
            var target = new NuGetProjectService(solutionManager.Object, settings.Object, telemetryProvider.Object);
            InstalledPackagesResult actual = await target.GetInstalledPackagesAsync(projectGuid, CancellationToken.None);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(InstalledPackageResultStatus.Successful, actual.Status);

            NuGetInstalledPackage package = actual.Packages.FirstOrDefault(p => p.Id == "a");
            Assert.NotNull(package);
            Assert.True(package.DirectDependency);

            package = actual.Packages.FirstOrDefault(p => p.Id == "b");
            Assert.NotNull(package);
            Assert.False(package.DirectDependency);
        }

        class TestPackageReferenceProject : PackageReferenceProject<List<PackageReference>, PackageReference>
        {
            public TestPackageReferenceProject(
                string projectName,
                string projectUniqueName,
                string projectFullPath,
                List<PackageReference> installedPackages,
                List<PackageReference> transitivePackages)
                : base(projectName, projectUniqueName, projectFullPath)
            {
                InstalledPackages = installedPackages;
                TransitivePackages = transitivePackages;
            }

            public override string MSBuildProjectPath => ProjectFullPath;

            public override Task AddFileToProjectAsync(string filePath)
            {
                throw new NotImplementedException();
            }

            public override Task<string> GetCacheFilePathAsync()
            {
                throw new NotImplementedException();
            }

            public override Task<ProjectPackages> GetInstalledAndTransitivePackagesAsync(bool includeTransitiveOrigins, CancellationToken token)
            {
                var transitivePackages = TransitivePackages.Select(p => new TransitivePackageReference(p));

                var projectPackages = new ProjectPackages(InstalledPackages.ToList(), transitivePackages.ToList());
                return Task.FromResult(projectPackages);
            }

            public override Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override Task<(IReadOnlyList<PackageSpec> dgSpecs, IReadOnlyList<IAssetsLogMessage> additionalMessages)> GetPackageSpecsAndAdditionalMessagesAsync(DependencyGraphCacheContext context)
            {
                DependencyGraphSpec dgSpec = DependencyGraphSpecTestUtilities.CreateMinimalDependencyGraphSpec(ProjectFullPath, MSBuildProjectPath);

                List<PackageSpec> packageSpecs = new List<PackageSpec>();
                packageSpecs.Add(dgSpec.GetProjectSpec(ProjectFullPath));

                (IReadOnlyList<PackageSpec>, IReadOnlyList<IAssetsLogMessage>) result = (packageSpecs, null);
                return Task.FromResult(result);
            }

            public override Task<bool> InstallPackageAsync(string packageId, VersionRange range, INuGetProjectContext nuGetProjectContext, BuildIntegratedInstallationContext installationContext, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            public override Task<bool> UninstallPackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nuGetProjectContext, CancellationToken token)
            {
                throw new NotImplementedException();
            }

            protected override Task<string> GetAssetsFilePathAsync(bool shouldThrow)
            {
                throw new NotImplementedException();
            }

            protected override Task<PackageSpec> GetPackageSpecAsync(CancellationToken ct)
            {
                throw new NotImplementedException();
            }

            protected override IEnumerable<PackageReference> ResolvedInstalledPackagesList(IEnumerable<LibraryDependency> libraries, NuGetFramework targetFramework, IList<LockFileTarget> targets, List<PackageReference> installedPackages)
            {
                throw new NotImplementedException();
            }

            protected override IReadOnlyList<PackageReference> ResolvedTransitivePackagesList(NuGetFramework targetFramework, IList<LockFileTarget> targets, List<PackageReference> installedPackages, List<PackageReference> transitivePackages)
            {
                throw new NotImplementedException();
            }

            protected override List<PackageReference> GetCollectionCopy(List<PackageReference> collection) => new(collection);

            public override Task<bool> UninstallPackageAsync(string packageId, BuildIntegratedInstallationContext installationContext, CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}

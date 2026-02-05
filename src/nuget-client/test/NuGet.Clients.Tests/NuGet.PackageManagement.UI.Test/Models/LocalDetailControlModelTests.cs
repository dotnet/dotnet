// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.PackageManagement.UI.Models.Package;
using NuGet.PackageManagement.UI.Test.Models.Package;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Models
{
    [Collection(MockedVS.Collection)]
    public abstract class LocalDetailControlModelTestBase : IClassFixture<LocalPackageSearchMetadataFixture>
    {
        protected readonly LocalPackageSearchMetadataFixture _testData;
        protected readonly PackageItemViewModel _testViewModel;
        protected readonly JoinableTaskContext _joinableTaskContext;
        protected bool disposedValue = false;

        public LocalDetailControlModelTestBase(GlobalServiceProvider sp, LocalPackageSearchMetadataFixture testData)
        {
            sp.Reset();
            _testData = testData;
            var testVersion = new NuGetVersion(0, 0, 1);
            var searchService = new Mock<INuGetSearchService>();

            var identity = new PackageIdentity("TestPackage", new NuGetVersion("1.0.0"));
            var embeddedResourceCapability = new Mock<IEmbeddedResourcesCapable>();
            var packagePath = "C:\\TestPackage";

            // Act
            var packageModel = PackageModelCreationTestHelper.CreateLocalPackageModel(identity, packagePath, embeddedResourceCapability.Object);

            _testViewModel = new PackageItemViewModel(searchService.Object, packageModel)
            {
                InstalledVersion = testVersion,
            };
        }
    }

    public class LocalPackageDetailControlModelTests : LocalDetailControlModelTestBase
    {
        private readonly PackageDetailControlModel _testInstance;

        public LocalPackageDetailControlModelTests(GlobalServiceProvider sp, LocalPackageSearchMetadataFixture testData)
            : base(sp, testData)
        {
            var solMgr = new Mock<INuGetSolutionManagerService>();
            _testInstance = new PackageDetailControlModel(
                Mock.Of<IServiceBroker>(),
                solutionManager: solMgr.Object,
                projects: new List<IProjectContextInfo>(),
                uiController: Mock.Of<INuGetUI>());

            _testInstance.SetCurrentPackageAsync(
                _testViewModel,
                ItemFilter.All,
                () => null,
                CancellationToken.None).Wait();
        }

        [Fact]
        public void PackagePath_Always_IsNotNull()
        {
            Assert.NotNull(_testInstance.PackagePath);
        }

        [Theory]
        [InlineData(NuGetProjectKind.Unknown)]
        [InlineData(NuGetProjectKind.PackageReference)]
        [InlineData(NuGetProjectKind.ProjectK)]
        public void Options_ShowClassicOptions_WhenProjectKindIsNotProjectConfig_ReturnsFalse(NuGetProjectKind projectKind)
        {
            // Arrange
            var project = new Mock<IProjectContextInfo>();

            project.SetupGet(p => p.ProjectKind)
                .Returns(projectKind);

            // Act
            var model = new PackageDetailControlModel(
                Mock.Of<IServiceBroker>(),
                Mock.Of<INuGetSolutionManagerService>(),
                projects: new[] { project.Object },
                uiController: Mock.Of<INuGetUI>());

            // Assert
            Assert.False(model.Options.ShowClassicOptions);
        }

        [Fact]
        public void Options_ShowClassicOptions_WhenProjectKindIsProjectConfig_ReturnsTrue()
        {
            // Assert
            var project = new Mock<IProjectContextInfo>();

            project.SetupGet(p => p.ProjectKind)
                .Returns(NuGetProjectKind.PackagesConfig);

            // Act
            var model = new PackageDetailControlModel(
                Mock.Of<IServiceBroker>(),
                Mock.Of<INuGetSolutionManagerService>(),
                projects: new[] { project.Object },
                uiController: Mock.Of<INuGetUI>());

            // Assert
            Assert.True(model.Options.ShowClassicOptions);
        }

        [Fact]
        public void IsSelectedVersionInstalled_WhenSelectedVersionAndInstalledVersionAreNull_ReturnsFalse()
        {
            // Arrange
            // Act
            var model = new PackageDetailControlModel(
                Mock.Of<IServiceBroker>(),
                Mock.Of<INuGetSolutionManagerService>(),
                Enumerable.Empty<IProjectContextInfo>(),
                uiController: Mock.Of<INuGetUI>());

            // Assert
            Assert.Null(model.SelectedVersion);
            Assert.Null(model.InstalledVersion);
            Assert.False(model.IsSelectedVersionInstalled);
        }

        [Fact]
        public async Task IsSelectedVersionInstalled_WhenSelectedVersionAndInstalledVersionAreNotEqual_ReturnsFalse()
        {
            // Arrange
            var model = new PackageDetailControlModel(
                Mock.Of<IServiceBroker>(),
                Mock.Of<INuGetSolutionManagerService>(),
                Enumerable.Empty<IProjectContextInfo>(),
                uiController: Mock.Of<INuGetUI>());

            NuGetVersion installedVersion = NuGetVersion.Parse("1.0.0");

            var searchService = new Mock<INuGetSearchService>();
            var identity = new PackageIdentity("package", installedVersion);
            var vulnerableCapability = new Mock<IVulnerableCapable>();
            var embeddedResourceCapability = new Mock<IEmbeddedResourcesCapable>();
            var packagePath = "C:\\TestPackage";

            // Act
            var packageModel = PackageModelCreationTestHelper.CreateLocalPackageModel(identity, packagePath, embeddedResourceCapability.Object);
            await model.SetCurrentPackageAsync(
                new PackageItemViewModel(searchService.Object, packageModel)
                {
                    InstalledVersion = installedVersion,
                },
                ItemFilter.All,
                () => null,
                CancellationToken.None);

            NuGetVersion selectedVersion = NuGetVersion.Parse("2.0.0");

            model.SelectedVersion = new DisplayVersion(selectedVersion, additionalInfo: null);

            // Assert
            Assert.NotNull(model.SelectedVersion);
            Assert.NotNull(model.InstalledVersion);
            Assert.False(model.IsSelectedVersionInstalled);
        }

        [Fact]
        public async Task IsSelectedVersionInstalled_WhenSelectedVersionAndInstalledVersionAreEqual_ReturnsTrue()
        {
            // Arrange
            var model = new PackageDetailControlModel(
                Mock.Of<IServiceBroker>(),
                Mock.Of<INuGetSolutionManagerService>(),
                Enumerable.Empty<IProjectContextInfo>(),
                uiController: Mock.Of<INuGetUI>());

            NuGetVersion installedVersion = NuGetVersion.Parse("1.0.0");

            var searchService = new Mock<INuGetSearchService>();
            var identity = new PackageIdentity("package", installedVersion);
            var vulnerableCapability = new Mock<IVulnerableCapable>();
            var embeddedResourceCapability = new Mock<IEmbeddedResourcesCapable>();
            var packagePath = "C:\\TestPackage";
            var packageModel = PackageModelCreationTestHelper.CreateLocalPackageModel(identity, packagePath, embeddedResourceCapability.Object);

            var packageItemModel = new PackageItemViewModel(searchService.Object, packageModel: packageModel)
            {
                InstalledVersion = installedVersion,
            };

            // Act
            await model.SetCurrentPackageAsync(
                packageItemModel,
                ItemFilter.All,
                () => null,
                CancellationToken.None);

            model.SelectedVersion = new DisplayVersion(installedVersion, additionalInfo: null);

            // Assert
            Assert.NotNull(model.SelectedVersion);
            Assert.NotNull(model.InstalledVersion);
            Assert.True(model.IsSelectedVersionInstalled);
        }
    }

    public class LocalPackageSolutionDetailControlModelTests : LocalDetailControlModelTestBase
    {
        private PackageSolutionDetailControlModel _testInstance;

        public LocalPackageSolutionDetailControlModelTests(GlobalServiceProvider sp, LocalPackageSearchMetadataFixture testData)
            : base(sp, testData)
        {
            var solMgr = new Mock<INuGetSolutionManagerService>();
            var serviceBroker = new Mock<IServiceBroker>();
            var projectManagerService = new Mock<INuGetProjectManagerService>();
            projectManagerService.Setup(x => x.GetProjectsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<IProjectContextInfo>());

#pragma warning disable ISB001 // Dispose of proxies
            serviceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(It.Is<ServiceJsonRpcDescriptor>(d => d.Moniker == NuGetServices.ProjectManagerService.Moniker), It.IsAny<ServiceActivationOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectManagerService.Object);
#pragma warning restore ISB001 // Dispose of proxies

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                _testInstance = await PackageSolutionDetailControlModel.CreateAsync(
                    solutionManager: solMgr.Object,
                    projects: new List<IProjectContextInfo>(),
                    serviceBroker: serviceBroker.Object,
                    uiController: Mock.Of<INuGetUI>(),
                    CancellationToken.None);
            });

            _testInstance.SetCurrentPackageAsync(
                _testViewModel,
                ItemFilter.All,
                () => null,
                CancellationToken.None).Wait();
        }
    }
}


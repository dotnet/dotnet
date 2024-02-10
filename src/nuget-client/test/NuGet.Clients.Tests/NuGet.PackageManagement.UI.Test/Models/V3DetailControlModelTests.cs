// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using Moq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.UI.Test.Models
{
    public abstract class V3DetailControlModelTestBase : IClassFixture<V3PackageSearchMetadataFixture>
    {
        protected readonly V3PackageSearchMetadataFixture _testData;
        protected readonly PackageItemViewModel _testViewModel;
        protected readonly Mock<IServiceBroker> _mockServiceBroker;
        protected Mock<INuGetUI> _mockNuGetUI;
        protected Mock<INuGetUIContext> _mockNuGetUIContext;
        protected Mock<PackageSourceMapping> _mockPackageSourceMapping;

        public V3DetailControlModelTestBase(GlobalServiceProvider sp, V3PackageSearchMetadataFixture testData)
        {
            sp.Reset();
            _testData = testData;

            // The versions pre-baked into the view model provide data for the first step of metadata extraction
            // which fails (null) in a V3 scenario--they need to be extracted using a metadata provider (below)
            var testVersion = new NuGetVersion(0, 0, 1);

            var packageSearchMetadata = new List<PackageSearchMetadataContextInfo>()
            {
                PackageSearchMetadataContextInfo.Create(_testData.TestData)
            };

            _mockNuGetUI = new Mock<INuGetUI>();
            _mockNuGetUIContext = new Mock<INuGetUIContext>();

            _mockPackageSourceMapping = new Mock<PackageSourceMapping>(new Dictionary<string, IReadOnlyList<string>>());
            _mockNuGetUIContext.Setup(_ => _.PackageSourceMapping).Returns(_mockPackageSourceMapping.Object);
            _mockNuGetUI.Setup(_ => _.UIContext).Returns(_mockNuGetUIContext.Object);

            var searchService = new Mock<INuGetSearchService>();
            _testViewModel = new PackageItemViewModel(searchService.Object)
            {
                Id = "nuget.psm",
                Version = testVersion,
                InstalledVersion = testVersion,
                Sources = new List<PackageSourceContextInfo> { new PackageSourceContextInfo("nuget.psm.test") },
            };

            var mockSearchService = new Mock<INuGetSearchService>();
            mockSearchService.Setup(x =>
                x.GetPackageMetadataListAsync(
                    It.IsAny<string>(),
                    It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<PackageSearchMetadataContextInfo>>(packageSearchMetadata));

            mockSearchService.Setup(x => x.GetPackageMetadataAsync(
                    It.IsAny<PackageIdentity>(),
                    It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo)>((packageSearchMetadata[0], null)));

            _mockServiceBroker = new Mock<IServiceBroker>();
#pragma warning disable ISB001 // Dispose of proxies
            _mockServiceBroker.Setup(
                x => x.GetProxyAsync<INuGetSearchService>(
                    NuGetServices.SearchService,
                    It.IsAny<ServiceActivationOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<INuGetSearchService>(mockSearchService.Object));
#pragma warning restore ISB001 // Dispose of proxies
        }

        protected void ConfigureNuGetUIWithPackageSourceMapping(ReadOnlyDictionary<string, IReadOnlyList<string>> packageSourceMappingPatterns)
        {
            if (packageSourceMappingPatterns != null)
            {
                _mockPackageSourceMapping.Reset();
                _mockPackageSourceMapping = new Mock<PackageSourceMapping>(packageSourceMappingPatterns);
                _mockNuGetUIContext.Setup(_ => _.PackageSourceMapping).Returns(_mockPackageSourceMapping.Object);
            }
        }

        /// <summary>
        /// Due to embedding the types we need to compare based on IsEquivalentTo
        /// </summary>
        protected class TypeEquivalenceComparer : IEqualityComparer<Type>
        {
            public static readonly TypeEquivalenceComparer Instance = new TypeEquivalenceComparer();

            private TypeEquivalenceComparer()
            {
            }

            public bool Equals(Type x, Type y)
            {
                return x.IsEquivalentTo(y);
            }

            public int GetHashCode(Type obj)
            {
                return obj.GUID.GetHashCode();
            }
        }
    }

    [Collection(MockedVS.Collection)]
    public class V3PackageDetailControlModelTests : V3DetailControlModelTestBase, IAsyncServiceProvider
    {
        private readonly Dictionary<Type, Task<object>> _services = new Dictionary<Type, Task<object>>(TypeEquivalenceComparer.Instance);
        private readonly PackageDetailControlModel _testInstance;

        public V3PackageDetailControlModelTests(GlobalServiceProvider sp, V3PackageSearchMetadataFixture testData)
                : base(sp, testData)
        {
            var solMgr = new Mock<INuGetSolutionManagerService>();

            var packageSearchMetadata = new List<PackageSearchMetadataContextInfo>()
            {
                PackageSearchMetadataContextInfo.Create(_testData.TestData)
            };

            var mockSearchService = new Mock<INuGetSearchService>();
            mockSearchService.Setup(x =>
                x.GetPackageMetadataListAsync(
                    It.IsAny<string>(),
                    It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<PackageSearchMetadataContextInfo>>(packageSearchMetadata));

            mockSearchService.Setup(x => x.GetPackageMetadataAsync(
                    It.IsAny<PackageIdentity>(),
                    It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<(PackageSearchMetadataContextInfo, PackageDeprecationMetadataContextInfo)>((packageSearchMetadata[0], null)));

            _testInstance = new PackageDetailControlModel(
                _mockServiceBroker.Object,
                solutionManager: solMgr.Object,
                Array.Empty<IProjectContextInfo>(),
                uiController: _mockNuGetUI.Object);
            _testInstance.SetCurrentPackageAsync(
                _testViewModel,
                ItemFilter.All,
                () => null).Wait();
        }

        [Fact]
        public void ViewModelMarkedVulnerableWhenMetadataHasVulnerability_Flagged()
        {
            Assert.True(_testInstance.IsPackageVulnerable);
        }

        [Fact]
        public void MaxVulnerabilitySeverityWhenMetadataHasVulnerability_Calculated()
        {
            Assert.Equal(_testInstance.PackageVulnerabilityMaxSeverity, _testData.TestData.Vulnerabilities.Max(v => v.Severity));
        }

        [Fact]
        public void VulnerabilityCountWhenMetadataHasVulnerability_Calculated()
        {
            Assert.Equal(_testInstance.PackageVulnerabilityCount, _testData.TestData.Vulnerabilities.Count());
        }

        [Fact]
        public void SetInstalledOrUpdateButtonIsEnabled_AfterPackageSourceMappingChanges_CanInstallWithPackageSourceMapping()
        {
            var packageIDWithSourceMapping = "a";
            var patterns = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { packageIDWithSourceMapping } }
            };
            var packageSourceMappingPatterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(patterns);

            _testInstance.SelectedVersion = new DisplayVersion(NuGetVersion.Parse("1.1.1"), additionalInfo: null);

            bool setInstalledOrUpdateButtonIsEnabled_RaisedPropertyChange_IsInstallOrUpdateButtonEnabled = false;
            var mockPropertyChangedEventHandler = new Mock<IPropertyChangedEventHandler>();
            mockPropertyChangedEventHandler.Setup(x => x.PropertyChanged(
                It.IsAny<object>(),
                It.IsAny<PropertyChangedEventArgs>()
            ))
            .Callback<object, PropertyChangedEventArgs>((d, p) =>
            {
                var detail = d as PackageDetailControlModel;
                if (detail != null
                    && p.PropertyName == nameof(PackageDetailControlModel.IsInstallorUpdateButtonEnabled))
                {
                    setInstalledOrUpdateButtonIsEnabled_RaisedPropertyChange_IsInstallOrUpdateButtonEnabled = detail.IsInstallorUpdateButtonEnabled;
                }
            });

            _testInstance.PropertyChanged += mockPropertyChangedEventHandler.Object.PropertyChanged;

            var beforeEnablingPackageSourceMapping_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;
            var beforeEnablingPackageSourceMapping_IsInstallOrUpdateButtonEnabled = _testInstance.IsInstallorUpdateButtonEnabled;

            PackageSourceMoniker singlePackageSourceMoniker = new("sourceName", new List<PackageSourceContextInfo>() { new PackageSourceContextInfo("sourceName") }, priorityOrder: 0);
            PackageSourceMoniker aggregatePackageSourceMoniker = new("sourceName",
                new List<PackageSourceContextInfo>() { new PackageSourceContextInfo("sourceName"), new PackageSourceContextInfo("sourceName2") },
                priorityOrder: 0);

            // Act

            //********************************************************************************************************************************************
            // Enable package source mapping; select an Aggregate package source so it won't allow automatically create a mapping when pressing Install.
            //********************************************************************************************************************************************
            ConfigureNuGetUIWithPackageSourceMapping(packageSourceMappingPatterns);
            _mockNuGetUI.Setup(_ => _.ActivePackageSourceMoniker).Returns(aggregatePackageSourceMoniker);

            var packageNotMapped_AggregateSourceSelected_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;
            var packageNotMapped_AggregateSourceSelected_IsInstallorUpdateButtonEnabled = _testInstance.IsInstallorUpdateButtonEnabled;

            //********************************************************************************************************************************************
            // Select a single package source so it will allow automatically create a mapping when pressing Install.
            //********************************************************************************************************************************************
            _mockNuGetUI.Setup(_ => _.ActivePackageSourceMoniker).Returns(singlePackageSourceMoniker);
            var packageNotMapped_SingleSourceSelected_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;
            var packageNotMapped_SingleSourceSelected_IsInstallOrUpdateButtonEnabled = _testInstance.IsInstallorUpdateButtonEnabled;

            //********************************************************************************************************************************************
            // Select a package which has a configured Package Source Mapping.
            //********************************************************************************************************************************************
            _testInstance.PackageSourceMappingViewModel.PackageId = packageIDWithSourceMapping;
            var afterSelectingPackageWithPackageSourceMapping_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;
            var afterSelectingPackageWithPackageSourceMapping_IsInstallOrUpdateButtonEnabled = _testInstance.IsInstallorUpdateButtonEnabled;

            // Explicitly trigger PropertyChanged event.
            _testInstance.SetInstalledOrUpdateButtonIsEnabled();
            var afterSetInstalledOrUpdateButtonIsEnabled_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;

            // Assert
            Assert.True(beforeEnablingPackageSourceMapping_CanInstallWithPackageSourceMapping, "Package Source Mapping is disabled.");
            Assert.True(beforeEnablingPackageSourceMapping_IsInstallOrUpdateButtonEnabled, "Package Source Mapping is disabled.");

            Assert.False(packageNotMapped_AggregateSourceSelected_CanInstallWithPackageSourceMapping,
                "Package Source Mapping is enabled but the Selected Package ID has no mapping.");
            Assert.False(packageNotMapped_AggregateSourceSelected_IsInstallorUpdateButtonEnabled,
                "Package Source Mapping is enabled but the Selected Package ID has no mapping.");

            Assert.True(packageNotMapped_SingleSourceSelected_CanInstallWithPackageSourceMapping,
                "Selected Package ID doesn't have a mapping but should automatically get one during Install.");
            Assert.True(packageNotMapped_SingleSourceSelected_IsInstallOrUpdateButtonEnabled,
                "Selected Package ID will automatically get a package source mapping, but the " + nameof(PackageDetailControlModel.IsInstallorUpdateButtonEnabled) + " hasn't been updated, yet.");

            Assert.True(afterSelectingPackageWithPackageSourceMapping_CanInstallWithPackageSourceMapping,
                "Selected Package ID has a package source mapping.");
            Assert.True(afterSelectingPackageWithPackageSourceMapping_IsInstallOrUpdateButtonEnabled,
                "Selected Package ID has a package source mapping, but the " + nameof(PackageDetailControlModel.IsInstallorUpdateButtonEnabled) + " hasn't been updated, yet.");
            Assert.True(afterSetInstalledOrUpdateButtonIsEnabled_CanInstallWithPackageSourceMapping, "Package Source Mapping is enabled and the Package ID is mapped.");

            Assert.True(setInstalledOrUpdateButtonIsEnabled_RaisedPropertyChange_IsInstallOrUpdateButtonEnabled,
                nameof(PackageDetailControlModel.IsInstallorUpdateButtonEnabled) + " should have raised a PropertyChanged when calling "
                + nameof(DetailControlModel.SetInstalledOrUpdateButtonIsEnabled) + " and the value should become true.");
        }

        [Fact]
        public void PackageVulnerabilities_WhenMetadataHasVulnerability_IsOrderedBySeverityDescending()
        {
            IEnumerable<PackageVulnerabilityMetadataContextInfo> sortedTestVulnerabilities =
                _testData.TestData.Vulnerabilities
                .OrderByDescending(v => v.Severity)
                .Select(v => new PackageVulnerabilityMetadataContextInfo(v.AdvisoryUrl, v.Severity));

            Assert.Equal(sortedTestVulnerabilities, _testInstance.PackageVulnerabilities);
        }

        [Fact]
        public async Task SetCurrentPackageAsync_SortsVersions_ByNuGetVersionDesc()
        {
            // Arrange
            NuGetVersion installedVersion = NuGetVersion.Parse("1.0.0");

            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01248")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01249")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01256")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01265")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0-dev-01187")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0-dev-01191")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0-dev-01211")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0")),
            };

            var searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            var vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "package",
                InstalledVersion = installedVersion,
                Version = installedVersion,
            };

            // Act

            await _testInstance.SetCurrentPackageAsync(
                vm,
                ItemFilter.All,
                () => vm);

            // Assert
            // Remove any added `null` separators, and any Additional Info entries (eg, "Latest Prerelease", "Latest Stable").
            List<DisplayVersion> actualVersions = _testInstance.Versions
                .Where(v => v != null && v.AdditionalInfo == null).ToList();

            var expectedVersions = new List<DisplayVersion>() {
                new DisplayVersion(version: new NuGetVersion("2.10.1-dev-01265"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.1-dev-01256"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.1-dev-01249"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.1-dev-01248"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.0"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.0-dev-01211"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.0-dev-01191"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.0-dev-01187"), additionalInfo: null),
            };

            Assert.Equal(expectedVersions, actualVersions);
        }

        [Fact]
        public async Task SetCurrentPackageAsync_WhenFloatingVersions()
        {
            // Arrange
            NuGetVersion installedVersion = NuGetVersion.Parse("1.0.0");

            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(new NuGetVersion("3.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("2.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("1.0.0-beta")),
                new VersionInfoContextInfo(new NuGetVersion("0.0.1")),
            };

            var searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            var vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "package",
                InstalledVersion = installedVersion,
                Version = installedVersion,
            };

            // Act

            await _testInstance.SetCurrentPackageAsync(
                vm,
                ItemFilter.All,
                () => vm);

            // Assert
            // Remove any added `null` separators, and any Additional Info entries (eg, "Latest Prerelease", "Latest Stable").
            List<DisplayVersion> actualVersions = _testInstance.Versions
                .Where(v => v != null && v.AdditionalInfo == null).ToList();

            var expectedVersions = new List<DisplayVersion>() {
                new DisplayVersion(version: new NuGetVersion("3.0.0"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.0.0"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("1.0.0-beta"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("0.0.1"), additionalInfo: null),
            };

            Assert.Equal(expectedVersions, actualVersions);
        }

        [Theory]
        [InlineData(ItemFilter.All, "3.0.0")]
        [InlineData(ItemFilter.Installed, "1.0.0")]
        [InlineData(ItemFilter.UpdatesAvailable, "3.0.0")]
        public async Task SetCurrentPackageAsync_CorrectSelectedVersion(ItemFilter tab, string expectedSelectedVersion)
        {
            // Arrange
            NuGetVersion installedVersion = NuGetVersion.Parse("1.0.0");

            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01248")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0")),
                new VersionInfoContextInfo(new NuGetVersion("3.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("1.0.0")),
            };

            var searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            var vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "package",
                InstalledVersion = installedVersion,
                Version = installedVersion,
            };

            // Act

            await _testInstance.SetCurrentPackageAsync(
                vm,
                tab,
                () => vm);

            NuGetVersion selectedVersion = NuGetVersion.Parse(expectedSelectedVersion);

            Assert.Equal(_testInstance.SelectedVersion.Version, selectedVersion);
        }

        [Fact]
        public async Task SetCurrentPackageAsync_ClearVersions_Always()
        {
            // Arrange
            var installedVersion = NuGetVersion.Parse("1.0.0");

            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(new NuGetVersion("1.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("1.0.1")),
            };

            var mockPropertyChangedEventHandler = new Mock<IPropertyChangedEventHandler>();
            var wasVersionsListCleared = false;

            var searchService = new Mock<INuGetSearchService>();
            searchService.Setup(s => s.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);
            var vm = new PackageItemViewModel(searchService.Object);

            vm.Id = "a";
            vm.Sources = new ReadOnlyCollection<PackageSourceContextInfo>(new List<PackageSourceContextInfo>());
            vm.InstalledVersion = installedVersion;
            vm.Version = installedVersion;

            // Test Setup already selected a package.
            int previousVersionListCount = _testInstance.Versions.Count;

            mockPropertyChangedEventHandler.Setup(x => x.PropertyChanged(
                It.IsAny<object>(),
                It.IsAny<PropertyChangedEventArgs>()
            ))
            .Callback<object, PropertyChangedEventArgs>((d, p) =>
            {
                DetailControlModel detail = d as DetailControlModel;
                if (detail != null
                    && detail.Versions.Count == 0
                    && p.PropertyName == nameof(DetailControlModel.Versions))
                {
                    wasVersionsListCleared = true;
                }
            });

            _testInstance.PropertyChanged += mockPropertyChangedEventHandler.Object.PropertyChanged;

            // Act

            //Select a different VM which should clear the Versions list from the previous selection.
            await _testInstance.SetCurrentPackageAsync(
                vm,
                ItemFilter.All,
                () => vm);

            // Assert

            Assert.True(previousVersionListCount > 0, "Test setup did not pre-populate versions list.");
            Assert.True(wasVersionsListCleared, "Versions list was not cleared.");
        }


        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsNotLatest(string allowedVersions, string installedVersion)
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse(allowedVersions), new NuGetVersion(installedVersion), string.Empty),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), "Latest stable"),
                null,
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsLatest(string allowedVersions, string installedVersion)
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse(allowedVersions), new NuGetVersion(installedVersion), string.Empty),
                null,
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsNotLatest_IncludePrerelease(string allowedVersions, string installedVersion)
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse(allowedVersions), new NuGetVersion(installedVersion), string.Empty),
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), "Latest prerelease"),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), "Latest stable"),
                null,
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), null),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsLatestPrerelease_IncludePrerelease(string allowedVersions, string installedVersion)
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse(allowedVersions), new NuGetVersion(installedVersion), string.Empty),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), "Latest stable"),
                null,
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), null),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsLatestStable_IncludePrerelease(string allowedVersions, string installedVersion)
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse(allowedVersions), new NuGetVersion(installedVersion), string.Empty),
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), "Latest prerelease"),
                null,
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), null),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        private List<VersionInfoContextInfo> ExpectedVersionsList()
        {
            return new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(new NuGetVersion("3.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("2.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("1.0.0-beta")),
                new VersionInfoContextInfo(new NuGetVersion("1.1.0")),
                new VersionInfoContextInfo(new NuGetVersion("1.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("0.0.1")),
            };
        }

        private List<VersionInfoContextInfo> ExpectedVersionsList_IncludePrerelease()
        {
            return new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(new NuGetVersion("3.0.1-beta")),
                new VersionInfoContextInfo(new NuGetVersion("3.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("2.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("1.0.0-beta")),
                new VersionInfoContextInfo(new NuGetVersion("1.1.0")),
                new VersionInfoContextInfo(new NuGetVersion("1.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("0.0.1")),
            };
        }

        public static IEnumerable<object[]> FloatingVersions_TestCases()
        {
            yield return new object[] { "3.*", "3.0.0", false, false };
            yield return new object[] { "[2.9,)", "3.0.0", false, false };
            yield return new object[] { "*", "3.0.0", false, false };
            yield return new object[] { "2.*", "2.0.0", false, false };
            yield return new object[] { "(1.*,)", "1.1.0", false, false };
            yield return new object[] { "(1.1*,)", "2.0.0", false, false };
            yield return new object[] { "[2.*,)", "2.0.0", false, false };
            yield return new object[] { "3.*", "3.0.0", false, true };
            yield return new object[] { "[2.9,)", "3.0.0", false, true };
            yield return new object[] { "*", "3.0.0", false, true };
            yield return new object[] { "2.*", "2.0.0", false, true };
            yield return new object[] { "(1.*,)", "1.1.0", false, true };
            yield return new object[] { "(1.1*,)", "2.0.0", false, true };
            yield return new object[] { "[2.*,)", "2.0.0", false, true };
            yield return new object[] { "2.0", "2.0.0", false, true };
            yield return new object[] { "2.0.0", "2.0.0", false, true };
            yield return new object[] { "2", "2.0.0", false, true };
            yield return new object[] { "3.0", "3.0.0", false, true };
            yield return new object[] { "3.0.0", "3.0.0", false, true };
            yield return new object[] { "3", "3.0.0", false, true };
        }

        [Theory]
        [MemberData(nameof(FloatingVersions_TestCases))]
        [InlineData("3.0", "3.0.0", true, false)]
        [InlineData("3.0.0", "3.0.0", true, false)]
        [InlineData("3", "3.0.0", true, false)]
        [InlineData("3.0.1-beta", "3.0.1-beta", true, true)]
        public async void WhenPackageStyleIsPackageReference_And_CustomVersion_InstalledTab_IsSelectedVersionCorrect(string allowedVersions, string installedVersion, bool isLatest, bool includePrerelease)
        {
            // Arange project
            Mock<IServiceBroker> mockServiceBroker = new Mock<IServiceBroker>();
            Mock<INuGetSearchService> mockSearchService = new Mock<INuGetSearchService>();

            PackageIdentity packageIdentity = new PackageIdentity("Contoso.A", NuGetVersion.Parse(installedVersion));

            PackageReferenceContextInfo[] installedPackages = new PackageReferenceContextInfo[]
            {
                PackageReferenceContextInfo.Create(
                    new PackageReference(
                        packageIdentity,
                        NuGetFramework.Parse("net45"),
                        userInstalled: true,
                        developmentDependency: false,
                        requireReinstallation: false,
                        allowedVersions: VersionRange.Parse(allowedVersions)))
            };

            Mock<INuGetProjectManagerService> projectManagerService = new Mock<INuGetProjectManagerService>();
            projectManagerService.Setup(x => x.GetInstalledPackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedPackages));

#pragma warning disable ISB001 // Dispose of proxies
            mockServiceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(It.Is<ServiceJsonRpcDescriptor>(d => d.Moniker == NuGetServices.ProjectManagerService.Moniker), It.IsAny<ServiceActivationOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectManagerService.Object);
#pragma warning restore ISB001 // Dispose of proxies

            // Setup project
            Mock<IProjectContextInfo> project = new Mock<IProjectContextInfo>();
            project.SetupGet(p => p.ProjectKind).Returns(NuGetProjectKind.PackageReference);
            project.SetupGet(p => p.ProjectStyle).Returns(ProjectModel.ProjectStyle.PackageReference);
            project.SetupGet(p => p.ProjectId).Returns("ProjectId");

            PackageDetailControlModel model = new PackageDetailControlModel(
                mockServiceBroker.Object,
                solutionManager: new Mock<INuGetSolutionManagerService>().Object,
                projects: new[] { project.Object },
                uiController: _mockNuGetUI.Object);

            // Arrange
            List<VersionInfoContextInfo> testVersions = includePrerelease ? ExpectedVersionsList_IncludePrerelease() : ExpectedVersionsList();

            Mock<INuGetSearchService> searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            // Act
            PackageItemViewModel vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "Contoso.A",
                Sources = new List<PackageSourceContextInfo> { new PackageSourceContextInfo("test_source") },
                InstalledVersion = packageIdentity.Version,
                AllowedVersions = VersionRange.Parse(allowedVersions),
                Version = packageIdentity.Version,
            };

            await model.SetCurrentPackageAsync(
                vm,
                ItemFilter.Installed,
                () => vm);

            // Assert
            VersionRange installedVersionRange = VersionRange.Parse(allowedVersions, true);
            NuGetVersion bestVersion = installedVersionRange.FindBestMatch(testVersions.Select(t => t.Version));
            DisplayVersion displayVersion = new DisplayVersion(installedVersionRange, bestVersion, additionalInfo: string.Empty);

            ItemsChangeObservableCollection<DisplayVersion> assertVersions;
            if (includePrerelease)
            {
                NuGetVersion version;
                NuGetVersion.TryParse(allowedVersions, out version);
                if (version != null && version.Version.Equals(new NuGetVersion("3.0.0").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestStable_IncludePrerelease(allowedVersions, installedVersion);
                }
                else if (version != null && version.Version.Equals(new NuGetVersion("3.0.1-beta").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestPrerelease_IncludePrerelease(allowedVersions, installedVersion);
                }
                else
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsNotLatest_IncludePrerelease(allowedVersions, installedVersion);
                }
            }
            else
            {
                assertVersions = isLatest ? VersionsList_WhenInstalledVersion_IsLatest(allowedVersions, installedVersion)
                                    : VersionsList_WhenInstalledVersion_IsNotLatest(allowedVersions, installedVersion);
            }

            Assert.Equal(model.SelectedVersion.ToString(), allowedVersions);
            Assert.Equal(model.Versions.FirstOrDefault(), displayVersion);
            Assert.Equal(model.SelectedVersion, displayVersion);
            Assert.Equal(model.Versions, assertVersions);
            Assert.True(model.CanInstallWithPackageSourceMapping);
            Assert.Equal(model.IsInstallorUpdateButtonEnabled, false);
        }

        [Theory]
        [MemberData(nameof(FloatingVersions_TestCases))]

        public async void WhenPackageStyleIsPackageReference_And_CustomVersion_UpdatesTab_IsSelectedVersionCorrect(string allowedVersions, string installedVersion, bool isLatest, bool includePrerelease)
        {
            // Assert
            // Updates Tab wont show package if it is latest
            Assert.Equal(isLatest, false);

            // Arange project
            Mock<IServiceBroker> mockServiceBroker = new Mock<IServiceBroker>();
            Mock<INuGetSearchService> mockSearchService = new Mock<INuGetSearchService>();

            PackageIdentity packageIdentity = new PackageIdentity("Contoso.A", NuGetVersion.Parse(installedVersion));

            PackageReferenceContextInfo[] installedPackages = new PackageReferenceContextInfo[]
            {
                PackageReferenceContextInfo.Create(
                    new PackageReference(
                        packageIdentity,
                        NuGetFramework.Parse("net45"),
                        userInstalled: true,
                        developmentDependency: false,
                        requireReinstallation: false,
                        allowedVersions: VersionRange.Parse(allowedVersions)))
            };

            Mock<INuGetProjectManagerService> projectManagerService = new Mock<INuGetProjectManagerService>();
            projectManagerService.Setup(x => x.GetInstalledPackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedPackages));

#pragma warning disable ISB001 // Dispose of proxies
            mockServiceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(It.Is<ServiceJsonRpcDescriptor>(d => d.Moniker == NuGetServices.ProjectManagerService.Moniker), It.IsAny<ServiceActivationOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectManagerService.Object);
#pragma warning restore ISB001 // Dispose of proxies

            Mock<IProjectContextInfo> project = new Mock<IProjectContextInfo>();

            project.SetupGet(p => p.ProjectKind).Returns(NuGetProjectKind.PackageReference);
            project.SetupGet(p => p.ProjectStyle).Returns(ProjectModel.ProjectStyle.PackageReference);
            project.SetupGet(p => p.ProjectId).Returns("ProjectId");

            PackageDetailControlModel model = new PackageDetailControlModel(
                mockServiceBroker.Object,
                solutionManager: new Mock<INuGetSolutionManagerService>().Object,
                projects: new[] { project.Object },
                uiController: _mockNuGetUI.Object);

            // Arrange
            List<VersionInfoContextInfo> testVersions = includePrerelease ? ExpectedVersionsList_IncludePrerelease() : ExpectedVersionsList();

            Mock<INuGetSearchService> searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            // Act
            PackageItemViewModel vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "Contoso.A",
                Sources = new List<PackageSourceContextInfo> { new PackageSourceContextInfo("Contoso.A.test") },
                InstalledVersion = packageIdentity.Version,
                AllowedVersions = VersionRange.Parse(allowedVersions),
                IncludePrerelease = includePrerelease,
                Version = packageIdentity.Version,
            };

            await model.SetCurrentPackageAsync(
                vm,
                ItemFilter.UpdatesAvailable,
                () => vm);

            // Assert
            VersionRange installedVersionRange = VersionRange.Parse(allowedVersions, true);
            NuGetVersion bestVersion = installedVersionRange.FindBestMatch(testVersions.Select(t => t.Version));
            DisplayVersion displayVersion = new DisplayVersion(installedVersionRange, bestVersion, additionalInfo: string.Empty);

            ItemsChangeObservableCollection<DisplayVersion> assertVersions;
            string expectedAditionalInfo = null;
            NuGetVersion version;
            NuGetVersion.TryParse(allowedVersions, out version);
            if (includePrerelease)
            {
                if (version != null && version.Version.Equals(new NuGetVersion("3.0.0").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestStable_IncludePrerelease(allowedVersions, installedVersion);
                    expectedAditionalInfo = "Latest prerelease";
                }
                else if (version != null && version.Version.Equals(new NuGetVersion("3.0.1-beta").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestPrerelease_IncludePrerelease(allowedVersions, installedVersion);
                    expectedAditionalInfo = string.Empty;
                }
                else
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsNotLatest_IncludePrerelease(allowedVersions, installedVersion);
                    expectedAditionalInfo = "Latest prerelease";
                }
            }
            else
            {
                assertVersions = VersionsList_WhenInstalledVersion_IsNotLatest(allowedVersions, installedVersion);
                if (isLatest)
                {
                    expectedAditionalInfo = (version != null && version.ToString().Equals(new NuGetVersion("3.0.0").ToString())) ? string.Empty : null;
                }
                else
                {
                    expectedAditionalInfo = "Latest stable";
                }
            }

            Assert.Equal(model.SelectedVersion.Version.ToString(), includePrerelease ? "3.0.1-beta" : "3.0.0");
            Assert.Equal(model.SelectedVersion.AdditionalInfo, expectedAditionalInfo);
            Assert.Equal(model.Versions.FirstOrDefault(), displayVersion);
            Assert.Equal(model.Versions, assertVersions);
            Assert.True(model.CanInstallWithPackageSourceMapping);
            Assert.Equal(model.IsInstallorUpdateButtonEnabled, true);
        }

        [Theory]
        [MemberData(nameof(FloatingVersions_TestCases))]
        // Browse Tab cases
        [InlineData("3.0", "3.0.0", true, false)]
        [InlineData("3.0.0", "3.0.0", true, false)]
        [InlineData("3", "3.0.0", true, false)]
        [InlineData("3.0.1-beta", "3.0.1-beta", true, true)]
        public async void WhenPackageStyleIsPackageReference_And_CustomVersion_BrowseTab_IsSelectedVersionCorrect(string allowedVersions, string installedVersion, bool isLatest, bool includePrerelease)
        {
            // Arange project
            Mock<IServiceBroker> mockServiceBroker = new Mock<IServiceBroker>();
            Mock<INuGetSearchService> mockSearchService = new Mock<INuGetSearchService>();

            PackageIdentity packageIdentity = new PackageIdentity("Contoso.A", NuGetVersion.Parse(installedVersion));

            PackageReferenceContextInfo[] installedPackages = new PackageReferenceContextInfo[]

            {
                PackageReferenceContextInfo.Create(
                    new PackageReference(
                        packageIdentity,
                        NuGetFramework.Parse("net45"),
                        userInstalled: true,
                        developmentDependency: false,
                        requireReinstallation: false,
                        allowedVersions: VersionRange.Parse(allowedVersions)))
            };

            Mock<INuGetProjectManagerService> projectManagerService = new Mock<INuGetProjectManagerService>();
            projectManagerService.Setup(x => x.GetInstalledPackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedPackages));

#pragma warning disable ISB001 // Dispose of proxies
            mockServiceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(It.Is<ServiceJsonRpcDescriptor>(d => d.Moniker == NuGetServices.ProjectManagerService.Moniker), It.IsAny<ServiceActivationOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectManagerService.Object);
#pragma warning restore ISB001 // Dispose of proxies

            Mock<IProjectContextInfo> project = new Mock<IProjectContextInfo>();

            project.SetupGet(p => p.ProjectKind).Returns(NuGetProjectKind.PackageReference);
            project.SetupGet(p => p.ProjectStyle).Returns(ProjectModel.ProjectStyle.PackageReference);
            project.SetupGet(p => p.ProjectId).Returns("ProjectId");

            PackageDetailControlModel model = new PackageDetailControlModel(
                mockServiceBroker.Object,
                solutionManager: new Mock<INuGetSolutionManagerService>().Object,
                projects: new[] { project.Object },
                uiController: _mockNuGetUI.Object);

            // Arrange
            List<VersionInfoContextInfo> testVersions = includePrerelease ? ExpectedVersionsList_IncludePrerelease() : ExpectedVersionsList();

            Mock<INuGetSearchService> searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            // Act
            PackageItemViewModel vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "Contoso.A",
                Sources = new List<PackageSourceContextInfo> { new PackageSourceContextInfo("Contoso.A.test") },
                InstalledVersion = NuGetVersion.Parse(installedVersion),
                AllowedVersions = VersionRange.Parse(allowedVersions),
                IncludePrerelease = includePrerelease,
                Version = NuGetVersion.Parse(installedVersion),
            };

            await model.SetCurrentPackageAsync(
                vm,
                ItemFilter.All,
                () => vm);

            // Assert
            VersionRange installedVersionRange = VersionRange.Parse(allowedVersions, true);
            NuGetVersion bestVersion = installedVersionRange.FindBestMatch(testVersions.Select(t => t.Version));
            DisplayVersion displayVersion = new DisplayVersion(installedVersionRange, bestVersion, additionalInfo: string.Empty);

            ItemsChangeObservableCollection<DisplayVersion> assertVersions;
            string expectedAditionalInfo = null;
            NuGetVersion version;
            NuGetVersion.TryParse(allowedVersions, out version);
            if (includePrerelease)
            {
                if (version != null && version.Version.Equals(new NuGetVersion("3.0.0").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestStable_IncludePrerelease(allowedVersions, installedVersion);
                    expectedAditionalInfo = "Latest prerelease";
                }
                else if (version != null && version.Version.Equals(new NuGetVersion("3.0.1-beta").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestPrerelease_IncludePrerelease(allowedVersions, installedVersion);
                    expectedAditionalInfo = string.Empty;
                }
                else
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsNotLatest_IncludePrerelease(allowedVersions, installedVersion);
                    expectedAditionalInfo = "Latest prerelease";
                }
            }
            else
            {
                assertVersions = isLatest ? VersionsList_WhenInstalledVersion_IsLatest(allowedVersions, installedVersion) : VersionsList_WhenInstalledVersion_IsNotLatest(allowedVersions, installedVersion);
                if (isLatest)
                {
                    expectedAditionalInfo = (version != null && version.ToString().Equals(new NuGetVersion("3.0.0").ToString())) ? string.Empty : null;
                }
                else
                {
                    expectedAditionalInfo = "Latest stable";
                }
            }

            // Some InstalledVersion resolve to the LatestVersion but are different in version range
            var shouldButtonBeEnabled = isLatest ? !model.SelectedVersion?.Range?.OriginalString.Equals(model.InstalledVersionRange?.OriginalString) : true;

            Assert.Equal(model.SelectedVersion.Version.ToString(), includePrerelease ? "3.0.1-beta" : "3.0.0");
            Assert.Equal(model.SelectedVersion.AdditionalInfo, expectedAditionalInfo);
            Assert.Equal(model.Versions.FirstOrDefault(), displayVersion);
            Assert.Equal(model.Versions, assertVersions);
            Assert.True(model.CanInstallWithPackageSourceMapping);
            Assert.Equal(model.IsInstallorUpdateButtonEnabled, shouldButtonBeEnabled);
        }

        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsLatest_NonPackageReferenceProject()
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsNotLatest_NonPackageReferenceProject()
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), "Latest stable"),
                null,
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsNotLatest_IncludePrerelease_NonPackageReferenceProject()
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), "Latest prerelease"),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), "Latest stable"),
                null,
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), null),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsLatestPrerelease_IncludePrerelease_NonPackageReferenceProject()
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), "Latest stable"),
                null,
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), null),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        private ItemsChangeObservableCollection<DisplayVersion> VersionsList_WhenInstalledVersion_IsLatestStable_IncludePrerelease_NonPackageReferenceProject()
        {
            return new ItemsChangeObservableCollection<DisplayVersion>() {
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), "Latest prerelease"),
                null,
                new DisplayVersion(VersionRange.Parse("3.0.1-beta"), new NuGetVersion("3.0.1-beta"), null),
                new DisplayVersion(VersionRange.Parse("3.0.0"), new NuGetVersion("3.0.0"), null),
                new DisplayVersion(VersionRange.Parse("2.0.0"), new NuGetVersion("2.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.1.0"), new NuGetVersion("1.1.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0"), new NuGetVersion("1.0.0"), null),
                new DisplayVersion(VersionRange.Parse("1.0.0-beta"), new NuGetVersion("1.0.0-beta"), null),
                new DisplayVersion(VersionRange.Parse("0.0.1"), new NuGetVersion("0.0.1"), null),
            };
        }

        public static IEnumerable<object[]> FloatingVersions_TestCases_NonPackageReferenceProject()
        {
            yield return new object[] { NuGetProjectKind.PackagesConfig, ProjectModel.ProjectStyle.PackagesConfig, null, "3.0.0", true, false };
            yield return new object[] { NuGetProjectKind.Unknown, ProjectModel.ProjectStyle.Unknown, "(0.1,3.4)", "3.0.0", true, false };
            yield return new object[] { NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "(0.1,3.4)", "3.0.0", true, false };
            yield return new object[] { NuGetProjectKind.Unknown, ProjectModel.ProjectStyle.DotnetCliTool, "*", "3.0.0", true, false };
            yield return new object[] { NuGetProjectKind.Unknown, ProjectModel.ProjectStyle.DotnetCliTool, "2.*", "2.0.0", false, false };
            yield return new object[] { NuGetProjectKind.Unknown, ProjectModel.ProjectStyle.Unknown, "[0.0.1,3.4)", "0.0.1", false, true };
            yield return new object[] { NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[1.0.0-beta,)", "1.0.0-beta", false, true };
            yield return new object[] { NuGetProjectKind.Unknown, ProjectModel.ProjectStyle.DotnetCliTool, "[1.0.0,)", "1.0.0", false, true };
            yield return new object[] { NuGetProjectKind.Unknown, ProjectModel.ProjectStyle.DotnetCliTool, "[1.1.0,)", "1.1.0", false, true };
            yield return new object[] { NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[3.0.0,)", "3.0.0", false, true };
        }

        [Theory]
        [MemberData(nameof(FloatingVersions_TestCases_NonPackageReferenceProject))]
        [InlineData(NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[3.0.1-beta,)", "3.0.1-beta", true, true)]
        [InlineData(NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[3,)", "3", true, false)]
        [InlineData(NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[3.0,)", "3.0", true, false)]
        [InlineData(NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[3.0.0,)", "3.0.0", true, false)]
        public async void WhenPackageStyleIsNotPackageReference_And_CustomVersion_InstalledTab_IsSelectedVersionCorrect(NuGetProjectKind projectKind, ProjectModel.ProjectStyle projectStyle, string allowedVersions, string installedVersion, bool isLatest, bool includePrerelease)
        {
            // Arange project
            Mock<IServiceBroker> mockServiceBroker = new Mock<IServiceBroker>();
            Mock<INuGetSearchService> mockSearchService = new Mock<INuGetSearchService>();

            PackageIdentity packageIdentity = new PackageIdentity("Contoso.A", NuGetVersion.Parse(installedVersion));

            PackageReferenceContextInfo[] installedPackages = new PackageReferenceContextInfo[]
            {
                PackageReferenceContextInfo.Create(
                    new PackageReference(
                        packageIdentity,
                        NuGetFramework.Parse("net45"),
                        userInstalled: true,
                        developmentDependency: false,
                        requireReinstallation: false,
                        allowedVersions: allowedVersions != null ? VersionRange.Parse(allowedVersions) : null))
            };

            Mock<INuGetProjectManagerService> projectManagerService = new Mock<INuGetProjectManagerService>();
            projectManagerService.Setup(x => x.GetInstalledPackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedPackages));

#pragma warning disable ISB001 // Dispose of proxies
            mockServiceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(It.Is<ServiceJsonRpcDescriptor>(d => d.Moniker == NuGetServices.ProjectManagerService.Moniker), It.IsAny<ServiceActivationOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectManagerService.Object);
#pragma warning restore ISB001 // Dispose of proxies

            Mock<IProjectContextInfo> project = new Mock<IProjectContextInfo>();

            project.SetupGet(p => p.ProjectKind).Returns(projectKind);
            project.SetupGet(p => p.ProjectStyle).Returns(projectStyle);
            project.SetupGet(p => p.ProjectId).Returns("ProjectId");

            PackageDetailControlModel model = new PackageDetailControlModel(
                mockServiceBroker.Object,
                solutionManager: new Mock<INuGetSolutionManagerService>().Object,
                projects: new[] { project.Object },
                uiController: _mockNuGetUI.Object);

            // Arrange
            List<VersionInfoContextInfo> testVersions = includePrerelease ? ExpectedVersionsList_IncludePrerelease() : ExpectedVersionsList();

            Mock<INuGetSearchService> searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            // Act
            PackageItemViewModel vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "Contoso.A",
                Sources = new List<PackageSourceContextInfo> { new PackageSourceContextInfo("Contoso.A.test") },
                InstalledVersion = NuGetVersion.Parse(installedVersion),
                Version = NuGetVersion.Parse(installedVersion),
            };

            await model.SetCurrentPackageAsync(
                vm,
                ItemFilter.Installed,
                () => vm);

            ItemsChangeObservableCollection<DisplayVersion> assertVersions;
            if (includePrerelease)
            {
                NuGetVersion version;
                NuGetVersion.TryParse(installedVersion, out version);
                if (version != null && version.Version.Equals(new NuGetVersion("3.0.0").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestStable_IncludePrerelease_NonPackageReferenceProject();
                }
                else if (version != null && version.Version.Equals(new NuGetVersion("3.0.1-beta").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestPrerelease_IncludePrerelease_NonPackageReferenceProject();
                }
                else
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsNotLatest_IncludePrerelease_NonPackageReferenceProject();
                }
            }
            else
            {
                assertVersions = isLatest ? VersionsList_WhenInstalledVersion_IsLatest_NonPackageReferenceProject()
                : VersionsList_WhenInstalledVersion_IsNotLatest_NonPackageReferenceProject();
            }

            // Assert
            Assert.NotEqual(model.SelectedVersion.ToString(), allowedVersions);
            Assert.Equal(model.SelectedVersion.Version, NuGetVersion.Parse(installedVersion));
            Assert.Equal(model.SelectedVersion.AdditionalInfo, null); // Always show the installed version
            Assert.Equal(model.Versions, assertVersions);
            Assert.True(model.CanInstallWithPackageSourceMapping);
            Assert.Equal(model.IsInstallorUpdateButtonEnabled, false);
        }

        [Theory]
        [MemberData(nameof(FloatingVersions_TestCases_NonPackageReferenceProject))]
        [InlineData(NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[3.0.1-beta,)", "3.0.1-beta", true, true)]
        [InlineData(NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[3,)", "3", true, false)]
        [InlineData(NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[3.0,)", "3.0", true, false)]
        [InlineData(NuGetProjectKind.ProjectK, ProjectModel.ProjectStyle.ProjectJson, "[3.0.0,)", "3.0.0", true, false)]
        public async void WhenPackageStyleIsNotPackageReference_And_CustomVersion_BrowseTab_IsSelectedVersionCorrect(NuGetProjectKind projectKind, ProjectModel.ProjectStyle projectStyle, string allowedVersions, string installedVersion, bool isLatest, bool includePrerelease)
        {
            // Arange project
            Mock<IServiceBroker> mockServiceBroker = new Mock<IServiceBroker>();
            Mock<INuGetSearchService> mockSearchService = new Mock<INuGetSearchService>();

            PackageIdentity packageIdentity = new PackageIdentity("Contoso.A", NuGetVersion.Parse(installedVersion));

            PackageReferenceContextInfo[] installedPackages = new PackageReferenceContextInfo[]
            {
                PackageReferenceContextInfo.Create(
                    new PackageReference(
                        packageIdentity,
                        NuGetFramework.Parse("net45"),
                        userInstalled: true,
                        developmentDependency: false,
                        requireReinstallation: false,
                        allowedVersions: allowedVersions != null ? VersionRange.Parse(allowedVersions) : null))
            };

            Mock<INuGetProjectManagerService> projectManagerService = new Mock<INuGetProjectManagerService>();
            projectManagerService.Setup(x => x.GetInstalledPackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedPackages));

#pragma warning disable ISB001 // Dispose of proxies
            mockServiceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(It.Is<ServiceJsonRpcDescriptor>(d => d.Moniker == NuGetServices.ProjectManagerService.Moniker), It.IsAny<ServiceActivationOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectManagerService.Object);
#pragma warning restore ISB001 // Dispose of proxies

            Mock<IProjectContextInfo> project = new Mock<IProjectContextInfo>();

            project.SetupGet(p => p.ProjectKind).Returns(projectKind);
            project.SetupGet(p => p.ProjectStyle).Returns(projectStyle);
            project.SetupGet(p => p.ProjectId).Returns("ProjectId");

            PackageDetailControlModel model = new PackageDetailControlModel(
                mockServiceBroker.Object,
                solutionManager: new Mock<INuGetSolutionManagerService>().Object,
                projects: new[] { project.Object },
                uiController: _mockNuGetUI.Object);

            // Arrange
            List<VersionInfoContextInfo> testVersions = includePrerelease ? ExpectedVersionsList_IncludePrerelease() : ExpectedVersionsList();

            Mock<INuGetSearchService> searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            // Act
            PackageItemViewModel vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "Contoso.A",
                Sources = new List<PackageSourceContextInfo> { new PackageSourceContextInfo("Contoso.A.test") },
                InstalledVersion = packageIdentity.Version,
                AllowedVersions = allowedVersions != null ? VersionRange.Parse(allowedVersions) : null,
                Version = packageIdentity.Version,
            };

            await model.SetCurrentPackageAsync(
                vm,
                ItemFilter.All,
                () => vm);

            ItemsChangeObservableCollection<DisplayVersion> assertVersions;
            string expectedAditionalInfo = null;
            if (includePrerelease)
            {
                NuGetVersion version;
                NuGetVersion.TryParse(installedVersion, out version);
                if (version != null && version.Version.Equals(new NuGetVersion("3.0.0").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestStable_IncludePrerelease_NonPackageReferenceProject();
                }
                else if (version != null && version.Version.Equals(new NuGetVersion("3.0.1-beta").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestPrerelease_IncludePrerelease_NonPackageReferenceProject();
                }
                else
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsNotLatest_IncludePrerelease_NonPackageReferenceProject();
                }
                expectedAditionalInfo = isLatest ? null : "Latest prerelease";
            }
            else
            {
                assertVersions = isLatest ? VersionsList_WhenInstalledVersion_IsLatest_NonPackageReferenceProject() : VersionsList_WhenInstalledVersion_IsNotLatest_NonPackageReferenceProject();
                expectedAditionalInfo = isLatest ? null : "Latest stable";
            }

            // Assert
            Assert.NotEqual(model.SelectedVersion.ToString(), allowedVersions);
            // Browse Tab should display latest available version
            Assert.Equal(model.SelectedVersion.AdditionalInfo, expectedAditionalInfo);
            Assert.Equal(model.SelectedVersion.Version.ToString(), includePrerelease ? "3.0.1-beta" : "3.0.0");
            Assert.Equal(model.Versions, assertVersions);
            Assert.True(model.CanInstallWithPackageSourceMapping);
            Assert.Equal(model.IsInstallorUpdateButtonEnabled, !isLatest);
        }

        [Theory]
        [MemberData(nameof(FloatingVersions_TestCases_NonPackageReferenceProject))]
        public async void WhenPackageStyleIsNotPackageReference_And_CustomVersion_UpdatesTab_IsSelectedVersionCorrect(NuGetProjectKind projectKind, ProjectModel.ProjectStyle projectStyle, string allowedVersions, string installedVersion, bool isLatest, bool includePrerelease)
        {
            // Arange project
            Mock<IServiceBroker> mockServiceBroker = new Mock<IServiceBroker>();
            Mock<INuGetSearchService> mockSearchService = new Mock<INuGetSearchService>();

            PackageIdentity packageIdentity = new PackageIdentity("Contoso.A", NuGetVersion.Parse(installedVersion));

            PackageReferenceContextInfo[] installedPackages = new PackageReferenceContextInfo[]
            {
                PackageReferenceContextInfo.Create(
                    new PackageReference(
                        packageIdentity,
                        NuGetFramework.Parse("net45"),
                        userInstalled: true,
                        developmentDependency: false,
                        requireReinstallation: false,
                        allowedVersions: allowedVersions != null ? VersionRange.Parse(allowedVersions) : null))
            };

            Mock<INuGetProjectManagerService> projectManagerService = new Mock<INuGetProjectManagerService>();
            projectManagerService.Setup(x => x.GetInstalledPackagesAsync(
                    It.IsAny<IReadOnlyCollection<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<IReadOnlyCollection<IPackageReferenceContextInfo>>(installedPackages));

#pragma warning disable ISB001 // Dispose of proxies
            mockServiceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(It.Is<ServiceJsonRpcDescriptor>(d => d.Moniker == NuGetServices.ProjectManagerService.Moniker), It.IsAny<ServiceActivationOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectManagerService.Object);
#pragma warning restore ISB001 // Dispose of proxies

            Mock<IProjectContextInfo> project = new Mock<IProjectContextInfo>();

            project.SetupGet(p => p.ProjectKind).Returns(projectKind);
            project.SetupGet(p => p.ProjectStyle).Returns(projectStyle);
            project.SetupGet(p => p.ProjectId).Returns("ProjectId");

            PackageDetailControlModel model = new PackageDetailControlModel(
                mockServiceBroker.Object,
                solutionManager: new Mock<INuGetSolutionManagerService>().Object,
                projects: new[] { project.Object },
                uiController: _mockNuGetUI.Object);

            // Arrange
            List<VersionInfoContextInfo> testVersions = includePrerelease ? ExpectedVersionsList_IncludePrerelease() : ExpectedVersionsList();

            Mock<INuGetSearchService> searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            // Act
            PackageItemViewModel vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "Contoso.A",
                Sources = new List<PackageSourceContextInfo> { new PackageSourceContextInfo("Contoso.A.test") },
                InstalledVersion = NuGetVersion.Parse(installedVersion),
                AllowedVersions = allowedVersions != null ? VersionRange.Parse(allowedVersions) : null,
                Version = NuGetVersion.Parse(installedVersion),
            };

            await model.SetCurrentPackageAsync(
                vm,
                ItemFilter.UpdatesAvailable,
                () => vm);

            ItemsChangeObservableCollection<DisplayVersion> assertVersions;
            string expectedAditionalInfo = null;
            if (includePrerelease)
            {
                NuGetVersion version;
                NuGetVersion.TryParse(installedVersion, out version);
                if (version != null && version.Version.Equals(new NuGetVersion("3.0.0").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestStable_IncludePrerelease_NonPackageReferenceProject();
                }
                else if (version != null && version.Version.Equals(new NuGetVersion("3.0.1-beta").Version))
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsLatestPrerelease_IncludePrerelease_NonPackageReferenceProject();
                }
                else
                {
                    assertVersions = VersionsList_WhenInstalledVersion_IsNotLatest_IncludePrerelease_NonPackageReferenceProject();
                }
                expectedAditionalInfo = isLatest ? null : "Latest prerelease";
            }
            else
            {
                assertVersions = isLatest ? VersionsList_WhenInstalledVersion_IsLatest_NonPackageReferenceProject() : VersionsList_WhenInstalledVersion_IsNotLatest_NonPackageReferenceProject();
                expectedAditionalInfo = isLatest ? null : "Latest stable";
            }

            // Assert
            Assert.NotEqual(model.SelectedVersion.ToString(), allowedVersions);
            // Updates Tab should display latest available version
            Assert.Equal(model.SelectedVersion.AdditionalInfo, expectedAditionalInfo);
            Assert.Equal(model.SelectedVersion.Version.ToString(), includePrerelease ? "3.0.1-beta" : "3.0.0");
            Assert.Equal(model.Versions, assertVersions);
            Assert.True(model.CanInstallWithPackageSourceMapping);
            Assert.Equal(model.IsInstallorUpdateButtonEnabled, !isLatest);
        }

        public Task<object> GetServiceAsync(Type serviceType)
        {
            if (_services.TryGetValue(serviceType, out Task<object> task))
            {
                return task;
            }

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// We need an interface that implements both: SVsBrokeredServiceContainer and IBrokeredServiceContainer so we can add it to the service host
        /// </summary>
        public interface IBrokeredServiceContainerMock : SVsBrokeredServiceContainer, IBrokeredServiceContainer
        {
        }
    }

    [Collection(MockedVS.Collection)]
    public class V3PackageSolutionDetailControlModelTests : V3DetailControlModelTestBase, IAsyncServiceProvider
    {
        private PackageSolutionDetailControlModel _testInstance;
        private readonly Dictionary<Type, Task<object>> _services = new Dictionary<Type, Task<object>>(TypeEquivalenceComparer.Instance);
        public V3PackageSolutionDetailControlModelTests(GlobalServiceProvider sp, V3PackageSearchMetadataFixture testData)
            : base(sp, testData)
        {
            var solMgr = new Mock<INuGetSolutionManagerService>();

            Mock<INuGetProjectManagerService> projectManagerService = new Mock<INuGetProjectManagerService>();
            Mock<IProjectContextInfo> project = new Mock<IProjectContextInfo>();
            project.SetupGet(p => p.ProjectKind).Returns(NuGetProjectKind.PackageReference);
            project.SetupGet(p => p.ProjectStyle).Returns(ProjectModel.ProjectStyle.PackageReference);
            project.SetupGet(p => p.ProjectId).Returns("ProjectId1");

            Mock<IProjectContextInfo> project2 = new Mock<IProjectContextInfo>();
            project2.SetupGet(p => p.ProjectKind).Returns(NuGetProjectKind.PackageReference);
            project2.SetupGet(p => p.ProjectStyle).Returns(ProjectModel.ProjectStyle.PackageReference);
            project2.SetupGet(p => p.ProjectId).Returns("ProjectId2");

            ReadOnlyCollection<IProjectContextInfo> projects = new ReadOnlyCollection<IProjectContextInfo>(
                new List<IProjectContextInfo>()
                {
                    project.Object,
                    project2.Object
                });

            projectManagerService.Setup(x => x.GetProjectsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(projects);
            projectManagerService.Setup(x => x.GetMetadataAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Mock.Of<IProjectMetadataContextInfo>());
#pragma warning disable ISB001 // Dispose of proxies
            _mockServiceBroker.Setup(x => x.GetProxyAsync<INuGetProjectManagerService>(It.Is<ServiceJsonRpcDescriptor>(d => d.Moniker == NuGetServices.ProjectManagerService.Moniker), It.IsAny<ServiceActivationOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectManagerService.Object);
#pragma warning restore ISB001 // Dispose of proxies

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                _testInstance = await PackageSolutionDetailControlModel.CreateAsync(
                    solutionManager: solMgr.Object,
                    projects: new List<IProjectContextInfo>(),
                    serviceBroker: _mockServiceBroker.Object,
                    uiController: _mockNuGetUI.Object,
                    CancellationToken.None);

                await _testInstance.SetCurrentPackageAsync(
                    _testViewModel,
                    ItemFilter.All,
                    () => null);
            });
        }

        public Task<object> GetServiceAsync(Type serviceType)
        {
            if (_services.TryGetValue(serviceType, out Task<object> task))
            {
                return task;
            }

            return Task.FromResult<object>(null);
        }

        [Fact]
        public async Task SetCurrentPackageAsync_SortsVersions_ByNuGetVersionDesc()
        {
            // Arrange
            NuGetVersion installedVersion = NuGetVersion.Parse("1.0.0");

            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01248")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01249")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01256")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01265")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0-dev-01187")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0-dev-01191")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0-dev-01211")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0")),
            };

            var searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            var vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "package",
                InstalledVersion = installedVersion,
                Version = installedVersion,
            };

            // Act

            await _testInstance.SetCurrentPackageAsync(
                vm,
                ItemFilter.All,
                () => vm);

            // Assert
            // Remove any added `null` separators, and any Additional Info entries (eg, "Latest Prerelease", "Latest Stable").
            List<DisplayVersion> actualVersions = _testInstance.Versions
                .Where(v => v != null && v.AdditionalInfo == null).ToList();

            var expectedVersions = new List<DisplayVersion>() {
                new DisplayVersion(version: new NuGetVersion("2.10.1-dev-01265"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.1-dev-01256"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.1-dev-01249"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.1-dev-01248"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.0"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.0-dev-01211"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.0-dev-01191"), additionalInfo: null),
                new DisplayVersion(version: new NuGetVersion("2.10.0-dev-01187"), additionalInfo: null),
            };

            Assert.Equal(expectedVersions, actualVersions);
        }

        [Theory]
        [InlineData(ItemFilter.All, "3.0.0")]
        [InlineData(ItemFilter.Installed, "1.0.0")]
        [InlineData(ItemFilter.UpdatesAvailable, "3.0.0")]
        public async Task SetCurrentPackageAsync_CorrectSelectedVersion(ItemFilter tab, string expectedSelectedVersion)
        {
            // Arrange
            NuGetVersion installedVersion = NuGetVersion.Parse("1.0.0");

            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(new NuGetVersion("2.10.1-dev-01248")),
                new VersionInfoContextInfo(new NuGetVersion("2.10.0")),
                new VersionInfoContextInfo(new NuGetVersion("3.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("1.0.0")),
            };

            var searchService = new Mock<INuGetSearchService>();
            searchService.Setup(ss => ss.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);

            var vm = new PackageItemViewModel(searchService.Object)
            {
                Id = "package",
                InstalledVersion = installedVersion,
                Version = installedVersion,
            };

            // Act
            await _testInstance.SetCurrentPackageAsync(
                vm,
                tab,
                () => vm);

            NuGetVersion selectedVersion = NuGetVersion.Parse(expectedSelectedVersion);

            Assert.Equal(_testInstance.SelectedVersion.Version, selectedVersion);
        }

        [Fact]
        public async Task SetCurrentPackageAsync_ClearVersions_Always()
        {
            // Arrange
            var installedVersion = NuGetVersion.Parse("1.0.0");

            var testVersions = new List<VersionInfoContextInfo>() {
                new VersionInfoContextInfo(new NuGetVersion("1.0.0")),
                new VersionInfoContextInfo(new NuGetVersion("1.0.1")),
            };

            var mockPropertyChangedEventHandler = new Mock<IPropertyChangedEventHandler>();
            var wasVersionsListCleared = false;

            var searchService = new Mock<INuGetSearchService>();
            searchService.Setup(s => s.GetPackageVersionsAsync(It.IsAny<PackageIdentity>(), It.IsAny<IReadOnlyCollection<PackageSourceContextInfo>>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<IProjectContextInfo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testVersions);
            var vm = new PackageItemViewModel(searchService.Object);

            vm.Id = "a";
            vm.Sources = new ReadOnlyCollection<PackageSourceContextInfo>(new List<PackageSourceContextInfo>());
            vm.InstalledVersion = installedVersion;
            vm.Version = installedVersion;

            // Test Setup already selected a package.
            int previousVersionListCount = _testInstance.Versions.Count;

            mockPropertyChangedEventHandler.Setup(x => x.PropertyChanged(
                It.IsAny<object>(),
                It.IsAny<PropertyChangedEventArgs>()
            ))
            .Callback<object, PropertyChangedEventArgs>((d, p) =>
            {
                DetailControlModel detail = d as DetailControlModel;
                if (detail != null
                    && detail.Versions.Count == 0
                    && p.PropertyName == nameof(DetailControlModel.Versions))
                {
                    wasVersionsListCleared = true;
                }
            });

            _testInstance.PropertyChanged += mockPropertyChangedEventHandler.Object.PropertyChanged;

            // Act

            //Select a different VM which should clear the Versions list from the previous selection.
            await _testInstance.SetCurrentPackageAsync(
                vm,
                ItemFilter.All,
                () => vm);

            // Assert

            Assert.True(previousVersionListCount > 0, "Test setup did not pre-populate versions list.");
            Assert.True(wasVersionsListCleared, "Versions list was not cleared.");
        }

        [Fact]
        public void SetInstalledOrUpdateButtonIsEnabled_AfterPackageSourceMappingChanges_CanInstallWithPackageSourceMapping()
        {
            var packageIDWithSourceMapping = "a";
            var patterns = new Dictionary<string, IReadOnlyList<string>>
            {
                { "sourceA", new List<string>() { packageIDWithSourceMapping } }
            };
            var packageSourceMappingPatterns = new ReadOnlyDictionary<string, IReadOnlyList<string>>(patterns);

            var version = NuGetVersion.Parse("1.1.1");

            _testInstance.SelectedVersion = new DisplayVersion(version, additionalInfo: null);

            PackageInstallationInfo firstProject = _testInstance.Projects.First();
            firstProject.IsSelected = true;
            firstProject.InstalledVersion = version;

            // Explicitly trigger PropertyChanged event.
            _testInstance.SetInstalledOrUpdateButtonIsEnabled();

            bool afterSetInstalledOrUpdateButtonIsEnabled_CanInstall_RaisedPropertyChanged = false;
            bool afterSetInstalledOrUpdateButtonIsEnabled_CanUninstall_RaisedPropertyChanged = false;
            var mockPropertyChangedEventHandler = new Mock<IPropertyChangedEventHandler>();
            mockPropertyChangedEventHandler.Setup(x => x.PropertyChanged(
                It.IsAny<object>(),
                It.IsAny<PropertyChangedEventArgs>()
            ))
            .Callback<object, PropertyChangedEventArgs>((d, p) =>
            {
                var detail = d as PackageSolutionDetailControlModel;
                if (detail != null)
                {
                    if (p.PropertyName == nameof(PackageSolutionDetailControlModel.CanInstall))
                    {
                        afterSetInstalledOrUpdateButtonIsEnabled_CanInstall_RaisedPropertyChanged = true;
                    }
                    if (p.PropertyName == nameof(PackageSolutionDetailControlModel.CanUninstall))
                    {
                        afterSetInstalledOrUpdateButtonIsEnabled_CanUninstall_RaisedPropertyChanged = true;
                    }
                }
            });

            _testInstance.PropertyChanged += mockPropertyChangedEventHandler.Object.PropertyChanged;

            var beforeEnablingPackageSourceMapping_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;
            var beforeEnablingPackageSourceMapping_CanInstall = _testInstance.CanInstall;
            var beforeEnablingPackageSourceMapping_CanUninstall = _testInstance.CanUninstall;

            PackageSourceMoniker singlePackageSourceMoniker = new("sourceName", new List<PackageSourceContextInfo>() { new PackageSourceContextInfo("sourceName") }, priorityOrder: 0);
            PackageSourceMoniker aggregatePackageSourceMoniker = new("sourceName",
                new List<PackageSourceContextInfo>() { new PackageSourceContextInfo("sourceName"), new PackageSourceContextInfo("sourceName2") },
                priorityOrder: 0);

            // Act

            //********************************************************************************************************************************************
            // Enable package source mapping; select an Aggregate package source so it won't allow automatically create a mapping when pressing Install.
            //********************************************************************************************************************************************
            ConfigureNuGetUIWithPackageSourceMapping(packageSourceMappingPatterns);
            _mockNuGetUI.Setup(_ => _.ActivePackageSourceMoniker).Returns(aggregatePackageSourceMoniker);

            // Explicitly trigger PropertyChanged event.
            _testInstance.SetInstalledOrUpdateButtonIsEnabled();

            var packageNotMapped_AggregateSourceSelected_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;
            var packageNotMapped_AggregateSourceSelected_CanInstall = _testInstance.CanInstall;
            var packageNotMapped_AggregateSourceSelected_CanUninstall = _testInstance.CanUninstall;

            //********************************************************************************************************************************************
            // Select a single package source so it will allow automatically create a mapping when pressing Install.
            //********************************************************************************************************************************************
            _mockNuGetUI.Setup(_ => _.ActivePackageSourceMoniker).Returns(singlePackageSourceMoniker);

            // Explicitly trigger PropertyChanged event.
            _testInstance.SetInstalledOrUpdateButtonIsEnabled();

            var packageNotMapped_SingleSourceSelected_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;
            var packageNotMapped_SingleSourceSelected_CanInstall = _testInstance.CanInstall;
            var packageNotMapped_SingleSourceSelected_CanUninstall = _testInstance.CanUninstall;

            //********************************************************************************************************************************************
            // Select a package which has a configured Package Source Mapping.
            //********************************************************************************************************************************************
            _testInstance.PackageSourceMappingViewModel.PackageId = packageIDWithSourceMapping;

            var afterSelectingPackageWithPackageSourceMapping_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;

            // Explicitly trigger PropertyChanged event.
            _testInstance.SetInstalledOrUpdateButtonIsEnabled();

            var afterSelectingPackageWithPackageSourceMapping_CanInstall = _testInstance.CanInstall;
            var afterSelectingPackageWithPackageSourceMapping_CanUninstall = _testInstance.CanUninstall;
            var afterSetInstalledOrUpdateButtonIsEnabled_CanInstallWithPackageSourceMapping = _testInstance.CanInstallWithPackageSourceMapping;

            // Assert
            Assert.True(beforeEnablingPackageSourceMapping_CanInstallWithPackageSourceMapping, "Package Source Mapping is disabled.");
            Assert.True(beforeEnablingPackageSourceMapping_CanInstall, "Package Source Mapping is disabled.");
            Assert.True(beforeEnablingPackageSourceMapping_CanUninstall, "Package Source Mapping is disabled.");

            Assert.False(packageNotMapped_AggregateSourceSelected_CanInstallWithPackageSourceMapping,
                "Package Source Mapping is enabled but the Selected Package ID has no mapping.");
            Assert.False(packageNotMapped_AggregateSourceSelected_CanInstall, "Package Source Mapping is enabled but the Selected Package ID has no mapping.");
            Assert.True(packageNotMapped_AggregateSourceSelected_CanUninstall, "Should be able to Uninstall regardless of package source mapping.");

            Assert.True(packageNotMapped_SingleSourceSelected_CanInstallWithPackageSourceMapping,
                "Selected Package ID doesn't have a mapping but should automatically get one during Install.");
            Assert.True(packageNotMapped_SingleSourceSelected_CanInstall,
                "Selected Package ID will automatically get a package source mapping, but the " + nameof(PackageSolutionDetailControlModel.CanInstall) + " hasn't been updated, yet.");
            Assert.True(packageNotMapped_SingleSourceSelected_CanUninstall,
                "Selected Package ID will automatically get a package source mapping, but the " + nameof(PackageSolutionDetailControlModel.CanUninstall) + " hasn't been updated, yet.");

            Assert.True(afterSelectingPackageWithPackageSourceMapping_CanInstallWithPackageSourceMapping,
                "Selected Package ID has a package source mapping.");
            Assert.True(afterSelectingPackageWithPackageSourceMapping_CanInstall, "Selected Package ID has a package source mapping.");
            Assert.True(afterSelectingPackageWithPackageSourceMapping_CanUninstall, "Selected Package ID has a package source mapping.");
            Assert.True(afterSetInstalledOrUpdateButtonIsEnabled_CanInstallWithPackageSourceMapping,
                nameof(PackageSolutionDetailControlModel.SetInstalledOrUpdateButtonIsEnabled) + " should not have changed " + nameof(PackageSolutionDetailControlModel.CanInstallWithPackageSourceMapping) + ".");

            Assert.True(afterSetInstalledOrUpdateButtonIsEnabled_CanInstall_RaisedPropertyChanged,
                nameof(PackageSolutionDetailControlModel.CanInstall) + " should have raised a PropertyChanged when calling "
                + nameof(DetailControlModel.SetInstalledOrUpdateButtonIsEnabled) + " and the value should become true.");
            Assert.True(afterSetInstalledOrUpdateButtonIsEnabled_CanUninstall_RaisedPropertyChanged,
                nameof(PackageSolutionDetailControlModel.CanUninstall) + " should have raised a PropertyChanged when calling "
                + nameof(DetailControlModel.SetInstalledOrUpdateButtonIsEnabled) + " and the value should become true.");
        }
    }

    public interface IPropertyChangedEventHandler
    {
        void PropertyChanged(object sender, PropertyChangedEventArgs e);
    }
}

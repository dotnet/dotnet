// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities.UnifiedSettings;
using Moq;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio.Options;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test.Options
{
    [Collection(MockedVS.Collection)]
    public class PackageSourcesPageTests : NuGetExternalSettingsProviderTests<PackageSourcesPage>
    {
        private IEnumerable<PackageSource> _packageSources;
        private int _countEnablePackageSourceCalled = 0;
        private int _countDisablePackageSourceCalled = 0;

        public PackageSourcesPageTests(GlobalServiceProvider sp)
        {
            sp.Reset();
            NuGetUIThreadHelper.SetCustomJoinableTaskFactory(ThreadHelper.JoinableTaskFactory);
            _packageSources = Enumerable.Empty<PackageSource>();
        }

        protected override PackageSourcesPage CreateInstance(VSSettings? vsSettings)
        {
            Mock<IPackageSourceProvider> mockedPackageSourceProvider = new Mock<IPackageSourceProvider>();
            mockedPackageSourceProvider.Setup(packageSourceProvider => packageSourceProvider.LoadPackageSources())
                .Returns(_packageSources);

            mockedPackageSourceProvider.Setup(packageSourceProvider =>
                packageSourceProvider.EnablePackageSource(It.IsAny<string>()))
                .Callback<string>(name =>
                {
                    _countEnablePackageSourceCalled++;
                });

            mockedPackageSourceProvider.Setup(packageSourceProvider =>
                packageSourceProvider.DisablePackageSource(It.IsAny<string>()))
                .Callback<string>(name =>
                {
                    _countDisablePackageSourceCalled++;
                });

            return new PackageSourcesPage(vsSettings!, mockedPackageSourceProvider.Object);
        }

        [Fact]
        public async Task GetValueAsync_WhenNuGetConfigHasInvalidSource_ReturnsFailureResultTaskAsync()
        {
            // Arrange
            string invalidSource = "http://";
            _packageSources =
            [
                new PackageSource(invalidSource, "unitTestingSourceName")
            ];

            PackageSourcesPage instance = CreateInstance(_vsSettings);

            // Act
            var result = await instance.GetValueAsync<List<Dictionary<string, object>>>(
                PackageSourcesPage.MonikerPackageSources,
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ExternalSettingOperationResult<List<Dictionary<string, object>>>.Failure>();

            var failure = result as ExternalSettingOperationResult<List<Dictionary<string, object>>>.Failure;
            failure.Should().NotBeNull();
            failure.IsTransient.Should().BeTrue();
            failure.Scope.Should().Be(ExternalSettingsErrorScope.SingleSettingOnly);
            failure.ErrorMessage.Should().StartWith(Strings.Error_NuGetConfig_InvalidState);
        }

        [Theory]
        [InlineData(@"http://")]
        [InlineData(@"https://")]
        [InlineData(@"https:// ")]
        [InlineData(@" https://")]
        [InlineData(@"ftp://")]
        [InlineData(@"http:/")]
        public async Task SetValueAsync_WithInvalidRemoteSource_ReturnsFailureResultTaskAsync(string invalidSource)
        {
            // Arrange
            PackageSourcesPage instance = CreateInstance(_vsSettings);
            Dictionary<string, object> packageSourceDictionary = new Dictionary<string, object>();

            packageSourceDictionary["sourceName"] = "unitTestingSourceName";
            packageSourceDictionary["sourceUrl"] = invalidSource;
            packageSourceDictionary["isEnabled"] = true;

            IList<IDictionary<string, object>> packageSourceDictionaryList =
                new List<IDictionary<string, object>>(capacity: 1)
                {
                    packageSourceDictionary
                };

            // Act
            ExternalSettingOperationResult result = await instance.SetValueAsync(
                PackageSourcesPage.MonikerPackageSources,
                packageSourceDictionaryList,
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ExternalSettingOperationResult.Failure>();

            var failure = result as ExternalSettingOperationResult.Failure;
            failure.Should().NotBeNull();
            failure.IsTransient.Should().BeTrue();
            failure.Scope.Should().Be(ExternalSettingsErrorScope.SingleSettingOnly);
            failure.ErrorMessage.Should().StartWith(Strings.Error_PackageSource_InvalidSource);
        }

        [Theory]
        [InlineData(@"C")]
        [InlineData(@"http")] // Missing :// causes this to be treated as a file path.
        [InlineData(@"http:")]
        [InlineData(@"ftp")] // Missing :// causes this to be treated as a file path.
        [InlineData(@"C:")]
        [InlineData(@"C:\invalid\*\'\chars")]
        [InlineData(@"\\server\invalid\*\")]
        [InlineData(@"..\packages")]
        [InlineData(@"./configs/source.config")]
        [InlineData(@"../local-packages/")]
        public async Task SetValueAsync_WithInvalidUncPath_ReturnsFailureResultTaskAsync(string invalidSource)
        {
            // Arrange
            PackageSourcesPage instance = CreateInstance(_vsSettings);
            Dictionary<string, object> packageSourceDictionary = new Dictionary<string, object>();

            packageSourceDictionary["sourceName"] = "unitTestingSourceName";
            packageSourceDictionary["sourceUrl"] = invalidSource;
            packageSourceDictionary["isEnabled"] = true;

            IList<IDictionary<string, object>> packageSourceDictionaryList =
                new List<IDictionary<string, object>>(capacity: 1)
                {
                    packageSourceDictionary
                };

            // Act
            ExternalSettingOperationResult result = await instance.SetValueAsync(
                PackageSourcesPage.MonikerPackageSources,
                packageSourceDictionaryList,
                CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ExternalSettingOperationResult.Failure>();

            var failure = result as ExternalSettingOperationResult.Failure;
            failure.Should().NotBeNull();
            failure.IsTransient.Should().BeTrue();
            failure.Scope.Should().Be(ExternalSettingsErrorScope.SingleSettingOnly);
            failure.ErrorMessage.Should().StartWith(Strings.Error_PackageSource_InvalidSource);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SetValueAsync_MonikerMachineWideSources_UpdatesIsEnabledAsync(bool isEnabled)
        {
            // Arrange
            string sourceName1 = "unitTestingSourceName1";
            string sourceUrl1 = "https://testsource1.com";
            bool isSource1Enabled = isEnabled;

            string sourceName2 = "unitTestingSourceName2";
            string sourceUrl2 = "https://testsource2.com";
            bool isSource2Enabled = true;

            string sourceName3 = "unitTestingSourceName3";
            string sourceUrl3 = "https://testsource3.com";
            bool isSource3Enabled = isEnabled;

            // Configure 3 existing package sources
            _packageSources =
            [
                new PackageSource(sourceUrl1, sourceName1, isSource1Enabled)
                {
                    IsMachineWide = true
                },
                new PackageSource(sourceUrl2, sourceName2, isSource2Enabled)
                {
                    IsMachineWide = true
                },
                new PackageSource(sourceUrl3, sourceName3, isSource3Enabled)
                {
                    IsMachineWide = true
                }
            ];

            PackageSourcesPage instance = CreateInstance(_vsSettings);

            // Configure Unified Settings input to Toggle 2 of the 3 IsEnabled states.
            Dictionary<string, object> packageSourceDictionary1 = new Dictionary<string, object>();
            packageSourceDictionary1["sourceName"] = sourceName1;
            packageSourceDictionary1["sourceUrl"] = sourceUrl1;
            packageSourceDictionary1["isEnabled"] = !isSource1Enabled; // Toggle the enabled state for the first source.

            Dictionary<string, object> packageSourceDictionary2 = new Dictionary<string, object>();
            packageSourceDictionary2["sourceName"] = sourceName2;
            packageSourceDictionary2["sourceUrl"] = sourceUrl2;
            packageSourceDictionary2["isEnabled"] = isSource2Enabled;

            Dictionary<string, object> packageSourceDictionary3 = new Dictionary<string, object>();
            packageSourceDictionary3["sourceName"] = sourceName3;
            packageSourceDictionary3["sourceUrl"] = sourceUrl3;
            packageSourceDictionary3["isEnabled"] = !isSource3Enabled; // Toggle the enabled state for the third source.

            IList<IDictionary<string, object>> packageSourceDictionaryList =
                new List<IDictionary<string, object>>(capacity: 3)
                {
                    packageSourceDictionary1,
                    packageSourceDictionary2,
                    packageSourceDictionary3
                };

            // Act
            ExternalSettingOperationResult result = await instance.SetValueAsync(
                PackageSourcesPage.MonikerMachineWideSources,
                packageSourceDictionaryList,
                CancellationToken.None);

            // Assert
            int expectedEnableCount = isEnabled ? 0 : 2;
            int expectedDisableCount = isEnabled ? 2 : 0;
            _countEnablePackageSourceCalled.Should().Be(expectedEnableCount);
            _countDisablePackageSourceCalled.Should().Be(expectedDisableCount);
            result.Should().NotBeNull();
            result.Should().BeOfType<ExternalSettingOperationResult.Success>();
        }
    }
}

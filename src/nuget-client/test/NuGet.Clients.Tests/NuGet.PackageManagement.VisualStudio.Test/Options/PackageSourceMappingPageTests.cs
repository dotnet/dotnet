// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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
using NuGet.Test.Utility;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test.Options
{
    [Collection(MockedVS.Collection)]
    public class PackageSourceMappingPageTests : NuGetExternalSettingsProviderTests<PackageSourceMappingPage>, IDisposable
    {
        private IEnumerable<PackageSource> _packageSources;
        private SimpleTestPathContext? _pathContext;

        public PackageSourceMappingPageTests(GlobalServiceProvider sp)
        {
            sp.Reset();
            NuGetUIThreadHelper.SetCustomJoinableTaskFactory(ThreadHelper.JoinableTaskFactory);
            _packageSources = Enumerable.Empty<PackageSource>();
        }

        protected override PackageSourceMappingPage CreateInstance(VSSettings vsSettings)
        {
            Mock<IPackageSourceProvider> mockedPackageSourceProvider = new Mock<IPackageSourceProvider>();
            mockedPackageSourceProvider.Setup(packageSourceProvider => packageSourceProvider.LoadPackageSources())
                .Returns(_packageSources);

            PackageSourceMappingProvider sourceMappingProvider = new(vsSettings);
            return new PackageSourceMappingPage(vsSettings, mockedPackageSourceProvider.Object, sourceMappingProvider);
        }

        protected override string GetSolutionDirectory()
        {
            if (_pathContext is not null)
            {
                return _pathContext.SolutionRoot;
            }

            return base.GetSolutionDirectory();
        }

        public void Dispose()
        {
            if (_pathContext is not null)
            {
                _pathContext.Dispose();
            }
        }

        [Fact]
        public void VsSettings_SettingsChanged_RaisesEnumSettingChoicesChanged()
        {
            // Arrange
            bool wasEnumSettingChoicesChangedRaised = false;
            PackageSourceMappingPage instance = CreateInstance(_vsSettings);
            instance.EnumSettingChoicesChanged += (s, e) =>
            {
                wasEnumSettingChoicesChangedRaised = true;
            };

            // Act
            instance.VsSettings_SettingsChanged(this, EventArgs.Empty);

            // Assert
            wasEnumSettingChoicesChangedRaised.Should().BeTrue();
        }

        [Fact]
        public async Task SetValueAsync_NewPackageSourceMapping_ReturnedByGetValueAsync()
        {
            // Arrange
            _pathContext = new SimpleTestPathContext();

            string addNewPackageSourceName = "unitTestingSourceName1";
            string packageIdPattern = "Contoso.*";
            PackageSourceMappingPage instance = CreateInstance(_vsSettings);

            Dictionary<string, object> sourceMappingDictionary1 = new Dictionary<string, object>();
            sourceMappingDictionary1[PackageSourceMappingPage.MonikerPackageId] = packageIdPattern;
            sourceMappingDictionary1[PackageSourceMappingPage.MonikerSourceNames] = new List<string>() { addNewPackageSourceName };

            IList<IDictionary<string, object>> sourceMappingDictionaryList =
                new List<IDictionary<string, object>>(capacity: 1)
                {
                    sourceMappingDictionary1, // Adding a new package source mapping
                };

            // Act
            ExternalSettingOperationResult resultSetValue = await instance.SetValueAsync(
                PackageSourceMappingPage.MonikerPackageSourceMapping,
                sourceMappingDictionaryList,
                CancellationToken.None);

            ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>> resultGetValue =
                await instance.GetValueAsync<IReadOnlyList<IDictionary<string, object>>>(
                    moniker: PackageSourceMappingPage.MonikerPackageSourceMapping,
                    cancellationToken: CancellationToken.None);

            // Assert
            resultSetValue.Should().NotBeNull();
            resultSetValue.Should().BeOfType<ExternalSettingOperationResult.Success>();

            resultGetValue.Should().NotBeNull();
            resultGetValue.Should().BeOfType<ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success>();
            IReadOnlyList<IDictionary<string, object>> successResult = resultGetValue
                .As<ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success>()
                .Value;
            successResult.Count.Should().Be(1);
            successResult.Should().ContainEquivalentOf(sourceMappingDictionary1);
        }

        [Fact]
        public async Task SetValueAsync_RemovePackageSourceMapping_NotReturnedByGetValueAsync()
        {
            // Arrange
            _pathContext = new SimpleTestPathContext();
            string unitTestingSourceName1 = "unitTestingSourceName1";
            string packageIdPattern = "Contoso.*";

            _packageSources =
            [
                new PackageSource(source: $"https://{unitTestingSourceName1}", unitTestingSourceName1),
            ];

            var unitTestingSourceMapping1 = new PackageSourceMappingSourceItem(unitTestingSourceName1, [new PackagePatternItem(packageIdPattern)]);
            _vsSettings.AddOrUpdate(ConfigurationConstants.PackageSourceMapping, unitTestingSourceMapping1);

            var emptySourceMappingDictionaryEnumerable = Enumerable.Empty<IDictionary<string, object>>();

            PackageSourceMappingPage instance = CreateInstance(_vsSettings);

            // Act
            ExternalSettingOperationResult resultSetValue = await instance.SetValueAsync(
                PackageSourceMappingPage.MonikerPackageSourceMapping,
                emptySourceMappingDictionaryEnumerable,
                CancellationToken.None);

            ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>> resultGetValue =
                await instance.GetValueAsync<IReadOnlyList<IDictionary<string, object>>>(
                    moniker: PackageSourceMappingPage.MonikerPackageSourceMapping,
                    cancellationToken: CancellationToken.None);

            // Assert
            resultSetValue.Should().NotBeNull();
            resultSetValue.Should().BeOfType<ExternalSettingOperationResult.Success>();

            resultGetValue.Should().NotBeNull();
            resultGetValue.Should().BeOfType<ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success>();
            IReadOnlyList<IDictionary<string, object>> successResult = resultGetValue
                .As<ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success>()
                .Value;
            successResult.Count.Should().Be(0);
        }

        [Fact]
        public async Task SetValueAsync_EditSourceNames_ReturnedByGetValueAsync()
        {
            // Arrange
            _pathContext = new SimpleTestPathContext();
            string unitTestingSourceName1 = "unitTestingSourceName1";
            string addedUnitTestingSourceName2 = "addedUnitTestingSourceName2";
            string packageIdPattern = "Contoso.*";

            _packageSources =
            [
                new PackageSource(source: $"https://{unitTestingSourceName1}", unitTestingSourceName1),
            ];

            var unitTestingSourceMapping1 = new PackageSourceMappingSourceItem(unitTestingSourceName1, [new PackagePatternItem(packageIdPattern)]);
            _vsSettings.AddOrUpdate(ConfigurationConstants.PackageSourceMapping, unitTestingSourceMapping1);

            Dictionary<string, object> sourceMappingDictionary1 = new Dictionary<string, object>();
            sourceMappingDictionary1[PackageSourceMappingPage.MonikerPackageId] = packageIdPattern;
            sourceMappingDictionary1[PackageSourceMappingPage.MonikerSourceNames] = new List<string>() { unitTestingSourceName1, addedUnitTestingSourceName2 };

            IList<IDictionary<string, object>> sourceMappingDictionaryList =
                new List<IDictionary<string, object>>(capacity: 1)
                {
                    sourceMappingDictionary1, // Adding a new source name to existing package source mapping
                };

            PackageSourceMappingPage instance = CreateInstance(_vsSettings);

            // Act
            ExternalSettingOperationResult resultSetValue = await instance.SetValueAsync(
                PackageSourceMappingPage.MonikerPackageSourceMapping,
                sourceMappingDictionaryList,
                CancellationToken.None);

            ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>> resultGetValue =
                await instance.GetValueAsync<IReadOnlyList<IDictionary<string, object>>>(
                    moniker: PackageSourceMappingPage.MonikerPackageSourceMapping,
                    cancellationToken: CancellationToken.None);

            // Assert
            resultSetValue.Should().NotBeNull();
            resultSetValue.Should().BeOfType<ExternalSettingOperationResult.Success>();

            resultGetValue.Should().NotBeNull();
            resultGetValue.Should().BeOfType<ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success>();
            IReadOnlyList<IDictionary<string, object>> successResult = resultGetValue
                .As<ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success>()
                .Value;

            // One package pattern.
            successResult.Count.Should().Be(1);
            IDictionary<string, object> packagePatternToSources = successResult[0];
            object? foundPackageIdPattern = packagePatternToSources[PackageSourceMappingPage.MonikerPackageId];
            foundPackageIdPattern.Should().NotBeNull();
            foundPackageIdPattern.ToString().Should().Be(packageIdPattern);

            // Two mapped sources, the original and the added source.
            object? sources = packagePatternToSources[PackageSourceMappingPage.MonikerSourceNames];
            sources.Should().NotBeNull();
            var listSources = ((IEnumerable<string>)sources).ToList();
            listSources.Count.Should().Be(2);
            listSources.Should().ContainInOrder(unitTestingSourceName1, addedUnitTestingSourceName2);
        }

        [Fact]
        public async Task SetValueAsync_EditPackageIdOrPattern_ReturnedByGetValueAsync()
        {
            // Arrange
            _pathContext = new SimpleTestPathContext();
            string unitTestingSourceName1 = "unitTestingSourceName1";
            string packageIdPattern = "Contoso.*";
            string editedPackageIdPattern = "Newtonsoft.Json";

            _packageSources =
            [
                new PackageSource(source: $"https://{unitTestingSourceName1}", unitTestingSourceName1),
            ];

            var unitTestingSourceMapping1 = new PackageSourceMappingSourceItem(unitTestingSourceName1, [new PackagePatternItem(packageIdPattern)]);
            _vsSettings.AddOrUpdate(ConfigurationConstants.PackageSourceMapping, unitTestingSourceMapping1);

            Dictionary<string, object> sourceMappingDictionary1 = new Dictionary<string, object>();
            sourceMappingDictionary1[PackageSourceMappingPage.MonikerPackageId] = editedPackageIdPattern;
            sourceMappingDictionary1[PackageSourceMappingPage.MonikerSourceNames] = new List<string>() { unitTestingSourceName1 };

            IList<IDictionary<string, object>> sourceMappingDictionaryList =
                new List<IDictionary<string, object>>(capacity: 1)
                {
                    sourceMappingDictionary1, // Changing the Package ID for an existing mapping.
                };

            PackageSourceMappingPage instance = CreateInstance(_vsSettings);

            // Act
            ExternalSettingOperationResult resultSetValue = await instance.SetValueAsync(
                PackageSourceMappingPage.MonikerPackageSourceMapping,
                sourceMappingDictionaryList,
                CancellationToken.None);

            ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>> resultGetValue =
                await instance.GetValueAsync<IReadOnlyList<IDictionary<string, object>>>(
                    moniker: PackageSourceMappingPage.MonikerPackageSourceMapping,
                    cancellationToken: CancellationToken.None);

            // Assert
            resultSetValue.Should().NotBeNull();
            resultSetValue.Should().BeOfType<ExternalSettingOperationResult.Success>();

            resultGetValue.Should().NotBeNull();
            resultGetValue.Should().BeOfType<ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success>();
            IReadOnlyList<IDictionary<string, object>> successResult = resultGetValue
                .As<ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success>()
                .Value;

            // One package pattern, the edited Package ID/Pattern.
            successResult.Count.Should().Be(1);
            IDictionary<string, object> packagePatternToSources = successResult[0];
            object? foundPackageIdPattern = packagePatternToSources[PackageSourceMappingPage.MonikerPackageId];
            foundPackageIdPattern.Should().NotBeNull();
            foundPackageIdPattern.ToString().Should().Be(editedPackageIdPattern);

            // One mapped source, the original should be unchanged.
            object? sources = packagePatternToSources[PackageSourceMappingPage.MonikerSourceNames];
            sources.Should().NotBeNull();
            var listSources = ((IEnumerable<string>)sources).ToList();
            listSources.Count.Should().Be(1);
            listSources.Should().Contain(unitTestingSourceName1);
        }

        [Fact]
        public async Task SetValueAsync_PreviousMappingsToNonexistantSources_AddNewMapping_ExistingMappingsAreUnchangedAsync()
        {
            // Arrange
            _pathContext = new SimpleTestPathContext();

            string addNewPackageSourceName4 = "unitTestingSourceName4";
            string packageIdPattern = "Contoso.*";

            string sourceName1 = "unitTestingSourceName1";
            string sourceUrl1 = "https://testsource1.com";

            string nonExistantSourceName2 = "nonExistantSourceName2";
            // nonExistantSourceName2 doesn't have a URL as it will not be a configured package source.

            string sourceName3 = "unitTestingSourceName3";
            string sourceUrl3 = "https://testsource3.com";

            var unitTestingSourceMapping1 = new PackageSourceMappingSourceItem(sourceName1, [new PackagePatternItem(packageIdPattern)]);
            var nonExistantSourceInSourceMapping2 = new PackageSourceMappingSourceItem(nonExistantSourceName2, [new PackagePatternItem(packageIdPattern)]);
            var unitTestingSourceMapping3 = new PackageSourceMappingSourceItem(sourceName3, [new PackagePatternItem(packageIdPattern)]);

            _vsSettings.AddOrUpdate(ConfigurationConstants.PackageSourceMapping, unitTestingSourceMapping1);
            _vsSettings.AddOrUpdate(ConfigurationConstants.PackageSourceMapping, nonExistantSourceInSourceMapping2);
            _vsSettings.AddOrUpdate(ConfigurationConstants.PackageSourceMapping, unitTestingSourceMapping3);

            PackageSourceMappingPage instance = CreateInstance(_vsSettings);

            _packageSources =
            [
                new PackageSource(sourceUrl1, sourceName1, isEnabled: true),
                // nonExistantSourceName is not added here intentionally.
                new PackageSource(sourceUrl3, sourceName3, isEnabled: true)
            ];

            // Configure Unified Settings input to modify an existing package source mapping.

            // Unchanged package source mapping.
            Dictionary<string, object> sourceMappingDictionary1 = new Dictionary<string, object>();
            sourceMappingDictionary1[PackageSourceMappingPage.MonikerPackageId] = unitTestingSourceMapping1.Patterns.Single().Pattern;
            sourceMappingDictionary1[PackageSourceMappingPage.MonikerSourceNames] = new List<string>() { unitTestingSourceMapping1.Key };

            Dictionary<string, object> sourceMappingDictionary2 = new Dictionary<string, object>();
            sourceMappingDictionary2[PackageSourceMappingPage.MonikerPackageId] = nonExistantSourceInSourceMapping2.Patterns.Single().Pattern;
            sourceMappingDictionary2[PackageSourceMappingPage.MonikerSourceNames] = new List<string>() { nonExistantSourceInSourceMapping2.Key };

            // Unchanged package source mapping.
            Dictionary<string, object> sourceMappingDictionary3 = new Dictionary<string, object>();
            sourceMappingDictionary3[PackageSourceMappingPage.MonikerPackageId] = unitTestingSourceMapping3.Patterns.Single().Pattern;
            sourceMappingDictionary3[PackageSourceMappingPage.MonikerSourceNames] = new List<string>() { unitTestingSourceMapping3.Key };

            // Add a new package source mapping for another existing source.
            Dictionary<string, object> sourceMappingDictionary4 = new Dictionary<string, object>();
            sourceMappingDictionary4[PackageSourceMappingPage.MonikerPackageId] = packageIdPattern;
            sourceMappingDictionary4[PackageSourceMappingPage.MonikerSourceNames] = new List<string>() { addNewPackageSourceName4 };

            IList<IDictionary<string, object>> sourceMappingDictionaryList =
                new List<IDictionary<string, object>>(capacity: 3)
                {
                    sourceMappingDictionary1, // Pre-existing and unchanged.
                    sourceMappingDictionary2, // Pre-existing and unchanged, with an unconfigured Package Source.
                    sourceMappingDictionary3, // Pre-existing and unchanged.
                    sourceMappingDictionary4 // A newly added source mapping.
                };

            // Act
            ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>> resultGetValueBeforeAddNew =
                await instance.GetValueAsync<IReadOnlyList<IDictionary<string, object>>>(
                    moniker: PackageSourceMappingPage.MonikerPackageSourceMapping,
                    cancellationToken: CancellationToken.None);

            ExternalSettingOperationResult result = await instance.SetValueAsync(
                PackageSourceMappingPage.MonikerPackageSourceMapping,
                sourceMappingDictionaryList,
                CancellationToken.None);

            // Assert

            resultGetValueBeforeAddNew.Should().NotBeNull();
            resultGetValueBeforeAddNew.Should().BeOfType<ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success>();
            IReadOnlyList<IDictionary<string, object>> successResultGetValueBeforeAddNew =
                ((ExternalSettingOperationResult<IReadOnlyList<IDictionary<string, object>>>.Success)resultGetValueBeforeAddNew)
                .Value;

            // Initially, 1 source mapping exists for 3 package sources.
            // 1 of these package sources is not configured.
            successResultGetValueBeforeAddNew.Should().HaveCount(1);
            successResultGetValueBeforeAddNew[0].Should().ContainKey(PackageSourceMappingPage.MonikerPackageId);
            successResultGetValueBeforeAddNew[0].TryGetValue(PackageSourceMappingPage.MonikerPackageId, out object? packageIdObject).Should().BeTrue();
            ((string)packageIdObject).Should().Be(packageIdPattern);

            successResultGetValueBeforeAddNew[0].Should().ContainKey(PackageSourceMappingPage.MonikerSourceNames);
            successResultGetValueBeforeAddNew[0].TryGetValue(PackageSourceMappingPage.MonikerSourceNames, out object? sourceNamesObject).Should().BeTrue();
            var sourceNamesBeforeAddNew = ((List<string>)sourceNamesObject);
            sourceNamesBeforeAddNew.Should().HaveCount(3);
            sourceNamesBeforeAddNew.Should().ContainEquivalentOf(sourceName1);
            sourceNamesBeforeAddNew.Should().ContainEquivalentOf(nonExistantSourceName2);
            sourceNamesBeforeAddNew.Should().ContainEquivalentOf(sourceName3);

            // The customer added a new package source mapping, so now 4 total are expected.
            result.Should().NotBeNull();
            result.Should().BeOfType<ExternalSettingOperationResult.Success>();
            IReadOnlyList<PackageSourceMappingSourceItem> packageSourceMappingItems = instance._packageSourceMappingProvider.GetPackageSourceMappingItems();
            packageSourceMappingItems.Should().HaveCount(4);
            packageSourceMappingItems.Should().Contain(unitTestingSourceMapping1);
            packageSourceMappingItems.Should().Contain(nonExistantSourceInSourceMapping2);
            packageSourceMappingItems.Should().Contain(unitTestingSourceMapping3);

            var foundResult = packageSourceMappingItems.Should().Contain(item => item.Key.Equals(addNewPackageSourceName4), because: "The package source was added in a new source mapping.");
            PackageSourceMappingSourceItem foundNewPackageSourceMapping = foundResult.Subject;
            foundNewPackageSourceMapping.Patterns.Should().ContainSingle(packagePatternItem => packagePatternItem.Pattern == packageIdPattern, because: "One pattern was added to the new source mapping.");
        }
    }
}

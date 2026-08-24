// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect.Configuration;
using Microsoft.IdentityModel.Telemetry;
using Microsoft.IdentityModel.Telemetry.Tests;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    [ResetAppContextSwitches]
    [Collection(nameof(AppContextSwitches.UpdateConfigAsBlocking))]
    public class ConfigurationManagerTelemetryTests
    {
        [Fact]
        public async Task RequestRefresh_ExpectedTagsExist()
        {
            await RequestRefresh_ExpectedTagsBody();
        }

        [Fact]
        public async Task RequestRefresh_ExpectedTagsExist_Blocking()
        {
            AppContext.SetSwitch(AppContextSwitches.UpdateConfigAsBlockingSwitch, true);
            await RequestRefresh_ExpectedTagsBody(true);
        }

        private static async Task RequestRefresh_ExpectedTagsBody(bool blocking = false)
        {
            // arrange
            var testTelemetryClient = new MockTelemetryClient();
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                OpenIdConfigData.AccountsGoogle,
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever(),
                new OpenIdConnectConfigurationValidator())
            {
                TelemetryClient = testTelemetryClient
            };

            AutoResetEvent resetEvent = ConfigurationManagerTests.SetupResetEvent(configurationManager);

            // act
            // Retrieve the configuration for the first time
            await configurationManager.GetConfigurationAsync();
            testTelemetryClient.ClearExportedItems();

            // Manually request a config refresh
            configurationManager.RequestRefresh();
            await configurationManager.GetConfigurationAsync();

            if (!blocking)
                ConfigurationManagerTests.WaitOrFail(resetEvent);

            // assert
            var expectedCounterTagList = new Dictionary<string, object>
            {
                { TelemetryConstants.MetadataAddressTag, OpenIdConfigData.AccountsGoogle },
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.OperationStatusTag, TelemetryConstants.Protocols.Manual },
                // This tag is set to ConfigurationSourceRetriever for blocking, and ConfigurationSourceUnknown for non-blocking due to the difference in implementation.
                // On manual refreshes, we don't know the source of the configuration upfront , so we set it to Unknown.
                { TelemetryConstants.ConfigurationSourceTag, blocking == true ? TelemetryConstants.Protocols.ConfigurationSourceRetriever :TelemetryConstants.Protocols.ConfigurationSourceUnknown },
            };

            var expectedHistogramTagList = new Dictionary<string, object>
            {
                { TelemetryConstants.MetadataAddressTag, OpenIdConfigData.AccountsGoogle },
                { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                { TelemetryConstants.ConfigurationSourceTag, TelemetryConstants.Protocols.ConfigurationSourceRetriever  },
            };

            await ConfigurationManagerTests.PollForConditionAsync(
                () => expectedCounterTagList.Count == testTelemetryClient.ExportedItems.Count &&
                    expectedHistogramTagList.Count == testTelemetryClient.ExportedHistogramItems.Count,
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromSeconds(20));

            Assert.Equal(expectedCounterTagList, testTelemetryClient.ExportedItems);
            Assert.Equal(expectedHistogramTagList, testTelemetryClient.ExportedHistogramItems);
        }

        [Theory, MemberData(nameof(GetConfiguration_ExpectedTagList_TheoryData), false, DisableDiscoveryEnumeration = true)]
        public async Task GetConfigurationAsync_ExpectedTagsExist(ConfigurationManagerTelemetryTheoryData<OpenIdConnectConfiguration> theoryData)
        {
            await GetConfigurationAsync_ExpectedTagList_Body(theoryData);
        }

        [Theory, MemberData(nameof(GetConfiguration_ExpectedTagList_TheoryData), true, DisableDiscoveryEnumeration = true)]
        public async Task GetConfigurationAsync_ExpectedTagsExist_Blocking(ConfigurationManagerTelemetryTheoryData<OpenIdConnectConfiguration> theoryData)
        {
            AppContext.SetSwitch(AppContextSwitches.UpdateConfigAsBlockingSwitch, true);
            theoryData.ExpectedTagList[TelemetryConstants.ConfigurationSourceTag] = TelemetryConstants.Protocols.ConfigurationSourceRetriever;
            await GetConfigurationAsync_ExpectedTagList_Body(theoryData, true);
        }

        private static async Task GetConfigurationAsync_ExpectedTagList_Body(
            ConfigurationManagerTelemetryTheoryData<OpenIdConnectConfiguration> theoryData,
            bool blocking = false)
        {
            var testTelemetryClient = new MockTelemetryClient();
            var timeProvider = new FakeTimeProvider();

            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                theoryData.MetadataAddress,
                new OpenIdConnectConfigurationRetriever(),
                theoryData.DocumentRetriever,
                theoryData.ConfigurationValidator)
            {
                TelemetryClient = testTelemetryClient,
                TimeProvider = timeProvider,
            };

            AutoResetEvent resetEvent = ConfigurationManagerTests.SetupResetEvent(configurationManager);

            OpenIdConnectConfiguration firstConfig = null;
            OpenIdConnectConfiguration secondConfig = null;

            try
            {
                firstConfig = await configurationManager.GetConfigurationAsync();
                if (theoryData.AdjustTime.HasValue)
                {
                    testTelemetryClient.ClearExportedItems();
                    timeProvider.Advance(theoryData.AdjustTime.Value);
                    secondConfig = await configurationManager.GetConfigurationAsync();

                    if (!blocking)
                        ConfigurationManagerTests.WaitOrFail(resetEvent);
                }
            }
            catch (Exception)
            {
                // Ignore exceptions
            }

            await ConfigurationManagerTests.PollForConditionAsync(
                () => theoryData.ExpectedTagList.Count == testTelemetryClient.ExportedItems.Count,
                TimeSpan.FromMilliseconds(250),
                TimeSpan.FromSeconds(20));

            DateTimeOffset syncAfter = (DateTimeOffset)TestUtilities.GetField(configurationManager, "_syncAfter");

            Assert.Equal(theoryData.ExpectedTagList, testTelemetryClient.ExportedItems);
        }

        public static TheoryData<ConfigurationManagerTelemetryTheoryData<OpenIdConnectConfiguration>> GetConfiguration_ExpectedTagList_TheoryData(bool blocking)
        {
            return new TheoryData<ConfigurationManagerTelemetryTheoryData<OpenIdConnectConfiguration>>
            {
                new ConfigurationManagerTelemetryTheoryData<OpenIdConnectConfiguration>("Success-retrieve from endpoint")
                {
                    MetadataAddress = OpenIdConfigData.AccountsGoogle,
                    ConfigurationValidator = new OpenIdConnectConfigurationValidator(),
                    ExpectedTagList = new Dictionary<string, object>
                    {
                        { TelemetryConstants.MetadataAddressTag, OpenIdConfigData.AccountsGoogle },
                        { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                        { TelemetryConstants.OperationStatusTag, TelemetryConstants.Protocols.FirstRefresh },
                        { TelemetryConstants.ConfigurationSourceTag, TelemetryConstants.Protocols.ConfigurationSourceRetriever },
                    }
                },
                new ConfigurationManagerTelemetryTheoryData<OpenIdConnectConfiguration>("Failure-invalid metadata address")
                {
                    MetadataAddress = OpenIdConfigData.HttpsBadUri,
                    ConfigurationValidator = new OpenIdConnectConfigurationValidator(),
                    ExpectedTagList = new Dictionary<string, object>
                    {
                        { TelemetryConstants.MetadataAddressTag, OpenIdConfigData.HttpsBadUri },
                        { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                        { TelemetryConstants.OperationStatusTag, TelemetryConstants.Protocols.FirstRefresh },
                        { TelemetryConstants.ConfigurationSourceTag, TelemetryConstants.Protocols.ConfigurationSourceRetriever },
                        { TelemetryConstants.ExceptionTypeTag, new IOException().GetType().ToString() },
                    }
                },
                new ConfigurationManagerTelemetryTheoryData<OpenIdConnectConfiguration>("Failure-invalid config")
                {
                    MetadataAddress = OpenIdConfigData.JsonFile,
                    DocumentRetriever = new FileDocumentRetriever(),
                    // The config being loaded has two keys; require three to force invalidity
                    ConfigurationValidator = new OpenIdConnectConfigurationValidator() { MinimumNumberOfKeys = 3 },
                    ExpectedTagList = new Dictionary<string, object>
                    {
                        { TelemetryConstants.MetadataAddressTag, OpenIdConfigData.JsonFile },
                        { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                        { TelemetryConstants.OperationStatusTag, TelemetryConstants.Protocols.FirstRefresh },
                        { TelemetryConstants.ConfigurationSourceTag, TelemetryConstants.Protocols.ConfigurationSourceRetriever },
                        { TelemetryConstants.ExceptionTypeTag, new InvalidConfigurationException().GetType().ToString() },
                    }
                },
                new ConfigurationManagerTelemetryTheoryData<OpenIdConnectConfiguration>("Success-refresh")
                {
                    MetadataAddress = OpenIdConfigData.AADCommonUrl,
                    ConfigurationValidator = new OpenIdConnectConfigurationValidator(),
                    AdjustTime = TimeSpan.FromDays(1),
                    ExpectedTagList = new Dictionary<string, object>
                    {
                        { TelemetryConstants.MetadataAddressTag, OpenIdConfigData.AADCommonUrl },
                        { TelemetryConstants.IdentityModelVersionTag, IdentityModelTelemetryUtil.ClientVer },
                        { TelemetryConstants.OperationStatusTag, TelemetryConstants.Protocols.Automatic },
                        // This tag is set to ConfigurationSourceRetriever for blocking, and ConfigurationSourceUnknown for non-blocking due to the difference in implementation.
                        // On manual refreshes, we don't know the source of the configuration upfront , so we set it to Unknown.
                        { TelemetryConstants.ConfigurationSourceTag, blocking ? TelemetryConstants.Protocols.ConfigurationSourceRetriever : TelemetryConstants.Protocols.ConfigurationSourceUnknown }
                    }
                },
            };
        }
    }

    public class ConfigurationManagerTelemetryTheoryData<T> : TheoryDataBase where T : class
    {
        public ConfigurationManagerTelemetryTheoryData(string testId) : base(testId) { }

        public string MetadataAddress { get; set; }

        public IDocumentRetriever DocumentRetriever { get; set; } = new HttpDocumentRetriever();

        public IConfigurationValidator<T> ConfigurationValidator { get; set; }

        public TimeSpan? AdjustTime { get; set; }

        public Dictionary<string, object> ExpectedTagList { get; set; }
    }
}

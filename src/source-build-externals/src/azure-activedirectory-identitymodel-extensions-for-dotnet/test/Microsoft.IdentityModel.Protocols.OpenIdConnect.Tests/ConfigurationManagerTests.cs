// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// Ignore Spelling: Metadata Validator Retreiver

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect.Configuration;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationManagerTests
    {

        [Theory, MemberData(nameof(ConstructorTheoryData))]
        public void OpenIdConnectConstructor(ConfigurationManagerTheoryData<OpenIdConnectConfiguration> theoryData)
        {
            TestUtilities.WriteHeader($"{this}.OpenIdConnectConstructor", theoryData);
            try
            {
                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(theoryData.MetadataAddress, theoryData.ConfigurationRetreiver, theoryData.DocumentRetriever, theoryData.ConfigurationValidator);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<ConfigurationManagerTheoryData<OpenIdConnectConfiguration>> ConstructorTheoryData
        {
            get
            {
                var theoryData = new TheoryData<ConfigurationManagerTheoryData<OpenIdConnectConfiguration>>();

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = new OpenIdConnectConfigurationValidator(),
                    DocumentRetriever = new HttpDocumentRetriever(),
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    First = true,
                    MetadataAddress = null,
                    TestId = "MetadataAddress: NULL"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = null,
                    ConfigurationValidator = new OpenIdConnectConfigurationValidator(),
                    DocumentRetriever = new HttpDocumentRetriever(),
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    MetadataAddress = "OpenIdConnectMetadata.json",
                    TestId = "ConfigurationRetreiver: NULL"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = new OpenIdConnectConfigurationValidator(),
                    DocumentRetriever = null,
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    MetadataAddress = "OpenIdConnectMetadata.json",
                    TestId = "DocumentRetriever: NULL"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = null,
                    DocumentRetriever = new HttpDocumentRetriever(),
                    ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                    MetadataAddress = "OpenIdConnectMetadata.json",
                    TestId = "ConfigurationValidator: NULL"
                });

                return theoryData;
            }
        }

        [Fact]
        public void Defaults()
        {
            TestUtilities.WriteHeader($"{this}.Defaults", "Defaults", true);

            Assert.Equal(ConfigurationManager<OpenIdConnectConfiguration>.DefaultAutomaticRefreshInterval, new TimeSpan(0, 12, 0, 0));
            Assert.Equal(ConfigurationManager<OpenIdConnectConfiguration>.DefaultRefreshInterval, new TimeSpan(0, 0, 5, 0));
            Assert.Equal(ConfigurationManager<OpenIdConnectConfiguration>.MinimumAutomaticRefreshInterval, new TimeSpan(0, 0, 5, 0));
            Assert.Equal(ConfigurationManager<OpenIdConnectConfiguration>.MinimumRefreshInterval, new TimeSpan(0, 0, 0, 1));
        }

        [Fact]
        public void FetchMetadataFailureTest()
        {
            var context = new CompareContext($"{this}.FetchMetadataFailureTest");

            var documentRetriever = new HttpDocumentRetriever(HttpResponseMessageUtils.SetupHttpClientThatReturns("OpenIdConnectMetadata.json", HttpStatusCode.NotFound));
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), documentRetriever);

            // First time to fetch metadata
            try
            {
                var configuration = configManager.GetConfigurationAsync().Result;
            }
            catch (Exception firstFetchMetadataFailure)
            {
                if (firstFetchMetadataFailure.InnerException == null)
                    context.AddDiff($"Expected exception to contain inner exception for fetch metadata failure.");

                // Fetch metadata again during refresh interval, the exception should be same from above
                try
                {
                    var configuration = configManager.GetConfigurationAsync().Result;
                }
                catch (Exception secondFetchMetadataFailure)
                {
                    if (secondFetchMetadataFailure.InnerException == null)
                        context.AddDiff($"Expected exception to contain inner exception for fetch metadata failure.");

                    IdentityComparer.AreEqual(firstFetchMetadataFailure, secondFetchMetadataFailure, context);
                }
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void BootstrapRefreshIntervalTest()
        {
            var context = new CompareContext($"{this}.BootstrapRefreshIntervalTest");

            var documentRetriever = new HttpDocumentRetriever(HttpResponseMessageUtils.SetupHttpClientThatReturns("OpenIdConnectMetadata.json", HttpStatusCode.NotFound));
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), documentRetriever) { RefreshInterval = TimeSpan.FromSeconds(2) };

            // First time to fetch metadata.
            try
            {
                var configuration = configManager.GetConfigurationAsync().Result;
            }
            catch (Exception firstFetchMetadataFailure)
            {
                // Refresh interval is BootstrapRefreshInterval
                var syncAfter = configManager.GetType().GetField("_syncAfter", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(configManager);
                if ((DateTimeOffset)syncAfter > DateTime.UtcNow + TimeSpan.FromSeconds(2))
                    context.AddDiff($"Expected the refresh interval is longer than 2 seconds.");

                if (firstFetchMetadataFailure.InnerException == null)
                    context.AddDiff($"Expected exception to contain inner exception for fetch metadata failure.");

                // Fetch metadata again during refresh interval, the exception should be same from above.
                try
                {
                    configManager.RequestRefresh();
                    var configuration = configManager.GetConfigurationAsync().Result;
                }
                catch (Exception secondFetchMetadataFailure)
                {
                    if (secondFetchMetadataFailure.InnerException == null)
                        context.AddDiff($"Expected exception to contain inner exception for fetch metadata failure.");

                    syncAfter = configManager.GetType().GetField("_syncAfter", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(configManager);

                    // Refresh interval is RefreshInterval
                    if ((DateTimeOffset)syncAfter > DateTime.UtcNow + configManager.RefreshInterval)
                        context.AddDiff($"Expected the refresh interval is longer than 2 seconds.");

                    IdentityComparer.AreEqual(firstFetchMetadataFailure, secondFetchMetadataFailure, context);
                }
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void GetSets()
        {
            TestUtilities.WriteHeader($"{this}.GetSets", "GetSets", true);

            int ExpectedPropertyCount = 7;
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), new FileDocumentRetriever());
            Type type = typeof(ConfigurationManager<OpenIdConnectConfiguration>);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != ExpectedPropertyCount)
                Assert.True(false, $"Number of properties has changed from {ExpectedPropertyCount} to: " + properties.Length + ", adjust tests");

            var defaultAutomaticRefreshInterval = ConfigurationManager<OpenIdConnectConfiguration>.DefaultAutomaticRefreshInterval;
            var defaultRefreshInterval = ConfigurationManager<OpenIdConnectConfiguration>.DefaultRefreshInterval;
            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("RefreshInterval", new List<object>{defaultRefreshInterval, TimeSpan.FromHours(1), TimeSpan.FromHours(10)}),
                },
                Object = configManager,
            };

            TestUtilities.GetSet(context);
            TestUtilities.SetGet(configManager, "AutomaticRefreshInterval", TimeSpan.FromMilliseconds(1), ExpectedException.ArgumentOutOfRangeException(substringExpected: "IDX10108:"), context);
            TestUtilities.SetGet(configManager, "RefreshInterval", TimeSpan.FromMilliseconds(1), ExpectedException.ArgumentOutOfRangeException(substringExpected: "IDX10107:"), context);
            TestUtilities.SetGet(configManager, "RefreshInterval", Timeout.InfiniteTimeSpan, ExpectedException.ArgumentOutOfRangeException(substringExpected: "IDX10107:"), context);
            TestUtilities.SetGet(configManager, "LastKnownGoodConfiguration", new OpenIdConnectConfiguration(), ExpectedException.NoExceptionExpected, context);
            TestUtilities.SetGet(configManager, "UseLastKnownGoodConfiguration", true, ExpectedException.NoExceptionExpected, context);
            TestUtilities.SetGet(configManager, "MetadataAddress", "OpenIdConnectMetadata2.json", ExpectedException.NoExceptionExpected, context);
            TestUtilities.SetGet(configManager, "LastKnownGoodLifetime", TimeSpan.FromDays(5) - TimeSpan.FromDays(15), ExpectedException.ArgumentOutOfRangeException(substringExpected: "IDX10110:"), context);
            TestUtilities.AssertFailIfErrors("ConfigurationManager_GetSets", context.Errors);
        }

        [Fact]
        public void GetConfiguration()
        {
            var docRetriever = new FileDocumentRetriever();
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), docRetriever);
            var context = new CompareContext($"{this}.GetConfiguration");

            // AutomaticRefreshInterval interval should return same config.
            var configuration = configManager.GetConfigurationAsync().Result;
            configManager.MetadataAddress = "OpenIdConnectMetadata2.json";
            var configuration2 = configManager.GetConfigurationAsync().Result;
            IdentityComparer.AreEqual(configuration, configuration2, context);
            if (!object.ReferenceEquals(configuration, configuration2))
                context.Diffs.Add("!object.ReferenceEquals(configuration, configuration2)");

            // AutomaticRefreshInterval should pick up new bits.
            configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), docRetriever);
            configManager.RequestRefresh();
            configuration = configManager.GetConfigurationAsync().Result;
            TestUtilities.SetField(configManager, "_lastRefresh", DateTimeOffset.UtcNow - TimeSpan.FromHours(1));
            configManager.MetadataAddress = "OpenIdConnectMetadata2.json";
            configManager.RequestRefresh();
            configuration2 = configManager.GetConfigurationAsync().Result;
            if (IdentityComparer.AreEqual(configuration, configuration2))
                context.Diffs.Add("IdentityComparer.AreEqual(configuration, configuration2)");

            if (object.ReferenceEquals(configuration, configuration2))
                context.Diffs.Add("object.ReferenceEquals(configuration, configuration2) (2)");

            // RefreshInterval is set to MaxValue
            configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), docRetriever);
            configuration = configManager.GetConfigurationAsync().Result;
            configManager.RefreshInterval = TimeSpan.MaxValue;
            configManager.MetadataAddress = "OpenIdConnectMetadata2.json";
            configuration2 = configManager.GetConfigurationAsync().Result;
            IdentityComparer.AreEqual(configuration, configuration2, context);
            if (!object.ReferenceEquals(configuration, configuration2))
                context.Diffs.Add("!object.ReferenceEquals(configuration, configuration2) (3)");

            configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), docRetriever);
            configuration = configManager.GetConfigurationAsync().Result;
            // First force refresh should pickup new config
            configManager.RequestRefresh();
            configManager.MetadataAddress = "OpenIdConnectMetadata2.json";
            configuration2 = configManager.GetConfigurationAsync().Result;
            if (IdentityComparer.AreEqual(configuration, configuration2))
                context.Diffs.Add("IdentityComparer.AreEqual(configuration, configuration2), should be different");
            if (object.ReferenceEquals(configuration, configuration2))
                context.Diffs.Add("object.ReferenceEquals(configuration, configuration2) (4)");
            // Next force refresh shouldn't pickup new config, as RefreshInterval hasn't passed
            configManager.RequestRefresh();
            configManager.MetadataAddress = "OpenIdConnectMetadata.json";
            var configuration3 = configManager.GetConfigurationAsync().Result;
            IdentityComparer.AreEqual(configuration2, configuration3, context);
            if (!object.ReferenceEquals(configuration2, configuration3))
                context.Diffs.Add("!object.ReferenceEquals(configuration2, configuration3) (5)");
            // Next force refresh should pickup config since, RefreshInterval is set to 1s
            configManager.RefreshInterval = TimeSpan.FromSeconds(1);
            Thread.Sleep(1000);
            configManager.RequestRefresh();
            var configuration4 = configManager.GetConfigurationAsync().Result;
            if (IdentityComparer.AreEqual(configuration2, configuration4))
                context.Diffs.Add("IdentityComparer.AreEqual(configuration2, configuration4), should be different");
            if (object.ReferenceEquals(configuration2, configuration4))
                context.Diffs.Add("object.ReferenceEquals(configuration2, configuration4) (6)");

            // Refresh should force pickup of new config
            configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), docRetriever);
            configuration = configManager.GetConfigurationAsync().Result;
            TestUtilities.SetField(configManager, "_lastRefresh", DateTimeOffset.UtcNow - TimeSpan.FromHours(1));
            configManager.RequestRefresh();
            configManager.MetadataAddress = "OpenIdConnectMetadata2.json";
            configuration2 = configManager.GetConfigurationAsync().Result;
            if (IdentityComparer.AreEqual(configuration, configuration2))
                context.Diffs.Add("IdentityComparer.AreEqual(configuration, configuration2), should be different");

            if (object.ReferenceEquals(configuration, configuration2))
                context.Diffs.Add("object.ReferenceEquals(configuration, configuration2)");

            // Refresh set to MaxValue
            configManager.RefreshInterval = TimeSpan.MaxValue;
            configuration = configManager.GetConfigurationAsync().Result;
            IdentityComparer.AreEqual(configuration, configuration2, context);
            if (!object.ReferenceEquals(configuration, configuration2))
                context.Diffs.Add("!object.ReferenceEquals(configuration, configuration2)");

            // get configuration from http address, should throw
            configManager = new ConfigurationManager<OpenIdConnectConfiguration>("http://someaddress.com", new OpenIdConnectConfigurationRetriever());
            var ee = new ExpectedException(typeof(InvalidOperationException), "IDX20803:", typeof(ArgumentException));
            try
            {
                configuration = configManager.GetConfigurationAsync().Result;
                ee.ProcessNoException(context);
            }
            catch (AggregateException ex)
            {
                // this should throw, because last configuration retrived was null
                Assert.Throws<AggregateException>(() => configuration = configManager.GetConfigurationAsync().Result);

                ex.Handle((x) =>
                {
                    ee.ProcessException(x, context);
                    return true;
                });
            }

            // get configuration from https address, should throw
            configManager = new ConfigurationManager<OpenIdConnectConfiguration>("https://someaddress.com", new OpenIdConnectConfigurationRetriever());
            ee = new ExpectedException(typeof(InvalidOperationException), "IDX20803:", typeof(IOException));
            try
            {
                configuration = configManager.GetConfigurationAsync().Result;
                ee.ProcessNoException(context);
            }
            catch (AggregateException ex)
            {
                // this should throw, because last configuration retrived was null
                Assert.Throws<AggregateException>(() => configuration = configManager.GetConfigurationAsync().Result);

                ex.Handle((x) =>
                {
                    ee.ProcessException(x, context);
                    return true;
                });
            }

            // get configuration with unsuccessful HTTP response status code
            configManager = new ConfigurationManager<OpenIdConnectConfiguration>("https://httpstat.us/429", new OpenIdConnectConfigurationRetriever());
            ee = new ExpectedException(typeof(InvalidOperationException), "IDX20803:", typeof(IOException));
            try
            {
                configuration = configManager.GetConfigurationAsync().Result;
                ee.ProcessNoException(context);
            }
            catch (AggregateException ex)
            {
                // this should throw, because last configuration retrived was null
                Assert.Throws<AggregateException>(() => configuration = configManager.GetConfigurationAsync().Result);

                ex.Handle((x) =>
                {
                    ee.ProcessException(x, context);
                    return true;
                });
            }

            // Unable to obtain a new configuration, but _currentConfiguration is not null so it should be returned.
            configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), docRetriever);
            configuration = configManager.GetConfigurationAsync().Result;
            TestUtilities.SetField(configManager, "_lastRefresh", DateTimeOffset.UtcNow - TimeSpan.FromHours(1));
            configManager.RequestRefresh();
            configManager.MetadataAddress = "http://someaddress.com";
            configuration2 = configManager.GetConfigurationAsync().Result;
            IdentityComparer.AreEqual(configuration, configuration2, context);
            if (!object.ReferenceEquals(configuration, configuration2))
                context.Diffs.Add("!object.ReferenceEquals(configuration, configuration2)");

            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that the LastKnownGood (LKG) Configuration lifetime is properly reset at the time
        // a new LKG is set.
        [Fact]
        public void ResetLastKnownGoodLifetime()
        {
            TestUtilities.WriteHeader($"{this}.ResetLastKnownGoodLifetime");
            var context = new CompareContext();

            var validConfig = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "oauth/token", Issuer = Default.Issuer };
            var configurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(validConfig);

            // set and retrieve config in order to set the first access time
            configurationManager.LastKnownGoodConfiguration = validConfig;
            var lkg = configurationManager.LastKnownGoodConfiguration;
            var lkgConfigFirstUseField = typeof(BaseConfigurationManager).GetField("_lastKnownGoodConfigFirstUse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var lkgConfigFirstUse1 = lkgConfigFirstUseField.GetValue(configurationManager as BaseConfigurationManager);

            Thread.Sleep(1);

            // set and retrieve config again to reset first access time
            configurationManager.LastKnownGoodConfiguration = validConfig;
            lkg = configurationManager.LastKnownGoodConfiguration;
            var lkgConfigFirstUse2 = lkgConfigFirstUseField.GetValue(configurationManager as BaseConfigurationManager);

            if (lkgConfigFirstUse1 == null)
                context.AddDiff("Last known good first use time was not properly set for the first configuration.");

            if (lkgConfigFirstUse2 == null)
                context.AddDiff("Last known good first use time was not properly set for the second configuration.");

            //LKG config first use was not reset when a new configuration was set
            if (lkgConfigFirstUse1.Equals(lkgConfigFirstUse2))
                context.AddDiff("Last known good first use time was not reset when a new LKG configuration was set.");

            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void TestConfigurationComparer()
        {
            TestUtilities.WriteHeader($"{this}.TestConfigurationComparer", "TestConfigurationComparer", true);
            var context = new CompareContext();

            var config = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
            config.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);
            config.SigningKeys.Add(KeyingMaterial.DefaultRsaSecurityKey1);
            config.SigningKeys.Add(KeyingMaterial.DefaultRsaSecurityKey2);

            var configWithSameKeysDiffOrder = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
            configWithSameKeysDiffOrder.SigningKeys.Add(KeyingMaterial.DefaultRsaSecurityKey1);
            configWithSameKeysDiffOrder.SigningKeys.Add(KeyingMaterial.DefaultX509Key_2048);
            configWithSameKeysDiffOrder.SigningKeys.Add(KeyingMaterial.DefaultRsaSecurityKey2);

            var configWithOverlappingKey = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
            configWithOverlappingKey.SigningKeys.Add(Default.SymmetricSigningKey256);

            var configWithOverlappingKeyDiffissuer = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer + "1" };
            configWithOverlappingKeyDiffissuer.SigningKeys.Add(Default.SymmetricSigningKey256);

            var configWithSameKidDiffKeyMaterial = new OpenIdConnectConfiguration() { TokenEndpoint = Default.Issuer + "/oauth/token", Issuer = Default.Issuer };
            configWithSameKidDiffKeyMaterial.SigningKeys.Add(new SymmetricSecurityKey(KeyingMaterial.DefaultSymmetricSecurityKey_128.Key) { KeyId = KeyingMaterial.DefaultSymmetricSecurityKey_256.KeyId });

            var configurationManager = new MockConfigurationManager<OpenIdConnectConfiguration>(config, config);
            IdentityComparer.AreEqual(configurationManager.GetValidLkgConfigurations().Length, 1, context);

            configurationManager.LastKnownGoodConfiguration = configWithSameKeysDiffOrder;
            IdentityComparer.AreEqual(configurationManager.GetValidLkgConfigurations().Length, 1, context);

            configurationManager.LastKnownGoodConfiguration = configWithOverlappingKey;
            IdentityComparer.AreEqual(configurationManager.GetValidLkgConfigurations().Length, 2, context);

            configurationManager.LastKnownGoodConfiguration = configWithOverlappingKeyDiffissuer;
            IdentityComparer.AreEqual(configurationManager.GetValidLkgConfigurations().Length, 3, context);

            configurationManager.LastKnownGoodConfiguration = configWithSameKidDiffKeyMaterial;
            IdentityComparer.AreEqual(configurationManager.GetValidLkgConfigurations().Length, 4, context);

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ValidateOpenIdConnectConfigurationTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidateOpenIdConnectConfigurationTests(ConfigurationManagerTheoryData<OpenIdConnectConfiguration> theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateOpenIdConnectConfigurationTests");
            var context = new CompareContext();
            OpenIdConnectConfiguration configuration;
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(theoryData.MetadataAddress, theoryData.ConfigurationRetreiver, theoryData.DocumentRetriever, theoryData.ConfigurationValidator);

            if (theoryData.PresetCurrentConfiguration)
                TestUtilities.SetField(configurationManager, "_currentConfiguration", new OpenIdConnectConfiguration() { Issuer = Default.Issuer });

            try
            {
                //create a listener and enable it for logs
                var listener = TestUtils.SampleListener.CreateLoggerListener(EventLevel.Warning);

                configuration = configurationManager.GetConfigurationAsync().Result;

                if (!string.IsNullOrEmpty(theoryData.ExpectedErrorMessage) && !listener.TraceBuffer.Contains(theoryData.ExpectedErrorMessage))
                    context.AddDiff($"Expected exception to contain: '{theoryData.ExpectedErrorMessage}'.{Environment.NewLine}Log is:{Environment.NewLine}'{listener.TraceBuffer}'");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (AggregateException ex)
            {
                // this should throw, because last configuration retrieved was null
                Assert.Throws<AggregateException>(() => configuration = configurationManager.GetConfigurationAsync().Result);

                ex.Handle((x) =>
                {
                    theoryData.ExpectedException.ProcessException(x, context);
                    return true;
                });
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ConfigurationManagerTheoryData<OpenIdConnectConfiguration>> ValidateOpenIdConnectConfigurationTestCases
        {
            get
            {
                var openIdConnectConfigurationValidator = new OpenIdConnectConfigurationValidator();
                var openIdConnectConfigurationValidator2 = new OpenIdConnectConfigurationValidator() { MinimumNumberOfKeys = 3 };
                var theoryData = new TheoryData<ConfigurationManagerTheoryData<OpenIdConnectConfiguration>>();

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator,
                    DocumentRetriever = new FileDocumentRetriever(),
                    First = true,
                    MetadataAddress = "OpenIdConnectMetadata.json",
                    TestId = "ValidConfiguration"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    ExpectedException = new ExpectedException(typeof(InvalidOperationException), "IDX21818:", typeof(InvalidConfigurationException)),
                    MetadataAddress = "OpenIdConnectMetadata.json",
                    TestId = "ValidConfiguration_NotEnoughKey"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    PresetCurrentConfiguration = true,
                    ExpectedErrorMessage = "IDX21818: ",
                    MetadataAddress = "OpenIdConnectMetadata.json",
                    TestId = "ValidConfiguration_NotEnoughKey_PresetCurrentConfiguration"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    ExpectedException = new ExpectedException(typeof(InvalidOperationException), "IDX10810:", typeof(InvalidConfigurationException)),
                    MetadataAddress = "OpenIdConnectMetadataUnrecognizedKty.json",
                    TestId = "InvalidConfiguration_UnrecognizedKty"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    PresetCurrentConfiguration = true,
                    ExpectedErrorMessage = "IDX10810: ",
                    MetadataAddress = "OpenIdConnectMetadataUnrecognizedKty.json",
                    TestId = "InvalidConfiguration_UnrecognizedKty_PresetCurrentConfiguration"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    ExpectedException = new ExpectedException(typeof(InvalidOperationException), "IDX21817:", typeof(InvalidConfigurationException)),
                    MetadataAddress = "JsonWebKeySetUnrecognizedKty.json",
                    TestId = "InvalidConfiguration_EmptyJsonWenKeySet"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    PresetCurrentConfiguration = true,
                    ExpectedErrorMessage = "IDX21817: ",
                    MetadataAddress = "JsonWebKeySetUnrecognizedKty.json",
                    TestId = "InvalidConfiguration_EmptyJsonWenKeySet_PresetCurrentConfiguration"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    ExpectedException = new ExpectedException(typeof(InvalidOperationException), "IDX10814:", typeof(InvalidConfigurationException)),
                    MetadataAddress = "OpenIdConnectMetadataBadRsaDataMissingComponent.json",
                    TestId = "InvalidConfiguration_RsaKeyMissingComponent"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    PresetCurrentConfiguration = true,
                    ExpectedErrorMessage = "IDX10814: ",
                    MetadataAddress = "OpenIdConnectMetadataBadRsaDataMissingComponent.json",
                    TestId = "InvalidConfiguration_RsaKeyMissingComponent_PresetCurrentConfiguration"
                });

                return theoryData;
            }
        }

        public class ConfigurationManagerTheoryData<T> : TheoryDataBase
        {
            public TimeSpan AutomaticRefreshInterval { get; set; }

            public IConfigurationRetriever<T> ConfigurationRetreiver { get; set; }

            public IConfigurationValidator<OpenIdConnectConfiguration> ConfigurationValidator { get; set; }

            public IDocumentRetriever DocumentRetriever { get; set; }

            public string ExpectedExceptionMessage { get; set; }

            public string ExpectedErrorMessage { get; set; }

            public string MetadataAddress { get; set; }

            public bool PresetCurrentConfiguration { get; set; } = false;

            public TimeSpan RefreshInterval { get; set; }

            public bool RequestRefresh { get; set; }

            public override string ToString()
            {
                return $"{TestId}, {MetadataAddress}, {ExpectedException}";
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant

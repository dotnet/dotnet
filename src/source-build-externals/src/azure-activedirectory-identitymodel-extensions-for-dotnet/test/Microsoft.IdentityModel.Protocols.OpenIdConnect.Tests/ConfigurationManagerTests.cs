//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Reflection;
using System.Threading;
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
        public void GetSets()
        {
            TestUtilities.WriteHeader($"{this}.GetSets", "GetSets", true);

            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>("OpenIdConnectMetadata.json", new OpenIdConnectConfigurationRetriever(), new FileDocumentRetriever());
            Type type = typeof(ConfigurationManager<OpenIdConnectConfiguration>);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 7)
                Assert.True(false, "Number of properties has changed from 7 to: " + properties.Length + ", adjust tests");

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

        [Theory, MemberData(nameof(ValidateOpenIdConnectConfigurationTestCases), DisableDiscoveryEnumeration = true)]
        public void ValidateOpenIdConnectConfigurationTests(ConfigurationManagerTheoryData<OpenIdConnectConfiguration> theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ValidateOpenIdConnectConfigurationTests");
            var context = new CompareContext();

            try
            {
                //create a listener and enable it for logs
                var listener = TestUtils.SampleListener.CreateLoggerListener(EventLevel.Warning);

                var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(theoryData.MetadataAddress, theoryData.ConfigurationRetreiver, theoryData.DocumentRetriever, theoryData.ConfigurationValidator);
                var configuration = configurationManager.GetConfigurationAsync().Result;

                if (!string.IsNullOrEmpty(theoryData.ExpectedErrorMessage) && !listener.TraceBuffer.Contains(theoryData.ExpectedErrorMessage))
                    context.AddDiff($"Expected exception to contain: '{theoryData.ExpectedErrorMessage}'.{Environment.NewLine}Log is:{Environment.NewLine}'{listener.TraceBuffer}'");

            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
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
                    ExpectedErrorMessage = "IDX21818: ",
                    MetadataAddress = "OpenIdConnectMetadata.json",
                    TestId = "ValidConfiguration_NotEnoughKey"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    ExpectedErrorMessage = "IDX10810: ",
                    MetadataAddress = "OpenIdConnectMetadataUnrecognizedKty.json",
                    TestId = "InvalidConfiguration_UnrecognizedKty"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    ExpectedErrorMessage = "IDX21817: ",
                    MetadataAddress = "JsonWebKeySetUnrecognizedKty.json",
                    TestId = "InvalidConfiguration_EmptyJsonWenKeySet"
                });

                theoryData.Add(new ConfigurationManagerTheoryData<OpenIdConnectConfiguration>
                {
                    ConfigurationRetreiver = new OpenIdConnectConfigurationRetriever(),
                    ConfigurationValidator = openIdConnectConfigurationValidator2,
                    DocumentRetriever = new FileDocumentRetriever(),
                    ExpectedErrorMessage = "IDX10814: ",
                    MetadataAddress = "OpenIdConnectMetadataBadRsaDataMissingComponent.json",
                    TestId = "InvalidConfiguration_RsaKeyMissingComponent"
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

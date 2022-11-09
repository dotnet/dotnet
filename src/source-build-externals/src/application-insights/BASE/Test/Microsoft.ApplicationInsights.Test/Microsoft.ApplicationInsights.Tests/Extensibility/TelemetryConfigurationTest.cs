namespace Microsoft.ApplicationInsights.Extensibility
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;

    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.Channel;

    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;

    [TestClass]
    public class TelemetryConfigurationTest
    {
        #region W3C
        [TestMethod]
        public void TelemetryConfigurationStaticConstructorSetsW3CToTrueIfNotEnforced()
        {
            try
            {
                // Accessing TelemetryConfiguration trigger static constructor
                var tc = new TelemetryConfiguration();

                Assert.IsTrue(Activity.ForceDefaultIdFormat);
                Assert.AreEqual(ActivityIdFormat.W3C, Activity.DefaultIdFormat);
            }
            finally
            {
                Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;
                Activity.ForceDefaultIdFormat = false;
            }
        }

        #endregion

        [TestMethod]
        public void TelemetryConfigurationIsPublicToAllowUsersManipulateConfigurationProgrammatically()
        {
            Assert.IsTrue(typeof(TelemetryConfiguration).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void NewTelemetryConfigurationWithChannelUsesSpecifiedChannel()
        {
            StubTelemetryChannel stubChannel = new StubTelemetryChannel();
            bool channelDisposed = false;
            stubChannel.OnDispose += () => { channelDisposed = true; };
            TelemetryConfiguration config = new TelemetryConfiguration(string.Empty, stubChannel);
            Assert.AreSame(stubChannel, config.TelemetryChannel);
            config.Dispose();
            Assert.IsFalse(channelDisposed);
        }

        [TestMethod]
        public void NewTelemetryConfigurationWithoutChannelCreatesDefaultInMemoryChannel()
        {
            TelemetryConfiguration config = new TelemetryConfiguration();
            var channel = config.TelemetryChannel as Channel.InMemoryChannel;
            Assert.IsNotNull(channel);
            config.Dispose();
            Assert.IsTrue(channel.IsDisposed);
        }

        [TestMethod]
        public void NewTelemetryConfigurationWithInstrumentationKeyAndChannelUsesSpecifiedKeyAndChannel()
        {
            string expectedKey = "expected";
            StubTelemetryChannel stubChannel = new StubTelemetryChannel();
            bool channelDisposed = false;
            stubChannel.OnDispose += () => { channelDisposed = true; };
            TelemetryConfiguration config = new TelemetryConfiguration(expectedKey, stubChannel);
            Assert.AreEqual(expectedKey, config.InstrumentationKey);
            Assert.AreSame(stubChannel, config.TelemetryChannel);
            config.Dispose();
            Assert.IsFalse(channelDisposed);
        }

        [TestMethod]
        public void NewTelemetryConfigurationWithInstrumentationKeyButNoChannelCreatesDefaultInMemoryChannel()
        {
            string expectedKey = "expected";
            TelemetryConfiguration config = new TelemetryConfiguration(expectedKey);
            Assert.AreEqual(expectedKey, config.InstrumentationKey);
            var channel = config.TelemetryChannel as Channel.InMemoryChannel;
            Assert.IsNotNull(channel);
            config.Dispose();
            Assert.IsTrue(channel.IsDisposed);
        }

        #region Active

        [TestMethod]
        public void ActiveIsPublicToAllowUsersToAccessActiveTelemetryConfigurationInAdvancedScenarios()
        {
            Assert.IsTrue(typeof(TelemetryConfiguration).GetTypeInfo().GetDeclaredProperty("Active").GetGetMethod(true).IsPublic);
        }

        [TestMethod]
        public void ActiveSetterIsInternalAndNotMeantToBeUsedByOurCustomers()
        {
            Assert.IsFalse(typeof(TelemetryConfiguration).GetTypeInfo().GetDeclaredProperty("Active").GetSetMethod(true).IsPublic);
        }
#pragma warning disable 612, 618
        [TestMethod]
        public void ActiveIsLazilyInitializedToDelayCostOfLoadingConfigurationFromFile()
        {
            try
            {
                TelemetryConfiguration.Active = null;
                Assert.IsNotNull(TelemetryConfiguration.Active);
            }
            finally
            {
                TelemetryConfiguration.Active = null;
            }
        }

        [TestMethod]
        public void ActiveInitializesTelemetryModuleCollection()
        {
            TelemetryModules modules = new TestableTelemetryModules();
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (c, m) =>
                {
                    modules = m;
                },
            };

            TelemetryConfiguration.Active = null;
            Assert.IsNotNull(TelemetryConfiguration.Active);

            Assert.AreSame(modules, TelemetryModules.Instance);
        }

        [TestMethod]
        public void ActiveUsesTelemetryConfigurationFactoryToInitializeTheInstance()
        {
            bool factoryInvoked = false;
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (configuration, _) => { factoryInvoked = true; },
            };
            TelemetryConfiguration.Active = null;
            try
            {
                var dummy = TelemetryConfiguration.Active;
                Assert.IsTrue(factoryInvoked);
            }
            finally
            {
                TelemetryConfigurationFactory.Instance = null;
                TelemetryConfiguration.Active = null;
            }
        }

        [TestMethod]
        public void ActiveInitializesSingleInstanceRegardlessOfNumberOfThreadsTryingToAccessIt()
        {
            int numberOfInstancesInitialized = 0;
            TelemetryConfiguration.Active = null;
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (configuration, _) => { Interlocked.Increment(ref numberOfInstancesInitialized); },
            };
            try
            {
                var tasks = new Task[8];
                for (int i = 0; i < tasks.Length; i++)
                {
                    tasks[i] = Task.Run(() => TelemetryConfiguration.Active);
                }

                Task.WaitAll(tasks);
                Assert.AreEqual(1, numberOfInstancesInitialized);
            }
            finally
            {
                TelemetryConfiguration.Active = null;
                TelemetryConfigurationFactory.Instance = null;
            }
        }

        [TestMethod]
        [Timeout(1000)]
        public void ActiveInitializesSingleInstanceWhenConfigurationComponentsAccessActiveRecursively()
        {
            int numberOfInstancesInitialized = 0;
            TelemetryConfiguration.Active = null;
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (configuration, _) =>
                {
                    Interlocked.Increment(ref numberOfInstancesInitialized);
                    var dummy = TelemetryConfiguration.Active;
                },
            };
            try
            {
                var dummy = TelemetryConfiguration.Active;
                Assert.AreEqual(1, numberOfInstancesInitialized);
            }
            finally
            {
                TelemetryConfiguration.Active = null;
                TelemetryConfigurationFactory.Instance = null;
            }
        }
#pragma warning restore 612, 618
        #endregion

        #region CreateDefault

        [TestMethod]
        public void DefaultDoesNotInitializeTelemetryModuleCollection()
        {
            TelemetryModules modules = new TestableTelemetryModules();
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (c, m) =>
                {
                    modules = m;
                },
            };

            Assert.IsNotNull(TelemetryConfiguration.CreateDefault());
            Assert.IsNull(modules);
        }

        [TestMethod]
        public void CreateDefaultReturnsNewConfigurationInstanceInitializedByTelemetryConfigurationFactory()
        {
            TelemetryConfiguration initializedConfiguration = null;
            TelemetryConfigurationFactory.Instance = new StubTelemetryConfigurationFactory
            {
                OnInitialize = (configuration, _) => initializedConfiguration = configuration,
            };
            try
            {
                var defaultConfiguration = TelemetryConfiguration.CreateDefault();
                Assert.IsNotNull(defaultConfiguration);
                Assert.AreSame(defaultConfiguration, initializedConfiguration);
            }
            finally
            {
                TelemetryConfigurationFactory.Instance = null;
            }
        }

        #endregion

        [TestMethod]
        public void DisableTelemetryIsFalseByDefault()
        {
            var configuration = new TelemetryConfiguration();

            Assert.IsFalse(configuration.DisableTelemetry);
        }

        #region InstrumentationKey

        [TestMethod]
        public void InstrumentationKeyIsEmptyStringByDefaultToAvoidNullReferenceExceptionWhenAccessingPropertyValue()
        {
            var configuration = new TelemetryConfiguration();
            Assert.AreEqual(0, configuration.InstrumentationKey.Length);
        }

        [TestMethod]
        public void InstrumentationKeyThrowsArgumentNullExceptionWhenNewValueIsNullToAvoidNullReferenceExceptionWhenAccessingPropertyValue()
        {
            var configuration = new TelemetryConfiguration();
            AssertEx.Throws<ArgumentNullException>(() => configuration.InstrumentationKey = null);
        }

        [TestMethod]
        public void InstrumentationKeyCanBeSetToProgrammaticallyDefineInstrumentationKeyForAllContextsInApplication()
        {
            var configuration = new TelemetryConfiguration();
            configuration.InstrumentationKey = "99C6A712-B2B5-46E3-97F4-F83F69999324";
            Assert.AreEqual("99C6A712-B2B5-46E3-97F4-F83F69999324", configuration.InstrumentationKey);
        }

        #endregion

        #region Connection String
        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_ShouldSetConnectionString()
        {
            var ikey = Guid.NewGuid().ToString();
            var connectionString = $"InstrumentationKey={ikey}";

            var configuration = new TelemetryConfiguration
            {
                ConnectionString = connectionString
            };

            Assert.AreEqual(connectionString, configuration.ConnectionString, "connection string was not set.");
            Assert.AreEqual(ikey, configuration.InstrumentationKey, "instrumentation key was not set.");
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VerifySetConnectionString_ThrowsNullException()
        {
            var configuration = new TelemetryConfiguration
            {
                ConnectionString = null
            };
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsEndpoint()
        {
            var explicitEndpoint = "https://127.0.0.1/";
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint={explicitEndpoint}";

            var configuration = new TelemetryConfiguration
            {
                ConnectionString = connectionString
            };

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual(explicitEndpoint, configuration.EndpointContainer.Ingestion.AbsoluteUri);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsChannelDefaultEndpoint()
        {
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000";

            var channel = new InMemoryChannel();

            var configuration = new TelemetryConfiguration
            {
                TelemetryChannel = channel,
                ConnectionString = connectionString,
            };

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
            Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", channel.EndpointAddress);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsChannelExpliticEndpoint()
        {
            var explicitEndpoint = "https://127.0.0.1/";
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint={explicitEndpoint}";

            var channel = new InMemoryChannel();

            var configuration = new TelemetryConfiguration
            {
                TelemetryChannel = channel,
                ConnectionString = connectionString,
            };

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual(explicitEndpoint, configuration.EndpointContainer.Ingestion.AbsoluteUri);
            Assert.AreEqual(explicitEndpoint + "v2/track", channel.EndpointAddress);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void Configuration_DefaultScenario()
        {
            var configuration = new TelemetryConfiguration();

            Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", configuration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void Configuration_DefaultScenario_ConfigurationConstructor()
        {
            var configuration = new TelemetryConfiguration("00000000-0000-0000-0000-000000000000", new InMemoryChannel());

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", configuration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void Configuration_DefaultScenario_WithConnectionString()
        {
            var configuration = new TelemetryConfiguration
            {
                ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=https://127.0.0.1/"
            };

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://127.0.0.1/v2/track", configuration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void Configuration_CreateDefaultScenario()
        {
            var configuration = TelemetryConfiguration.CreateDefault();

            Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/v2/track", configuration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void Configuration_CreateDefaultScenario_WithConnectionString()
        {
            var configuration = TelemetryConfiguration.CreateDefault();
            configuration.ConnectionString = "InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=https://127.0.0.1/";

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://127.0.0.1/v2/track", configuration.DefaultTelemetrySink.TelemetryChannel.EndpointAddress);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsApplicationIdProvider_FromDefault()
        {
            var applicationIdProvider = new ApplicationInsightsApplicationIdProvider();

            var configuration = new TelemetryConfiguration
            {
                ApplicationIdProvider = applicationIdProvider,
            };

            Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
            Assert.AreEqual("https://dc.services.visualstudio.com/api/profiles/{0}/appId", applicationIdProvider.ProfileQueryEndpoint);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsApplicationIdProvider_FromConnectionString()
        {
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000";

            var applicationIdProvider = new ApplicationInsightsApplicationIdProvider();

            var configuration = new TelemetryConfiguration
            {
                ApplicationIdProvider = applicationIdProvider,
                ConnectionString = connectionString,
            };

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
            Assert.AreEqual("https://dc.services.visualstudio.com/api/profiles/{0}/appId", applicationIdProvider.ProfileQueryEndpoint);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsApplicationIdProvider_FromConnectionString_Reverse()
        {
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000";

            var applicationIdProvider = new ApplicationInsightsApplicationIdProvider();

            var configuration = new TelemetryConfiguration
            {
                ConnectionString = connectionString,
                ApplicationIdProvider = applicationIdProvider,
            };

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
            Assert.AreEqual("https://dc.services.visualstudio.com/api/profiles/{0}/appId", applicationIdProvider.ProfileQueryEndpoint);
        }


        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_SetsDictionaryApplicationIdProvider_FromConnectionString()
        {
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000";

            var applicationIdProvider = new ApplicationInsightsApplicationIdProvider();
            var dictionaryApplicationIdProvider = new DictionaryApplicationIdProvider
            {
                Next = applicationIdProvider
            };

            var configuration = new TelemetryConfiguration
            {
                ConnectionString = connectionString,
            };

            configuration.ApplicationIdProvider = applicationIdProvider;

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
            Assert.AreEqual("https://dc.services.visualstudio.com/api/profiles/{0}/appId", applicationIdProvider.ProfileQueryEndpoint);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySetConnectionString_IgnoresDictionaryApplicationIdProvider()
        {
            var connectionString = $"InstrumentationKey=00000000-0000-0000-0000-000000000000";

            var applicationIdProvider = new DictionaryApplicationIdProvider();

            var configuration = new TelemetryConfiguration
            {
                ApplicationIdProvider = applicationIdProvider,
                ConnectionString = connectionString,
            };

            Assert.AreEqual("00000000-0000-0000-0000-000000000000", configuration.InstrumentationKey);
            Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);
        }

        #endregion

        #region TelemetryInitializers

        [TestMethod]
        public void TelemetryInitializersReturnsAnEmptyListByDefaultToAvoidNullReferenceExceptionsInUserCode()
        {
            var configuration = new TelemetryConfiguration();
            Assert.AreEqual(0, configuration.TelemetryInitializers.Count);
        }

        [TestMethod]
        public void TelemetryInitializersReturnsThreadSafeList()
        {
            var configuration = new TelemetryConfiguration();
            Assert.AreEqual(typeof(SnapshottingList<ITelemetryInitializer>), configuration.TelemetryInitializers.GetType());
        }

        #endregion

        #region TelemetryChannel

        [TestMethod]
        public void TelemetryChannelCanBeSetByUserToReplaceDefaultChannelForTesting()
        {
            var configuration = new TelemetryConfiguration();

            var customChannel = new StubTelemetryChannel();
            configuration.TelemetryChannel = customChannel;

            Assert.AreSame(customChannel, configuration.TelemetryChannel);
        }

        [TestMethod]
        public void CommonTelemetryChannelIsDefaultSinkTelemetryChannel()
        {
            var configuration = new TelemetryConfiguration();

            var c1 = new StubTelemetryChannel();
            var c2 = new StubTelemetryChannel();

            configuration.TelemetryChannel = c1;
            Assert.AreSame(c1, configuration.TelemetryChannel);

            configuration.DefaultTelemetrySink.TelemetryChannel = c2;
            Assert.AreSame(c2, configuration.TelemetryChannel);
        }

        #endregion

        #region TelemetryProcessor

        [TestMethod]
        public void TelemetryConfigurationAlwaysGetDefaultTransmissionProcessor()
        {
            var configuration = new TelemetryConfiguration();
            var tp = configuration.DefaultTelemetrySink.TelemetryProcessorChain;

            AssertEx.IsType<TransmissionProcessor>(tp.FirstTelemetryProcessor);
        }

        [TestMethod]
        public void TelemetryProcessorsCollectionIsReadOnly()
        {
            var configuration = new TelemetryConfiguration();
            
            AssertEx.IsType<ReadOnlyCollection<ITelemetryProcessor>>(configuration.TelemetryProcessors);
        }

        #endregion

        #region Serialized Configuration
        [TestMethod]
        public void TelemetryConfigThrowsIfSerializedConfigIsNull()
        {
            AssertEx.Throws<ArgumentNullException>(() =>
             {
                 TelemetryConfiguration.CreateFromConfiguration(null);
             });
        }

        [TestMethod]
        public void TelemetryConfigThrowsIfSerializedConfigIsEmpty()
        {
            AssertEx.Throws<ArgumentNullException>(() =>
            {
                TelemetryConfiguration.CreateFromConfiguration(String.Empty);
            });
        }

        [TestMethod]
        public void TelemetryConfigThrowsIfSerializedConfigIsWhitespace()
        {
            AssertEx.Throws<ArgumentNullException>(() =>
            {
                TelemetryConfiguration.CreateFromConfiguration(" ");
            });
        }
        #endregion

        #region Sampling Store
        [TestMethod]
        public void TelemetryConfigurationAllowsToManageLastKnownSampleRate()
        {
            var configuration = new TelemetryConfiguration();
            configuration.SetLastObservedSamplingPercentage(DataContracts.SamplingTelemetryItemTypes.Request, 10);
            Assert.AreEqual(configuration.GetLastObservedSamplingPercentage(DataContracts.SamplingTelemetryItemTypes.Request), 10);
        }
        #endregion

        private class TestableTelemetryModules : TelemetryModules
        {
        }

        private class StubTelemetryConfigurationFactory : TelemetryConfigurationFactory
        {
            public Action<TelemetryConfiguration, TelemetryModules> OnInitialize = (configuration, module) => { };

            public override void Initialize(TelemetryConfiguration configuration, TelemetryModules modules)
            {
                this.OnInitialize(configuration, modules);
            }
        }
    }
}

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Shared.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.TestFramework;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    

    using EventLevel = System.Diagnostics.Tracing.EventLevel;

    [TestClass]
    public class TelemetryConfigurationFactoryTest
    {
        private const string EnvironmentVariableName = "APPINSIGHTS_INSTRUMENTATIONKEY";
        private const string EnvironmentVariableConnectionString = "APPLICATIONINSIGHTS_CONNECTION_STRING";

        [TestCleanup]
        public void TestCleanup()
        {
            Environment.SetEnvironmentVariable(EnvironmentVariableName, null);
            Environment.SetEnvironmentVariable(EnvironmentVariableConnectionString, null);
            PlatformSingleton.Current = null; // Force reinitialization in future tests so that new environment variables will be loaded.
        }

        #region Instance

        [TestMethod]
        public void ClassIsInternalAndNotMeantForPublicConsumption()
        {
            Assert.IsFalse(typeof(TelemetryConfigurationFactory).GetTypeInfo().IsPublic);
        }

        [TestMethod]
        public void InstanceReturnsDefaultTelemetryConfigurationFactoryInstanceUsedByTelemetryConfiguration()
        {
            Assert.IsNotNull(TelemetryConfigurationFactory.Instance);
        }

        [TestMethod]
        public void InstanceCanGeSetByTestsToIsolateTestingOfTelemetryConfigurationFromRealFactoryLogic()
        {
            var replacement = new TestableTelemetryConfigurationFactory();
            TelemetryConfigurationFactory.Instance = replacement;
            Assert.AreSame(replacement, TelemetryConfigurationFactory.Instance);
        }

        [TestMethod]
        public void InstanceIsLazilyInitializedToSimplifyResettingOfGlobalStateInTests()
        {
            TelemetryConfigurationFactory.Instance = null;
            Assert.IsNotNull(TelemetryConfigurationFactory.Instance);
        }

        #endregion

        #region Initialize

        [TestMethod]
        public void InitializeCreatesInMemoryChannel()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null);

            AssertEx.IsType<InMemoryChannel>(configuration.TelemetryChannel);
        }
        
        [TestMethod]
        public void InitializesInstanceWithEmptyInstrumentationKey()
        {
            string configFileContents = Configuration("<InstrumentationKey></InstrumentationKey>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                // Assume that LoadFromXml method is called, tested separately
                Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
            }
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void InitializesInstanceWithEmptyConnectionString()
        {
            string configFileContents = Configuration($"<ConnectionString></ConnectionString>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                // Assume that LoadFromXml method is called, tested separately
                Assert.AreEqual(null, configuration.ConnectionString);
                Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
            }
        }

        [TestMethod]
        public void InitializesInstanceWithInformationFromConfigurationFileWhenItExists()
        {
            string configFileContents = Configuration("<InstrumentationKey>F8474271-D231-45B6-8DD4-D344C309AE69</InstrumentationKey>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                // Assume that LoadFromXml method is called, tested separately
                Assert.IsFalse(string.IsNullOrEmpty(configuration.InstrumentationKey));
            }
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifyChannelEndpointsAreSetWhenParsingFromConfigFile_InMemoryChannel()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            // PART 1 - CONFIGURATION FACTORY IS EXPECTED TO CREATE A CONFIG THAT MATCHES THE XML
            string ikeyConfig = "00000000-0000-0000-1111-000000000000";
            string ikeyConfigConnectionString = "00000000-0000-0000-2222-000000000000";

            string configString = @"<InstrumentationKey>00000000-0000-0000-1111-000000000000</InstrumentationKey>
  <TelemetryChannel Type=""Microsoft.ApplicationInsights.Channel.InMemoryChannel, Microsoft.ApplicationInsights"">
    <EndpointAddress>http://10.0.0.0/v2/track</EndpointAddress>
    <DeveloperMode>true</DeveloperMode>
  </TelemetryChannel>";

            string configFileContents = Configuration(configString);
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(ikeyConfig, configuration.InstrumentationKey);
            Assert.AreEqual(true, configuration.TelemetryChannel.DeveloperMode);
            Assert.AreEqual("http://10.0.0.0/v2/track", configuration.TelemetryChannel.EndpointAddress, "failed to set Channel Endpoint to config value");

            // PART 2 - VERIFY SETTING THE CONNECTION STRING WILL OVERWRITE CHANNEL ENDPOINT.
            TelemetryConfiguration.Active = configuration;

            TelemetryConfiguration.Active.ConnectionString = $"InstrumentationKey={ikeyConfigConnectionString};IngestionEndpoint=https://localhost:63029/";

            var client = new TelemetryClient();

            Assert.AreEqual(string.Empty, client.InstrumentationKey);
            Assert.AreEqual(ikeyConfigConnectionString, client.TelemetryConfiguration.InstrumentationKey);
            Assert.AreEqual("https://localhost:63029/v2/track", client.TelemetryConfiguration.TelemetryChannel.EndpointAddress);
#pragma warning restore CS0618 // Type or member is obsolete
        }


        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifyChannelEndpointsAreSetWhenParsingFromConfigFile_ServerTelemetryChannel()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            // PART 1 - CONFIGURATION FACTORY IS EXPECTED TO CREATE A CONFIG THAT MATCHES THE XML
            string ikeyConfig = "00000000-0000-0000-1111-000000000000";
            string ikeyConfigConnectionString = "00000000-0000-0000-2222-000000000000";

            string configString = @"<InstrumentationKey>00000000-0000-0000-1111-000000000000</InstrumentationKey>
  <TelemetryChannel Type=""Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.ServerTelemetryChannel, Microsoft.AI.ServerTelemetryChannel"">
    <EndpointAddress>http://10.0.0.0/v2/track</EndpointAddress>
  </TelemetryChannel>";

            string configFileContents = Configuration(configString);
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(ikeyConfig, configuration.InstrumentationKey);
            Assert.AreEqual("http://10.0.0.0/v2/track", configuration.TelemetryChannel.EndpointAddress, "failed to set Channel Endpoint to config value");

            // PART 2 - VERIFY SETTING THE CONNECTION STRING WILL OVERWRITE CHANNEL ENDPOINT.
            TelemetryConfiguration.Active = configuration;

            TelemetryConfiguration.Active.ConnectionString = $"InstrumentationKey={ikeyConfigConnectionString};IngestionEndpoint=https://localhost:63029/";

            var client = new TelemetryClient();

            Assert.AreEqual(string.Empty, client.InstrumentationKey);
            Assert.AreEqual(ikeyConfigConnectionString, client.TelemetryConfiguration.InstrumentationKey);
            Assert.AreEqual("https://localhost:63029/v2/track", client.TelemetryConfiguration.TelemetryChannel.EndpointAddress);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifyThatChannelEndpointIsNotOverwrittenIfManuallyConfigured()
        {
            var configuration = new TelemetryConfiguration();
            Assert.AreEqual("https://dc.services.visualstudio.com/", configuration.EndpointContainer.Ingestion.AbsoluteUri);

            var customEndpoint = "http://10.0.0.0/v2/track";
            var customChannel = new InMemoryChannel
            {
                EndpointAddress = customEndpoint
            };

            Assert.AreEqual(customEndpoint, customChannel.EndpointAddress);

            configuration.TelemetryChannel = customChannel;

            Assert.AreEqual(customEndpoint, customChannel.EndpointAddress, "channel endpoint was overwritten by config");
        }


        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySelectInstrumentationKeyChooses_EnVarConnectionString()
        {
            // SETUP
            string ikeyEnVarConnectionString = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable(EnvironmentVariableConnectionString, $"InstrumentationKey={ikeyEnVarConnectionString}");

            string ikeyEnVar = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable(EnvironmentVariableName, ikeyEnVar);

            string ikeyConfig = "e6f55001-f7d1-4242-b9f4-83660d0487f9";
            string ikeyConfigConnectionString = "F8474271-D231-45B6-8DD4-D344C309AE69";

            string configFileContents = Configuration($"<InstrumentationKey>{ikeyConfig}</InstrumentationKey><ConnectionString>InstrumentationKey={ikeyConfigConnectionString}</ConnectionString>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyEnVarConnectionString, configuration.InstrumentationKey);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySelectInstrumentationKeyChooses_EnVar()
        {
            // SETUP
            string ikeyEnVar = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable(EnvironmentVariableName, ikeyEnVar);

            string ikeyConfig = "e6f55001-f7d1-4242-b9f4-83660d0487f9";
            string ikeyConfigConnectionString = "F8474271-D231-45B6-8DD4-D344C309AE69";

            string configFileContents = Configuration($"<InstrumentationKey>{ikeyConfig}</InstrumentationKey><ConnectionString>InstrumentationKey={ikeyConfigConnectionString}</ConnectionString>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyEnVar, configuration.InstrumentationKey);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySelectInstrumentationKeyChooses_ConfigConnectionString()
        {
            // SETUP
            string ikeyConfig = "e6f55001-f7d1-4242-b9f4-83660d0487f9";
            string ikeyConfigConnectionString = "F8474271-D231-45B6-8DD4-D344C309AE69";

            string configFileContents = Configuration($"<InstrumentationKey>{ikeyConfig}</InstrumentationKey><ConnectionString>InstrumentationKey={ikeyConfigConnectionString}</ConnectionString>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyConfigConnectionString, configuration.InstrumentationKey);
        }

        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySelectInstrumentationKeyChooses_ConfigConnectionString_Reverse()
        {
            // SETUP
            string ikeyConfig = "e6f55001-f7d1-4242-b9f4-83660d0487f9";
            string ikeyConfigConnectionString = "F8474271-D231-45B6-8DD4-D344C309AE69";

            string configFileContents = Configuration($"<ConnectionString>InstrumentationKey={ikeyConfigConnectionString}</ConnectionString><InstrumentationKey>{ikeyConfig}</InstrumentationKey>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyConfig, configuration.InstrumentationKey);
        }
        
        [TestMethod]
        [TestCategory("ConnectionString")]
        public void VerifySelectInstrumentationKeyChooses_ConfigIKey()
        {
            // SETUP
            string ikeyConfig = "e6f55001-f7d1-4242-b9f4-83660d0487f9";

            string configFileContents = Configuration($"<InstrumentationKey>{ikeyConfig}</InstrumentationKey>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // ASSERT
            Assert.AreEqual(ikeyConfig, configuration.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeAddsOperationContextTelemetryInitializerByDefault()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null);

            var contextInitializer = configuration.TelemetryInitializers[0];
            AssertEx.IsType<OperationCorrelationTelemetryInitializer>(contextInitializer);
        }

        [TestMethod]
        public void InitializeNotifiesTelemetryInitializersImplementingITelemetryModuleInterface()
        {
            var initializer = new StubConfigurableTelemetryInitializer();
            var configuration = new TelemetryConfiguration { TelemetryInitializers = { initializer } };

            new TestableTelemetryConfigurationFactory().Initialize(configuration, null);

            Assert.IsTrue(initializer.Initialized);
            Assert.AreSame(configuration, initializer.Configuration);
        }

        [TestMethod]
        public void InitializeCreatesInMemoryChannelEvenWhenConfigIsBroken()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, Configuration("</blah>"));

            AssertEx.IsType<InMemoryChannel>(configuration.TelemetryChannel);
        }

        [TestMethod]
        public void InitializeCreatesInMemoryChannelEvenWhenConfigIsInvalid()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, Configuration("<blah></blah>"));

            AssertEx.IsType<InMemoryChannel>(configuration.TelemetryChannel);
        }

        [TestMethod]
        public void InitializeReadsInstrumentationKeyFromEnvironmentVariableIfNotSpecifiedInConfig()
        {
            // ARRANGE
            string ikey = Guid.NewGuid().ToString();
            Environment.SetEnvironmentVariable(EnvironmentVariableName, ikey);
            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null);

            // ASSERT
            Assert.AreEqual(ikey, configuration.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeReadsInstrumentationKeyFromEnvironmentVariableEvenIfSpecifiedInConfig()
        {
            // ARRANGE
            string ikeyConfig = Guid.NewGuid().ToString();
            string ikeyEnvironmentVariable = Guid.NewGuid().ToString();

            Environment.SetEnvironmentVariable(EnvironmentVariableName, ikeyEnvironmentVariable);
            TelemetryConfiguration configuration = new TelemetryConfiguration(ikeyConfig);

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null);

            // ASSERT
            Assert.AreEqual(ikeyEnvironmentVariable, configuration.InstrumentationKey);
        }

        [TestMethod]
        public void InitializeLeavesInstrumentationKeyEmptyWhenNotSpecifiedInConfigOrInEnvironmentVariable()
        {
            // ARRANGE
            Environment.SetEnvironmentVariable(EnvironmentVariableName, null);

            TelemetryConfiguration configuration = new TelemetryConfiguration();

            // ACT
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null);

            // ASSERT
            Assert.AreEqual(string.Empty, configuration.InstrumentationKey);
        }

        #endregion

        #region CreateInstance

        [TestMethod]
        public void CreateInstanceReturnsInstanceOfTypeSpecifiedByTypeName()
        {
            Type type = typeof(StubTelemetryInitializer);
            object instance = TestableTelemetryConfigurationFactory.CreateInstance(typeof(ITelemetryInitializer), type.AssemblyQualifiedName);
            Assert.AreEqual(type, instance.GetType());
        }

        [TestMethod]
        public void CreateInstanceReturnsNullWhenTypeCannotBeFound()
        {
            Assert.IsNull(TestableTelemetryConfigurationFactory.CreateInstance(typeof(ITelemetryInitializer), "MissingType, MissingAssembly"));
        }

        [TestMethod]
        public void CreateInstanceThrowsInvalidOperationExceptionWhenTypeNameIsInvalidToHelpDeveloperIdentifyAndFixTheProblem()
        {
            Assert.IsNull(TestableTelemetryConfigurationFactory.CreateInstance(typeof(ITelemetryInitializer), "Invalid Type Name"));
        }

        [TestMethod]
        public void CreateInstanceReturnsNullWhenInstanceDoesNotImplementExpectedInterface()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            Type invalidType = typeof(object);
            Assert.IsNull(TestableTelemetryConfigurationFactory.CreateInstance(typeof(ITelemetryInitializer), invalidType.AssemblyQualifiedName));
        }

        #endregion

        #region LoadFromXml

        [TestMethod]
        public void LoadFromXmlInitializesGivenTelemetryConfigurationInstanceFromXml()
        {
            string expected = Guid.NewGuid().ToString();
            string profile = Configuration("<InstrumentationKey>" + expected + "</InstrumentationKey>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            TestableTelemetryConfigurationFactory.LoadFromXml(configuration, null, XDocument.Parse(profile));

            // Assume LoadFromXml calls LoadInstance, which is tested separately.
            Assert.AreEqual(expected, configuration.InstrumentationKey);
        }

        #endregion

        #region LoadInstance

        [TestMethod]
        public void LoadInstanceReturnsInstanceOfTypeSpecifiedInTypeAttributeOfGivenXmlDefinition()
        {
            var definition = new XElement("Definition", new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName));
            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);
            Assert.AreEqual(typeof(StubClassWithProperties), instance.GetType());
        }

        [TestMethod]
        public void LoadInstanceSetsInstancePropertiesFromChildElementValuesOfDefinition()
        {
            var definition = new XElement(
                "Definition",
                new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName),
                new XElement("StringProperty", "TestValue"));

            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);

            Assert.AreEqual("TestValue", ((StubClassWithProperties)instance).StringProperty);
        }

        [TestMethod]
        public void LoadInstanceSetsInstancePropertiesOfTimeSpanTypeFromChildElementValuesOfDefinitionWithTimeSpanFormat()
        {
            var definition = new XElement(
                "Definition",
                new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName),
                new XElement("TimeSpanProperty", "00:00:07"));

            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);

            Assert.AreEqual(TimeSpan.FromSeconds(7), ((StubClassWithProperties)instance).TimeSpanProperty);
        }

        [TestMethod]
        public void LoadInstanceSetsInstancePropertiesOfTimeSpanTypeFromChildElementValuesOfDefinitionWithOneInteger()
        {
            var definition = new XElement(
                "Definition",
                new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName),
                new XElement("TimeSpanProperty", "7"));

            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);

            Assert.AreEqual(TimeSpan.FromDays(7), ((StubClassWithProperties)instance).TimeSpanProperty);
        }

        [TestMethod]
#if NETCOREAPP
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Failed to parse configuration value. Property: 'TimeSpanProperty' Reason: String 'TestValue' was not recognized as a valid TimeSpan.")]
#else
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Failed to parse configuration value. Property: 'TimeSpanProperty' Reason: String was not recognized as a valid TimeSpan.")]
#endif
        public void LoadInstanceSetsInstancePropertiesOfTimeSpanTypeFromChildElementValuesOfDefinitionWithInvalidFormatThrowsException()
        {
            var definition = new XElement(
                "Definition",
                new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName),
                new XElement("TimeSpanProperty", "TestValue"));

            TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);
        }

        [TestMethod]
        public void LoadInstanceInitializesGivenInstanceAndDoesNotRequireSpecifyingTypeAttributeToSimplifyConfiguration()
        {
            var definition = new XElement(
                "Definition",
                new XElement("StringProperty", "TestValue"));

            var original = new StubClassWithProperties();
            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), original, null);

            Assert.AreEqual("TestValue", original.StringProperty);
        }

        [TestMethod]
        public void LoadInstanceHandlesEnumPropertiesWithNumericValue()
        {
            var definition = new XElement(
                "Definition",
                new XElement("EnumProperty", "3"));

            var original = new StubClassWithProperties();
            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), original, null);

            Assert.AreEqual(EventLevel.Warning, original.EnumProperty);
        }

        [TestMethod]
        public void LoadInstanceHandlesEnumPropertiesWithEnumerationValueName()
        {
            var definition = new XElement(
                "Definition",
                new XElement("EnumProperty", "Informational"));

            var original = new StubClassWithProperties();
            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), original, null);

            Assert.AreEqual(EventLevel.Informational, original.EnumProperty);
        }

        [TestMethod]
        public void LoadInstanceConvertsValueToExpectedTypeGivenXmlDefinitionWithNoChildElements()
        {
            var definition = new XElement("Definition", "42");
            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(int), null, null);
            Assert.AreEqual(42, instance);
        }
        
        [TestMethod]
        public void LoadInstanceConvertsValueToExpectedTypeGivenXmlDefinitionWithNoChildElementsParseHexValue()
        {
            var definition = new XElement("Definition", "0x42");
            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(int), null, null);
            Assert.AreEqual(66, instance);
        }

        [TestMethod]
        public void LoadInstanceTrimsValueOfGivenXmlElementToIgnoreWhitespaceUsersMayAddToConfiguration()
        {
            string expected = Guid.NewGuid().ToString();
            var definition = new XElement("InstrumentationKey", "\n" + expected + "\n");

            object actual = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(string), null, null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LoadInstanceReturnsNullGivenEmptyXmlElementForReferenceType()
        {
            var definition = new XElement("Definition");
            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(string), "Test Value", null);
            Assert.IsNull(instance);
        }

        [TestMethod]
        public void LoadInstanceReturnsOriginalValueGivenNullXmlElement()
        {
            var original = "Test Value";
            object loaded = TestableTelemetryConfigurationFactory.LoadInstance(null, original.GetType(), original, null);
            Assert.AreSame(original, loaded);
        }

        [TestMethod]
        public void LoadInstanceReturnsDefaultValueGivenValueEmptyXmlElementForValueType()
        {
            var definition = new XElement("Definition");
            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(int), 12, null);
            Assert.AreEqual(0, instance);
        }

        [TestMethod]
        public void LoadInstanceReturnsNullWhenDefinitionElementDoesNotHaveTypeAttributeAndInstanceIsNotInitialized()
        {
            var elementWithoutType = new XElement("Add", new XElement("PropertyName"));
            Assert.IsNull(TestableTelemetryConfigurationFactory.LoadInstance(elementWithoutType, typeof(IComparable), null, null));
        }

        [TestMethod]
        public void LoadInstanceReturnsNullWhenDefinitionElementContainsInvalidContentAndNoTypeAttribute()
        {
            var definition = new XElement("InvalidElement", "InvalidText");
            Assert.IsNull(TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(ITelemetryChannel), null, null));
        }

        [TestMethod]
        public void LoadInstanceCreatesNewInstanceOfExpectedTypeWhenTypeAttributeIsNotSpecified()
        {
            var definition = new XElement("Definition", new XElement("Int32Property", 42));

            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);

            var loaded = AssertEx.IsType<StubClassWithProperties>(instance);
            Assert.AreEqual(42, loaded.Int32Property);
        }

        [TestMethod]
        public void LoadInstanceCreatesNewInstanceOfExpectedTypeWhenPropertiesAreSpecifiedOnlyAsAttributes()
        {
            var definition = new XElement("Definition", new XAttribute("Int32Property", 42));

            object instance = TestableTelemetryConfigurationFactory.LoadInstance(definition, typeof(StubClassWithProperties), null, null);

            var loaded = AssertEx.IsType<StubClassWithProperties>(instance);
            Assert.AreEqual(42, loaded.Int32Property);
        }

#endregion

#region TelemetryProcesors

        [TestMethod]
        public void InitializeTelemetryProcessorsFromConfigurationFile()
        {
            string configFileContents = Configuration(
                "<TelemetryProcessors>" +
                  "<Add Type=\"" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + "\" />" +
                  "<Add Type=\"" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + "\" />" +
                  "</TelemetryProcessors>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                // Assume that LoadFromXml method is called, tested separately
                Assert.IsTrue(configuration.TelemetryProcessors != null);
                AssertEx.IsType<StubTelemetryProcessor>(configuration.TelemetryProcessorChain.FirstTelemetryProcessor);

                //validate the chain linking stub1->stub2->pass through->sink
                var tp1 = (StubTelemetryProcessor)configuration.TelemetryProcessorChain.FirstTelemetryProcessor;
                var tp2 = (StubTelemetryProcessor2)tp1.next;
                var passThroughProcessor = tp2.next as PassThroughProcessor;
                Assert.IsNotNull(passThroughProcessor);

                // The sink has only a transmission processor and a default channel.
                var sink = passThroughProcessor.Sink;
                Assert.IsNotNull(sink);
                Assert.AreEqual(1, sink.TelemetryProcessorChain.TelemetryProcessors.Count);
                AssertEx.IsType<TransmissionProcessor>(sink.TelemetryProcessorChain.FirstTelemetryProcessor);
                AssertEx.IsType<InMemoryChannel>(sink.TelemetryChannel);
            }
        }

        [TestMethod]
        public void InitializeTelemetryProcessorsWithWrongProcessorInTheMiddle()
        {
            string configFileContents = Configuration(
                "<TelemetryProcessors>" +
                  "<Add Type=\"" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + "\" />" +
                  "<Add Type = \"Invalid, Invalid\" />" +
                  "<Add Type=\"" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + "\" />" +
                  "</TelemetryProcessors>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            using (var testableTelemetryModules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(configuration, testableTelemetryModules, configFileContents);

                Assert.IsTrue(configuration.TelemetryProcessors != null);
                AssertEx.IsType<StubTelemetryProcessor>(configuration.TelemetryProcessorChain.FirstTelemetryProcessor);

                //validate the chain linking stub1->stub2->pass through->sink
                var tp1 = (StubTelemetryProcessor)configuration.TelemetryProcessorChain.FirstTelemetryProcessor;
                var tp2 = (StubTelemetryProcessor2)tp1.next;
                var passThroughProcessor = tp2.next as PassThroughProcessor;
                Assert.IsNotNull(passThroughProcessor);

                // The sink has only a transmission processor and a default channel.
                var sink = passThroughProcessor.Sink;
                Assert.IsNotNull(sink);
                Assert.AreEqual(1, sink.TelemetryProcessorChain.TelemetryProcessors.Count);
                AssertEx.IsType<TransmissionProcessor>(sink.TelemetryProcessorChain.FirstTelemetryProcessor);
                AssertEx.IsType<InMemoryChannel>(sink.TelemetryChannel);
            }
        }

        [TestMethod]
        public void InitializeTelemetryProcessorFromConfigurationFile()
        {
            string configFileContents = Configuration(
                @"<TelemetryProcessors>
                    <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                </TelemetryProcessors>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // Assume that LoadFromXml method is called, tested separately
            Assert.IsTrue(configuration.TelemetryProcessors != null);
            AssertEx.IsType<StubTelemetryProcessor>(configuration.TelemetryProcessorChain.FirstTelemetryProcessor);

            // The next telemetry processor should be the PassThroughProcessor that terminates the common telemetry processor chain and feeds into the sink.
            var stub1 = (StubTelemetryProcessor)configuration.TelemetryProcessorChain.FirstTelemetryProcessor;
            var passThroughProcessor = stub1.next as PassThroughProcessor;
            Assert.IsNotNull(passThroughProcessor);

            // The sink has only a transmission processor and a default channel.
            var sink = passThroughProcessor.Sink;
            Assert.IsNotNull(sink);
            Assert.AreEqual(1, sink.TelemetryProcessorChain.TelemetryProcessors.Count);
            AssertEx.IsType<TransmissionProcessor>(sink.TelemetryProcessorChain.FirstTelemetryProcessor);
            AssertEx.IsType<InMemoryChannel>(sink.TelemetryChannel);
        }

        [TestMethod]
        public void InitializeInvokedWhenTelemetryProcessorAlsoImplementsITelemetryModule()
        {
            string configFileContents = Configuration(
                "<TelemetryProcessors>" +
                  "<Add Type=\"" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + "\" />" +
                  "</TelemetryProcessors>");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.IsTrue(configuration.TelemetryProcessors != null);
            AssertEx.IsType<StubTelemetryProcessor2>(configuration.TelemetryProcessorChain.FirstTelemetryProcessor);
            Assert.IsTrue(((StubTelemetryProcessor2)configuration.TelemetryProcessorChain.FirstTelemetryProcessor).Initialized);
        }

        [TestMethod]
        public void InitializeTelemetryProcessorFromConfigurationFileWhenNoTelemetryProcessorsTagSpecified()
        {
            // no TelemetryProcessors - TransmissionProcessor should be automatically created.
            string configFileContents = Configuration(
                @"                  
                  <!--<TelemetryProcessors>
                  <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                  <Add Type=""" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                  </TelemetryProcessors>-->"
                );

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // Assume that LoadFromXml method is called, tested separately
            Assert.IsTrue(configuration.TelemetryProcessors != null);
            var passThroughProcessor = configuration.TelemetryProcessorChain.FirstTelemetryProcessor as PassThroughProcessor;
            Assert.IsNotNull(passThroughProcessor);

            // The sink has only a transmission processor and a default channel.
            var sink = passThroughProcessor.Sink;
            Assert.IsNotNull(sink);
            Assert.AreEqual(1, sink.TelemetryProcessorChain.TelemetryProcessors.Count);
            AssertEx.IsType<TransmissionProcessor>(sink.TelemetryProcessorChain.FirstTelemetryProcessor);
            AssertEx.IsType<InMemoryChannel>(sink.TelemetryChannel);
        }

        [TestMethod]
        public void InitializeTelemetryProcessorFromConfigurationFileWhenEmptyTelemetryProcessorsTagSpecified()
        {
            // no TelemetryProcessors - TransmissionProcessor should be automatically created.
            string configFileContents = Configuration(
                @"
                  <TelemetryInitializers>
                    <Add Type=""" + typeof(StubTelemetryInitializer).AssemblyQualifiedName + @""" />
                  </TelemetryInitializers>
                  <TelemetryProcessors>                  
                  </TelemetryProcessors>"
                );

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // Assume that LoadFromXml method is called, tested separately
            Assert.IsTrue(configuration.TelemetryProcessors != null);
            var passThroughProcessor = configuration.TelemetryProcessorChain.FirstTelemetryProcessor as PassThroughProcessor;
            Assert.IsNotNull(passThroughProcessor);

            // The sink has only a transmission processor and a default channel.
            var sink = passThroughProcessor.Sink;
            Assert.IsNotNull(sink);
            Assert.AreEqual(1, sink.TelemetryProcessorChain.TelemetryProcessors.Count);
            AssertEx.IsType<TransmissionProcessor>(sink.TelemetryProcessorChain.FirstTelemetryProcessor);
            AssertEx.IsType<InMemoryChannel>(sink.TelemetryChannel);
        }

        [TestMethod]
        public void RebuildDoesNotRemoveTelemetryProcessorsLoadedFromConfiguration()
        {
            string configFileContents = Configuration(
                @"                  
                  <TelemetryProcessors>
                    <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />                  
                  </TelemetryProcessors>"
                );

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            configuration.TelemetryProcessorChainBuilder.Build();

            Assert.AreEqual(2, configuration.TelemetryProcessors.Count);
            AssertEx.IsType<StubTelemetryProcessor>(configuration.TelemetryProcessors[0]);
        }

        [TestMethod]
        public void UseDoesNotRemoveTelemetryProcessorsLoadedFromConfiguration()
        {
            string configFileContents = Configuration(
                @"                  
                  <TelemetryProcessors>
                    <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />                  
                  </TelemetryProcessors>"
                );

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            var builder = configuration.TelemetryProcessorChainBuilder;
            builder.Use(_ => new StubTelemetryProcessor2(_));
            builder.Build();

            Assert.AreEqual(3, configuration.TelemetryProcessors.Count);
            AssertEx.IsType<StubTelemetryProcessor>(configuration.TelemetryProcessors[0]);
        }

        [TestMethod]
        public void UseAddsProcessorAfterProcessorsDefinedInConfiguration()
        {
            string configFileContents = Configuration(
                @"                  
                  <TelemetryProcessors>
                    <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />                  
                  </TelemetryProcessors>"
                );

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            var builder = configuration.TelemetryProcessorChainBuilder;
            builder.Use(_ => new StubTelemetryProcessor2(_));
            builder.Build();

            Assert.AreEqual(3, configuration.TelemetryProcessors.Count);
            AssertEx.IsType<StubTelemetryProcessor2>(configuration.TelemetryProcessors[1]);
        }

#endregion

#region Modules

        [TestMethod]
        public void InitializeTelemetryModulesFromConfigurationFile()
        {
            string configFileContents = Configuration(
                @"<TelemetryModules>
                    <Add Type = """ + typeof(StubConfigurable).AssemblyQualifiedName + @"""  />
                  </TelemetryModules>"
                );

            using (var modules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(new TelemetryConfiguration(), modules, configFileContents);

                Assert.AreEqual(2, modules.Modules.Count); // Diagnostics module is added by default
            }
        }

        [TestMethod]
        public void InitializeTelemetryModulesFromConfigurationFileWithNoModulesHasOneDiagnosticsModuleByDefault()
        {
            string configFileContents = Configuration(
                @"<TelemetryModules>    
                  </TelemetryModules>"
                );

            using (var modules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(new TelemetryConfiguration(), modules, configFileContents);


                // Initialize a 2nd TelemetryConfiguration to check that only one diagnostics module is added.
                new TestableTelemetryConfigurationFactory().Initialize(new TelemetryConfiguration(), modules, configFileContents);

                Assert.AreEqual(1, modules.Modules.Count);
                AssertEx.IsType<DiagnosticsTelemetryModule>(modules.Modules[0]);
            }
        }


        [TestMethod]
        public void InitializeTelemetryModulesFromConfigurationFileWhenOneModuleCannotBeLoaded()
        {
            string configFileContents = Configuration(
                @"<TelemetryModules>
                    <Add Type = """ + typeof(StubConfigurable).AssemblyQualifiedName + @"""  />
                    <Add Type = ""Invalid, Invalid"" />
                    <Add Type = """ + typeof(StubConfigurableTelemetryInitializer).AssemblyQualifiedName + @"""  />
                  </TelemetryModules>"
                );

            using (var modules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(new TelemetryConfiguration(), modules, configFileContents);

                Assert.AreEqual(3, modules.Modules.Count); // Diagnostics module is added by default
            }
        }

        [TestMethod]
        public void InitializeDoesNotThrowIsModuleInitializationFails()
        {
            string configFileContents = Configuration(
                @"<TelemetryModules>
                    <Add Type = """ + typeof(StubConfigurableWithProperties).AssemblyQualifiedName + @"""  />
                  </TelemetryModules>"
                );

            var module = new StubConfigurableWithProperties(null)
            {
                OnInitialize = _ => { throw new ArgumentException(); }
            };

            using (var modules = new TestableTelemetryModules())
            {
                modules.Modules.Add(module);

                //Assert.DoesNotThrow
                new TestableTelemetryConfigurationFactory().Initialize(
                    new TelemetryConfiguration(),
                    modules,
                    configFileContents);
            }
        }

#endregion

#region TelemetryInitializers
        [TestMethod]
        public void InitializeAddTelemetryInitializersWithOneInvalid()
        {
            string configFileContents = Configuration(
                @"<TelemetryInitializers>
                    <Add Type=""Invalid, Invalid"" />
                    <Add Type=""" + typeof(StubTelemetryInitializer).AssemblyQualifiedName + @""" />
                  </TelemetryInitializers>"
                );

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(2, configuration.TelemetryInitializers.Count); // Time and operation initializers are added by default
            Assert.IsNotNull(configuration.TelemetryInitializers.First(item => item.GetType().Name == "StubTelemetryInitializer"));
        }


#endregion

#region LoadInstances<T>

        [TestMethod]
        public void LoadInstancesPopulatesListWithInstancesOfSpecifiedType()
        {
            var element = XElement.Parse(@"
                <List xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                    <Add Type=""" + typeof(StubTelemetryInitializer).AssemblyQualifiedName + @""" />           
                </List>");
            var instances = new List<ITelemetryInitializer>();

            TestableTelemetryConfigurationFactory.LoadInstances(element, instances, null);

            Assert.AreEqual(1, instances.Count);
            Assert.AreEqual(typeof(StubTelemetryInitializer), instances[0].GetType());
        }

        [TestMethod]
        public void LoadInstancesUpdatesInstanceWithMatchingType()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            var element = XElement.Parse(@"
                <List xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                    <Add Type=""" + typeof(StubConfigurableWithProperties).AssemblyQualifiedName + @""" > 
                        <Int32Property>77</Int32Property>
                    </Add>
                </List>");

            var configurableElement = new StubConfigurableWithProperties(configuration);
            var instances = new List<object>();
            instances.Add(configurableElement);

            TestableTelemetryConfigurationFactory.LoadInstances(element, instances, null);

            var telemetryModules = instances.OfType<StubConfigurableWithProperties>().ToArray();
            Assert.AreEqual(1, telemetryModules.Count());
            Assert.AreEqual(configurableElement, telemetryModules[0]);
            Assert.AreEqual(77, configurableElement.Int32Property);
        }

        [TestMethod]
        public void LoadInstancesPopulatesListWithPrimitiveValues()
        {
            var definition = XElement.Parse(@"
                <List xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                    <Add>41</Add>
                    <Add>42</Add>
                </List>");

            var instances = new List<int>();
            TestableTelemetryConfigurationFactory.LoadInstances(definition, instances, null);

            AssertEx.AreEqual(new[] { 41, 42 }, instances);
        }

        [TestMethod]
        public void LoadInstancesIgnoresElementsOtherThanAdd()
        {
            var definition = XElement.Parse(@"
                <List xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
                    <Unknown/>
                    <Add>42</Add>
                </List>");

            var instances = new List<int>();
            //Assert.
            TestableTelemetryConfigurationFactory.LoadInstances(definition, instances, null);

            AssertEx.AreEqual(new[] { 42 }, instances);
        }

#endregion

#region LoadProperties

        [TestMethod]
        public void LoadPropertiesConvertsPropertyValuesFromStringToPropertyType()
        {
            var definition = new XElement("Definition", new XElement("Int32Property", "42"));

            var instance = new StubClassWithProperties();
            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);

            Assert.AreEqual(42, instance.Int32Property);
        }

        [TestMethod]
        public void LoadPropertiesReturnsNullWhenInstanceDoesNotHavePropertyWithSpecifiedName()
        {
            var definition = new XElement("Definition", new XElement("InvalidProperty", "AnyValue"));
            //Assert.DoesNotThrow
            TestableTelemetryConfigurationFactory.LoadProperties(definition, new StubClassWithProperties(), null);
        }

        [TestMethod]
        public void LoadPropertiesIgnoresUnknownTelemetryConfigurationPropertiesToAllowStatusMonitorDefineItsOwnSections()
        {
            string configuration = Configuration("<UnknownSection/>");
            XElement aplicationInsightsElement = XDocument.Parse(configuration).Root;
            //Assert.DoesNotThrow
            TestableTelemetryConfigurationFactory.LoadProperties(aplicationInsightsElement, new TelemetryConfiguration(), null);
        }

        [TestMethod]
        public void LoadPropertiesInstantiatesObjectOfTypeSpecifiedInTypeAttribute()
        {
            var definition = new XElement("Definition", new XElement("ChildProperty", new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName)));
            var instance = new StubClassWithProperties();

            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);

            Assert.AreEqual(typeof(StubClassWithProperties), instance.ChildProperty.GetType());
        }

        [TestMethod]
        public void LoadPropertiesRecursivelyLoadsInstanceSpecifiedByTypeAttribute()
        {
            var definition = new XElement(
                "Definition",
                new XElement(
                    "ChildProperty",
                    new XAttribute("Type", typeof(StubClassWithProperties).AssemblyQualifiedName),
                    new XElement("StringProperty", "TestValue")));
            var instance = new StubClassWithProperties();

            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);

            Assert.AreEqual("TestValue", instance.ChildProperty.StringProperty);
        }

        [TestMethod]
        public void LoadPropertiesDoesNotAttemptToSetReadOnlyProperty()
        {
            XElement definition = XDocument.Parse(Configuration(@"<TelemetryModules/>")).Root;
            var instance = new TelemetryConfiguration();
            //Assert.DoesNotThrow
            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);
        }

        [TestMethod]
        public void LoadPropertiesLoadsPropertiesFromAttributes()
        {
            var definition = new XElement("Definition", new XAttribute("Int32Property", "42"));

            var instance = new StubClassWithProperties();
            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);

            Assert.AreEqual(42, instance.Int32Property);
        }

        [TestMethod]
        public void LoadPropertiesGivesPrecedenceToValuesFromElementsBecauseTheyAppearBelowAttributes()
        {
            var definition = new XElement("Definition", new XAttribute("Int32Property", "41"), new XElement("Int32Property", "42"));

            var instance = new StubClassWithProperties();
            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);

            Assert.AreEqual(42, instance.Int32Property);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Failed to parse configuration value. Property: 'IntegerProperty' Reason: Input string was not in a correct format.")]
        public void LoadPropertiesThrowsExceptionWithPropertyName()
        {
            // parsing this integer will throw "System.FormatException: Input string was not in a correct format."
            // This is not useful without also specifying the errant property name.

            XElement definition = XDocument.Parse(Configuration(
                @"<TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""">
                    <IntegerProperty>123a</IntegerProperty>
                 </TelemetryChannel>")).Root;

            var instance = new TelemetryConfiguration();

            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Failed to parse configuration value. Property: 'IntegerProperty' Reason: Input string was not in a correct format.")]
        public void LoadProperties_TelemetryClientThrowsException()
        {
            string testConfig = Configuration(
                @"<TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""">
                    <IntegerProperty>123a</IntegerProperty>
                 </TelemetryChannel>");

            new TelemetryClient(TelemetryConfiguration.CreateFromConfiguration(testConfig));
        }

        [TestMethod]
        public void LoadPropertiesIgnoresNamespaceDeclarationWhenLoadingFromAttributes()
        {
            var definition = new XElement("Definition", new XAttribute("xmlns", "http://somenamespace"));

            var instance = new StubClassWithProperties();

            //Assert.DoesNotThrow
            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);
        }

        [TestMethod]
        public void DeveloperModePropertyCanLoadTrueValue()
        {
            TelemetryConfiguration instance = CreateTelemetryConfigurationWithDeveloperModeValue("true");
            Assert.IsTrue(instance.TelemetryChannel.DeveloperMode.HasValue);
            Assert.IsTrue(instance.TelemetryChannel.DeveloperMode.Value);
        }

        [TestMethod]
        public void DeveloperModePropertyCanLoadFalseValue()
        {
            TelemetryConfiguration instance = CreateTelemetryConfigurationWithDeveloperModeValue("false");
            Assert.IsTrue(instance.TelemetryChannel.DeveloperMode.HasValue);
            Assert.IsFalse(instance.TelemetryChannel.DeveloperMode.Value);
        }

        [TestMethod]
        public void DeveloperModePropertyCanLoadNullValue()
        {
            TelemetryConfiguration instance = CreateTelemetryConfigurationWithDeveloperModeValue("null");
            Assert.IsFalse(instance.TelemetryChannel.DeveloperMode.HasValue);
        }

        [TestMethod]
        public void DeveloperModePropertyCanLoadEmptyValue()
        {
            TelemetryConfiguration instance = CreateTelemetryConfigurationWithDeveloperModeValue(string.Empty);
            Assert.IsFalse(instance.TelemetryChannel.DeveloperMode.HasValue);
        }

#endregion

#region TelemetrySinks

        [TestMethod]
        public void EmptyConfigurationCreatesDefaultSink()
        {
            string configFileContents = Configuration(string.Empty);

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is PassThroughProcessor);

            // The sink has just a transmission processor feeding into InMemoryChannel.
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);

            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void NoSinkConfigurationWithCustomChannel()
        {
            string configFileContents = Configuration(@"
                <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
            ");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is PassThroughProcessor);

            // The sink has just a transmission processor feeding into the custom channel.
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);

            Assert.IsTrue(defaultSink.TelemetryChannel is StubTelemetryChannel);
        }

        [TestMethod]
        public void NoSinkConfigurationWithCustomProcessor()
        {
            string configFileContents = Configuration(@"
                <TelemetryProcessors>
                    <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                </TelemetryProcessors>
            ");

            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has the custom procesor and a PassThroughProcessor, feeding into the sink.
            Assert.AreEqual(2, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(configuration.TelemetryProcessors[1] is PassThroughProcessor);

            // The sink has just a transmission processor feeding into the custom channel.
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);

            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void EmptyDefaultSink()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""default"" />
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is PassThroughProcessor);

            // The sink has just a transmission processor feeding into InMemoryChannel.
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);

            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void DefaultSinkWithCustomProcessors()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""default"">
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                            <Add Type=""" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is PassThroughProcessor);

            // The sink has two processors feeding into InMemoryChannel (plus the transmission processor).
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(3, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(sinkTelemetryProcessors[1] is StubTelemetryProcessor2);
            Assert.IsTrue(sinkTelemetryProcessors[2] is TransmissionProcessor);

            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void DefaultSinkWithCustomChannel()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""default"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is PassThroughProcessor);

            // The sink has just the transmission processor feeding into custom channel.
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);

            Assert.IsTrue(defaultSink.TelemetryChannel is StubTelemetryChannel);
        }

        [TestMethod]
        public void CommonProcessorsAndDefaultSinkProcessors()
        {
            string configFileContents = Configuration(@"
                <TelemetryProcessors>
                    <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                </TelemetryProcessors>

                <TelemetrySinks>
                    <Add Name=""default"">
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has the custom procesor and a PassThroughProcessor, feeding into the sink.
            Assert.AreEqual(2, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(configuration.TelemetryProcessors[1] is PassThroughProcessor);

            // The sink has one custom processor feeding into InMemoryChannel (plus the transmission processor).
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is StubTelemetryProcessor2);
            Assert.IsTrue(sinkTelemetryProcessors[1] is TransmissionProcessor);

            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void DefaultSinkChannelWinsOverCommonChannel()
        {
            // This is not really a useful, or supported, configuration, but we just want to verify that if the channel appears both at the common level,
            // and at default sink level, the setting at the sink level wins and is applies successfully.

            string configFileContents = Configuration(@"
                <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />

                <TelemetrySinks>
                    <Add Name=""default"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel2).AssemblyQualifiedName + @""" />
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(1, configuration.TelemetrySinks.Count);

            // Common telemetry processor chain has just one PassThroughProcessor.
            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is PassThroughProcessor);

            // The sink has just the transmission processor feeding into custom channel.
            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);

            // The sink configuration setting overrode the common level setting.
            Assert.IsTrue(defaultSink.TelemetryChannel is StubTelemetryChannel2);
        }

        [TestMethod]
        public void NonDefaultEmptySink()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""custom"" />
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(2, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);

            var customSink = configuration.TelemetrySinks[1];
            var customSinkTelemetryProcessors = customSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, customSinkTelemetryProcessors.Count);
            Assert.IsTrue(customSinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(customSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void NonDefaultSinkWithCustomChannel()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""custom"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(2, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);

            var customSink = configuration.TelemetrySinks[1];
            var customSinkTelemetryProcessors = customSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, customSinkTelemetryProcessors.Count);
            Assert.IsTrue(customSinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(customSink.TelemetryChannel is StubTelemetryChannel);
        }

        [TestMethod]
        public void NonDefaultSinkWithCustomProcessors()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""custom"">
                        <TelemetryProcessors>
                            <Add Type = """ + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(2, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);

            var customSink = configuration.TelemetrySinks[1];
            var customSinkTelemetryProcessors = customSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, customSinkTelemetryProcessors.Count);
            Assert.IsTrue(customSinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(customSinkTelemetryProcessors[1] is TransmissionProcessor);
            Assert.IsTrue(customSink.TelemetryChannel is InMemoryChannel);
        }


        [TestMethod]
        public void NonDefaultSinkWithCustomChannelAndProcessors()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""custom"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type = """ + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(2, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);

            var customSink = configuration.TelemetrySinks[1];
            var customSinkTelemetryProcessors = customSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, customSinkTelemetryProcessors.Count);
            Assert.IsTrue(customSinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(customSinkTelemetryProcessors[1] is TransmissionProcessor);
            Assert.IsTrue(customSink.TelemetryChannel is StubTelemetryChannel);
        }

        [TestMethod]
        public void DefaultAndNonDefaultSink()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""default"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add Name=""custom"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel2).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type = """ + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(2, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(sinkTelemetryProcessors[1] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is StubTelemetryChannel);

            var customSink = configuration.TelemetrySinks[1];
            var customSinkTelemetryProcessors = customSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, customSinkTelemetryProcessors.Count);
            Assert.IsTrue(customSinkTelemetryProcessors[0] is StubTelemetryProcessor2);
            Assert.IsTrue(customSinkTelemetryProcessors[1] is TransmissionProcessor);
            Assert.IsTrue(customSink.TelemetryChannel is StubTelemetryChannel2);
        }

        [TestMethod]
        public void MultipleCustomSinks()
        {
            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""alpha"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add Name=""bravo"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel2).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type = """ + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            Assert.AreEqual(3, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var sinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, sinkTelemetryProcessors.Count);
            Assert.IsTrue(sinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is InMemoryChannel);

            var alphaSink = configuration.TelemetrySinks[1];
            Assert.AreEqual("alpha", alphaSink.Name);
            var alphaSinkTelemetryProcessors = alphaSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, alphaSinkTelemetryProcessors.Count);
            Assert.IsTrue(alphaSinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(alphaSinkTelemetryProcessors[1] is TransmissionProcessor);
            Assert.IsTrue(alphaSink.TelemetryChannel is StubTelemetryChannel);

            var bravoSink = configuration.TelemetrySinks[2];
            Assert.AreEqual("bravo", bravoSink.Name);
            var bravoSinkTelemetryProcessors = bravoSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, bravoSinkTelemetryProcessors.Count);
            Assert.IsTrue(bravoSinkTelemetryProcessors[0] is StubTelemetryProcessor2);
            Assert.IsTrue(bravoSinkTelemetryProcessors[1] is TransmissionProcessor);
            Assert.IsTrue(bravoSink.TelemetryChannel is StubTelemetryChannel2);
        }

        [TestMethod]
        public void NamedSinkConfigurationIsMerged()
        {
            // This is not an example of a configuration that we will support, but we need to adopt _some_ behavior
            // when multiple sinks with the same name appear in configuration.
            // The currently implemented behavior is:
            //   1. Named sinks are created just once. Configuration is applied to them as it read from the XML doc.
            //      a. TelemetryChannel is replaced each time (if present).
            //      b. TelemetryProcessors are added.
            //   2. <Add> elements without a name are assumed to represent unique sinks--new sink is created for each of them.

            string configFileContents = Configuration(@"
                <TelemetrySinks>
                    <Add Name=""default"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add Name=""alpha"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add>
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>
                    <Add Name=""default"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel2).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type = """ + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                       </TelemetryProcessors>
                    </Add>
                    <Add Name=""alpha"">
                        <TelemetryChannel Type=""" + typeof(StubTelemetryChannel2).AssemblyQualifiedName + @""" />
                        <TelemetryProcessors>
                            <Add Type=""" + typeof(StubTelemetryProcessor2).AssemblyQualifiedName + @""" />
                        </TelemetryProcessors>
                    </Add>

                    <!-- Custom sink with all properties set to default values. -->
                    <Add />
                </TelemetrySinks>
            ");
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            new TestableTelemetryConfigurationFactory().Initialize(configuration, null, configFileContents);

            // Default sink, "alpha" sink and two unnamed sinks.
            Assert.AreEqual(4, configuration.TelemetrySinks.Count);

            Assert.AreEqual(1, configuration.TelemetryProcessors.Count);
            Assert.IsTrue(configuration.TelemetryProcessors[0] is BroadcastProcessor);

            var defaultSink = configuration.DefaultTelemetrySink;
            var defaultSinkTelemetryProcessors = defaultSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(3, defaultSinkTelemetryProcessors.Count);
            Assert.IsTrue(defaultSinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(defaultSinkTelemetryProcessors[1] is StubTelemetryProcessor2);
            Assert.IsTrue(defaultSinkTelemetryProcessors[2] is TransmissionProcessor);
            Assert.IsTrue(defaultSink.TelemetryChannel is StubTelemetryChannel2);

            var alphaSink = configuration.TelemetrySinks[1];
            Assert.AreEqual("alpha", alphaSink.Name);
            var alphaSinkTelemetryProcessors = alphaSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(3, alphaSinkTelemetryProcessors.Count);
            Assert.IsTrue(alphaSinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(alphaSinkTelemetryProcessors[1] is StubTelemetryProcessor2);
            Assert.IsTrue(alphaSinkTelemetryProcessors[2] is TransmissionProcessor);
            Assert.IsTrue(alphaSink.TelemetryChannel is StubTelemetryChannel2);

            var firstUnnamedSink = configuration.TelemetrySinks[2];
            Assert.IsTrue(string.IsNullOrEmpty(firstUnnamedSink.Name));
            var firstUnnamedSinkTelemetryProcessors = firstUnnamedSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(2, firstUnnamedSinkTelemetryProcessors.Count);
            Assert.IsTrue(firstUnnamedSinkTelemetryProcessors[0] is StubTelemetryProcessor);
            Assert.IsTrue(firstUnnamedSinkTelemetryProcessors[1] is TransmissionProcessor);
            Assert.IsTrue(firstUnnamedSink.TelemetryChannel is StubTelemetryChannel);

            var secondUnnamedSink = configuration.TelemetrySinks[3];
            Assert.IsTrue(string.IsNullOrEmpty(secondUnnamedSink.Name));
            var secondUnnamedSinkTelemetryProcessors = secondUnnamedSink.TelemetryProcessorChain.TelemetryProcessors;
            Assert.AreEqual(1, secondUnnamedSinkTelemetryProcessors.Count);
            Assert.IsTrue(secondUnnamedSinkTelemetryProcessors[0] is TransmissionProcessor);
            Assert.IsTrue(secondUnnamedSink.TelemetryChannel is InMemoryChannel);
        }

        [TestMethod]
        public void TelemetrySinkInitializesChannelAndAllProcessors()
        {
            TelemetryConfiguration configuration = new TelemetryConfiguration();
            TelemetrySink sink = new TelemetrySink(configuration);
            var channel = new StubTelemetryChannel2();
            sink.TelemetryChannel = channel;
            StubTelemetryProcessor2 processor = null;
            sink.TelemetryProcessorChainBuilder.Use(next =>
            {
                processor = new StubTelemetryProcessor2(next);
                return processor;
            });
            sink.Initialize(configuration);

            Assert.IsTrue(channel.Initialized);
            Assert.IsTrue(processor.Initialized);
        }

#endregion

        [TestMethod]
        public void InitializeIsMarkesAsInternalSdkOperation()
        {
            bool isInternalOperation = false;

            StubConfigurableWithStaticCallback.OnInitialize = (item) => { isInternalOperation = SdkInternalOperationsMonitor.IsEntered(); };

            Assert.AreEqual(false, SdkInternalOperationsMonitor.IsEntered());
            string configFileContents = Configuration(
                @"<TelemetryModules>
                    <Add Type = """ + typeof(StubConfigurableWithStaticCallback).AssemblyQualifiedName + @"""  />
                  </TelemetryModules>"
                );

            using (var modules = new TestableTelemetryModules())
            {
                new TestableTelemetryConfigurationFactory().Initialize(new TelemetryConfiguration(), modules, configFileContents);

                Assert.AreEqual(true, isInternalOperation);
                Assert.AreEqual(false, SdkInternalOperationsMonitor.IsEntered());
            }
        }

        private static TelemetryConfiguration CreateTelemetryConfigurationWithDeveloperModeValue(string developerModeValue)
        {
            XElement definition = XDocument.Parse(Configuration(
                @"<TelemetryChannel Type=""" + typeof(StubTelemetryChannel).AssemblyQualifiedName + @""">
                    <DeveloperMode>" + developerModeValue + @"</DeveloperMode>
                 </TelemetryChannel>")).Root;

            var instance = new TelemetryConfiguration();
            //Assert.DoesNotThrow
            TestableTelemetryConfigurationFactory.LoadProperties(definition, instance, null);
            return instance;
        }

        private static string Configuration(string innerXml)
        {
            return
              @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                <ApplicationInsights xmlns=""http://schemas.microsoft.com/ApplicationInsights/2013/Settings"">
" + innerXml + @"
                </ApplicationInsights>";
        }

        private class TestableTelemetryModules : TelemetryModules, IDisposable
        {
            public void Dispose()
            {
                foreach (var module in this.Modules)
                {
                    (module as IDisposable)?.Dispose();
                }
            }
        }

        private class TestableTelemetryConfigurationFactory : TelemetryConfigurationFactory
        {
            public static object CreateInstance(Type interfaceType, string typeName)
            {
                return TelemetryConfigurationFactory.CreateInstance(interfaceType, typeName);
            }

            public static new void LoadFromXml(TelemetryConfiguration configuration, TelemetryModules modules, XDocument xml)
            {
                TelemetryConfigurationFactory.LoadFromXml(configuration, modules, xml);
            }

            public static object LoadInstance(XElement definition, Type expectedType, object instance, TelemetryModules modules)
            {
                return TelemetryConfigurationFactory.LoadInstance(definition, expectedType, instance, null, modules);
            }

            [SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods", Justification = "This method allows calling protected base method in this test class.")]
            public static new void LoadInstances<T>(XElement definition, ICollection<T> instances, TelemetryModules modules)
            {
                TelemetryConfigurationFactory.LoadInstances(definition, instances, modules);
            }

            public static new void LoadProperties(XElement definition, object instance, TelemetryModules modules)
            {
                TelemetryConfigurationFactory.LoadProperties(definition, instance, modules);
            }
        }

        private class StubClassWithProperties
        {
            public int Int32Property { get; set; }

            public string StringProperty { get; set; }

            public TimeSpan TimeSpanProperty { get; set; }

            public StubClassWithProperties ChildProperty { get; set; }

            public EventLevel EnumProperty { get; set; }
        }

        private class StubConfigurable : ITelemetryModule
        {
            public TelemetryConfiguration Configuration { get; set; }

            public bool Initialized { get; set; }

            public void Initialize(TelemetryConfiguration configuration)
            {
                this.Configuration = configuration;
                this.Initialized = true;
            }
        }

        private class StubConfigurableWithStaticCallback : ITelemetryModule
        {
            /// <summary>
            /// Gets or sets the callback invoked by the <see cref="Initialize"/> method.
            /// </summary>
            public static Action<TelemetryConfiguration> OnInitialize = item => { };

            public void Initialize(TelemetryConfiguration configuration)
            {
                OnInitialize(configuration);
            }
        }

        private class StubConfigurableTelemetryInitializer : StubConfigurable, ITelemetryInitializer
        {
            public void Initialize(ITelemetry telemetry)
            {
            }
        }

        private class StubConfigurableWithProperties : ITelemetryModule
        {
            public StubConfigurableWithProperties(TelemetryConfiguration configuration)
            {
                this.Configuration = configuration;
            }

            public int Int32Property { get; set; }

            public string StringProperty { get; set; }

            public TelemetryConfiguration Configuration { get; set; }

            public Action<TelemetryConfiguration> OnInitialize { get; set; }

            public void Initialize(TelemetryConfiguration configuration)
            {
                if (this.OnInitialize != null)
                {
                    this.OnInitialize(configuration);
                }
            }
        }

        public class StubTelemetryProcessor2 : ITelemetryProcessor, ITelemetryModule
        {
            /// <summary>
            /// Made public for testing if the chain of processors is correctly created.
            /// </summary>
            public ITelemetryProcessor next;

            public bool Initialized { get; private set; } = false;

            public StubTelemetryProcessor2(ITelemetryProcessor next)
            {
                this.next = next;
            }

            public void Process(ITelemetry telemetry) {  }

            public void Initialize(TelemetryConfiguration config)
            {
                this.Initialized = true;
            }
        }

        private class StubTelemetryChannel2 : ITelemetryChannel, ITelemetryModule
        {
            public bool? DeveloperMode { get; set; }

            public bool Initialized { get; private set; } = false;

            public string EndpointAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public void Dispose() { }

            public void Flush() { }

            public void Initialize(TelemetryConfiguration configuration)
            {
                this.Initialized = true;
            }

            public void Send(ITelemetry item) { }
        }
    }
}

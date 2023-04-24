// <copyright file="ILoggerIntegrationTests.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.ApplicationInsights;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests the <see cref="ILogger"/> integration for Application Insights.
    /// </summary>
    [TestClass]
    public class ILoggerIntegrationTests
    {
        /// <summary>
        /// Ensures that <see cref="ApplicationInsightsLogger"/> populates params for structured logging into custom properties <see cref="ILogger"/>.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerPopulateStructureLoggingParamsIntoCustomProperties()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            // Scopes are enabled.
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.IncludeScopes = true);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();
            testLogger.LogInformation("Testing structured with {CustomerName} {Age}", "TestCustomerName", 20);

            Assert.AreEqual("Testing structured with TestCustomerName 20", (itemsReceived[0] as TraceTelemetry).Message);
            var customProperties = (itemsReceived[0] as TraceTelemetry).Properties;
            Assert.IsTrue(customProperties["CustomerName"].Equals("TestCustomerName"));
            Assert.IsTrue(customProperties["Age"].Equals("20"));
        }

        /// <summary>
        /// Ensures that <see cref="ApplicationInsightsLogger"/> populates params for structured logging into custom properties <see cref="ILogger"/>.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerPopulateStructureLoggingParamsIntoCustomPropertiesWhenScopeDisabled()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            // Disable scope
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.IncludeScopes = false);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();
            testLogger.LogInformation("Testing structured with {CustomerName} {Age}", "TestCustomerName", 20);

            Assert.AreEqual("Testing structured with TestCustomerName 20", (itemsReceived[0] as TraceTelemetry).Message);
            var customProperties = (itemsReceived[0] as TraceTelemetry).Properties;
            Assert.IsTrue(customProperties["CustomerName"].Equals("TestCustomerName"));
            Assert.IsTrue(customProperties["Age"].Equals("20"));
            Assert.IsTrue(customProperties.ContainsKey("OriginalFormat"));
            Assert.IsFalse(customProperties.ContainsKey("{OriginalFormat}"));
        }

        /// <summary>
        /// Ensures that <see cref="ApplicationInsightsLogger"/> is invoked when user logs using <see cref="ILogger"/>.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerIsInvokedWhenUsingILogger()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration((telemetryItem, telemetryProcessor) =>
            {
                itemsReceived.Add(telemetryItem);
            });

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();

            testLogger.LogInformation("Testing");
            testLogger.LogError(new Exception("ExceptionMessage"), "LoggerMessage");
            testLogger.LogInformation(new EventId(100, "TestEvent"), "TestingEvent");
            testLogger.LogCritical("Critical");
            testLogger.LogTrace("Trace");
            testLogger.LogWarning("Warning");
            testLogger.LogDebug("Debug");

            Assert.AreEqual(7, itemsReceived.Count);

            Assert.IsTrue((itemsReceived[2] as ISupportProperties).Properties.ContainsKey("EventId"));
            Assert.IsTrue((itemsReceived[2] as ISupportProperties).Properties.ContainsKey("EventName"));
            Assert.AreEqual("100", (itemsReceived[2] as ISupportProperties).Properties["EventId"]);
            Assert.AreEqual("TestEvent", (itemsReceived[2] as ISupportProperties).Properties["EventName"]);

            Assert.AreEqual("Microsoft.ApplicationInsights.ILoggerIntegrationTests", (itemsReceived[2] as ISupportProperties).Properties["CategoryName"]);
            Assert.AreEqual("Microsoft.ApplicationInsights.ILoggerIntegrationTests", (itemsReceived[0] as ISupportProperties).Properties["CategoryName"]);

            Assert.AreEqual(SeverityLevel.Information, (itemsReceived[0] as TraceTelemetry).SeverityLevel);
            Assert.AreEqual(SeverityLevel.Error, (itemsReceived[1] as ExceptionTelemetry).SeverityLevel);
            Assert.AreEqual(SeverityLevel.Information, (itemsReceived[2] as TraceTelemetry).SeverityLevel);
            Assert.AreEqual(SeverityLevel.Critical, (itemsReceived[3] as TraceTelemetry).SeverityLevel);
            Assert.AreEqual(SeverityLevel.Verbose, (itemsReceived[4] as TraceTelemetry).SeverityLevel);
            Assert.AreEqual(SeverityLevel.Warning, (itemsReceived[5] as TraceTelemetry).SeverityLevel);
            Assert.AreEqual(SeverityLevel.Verbose, (itemsReceived[6] as TraceTelemetry).SeverityLevel);

            Assert.AreEqual("Testing", (itemsReceived[0] as TraceTelemetry).Message);
            Assert.AreEqual("ExceptionMessage", (itemsReceived[1] as ExceptionTelemetry).Message);
            Assert.AreEqual("LoggerMessage", (itemsReceived[1] as ExceptionTelemetry).Properties["FormattedMessage"]);
            Assert.AreEqual("TestingEvent", (itemsReceived[2] as TraceTelemetry).Message);
            Assert.AreEqual("Critical", (itemsReceived[3] as TraceTelemetry).Message);
            Assert.AreEqual("Trace", (itemsReceived[4] as TraceTelemetry).Message);
            Assert.AreEqual("Warning", (itemsReceived[5] as TraceTelemetry).Message);
            Assert.AreEqual("Debug", (itemsReceived[6] as TraceTelemetry).Message);
        }

        /// <summary>
        /// Ensures that the <see cref="ApplicationInsightsLoggerOptions.TrackExceptionsAsExceptionTelemetry"/> switch is honored
        /// and exceptions are logged as trace messages when value is true.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerLogsExceptionAsExceptionWhenSwitchIsTrue()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            // Case where Exceptions are logged as Exception Telemetry.
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.TrackExceptionsAsExceptionTelemetry = true);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();

            testLogger.LogInformation("Testing");
            testLogger.LogError(new Exception("ExceptionMessage"), "LoggerMessage");

            Assert.IsInstanceOfType(itemsReceived[0], typeof(TraceTelemetry));
            Assert.IsInstanceOfType(itemsReceived[1], typeof(ExceptionTelemetry));

            Assert.AreEqual("Testing", (itemsReceived[0] as TraceTelemetry).Message);
            Assert.AreEqual("ExceptionMessage", (itemsReceived[1] as ExceptionTelemetry).Message);
            Assert.AreEqual("LoggerMessage", (itemsReceived[1] as ExceptionTelemetry).Properties["FormattedMessage"]);
        }

        /// <summary>
        /// Ensures that the <see cref="ApplicationInsightsLoggerOptions.TrackExceptionsAsExceptionTelemetry"/> switch is honored
        /// and exceptions are logged as trace messages when value is false.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerLogsExceptionAsTraceWhenSwitchIsFalse()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.TrackExceptionsAsExceptionTelemetry = false);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();

            testLogger.LogInformation("Testing");

            Exception trackingException = null;
            try
            {
                ThrowException();
            }
            catch (Exception ex)
            {
                trackingException = ex;
                testLogger.LogError(ex, "LoggerMessage");
            }

            Assert.IsInstanceOfType(itemsReceived[0], typeof(TraceTelemetry));
            Assert.IsInstanceOfType(itemsReceived[1], typeof(TraceTelemetry));

            Assert.AreEqual(SeverityLevel.Information, (itemsReceived[0] as TraceTelemetry).SeverityLevel);
            Assert.AreEqual("Testing", (itemsReceived[0] as TraceTelemetry).Message);

            Assert.AreEqual(SeverityLevel.Error, (itemsReceived[1] as TraceTelemetry).SeverityLevel);
            Assert.AreEqual("LoggerMessage", (itemsReceived[1] as TraceTelemetry).Message);

            Assert.IsTrue((itemsReceived[1] as TraceTelemetry).Properties["ExceptionMessage"].Contains("StackTraceEnabled"));

            Assert.IsTrue((itemsReceived[1] as TraceTelemetry).Properties.ContainsKey("ExceptionStackTrace"));

            Assert.AreEqual(
                trackingException.ToInvariantString(),
                (itemsReceived[1] as TraceTelemetry).Properties["ExceptionStackTrace"]);

            void ThrowException()
            {
                throw new Exception("StackTraceEnabled");
            }
        }

        /// <summary>
        /// Ensures that the <see cref="ApplicationInsightsLoggerOptions.IncludeScopes"/> switch is honored and scopes are added
        /// when switch is true.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerAddsScopeWhenSwitchIsTrue()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            // Case where Scope is included.
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.IncludeScopes = true);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();

            using (testLogger.BeginScope("TestScope"))
            {
                using (testLogger.BeginScope<IReadOnlyCollection<KeyValuePair<string, object>>>(new Dictionary<string, object> { { "Key", "Value" } }))
                {
                    using (testLogger.BeginScope("TestScope {Key1} and {Key2}", "Value1", "Value2"))
                    {
                        testLogger.LogInformation("Testing");
                        testLogger.LogError(new Exception("ExceptionMessage"), "LoggerMessage");
                    }
                }
            }

            Assert.AreEqual(" => TestScope", (itemsReceived[0] as ISupportProperties).Properties["Scope"]);
            Assert.AreEqual("Value", (itemsReceived[0] as ISupportProperties).Properties["Key"]);
            Assert.AreEqual("Value1", (itemsReceived[0] as ISupportProperties).Properties["Key1"]);
            Assert.AreEqual("Value2", (itemsReceived[0] as ISupportProperties).Properties["Key2"]);
            Assert.IsTrue((itemsReceived[0] as ISupportProperties).Properties.ContainsKey("OriginalFormat"));
            Assert.IsFalse((itemsReceived[0] as ISupportProperties).Properties.ContainsKey("{OriginalFormat}"));

            Assert.AreEqual(" => TestScope", (itemsReceived[1] as ISupportProperties).Properties["Scope"]);
            Assert.AreEqual("Value", (itemsReceived[1] as ISupportProperties).Properties["Key"]);
            Assert.AreEqual("Value1", (itemsReceived[1] as ISupportProperties).Properties["Key1"]);
            Assert.AreEqual("Value2", (itemsReceived[1] as ISupportProperties).Properties["Key2"]);
            Assert.IsTrue((itemsReceived[1] as ISupportProperties).Properties.ContainsKey("OriginalFormat"));
            Assert.IsFalse((itemsReceived[1] as ISupportProperties).Properties.ContainsKey("{OriginalFormat}"));

            Assert.AreEqual("Testing", (itemsReceived[0] as TraceTelemetry).Message);
            Assert.AreEqual("ExceptionMessage", (itemsReceived[1] as ExceptionTelemetry).Message);
        }

        /// <summary>
        /// Ensures that the <see cref="ApplicationInsightsLoggerOptions.IncludeScopes"/> switch is honored and scopes are excluded
        /// when switch is false.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerDoesNotAddScopeWhenSwitchIsFalse()
        {
            List<ITelemetry> itemsReceived = new List<ITelemetry>();

            // Case where Scope is NOT Included
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => itemsReceived.Add(telemetryItem),
                configureTelemetryConfiguration: null,
                configureApplicationInsightsOptions: (appInsightsLoggerOptions) => appInsightsLoggerOptions.IncludeScopes = false);

            ILogger<ILoggerIntegrationTests> testLogger = serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();

            using (testLogger.BeginScope("TestScope"))
            {
                using (testLogger.BeginScope<IReadOnlyCollection<KeyValuePair<string, object>>>(new Dictionary<string, object> { { "Key", "Value" } }))
                {
                    testLogger.LogInformation("Testing");
                    testLogger.LogError(new Exception("ExceptionMessage"), "LoggerMessage");
                }
            }

            Assert.IsFalse((itemsReceived[0] as ISupportProperties).Properties.ContainsKey("Scope"));
            Assert.IsFalse((itemsReceived[0] as ISupportProperties).Properties.ContainsKey("Key"));

            Assert.IsFalse((itemsReceived[1] as ISupportProperties).Properties.ContainsKey("Scope"));
            Assert.IsFalse((itemsReceived[1] as ISupportProperties).Properties.ContainsKey("Key"));

            Assert.AreEqual("Testing", (itemsReceived[0] as TraceTelemetry).Message);
            Assert.AreEqual("ExceptionMessage", (itemsReceived[1] as ExceptionTelemetry).Message);
        }

        /// <summary>
        /// Test to ensure Instrumentation key is set correctly.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerInstrumentationKeyIsSetCorrectly()
        {
            // Create DI container.
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddApplicationInsights("TestAIKey");
            });

            TelemetryConfiguration telemetryConfiguration = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<TelemetryConfiguration>>().Value;

            Assert.AreEqual("TestAIKey", telemetryConfiguration.InstrumentationKey);
        }

        /// <summary>
        /// Test to ensure TelemetryConfiguration is set correctly.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void ApplicationInsightsLoggerTelemetryConfigurationIsSetCorrectly()
        {
            // Create DI container.
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddApplicationInsights(
                    telemetryConfiguration => { telemetryConfiguration.ConnectionString = "InstrumentationKey=TestAIKey"; },
                    applicationInsightsLoggerOptions => { });
            });

            TelemetryConfiguration actualTelemetryConfiguration = services
                .BuildServiceProvider()
                .GetRequiredService<IOptions<TelemetryConfiguration>>().Value;

            Assert.AreEqual("TestAIKey", actualTelemetryConfiguration.InstrumentationKey);
        }

        /// <summary>
        /// Ensures that the default <see cref="ApplicationInsightsLoggerOptions"/> are as expected.
        /// </summary>
        [TestMethod]
        [TestCategory("ILogger")]
        public void DefaultLoggerOptionsAreCorrectlyRegistered()
        {
            IServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                (telemetryItem, telemetryProcessor) => { });

            IOptions<ApplicationInsightsLoggerOptions> registeredOptions =
                serviceProvider.GetRequiredService<IOptions<ApplicationInsightsLoggerOptions>>();

            Assert.IsTrue(registeredOptions.Value.TrackExceptionsAsExceptionTelemetry);
            Assert.IsTrue(registeredOptions.Value.IncludeScopes);
        }

        [TestMethod]
        [TestCategory("ILogger")]
        public void TelemetryChannelIsFlushedWhenServiceProviderIsDisposed()
        {
            TestTelemetryChannel testTelemetryChannel = new TestTelemetryChannel();

            using (ServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                delegate { },
                telemetryConfiguration => telemetryConfiguration.TelemetryChannel = testTelemetryChannel))
            {
                serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();
            }

            Assert.AreEqual(1, testTelemetryChannel.FlushCount);
        }

        [TestMethod]
        [TestCategory("ILogger")]
        public void TelemetryChannelIsNotFlushedWhenFlushOnDisposeIsFalse()
        {
            TestTelemetryChannel testTelemetryChannel = new TestTelemetryChannel();

            using (ServiceProvider serviceProvider = ILoggerIntegrationTests.SetupApplicationInsightsLoggerIntegration(
                delegate { },
                telemetryConfiguration => telemetryConfiguration.TelemetryChannel = testTelemetryChannel,
                applicationInsightsOptions => applicationInsightsOptions.FlushOnDispose = false))
            {
                serviceProvider.GetRequiredService<ILogger<ILoggerIntegrationTests>>();
            }

            Assert.AreEqual(0, testTelemetryChannel.FlushCount);
        }

        /// <summary>
        /// Sets up the Application insights logger.
        /// </summary>
        /// <param name="telemetryActionCallback">Callback to execute when telemetry items are emitted.</param>
        /// <param name="configureTelemetryConfiguration">Action to configure telemetry configuration.</param>
        /// <param name="configureApplicationInsightsOptions">Action to configure logger options.</param>
        /// <param name="configureServices">Action to add, configure services to DI container.</param>
        /// <returns>Built DI container.</returns>
        private static ServiceProvider SetupApplicationInsightsLoggerIntegration(
            Action<ITelemetry, ITelemetryProcessor> telemetryActionCallback,
            Action<TelemetryConfiguration> configureTelemetryConfiguration = null,
            Action<ApplicationInsightsLoggerOptions> configureApplicationInsightsOptions = null,
            Func<IServiceCollection, IServiceCollection> configureServices = null)
        {
            // Create DI container.
            IServiceCollection services = new ServiceCollection();

            // Configure the Telemetry configuration to be used to send data to AI.
            services.Configure<TelemetryConfiguration>(telemetryConfiguration =>
            {
                telemetryConfiguration.TelemetryProcessorChainBuilder.Use((existingProcessor) =>
                {
                    return new TestTelemetryProcessor(existingProcessor, telemetryActionCallback);
                }).Build();
            });

            if (configureTelemetryConfiguration != null)
            {
                services.Configure<TelemetryConfiguration>(configureTelemetryConfiguration);
            }

            services.AddLogging(loggingBuilder =>
            {
                if (configureApplicationInsightsOptions != null)
                {
                    loggingBuilder.AddApplicationInsights(configureApplicationInsightsOptions);
                }
                else
                {
                    loggingBuilder.AddApplicationInsights();
                }

                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            });

            if (configureServices != null)
            {
                services = configureServices.Invoke(services);
            }

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}

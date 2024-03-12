// -----------------------------------------------------------------------
// <copyright file="ApplicationInsightsAppenderTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. 
// All rights reserved.  2013
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.Log4NetAppender.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using log4net;
    using log4net.Config;
    using log4net.Util;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.CommonTestShared;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Log4NetAppender;
    using Microsoft.ApplicationInsights.Tracing.Tests;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static System.Globalization.CultureInfo;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        category: "Microsoft.Design", checkId: "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Disposing the object in the Test Cleanup")]
    [TestClass]
    public class ApplicationInsightsAppenderTests
    {
        private static readonly Assembly callingAssembly = typeof(ApplicationInsightsAppenderTests).GetTypeInfo().Assembly;
        private AdapterHelper adapterHelper;
        private AppendableLogger appendableLogger;

        [TestInitialize]
        public void Initialize()
        {
            this.adapterHelper = new AdapterHelper();
            this.appendableLogger = new AppendableLogger();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.adapterHelper.Dispose();
            this.appendableLogger.Dispose();
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.Dispose();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void InitializeAppenderNotFailingWhenInstrumentionKeyIsEmptyString()
        {
            this.VerifyInitializationSuccess(
                () => ApplicationInsightsAppenderTests.InitializeLog4NetAIAdapter(@"<InstrumentationKey value="""" />"),
                string.Empty);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void InitializeAppenderDoesNotThrowLogExceptionWhenInstrumentationKeyIsCorrect()
        {
            string instrumentationKey = Guid.NewGuid().ToString();
            this.VerifyInitializationSuccess(
                    () => ApplicationInsightsAppenderTests.InitializeLog4NetAIAdapter(string.Format(InvariantCulture, @"<InstrumentationKey value=""{0}"" />", instrumentationKey)),
                    instrumentationKey);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void SdkVersionIsCorrect()
        {
            this.appendableLogger.Logger.Debug("Trace Debug");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];
            Assert.AreNotEqual(default(DateTimeOffset), telemetry.Context);

            string expectedVersion = SdkVersionHelper.GetExpectedSdkVersion(prefix: "log4net:", loggerType: typeof(ApplicationInsightsAppender));
            Assert.AreEqual(expectedVersion, telemetry.Context.GetInternalContext().SdkVersion);
        }
        
        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void ValidateLoggingIsWorking()
        {
            string instrumentationKey = "93d9c2b7-e633-4571-8520-d391511a1df5";
            ApplicationInsightsAppenderTests.InitializeLog4NetAIAdapter(string.Format(InvariantCulture, @"<InstrumentationKey value=""{0}"" />", instrumentationKey));

            // Set up error handler to intercept exception
            ApplicationInsightsAppender aiAppender = (ApplicationInsightsAppender)log4net.LogManager.GetRepository(callingAssembly).GetAppenders()[0];
            log4net.Util.OnlyOnceErrorHandler errorHandler = new log4net.Util.OnlyOnceErrorHandler();
            aiAppender.ErrorHandler = errorHandler;
            
            // Log something
            ILog logger = log4net.LogManager.GetLogger(callingAssembly, "TestAIAppender");
            for (int i = 0; i < 1500; i++)
            {
                logger.Debug("Trace Debug" + i + DateTime.Now);
            }
            
            Assert.AreEqual(instrumentationKey, aiAppender.TelemetryClient.Context.InstrumentationKey);
            Assert.IsNull(errorHandler.Exception);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void TracesAreEnqueuedInChannel()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.InstrumentationKey = this.adapterHelper.InstrumentationKey;
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

            ApplicationInsightsAppenderTests.InitializeLog4NetAIAdapter(string.Empty);
            this.SendMessagesToMockChannel(this.adapterHelper.InstrumentationKey);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void InstrumentationKeyCanBeOverridden()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

            // Set instrumentation key in Log4Net environment
            string instrumentationKey = "93d9c2b7-e633-4571-8520-d391511a1df5";
            ApplicationInsightsAppenderTests.InitializeLog4NetAIAdapter(string.Format(InvariantCulture, @"<InstrumentationKey value=""{0}"" />", instrumentationKey));
            Assert.AreNotEqual(instrumentationKey, this.adapterHelper.InstrumentationKey, "This test will not verify anything if the same instrumentation key is used by both");
            this.SendMessagesToMockChannel(instrumentationKey);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetAddsCustomPropertiesToTraceTelemetry()
        {
            ApplicationInsightsAppenderTests.InitializeLog4NetAIAdapter(string.Empty);
            this.VerifyPropertiesInTelemetry();
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetSetsTimespamp()
        {
            this.appendableLogger.Logger.Debug("Trace Debug");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];
            Assert.AreNotEqual(default(DateTimeOffset), telemetry.Timestamp);
        }

        [TestMethod]
        [Ignore]
        [TestCategory("Log4NetAppender")]
        public void TelemetryIsAcceptedByValidateEndpoint()
        {
            this.appendableLogger.Logger.Debug("Trace Debug");

            ITelemetry telemetry = this.appendableLogger.SentItems.First();

            Assert.IsNull(TelemetrySender.ValidateEndpointSend(telemetry));
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetDoesNotSetAuthenticatedUser()
        {
            this.appendableLogger.Logger.Debug("Trace Debug");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];
            Assert.IsNull(telemetry.Context.User.AuthenticatedUserId);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetAddsCustomProperties()
        {
            GlobalContext.Properties["CustomProperty1"] = "Value1";

            this.appendableLogger.Logger.Debug("Trace Debug");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];
            Assert.AreEqual("Value1", telemetry.Properties["CustomProperty1"]);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetDoesNotAddPropertiesPropertiesWithPrefix()
        {
            this.appendableLogger.Logger.Debug("Trace Debug");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];

            foreach (var key in telemetry.Properties.Keys)
            {
                Assert.IsFalse(key.StartsWith("log4net", StringComparison.OrdinalIgnoreCase));
            }
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetSetsVerboseSeverityLevelToTraceTelemetry()
        {
            this.appendableLogger.Logger.Debug("Trace Debug");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];
            Assert.AreEqual(SeverityLevel.Verbose, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetSetsInformationSeverityLevelToTraceTelemetry()
        {
            this.appendableLogger.Logger.Info("Trace Debug");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];
            Assert.AreEqual(SeverityLevel.Information, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetSetsWarningSeverityLevelToTraceTelemetry()
        {
            this.appendableLogger.Logger.Warn("Trace");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];
            Assert.AreEqual(SeverityLevel.Warning, telemetry.SeverityLevel);
       }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetSetsErrorSeverityLevelToTraceTelemetry()
        {
            this.appendableLogger.Logger.Error("Trace");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];
            Assert.AreEqual(SeverityLevel.Error, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetSetsFatalSeverityLevelToTraceTelemetry()
        {
            this.appendableLogger.Logger.Fatal("Trace");

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (TraceTelemetry)sentItems[0];
            Assert.AreEqual(SeverityLevel.Critical, telemetry.SeverityLevel);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void Log4NetSendsExceptionToExceptionTelemetry()
        {
            Exception expectedException;
            ILog logger = this.appendableLogger.Logger;

            try
            {
                throw new Exception("Test logging exception");
            }
            catch (Exception exception)
            {
                expectedException = exception;
                logger.Error("testing exception scenario", exception);
            }

            var sentItems = this.appendableLogger.SentItems;
            Assert.AreEqual(1, sentItems.Length);

            var telemetry = (ExceptionTelemetry)sentItems[0];
            Assert.AreEqual(SeverityLevel.Error, telemetry.SeverityLevel);
            Assert.AreEqual(expectedException.Message, telemetry.Exception.Message);
        }

        [TestMethod]
        [TestCategory("Log4NetAppender")]
        public void CustomMessageIsAddedToExceptionTelemetryCustomProperties()
        {
            ILog logger = this.appendableLogger.Logger;

            try
            {
                throw new Exception("Test logging exception");
            }
            catch (Exception exception)
            {
                logger.Error("custom message", exception);
            }

            ExceptionTelemetry telemetry = (ExceptionTelemetry)this.appendableLogger.SentItems.First();
            Assert.AreEqual("System.Exception: Test logging exception", telemetry.Message);
            Assert.IsTrue(telemetry.Properties["Message"].StartsWith("custom message", StringComparison.Ordinal));
        }

        internal static void InitializeLog4NetAIAdapter(string adapterComponentIdSnippet)
        {
            string xmlRawText =
@"<configuration>" +
@"    <configSections>" +
@"       <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net""/>" +
@"    </configSections>" +
@"    <log4net>" +
@"       <appender name=""TestAIAppender"" type=""Microsoft.ApplicationInsights.Log4NetAppender.ApplicationInsightsAppender, Microsoft.ApplicationInsights.Log4NetAppender"">" +
adapterComponentIdSnippet +
@"           <layout type=""log4net.Layout.PatternLayout"">" +
@"               <conversionPattern value=""%message%newline""/>" +
@"           </layout>" +
@"        </appender>" +
@"        <root>" +
@"           <appender-ref ref=""TestAIAppender""/>" +
@"        </root>" +
@"    </log4net>" +
@"</configuration>";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlRawText)))
            {
                XmlConfigurator.Configure(log4net.LogManager.GetRepository(callingAssembly), stream);
            }
        }

        private void VerifyInitializationSuccess(Action testAction, string expectedInstrumentationKey)
        {
            this.VerifyInitializationError(testAction, 0, null);
            
            ApplicationInsightsAppender aiAppender = (ApplicationInsightsAppender)log4net.LogManager.GetRepository(callingAssembly).GetAppenders()[0];
            Assert.AreEqual(expectedInstrumentationKey, aiAppender.InstrumentationKey);
        }

        private void VerifyInitializationError(Action testAction, int logCount, Type exceptionType)
        {
            List<LogLog> logs = new List<LogLog>();
            Action<object, LogReceivedEventArgs> recordLogs = (s, e) => logs.Add(e.LogLog);
            LogLog.LogReceived += new LogReceivedEventHandler(recordLogs);
            try
            {
                // execute
                testAction();

                // verification
                foreach (var log in logs)
                {
                    Console.WriteLine(log);
                }

                Assert.AreEqual(logCount, logs.Count);
                if (exceptionType != null)
                {
                    var exceptions = logs.Select(log => log.Exception).SelectMany(this.GetAllInnerExceptions).ToList();
                    Assert.IsTrue(exceptions.Any(ex => ex.GetType() == exceptionType), "No exceptions of type {0} found.", exceptionType);
                }
            }
            finally
            {
                LogLog.LogReceived -= new LogReceivedEventHandler(recordLogs);
            }
        }

        private IEnumerable<Exception> GetAllInnerExceptions(Exception ex)
        {
            if (ex == null)
            {
                return Enumerable.Empty<Exception>();
            }
            
            if (ex.InnerException == null)
            {
                return new Exception[] { ex };
            }

            List<Exception> exceptions = new List<Exception>();
            exceptions.Add(ex);
            exceptions.AddRange(this.GetAllInnerExceptions(ex.InnerException));
            return exceptions;
        }

        private void SendMessagesToMockChannel(string instrumentationKey)
        {
            // Set up error handler to intercept exception
            ApplicationInsightsAppender aiAppender = (ApplicationInsightsAppender)log4net.LogManager.GetRepository(callingAssembly).GetAppenders()[0];
            log4net.Util.OnlyOnceErrorHandler errorHandler = new log4net.Util.OnlyOnceErrorHandler();
            aiAppender.ErrorHandler = errorHandler;

            // Log something
            ILog logger = log4net.LogManager.GetLogger(callingAssembly, "TestAIAppender");
            logger.Debug("Trace Debug");
            logger.Error("Trace Error");
            logger.Fatal("Trace Fatal");
            logger.Info("Trace Info");
            logger.Warn("Trace Warn");

            AdapterHelper.ValidateChannel(this.adapterHelper, instrumentationKey, 5);
        }

        private void VerifyPropertiesInTelemetry()
        {
            // Mock channel to validate that our appender is trying to send logs
#pragma warning disable CS0618 // Type or member is obsolete
            TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

            // Set up error handler to intercept exception
            ApplicationInsightsAppender aiAppender = (ApplicationInsightsAppender)LogManager.GetRepository(callingAssembly).GetAppenders()[0];
            OnlyOnceErrorHandler errorHandler = new OnlyOnceErrorHandler();
            aiAppender.ErrorHandler = errorHandler;            

            // Log something
            ILog logger = LogManager.GetLogger(callingAssembly, "TestAIAppender");
            logger.Debug("Trace Debug");

            ITelemetry[] sentItems = this.adapterHelper.Channel.SentItems;
            Assert.IsTrue(sentItems.Count() == 1);
            var telemetry = (TraceTelemetry)sentItems.First();
            IDictionary<string, string> properties = telemetry.Properties;
            Assert.IsTrue(properties.Any());

            string value;
            
            properties.TryGetValue("LoggerName", out value);
            Assert.AreEqual("TestAIAppender", value);

            Assert.IsTrue(properties.ContainsKey("ThreadName"));
            Assert.IsTrue(properties.ContainsKey("ClassName"));
            Assert.IsTrue(properties.ContainsKey("FileName"));
            Assert.IsTrue(properties.ContainsKey("MethodName"));
            Assert.IsTrue(properties.ContainsKey("LineNumber"));
            Assert.IsTrue(properties.ContainsKey("Domain"));
        }

        private class AppendableLogger : IDisposable
        {
            private readonly AdapterHelper adapterHelper;
            
            public AppendableLogger()
            {
                this.adapterHelper = new AdapterHelper();

                // Mock channel to validate that our appender is trying to send logs
#pragma warning disable CS0618 // Type or member is obsolete
                TelemetryConfiguration.Active.TelemetryChannel = this.adapterHelper.Channel;
#pragma warning restore CS0618 // Type or member is obsolete

                ApplicationInsightsAppenderTests.InitializeLog4NetAIAdapter(string.Empty);

                // Set up error handler to intercept exception
                var aiAppender = (ApplicationInsightsAppender)LogManager.GetRepository(callingAssembly).GetAppenders()[0];
                var errorHandler = new OnlyOnceErrorHandler();
                aiAppender.ErrorHandler = errorHandler;               

                this.Logger = LogManager.GetLogger(callingAssembly, "TestAIAppender");
            }

            public ILog Logger { get; set; }

            public ITelemetry[] SentItems
            {
                get { return this.adapterHelper.Channel.SentItems; }
            }

            public void Dispose()
            {
                this.adapterHelper.Dispose();
            }
        }
    }
}